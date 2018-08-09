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
using System.Windows.Input;

namespace Posto.Win.Update.ViewModel
{
    public class MainWindowViewModel : NotificationObject
    {
        public class Status : NotificationObject
        {
            #region Construtor

            public Status()
            {
                BarraProgresso = new Barra();
                StatusLabel = new Label();
            }

            #endregion

            #region Propriedades

            private Barra _barraprogresso;
            private Label _statuslabel;

            #endregion

            #region Elementos

            public Barra BarraProgresso
            {
                get
                {
                    return _barraprogresso;
                }
                set
                {
                    if (_barraprogresso != value)
                    {
                        _barraprogresso = value;
                        RaisePropertyChanged(() => BarraProgresso);
                    }
                }
            }
            public Label StatusLabel
            {
                get
                {
                    return _statuslabel;
                }
                set
                {
                    if (_statuslabel != value)
                    {
                        _statuslabel = value;
                        RaisePropertyChanged(() => StatusLabel);
                    }
                }
            }

            #endregion

            #region Classes

            public class Barra : NotificationObject
            {
                public Barra()
                {
                    IsEnable = false;
                    Visao = System.Windows.Visibility.Hidden;
                    ProgressoBarra1 = 0;
                    ProgressoBarra2 = 0;
                }

                #region Propriedades

                private bool _isEnable;
                private System.Windows.Visibility _visao;
                private double _progressbarra1;
                private double _progressbarra2;

                #endregion

                public bool IsEnable
                {
                    get
                    {
                        return _isEnable;
                    }
                    set
                    {
                        if (_isEnable != value)
                        {
                            _isEnable = value;
                            RaisePropertyChanged(() => IsEnable);
                        }
                    }
                }
                public System.Windows.Visibility Visao
                {
                    get
                    {
                        return _visao;
                    }
                    set
                    {
                        if (_visao != value)
                        {
                            _visao = value;
                            RaisePropertyChanged(() => Visao);
                        }
                    }
                }
                public double ProgressoBarra1
                {
                    get
                    {
                        return _progressbarra1;
                    }
                    set
                    {
                        if (_progressbarra1 != value)
                        {
                            _progressbarra1 = value;
                            RaisePropertyChanged(() => ProgressoBarra1);
                        }
                    }
                }
                public double ProgressoBarra2
                {
                    get
                    {
                        return _progressbarra2;
                    }
                    set
                    {
                        if (_progressbarra2 != value)
                        {
                            _progressbarra2 = value;
                            RaisePropertyChanged(() => ProgressoBarra2);
                        }
                    }
                }

            }
            public class Label : NotificationObject
            {
                public Label()
                {
                    Margin = new System.Windows.Thickness(10, 25, 10, 0);
                }

                #region Propriedades

                private System.Windows.Thickness _margin;

                #endregion

                public System.Windows.Thickness Margin
                {
                    get
                    {
                        return _margin;
                    }
                    set
                    {
                        if (_margin != value)
                        {
                            _margin = value;
                            RaisePropertyChanged(() => Margin);
                        }
                    }
                }
            }

            #endregion
        }

        public class Configuracao : NotificationObject
        {
            #region Enums

            private enum Focus
            {
                InputServidor,
                InputPorta,
                InputBanco,
                InputUsuario,
                InputSenha,
                InputLocalDiretorio
            }

            #endregion

            #region Propriedades

            private ConfiguracaoModel _configuracoes;
            private bool _enableButtonConfiguracao;
            private string _focusElement;
            private string _mensagemlabel;

            #endregion

            #region Construtor

            public Configuracao()
            {
                Configuracoes = ConfiguracaoXml.CarregarConfiguracao().ToModel();
                EnableButtonConfiguracao = true;
            }

            #endregion

            #region Funcoes

