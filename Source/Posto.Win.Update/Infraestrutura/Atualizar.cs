using log4net;
using Microsoft.Practices.Prism.Commands;
using Posto.Win.Update.DataContext;
using Posto.Win.Update.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace Posto.Win.Update.Infraestrutura
{
    public class Atualizar
    {
        #region Gerenciador de log

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constantes

        private const string PathExe = "public_html/posto_att/PostoSQL_Exe.zip";

        private const string PathSql = "public_html/posto_att/sql/";

        private const string PathLeitor = @"C:\Metodos\uLeitor.exe";

        #endregion

        #region Propriedades

        private AtualizarModel _atualizar;

        private Timer _timerProximaAtualizacao;

        private Ftp _ftp;

        private ConfiguracaoModel _configuracaoModel;

        #endregion

        public Atualizar(AtualizarModel atualizar)
        {
            this._atualizar = atualizar;
            this._ftp = new Ftp();

            this._timerProximaAtualizacao = new Timer();
            this._timerProximaAtualizacao.AutoReset = false;
            this._timerProximaAtualizacao.Elapsed += this.Execute;
        }

        public void Iniciar(ConfiguracaoModel configuracaoModel)
        {
            this._configuracaoModel = configuracaoModel;
            this.SetTime();
        }

        public void Pausar()
        {
            this._timerProximaAtualizacao.Stop();
        }

        public async void Manual(ConfiguracaoModel configuracaoModel, DelegateCommand atualizarAfter)
        {
            this._configuracaoModel = configuracaoModel;
            await this.ExecuteAsync();
            this._timerProximaAtualizacao.Stop();
            atualizarAfter.Execute();
        }

        /// <summary>
        /// Atualiza viewmodel com as informações do banco de dados
        /// </summary>
        public void AtualizaViewModel()
        {
            var context = new PostoContext(this._configuracaoModel);
            var query = context.Query("SELECT *, (SELECT oft000.parversao FROM oft000 LIMIT 1) AS versao, NOW() AS dataAtual FROM atualiz");

            using (var reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    this._atualizar.Dia         = reader.GetInt16(reader.GetOrdinal("atuDiaSema"));
                    this._atualizar.Hora        = Convert.ToInt16(reader.GetString(reader.GetOrdinal("atuHoraMin")).Split(':')[0]);
                    this._atualizar.Minuto      = Convert.ToInt16(reader.GetString(reader.GetOrdinal("atuHoraMin")).Split(':')[1]);
                    this._atualizar.UltimaData  = reader.GetDateTime(reader.GetOrdinal("atuDatAtua"));
                    this._atualizar.Versao      = reader.GetInt32(reader.GetOrdinal("versao"));
                    this._atualizar.DataAtual   = reader.GetDateTime(reader.GetOrdinal("dataAtual"));
                }
            }
            context.Close();
        }

        /// <summary>
        /// Calcula o tempo para a proxima atualização
        /// </summary>
        private void SetTime()
        {
            this.AtualizaViewModel();

            var milisegundos = (this._atualizar.GetDataProximaAtualizacao.Value - this._atualizar.DataAtual).TotalMilliseconds;

            this._timerProximaAtualizacao.Interval = milisegundos;
            this._timerProximaAtualizacao.Start();
        }
        
        private void Execute(object sender, ElapsedEventArgs e)
        {
            this.ExecuteAsync();
        }

        private async Task ExecuteAsync()
        {
            await Task.Run(() =>
            {
                // INDICA AO GENEXUS MANUTENÇÃO ATIVADA E DESCONECTA TODOS OS USUÁRIOS DO BANCO.
                this.ManutencaoAtiva();

                var context  = new PostoContext(this._configuracaoModel);
                
                try
                {
                    context.BeginTransaction();

                    this._atualizar.MensagemStatus = "Iniciando atualização...";

                    Process[] processes = Process.GetProcessesByName("uLeitor");
                    foreach (Process process in processes)
                    {
                        process.Kill();
                    }

                    this._timerProximaAtualizacao.Stop();
                    this.AtualizaViewModel();

                    var ultimaVersao = this.AtualizarSql(context);

                    this.AtualizarExe();
                    this.AtualizarBanco(context, ultimaVersao);
                    
                    // SALVA
                    context.Commit();

                    this._atualizar.MensagemStatus = "Atualizado com sucesso.";

                }
                catch (Exception e)
                {
                    context.RollBack();
                    log.Error(e.Message);

                    this._atualizar.MensagemStatus = "Problemas ao atualizar.";
                }
                finally 
                {
                    context.Close();

                    this._timerProximaAtualizacao.Stop();
                    this.SetTime();
                    this.ManutencaoDesativa(); // INDICA AO GENEXUS MANUTENÇÃO DESATIVADA

                    //INICIA O PROCESSO DO LEITOR DE BOMBAS
                    if (File.Exists(PathLeitor)) 
                    {
                        Process.Start(PathLeitor);
                    }
                }
            });
            
        }

        /// <summary>
        /// Atualiza arquivos
        /// </summary>
        private void AtualizarExe()
        {
            this._atualizar.MensagemStatus = "Atualizando programas...";

            var arquivos = this._ftp.Download(PathExe);

            using (MemoryStream mem = new MemoryStream(arquivos))
            using (ZipArchive zipStream = new ZipArchive(mem))
            {
                foreach (ZipArchiveEntry file in zipStream.Entries)
                {
                    string completeFileName = Path.Combine(this._configuracaoModel.LocalDiretorio, file.FullName);
                    if (file.Name == "")
                    {// Assuming Empty for Directory
                        Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                        continue;
                    }
                    file.ExtractToFile(completeFileName, true);
                }
            }
        }

        /// <summary>
        /// Atualiza SQLs e retorna a ultima versão
        /// </summary>
        private int? AtualizarSql(PostoContext context) 
        {
            var retornoSql = this._ftp.GetFileList(PathSql);
            var listaSql = retornoSql.Where(row => row.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
                                        .Select(row => new
                                        {
                                            Versao = Convert.ToInt32(Regex.Replace(row, "[^0-9]+", "")),
                                            Arquivo = row,
                                        })
                                        .Where(row => row.Versao > this._atualizar.Versao)
                                        .OrderBy(row => row.Versao)
                                        .ToList();

            listaSql.ForEach(row =>
            {
                this._atualizar.MensagemStatus = "Atualizando Rmenu " + row.Versao.ToString() + "...";

                var sql = Encoding.ASCII.GetString(this._ftp.Download(PathSql + row.Arquivo));
                context.Query(sql).ExecuteNonQuery();
            });

            var ultimaVersao = listaSql.OrderByDescending(x => x.Versao)
                                       .Select(x => x.Versao)
                                       .FirstOrDefault();

            return ultimaVersao > 0 ? ultimaVersao : this._atualizar.Versao;
        }
        
        /// <summary>
        /// Atualiza informações do banco
        /// </summary>
        private void AtualizarBanco(PostoContext context, int? versao)
        {
            this._atualizar.MensagemStatus = "Atualizando versão...";

            if (versao != null)
            {
                context.Query("UPDATE oft000 SET parversao = " + versao.ToString()).ExecuteNonQuery();
            }

            context.Query("UPDATE atualiz SET atuDatAtua = '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "'").ExecuteNonQuery();
        }

        private void ManutencaoAtiva() 
        {
            this._atualizar.MensagemStatus = "Atualizando dados...";

            var context = new PostoContext(this._configuracaoModel);
            context.Query("UPDATE atualiz SET atumanuten = 'S'").ExecuteNonQuery();
            context.Query("SELECT pg_terminate_backend(PID) FROM pg_stat_activity WHERE PID <> pg_backend_pid()").ExecuteNonQuery();
            context.Close();
            
        }

        private void ManutencaoDesativa()
        {
            var context = new PostoContext(this._configuracaoModel);
            context.Query("UPDATE atualiz SET atumanuten = 'N'").ExecuteNonQuery();
            context.Close();
        }
    }
}