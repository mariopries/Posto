using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Windows.Forms;


namespace Posto
{
    public class Inicializador
    {
        private string fLocal;
        private string fServidor;
        private List<FileInfo> fArquivos;
        private List<FileInfo> fArquivosNovos;
        private BackgroundWorker fBgWInicializador;

        /// <summary>
        /// Inicia o processo de verificação da versão dos arquivos
        /// </summary>
        /// <param name="worker">BackgroundWorker que mostrará o status na tela</param>
        public void Start(BackgroundWorker worker, Aplicativo executavel)
        {
            try
            {
                // Backgound Worker da tela principal
                fBgWInicializador = worker;

                // Carrega as configurações
                LoadConfiguracoes();

                // Verifica quais arquivos devem ser copiados do servidor
                VerificaVersaoDosArquivos();

                // Faz a cópia somente dos arquivos modificados/novos
                AtualizaArquivos();

                // Executa o Posto
                Executar(executavel);
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Executa o Posto
        /// </summary>
        private void Executar(Aplicativo executavel)
        {
            var processo = new Process();

            try
            {
                switch (executavel) 
                {
                    case Aplicativo.Retaguarda:
                        processo.StartInfo.FileName = "uGsw002.exe";
                        break;

                    case Aplicativo.PreVenda:
                        processo.StartInfo.FileName = "aPreVend.exe";
                        break;

                    case Aplicativo.FrenteCaixa:
                        processo.StartInfo.FileName = "uSGM.exe";
                        break;

                    case Aplicativo.Estoque:
                        processo.StartInfo.FileName = "uMenuEst.exe";
                        break;

                    case Aplicativo.TrocaOleo:
                        processo.StartInfo.FileName = "uTroOleo.exe";
                        break;
                }
                processo.StartInfo.WorkingDirectory = fLocal;
                processo.StartInfo.Arguments = "1";
                processo.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                processo.Start();
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                throw new Exception(String.Format("Não foi possível executar o Posto.exe: \n{0}", e.Message));
            }
            finally
            {
                processo.Close();
                processo.Dispose();
                processo = null;
            }
        }

        /// <summary>
        /// Carrega as configurações
        /// </summary>
        private void LoadConfiguracoes()
        {
            try
            {
                UpdateProgress("Carregando configurações do sistema.");

                // Endereço local
                fLocal = ConfigurationManager.AppSettings["Local"];

                // Endereço do servidor
                fServidor = ConfigurationManager.AppSettings["Servidor"];

                // Copia o arquivo posto_ini.xml para a máquina local
                var local = Path.Combine(fLocal, "posto_ini.xml");
                var server = Path.Combine(fServidor, "posto_ini.xml");
                File.Copy(server, local, true);
                XElement config = XElement.Load(local);

                #region Arquivos

                fArquivos = new List<FileInfo>();

                // Arquivos
                if (((string)config.XPathSelectElement(@"//inicializador/arquivos")) != "")
                {
                    List<string> arquivos = new List<string>();
                    arquivos.AddRange(((string)config.XPathSelectElement(@"//inicializador/arquivos")).Split(','));

                    foreach (var arquivo in arquivos)
                    {
                        fArquivos.Add(new FileInfo(Path.Combine(fServidor, arquivo)));
                    }
                }

                #endregion

                #region Extensões

                // Extensões
                if (((string)config.XPathSelectElement(@"//inicializador/extensoes")) != "")
                {
                    List<string> extensoes = new List<string>();
                    extensoes.AddRange(((string)config.XPathSelectElement(@"//inicializador/extensoes")).Split(','));

                    foreach (var extensao in extensoes)
                    {
                        try
                        {
                            DirectoryInfo directory = new DirectoryInfo(fServidor);
                            FileInfo[] files = directory.GetFiles(extensao, SearchOption.TopDirectoryOnly);
                            fArquivos.AddRange(FiltarArquivos(files));
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }

                #endregion

                #region Subpastas

                // SubPastas
                if (((string)config.XPathSelectElement(@"//inicializador/subpastas")) != "")
                {

                    List<string> subPastas = new List<string>();
                    subPastas.AddRange(((string)config.XPathSelectElement(@"//inicializador/subpastas")).Split(','));

                    foreach (var pasta in subPastas)
                    {
                        try
                        {
                            DirectoryInfo directory = new DirectoryInfo(Path.Combine(fServidor, pasta));
                            FileInfo[] files = directory.GetFiles("*.*", SearchOption.AllDirectories);
                            fArquivos.AddRange(FiltarArquivos(files));
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }

                #endregion

            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                throw new Exception(String.Format("Não foi possível carregar as configurações, por favor verifique:\n{0}", e.Message));
            }
        }

        /// <summary>
        /// Filtar ums lista de arquivos
        /// </summary>
        /// <param name="files">Lista de arquivos</param>
        /// <returns>Lista de arquivos que podem ser copiados</returns>
        private static IEnumerable<FileInfo> FiltarArquivos(FileInfo[] files)
        {
            var visibleFiles = from f in files
                               where (f.Attributes & FileAttributes.Hidden) == 0 &&
                                     (f.Attributes & FileAttributes.Temporary) == 0 &&
                                     (f.Attributes & FileAttributes.System) == 0 &&
                                     (f.Attributes & FileAttributes.ReadOnly) == 0
                               select f;
            return visibleFiles;
        }

        /// <summary>
        /// Verifica a versão dos arquivos numa determinada pasta
        /// </summary>
        private void VerificaVersaoDosArquivos()
        {
            try
            {
                UpdateProgress("Verificando novos arquivos no servidor.");

                fArquivosNovos = new List<FileInfo>();

                foreach (var arquivo in fArquivos)
                {
                    if (arquivo.Exists)
                    {
                        var local = new FileInfo(arquivo.FullName.Replace(fServidor, fLocal));
                        var servidor = arquivo;

                        if (local.LastWriteTimeUtc != servidor.LastWriteTimeUtc || local.Length != servidor.Length)
                        {
                            fArquivosNovos.Add(arquivo);
                        }
                    }
                    else
                    {
                        fArquivosNovos.Add(arquivo);
                    }
                }
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                throw new Exception(String.Format("Não foi possível verificar a versão dos arquivos: \n{0}", e.Message));
            }
        }

        /// <summary>
        /// Faz a cópia dos arquivos do servidor
        /// </summary>
        private void AtualizaArquivos()
        {
            try
            {
                UpdateProgress("Atualizando o Posto, aguarde...");

                foreach (var arquivo in fArquivosNovos)
                {
                    var local = arquivo.FullName.Replace(fServidor, fLocal);
                    var diretorio = Path.GetDirectoryName(local);
                    if (!Directory.Exists(diretorio))
                    {
                        Directory.CreateDirectory(diretorio);
                    }

                    File.Copy(arquivo.FullName, local, true);
                }
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                throw new Exception(String.Format("Não foi possível atualizar os arquivos: \n{0}", e.Message));
            }
        }

        /// <summary>
        /// Atualiza o status doque está sendo feito
        /// </summary>
        /// <param name="mensagem">Mensagem a ser mostrada na tela</param>
        private void UpdateProgress(string mensagem)
        {
            try
            {
                // Mostra a mensagem
                fBgWInicializador.ReportProgress(0, mensagem);
            }
            catch
            {
            }
        }


        public int args { get; set; }
    }
}