            public ConfiguracaoModel Configuracoes
            {
                get
                {
                    return _configuracoes;
                }
                set
                {
                    if (_configuracoes != value)
                    {
                        _configuracoes = value;
                        RaisePropertyChanged(() => Configuracoes);
                    }
                }
            }
            public string FocusElement
            {
                get
                {
                    return _focusElement;
                }
                set
                {
                    _focusElement = value;
                    RaisePropertyChanged(() => FocusElement);
                }
            }
            public bool IsValidarConfiguracao(ConfiguracaoModel configuracao)
            {
                if (string.IsNullOrWhiteSpace(configuracao.Servidor))
                {
                    MensagemLabel = "Erro: Preencha o servidor";
                    SetFocus(Focus.InputServidor);
                    return false;
                }

                if (configuracao.Porta <= 0)
                {
                    MensagemLabel = "Erro: Preencha a porta";
                    SetFocus(Focus.InputPorta);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(configuracao.Banco))
                {
                    MensagemLabel = "Erro: Preencha o banco de dados";
                    SetFocus(Focus.InputBanco);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(configuracao.Usuario))
                {
                    MensagemLabel = "Erro: Preencha o usuário";
                    SetFocus(Focus.InputUsuario);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(configuracao.Senha))
                {
                    MensagemLabel = "Erro: Preencha o senha";
                    SetFocus(Focus.InputSenha);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(configuracao.LocalDiretorio))
                {
                    MensagemLabel = "Erro: Preencha o diretório";
                    SetFocus(Focus.InputLocalDiretorio);
                    return false;
                }
                return true;
            }
            public bool EnableButtonConfiguracao
            {
                get
                {
                    return _enableButtonConfiguracao;
                }
                set
                {
                    if (_enableButtonConfiguracao != value)
                    {
                        _enableButtonConfiguracao = value;
                        RaisePropertyChanged(() => EnableButtonConfiguracao);
                    }
                }
            }
            public string MensagemLabel
            {
                get
                {
                    return _mensagemlabel;
                }
                set
                {
                    if (_mensagemlabel != value)
                    {
                        _mensagemlabel = value;
                        RaisePropertyChanged(() => MensagemLabel);
                    }
                }
            }

            #endregion

            #region Helpers

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

            public async void OnSalvar()
            {
                await Task.Run(() =>
                {
                    if (IsValidarConfiguracao(Configuracoes))
                    {
                        EnableButtonConfiguracao = false;
                        MensagemLabel = "Salvando configuração...";

                        Configuracoes.ToModel().GravarConfiguracao();
                        MensagemLabel = "Configuração Salva.";
                        EnableButtonConfiguracao = true;
                    }
                });
            }

            public async void OnTestarConexao()
            {
                await OnTestarConexaoAsync();
            }

            public async Task<bool> OnTestarConexaoAsync()
            {
                return await Task.Run(() =>
                {
                    var retorno = false;

                    try
                    {
                        EnableButtonConfiguracao = false;
                        MensagemLabel = "Testando Conexao...";

                        (new PostoContext(Configuracoes)).Close();
                        MensagemLabel = "Conectado com sucesso!";
                        retorno = true;
                    }
                    catch (Exception e)
                    {
                        MensagemLabel = e.Message;
                    }
                    finally
                    {
                        EnableButtonConfiguracao = true;
                    }

                    return retorno;
                });
            }

            #endregion
        }
        
        private Status _status;        
        private AtualizarModel _atualizar;
        private Configuracao _configuracoes;
        private IndicadoresManutencao _indicadores;
        private Atualizar _atualizarAsync;               
        private bool _isVisibleButtonPausar;
        private bool _isEnableButtonAtualizar;
        private bool _dynamicContentControlIsActive;
        private NotificationObject _dynamicContentControl;

        public MainWindowViewModel()
        {
            OnLoad();
        }

