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

        private async void MatBtnSalvarConfig_Click(object sender, EventArgs e)
        {
            AbaConfiguracoes.MensagemLabel = "Salvando configuração...";
            AbaConfiguracoes.EnableButtonConfiguracao = false;

            await Task.Run(() =>
            {
                if (IsValidarConfiguracao(AbaConfiguracoes.ConfiguracaoModel))
                {
                    AbaConfiguracoes.ConfiguracaoModel.ToModel().GravarConfiguracao();
                    MessageBox.Show("Configuração salvas com sucesso!", "Salvar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });

            AbaConfiguracoes.MensagemLabel = "";
            AbaConfiguracoes.EnableButtonConfiguracao = true;
        }

        private async void MatBtnTestarConexao_Click(object sender, EventArgs e)
        {
            AbaConfiguracoes.EnableButtonConfiguracao = false;
            AbaConfiguracoes.MensagemLabel = "Testando conexão...";

            bool retorno = await TestarConexaoAsync();

            if (retorno)
            {
                AbaConfiguracoes.MensagemLabel = "";
                MessageBox.Show("Conectado com sucesso!", "Teste de conexão", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    AbaConfiguracoes.MensagemLabel = "";
                    MessageBox.Show(e.Message, "Teste de conexão", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    this.matServer.Select();
                }
                return false;
            }

            if (configuracao.Porta <= 0)
            {
                DialogResult result = MessageBox.Show("Preencha o porta", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    this.matPorta.Select();
                }
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.Banco))
            {
                DialogResult result = MessageBox.Show("Preencha o banco de dados", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    this.matBanco.Select();
                }
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.Usuario))
            {
                DialogResult result = MessageBox.Show("Preencha o usuário", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    this.matUsuario.Select();
                }
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.Senha))
            {
                DialogResult result = MessageBox.Show("Preencha o senha", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    this.matSenha.Select();
                }
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.DiretorioSistema))
            {
                DialogResult result = MessageBox.Show("Selecione o diretório de instalação", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        matBtnSistemDir.PerformClick();
                    });
                }
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.DiretorioPostgreSql))
            {
                DialogResult result = MessageBox.Show("Selecione o diretório de PostgresSql", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        matBtnPostgreSqlDir.PerformClick();
                    });
                }
                return false;
            }
            return true;
        }

        #endregion

        private void MatBtnSistemDir_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new CommonOpenFileDialog())
            {
                folderDialog.IsFolderPicker = true;
                folderDialog.Title = "Selecione o local do sistema";

                CommonFileDialogResult result = folderDialog.ShowDialog();

                if (result.ToString() == "Ok")
                {
                    AbaConfiguracoes.ConfiguracaoModel.DiretorioSistema = folderDialog.FileName;
                }
            }
        }

        private void MatBtnPostgreSqlDir_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new CommonOpenFileDialog())
            {
                folderDialog.IsFolderPicker = true;
                folderDialog.Title = "Selecione a pasta bin do PostgreSQL";

                CommonFileDialogResult result = folderDialog.ShowDialog();

                if (result.ToString() == "Ok")
                {
                    AbaConfiguracoes.ConfiguracaoModel.DiretorioPostgreSql = folderDialog.FileName;
                }
            }
        }

    }
}
