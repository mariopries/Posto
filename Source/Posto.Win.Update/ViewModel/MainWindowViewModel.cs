using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Posto.Win.Update.Infraestrutura;
using Posto.Win.Update.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Posto.Win.Update.Extensions;
using Posto.Win.Update.DataContext;

namespace Posto.Win.Update.ViewModel
{
    public class MainWindowViewModel : NotificationObject
    {

        /// <summary>
        /// Controles que podem receber focus
        /// </summary>
        private enum Focus
        {
            InputServidor,
            InputPorta,
            InputBanco,
            InputUsuario,
            InputSenha,
            InputLocalDiretorio
        }

        private ConfiguracaoModel _configuracao;
        private AtualizarModel _atualizar;
        private Atualizar _atualizarAsync;
        private string _focusElement;
        private bool _enableButtonConfiguracao;
        private bool _isVisibleButtonPausar;
        private bool _isEnableButtonAtualizar;

        private bool _dynamicContentControlIsActive;
        private NotificationObject _dynamicContentControl;
        
        public MainWindowViewModel() 
        {
            this.OnLoad();
        }

        private async void OnLoad() 
        {
            this.Atualizar              = new AtualizarModel();
            this._atualizarAsync        = new Atualizar(this.Atualizar);
            this.Configuracao           = ConfiguracaoXml.CarregarConfiguracao().ToModel();
            this.AbrirExplorerCommand   = new DelegateCommand(OnAbrirExplorer);
            this.SalvarCommand          = new DelegateCommand(OnSalvar);
            this.TestarConexaoCommand   = new DelegateCommand(OnTestarConexao);
            this.AtualizarCommand       = new DelegateCommand(OnAtualizar);
            this.AtualizarAfterCommand  = new DelegateCommand(OnAtualizarAfter);
            this.IniciarCommand         = new DelegateCommand(OnIniciar);
            this.PausarCommand          = new DelegateCommand(OnPausar);
            this.LoginCommand           = new DelegateCommand(OnLogin);
            this.BloquearCommand        = new DelegateCommand(OnBloquear);

            this.EnableButtonConfiguracao = true;
            this.IsVisibleButtonPausar    = false;
            this.IsEnableButtonAtualizar  = true;

            var conexao = await this.OnTestarConexaoAsync();

            if (conexao) 
            {
                this.OnIniciar();
            }

            //this.DynamicContentControl = new LoginViewModel(this.LoginCommand);
        }

        #region Campos

        public ConfiguracaoModel Configuracao
        {
            get
            {
                return this._configuracao;
            }
            set
            {
                if (this._configuracao != value)
                {
                    this._configuracao = value;
                    this.RaisePropertyChanged(() => this.Configuracao);
                }
            }
        }

        public AtualizarModel Atualizar
        {
            get
            {
                return this._atualizar;
            }
            set
            {
                if (this._atualizar != value)
                {
                    this._atualizar = value;
                    this.RaisePropertyChanged(() => this.Atualizar);
                }
            }
        }

        ///// <summary>
        ///// Elemento que receberá o focus.
        ///// </summary>
        public string FocusElement
        {
            get
            {
                return this._focusElement;
            }
            set
            {
                this._focusElement = value;
                this.RaisePropertyChanged(() => this.FocusElement);
            }
        }

        public bool EnableButtonConfiguracao // here, underscore typo
        {
            get
            {
                return this._enableButtonConfiguracao;
            }
            set
            {
                if (this._enableButtonConfiguracao != value)
                {
                    this._enableButtonConfiguracao = value;
                    this.RaisePropertyChanged(() => this.EnableButtonConfiguracao);
                }
            }
        }

        public bool IsVisibleButtonPausar
        {
            get
            {
                return this._isVisibleButtonPausar;
            }
            set
            {
                if (this._isVisibleButtonPausar != value)
                {
                    this._isVisibleButtonPausar = value;
                    this.RaisePropertyChanged(() => this.IsVisibleButtonPausar);
                }
            }
        }

        public bool IsEnableButtonAtualizar
        {
            get
            {
                return this._isEnableButtonAtualizar;
            }
            set
            {
                if (this._isEnableButtonAtualizar != value)
                {
                    this._isEnableButtonAtualizar = value;
                    this.RaisePropertyChanged(() => this.IsEnableButtonAtualizar);
                }
            }
        }

        #endregion

        #region Commands

        public DelegateCommand AbrirExplorerCommand { get; set; }
        public DelegateCommand SalvarCommand { get; set; }
        public DelegateCommand TestarConexaoCommand { get; set; }
        public DelegateCommand AtualizarCommand { get; set; }
        public DelegateCommand AtualizarAfterCommand { get; set; }
        public DelegateCommand IniciarCommand { get; set; }
        public DelegateCommand PausarCommand { get; set; }
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand BloquearCommand { get; set; }

        #endregion

        #region Helpers

