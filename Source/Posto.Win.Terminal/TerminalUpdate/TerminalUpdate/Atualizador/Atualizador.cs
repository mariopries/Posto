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

namespace TerminalUpdate
{
    class Atualizador
    {
        private string _local;
        private string _servidor;
        private string _argumento;
        private List<FileInfo> _arquivos;
        private List<FileInfo> _arquivosNovos;

        public Atualizador(string argumento)
        {
            Console.Write("Carregando configurações do sistema.\n");
            try
            {
                Local = ConfigurationManager.AppSettings["Local"];
                Server = ConfigurationManager.AppSettings["Servidor"];
                Argumento = argumento;
                Arquivos = new List<FileInfo>();
                ArquivosNovos = new List<FileInfo>();
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                Console.Write(string.Format("Não foi possível iniciar a atualização porque: \n{0}", e.Message));
            }
            finally
            {
                Start();
            }
        }


        private string Argumento
        {
            get { return _argumento; }
            set { if (_argumento != value) { _argumento = value; } }
        }
        private string Local
        {
            get { return _local; }
            set { if (_local != value) { _local = value; } }
        }
        private string Server
        {
            get { return _servidor; }
            set { if (_servidor != value) { _servidor = value; } }
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
        
        public void Start()
        {
            CarregaArquivosServidor();
            VerificaVersaoDosArquivos();
            AtualizaArquivos();
            Executar();
        }

        public void CarregaArquivosServidor()
        {
            try
            {
                //-- Adiciona todos os arquivos na lista na pasta templater
                foreach (string newPath in Directory.GetFiles(Server + @"\App", "*.*", SearchOption.AllDirectories))
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
                Console.WriteLine(string.Format("Não foi possível adicionar os arquivos: \n{0}", e.Message));
            }
        }
        private void VerificaVersaoDosArquivos()
        {
            try
            {
                Console.WriteLine("Verificando novos arquivos no servidor.\n");                

                foreach (var arquivo in Arquivos)
                {
                    if (arquivo.Exists)
                    {
                        var local = new FileInfo(arquivo.FullName.Replace(Server, Local));
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
                Console.WriteLine(string.Format("Não foi possível verificar a versão dos arquivos: \n{0}", e.Message));
            }
        }
        private void AtualizaArquivos()
        {
            try
            {
                Console.Write("Atualizando o Posto, aguarde...");

                foreach (var arquivo in ArquivosNovos)
                {
                    var local = arquivo.FullName.Replace(Server, Local);
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
        private void Executar()
        {
            var processo = new Process();

            try
            {
                //Executa os .EXE corretos
                processo.StartInfo.FileName = "Posto.exe";
                processo.StartInfo.WorkingDirectory = Local + "App\\";
                processo.StartInfo.Arguments = Argumento;
                processo.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                processo.Start();
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                Console.WriteLine(string.Format("Não foi possível executar o Posto.exe: \n{0}", e.Message));
            }
            finally
            {
                processo.Close();
                processo.Dispose();
                processo = null;
            }
        }
    }
}
