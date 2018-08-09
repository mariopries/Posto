﻿using log4net;
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

        private const string PathExe = "public_html/posto_att/Update.zip";

        private const string PathSql = "public_html/posto_att/rmenu/";

        private const string PathAtualizador = @"C:\Metodos\App\Posto.exe";

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
        public async void Manual(DelegateCommand atualizarAfter)
        {
            AtualizaViewModel();
            BuscaVersoes();
            await ExecuteAsync();
            TimerProximaAtualizacao.Stop();
            atualizarAfter.Execute();
        }

        /// <summary>
        /// Atualiza viewmodel com as informações do banco de dados
        /// </summary>
        public void AtualizaViewModel()
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
            await ExecuteAsync();
        }

        /// <summary>
        /// Task de atualização
        /// </summary>
        private async Task ExecuteAsync()
        {
            await Task.Run(() =>
            {
                SetaBarraStatus(System.Windows.Visibility.Visible, 0);

                //-- Verifica se existe informação para atualizar
                BuscaVersoes();

                //-- Ativa manutenção no banco, GeneXus se encarregará de indicar manutenção
                ManutencaoAtiva();

                if (AtualizarModel.Versao < UltimaVersao)
                {
                    var context = new PostoContext(ConfiguracaoModel);

                    try
                    {
                        context.BeginTransaction();

                        //-- Inicia a atualização
                        MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.ProgressoBarra2 = 0;
                        MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Iniciando atualização...";


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
                        AtualizaViewModel();

                        //-- Roda os Rmenu de acordo com a versão
                        MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.ProgressoBarra2 = 10;
                        AtualizarSql(context);

                        //-- Baixa os arquivos do FTP e extrai na pasta Update
                        MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.ProgressoBarra2 = 50;
                        AtualizarExe();

                        //-- Atualiza o parversao no banco
                        MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.ProgressoBarra2 = 80;
                        AtualizarBanco(context);

                        //-- Salva as atualizações
                        MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.ProgressoBarra2 = 100;
                        context.Commit();
                    }
                    catch (Exception e)
                    {
                        context.RollBack();

                        log.Error(e.Message);

                        MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Problemas ao atualizar.";

                        //-- Reinicializa os programas
                        ReinicializaProgramas(true, context);
                    }
                    finally
                    {
                        TimerProximaAtualizacao.Stop();

                        MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Atualizado com sucesso.";

                        SetTime();

                        //-- Reinicializa os programas
                        ReinicializaProgramas(false, context);
                    }
                }
                else
                {   
                    var context = new PostoContext(ConfiguracaoModel);   
                                     
                    //-- Atualiza no banco a data da ultima verificação de atualização
                    AtualizarBanco(context);

                    //-- Atualiza o viewmodel com a nova data
                    AtualizaViewModel();

                    SetaBarraStatus(System.Windows.Visibility.Hidden, 25);

                    MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Sistema já atualizado com a última versão.";
                }

                //-- Indica fim de atualização
                ManutencaoDesativa();
                
            });
        }

        /// <summary>
        /// Atualização de arquivos
        /// </summary>
        private void AtualizarExe()
        {
            try
            {
                MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Baixando arquivos da atualizaçãos...";

                var arquivos = Ftp.Download(PathExe, MainWindowViewModel);

                using (MemoryStream mem = new MemoryStream(arquivos))
                using (ZipArchive zipStream = new ZipArchive(mem))
                {
                    var filesCount = 1;
                    foreach (ZipArchiveEntry file in zipStream.Entries)
                    {
                        string completeFileName = Path.Combine(ConfiguracaoModel.LocalDiretorio, file.FullName);

                        if (file.Name == "")
                        {
                            //-- Assuming Empty for Directory
                            Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                            continue;
                        }

                        MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Extraindo arquivo (" + filesCount + "/" + zipStream.Entries.Count.ToString() + ")";
                        MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.ProgressoBarra1 = ((double)filesCount / zipStream.Entries.Count) * 100;
                        file.ExtractToFile(completeFileName, true);
                        filesCount++;                        
                    }
                }

                MainWindowViewModel.Indicadores.AtualizouExe = true;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Problemas ao atualizar programas.";
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

                Rmenu = "";

                ListaSql.ForEach(row =>
                {
                    Rmenu += Encoding.ASCII.GetString(Ftp.Download((PathSql + row.Arquivo), MainWindowViewModel));
                });

                MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Executando Rmenu de v." + AtualizarModel.Versao.ToString() + " até v." + UltimaVersao.ToString() + "";

                context.Query(Rmenu).ExecuteNonQuery();

                MainWindowViewModel.Indicadores.AtualizouBanco = true;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Problemas ao rodar o Rmenu.";
            }

            if (UltimaVersao <= 0)
            {
                UltimaVersao = AtualizarModel.Versao.GetValueOrDefault();
            }
        }

        /// <summary>
        /// Atualiza data da atualização no banco de dados com a data atual
        /// </summary>
        private void AtualizarBanco(PostoContext context)
        {
            try
            {
                MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Atualizando versão do banco de dados...";
                context.Query("UPDATE atualiz SET atuDatAtua = '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "'").ExecuteNonQuery();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Problemas ao atualizar versão.";
            }
        }

        /// <summary>
        /// Ativa manutenção no banco de dados
        /// </summary>
        private void ManutencaoAtiva()
        {
            try
            {
                MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Preparando para a atualização...";
                MainWindowViewModel.Indicadores.EmManutencao = true;
                MainWindowViewModel.Indicadores.FimManutencao = false;
                MainWindowViewModel.Indicadores.AtualizouBanco = false;
                MainWindowViewModel.Indicadores.AtualizouExe = false;
                MainWindowViewModel.Indicadores.DerrubaConexoes();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Problemas ao atualizar dados.";
            }

        }

        /// <summary>
        /// Desativa manutenção no banco de dados
        /// </summary>
        private void ManutencaoDesativa()
        {
            try
            {
                MainWindowViewModel.Indicadores.EmManutencao = false;
                MainWindowViewModel.Indicadores.FimManutencao = true;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                MainWindowViewModel.AbaAtualizar.Status.StatusLabel.LabelContent = "Problemas ao finalizar manutenção.";
            }
        }

        /// <summary>
        /// Reinicializa os programas de acordo com os parâmetros
        /// </summary>
        private void ReinicializaProgramas(bool teveErro, PostoContext context)
        {
            context.Close();
            MainWindowViewModel.AbaAtualizar.Status.BarraProgresso.ProgressoBarra2 = 0;
            
            //-- Se diretório existir, inicia processo
            if (File.Exists(PathAtualizador))
            {
                //-- Inicia processo do leitor de bombas caso parâmetro seja true
                if (ConfiguracaoModel.LeitorBomba)
                {
                    Process.Start(PathAtualizador, "6");
                }
                //-- Se diretório existir, inicia processo
                if (ConfiguracaoModel.PostoWeb)
                {
                    Process.Start(PathAtualizador, "7");
                }
            }
            
            if (teveErro)
            {
                MessageBox.Show("Houve um erro ao atualizar a versão do sistema. Por favor, entre em contato com o suporte.", "Erro na atualização", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            SetaBarraStatus(System.Windows.Visibility.Hidden, 25);
        }

        /// <summary>
        /// Busca as versões para atualização de acordo com a versão do cliente
        /// </summary>
        private void BuscaVersoes()
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