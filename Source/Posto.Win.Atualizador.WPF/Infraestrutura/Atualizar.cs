using log4net;
using Microsoft.Practices.Prism.Commands;
using Posto.Win.Update.DataContext;
using Posto.Win.Update.Extensions;
using Posto.Win.Update.Model;
using Posto.Win.Update.ViewModel;
using SharpCompress.Archives;
using SharpCompress.Common;
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
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Media;

namespace Posto.Win.Update.Infraestrutura
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
        private MainWindowViewModel _mainwindowviewmodel;
        private ConfiguracaoModel _configuracaoModel;

        #endregion

        #region Constantes

        private const string FtpPath = "public_html/posto_att/";

        #endregion

        #region Variaveis

        private string Rmenu;
        private int PrimeiraVersao;
        private int UltimaVersao;
        private List<Atualizacao> ListaSql;

        #endregion

        #region Construtor
        /// <summary>
        /// Inicia o processo de atualização
        /// </summary>
        public Atualizar(MainWindowViewModel mainwindowviewmodel)
        {
            MainWindowViewModel = mainwindowviewmodel;
            AtualizarModel = MainWindowViewModel.AbaAtualizar.AtualizarModel;
            ConfiguracaoModel = MainWindowViewModel.AbaConfiguracoes.ConfiguracaoModel;
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
        public MainWindowViewModel MainWindowViewModel
        {
            get { return _mainwindowviewmodel; }
            set { if (_mainwindowviewmodel != value) { _mainwindowviewmodel = value; } }
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
        public async void Manual(DelegateCommand atualizarAfter, bool backup = false, bool reindex = false, bool vacuum = false)
        {
            await AtualizaViewModel();
            await BuscaVersoes();
            await ExecuteAsync(backup, vacuum, reindex);
            TimerProximaAtualizacao.Stop();
            atualizarAfter.Execute();
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
                    SetaBarraStatus(System.Windows.Visibility.Visible, 0);

                    //-- Verifica se existe informação para atualizar
                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Buscando versões do sistema...";

                    if (!await BuscaVersoes())
                    {
                        throw new Exception("Problemas com a comunicação com o servidor ftp");
                    }

                    //-- Ativa manutenção no banco, GeneXus se encarregará de indicar manutenção                        
                    if (!await ManutencaoAtiva())
                    {
                        throw new Exception("Problemas com a comunicação com o banco de dados.");
                    }

                    var context = new PostoContext(ConfiguracaoModel);

                    //-- Inicia a atualização
                    MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.ProgressoBarra2 = 0;
                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Iniciando atualização...";

                    context.BeginTransaction();

                    //-- Encerra o processo do leitor de bombas caso parâmetro seja true
                    if (ConfiguracaoModel.LeitorBomba)
                    {
                        Kill("uLeitor");
                    }

                    //-- Encerra o processo do posto web caso parâmetro seja true
                    if (ConfiguracaoModel.PostoWeb)
                    {
                        Kill("uPostoWe");
                    }

                    TimerProximaAtualizacao.Stop();

                    if (!await AtualizaViewModel())
                    {
                        throw new Exception("Problemas ao atualizar informações do programa.");
                    }

                    MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.ProgressoBarra2 = 10;

                    //-- Roda os Rmenu de acordo com a versão
                    if (!await AtualizarSql(context, backup))
                    {
                        throw new Exception("Problemas ao rodar o rmenu.");
                    }

                    if (vacuum)
                    {
                        MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.ProgressoBarra2 = 32.5;

                        //-- Roda comando de Vacuum no banco de dados
                        if (!await ExecutaVacuum())
                        {
                            throw new Exception("Ocorreu um erro ao tentar rodar o comando de vacuum no banco de dados.");
                        }
                    }

                    if (reindex)
                    {
                        MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.ProgressoBarra2 = 40;

                        //-- Roda comando de Reindex no banco de dados
                        if (!await ExecutaReindex())
                        {
                            throw new Exception("Ocorreu um erro ao tentar rodar o comando de reindex no banco de dados.");
                        }
                    }

                    MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.ProgressoBarra2 = 50;

                    //-- Baixa os arquivos do FTP e extrai na pasta Update
                    if (!await AtualizarExe())
                    {
                        throw new Exception("Problemas com o download dos arquivos.");
                    }

                    MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.ProgressoBarra2 = 80;

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
                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Atualizado com sucesso.";
                    MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.ProgressoBarra2 = 100;
                    ReinicializaProgramas(false, context);
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Problemas ao atualizar.";
                    ReinicializaProgramas(true, null);
                }
            });

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
                    MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.IsIndeterminateBarra1 = true;
                    DateTime Time = DateTime.Now;

                    // Verifica se o diretório já existe, se não, irá criar um novo
                    if (!Directory.Exists($@"{ConfiguracaoModel.LocalDiretorio}\backup"))
                        Directory.CreateDirectory($@"{ConfiguracaoModel.LocalDiretorio}\backup");

                    using (Process processo = new Process())
                    {
                        processo.StartInfo.FileName = Environment.GetEnvironmentVariable("comspec");
                        processo.StartInfo.WorkingDirectory = ConfiguracaoModel.LocalPostgres;
                        processo.StartInfo.EnvironmentVariables["PGUSER"] = ConfiguracaoModel.Usuario;
                        processo.StartInfo.EnvironmentVariables["PGPASSWORD"] = ConfiguracaoModel.Senha;
                        // Formata a string para passar como argumento para o cmd.exe
                        processo.StartInfo.Arguments = string.Format("/c {0}", $@"pg_dump.exe --host {ConfiguracaoModel.Servidor} --port {ConfiguracaoModel.Porta} --username {ConfiguracaoModel.Usuario} --no-password  --format custom --blobs --verbose --file {ConfiguracaoModel.LocalDiretorio}\backup\backup_{Time.Day.ToString("00")}{Time.Month.ToString("00")}{Time.Year.ToString("0000")}{Time.Hour.ToString("00")}{Time.Minute.ToString("00")}{Time.Second.ToString("00")}.backup {ConfiguracaoModel.Banco}");
                        processo.StartInfo.RedirectStandardOutput = true;
                        processo.StartInfo.UseShellExecute = false;
                        processo.StartInfo.CreateNoWindow = true;
                        processo.Start();
                        processo.WaitForExit();
                    }
                    MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.IsIndeterminateBarra1 = false;
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Problemas ao executa o backup.";
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
                    MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.IsIndeterminateBarra1 = true;
                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Executando comando vacuum no banco de dados";
                    using (Process processo = new Process())
                    {
                        processo.StartInfo.FileName = Environment.GetEnvironmentVariable("comspec");
                        processo.StartInfo.WorkingDirectory = ConfiguracaoModel.LocalPostgres;
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

                    MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.IsIndeterminateBarra1 = false;
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Problemas ao executa o reindex.";
                    MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.IsIndeterminateBarra1 = false;
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
                    MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.IsIndeterminateBarra1 = true;
                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Executando comando reindex no banco de dados";
                    using (Process processo = new Process())
                    {
                        processo.StartInfo.FileName = Environment.GetEnvironmentVariable("comspec");
                        processo.StartInfo.WorkingDirectory = ConfiguracaoModel.LocalPostgres;
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
                    MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.IsIndeterminateBarra1 = false;
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Problemas ao executa o reindex.";
                    MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.IsIndeterminateBarra1 = false;
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
                    MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.IsIndeterminateBarra1 = false;

                    var RetornoFtp = Ftp.GetFileList(FtpPath);

                    RetornoFtp.ForEach(arquivo =>
                    {
                        if (arquivo.EndsWith(".rar", StringComparison.OrdinalIgnoreCase) || arquivo.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                        {
                            var FileLastModified = Ftp.GetFileLasModified(FtpPath + arquivo);

                            if (!FileLastModified.ToString().Equals(ConfiguracaoModel.VersaoArquivo))
                            {
                                MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Baixando arquivos da atualizaçãos...";
                                var arquivos = Ftp.Download(FtpPath + arquivo, MainWindowViewModel);

                                using (MemoryStream mem = new MemoryStream(arquivos))
                                using (var file = ArchiveFactory.Open(mem))
                                {
                                    var filesCount = 1;
                                    file.Entries.ToList().ForEach(entry =>
                                    {
                                        if (!entry.IsDirectory && !entry.Key.Contains("~$"))
                                        {
                                            string completeFileName = Path.Combine($@"{ConfiguracaoModel.LocalDiretorio}\Update", entry.Key);
                                            string directory = Path.GetDirectoryName(completeFileName);

                                            if (entry.Key != "")
                                                entry.WriteToDirectory($@"{ConfiguracaoModel.LocalDiretorio}\Update", new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });

                                            MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Extraindo arquivo (" + filesCount + "/" + file.Entries.Count().ToString() + ")";
                                            MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.ProgressoBarra1 = ((double)filesCount / file.Entries.Count()) * 100;

                                            filesCount++;
                                        }
                                    });
                                }
                            }
                            
                            ConfiguracaoModel.VersaoArquivo = FileLastModified.ToString().Trim();
                            ConfiguracaoModel.ToModel().GravarConfiguracao();
                            MainWindowViewModel.Indicadores.AtualizouExe = true;
                        }

                    });

                    return true;
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Problemas ao atualizar programas.";
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

                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Baixando o Rmenu...";
                    ListaSql.ForEach(row =>
                    {
                        Rmenu += Encoding.ASCII.GetString(Ftp.Download(($"{FtpPath}/rmenu/{row.Arquivo}"), MainWindowViewModel));
                    });
                    if (Rmenu != "")
                    {
                        if (backup)
                        {
                            MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.ProgressoBarra2 = 17.5;

                            if (MainWindowViewModel.AbaConfiguracoes.ConfiguracaoModel.LocalPostgres == "")
                            {
                                throw new Exception("Não definido o diretório para do postgres para a rotina de backup do banco.");
                            }
                            else
                            {
                                MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Gerando backup do banco de dados";

                                if (!await ExecutaBackup())
                                {
                                    throw new Exception("Ocorreu um erro ao tentar gerar backup antes de atualizar o sistema.");
                                }
                            }
                        }

                        MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.ProgressoBarra2 = 25;
                        MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.IsIndeterminateBarra1 = true;
                        MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Executando Rmenu de v." + AtualizarModel.Versao.ToString() + " até v." + UltimaVersao.ToString() + "";

                        context.Query(Rmenu).ExecuteNonQuery();
                    }

                    MainWindowViewModel.Indicadores.AtualizouBanco = true;
                    MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.IsIndeterminateBarra1 = false;
                }
                catch (Exception e)
                {
                    context.RollBack();
                    log.Error(e.Message);
                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Problemas ao rodar o Rmenu.";
                    
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
                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Atualizando versão do banco de dados...";
                    context.Query("UPDATE atualiz SET atuDatAtua = '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "'").ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Problemas ao atualizar versão.";
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
                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Preparando para a atualização...";
                    MainWindowViewModel.Indicadores.EmManutencao = true;
                    MainWindowViewModel.Indicadores.FimManutencao = false;
                    MainWindowViewModel.Indicadores.AtualizouBanco = false;
                    MainWindowViewModel.Indicadores.AtualizouExe = false;
                    MainWindowViewModel.Indicadores.DerrubaConexoes();
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Problemas ao atualizar dados.";
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
                    MainWindowViewModel.Indicadores.EmManutencao = false;
                    MainWindowViewModel.Indicadores.FimManutencao = true;
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Problemas ao finalizar manutenção.";
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

                MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.ProgressoBarra2 = 0;

                //-- Se diretório existir, inicia processo
                if (File.Exists($@"{ConfiguracaoModel.LocalDiretorio}\App\Posto.exe"))
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

            SetaBarraStatus(System.Windows.Visibility.Hidden, 25);
        }

        private void Execute(string Parm)
        {
            var processo = new Process();

            try
            {
                processo.StartInfo.FileName = $@"{ConfiguracaoModel.LocalDiretorio}\App\Posto.exe";
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

        private void Kill(string Parm)
        {
            Process[] processes = Process.GetProcessesByName(Parm);
            foreach (Process process in processes)
            {
                process.Kill();
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
                    var RetornoFtp = Ftp.GetFileList($"{FtpPath}/rmenu/");

                    //-- Faz a formatação da string e adiciona na lista de SQLs para rodar
                    ListaSql = RetornoFtp.Where(row => row.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
                                .Select((row) => new Atualizacao
                                {
                                    Versao = Convert.ToInt32(Regex.Replace(row.Substring(row.IndexOf("v_"), row.Length - row.IndexOf("v_")), "[^0-9]+", "")),
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
                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Problemas ao buscar versões.";
                    return false;
                }

            });
        }

        /// <summary>
        /// Define o status da barra de progresso
        /// </summary>
        private void SetaBarraStatus(System.Windows.Visibility status, double posicao)
        {
            MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.Visao = status;
            MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelMargin = new System.Windows.Thickness(10, posicao, 10, 0);
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