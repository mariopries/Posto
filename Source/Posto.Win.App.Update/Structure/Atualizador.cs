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

        #region Variaveis

        private bool _finalizouProcesso;
        private DateTime Tempo;

        #endregion

        #region Construtor

        public Atualizador(string argumento)
        {
            Console.WriteLine("Carregando configurações do sistema.");
            try
            {
                Configuracoes = ConfiguracoesXml.CarregarConfiguracao().ToModel();
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
                Task.Run(() => Start()).Wait();
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
        private bool FinalizouProcesso
        {
            get { return _finalizouProcesso; }
            set { if (_finalizouProcesso != value) { _finalizouProcesso = value; } }
        }

        #endregion

        #region Funções

        /// <summary>
        /// Inicializa o procedimento de atualização do aplicativo
        /// </summary>
        public async Task Start()
        {
            await Task.Run(async () =>
            {
                try
                {
                    if (!await VerificaConfiguracao())
                    {
                        MessageBox.Show("Não foi possível testar as configurações do programa.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }

                    if (!await FinalizaAplicativo())
                    {
                        MessageBox.Show("Não foi possível encerrar Posto.exe.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }

                    if (!await CarregaArquivosServidor())
                    {
                        MessageBox.Show("Não foi possível encontrar o caminho dos arquivos do servidor.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }

                    if (!await VerificaVersaoDosArquivos())
                    {
                        MessageBox.Show("Não foi possível verificar a versão dos arquivos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }
                    if (!await AtualizaArquivos())
                    {
                        MessageBox.Show("Não foi possível atualizar os arquivos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }
                    if (!await Executar())
                    {
                        MessageBox.Show("Não foi possível executar o Posto.exe.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(0);

                    }

                    Processo.Close();
                    Processo.Dispose();
                    Processo = null;
                }

                catch (Exception e)
                {
                    Logs.Error(e.Message);
                }
            });
        }

        /// <summary>
        /// Verifica se existe as configurações no arquivo xml, caso não exista, gera as configurações padrões e informa o usuário
        /// </summary>
        private async Task<bool> VerificaConfiguracao()
        {
            return await Task.Run(() =>
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
                            Configuracoes.Servidor = @"\\servidor\metodos";
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
                    Logs.Error(e.Message);
                    return false;
                }
            });
        }

        /// <summary>
        /// Finaliza o aplicativo se estiver em execução
        /// </summary>
        public async Task<bool> FinalizaAplicativo()
        {
            return await Task.Run(() =>
            {
                var retorno = false;
                try
                {
                    var processofim = true;

                    while (processofim)
                    {
                        var existe = false;

                        Process[]
                        processes = Process.GetProcessesByName("Posto");

                        if (DateTime.Now >= Tempo.AddSeconds(2))
                        {
                            Console.WriteLine("Esperando Posto.exe fechar");
                            Tempo = DateTime.Now;
                        }

                        foreach (Process process in processes)
                        {
                            existe = true;
                        }

                        if (!existe)
                        {
                            processofim = false;
                            retorno = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logs.Error(e.Message);
                }
                return retorno;
            });
        }

        /// <summary>
        /// Busca arquivos no servidor a sere copiados
        /// </summary>
        public async Task<bool> CarregaArquivosServidor()
        {
            return await Task.Run(() =>
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
                    Logs.Error(e.Message);
                    return false;
                }
                return true;
            });
        }

        /// <summary>
        /// Testa as versões de todos os arquivos da pasta App
        /// </summary>
        private async Task<bool> VerificaVersaoDosArquivos()
        {
            return await Task.Run(() =>
            {
                try
                {
                    Console.WriteLine("Verificando novos arquivos no servidor.");
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
                    Logs.Error(e.Message);
                    return false;
                }
                return true;
            });
        }

        /// <summary>
        /// Atualiza os arquivos do terminal
        /// </summary>
        private async Task<bool> AtualizaArquivos()
        {
            return await Task.Run(() =>
            {
                try
                {
                    Console.WriteLine("Atualizando o Posto, aguarde...");

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
                    Logs.Error(e.Message);
                    return false;
                }
                return true;
            });
        }

        /// <summary>
        /// Executa o aplicativo com o parâmetro recebido
        /// </summary>
        private async Task<bool> Executar()
        {
            return await Task.Run(() =>
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
                    Logs.Error(e.Message);
                    return false;
                }
                return true;
            });
        }

        #endregion
    }
}
