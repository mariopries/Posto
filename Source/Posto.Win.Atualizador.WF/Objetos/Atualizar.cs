using Atualizador.DataContext;
using Atualizador.Extensions;
using Atualizador.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Atualizador.Objetos
{
    public class Atualizar
    {
        #region Gerenciador de log

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Propriedades

        private Ftp _ftp;
        private AtualizarModel _atualizarModel;
        private System.Timers.Timer _timerProximaAtualizacao;
        private TelaPrincipal _telaPrincipal;
        private ConfiguracaoModel _configuracaoModel;

        #endregion

        #region Constantes

        private const string PathExe = "public_html/posto_att/Update.zip";

        private const string PathSql = "public_html/posto_att/rmenu/";

        private const string PathAtualizador = @"C:\Metodos\App\";

        private const string PathLeitor = @"C:\Metodos\uLeitor.exe";

        private const string PathPostoWeb = @"C:\Metodos\uPostoWe.exe";

        #endregion

        #region Variaveis

        private string Rmenu;
        private int PrimeiraVersao;
        private int UltimaVersao;
        private string[] RetornoFtp;
        private List<Atualizacao> ListaSql;

        #endregion

        #region Construtor
        /// <summary>
        /// Inicia o processo de atualização
        /// </summary>
        public Atualizar(TelaPrincipal telaPrincipal)
        {
            TelaPrincipal = telaPrincipal;
            AtualizarModel = TelaPrincipal.AbaAtualizacao.AtualizarModel;
            ConfiguracaoModel = TelaPrincipal.AbaConfiguracoes.ConfiguracaoModel;
            Ftp = new Ftp();

            TimerProximaAtualizacao = new System.Timers.Timer();
            TimerProximaAtualizacao.AutoReset = false;
            TimerProximaAtualizacao.Elapsed += Execute;

        }
        #endregion

        #region Objetos

        public Ftp Ftp
        {
            get { return _ftp; }
            set { if (_ftp != value) { _ftp = value; } }
        }
        public AtualizarModel AtualizarModel
        {
            get { return _atualizarModel; }
            set { if (_atualizarModel != value) { _atualizarModel = value; } }
        }
        public ConfiguracaoModel ConfiguracaoModel
        {
            get { return _configuracaoModel; }
            set { if (_configuracaoModel != value) { _configuracaoModel = value; } }
        }
        public TelaPrincipal TelaPrincipal
        {
            get { return _telaPrincipal; }
            set { if (_telaPrincipal != value) { _telaPrincipal = value; } }
        }
        public System.Timers.Timer TimerProximaAtualizacao
        {
            get { return _timerProximaAtualizacao; }
            set { if (_timerProximaAtualizacao != value) { _timerProximaAtualizacao = value; } }
        }

        #endregion

        #region Funções

        /// <summary>
        /// Inicia o timer que busca por atualizações
        /// </summary>
        public void Iniciar()
        {
            SetTime();
        }

        /// <summary>
        /// Pausa o timer que busca por atualizações
        /// </summary>
        public void Pausar()
        {
            TimerProximaAtualizacao.Stop();
        }

        /// <summary>
        /// Inicia o processo de atualização manualmente
        /// </summary>
        public async void Manual(bool backup = false, bool reindex = false, bool vacuum = false)
        {
            await AtualizaViewModel();
            await BuscaVersoes();
            await ExecuteAsync(backup, vacuum, reindex);
            TimerProximaAtualizacao.Stop();
            TelaPrincipal.AtualizarAfter();
        }

        /// <summary>
        /// Atualiza viewmodel com as informações do banco de dados
        /// </summary>
        public async Task<bool> AtualizaViewModel()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var context = new PostoContext(ConfiguracaoModel);
                    var query = context.Query("SELECT *, (SELECT oft000.parversao FROM oft000 LIMIT 1) AS versao, NOW() AS dataAtual FROM atualiz");

                    using (var reader = query.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AtualizarModel.Dia = reader.GetInt16(reader.GetOrdinal("atuDiaSema"));
                            AtualizarModel.Hora = Convert.ToInt16(reader.GetString(reader.GetOrdinal("atuHoraMin")).Split(':')[0]);
                            AtualizarModel.Minuto = Convert.ToInt16(reader.GetString(reader.GetOrdinal("atuHoraMin")).Split(':')[1]);
                            AtualizarModel.UltimaData = reader.GetDateTime(reader.GetOrdinal("atuDatAtua"));
                            AtualizarModel.Versao = reader.GetInt32(reader.GetOrdinal("versao"));
                            AtualizarModel.DataAtual = reader.GetDateTime(reader.GetOrdinal("dataAtual"));
                        }
                    }
                    context.Close();
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    return false;
                }
            });
        }

        /// <summary>
        /// Calcula o tempo para a proxima atualização
        /// </summary>
        private async void SetTime()
        {
            try
            {
                await AtualizaViewModel();

                var milisegundos = (AtualizarModel.GetDataProximaAtualizacao.Value - AtualizarModel.DataAtual).TotalMilliseconds;

                TimerProximaAtualizacao.Interval = milisegundos;
                TimerProximaAtualizacao.Start();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
        }

        /// <summary>
        /// Execulta a task de atualizaçao
        /// </summary>
        private async void Execute(object sender, ElapsedEventArgs e)
        {
            await ExecuteAsync(ConfiguracaoModel.Backup, ConfiguracaoModel.Vacuum, ConfiguracaoModel.Reindex);
        }

        /// <summary>
        /// Task de atualização
        /// </summary>
        private async Task ExecuteAsync(bool backup, bool vacuum, bool reindex)
        {
            await Task.Run(async () =>
            {
                try
                {
                    SetaBarraStatus(true, 0);

                    //-- Verifica se existe informação para atualizar
                    TelaPrincipal.AbaAtualizacao.LabelContent = "Buscando versões do sistema...";

                    if (!await BuscaVersoes())
                    {
                        throw new Exception("Problemas com a comunicação com o servidor ftp");
                    }

                    //if (AtualizarModel.Versao < UltimaVersao)
                    //{
                    //-- Ativa manutenção no banco, GeneXus se encarregará de indicar manutenção                        
                    if (!await ManutencaoAtiva())
                    {
                        throw new Exception("Problemas com a comunicação com o banco de dados.");
                    }

                    var context = new PostoContext(ConfiguracaoModel);

                    //-- Inicia a atualização
                    TelaPrincipal.AbaAtualizacao.ProgressoBarra2 = 0;
                    TelaPrincipal.AbaAtualizacao.LabelContent = "Iniciando atualização...";

                    context.BeginTransaction();

                    //-- Encerra o processo do leitor de bombas caso parâmetro seja true
                    if (ConfiguracaoModel.LeitorBomba)
                    {
                        Process[] processes = Process.GetProcessesByName("uLeitor");
                        foreach (Process process in processes)
                        {
                            process.Kill();
                        }
                    }

                    //-- Encerra o processo do posto web caso parâmetro seja true
                    if (ConfiguracaoModel.PostoWeb)
                    {
                        Process[] processes = Process.GetProcessesByName("uPostoWe");
                        foreach (Process process in processes)
                        {
                            process.Kill();
                        }
                    }

                    TimerProximaAtualizacao.Stop();

                    if (!await AtualizaViewModel())
                    {
                        throw new Exception("Problemas ao atualizar informações do programa.");
                    }

                    TelaPrincipal.AbaAtualizacao.ProgressoBarra2 = 10;

                    //-- Roda os Rmenu de acordo com a versão
                    if (!await AtualizarSql(context, backup))
                    {
                        throw new Exception("Problemas ao rodar o rmenu.");
                    }

                    if (vacuum)
                    {
                        TelaPrincipal.AbaAtualizacao.ProgressoBarra2 = 32;

                        //-- Roda comando de Vacuum no banco de dados
                        if (!await ExecutaVacuum())
                        {
                            throw new Exception("Ocorreu um erro ao tentar rodar o comando de vacuum no banco de dados.");
                        }
                    }

                    if (reindex)
                    {
                        TelaPrincipal.AbaAtualizacao.ProgressoBarra2 = 40;

                        //-- Roda comando de Reindex no banco de dados
                        if (!await ExecutaReindex())
                        {
                            throw new Exception("Ocorreu um erro ao tentar rodar o comando de reindex no banco de dados.");
                        }
                    }

                    TelaPrincipal.AbaAtualizacao.ProgressoBarra2 = 50;

                    //-- Baixa os arquivos do FTP e extrai na pasta Update
                    if (!await AtualizarExe())
                    {
                        throw new Exception("Problemas com o download dos arquivos.");
                    }

                    TelaPrincipal.AbaAtualizacao.ProgressoBarra2 = 80;

                    //-- Atualiza o parversao no banco
                    if (!await AtualizarBanco(context))
                    {
                        throw new Exception("Problemas ao atualizar o banco de dados.");
                    }

                    if (!await ManutencaoDesativa())
                    {
                        throw new Exception("Problemas ao finalizar a manutenção.");
                    }

                    SetTime();
                    TimerProximaAtualizacao.Stop();
                    TelaPrincipal.AbaAtualizacao.LabelContent = "Atualizado com sucesso.";
                    TelaPrincipal.AbaAtualizacao.ProgressoBarra2 = 100;
                    ReinicializaProgramas(false, context);
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    TelaPrincipal.AbaAtualizacao.LabelContent = "Problemas ao atualizar.";
                    ReinicializaProgramas(true, null);
                }
            });

        }


        private void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //* Do your stuff with the output (write to console/log/StringBuilder)
            TelaPrincipal.AbaAtualizacao.LabelContent = outLine.Data;
            //Console.WriteLine(outLine.Data);
        }

        /// <summary>
        /// Executa rotina de backup do banco
        /// </summary>
        private async Task<bool> ExecutaBackup()
        {
            return await Task.Run(() =>
            {
                try
                {
                    TelaPrincipal.AbaAtualizacao.IsIndeterminateBarra1 = true;
                    DateTime Time = DateTime.Now;
                    using (Process processo = new Process())
                    {
                        processo.StartInfo.FileName = Environment.GetEnvironmentVariable("comspec");
                        processo.StartInfo.WorkingDirectory = ConfiguracaoModel.DiretorioPostgreSql;
                        processo.StartInfo.EnvironmentVariables["PGUSER"] = ConfiguracaoModel.Usuario;
                        processo.StartInfo.EnvironmentVariables["PGPASSWORD"] = ConfiguracaoModel.Senha;
                        // Formata a string para passar como argumento para o cmd.exe
                        processo.StartInfo.Arguments = string.Format("/c {0}", $@"pg_dump.exe --host {ConfiguracaoModel.Servidor} --port {ConfiguracaoModel.Porta} --username {ConfiguracaoModel.Usuario} --no-password  --format custom --blobs --verbose --file {ConfiguracaoModel.DiretorioSistema}\backup\backup_{Time.Day.ToString("00")}{Time.Month.ToString("00")}{Time.Year.ToString("0000")}{Time.Hour.ToString("00")}{Time.Minute.ToString("00")}{Time.Second.ToString("00")}.backup {ConfiguracaoModel.Banco}");
                        processo.StartInfo.RedirectStandardOutput = true;
                        processo.StartInfo.UseShellExecute = false;
                        processo.StartInfo.CreateNoWindow = true;
                        processo.Start();
                        processo.WaitForExit();
                    }
                    TelaPrincipal.AbaAtualizacao.IsIndeterminateBarra1 = false;
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    TelaPrincipal.AbaAtualizacao.LabelContent = "Problemas ao executa o backup.";
                    return false;
                }
            });
        }

        /// <summary>
        /// Executa reindex no do banco
        /// </summary>
        private async Task<bool> ExecutaVacuum()
        {
            return await Task.Run(() =>
            {
                try
                {
                    TelaPrincipal.AbaAtualizacao.IsIndeterminateBarra1 = true;
                    TelaPrincipal.AbaAtualizacao.LabelContent = "Executando comando vacuum no banco de dados";
                    using (Process processo = new Process())
                    {
                        processo.StartInfo.FileName = Environment.GetEnvironmentVariable("comspec");
                        processo.StartInfo.WorkingDirectory = ConfiguracaoModel.DiretorioPostgreSql;
                        processo.StartInfo.EnvironmentVariables["PGUSER"] = ConfiguracaoModel.Usuario;
                        processo.StartInfo.EnvironmentVariables["PGPASSWORD"] = ConfiguracaoModel.Senha;
                        // Formata a string para passar como argumento para o cmd.exe
                        processo.StartInfo.Arguments = string.Format("/c {0}", $@"vacuumdb.exe -h {ConfiguracaoModel.Servidor} -p {ConfiguracaoModel.Porta} -U {ConfiguracaoModel.Usuario} --no-password -d {ConfiguracaoModel.Banco} --full --analyze --verbose");
                        processo.StartInfo.RedirectStandardOutput = true;
                        processo.StartInfo.UseShellExecute = false;
                        processo.StartInfo.CreateNoWindow = true;
                        processo.Start();
                        processo.WaitForExit();
                    }

                    TelaPrincipal.AbaAtualizacao.IsIndeterminateBarra1 = false;
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    TelaPrincipal.AbaAtualizacao.LabelContent = "Problemas ao executa o reindex.";
                    TelaPrincipal.AbaAtualizacao.IsIndeterminateBarra1 = false;
                    return false;
                }
            });
        }

        /// <summary>
        /// Executa reindex no do banco
        /// </summary>
        private async Task<bool> ExecutaReindex()
        {
            return await Task.Run(() =>
            {
                try
                {
                    TelaPrincipal.AbaAtualizacao.IsIndeterminateBarra1 = true;
                    TelaPrincipal.AbaAtualizacao.LabelContent = "Executando comando reindex no banco de dados";
                    using (Process processo = new Process())
                    {
                        processo.StartInfo.FileName = Environment.GetEnvironmentVariable("comspec");
                        processo.StartInfo.WorkingDirectory = ConfiguracaoModel.DiretorioPostgreSql;
                        processo.StartInfo.EnvironmentVariables["PGUSER"] = ConfiguracaoModel.Usuario;
                        processo.StartInfo.EnvironmentVariables["PGPASSWORD"] = ConfiguracaoModel.Senha;
                        // Formata a string para passar como argumento para o cmd.exe
                        processo.StartInfo.Arguments = string.Format("/c {0}", $@"reindexdb.exe -h {ConfiguracaoModel.Servidor} -p {ConfiguracaoModel.Porta} -U {ConfiguracaoModel.Usuario} -d {ConfiguracaoModel.Banco} --no-password --verbose");
                        processo.StartInfo.RedirectStandardOutput = true;
                        processo.StartInfo.UseShellExecute = false;
                        processo.StartInfo.CreateNoWindow = true;
                        processo.Start();
                        processo.WaitForExit();
                    }
                    TelaPrincipal.AbaAtualizacao.IsIndeterminateBarra1 = false;
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    TelaPrincipal.AbaAtualizacao.LabelContent = "Problemas ao executa o reindex.";
                    TelaPrincipal.AbaAtualizacao.IsIndeterminateBarra1 = false;
                    return false;
                }
            });
        }

        /// <summary>
        /// Atualização de arquivos
        /// </summary>
        private async Task<bool> AtualizarExe()
        {
            return await Task.Run(() =>
            {
                try
                {
                    TelaPrincipal.AbaAtualizacao.IsIndeterminateBarra1 = false;
                    var FileLastModified = Ftp.GetFileLasModified(PathExe);

                    if (!FileLastModified.ToString().Equals(ConfiguracaoModel.VersaoArquivo))
                    {
                        TelaPrincipal.AbaAtualizacao.LabelContent = "Baixando arquivos da atualizaçãos...";
                        var arquivos = Ftp.Download(PathExe, TelaPrincipal);

                        using (MemoryStream mem = new MemoryStream(arquivos))
                        using (ZipArchive zipStream = new ZipArchive(mem))
                        {
                            var filesCount = 1;
                            foreach (ZipArchiveEntry file in zipStream.Entries)
                            {
                                string completeFileName = Path.Combine($@"{ConfiguracaoModel.DiretorioSistema}\Update", file.FullName);
                                string directory = Path.GetDirectoryName(completeFileName);

                                if (!Directory.Exists(directory))
                                    Directory.CreateDirectory(directory);
                                if (file.Name != "")
                                    file.ExtractToFile(completeFileName, true);

                                TelaPrincipal.AbaAtualizacao.LabelContent = "Extraindo arquivo (" + filesCount + "/" + zipStream.Entries.Count.ToString() + ")";
                                TelaPrincipal.AbaAtualizacao.ProgressoBarra1 = Math.Min((int)((double)filesCount / zipStream.Entries.Count) * 100, 100);
                                //file.ExtractToFile(completeFileName, true);
                                filesCount++;
                            }
                        }
                    }

                    ConfiguracaoModel.VersaoArquivo = FileLastModified.ToString().Trim();
                    ConfiguracaoModel.ToModel().GravarConfiguracao();
                    TelaPrincipal.Indicadores.AtualizouExe = true;
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    TelaPrincipal.AbaAtualizacao.LabelContent = "Problemas ao atualizar programas.";
                    return false;
                }
            });
        }

        /// <summary>
        /// Roda Rmenu todos de uma vez e retorna a ultima versão
        /// </summary>
        private async Task<bool> AtualizarSql(PostoContext context, bool backup)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    await BuscaVersoes();

                    Rmenu = "";

                    TelaPrincipal.AbaAtualizacao.LabelContent = "Baixando o Rmenu...";
                    ListaSql.ForEach(row =>
                    {
                        Rmenu += Encoding.ASCII.GetString(Ftp.Download((PathSql + row.Arquivo), TelaPrincipal));
                    });

                    if (backup)
                    {
                        TelaPrincipal.AbaAtualizacao.ProgressoBarra2 = 17;

                        if (Rmenu != "")
                        {
                            if (ConfiguracaoModel.DiretorioPostgreSql == "")
                            {
                                throw new Exception("Não definido o diretório para do postgres para a rotina de backup do banco.");
                            }
                            else
                            {
                                TelaPrincipal.AbaAtualizacao.LabelContent = "Gerando backup do banco de dados";

                                if (!await ExecutaBackup())
                                {
                                    throw new Exception("Ocorreu um erro ao tentar gerar backup antes de atualizar o sistema.");
                                }
                            }
                        }
                    }

                    TelaPrincipal.AbaAtualizacao.ProgressoBarra2 = 25;
                    TelaPrincipal.AbaAtualizacao.IsIndeterminateBarra1 = true;
                    TelaPrincipal.AbaAtualizacao.LabelContent = "Executando Rmenu de v." + AtualizarModel.Versao.ToString() + " até v." + UltimaVersao.ToString() + "";

                    context.Query(Rmenu).ExecuteNonQuery();

                    TelaPrincipal.Indicadores.AtualizouBanco = true;
                    TelaPrincipal.AbaAtualizacao.IsIndeterminateBarra1 = false;
                }
                catch (Exception e)
                {
                    context.RollBack();
                    log.Error(e.Message);
                    TelaPrincipal.AbaAtualizacao.LabelContent = "Problemas ao rodar o Rmenu.";

                    return false;
                }
                finally
                {
                    context.Commit();

                    if (UltimaVersao <= 0)
                    {
                        UltimaVersao = AtualizarModel.Versao.GetValueOrDefault();
                    }
                }
                return true;
            });
        }

        /// <summary>
        /// Atualiza data da atualização no banco de dados com a data atual
        /// </summary>
        private async Task<bool> AtualizarBanco(PostoContext context)
        {
            return await Task.Run(() =>
            {
                try
                {
                    TelaPrincipal.AbaAtualizacao.LabelContent = "Atualizando versão do banco de dados...";
                    context.Query("UPDATE atualiz SET atuDatAtua = '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "'").ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    TelaPrincipal.AbaAtualizacao.LabelContent = "Problemas ao atualizar versão.";
                    return false;
                }
            });
        }

        /// <summary>
        /// Ativa manutenção no banco de dados
        /// </summary>
        private async Task<bool> ManutencaoAtiva()
        {
            return await Task.Run(() =>
            {
                try
                {
                    TelaPrincipal.AbaAtualizacao.LabelContent = "Preparando para a atualização...";
                    TelaPrincipal.Indicadores.EmManutencao = true;
                    TelaPrincipal.Indicadores.FimManutencao = false;
                    TelaPrincipal.Indicadores.AtualizouBanco = false;
                    TelaPrincipal.Indicadores.AtualizouExe = false;
                    TelaPrincipal.Indicadores.DerrubaConexoes();
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    TelaPrincipal.AbaAtualizacao.LabelContent = "Problemas ao atualizar dados.";
                    return false;
                }
            });
        }

        /// <summary>
        /// Desativa manutenção no banco de dados
        /// </summary>
        private async Task<bool> ManutencaoDesativa()
        {
            return await Task.Run(() =>
            {
                try
                {
                    TelaPrincipal.Indicadores.EmManutencao = false;
                    TelaPrincipal.Indicadores.FimManutencao = true;
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    TelaPrincipal.AbaAtualizacao.LabelContent = "Problemas ao finalizar manutenção.";
                    return false;
                }
            });
        }

        /// <summary>
        /// Reinicializa os programas de acordo com os parâmetros
        /// </summary>
        private void ReinicializaProgramas(bool teveErro, PostoContext context)
        {
            try
            {
                if (context != null)
                {
                    context.Close();
                }

                TelaPrincipal.AbaAtualizacao.ProgressoBarra2 = 0;

                //-- Se diretório existir, inicia processo
                if (File.Exists(PathAtualizador))
                {
                    //-- Inicia processo do leitor de bombas caso parâmetro seja true
                    if (ConfiguracaoModel.LeitorBomba)
                        Execute("6");
                    //-- Se diretório existir, inicia processo
                    if (ConfiguracaoModel.PostoWeb)
                        Execute("7");
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }

            if (teveErro)
            {
                MessageBox.Show("Houve um erro ao atualizar a versão do sistema. Por favor, entre em contato com o suporte.", "Erro na atualização", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            SetaBarraStatus(false, 25);
        }

        private void Execute(string Parm)
        {
            var processo = new Process();

            try
            {
                processo.StartInfo.FileName = "Posto.exe";
                processo.StartInfo.WorkingDirectory = PathAtualizador;
                processo.StartInfo.Arguments = Parm;
                processo.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                processo.Start();
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                log.Error(e.Message);
            }
            finally
            {
                processo.Close();
                processo.Dispose();
                processo = null;
            }
        }
        /// <summary>
        /// Busca as versões para atualização de acordo com a versão do cliente
        /// </summary>
        private async Task<bool> BuscaVersoes()
        {
            return await Task.Run(() =>
            {
                try
                {
                    ListaSql = new List<Atualizacao>();
                    RetornoFtp = Ftp.GetFileList(PathSql);

                    //-- Faz a formatação da string e adiciona na lista de SQLs para rodar
                    ListaSql = RetornoFtp.Where(row => row.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
                                .Select((row) => new Atualizacao
                                {
                                    Versao = Convert.ToInt32(Regex.Replace(row, "[^0-9]+", "")),
                                    Arquivo = row,
                                })
                                .Where(row => row.Versao > AtualizarModel.Versao)
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

                    return true;
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    TelaPrincipal.AbaAtualizacao.LabelContent = "Problemas ao buscar versões.";
                    return false;
                }

            });
        }

        /// <summary>
        /// Define o status da barra de progresso
        /// </summary>
        private void SetaBarraStatus(bool status, int posicao)
        {
            TelaPrincipal.AbaAtualizacao.IsVisibleBarras = status;
            TelaPrincipal.AbaAtualizacao.LabelPadding = new Padding(0, posicao, 0, 0);
        }

        #endregion

        #region Classes

        /// <summary>
        /// Estrutura para a lista de aquivos Rmenu
        /// </summary>
        public class Atualizacao
        {
            public int Versao;
            public string Arquivo;
        }

        #endregion
    }
}
