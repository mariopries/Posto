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
            this.materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            this.materialTabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.matLabVerAtu = new MaterialSkin.Controls.MaterialLabel();
            this.matLabelUlt = new MaterialSkin.Controls.MaterialLabel();
            this.matLabelProx = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.matBarraProgresso2 = new MaterialSkin.Controls.MaterialProgressBar();
            this.barraBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.matBarraProgresso1 = new MaterialSkin.Controls.MaterialProgressBar();
            this.matBtnAtualizar = new MaterialSkin.Controls.MaterialFlatButton();
            this.matBtnIniciar = new MaterialSkin.Controls.MaterialFlatButton();
            this.materialDivider1 = new MaterialSkin.Controls.MaterialDivider();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.matReindex = new MaterialSkin.Controls.MaterialCheckBox();
            this.configuracaoModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.matVacuum = new MaterialSkin.Controls.MaterialCheckBox();
            this.matBackup = new MaterialSkin.Controls.MaterialCheckBox();
            this.matLabelMesage = new MaterialSkin.Controls.MaterialLabel();
            this.abaConfiguracoesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.matBtnTestarConexao = new MaterialSkin.Controls.MaterialFlatButton();
            this.matBtnSalvarConfig = new MaterialSkin.Controls.MaterialFlatButton();
            this.materialCheckBox1 = new MaterialSkin.Controls.MaterialCheckBox();
            this.matPostoWeb = new MaterialSkin.Controls.MaterialCheckBox();
            this.matBtnPostgreSqlDir = new MaterialSkin.Controls.MaterialRaisedButton();
            this.matDirPostgreSql = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.matBtnSistemDir = new MaterialSkin.Controls.MaterialRaisedButton();
            this.matSistemaDir = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.matSenha = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.matUsuario = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.matBanco = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.matPorta = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.matServer = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.atualizarModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.abaAtualizacaoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.materialTabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barraBindingSource)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.configuracaoModelBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.abaConfiguracoesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.atualizarModelBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.abaAtualizacaoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // materialTabSelector1
            // 
            this.materialTabSelector1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialTabSelector1.BaseTabControl = this.materialTabControl1;
            this.materialTabSelector1.Depth = 0;
            this.materialTabSelector1.Location = new System.Drawing.Point(-2, 64);
            this.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabSelector1.Name = "materialTabSelector1";
            this.materialTabSelector1.Size = new System.Drawing.Size(841, 48);
            this.materialTabSelector1.TabIndex = 1;
            this.materialTabSelector1.Text = "materialTabSelector1";
            // 
            // materialTabControl1
            // 
            this.materialTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialTabControl1.Controls.Add(this.tabPage2);
            this.materialTabControl1.Controls.Add(this.tabPage3);
            this.materialTabControl1.Depth = 0;
            this.materialTabControl1.Location = new System.Drawing.Point(12, 118);
            this.materialTabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabControl1.Name = "materialTabControl1";
            this.materialTabControl1.SelectedIndex = 0;
            this.materialTabControl1.Size = new System.Drawing.Size(814, 359);
            this.materialTabControl1.TabIndex = 2;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.matLabVerAtu);
            this.tabPage2.Controls.Add(this.matLabelUlt);
            this.tabPage2.Controls.Add(this.matLabelProx);
            this.tabPage2.Controls.Add(this.materialLabel3);
            this.tabPage2.Controls.Add(this.materialLabel2);
            this.tabPage2.Controls.Add(this.materialLabel1);
            this.tabPage2.Controls.Add(this.matBarraProgresso2);
            this.tabPage2.Controls.Add(this.matBarraProgresso1);
            this.tabPage2.Controls.Add(this.matBtnAtualizar);
            this.tabPage2.Controls.Add(this.matBtnIniciar);
            this.tabPage2.Controls.Add(this.materialDivider1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(806, 333);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Atualização";
            // 
            // matLabVerAtu
            // 
            this.matLabVerAtu.AutoSize = true;
            this.matLabVerAtu.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.atualizarModelBindingSource, "Versao", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.matLabVerAtu.Depth = 0;
            this.matLabVerAtu.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.matLabVerAtu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.matLabVerAtu.Location = new System.Drawing.Point(232, 139);
            this.matLabVerAtu.MouseState = MaterialSkin.MouseState.HOVER;
            this.matLabVerAtu.Name = "matLabVerAtu";
            this.matLabVerAtu.Size = new System.Drawing.Size(12, 18);
            this.matLabVerAtu.TabIndex = 13;
            this.matLabVerAtu.Text = " ";
            // 
            // matLabelUlt
            // 
            this.matLabelUlt.AutoSize = true;
            this.matLabelUlt.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.atualizarModelBindingSource, "UltimaData", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.matLabelUlt.Depth = 0;
            this.matLabelUlt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.matLabelUlt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.matLabelUlt.Location = new System.Drawing.Point(232, 105);
            this.matLabelUlt.MouseState = MaterialSkin.MouseState.HOVER;
            this.matLabelUlt.Name = "matLabelUlt";
            this.matLabelUlt.Size = new System.Drawing.Size(12, 18);
            this.matLabelUlt.TabIndex = 12;
            this.matLabelUlt.Text = " ";
            // 
            // matLabelProx
            // 
            this.matLabelProx.AutoSize = true;
            this.matLabelProx.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.atualizarModelBindingSource, "GetDataProximaAtualizacao", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.matLabelProx.Depth = 0;
            this.matLabelProx.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.matLabelProx.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.matLabelProx.Location = new System.Drawing.Point(232, 72);
            this.matLabelProx.MouseState = MaterialSkin.MouseState.HOVER;
            this.matLabelProx.Name = "matLabelProx";
            this.matLabelProx.Size = new System.Drawing.Size(12, 18);
            this.matLabelProx.TabIndex = 11;
            this.matLabelProx.Text = " ";
            // 
            // materialLabel3
            // 
            this.materialLabel3.AutoSize = true;
            this.materialLabel3.Depth = 0;
            this.materialLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.materialLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel3.Location = new System.Drawing.Point(126, 139);
            this.materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            this.materialLabel3.Size = new System.Drawing.Size(95, 18);
            this.materialLabel3.TabIndex = 10;
            this.materialLabel3.Text = "Versão Atual:";
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(169, 105);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(54, 18);
            this.materialLabel2.TabIndex = 9;
            this.materialLabel2.Text = "Última:";
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(158, 72);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(67, 18);
            this.materialLabel1.TabIndex = 8;
            this.materialLabel1.Text = "Próxima:";
            // 
            // matBarraProgresso2
            // 
            this.matBarraProgresso2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.barraBindingSource, "ProgressoBarra2", true));
            this.matBarraProgresso2.Depth = 0;
            this.matBarraProgresso2.Location = new System.Drawing.Point(23, 315);
            this.matBarraProgresso2.MouseState = MaterialSkin.MouseState.HOVER;
            this.matBarraProgresso2.Name = "matBarraProgresso2";
            this.matBarraProgresso2.Size = new System.Drawing.Size(541, 5);
            this.matBarraProgresso2.TabIndex = 7;
            // 
            // matBarraProgresso1
            // 
            this.matBarraProgresso1.Depth = 0;
            this.matBarraProgresso1.Location = new System.Drawing.Point(23, 295);
            this.matBarraProgresso1.MouseState = MaterialSkin.MouseState.HOVER;
            this.matBarraProgresso1.Name = "matBarraProgresso1";
            this.matBarraProgresso1.Size = new System.Drawing.Size(541, 5);
            this.matBarraProgresso1.TabIndex = 6;
            // 
            // matBtnAtualizar
            // 
            this.matBtnAtualizar.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.matBtnAtualizar.AutoSize = true;
            this.matBtnAtualizar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.matBtnAtualizar.Depth = 0;
            this.matBtnAtualizar.Icon = null;
            this.matBtnAtualizar.Location = new System.Drawing.Point(617, 288);
            this.matBtnAtualizar.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.matBtnAtualizar.MouseState = MaterialSkin.MouseState.HOVER;
            this.matBtnAtualizar.Name = "matBtnAtualizar";
            this.matBtnAtualizar.Primary = true;
            this.matBtnAtualizar.Size = new System.Drawing.Size(94, 36);
            this.matBtnAtualizar.TabIndex = 5;
            this.matBtnAtualizar.Text = "Atualizar";
            this.matBtnAtualizar.UseVisualStyleBackColor = true;
            // 
            // matBtnIniciar
            // 
            this.matBtnIniciar.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.matBtnIniciar.AutoSize = true;
            this.matBtnIniciar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.matBtnIniciar.Depth = 0;
            this.matBtnIniciar.Icon = null;
            this.matBtnIniciar.Location = new System.Drawing.Point(719, 288);
            this.matBtnIniciar.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.matBtnIniciar.MouseState = MaterialSkin.MouseState.HOVER;
            this.matBtnIniciar.Name = "matBtnIniciar";
            this.matBtnIniciar.Primary = true;
            this.matBtnIniciar.Size = new System.Drawing.Size(69, 36);
            this.matBtnIniciar.TabIndex = 4;
            this.matBtnIniciar.Text = "Iniciar";
            this.matBtnIniciar.UseVisualStyleBackColor = true;
            // 
            // materialDivider1
            // 
            this.materialDivider1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialDivider1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialDivider1.Depth = 0;
            this.materialDivider1.Location = new System.Drawing.Point(102, 49);
            this.materialDivider1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider1.Name = "materialDivider1";
            this.materialDivider1.Size = new System.Drawing.Size(600, 166);
            this.materialDivider1.TabIndex = 3;
            this.materialDivider1.Text = "materialDivider1";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.White;
            this.tabPage3.Controls.Add(this.matReindex);
            this.tabPage3.Controls.Add(this.matVacuum);
            this.tabPage3.Controls.Add(this.matBackup);
            this.tabPage3.Controls.Add(this.matLabelMesage);
            this.tabPage3.Controls.Add(this.matBtnTestarConexao);
            this.tabPage3.Controls.Add(this.matBtnSalvarConfig);
            this.tabPage3.Controls.Add(this.materialCheckBox1);
            this.tabPage3.Controls.Add(this.matPostoWeb);
            this.tabPage3.Controls.Add(this.matBtnPostgreSqlDir);
            this.tabPage3.Controls.Add(this.matDirPostgreSql);
            this.tabPage3.Controls.Add(this.matBtnSistemDir);
            this.tabPage3.Controls.Add(this.matSistemaDir);
            this.tabPage3.Controls.Add(this.matSenha);
            this.tabPage3.Controls.Add(this.matUsuario);
            this.tabPage3.Controls.Add(this.matBanco);
            this.tabPage3.Controls.Add(this.matPorta);
            this.tabPage3.Controls.Add(this.matServer);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(806, 333);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Configurações";
            // 
            // matReindex
            // 
            this.matReindex.AutoSize = true;
            this.matReindex.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.configuracaoModelBindingSource, "Reindex", true));
            this.matReindex.Depth = 0;
            this.matReindex.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.matReindex.Location = new System.Drawing.Point(547, 177);
            this.matReindex.Margin = new System.Windows.Forms.Padding(0);
            this.matReindex.MouseLocation = new System.Drawing.Point(-1, -1);
            this.matReindex.MouseState = MaterialSkin.MouseState.HOVER;
            this.matReindex.Name = "matReindex";
            this.matReindex.Ripple = true;
            this.matReindex.Size = new System.Drawing.Size(117, 30);
            this.matReindex.TabIndex = 19;
            this.matReindex.Text = "Fazer Reindex";
            this.matReindex.UseVisualStyleBackColor = true;
            // 
            // matVacuum
            // 
            this.matVacuum.AutoSize = true;
            this.matVacuum.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.configuracaoModelBindingSource, "Vacuum", true));
            this.matVacuum.Depth = 0;
            this.matVacuum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.matVacuum.Location = new System.Drawing.Point(375, 207);
            this.matVacuum.Margin = new System.Windows.Forms.Padding(0);
            this.matVacuum.MouseLocation = new System.Drawing.Point(-1, -1);
            this.matVacuum.MouseState = MaterialSkin.MouseState.HOVER;
            this.matVacuum.Name = "matVacuum";
            this.matVacuum.Ripple = true;
            this.matVacuum.Size = new System.Drawing.Size(118, 30);
            this.matVacuum.TabIndex = 18;
            this.matVacuum.Text = "Fazer Vacuum";
            this.matVacuum.UseVisualStyleBackColor = true;
            // 
            // matBackup
            // 
            this.matBackup.AutoSize = true;
            this.matBackup.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.configuracaoModelBindingSource, "Backup", true));
            this.matBackup.Depth = 0;
            this.matBackup.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.matBackup.Location = new System.Drawing.Point(375, 177);
            this.matBackup.Margin = new System.Windows.Forms.Padding(0);
            this.matBackup.MouseLocation = new System.Drawing.Point(-1, -1);
            this.matBackup.MouseState = MaterialSkin.MouseState.HOVER;
            this.matBackup.Name = "matBackup";
            this.matBackup.Ripple = true;
            this.matBackup.Size = new System.Drawing.Size(113, 30);
            this.matBackup.TabIndex = 17;
            this.matBackup.Text = "Fazer Backup";
            this.matBackup.UseVisualStyleBackColor = true;
            // 
            // matLabelMesage
            // 
            this.matLabelMesage.AutoSize = true;
            this.matLabelMesage.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.abaConfiguracoesBindingSource, "MensagemLabel", true));
            this.matLabelMesage.Depth = 0;
            this.matLabelMesage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.matLabelMesage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.matLabelMesage.Location = new System.Drawing.Point(15, 296);
            this.matLabelMesage.MouseState = MaterialSkin.MouseState.HOVER;
            this.matLabelMesage.Name = "matLabelMesage";
            this.matLabelMesage.Size = new System.Drawing.Size(0, 18);
            this.matLabelMesage.TabIndex = 16;
            // 
            // matBtnTestarConexao
            // 
            this.matBtnTestarConexao.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.matBtnTestarConexao.AutoSize = true;
            this.matBtnTestarConexao.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.matBtnTestarConexao.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.abaConfiguracoesBindingSource, "EnableButtonConfiguracao", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.matBtnTestarConexao.Depth = 0;
            this.matBtnTestarConexao.Icon = null;
            this.matBtnTestarConexao.Location = new System.Drawing.Point(567, 288);
            this.matBtnTestarConexao.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.matBtnTestarConexao.MouseState = MaterialSkin.MouseState.HOVER;
            this.matBtnTestarConexao.Name = "matBtnTestarConexao";
            this.matBtnTestarConexao.Primary = false;
            this.matBtnTestarConexao.Size = new System.Drawing.Size(138, 36);
            this.matBtnTestarConexao.TabIndex = 15;
            this.matBtnTestarConexao.Text = "Testar Conexão";
            this.matBtnTestarConexao.UseVisualStyleBackColor = true;
            this.matBtnTestarConexao.Click += new System.EventHandler(this.MatBtnTestarConexao_Click);
            // 
            // matBtnSalvarConfig
            // 
            this.matBtnSalvarConfig.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.matBtnSalvarConfig.AutoSize = true;
            this.matBtnSalvarConfig.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.matBtnSalvarConfig.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.abaConfiguracoesBindingSource, "EnableButtonConfiguracao", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.matBtnSalvarConfig.Depth = 0;
            this.matBtnSalvarConfig.Icon = null;
            this.matBtnSalvarConfig.Location = new System.Drawing.Point(713, 288);
            this.matBtnSalvarConfig.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.matBtnSalvarConfig.MouseState = MaterialSkin.MouseState.HOVER;
            this.matBtnSalvarConfig.Name = "matBtnSalvarConfig";
            this.matBtnSalvarConfig.Primary = true;
            this.matBtnSalvarConfig.Size = new System.Drawing.Size(72, 36);
            this.matBtnSalvarConfig.TabIndex = 14;
            this.matBtnSalvarConfig.Text = "Salvar";
            this.matBtnSalvarConfig.UseVisualStyleBackColor = true;
            this.matBtnSalvarConfig.Click += new System.EventHandler(this.MatBtnSalvarConfig_Click);
            // 
            // materialCheckBox1
            // 
            this.materialCheckBox1.AutoSize = true;
            this.materialCheckBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.configuracaoModelBindingSource, "PostoWeb", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.materialCheckBox1.Depth = 0;
            this.materialCheckBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.materialCheckBox1.Location = new System.Drawing.Point(19, 207);
            this.materialCheckBox1.Margin = new System.Windows.Forms.Padding(0);
            this.materialCheckBox1.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialCheckBox1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckBox1.Name = "materialCheckBox1";
            this.materialCheckBox1.Ripple = true;
            this.materialCheckBox1.Size = new System.Drawing.Size(286, 30);
            this.materialCheckBox1.TabIndex = 13;
            this.materialCheckBox1.Text = "Encerrar Posto web durante a atualização";
            this.materialCheckBox1.UseVisualStyleBackColor = true;
            // 
            // matPostoWeb
            // 
            this.matPostoWeb.AutoSize = true;
            this.matPostoWeb.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.configuracaoModelBindingSource, "LeitorBomba", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.matPostoWeb.Depth = 0;
            this.matPostoWeb.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.matPostoWeb.Location = new System.Drawing.Point(19, 177);
            this.matPostoWeb.Margin = new System.Windows.Forms.Padding(0);
            this.matPostoWeb.MouseLocation = new System.Drawing.Point(-1, -1);
            this.matPostoWeb.MouseState = MaterialSkin.MouseState.HOVER;
            this.matPostoWeb.Name = "matPostoWeb";
            this.matPostoWeb.Ripple = true;
            this.matPostoWeb.Size = new System.Drawing.Size(317, 30);
            this.matPostoWeb.TabIndex = 12;
            this.matPostoWeb.Text = "Encerrar Leitor de bombas durante atualização";
            this.matPostoWeb.UseVisualStyleBackColor = true;
            // 
            // matBtnPostgreSqlDir
            // 
            this.matBtnPostgreSqlDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.matBtnPostgreSqlDir.AutoSize = true;
            this.matBtnPostgreSqlDir.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.matBtnPostgreSqlDir.Depth = 0;
            this.matBtnPostgreSqlDir.Icon = null;
            this.matBtnPostgreSqlDir.Location = new System.Drawing.Point(753, 136);
            this.matBtnPostgreSqlDir.MaximumSize = new System.Drawing.Size(32, 32);
            this.matBtnPostgreSqlDir.MinimumSize = new System.Drawing.Size(32, 32);
            this.matBtnPostgreSqlDir.MouseState = MaterialSkin.MouseState.HOVER;
            this.matBtnPostgreSqlDir.Name = "matBtnPostgreSqlDir";
            this.matBtnPostgreSqlDir.Primary = true;
            this.matBtnPostgreSqlDir.Size = new System.Drawing.Size(32, 32);
            this.matBtnPostgreSqlDir.TabIndex = 11;
            this.matBtnPostgreSqlDir.Text = "...";
            this.matBtnPostgreSqlDir.UseVisualStyleBackColor = true;
            this.matBtnPostgreSqlDir.Click += new System.EventHandler(this.MatBtnPostgreSqlDir_Click);
            // 
            // matDirPostgreSql
            // 
            this.matDirPostgreSql.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.matDirPostgreSql.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configuracaoModelBindingSource, "DiretorioPostgreSql", true));
            this.matDirPostgreSql.Depth = 0;
            this.matDirPostgreSql.Hint = "Diretório da pasta bin do PosgreSQL";
            this.matDirPostgreSql.Location = new System.Drawing.Point(19, 145);
            this.matDirPostgreSql.MaxLength = 32767;
            this.matDirPostgreSql.MouseState = MaterialSkin.MouseState.HOVER;
            this.matDirPostgreSql.Name = "matDirPostgreSql";
            this.matDirPostgreSql.PasswordChar = '\0';
            this.matDirPostgreSql.SelectedText = "";
            this.matDirPostgreSql.SelectionLength = 0;
            this.matDirPostgreSql.SelectionStart = 0;
            this.matDirPostgreSql.Size = new System.Drawing.Size(726, 23);
            this.matDirPostgreSql.TabIndex = 10;
            this.matDirPostgreSql.TabStop = false;
            this.matDirPostgreSql.UseSystemPasswordChar = false;
            // 
            // matBtnSistemDir
            // 
            this.matBtnSistemDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.matBtnSistemDir.AutoSize = true;
            this.matBtnSistemDir.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.matBtnSistemDir.Depth = 0;
            this.matBtnSistemDir.Icon = null;
            this.matBtnSistemDir.Location = new System.Drawing.Point(753, 93);
            this.matBtnSistemDir.MaximumSize = new System.Drawing.Size(32, 32);
            this.matBtnSistemDir.MinimumSize = new System.Drawing.Size(32, 32);
            this.matBtnSistemDir.MouseState = MaterialSkin.MouseState.HOVER;
            this.matBtnSistemDir.Name = "matBtnSistemDir";
            this.matBtnSistemDir.Primary = true;
            this.matBtnSistemDir.Size = new System.Drawing.Size(32, 32);
            this.matBtnSistemDir.TabIndex = 9;
            this.matBtnSistemDir.Text = "...";
            this.matBtnSistemDir.UseVisualStyleBackColor = true;
            this.matBtnSistemDir.Click += new System.EventHandler(this.MatBtnSistemDir_Click);
            // 
            // matSistemaDir
            // 
            this.matSistemaDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.matSistemaDir.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configuracaoModelBindingSource, "DiretorioSistema", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.matSistemaDir.Depth = 0;
            this.matSistemaDir.Hint = "Diretório do sistema";
            this.matSistemaDir.Location = new System.Drawing.Point(19, 102);
            this.matSistemaDir.MaxLength = 32767;
            this.matSistemaDir.MouseState = MaterialSkin.MouseState.HOVER;
            this.matSistemaDir.Name = "matSistemaDir";
            this.matSistemaDir.PasswordChar = '\0';
            this.matSistemaDir.SelectedText = "";
            this.matSistemaDir.SelectionLength = 0;
            this.matSistemaDir.SelectionStart = 0;
            this.matSistemaDir.Size = new System.Drawing.Size(726, 23);
            this.matSistemaDir.TabIndex = 8;
            this.matSistemaDir.TabStop = false;
            this.matSistemaDir.UseSystemPasswordChar = false;
            // 
            // matSenha
            // 
            this.matSenha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.matSenha.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configuracaoModelBindingSource, "Senha", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.matSenha.Depth = 0;
            this.matSenha.Hint = "Senha";
            this.matSenha.Location = new System.Drawing.Point(496, 59);
            this.matSenha.MaxLength = 32767;
            this.matSenha.MinimumSize = new System.Drawing.Size(216, 0);
            this.matSenha.MouseState = MaterialSkin.MouseState.HOVER;
            this.matSenha.Name = "matSenha";
            this.matSenha.PasswordChar = '*';
            this.matSenha.SelectedText = "";
            this.matSenha.SelectionLength = 0;
            this.matSenha.SelectionStart = 0;
            this.matSenha.Size = new System.Drawing.Size(289, 23);
            this.matSenha.TabIndex = 7;
            this.matSenha.TabStop = false;
            this.matSenha.UseSystemPasswordChar = false;
            // 
            // matUsuario
            // 
            this.matUsuario.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.matUsuario.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configuracaoModelBindingSource, "Usuario", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.matUsuario.Depth = 0;
            this.matUsuario.Hint = "Usuário";
            this.matUsuario.Location = new System.Drawing.Point(19, 57);
            this.matUsuario.MaxLength = 32767;
            this.matUsuario.MouseState = MaterialSkin.MouseState.HOVER;
            this.matUsuario.Name = "matUsuario";
            this.matUsuario.PasswordChar = '\0';
            this.matUsuario.SelectedText = "";
            this.matUsuario.SelectionLength = 0;
            this.matUsuario.SelectionStart = 0;
            this.matUsuario.Size = new System.Drawing.Size(463, 23);
            this.matUsuario.TabIndex = 6;
            this.matUsuario.TabStop = false;
            this.matUsuario.UseSystemPasswordChar = false;
            // 
            // matBanco
            // 
            this.matBanco.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.matBanco.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configuracaoModelBindingSource, "Banco", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.matBanco.Depth = 0;
            this.matBanco.Hint = "Banco";
            this.matBanco.Location = new System.Drawing.Point(496, 22);
            this.matBanco.MaxLength = 32767;
            this.matBanco.MinimumSize = new System.Drawing.Size(216, 0);
            this.matBanco.MouseState = MaterialSkin.MouseState.HOVER;
            this.matBanco.Name = "matBanco";
            this.matBanco.PasswordChar = '\0';
            this.matBanco.SelectedText = "";
            this.matBanco.SelectionLength = 0;
            this.matBanco.SelectionStart = 0;
            this.matBanco.Size = new System.Drawing.Size(289, 23);
            this.matBanco.TabIndex = 5;
            this.matBanco.TabStop = false;
            this.matBanco.UseSystemPasswordChar = false;
            // 
            // matPorta
            // 
            this.matPorta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.matPorta.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configuracaoModelBindingSource, "Porta", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, null, "0"));
            this.matPorta.Depth = 0;
            this.matPorta.Hint = "Porta";
            this.matPorta.Location = new System.Drawing.Point(255, 22);
            this.matPorta.MaxLength = 32767;
            this.matPorta.MinimumSize = new System.Drawing.Size(185, 0);
            this.matPorta.MouseState = MaterialSkin.MouseState.HOVER;
            this.matPorta.Name = "matPorta";
            this.matPorta.PasswordChar = '\0';
            this.matPorta.SelectedText = "";
            this.matPorta.SelectionLength = 0;
            this.matPorta.SelectionStart = 0;
            this.matPorta.Size = new System.Drawing.Size(227, 23);
            this.matPorta.TabIndex = 4;
            this.matPorta.TabStop = false;
            this.matPorta.UseSystemPasswordChar = false;
            // 
            // matServer
            // 
            this.matServer.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configuracaoModelBindingSource, "Servidor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.matServer.Depth = 0;
            this.matServer.Hint = "Servidor";
            this.matServer.Location = new System.Drawing.Point(19, 22);
            this.matServer.MaxLength = 32767;
            this.matServer.MinimumSize = new System.Drawing.Size(185, 0);
            this.matServer.MouseState = MaterialSkin.MouseState.HOVER;
            this.matServer.Name = "matServer";
            this.matServer.PasswordChar = '\0';
            this.matServer.SelectedText = "";
            this.matServer.SelectionLength = 0;
            this.matServer.SelectionStart = 0;
            this.matServer.Size = new System.Drawing.Size(227, 23);
            this.matServer.TabIndex = 3;
            this.matServer.TabStop = false;
            this.matServer.UseSystemPasswordChar = false;
            // 
            // atualizarModelBindingSource
            // 
            this.atualizarModelBindingSource.DataSource = typeof(Atualizador.Models.AtualizarModel);
            // 
            // abaAtualizacaoBindingSource
            // 
            this.abaAtualizacaoBindingSource.DataSource = typeof(Atualizador.Structures.AbaAtualizacao);
            // 
            // TelaPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Atualizador.Properties.Resources.background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(838, 489);
            this.Controls.Add(this.materialTabControl1);
            this.Controls.Add(this.materialTabSelector1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(838, 489);
            this.Name = "TelaPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Atualizador - Controle de atualizações do servidor ";
            this.materialTabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barraBindingSource)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.configuracaoModelBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.abaConfiguracoesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.atualizarModelBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.abaAtualizacaoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource configuracaoModelBindingSource;
        private System.Windows.Forms.BindingSource abaConfiguracoesBindingSource;
        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector1;
        private MaterialSkin.Controls.MaterialTabControl materialTabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private MaterialSkin.Controls.MaterialFlatButton matBtnIniciar;
        private MaterialSkin.Controls.MaterialDivider materialDivider1;
        private System.Windows.Forms.TabPage tabPage3;
        private MaterialSkin.Controls.MaterialSingleLineTextField matServer;
        private MaterialSkin.Controls.MaterialSingleLineTextField matPorta;
        private MaterialSkin.Controls.MaterialSingleLineTextField matUsuario;
        private MaterialSkin.Controls.MaterialSingleLineTextField matBanco;
        private MaterialSkin.Controls.MaterialRaisedButton matBtnSistemDir;
        private MaterialSkin.Controls.MaterialSingleLineTextField matSistemaDir;
        private MaterialSkin.Controls.MaterialSingleLineTextField matSenha;
        private MaterialSkin.Controls.MaterialSingleLineTextField matDirPostgreSql;
        private MaterialSkin.Controls.MaterialRaisedButton matBtnPostgreSqlDir;
        private MaterialSkin.Controls.MaterialCheckBox materialCheckBox1;
        private MaterialSkin.Controls.MaterialCheckBox matPostoWeb;
        private MaterialSkin.Controls.MaterialFlatButton matBtnTestarConexao;
        private MaterialSkin.Controls.MaterialFlatButton matBtnSalvarConfig;
        private MaterialSkin.Controls.MaterialLabel matLabelMesage;
        private MaterialSkin.Controls.MaterialCheckBox matVacuum;
        private MaterialSkin.Controls.MaterialCheckBox matBackup;
        private MaterialSkin.Controls.MaterialCheckBox matReindex;
        private MaterialSkin.Controls.MaterialFlatButton matBtnAtualizar;
        private MaterialSkin.Controls.MaterialProgressBar matBarraProgresso2;
        private MaterialSkin.Controls.MaterialProgressBar matBarraProgresso1;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private System.Windows.Forms.BindingSource barraBindingSource;
        private MaterialSkin.Controls.MaterialLabel matLabVerAtu;
        private MaterialSkin.Controls.MaterialLabel matLabelUlt;
        private MaterialSkin.Controls.MaterialLabel matLabelProx;
        private System.Windows.Forms.BindingSource atualizarModelBindingSource;
        private System.Windows.Forms.BindingSource abaAtualizacaoBindingSource;
    }
}

