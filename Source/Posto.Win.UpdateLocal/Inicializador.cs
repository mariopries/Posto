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
using System.Reflection;


namespace Posto.Win.UpdateLocal
{
    public class Inicializador
    {
        private string _local;
        private string _servidor;
        private List<FileInfo> _arquivos;
        private List<FileInfo> _arquivosNovos;
        private BackgroundWorker _bgWInicializador;

        /// <summary>
        /// Inicia o processo de verificação da versão dos arquivos
        /// </summary>
        /// <param name="worker">BackgroundWorker que mostrará o status na tela</param>
        public void Start(BackgroundWorker worker, Aplicativo executavel)
        {
            try
            {
                _arquivos = new List<FileInfo>();
                _arquivosNovos = new List<FileInfo>();

                //-- Backgound Worker da tela principal
                _bgWInicializador = worker;

                //-- Carrega as configurações
                LoadConfiguracoes(executavel);

                //-- Verifica quais arquivos devem ser copiados do servidor
                VerificaVersaoDosArquivos();

                //-- Faz a cópia somente dos arquivos modificados/novos
                AtualizaArquivos();

                //-- Executa o Posto
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
                //Executa os .EXE corretos
                processo.StartInfo.FileName = GetEnumCategory(executavel);

                processo.StartInfo.WorkingDirectory = _local;
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

                //-- Endereço local
                _local = ConfigurationManager.AppSettings["Local"];

                //-- Endereço do servidor
                _servidor = ConfigurationManager.AppSettings["Servidor"];

                //-- Adiciona Executávl solicitado pelo usuário.
                _arquivos.Add(new FileInfo(_servidor + GetEnumCategory(aplicativo)));

                //-- Adiciona DLLs
                foreach (string newPath in Directory.GetFiles(_servidor, "*.dll", SearchOption.AllDirectories))
                {
                    _arquivos.Add(new FileInfo(newPath));
                }

                //-- Adiciona todos os arquivos na lista na pasta templater
                foreach (string newPath in Directory.GetFiles(_servidor + @"\Template", "*.*", SearchOption.AllDirectories))
                {
                    _arquivos.Add(new FileInfo(newPath));
                }
                //-- Adiciona .jpg
                foreach (string newPath in Directory.GetFiles(_servidor, "*.jpg", SearchOption.AllDirectories))
                {
                    _arquivos.Add(new FileInfo(newPath));
                }

                //-- Adiciona .xml
                foreach (string newPath in Directory.GetFiles(_servidor, "*.xml", SearchOption.AllDirectories))
                {
                    _arquivos.Add(new FileInfo(newPath));
                }

                //-- Adiciona .config
                foreach (string newPath in Directory.GetFiles(_servidor, "*.config", SearchOption.AllDirectories))
                {
                    _arquivos.Add(new FileInfo(newPath));
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

                _arquivosNovos = new List<FileInfo>();

                foreach (var arquivo in _arquivos)
                {
                    if (arquivo.Exists)
                    {
                        var local = new FileInfo(arquivo.FullName.Replace(_servidor, _local));
                        var servidor = arquivo;

                        if (local.LastWriteTimeUtc != servidor.LastWriteTimeUtc || local.Length != servidor.Length)
                        {
                            _arquivosNovos.Add(arquivo);
                        }
                    }
                    else
                    {
                        _arquivosNovos.Add(arquivo);
                    }
                }
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

                foreach (var arquivo in _arquivosNovos)
                {
                    var local = arquivo.FullName.Replace(_servidor, _local);
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