        private async void OnLoad()
        {
            Atualizar              = new AtualizarModel();            
            StackStatus            = new Status();
            Configurar             = new Configuracao();
            Indicadores            = new IndicadoresManutencao(Configurar.Configuracoes);
            _atualizarAsync        = new Atualizar(Atualizar, this);
            AbrirExplorerCommand   = new DelegateCommand(OnAbrirExplorer);
            SalvarCommand          = new DelegateCommand(Configurar.OnSalvar);
            TestarConexaoCommand   = new DelegateCommand(Configurar.OnTestarConexao);
            AtualizarCommand       = new DelegateCommand(OnAtualizar);
            AtualizarAfterCommand  = new DelegateCommand(OnAtualizarAfter);
            IniciarCommand         = new DelegateCommand(OnIniciar);
            PausarCommand          = new DelegateCommand(OnPausar);
            LoginCommand           = new DelegateCommand(OnLogin);
            BloquearCommand        = new DelegateCommand(OnBloquear);

            //FecharCommand          = new DelegateCommand(OnFechar);
            //CancelFecharCommand    = new DelegateCommand(OnCancelFechar);

            IsVisibleButtonPausar       = false;
            IsEnableButtonAtualizar     = true;
            
            var conexao = await Configurar.OnTestarConexaoAsync();

            if (conexao)
            {
                if (!Indicadores.EmManutencao && Indicadores.FimManutencao)
                {
                    OnIniciar();
                }
                else
                {
                    OnAtualizar();
                }
            }

            //this.DynamicContentControl = new LoginViewModel(this.LoginCommand);
        }

        #region Campos
        
        public AtualizarModel Atualizar
        {
            get
            {
                return _atualizar;
            }
            set
            {
                if (_atualizar != value)
                {
                    _atualizar = value;
                    RaisePropertyChanged(() => Atualizar);
                }
            }
        }

        public Configuracao Configurar
        {
            get
            {
                return _configuracoes;
            }
            set
            {
                if (_configuracoes != value)
                {
                    _configuracoes = value;
                    RaisePropertyChanged(() => Configurar);
                }
            }
        }

        public Status StackStatus
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    RaisePropertyChanged(() => StackStatus);
                }
            }
        }

        public bool IsVisibleButtonPausar
        {
            get
            {
                return _isVisibleButtonPausar;
            }
            set
            {
                if (_isVisibleButtonPausar != value)
                {
                    _isVisibleButtonPausar = value;
                    RaisePropertyChanged(() => IsVisibleButtonPausar);
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

        public IndicadoresManutencao Indicadores
        {
            get
            {
                return _indicadores;
            }
            set
            {
                if (_indicadores != value)
                {
                    _indicadores = value;
                    RaisePropertyChanged(() => Indicadores);
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
                    Configurar.Configuracoes.LocalDiretorio = folderDialog.SelectedPath;
                }
            }
        }      
        
        private void OnAtualizar()
        {
            if (Configurar.IsValidarConfiguracao(Configurar.Configuracoes))
            {
                IsEnableButtonAtualizar = false;
                StackStatus.BarraProgresso.IsEnable = true;
                _atualizarAsync.Manual(Configurar.Configuracoes, AtualizarAfterCommand);
            }
            else
            {
                MessageBox.Show("Verifique a aba de configuração!", "Problemas ao inicializar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnAtualizarAfter()
        {
            IsEnableButtonAtualizar = true;
            StackStatus.BarraProgresso.IsEnable = false;
        }

        private void OnIniciar()
        {
            var config = ConfiguracaoXml.CarregarConfiguracao().ToModel();

            if (Configurar.IsValidarConfiguracao(Configurar.Configuracoes))
            {
                _atualizarAsync.Iniciar(config);
                IsVisibleButtonPausar = true;
                IsEnableButtonAtualizar = false;
                StackStatus.BarraProgresso.IsEnable = true;
            }
            else
            {
                MessageBox.Show("Verifique a aba de configuração!", "Problemas ao inicializar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnPausar()
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

        private void OnFechar()
        {
            if (MessageBox.Show("Existe uma atualização em andamento, se sair o procedimento será cancelado. Tem certeza que deseja sair ?", "Sysloja Informa", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                Fecha();
            }
            else
            {

            }
        }

        private void OnCancelFechar()
        {
            MessageBox.Show("Fechamento cancelado", "Oi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Fecha()
        {
            System.Windows.Application.Current.Shutdown();
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
