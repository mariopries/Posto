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
using Posto.Win.Update.Abas;

namespace Posto.Win.Update.ViewModel
{
    public class MainWindowViewModel : NotificationObject
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

        private AbaAtualizar _abaatualizar;        
        private AbaConfiguracoes _abaconfiguracoes;
        private IndicadoresManutencao _indicadores;
        private Atualizar _atualizarAsync;
        private bool _dynamicContentControlIsActive;
        private NotificationObject _sobreContent;
        private NotificationObject _dynamicContentControl;
        private string _focusElement;

        #endregion

        #region Construtor

        public MainWindowViewModel()
        {
            OnLoad();
        }

        private async void OnLoad()
        {
            AbaAtualizar            = new AbaAtualizar();
            AbaConfiguracoes        = new AbaConfiguracoes();
            Indicadores             = new IndicadoresManutencao(AbaConfiguracoes.ConfiguracaoModel);
            _atualizarAsync         = new Atualizar(this);
            AbrirExplorerCommand    = new DelegateCommand(OnAbrirExplorer);
            SalvarCommand           = new DelegateCommand(OnSalvar);
            TestarConexaoCommand    = new DelegateCommand(OnTestarConexao);
            AtualizarCommand        = new DelegateCommand(OnAtualizar);
            AtualizarAfterCommand   = new DelegateCommand(OnAtualizarAfter);
            IniciarCommand          = new DelegateCommand(OnIniciar);
            PausarCommand           = new DelegateCommand(OnPausar);
            LoginCommand            = new DelegateCommand(OnLogin);
            BloquearCommand         = new DelegateCommand(OnBloquear);
            FecharCommand           = new DelegateCommand(OnFechar, OnPodeFechar);
            MenuFecharCommand       = new DelegateCommand(OnMenuFechar);
            SobreContentCommand     = new DelegateCommand(OnSobre);
            
            
            
            var conexao = await OnTestarConexaoAsync();

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
        #endregion

        #region Objetos

        private string FocusElement
        {
            get { return _focusElement; }
            set { if (_focusElement != value) { _focusElement = value; RaisePropertyChanged(() => FocusElement); } }
        }
        public AbaAtualizar AbaAtualizar
        {
            get { return _abaatualizar; }
            set { if (_abaatualizar != value) { _abaatualizar = value; } }
        }
        public AbaConfiguracoes AbaConfiguracoes
        {
            get
            {
                return _abaconfiguracoes;
            }
            set
            {
                if (_abaconfiguracoes != value)
                {
                    _abaconfiguracoes = value;
                    RaisePropertyChanged(() => AbaConfiguracoes);
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
        public DelegateCommand FecharCommand { get; set; }
        public DelegateCommand CancelaFecharCommand { get; set; }
        public DelegateCommand MenuFecharCommand { get; set; }
        public DelegateCommand SobreContentCommand { get; set; }


        #endregion

        #region Helpers

        private void OnAbrirExplorer()
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    AbaConfiguracoes.ConfiguracaoModel.LocalDiretorio = folderDialog.SelectedPath;
                }
            }
        }              
        private void OnAtualizar()
        {
            if (IsValidarConfiguracao(AbaConfiguracoes.ConfiguracaoModel))
            {
                AbaAtualizar.IsEnableButtonAtualizar = false;
                AbaAtualizar.Status.BarraProgresso.IsEnable = true;
                _atualizarAsync.Manual(AtualizarAfterCommand);
            }
            else
            {
                MessageBox.Show("Verifique a aba de configuração!", "Problemas ao inicializar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void OnAtualizarAfter()
        {
            AbaAtualizar.IsEnableButtonAtualizar = true;
            AbaAtualizar.Status.BarraProgresso.IsEnable = false;
        }
        private void OnIniciar()
        {
            var config = ConfiguracaoXml.CarregarConfiguracao().ToModel();

            if (IsValidarConfiguracao(AbaConfiguracoes.ConfiguracaoModel))
            {
                _atualizarAsync.Iniciar();
                AbaAtualizar.IsVisibleButtonPausar = true;
                AbaAtualizar.IsEnableButtonAtualizar = false;
                AbaAtualizar.Status.BarraProgresso.IsEnable = true;
            }
            else
            {
                MessageBox.Show("Verifique a aba de configuração!", "Problemas ao inicializar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void OnPausar()
        {
            _atualizarAsync.Pausar();
            AbaAtualizar.IsVisibleButtonPausar = false;
            AbaAtualizar.IsEnableButtonAtualizar = true;
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
        private void OnSobre()
        {
            SobreContent = new SobreViewModel();
        }
        private void OnFechar()
        {
            OnPausar();
            //MessageBox.Show("A manutenção ficará pendente, ao abrir o programa, a atualização será automaticamente iniciada.", "Atualização cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        private bool OnPodeFechar()
        {
            if (Indicadores.EmManutencao && !Indicadores.FimManutencao)
            {
                return MessageBox.Show("Existe uma atualização em andamento, se sair o procedimento será cancelado. Ao abrir o programa, a atualização será automaticamente iniciada. Tem certeza que deseja sair?", "Atualização em andamento", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
            }
            else
            {
                return true;
            }
        }
        private void OnMenuFechar()
        {
            if (OnPodeFechar())
            {
                OnFechar();
                Fecha();
            }
        }        

        #endregion

        #region Async

        private async void OnSalvar()
        {
            await Task.Run(() =>
            {
                if (IsValidarConfiguracao(AbaConfiguracoes.ConfiguracaoModel))
                {
                    AbaConfiguracoes.EnableButtonConfiguracao = false;
                    AbaConfiguracoes.MensagemLabel = "Salvando configuração...";

                    AbaConfiguracoes.ConfiguracaoModel.ToModel().GravarConfiguracao();
                    AbaConfiguracoes.MensagemLabel = "Configuração Salva.";
                    AbaConfiguracoes.EnableButtonConfiguracao = true;
                }
            });
        }
        public async void OnTestarConexao()
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
                    AbaConfiguracoes.EnableButtonConfiguracao = false;
                    AbaConfiguracoes.MensagemLabel = "Testando Conexao...";

                    (new PostoContext(AbaConfiguracoes.ConfiguracaoModel)).Close();
                    AbaConfiguracoes.MensagemLabel = "Conectado com sucesso!";
                    retorno = true;
                }
                catch (Exception e)
                {
                    AbaConfiguracoes.MensagemLabel = e.Message;
                }
                finally
                {
                    AbaConfiguracoes.EnableButtonConfiguracao = true;
                }

                return retorno;
            });
        }

        #endregion

        #region Funções

        private void Fecha()
        {
            System.Windows.Application.Current.Shutdown();
        }
        public bool IsValidarConfiguracao(ConfiguracaoModel configuracao)
        {
            if (string.IsNullOrWhiteSpace(configuracao.Servidor))
            {
                AbaConfiguracoes.MensagemLabel = "Erro: Preencha o servidor";
                SetFocus(Focus.InputServidor);
                return false;
            }

            if (configuracao.Porta <= 0)
            {
                AbaConfiguracoes.MensagemLabel = "Erro: Preencha a porta";
                SetFocus(Focus.InputPorta);
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.Banco))
            {
                AbaConfiguracoes.MensagemLabel = "Erro: Preencha o banco de dados";
                SetFocus(Focus.InputBanco);
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.Usuario))
            {
                AbaConfiguracoes.MensagemLabel = "Erro: Preencha o usuário";
                SetFocus(Focus.InputUsuario);
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.Senha))
            {
                AbaConfiguracoes.MensagemLabel = "Erro: Preencha o senha";
                SetFocus(Focus.InputSenha);
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.LocalDiretorio))
            {
                AbaConfiguracoes.MensagemLabel = "Erro: Preencha o diretório";
                SetFocus(Focus.InputLocalDiretorio);
                return false;
            }
            return true;
        }
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

        public NotificationObject SobreContent
        {
            get
            {
                return _sobreContent;
            }
            set
            {
                if (_sobreContent != value)
                {
                    _sobreContent = value;
                    RaisePropertyChanged(() => SobreContent);
                    //DynamicContentControlIsActive = SobreContent != null;
                }
            }
        }

        #endregion
    }
}
