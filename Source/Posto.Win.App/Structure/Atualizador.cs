using log4net;
using Posto.Win.App.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Posto.Win.App.Structure
{
    class Atualizador
    {
        #region Gerenciador de log

        private static readonly ILog Logs = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Propriedades

        private string _argumento;
        private List<FileInfo> _arquivos;
        private List<FileInfo> _arquivosNovos;
        private Configuracoes _configuracoes;
        private Process _processo;

        #endregion

        #region Construtor

        public Atualizador(string argumento)
        {
            Console.WriteLine("Carregando configurações do sistema.");
            try
            {
                Configuracoes = ConfiguracoesXml.CarregarConfiguracao().ToModel();
                if (Configuracoes.Local == null) { Configuracoes.Local = @"C:\Metodos\"; }
                if (Configuracoes.Servidor == null) { Configuracoes.Servidor = @"C:\Metodos\"; }
                Configuracoes.ToModel().GravarConfiguracao();
                Argumento = argumento;
                Arquivos = new List<FileInfo>();
                ArquivosNovos = new List<FileInfo>();
                Processo = new Process();
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                MessageBox.Show("Não foi possível iniciar a atualização", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logs.Error(e.Message);
                Environment.Exit(0);
            }
            finally
            {
                Start();
            }
        }

        #endregion

        #region Objetos
        
        private string Argumento
        {
            get { return _argumento; }
            set { if (_argumento != value) { _argumento = value; } }
        }
        private Configuracoes Configuracoes
        {
            get { return _configuracoes; }
            set { if (_configuracoes != value) { _configuracoes = value; } }
        }
        private List<FileInfo> Arquivos
        {
            get { return _arquivos; }
            set { if (_arquivos != value) { _arquivos = value; } }
        }
        private List<FileInfo> ArquivosNovos
        {
            get { return _arquivosNovos; }
            set { if (_arquivosNovos != value) { _arquivosNovos = value; } }
        }
        private Process Processo
        {
            get { return _processo; }
            set { if (_processo != value) { _processo = value; } }
        }

        #endregion

        #region Funções

        /// <summary>
        /// Inicializa o procedimento de atualização do aplicativo
        /// </summary>
        public void Start()
        {
            FinalizaAplicativo();
            CarregaArquivosServidor();
            VerificaVersaoDosArquivos();
            AtualizaArquivos();
            Executar();
        }
        /// <summary>
        /// Finaliza o aplicativo se estiver em execução
        /// </summary>
        public void FinalizaAplicativo()
        {
            try
            {
                Process[]
                processes = Process.GetProcessesByName("Posto");
                foreach (Process process in processes)
                {
                    process.Kill();
                }
            }
            catch(Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                MessageBox.Show("Não foi possível encerrar Posto.exe", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logs.Error(e.Message);
                Environment.Exit(0);
            }
        }
        /// <summary>
        /// Busca arquivos no servidor a sere copiados
        /// </summary>
        public void CarregaArquivosServidor()
        {
            try
            {
                //-- Adiciona todos os arquivos na lista na pasta templater
                foreach (string newPath in Directory.GetFiles(Configuracoes.Servidor + @"\App", "*.*", SearchOption.AllDirectories))
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
                MessageBox.Show("Não foi possível carregar arquivos do servidor", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logs.Error(e.Message);
                Environment.Exit(0);
            }
        }
        /// <summary>
        /// Testa as versões de todos os arquivos da pasta App
        /// </summary>
        private void VerificaVersaoDosArquivos()
        {
            try
            {
                Console.WriteLine("Verificando novos arquivos no servidor.");                

                foreach (var arquivo in Arquivos)
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
                }
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                MessageBox.Show("Não foi possível verificar a versão dos arquivos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logs.Error(e.Message);
                Environment.Exit(0);
            }
        }
        /// <summary>
        /// Atualiza os arquivos do terminal
        /// </summary>
        private void AtualizaArquivos()
        {
            try
            {
                Console.WriteLine("Atualizando o Posto, aguarde...");

                foreach (var arquivo in ArquivosNovos)
                {
                    var local = arquivo.FullName.Replace(Configuracoes.Servidor, Configuracoes.Local);
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
                MessageBox.Show("Não foi possível atualizar os arquivos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logs.Error(e.Message);
                Environment.Exit(0);
            }
        }
        /// <summary>
        /// Executa o aplicativo com o parâmetro recebido
        /// </summary>
        private void Executar()
        {       
            try
            {
                Processo.StartInfo.FileName = "Posto.exe";
                Processo.StartInfo.WorkingDirectory = Configuracoes.Local + "App\\";
                Processo.StartInfo.Arguments = Argumento;
                Processo.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                Processo.Start();
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                MessageBox.Show("Não foi possível executar o Posto.exe", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logs.Error(string.Format(": \n{0}", e.Message));
            }
            finally
            {
                Processo.Close();
                Processo.Dispose();
                Processo = null;
            }
        }
                
        #endregion
    }
}
