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

namespace Atualizador
{
    public partial class TelaPrincipal : Form
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

        public TelaPrincipal()
        {
            InitializeComponent();
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
                    AbaConfiguracoes.MensagemLabel = "Configuração Salva.";

                }
            });

            AbaConfiguracoes.EnableButtonConfiguracao = true;
        }

        public async void TestarConexao_Click(object sender, EventArgs e)
        {
            AbaConfiguracoes.EnableButtonConfiguracao = false;
            AbaConfiguracoes.MensagemLabel = "Testando Conexao...";

            bool retorno = await TestarConexaoAsync();

            if (retorno)
            {
                AbaConfiguracoes.MensagemLabel = "Conectado com sucesso!";
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
                DialogResult result = MessageBox.Show("Preencha o servidor", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    this._servidor.Select();
                }
                return false;
            }

            if (configuracao.Porta <= 0)
            {
                DialogResult result = MessageBox.Show("Preencha o porta", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    this._porta.Select();
                }
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.Banco))
            {
                DialogResult result = MessageBox.Show("Preencha o banco de dados", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    this._banco.Select();
                }
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.Usuario))
            {
                DialogResult result = MessageBox.Show("Preencha o usuário", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    this._usuario.Select();
                }
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.Senha))
            {
                DialogResult result = MessageBox.Show("Preencha o senha", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    this._senha.Select();
                }
                return false;
            }

            if (string.IsNullOrWhiteSpace(configuracao.LocalDiretorio))
            {
                DialogResult result = MessageBox.Show("Selecione o diretório de instalação", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
