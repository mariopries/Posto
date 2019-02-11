using Atualizador.DataContext;
using Atualizador.Extensions;
using Atualizador.Models;
using Atualizador.Structures;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace Atualizador
{
    public partial class TelaPrincipal : MaterialForm
    {
        #region Propriedades

        //private AbaAtualizar _abaatualizar;
        private AbaConfiguracoes _abaconfiguracoes;
        //private IndicadoresManutencao _indicadores;
        //private Atualizar _atualizarAsync;
        //private bool _dynamicContentControlIsActive;
        //private NotificationObject _sobreContent;
        //private NotificationObject _dynamicContentControl;

        #endregion
        private readonly MaterialSkinManager materialSkinManager;
        public TelaPrincipal()
        {
            InitializeComponent();

            // Initialize MaterialSkinManager
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Green600, Primary.Green700, Primary.Green200, Accent.Red100, TextShade.WHITE);
            
            AbaConfiguracoes = new AbaConfiguracoes();
            configuracaoModelBindingSource.DataSource = AbaConfiguracoes.ConfiguracaoModel;
            abaConfiguracoesBindingSource.DataSource = AbaConfiguracoes;
















        }

        #region Objetos

        private AbaConfiguracoes AbaConfiguracoes
        {
            get { return _abaconfiguracoes; }
            set
            {
                if (_abaconfiguracoes != value)
                {
                    _abaconfiguracoes = value;
                }
            }
        }

        #endregion

        #region Funções Async

        private async void Salvar_Click(object sender, EventArgs e)
        {
            AbaConfiguracoes.MensagemLabel = "Salvando configuração...";
            AbaConfiguracoes.EnableButtonConfiguracao = false;

            await Task.Run(() =>
            {
                if (IsValidarConfiguracao(AbaConfiguracoes.ConfiguracaoModel))
                {
                    AbaConfiguracoes.ConfiguracaoModel.ToModel().GravarConfiguracao();
                    DialogResult result = MessageBox.Show("Configuração salvas com sucesso!", "Salvar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });

            AbaConfiguracoes.MensagemLabel = "";
            AbaConfiguracoes.EnableButtonConfiguracao = true;
        }

        public async void TestarConexao_Click(object sender, EventArgs e)
        {
            AbaConfiguracoes.EnableButtonConfiguracao = false;
            AbaConfiguracoes.MensagemLabel = "Testando conexão...";

            bool retorno = await TestarConexaoAsync();

            if (retorno)
            {
                AbaConfiguracoes.MensagemLabel = "";
                DialogResult result = MessageBox.Show("Conectado com sucesso!", "Teste de conexão", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            AbaConfiguracoes.EnableButtonConfiguracao = true;

        }
        private async Task<bool> TestarConexaoAsync()
        {
            return await Task.Run(() =>
            {
                var retorno = false;

                try
                {
                    (new PostoContext(AbaConfiguracoes.ConfiguracaoModel)).Close();
                    retorno = true;
                }
                catch (Exception e)
                {
                    AbaConfiguracoes.MensagemLabel = e.Message;
                }

                return retorno;
            });
        }

        #endregion
        
        #region Funções

        private bool IsValidarConfiguracao(ConfiguracaoModel configuracao)
        {
            if (string.IsNullOrWhiteSpace(configuracao.Servidor))
            {
                DialogResult result = MessageBox.Show("Preencha o servidor", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    this._servidor.Select();
                }
                return false;
            }

            if (configuracao.Porta <= 0)
            {
                DialogResult result = MessageBox.Show("Preencha o porta", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    this._porta.Select();
                }
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.Banco))
            {
                DialogResult result = MessageBox.Show("Preencha o banco de dados", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    this._banco.Select();
                }
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.Usuario))
            {
                DialogResult result = MessageBox.Show("Preencha o usuário", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    this._usuario.Select();
                }
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.Senha))
            {
                DialogResult result = MessageBox.Show("Preencha o senha", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    this._senha.Select();
                }
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.LocalDiretorio))
            {
                DialogResult result = MessageBox.Show("Selecione o diretório de instalação", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        AbrirExplorer.PerformClick();
                    });
                }
                return false;
            }
            return true;
        }


        private void AbrirExplorer_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new CommonOpenFileDialog())
            {
                folderDialog.IsFolderPicker = true;
                folderDialog.Title = "Selecionar local de instalação";

                CommonFileDialogResult result = folderDialog.ShowDialog();

                if (result.ToString() == "Ok")
                {
                    AbaConfiguracoes.ConfiguracaoModel.LocalDiretorio = folderDialog.FileName;
                }
            }
        }

        #endregion

    }
}
