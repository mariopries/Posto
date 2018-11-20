namespace Atualizador
{
    partial class TelaPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TelaPrincipal));
            this.Principal = new System.Windows.Forms.TabControl();
            this.Atualizacao = new System.Windows.Forms.TabPage();
            this.Configuracoes = new System.Windows.Forms.TabPage();
            this.statusLabel = new System.Windows.Forms.Label();
            this.AbrirExplorer = new System.Windows.Forms.Button();
            this.TestarConexao = new System.Windows.Forms.Button();
            this.Salvar = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this._localDiretorio = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this._leitorBomba = new System.Windows.Forms.CheckBox();
            this._postoWeb = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this._senha = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this._banco = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this._usuario = new System.Windows.Forms.TextBox();
            this.labelPorta = new System.Windows.Forms.Label();
            this._porta = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._servidor = new System.Windows.Forms.TextBox();
            this.abaConfiguracoesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.configuracaoModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Principal.SuspendLayout();
            this.Configuracoes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.abaConfiguracoesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.configuracaoModelBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // Principal
            // 
            this.Principal.Controls.Add(this.Atualizacao);
            this.Principal.Controls.Add(this.Configuracoes);
            this.Principal.Location = new System.Drawing.Point(12, 12);
            this.Principal.Name = "Principal";
            this.Principal.SelectedIndex = 0;
            this.Principal.Size = new System.Drawing.Size(503, 250);
            this.Principal.TabIndex = 0;
            // 
            // Atualizacao
            // 
            this.Atualizacao.Location = new System.Drawing.Point(4, 22);
            this.Atualizacao.Name = "Atualizacao";
            this.Atualizacao.Padding = new System.Windows.Forms.Padding(3);
            this.Atualizacao.Size = new System.Drawing.Size(495, 224);
            this.Atualizacao.TabIndex = 0;
            this.Atualizacao.Text = "Atualização";
            this.Atualizacao.UseVisualStyleBackColor = true;
            // 
            // Configuracoes
            // 
            this.Configuracoes.Controls.Add(this.statusLabel);
            this.Configuracoes.Controls.Add(this.AbrirExplorer);
            this.Configuracoes.Controls.Add(this.TestarConexao);
            this.Configuracoes.Controls.Add(this.Salvar);
            this.Configuracoes.Controls.Add(this.label7);
            this.Configuracoes.Controls.Add(this._localDiretorio);
            this.Configuracoes.Controls.Add(this.label6);
            this.Configuracoes.Controls.Add(this._leitorBomba);
            this.Configuracoes.Controls.Add(this._postoWeb);
            this.Configuracoes.Controls.Add(this.label5);
            this.Configuracoes.Controls.Add(this._senha);
            this.Configuracoes.Controls.Add(this.label4);
            this.Configuracoes.Controls.Add(this._banco);
            this.Configuracoes.Controls.Add(this.label3);
            this.Configuracoes.Controls.Add(this._usuario);
            this.Configuracoes.Controls.Add(this.labelPorta);
            this.Configuracoes.Controls.Add(this._porta);
            this.Configuracoes.Controls.Add(this.label1);
            this.Configuracoes.Controls.Add(this._servidor);
            this.Configuracoes.Location = new System.Drawing.Point(4, 22);
            this.Configuracoes.Name = "Configuracoes";
            this.Configuracoes.Padding = new System.Windows.Forms.Padding(3);
            this.Configuracoes.Size = new System.Drawing.Size(495, 224);
            this.Configuracoes.TabIndex = 1;
            this.Configuracoes.Text = "Configurações";
            this.Configuracoes.UseVisualStyleBackColor = true;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.abaConfiguracoesBindingSource, "MensagemLabel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.statusLabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(9, 204);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(48, 13);
            this.statusLabel.TabIndex = 19;
            this.statusLabel.Text = "Status";
            // 
            // AbrirExplorer
            // 
            this.AbrirExplorer.Location = new System.Drawing.Point(463, 104);
            this.AbrirExplorer.Name = "AbrirExplorer";
            this.AbrirExplorer.Size = new System.Drawing.Size(26, 21);
            this.AbrirExplorer.TabIndex = 18;
            this.AbrirExplorer.Text = "...";
            this.AbrirExplorer.UseVisualStyleBackColor = true;
            this.AbrirExplorer.Click += new System.EventHandler(this.AbrirExplorer_Click);
            // 
            // TestarConexao
            // 
            this.TestarConexao.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.abaConfiguracoesBindingSource, "EnableButtonConfiguracao", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.TestarConexao.Location = new System.Drawing.Point(301, 184);
            this.TestarConexao.Name = "TestarConexao";
            this.TestarConexao.Size = new System.Drawing.Size(106, 34);
            this.TestarConexao.TabIndex = 17;
            this.TestarConexao.Text = "Testar conexão";
            this.TestarConexao.UseVisualStyleBackColor = true;
            this.TestarConexao.Click += new System.EventHandler(this.TestarConexao_Click);
            // 
            // Salvar
            // 
            this.Salvar.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.abaConfiguracoesBindingSource, "EnableButtonConfiguracao", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Salvar.Location = new System.Drawing.Point(413, 184);
            this.Salvar.Name = "Salvar";
            this.Salvar.Size = new System.Drawing.Size(75, 34);
            this.Salvar.TabIndex = 16;
            this.Salvar.Text = "Salvar";
            this.Salvar.UseVisualStyleBackColor = true;
            this.Salvar.Click += new System.EventHandler(this.Salvar_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(132, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Local dos executáveis";
            // 
            // _localDiretorio
            // 
            this._localDiretorio.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configuracaoModelBindingSource, "LocalDiretorio", true));
            this._localDiretorio.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._localDiretorio.Location = new System.Drawing.Point(6, 104);
            this._localDiretorio.Name = "_localDiretorio";
            this._localDiretorio.Size = new System.Drawing.Size(451, 21);
            this._localDiretorio.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Location = new System.Drawing.Point(6, 175);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(483, 2);
            this.label6.TabIndex = 13;
            // 
            // _leitorBomba
            // 
            this._leitorBomba.AutoSize = true;
            this._leitorBomba.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.configuracaoModelBindingSource, "LeitorBomba", true));
            this._leitorBomba.Location = new System.Drawing.Point(19, 131);
            this._leitorBomba.Name = "_leitorBomba";
            this._leitorBomba.Size = new System.Drawing.Size(246, 17);
            this._leitorBomba.TabIndex = 11;
            this._leitorBomba.Text = "Encerrar Leitor de bombas durante atualização";
            this._leitorBomba.UseVisualStyleBackColor = true;
            // 
            // _postoWeb
            // 
            this._postoWeb.AutoSize = true;
            this._postoWeb.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.configuracaoModelBindingSource, "PostoWeb", true));
            this._postoWeb.Location = new System.Drawing.Point(19, 154);
            this._postoWeb.Name = "_postoWeb";
            this._postoWeb.Size = new System.Drawing.Size(233, 17);
            this._postoWeb.TabIndex = 12;
            this._postoWeb.Text = "Encerrar o Posto web durante a atualização\r\n";
            this._postoWeb.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(298, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Senha";
            // 
            // _senha
            // 
            this._senha.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configuracaoModelBindingSource, "Senha", true));
            this._senha.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._senha.Location = new System.Drawing.Point(301, 64);
            this._senha.Name = "_senha";
            this._senha.PasswordChar = '*';
            this._senha.Size = new System.Drawing.Size(188, 21);
            this._senha.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(298, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Banco de dados";
            // 
            // _banco
            // 
            this._banco.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configuracaoModelBindingSource, "Banco", true));
            this._banco.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._banco.Location = new System.Drawing.Point(302, 24);
            this._banco.Name = "_banco";
            this._banco.Size = new System.Drawing.Size(187, 21);
            this._banco.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Usuário";
            // 
            // _usuario
            // 
            this._usuario.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configuracaoModelBindingSource, "Usuario", true));
            this._usuario.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._usuario.Location = new System.Drawing.Point(6, 64);
            this._usuario.Name = "_usuario";
            this._usuario.Size = new System.Drawing.Size(280, 21);
            this._usuario.TabIndex = 5;
            // 
            // labelPorta
            // 
            this.labelPorta.AutoSize = true;
            this.labelPorta.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPorta.Location = new System.Drawing.Point(147, 8);
            this.labelPorta.Name = "labelPorta";
            this.labelPorta.Size = new System.Drawing.Size(37, 13);
            this.labelPorta.TabIndex = 4;
            this.labelPorta.Text = "Porta";
            // 
            // _porta
            // 
            this._porta.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configuracaoModelBindingSource, "Porta", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, "0"));
            this._porta.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._porta.Location = new System.Drawing.Point(150, 24);
            this._porta.Name = "_porta";
            this._porta.Size = new System.Drawing.Size(136, 21);
            this._porta.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Servidor";
            // 
            // _servidor
            // 
            this._servidor.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configuracaoModelBindingSource, "Servidor", true));
            this._servidor.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._servidor.Location = new System.Drawing.Point(6, 24);
            this._servidor.Name = "_servidor";
            this._servidor.Size = new System.Drawing.Size(138, 21);
            this._servidor.TabIndex = 1;
            // 
            // abaConfiguracoesBindingSource
            // 
            this.abaConfiguracoesBindingSource.DataSource = typeof(Atualizador.Structures.AbaConfiguracoes);
            // 
            // configuracaoModelBindingSource
            // 
            this.configuracaoModelBindingSource.DataSource = typeof(Atualizador.Models.ConfiguracaoModel);
            // 
            // TelaPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Atualizador.Properties.Resources.background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(527, 274);
            this.Controls.Add(this.Principal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TelaPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Atualizador - Controle de atualizações do servidor ";
            this.Principal.ResumeLayout(false);
            this.Configuracoes.ResumeLayout(false);
            this.Configuracoes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.abaConfiguracoesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.configuracaoModelBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Principal;
        private System.Windows.Forms.TabPage Atualizacao;
        private System.Windows.Forms.TabPage Configuracoes;
        private System.Windows.Forms.TextBox _servidor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _usuario;
        private System.Windows.Forms.Label labelPorta;
        private System.Windows.Forms.TextBox _porta;
        private System.Windows.Forms.TextBox _banco;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox _senha;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox _postoWeb;
        private System.Windows.Forms.CheckBox _leitorBomba;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button TestarConexao;
        private System.Windows.Forms.Button Salvar;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox _localDiretorio;
        private System.Windows.Forms.Button AbrirExplorer;
        private System.Windows.Forms.BindingSource configuracaoModelBindingSource;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.BindingSource abaConfiguracoesBindingSource;
    }
}

