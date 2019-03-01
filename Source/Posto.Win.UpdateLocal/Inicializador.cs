using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Windows.Forms;
using System.Reflection;
using Posto.Win.UpdateLocal.Estrutura;
using Posto.Win.UpdateLocal.Extensions;

namespace Posto.Win.UpdateLocal
{
    public class Inicializador
    {
        #region Propriedades

        private Configuracoes _configuracoes;
        private List<FileInfo> _arquivos;
        private List<FileInfo> _arquivosNovos;
        private BackgroundWorker _bgWInicializador;

        #endregion

        #region Construtor

        public Inicializador()
        {
            Configuracoes = ConfiguracoesXml.CarregarConfiguracao().ToModel();
            Arquivos = new List<FileInfo>();
            ArquivosNovos = new List<FileInfo>();
        }

        #endregion

        #region Objetos

        public Configuracoes Configuracoes
        {
            get { return _configuracoes; }
            set { if (_configuracoes != value) { _configuracoes = value; } }
        }
        public List<FileInfo> Arquivos
        {
            get { return _arquivos; }
            set { if (_arquivos != value) { _arquivos = value; } }
        }
        public List<FileInfo> ArquivosNovos
        {
            get { return _arquivosNovos; }
            set { if (_arquivosNovos != value) { _arquivosNovos = value; } }
        }
        public BackgroundWorker BackgroundWorker
        {
            get { return _bgWInicializador; }
            set { if (_bgWInicializador != value) { _bgWInicializador = value; } }
        }

        #endregion