        private void OnAbrirExplorer()
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    this.Configuracao.LocalDiretorio = folderDialog.SelectedPath;
                }
            }
        }

        private async void OnSalvar()
        {
            await Task.Run(() =>
            {
                if (this.IsValidarConfiguracao(this.Configuracao))
                {
                    this.EnableButtonConfiguracao = false;
                    this.Configuracao.Mensagem = "Salvando configuração...";

                    this.Configuracao.ToModel().GravarConfiguracao();
                    this.Configuracao.Mensagem = "Configuração Salva.";
                    this.EnableButtonConfiguracao = true;
                }
            });
        }

        private async void OnTestarConexao()
        {
            await OnTestarConexaoAsync();
        }

        private async Task<bool> OnTestarConexaoAsync()
        {
            return await Task.Run(() =>
            {
                var retorno = false;

                try
                {
                    this.EnableButtonConfiguracao = false;
                    this.Configuracao.Mensagem = "Testando Conexao...";

                    (new PostoContext(this.Configuracao)).Close();
                    this.Configuracao.Mensagem = "Conectado com sucesso!";
                    retorno = true;
                }
                catch (Exception e)
                {
                    this.Configuracao.Mensagem = e.Message;
                }
                finally 
                {
                    this.EnableButtonConfiguracao = true;
                }

                return retorno;
            });
        }

        private async void OnAtualizar()
        {
            var config = ConfiguracaoXml.CarregarConfiguracao().ToModel();

            if (this.IsValidarConfiguracao(config))
            {
                this._atualizarAsync.Manual(config, this.AtualizarAfterCommand);
                this.IsEnableButtonAtualizar = false;
            }
            else
            {
                MessageBox.Show("Verifique a aba de configuração!", "Problemas ao inicializar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnAtualizarAfter()
        {
            this.IsEnableButtonAtualizar = true;
        }

        private async void OnIniciar()
        {
            var config = ConfiguracaoXml.CarregarConfiguracao().ToModel();

            if(this.IsValidarConfiguracao(config))
            {
                this._atualizarAsync.Iniciar(config);
                this.IsVisibleButtonPausar = true;
                this.IsEnableButtonAtualizar = false;
            }
            else 
            {
                MessageBox.Show("Verifique a aba de configuração!", "Problemas ao inicializar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void OnPausar()
        {
            this._atualizarAsync.Pausar();
            this.IsVisibleButtonPausar = false;
            this.IsEnableButtonAtualizar = true;
        }

        private void OnLogin() 
        {
            this.DynamicContentControl = null;
        }

        private void OnBloquear()
        {
            //this.DynamicContentControl = null;
            this.DynamicContentControl = new LoginViewModel(this.LoginCommand);
        }

        /// <summary>
        /// Seta o focus
        /// </summary>
        /// <param name="focus"></param>
        private void SetFocus(Focus? focus)
        {
            if (focus.HasValue)
            {
                this.FocusElement = focus.Value.ToString();
            }
            else
            {
                this.FocusElement = string.Empty;
            }
        }

        private bool IsValidarConfiguracao(ConfiguracaoModel configuracao)
        {
            if (string.IsNullOrWhiteSpace(configuracao.Servidor))
            {
                this.Configuracao.Mensagem = "Erro: Preencha o servidor";
                this.SetFocus(Focus.InputServidor);
                return false;
            }

            if (configuracao.Porta <= 0)
            {
                this.Configuracao.Mensagem = "Erro: Preencha a porta";
                this.SetFocus(Focus.InputPorta);
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.Banco))
            {
                this.Configuracao.Mensagem = "Erro: Preencha o banco de dados";
                this.SetFocus(Focus.InputBanco);
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.Usuario))
            {
                this.Configuracao.Mensagem = "Erro: Preencha o usuário";
                this.SetFocus(Focus.InputUsuario);
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.Senha))
            {
                this.Configuracao.Mensagem = "Erro: Preencha o senha";
                this.SetFocus(Focus.InputSenha);
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.LocalDiretorio))
            {
                this.Configuracao.Mensagem = "Erro: Preencha o diretório";
                this.SetFocus(Focus.InputLocalDiretorio);
                return false;
            }
            return true;
        }

        #endregion

        #region Controle de Login

        public bool DynamicContentControlIsActive
        {
            get
            {
                return this._dynamicContentControlIsActive;
            }
            set
            {
                if (this._dynamicContentControlIsActive != value)
                {
                    this._dynamicContentControlIsActive = value;
                    this.RaisePropertyChanged(() => this.DynamicContentControlIsActive);
                }
            }
        }

        public NotificationObject DynamicContentControl
        {
            get
            {
                return this._dynamicContentControl;
            }
            set
            {
                if (this._dynamicContentControl != value)
                {
                    this._dynamicContentControl = value;
                    this.RaisePropertyChanged(() => this.DynamicContentControl);
                    this.DynamicContentControlIsActive = this.DynamicContentControl != null;
                }
            }
        }

        #endregion
    }
}
