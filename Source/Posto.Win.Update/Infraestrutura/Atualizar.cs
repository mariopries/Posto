using log4net;
using Microsoft.Practices.Prism.Commands;
using Posto.Win.Update.DataContext;
using Posto.Win.Update.Model;
using Posto.Win.Update.ViewModel;
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
using System.Windows.Forms;

namespace Posto.Win.Update.Infraestrutura
{
    public class Atualizar
    {

        public class Atualizacao
        {
            public int Versao;
            public string Arquivo;
        }

        #region Gerenciador de log

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constantes

        private const string PathExe = "public_html/posto_att/Update.zip";

        private const string PathSql = "public_html/posto_att/rmenu/";

        private const string PathAtualizador = @"C:\Metodos\App\Posto.exe";

        private const string PathLeitor = @"C:\Metodos\uLeitor.exe";

        private const string PathPostoWeb = @"C:\Metodos\uPostoWe.exe";

        #endregion

        #region Propriedades

        private AtualizarModel _atualizar;

        private System.Timers.Timer _timerProximaAtualizacao;

        private MainWindowViewModel _mainwindowviewmodel;

        private Ftp _ftp;

        private ConfiguracaoModel _configuracaoModel;

        private string[] RetornoFtp;

        private List<Atualizacao> ListaSql;

        #endregion

        #region Variaveis

        private string Rmenu;
        private int PrimeiraVersao;
        private int UltimaVersao;

        #endregion

        /// <summary>
        /// Inicia o processo de atualização
        /// </summary>
        public Atualizar(AtualizarModel atualizar, MainWindowViewModel mainwindowviewmodel)
        {
            _atualizar = atualizar;
            _mainwindowviewmodel = mainwindowviewmodel;
            _ftp = new Ftp();

            _timerProximaAtualizacao = new System.Timers.Timer();
            _timerProximaAtualizacao.AutoReset = false;
            _timerProximaAtualizacao.Elapsed += Execute;
        }

        public void Iniciar(ConfiguracaoModel configuracaoModel)
        {
            _configuracaoModel = configuracaoModel;
            SetTime();
        }

        public void Pausar()
        {
            _timerProximaAtualizacao.Stop();
        }

        /// <summary>
        /// Inicia o processo de atualização manualmente
        /// </summary>
        public async void Manual(ConfiguracaoModel configuracaoModel, DelegateCommand atualizarAfter)
        {
            AtualizaViewModel();
            BuscaVersoes();
            if (_atualizar.Versao < UltimaVersao)
            {
                _configuracaoModel = configuracaoModel;
                await ExecuteAsync();
                _timerProximaAtualizacao.Stop();
            }
            else
            {
                SetaBarraStatus(System.Windows.Visibility.Hidden, 19);

                _atualizar.MensagemStatus = "Sistema já atualizado com a última versão.";

                MessageBox.Show("Seu sistema já está na última versão disponível", "Sistema atualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            atualizarAfter.Execute();
        }

        /// <summary>
        /// Atualiza viewmodel com as informações do banco de dados
        /// </summary>
        public void AtualizaViewModel()
        {
            try
            {
                var context = new PostoContext(_configuracaoModel);
                var query = context.Query("SELECT *, (SELECT oft000.parversao FROM oft000 LIMIT 1) AS versao, NOW() AS dataAtual FROM atualiz");

                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _atualizar.Dia = reader.GetInt16(reader.GetOrdinal("atuDiaSema"));
                        _atualizar.Hora = Convert.ToInt16(reader.GetString(reader.GetOrdinal("atuHoraMin")).Split(':')[0]);
                        _atualizar.Minuto = Convert.ToInt16(reader.GetString(reader.GetOrdinal("atuHoraMin")).Split(':')[1]);
                        _atualizar.UltimaData = reader.GetDateTime(reader.GetOrdinal("atuDatAtua"));
                        _atualizar.Versao = reader.GetInt32(reader.GetOrdinal("versao"));
                        _atualizar.DataAtual = reader.GetDateTime(reader.GetOrdinal("dataAtual"));
                    }
                }
                context.Close();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }

        }