        /// <summary>
        /// Inicia o processo de verificação da versão dos arquivos
        /// </summary>
        /// <param name="worker">BackgroundWorker que mostrará o status na tela</param>
        public void Start(BackgroundWorker worker, Aplicativo executavel)
        {
            try
            {
                //-- Backgound Worker da tela principal
                //BackgroundWorker = worker;

                //-- Verifica se existe configurações no xml
                if (VerificaConfiguracao())
                {
                    //-- Testa a versão do aplicativo 
                    TestaVersaoPrograma(executavel);

                    //-- Carrega as arquivos a serem buscados
                    LoadConfiguracoes(executavel);

                    //-- Verifica quais arquivos devem ser copiados do servidor
                    VerificaVersaoDosArquivos();

                    //-- Faz a cópia somente dos arquivos modificados/novos
                    AtualizaArquivos();

                    //-- Executa o Posto
                    Executar(executavel);
                }
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
        private bool VerificaConfiguracao()
        {
            try
            {
                if ((Configuracoes.Local == null || Configuracoes.Local == "") || (Configuracoes.Servidor == null || Configuracoes.Servidor == ""))
                {
                    if (Configuracoes.Local == null || Configuracoes.Local == "")
                    {
                        Configuracoes.Local = @"C:\metodos\";
                    }
                    if (Configuracoes.Servidor == null || Configuracoes.Servidor == "")
                    {
                        Configuracoes.Servidor = @"\\servidor\metodos\Update\";
                    }

                    Configuracoes.ToModel().GravarConfiguracao();
                    MessageBox.Show("Foi gerado um novo arquivo de configurações e carregado as configurações padrões.", "Configurações", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    Environment.Exit(0);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                throw new Exception(string.Format("Não foi possível iniciar verificar as configurações: \n{0}", e.Message));
            }
        }
        /// <summary>
        /// Testa a versão do exe, se tiver diferença roda o programa de atualizar
        /// </summary>
        /// <param name="executavel"></param>
        private void TestaVersaoPrograma(Aplicativo executavel)
        {
            try
            {
                UpdateProgress("Verificando a versão do programa.");

                Arquivos = new List<FileInfo>();
                ArquivosNovos = new List<FileInfo>();

                Arquivos.Add(new FileInfo(Configuracoes.Servidor + @"App\Posto.exe"));

                Arquivos.ForEach(arquivo =>
                {
                    var local = new FileInfo(arquivo.FullName.Replace(Configuracoes.Servidor, Configuracoes.Local));
                    var servidor = arquivo;

                    if (servidor.Exists)
                    {
                        if (local.LastWriteTimeUtc != servidor.LastWriteTimeUtc || local.Length != servidor.Length)
                        {
                            ExecutaAtualizador((int)executavel);
                            Environment.Exit(0);
                        }
                    }
                });
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                throw new Exception(string.Format("Não foi possível verificar a versão dos arquivos: \n{0}", e.Message));
            }
        }
        private void ExecutaAtualizador(int executavel)
        {
            var processo = new Process();

            try
            {
                processo.StartInfo.FileName = "uAppUpdate.exe";
                processo.StartInfo.WorkingDirectory = Configuracoes.Local;
                processo.StartInfo.Arguments = Convert.ToString(executavel);
                processo.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                processo.Start();
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                throw new Exception(string.Format("Não foi possível iniciar atualização do Posto.exe: \n{0}", e.Message));
            }
            finally
            {
                processo.Close();
                processo.Dispose();
                processo = null;
            }
        }
        /// <summary>
        /// Executa aplicativo de acordo com parâmetro
        /// </summary>
        private void Executar(Aplicativo executavel)
        {
            var processo = new Process();

            try
            {
                //Executa os .EXE corretos
                processo.StartInfo.FileName = GetEnumCategory(executavel);

                processo.StartInfo.WorkingDirectory = Configuracoes.Local;
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
                throw new Exception(string.Format("Não foi possível executar o Posto.exe: \n{0}", e.Message));
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
        private void LoadConfiguracoes(Aplicativo aplicativo)
        {
            try
            {
                UpdateProgress("Carregando configurações do sistema.");

                //-- Adiciona Executávl solicitado pelo usuário.
                Arquivos.Add(new FileInfo(Configuracoes.Servidor + GetEnumCategory(aplicativo)));

                //-- Adiciona DLLs
                foreach (string newPath in Directory.GetFiles(Configuracoes.Servidor, "*.dll", SearchOption.AllDirectories))
                {
                    Arquivos.Add(new FileInfo(newPath));
                }

                //-- Adiciona todos os arquivos na lista na pasta templater
                foreach (string newPath in Directory.GetFiles(Configuracoes.Servidor + @"\Template", "*.*", SearchOption.AllDirectories))
                {
                    Arquivos.Add(new FileInfo(newPath));
                }

                //-- Adiciona .jpg
                foreach (string newPath in Directory.GetFiles(Configuracoes.Servidor, "*.jpg", SearchOption.AllDirectories))
                {
                    Arquivos.Add(new FileInfo(newPath));
                }

                //-- Adiciona .xml
                foreach (string newPath in Directory.GetFiles(Configuracoes.Servidor, "*.xml", SearchOption.AllDirectories))
                {
                    Arquivos.Add(new FileInfo(newPath));
                }

                //-- Adiciona .config
                foreach (string newPath in Directory.GetFiles(Configuracoes.Servidor, "*.config", SearchOption.AllDirectories))
                {
                    Arquivos.Add(new FileInfo(newPath));
                }
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                throw new Exception(string.Format("Não foi possível adicionar os arquivos: \n{0}", e.Message));
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

                ArquivosNovos = new List<FileInfo>();

                Arquivos.ForEach(arquivo =>
                {
                    if (arquivo.Exists)
                    {
                        var local = new FileInfo(arquivo.FullName.Replace(Configuracoes.Servidor, Configuracoes.Local));
                        var servidor = arquivo;

                        if (local.LastWriteTimeUtc != servidor.LastWriteTimeUtc || local.Length != servidor.Length)
                        {
                            ArquivosNovos.Add(arquivo);
                        }
                    }
                    else
                    {
                        ArquivosNovos.Add(arquivo);
                    }
                });
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                throw new Exception(string.Format("Não foi possível verificar a versão dos arquivos: \n{0}", e.Message));
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

                ArquivosNovos.ForEach(arquivo =>
                {
                    var local = arquivo.FullName.Replace(Configuracoes.Servidor, Configuracoes.Local);
                    var diretorio = Path.GetDirectoryName(local);

                    if (!Directory.Exists(diretorio))
                    {
                        Directory.CreateDirectory(diretorio);
                    }

                    File.Copy(arquivo.FullName, local, true);
                });
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                throw new Exception(string.Format("Não foi possível atualizar os arquivos: \n{0}", e.Message));
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
                _bgWInicializador.ReportProgress(0, mensagem);
            }
            catch
            {
            }
        }

        public string GetEnumCategory<TEnum>(TEnum value)
        {
            if (value != null)
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());
                CategoryAttribute[] attributes =
                    (CategoryAttribute[])fi.GetCustomAttributes(typeof(CategoryAttribute), false);

                if ((attributes != null) && (attributes.Length > 0))
                {
                    return attributes[0].Category;
                }
            }
            return value.ToString();
        }
    }
}
