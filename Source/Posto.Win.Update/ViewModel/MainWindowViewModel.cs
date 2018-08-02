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
        private bool _isEnableProgressBar;
        private bool _leitor;
        private bool _web;

        private bool _dynamicContentControlIsActive;
        private NotificationObject _dynamicContentControl;

        public MainWindowViewModel()
        {
            OnLoad();
        }

        private async void OnLoad()
        {
            Atualizar              = new AtualizarModel();
            _atualizarAsync        = new Atualizar(Atualizar);
            Configuracao           = ConfiguracaoXml.CarregarConfiguracao().ToModel();
            AbrirExplorerCommand   = new DelegateCommand(OnAbrirExplorer);
            SalvarCommand          = new DelegateCommand(OnSalvar);
            TestarConexaoCommand   = new DelegateCommand(OnTestarConexao);
            AtualizarCommand       = new DelegateCommand(OnAtualizar);
            AtualizarAfterCommand  = new DelegateCommand(OnAtualizarAfter);
            IniciarCommand         = new DelegateCommand(OnIniciar);
            PausarCommand          = new DelegateCommand(OnPausar);
            LoginCommand           = new DelegateCommand(OnLogin);
            BloquearCommand        = new DelegateCommand(OnBloquear);

            EnableButtonConfiguracao    = true;
            IsVisibleButtonPausar       = false;
            IsEnableButtonAtualizar     = true;
            IsEnableProgressBar         = false;

            LeitorBomba    = LeitorBomba;
            PostoWeb       = PostoWeb;

            var conexao = await OnTestarConexaoAsync();

            if (conexao)
            {
                OnIniciar();
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
                return _isEnableButtonAtualizar;
            }
            set
            {
                if (_isEnableButtonAtualizar != value)
                {
                    _isEnableButtonAtualizar = value;
                    RaisePropertyChanged(() => IsEnableButtonAtualizar);
                }
            }
        }

        public bool IsEnableProgressBar
        {
            get
            {
                return _isEnableProgressBar;
            }
            set
            {
                if (_isEnableProgressBar != value)
                {
                    _isEnableProgressBar = value;
                    RaisePropertyChanged(() => IsEnableProgressBar);
                }
            }
        }

        public bool LeitorBomba
        {
            get
            {
                return _leitor;
            }
            set
            {
                if (_leitor != value)
                {

                    _leitor = value;
                    RaisePropertyChanged(() => LeitorBomba);
                }
            }
        }

        public bool PostoWeb
        {
            get
            {
                return _web;
            }
            set
            {
                if (_web != value)
                {

                    _web = value;
                    RaisePropertyChanged(() => PostoWeb);
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
                    Configuracao.LocalDiretorio = folderDialog.SelectedPath;
                }
            }
        }

        private async void OnSalvar()
        {
            await Task.Run(() =>
            {
                if (IsValidarConfiguracao(Configuracao))
                {
                    EnableButtonConfiguracao = false;
                    Configuracao.Mensagem = "Salvando configuração...";

                    Configuracao.ToModel().GravarConfiguracao();
                    Configuracao.Mensagem = "Configuração Salva.";
                    EnableButtonConfiguracao = true;
                }
            });
        }

        private void OnLeitor()
        {
            Configuracao.LeitorBomba = true;
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
                    EnableButtonConfiguracao = false;
                    Configuracao.Mensagem = "Testando Conexao...";

                    (new PostoContext(Configuracao)).Close();
                    Configuracao.Mensagem = "Conectado com sucesso!";
                    retorno = true;
                }
                catch (Exception e)
                {
                    Configuracao.Mensagem = e.Message;
                }
                finally
                {
                    EnableButtonConfiguracao = true;
                }

                return retorno;
            });
        }

        private void OnAtualizar()
        {
            var config = ConfiguracaoXml.CarregarConfiguracao().ToModel();

            if (IsValidarConfiguracao(config))
            {
                _atualizarAsync.Manual(config, AtualizarAfterCommand);
                IsEnableButtonAtualizar = false;
                IsEnableProgressBar = true;
            }
            else
            {
                MessageBox.Show("Verifique a aba de configuração!", "Problemas ao inicializar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnAtualizarAfter()
        {
            IsEnableButtonAtualizar = true;
            IsEnableProgressBar = false;
        }

        private async void OnIniciar()
        {
            var config = ConfiguracaoXml.CarregarConfiguracao().ToModel();

            if (IsValidarConfiguracao(config))
            {
                _atualizarAsync.Iniciar(config);
                IsVisibleButtonPausar = true;
                IsEnableButtonAtualizar = false;
            }
            else
            {
                MessageBox.Show("Verifique a aba de configuração!", "Problemas ao inicializar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void OnPausar()
        {
            _atualizarAsync.Pausar();
            IsVisibleButtonPausar = false;
            IsEnableButtonAtualizar = true;
        }

        private void OnLogin()
        {
            DynamicContentControl = null;
        }

        private void OnBloquear()
        {
            //this.DynamicContentControl = null;
            DynamicContentControl = new LoginViewModel(this.LoginCommand);
        }

        /// <summary>
        /// Seta o focus
        /// </summary>
        /// <param name="focus"></param>
        private void SetFocus(Focus? focus)
        {
            if (focus.HasValue)
            {
                FocusElement = focus.Value.ToString();
            }
            else
            {
                FocusElement = string.Empty;
            }
        }

        private bool IsValidarConfiguracao(ConfiguracaoModel configuracao)
        {
            if (string.IsNullOrWhiteSpace(configuracao.Servidor))
            {
                Configuracao.Mensagem = "Erro: Preencha o servidor";
                SetFocus(Focus.InputServidor);
                return false;
            }

            if (configuracao.Porta <= 0)
            {
                Configuracao.Mensagem = "Erro: Preencha a porta";
                SetFocus(Focus.InputPorta);
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.Banco))
            {
                Configuracao.Mensagem = "Erro: Preencha o banco de dados";
                SetFocus(Focus.InputBanco);
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.Usuario))
            {
                Configuracao.Mensagem = "Erro: Preencha o usuário";
                SetFocus(Focus.InputUsuario);
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.Senha))
            {
                Configuracao.Mensagem = "Erro: Preencha o senha";
                SetFocus(Focus.InputSenha);
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.LocalDiretorio))
            {
                Configuracao.Mensagem = "Erro: Preencha o diretório";
                SetFocus(Focus.InputLocalDiretorio);
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
                return _dynamicContentControlIsActive;
            }
            set
            {
                if (_dynamicContentControlIsActive != value)
                {
                    _dynamicContentControlIsActive = value;
                    RaisePropertyChanged(() => DynamicContentControlIsActive);
                }
            }
        }

        public NotificationObject DynamicContentControl
        {
            get
            {
                return _dynamicContentControl;
            }
            set
            {
                if (_dynamicContentControl != value)
                {
                    _dynamicContentControl = value;
                    RaisePropertyChanged(() => DynamicContentControl);
                    DynamicContentControlIsActive = DynamicContentControl != null;
                }
            }
        }

        #endregion
    }
}