        /// <summary>
        /// Calcula o tempo para a proxima atualização
        /// </summary>
        private void SetTime()
        {
            try
            {
                AtualizaViewModel();

                var milisegundos = (_atualizar.GetDataProximaAtualizacao.Value - _atualizar.DataAtual).TotalMilliseconds;

                _timerProximaAtualizacao.Interval = milisegundos;
                _timerProximaAtualizacao.Start();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
        }

        private async void Execute(object sender, ElapsedEventArgs e)
        {
            await ExecuteAsync();
        }

        private async Task ExecuteAsync()
        {
            await Task.Run(() =>
            {
                SetaBarraStatus(System.Windows.Visibility.Visible, 0);

                //-- Verifica se existe informação para atualizar
                BuscaVersoes();

                if (_atualizar.Versao < UltimaVersao)
                {

                    //-- Ativa manutenção no banco, GeneXus se encarregará de indicar manutenção
                    ManutencaoAtiva();

                    var context = new PostoContext(_configuracaoModel);

                    try
                    {
                        context.BeginTransaction();

                        //-- Inicia a atualização
                        _atualizar.Progresso = 0;
                        _atualizar.MensagemStatus = "Iniciando atualização...";


                        //-- Encerra o processo do leitor de bombas caso parâmetro seja true
                        if (_configuracaoModel.LeitorBomba)
                        {
                            Process[] processes = Process.GetProcessesByName("uLeitor");
                            foreach (Process process in processes)
                            {
                                process.Kill();
                            }
                        }

                        //-- Encerra o processo do posto web caso parâmetro seja true
                        if (_configuracaoModel.PostoWeb)
                        {
                            Process[] processes = Process.GetProcessesByName("uPostoWe");
                            foreach (Process process in processes)
                            {
                                process.Kill();
                            }
                        }

                        _timerProximaAtualizacao.Stop();
                        AtualizaViewModel();

                        //-- Roda os Rmenu de acordo com a versão
                        _atualizar.Progresso = 10;
                        AtualizarSql(context);

                        //-- Baixa os arquivos do FTP e extrai na pasta Update
                        _atualizar.Progresso = 50;
                        AtualizarExe();

                        //-- Atualiza o parversao no banco
                        _atualizar.Progresso = 80;
                        AtualizarBanco(context);

                        //-- Salva as atualizações
                        _atualizar.Progresso = 100;
                        context.Commit();
                    }
                    catch (Exception e)
                    {
                        context.RollBack();

                        log.Error(e.Message);

                        _atualizar.MensagemStatus = "Problemas ao atualizar.";

                        //-- Reinicializa os programas
                        ReinicializaProgramas(true, context);
                    }
                    finally
                    {
                        _timerProximaAtualizacao.Stop();

                        _atualizar.MensagemStatus = "Atualizado com sucesso.";

                        SetTime();

                        //-- Indica fim de atualização
                        ManutencaoDesativa();

                        //-- Reinicializa os programas
                        ReinicializaProgramas(false, context);
                    }
                }
                else
                {   
                    var context = new PostoContext(_configuracaoModel);   
                                     
                    //-- Atualiza no banco a data da ultima verificação de atualização
                    AtualizarBanco(context);

                    //-- Atualiza o viewmodel com a nova data
                    AtualizaViewModel();

                    SetaBarraStatus(System.Windows.Visibility.Hidden, 19);

                    _atualizar.MensagemStatus = "Sistema já atualizado com a última versão.";
                }
            });
        }

        /// <summary>
        /// Atualização de arquivos
        /// </summary>
        private void AtualizarExe()
        {
            try
            {
                _atualizar.MensagemStatus = "Baixando arquivos da atualizaçãos...";

                var arquivos = _ftp.Download(PathExe, _atualizar);

                using (MemoryStream mem = new MemoryStream(arquivos))
                using (ZipArchive zipStream = new ZipArchive(mem))
                {
                    var filesCount = 1;
                    foreach (ZipArchiveEntry file in zipStream.Entries)
                    {
                        string completeFileName = Path.Combine(_configuracaoModel.LocalDiretorio, file.FullName);

                        if (file.Name == "")
                        {
                            //-- Assuming Empty for Directory
                            Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                            continue;
                        }

                        _atualizar.MensagemStatus = "Extraindo arquivo (" + filesCount + "/" + zipStream.Entries.Count.ToString() + ")";
                        file.ExtractToFile(completeFileName, true);
                        filesCount++;
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                _atualizar.MensagemStatus = "Problemas ao atualizar programas.";
            }
        }

        /// <summary>
        /// Roda Rmenu todos de uma vez e retorna a ultima versão
        /// </summary>
        private void AtualizarSql(PostoContext context)
        {
            try
            {
                BuscaVersoes();

                _atualizar.MensagemStatus = "Executando Rmenu de v." + PrimeiraVersao.ToString() + " até v." + UltimaVersao.ToString() + "";

                Rmenu = "";

                ListaSql.ForEach(row =>
                {
                    Rmenu += Encoding.ASCII.GetString(_ftp.Download((PathSql + row.Arquivo), _atualizar));
                });

                context.Query(Rmenu).ExecuteNonQuery();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                _atualizar.MensagemStatus = "Problemas ao rodar o Rmenu.";
            }

            if (UltimaVersao <= 0)
            {
                UltimaVersao = _atualizar.Versao.GetValueOrDefault();
            }
        }

        /// <summary>
        /// Atualiza data da atualização no banco de dados com a data atual
        /// </summary>
        private void AtualizarBanco(PostoContext context)
        {
            try
            {
                _atualizar.MensagemStatus = "Atualizando versão do banco de dados...";
                context.Query("UPDATE atualiz SET atuDatAtua = '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "'").ExecuteNonQuery();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                _atualizar.MensagemStatus = "Problemas ao atualizar versão.";
            }
        }

        /// <summary>
        /// Ativa manutenção no banco de dados
        /// </summary>
        private void ManutencaoAtiva()
        {
            try
            {
                _atualizar.MensagemStatus = "Preparando para a atualização...";

                var context = new PostoContext(_configuracaoModel);
                context.Query("UPDATE atualiz SET atumanuten = 'S'").ExecuteNonQuery();
                context.Query("SELECT pg_terminate_backend(PID) FROM pg_stat_activity WHERE PID <> pg_backend_pid()").ExecuteNonQuery();
                context.Close();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                _atualizar.MensagemStatus = "Problemas ao atualizar dados.";
            }

        }

        /// <summary>
        /// Desativa manutenção no banco de dados
        /// </summary>
        private void ManutencaoDesativa()
        {
            try
            {
                var context = new PostoContext(this._configuracaoModel);
                context.Query("UPDATE atualiz SET atumanuten = 'N'").ExecuteNonQuery();
                context.Close();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                _atualizar.MensagemStatus = "Problemas ao finalizar manutenção.";
            }
        }

        /// <summary>
        /// Reinicializa os programas de acordo com os parâmetros
        /// </summary>
        private void ReinicializaProgramas(bool teveErro, PostoContext context)
        {
            context.Close();
            _atualizar.Progresso = 0;
            
            //-- Se diretório existir, inicia processo
            if (File.Exists(PathAtualizador))
            {
                //-- Inicia processo do leitor de bombas caso parâmetro seja true
                if (_configuracaoModel.LeitorBomba)
                {
                    Process.Start(PathAtualizador, "6");
                }
                //-- Se diretório existir, inicia processo
                if (_configuracaoModel.PostoWeb)
                {
                    Process.Start(PathAtualizador, "7");
                }
            }
            
            if (teveErro)
            {
                MessageBox.Show("Houve um erro ao atualizar a versão do sistema. Por favor, entre em contato com o suporte.", "Erro na atualização", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            SetaBarraStatus(System.Windows.Visibility.Hidden, 19);
        }

        /// <summary>
        /// Busca as versões para atualização de acordo com a versão do cliente
        /// </summary>
        private void BuscaVersoes()
        {
            ListaSql = new List<Atualizacao>();
            RetornoFtp = _ftp.GetFileList(PathSql);

            //-- Faz a formatação da string e adiciona na lista de SQLs para rodar
            ListaSql = RetornoFtp.Where(row => row.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
                        .Select((row) => new Atualizacao
                        {
                            Versao = Convert.ToInt32(Regex.Replace(row, "[^0-9]+", "")),
                            Arquivo = row,
                        })
                        .Where(row => row.Versao > _atualizar.Versao)
                        .OrderBy(row => row.Versao)
                        .ToList();

            //-- Primeira versão Rmenu disponível
            PrimeiraVersao = ListaSql.OrderBy(x => x.Versao)
                                     .Select(x => x.Versao)
                                     .FirstOrDefault();

            //-- Última versão Rmnu disponível
            UltimaVersao = ListaSql.OrderByDescending(x => x.Versao)
                                   .Select(x => x.Versao)
                                   .FirstOrDefault();
        }

        /// <summary>
        /// Define o status da barra de progresso
        /// </summary>
        private void SetaBarraStatus(System.Windows.Visibility status, double posicao)
        {
            _mainwindowviewmodel.StackStatus.BarraProgresso.Visao = status;
            _mainwindowviewmodel.StackStatus.StatusLabel.Margin = new System.Windows.Thickness(10, posicao, 10, 0);
        }
    }
}