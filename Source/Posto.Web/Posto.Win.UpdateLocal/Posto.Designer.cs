namespace Posto
{
    partial class Posto
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
            this.labelProcesso = new System.Windows.Forms.Label();
            this.labelTitulo = new System.Windows.Forms.Label();
            this.pictureBoxLoading = new System.Windows.Forms.PictureBox();
            this.buttonCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // labelProcesso
            // 
            this.labelProcesso.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProcesso.Location = new System.Drawing.Point(88, 49);
            this.labelProcesso.Name = "labelProcesso";
            this.labelProcesso.Size = new System.Drawing.Size(433, 29);
            this.labelProcesso.TabIndex = 8;
            this.labelProcesso.Text = "Testando conectividade com o servidor.";
            this.labelProcesso.Click += new System.EventHandler(this.labelProcesso_Click);
            // 
            // labelTitulo
            // 
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitulo.Location = new System.Drawing.Point(88, 31);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Size = new System.Drawing.Size(204, 14);
            this.labelTitulo.TabIndex = 7;
            this.labelTitulo.Text = "Aguarde, inicializando o Posto...";
            this.labelTitulo.Click += new System.EventHandler(this.labelTitulo_Click);
            // 
            // pictureBoxLoading
            // 
            this.pictureBoxLoading.Image = global::Posto.Properties.Resources.loading;
            this.pictureBoxLoading.Location = new System.Drawing.Point(14, 14);
            this.pictureBoxLoading.Name = "pictureBoxLoading";
            this.pictureBoxLoading.Size = new System.Drawing.Size(64, 64);
            this.pictureBoxLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxLoading.TabIndex = 6;
            this.pictureBoxLoading.TabStop = false;
            // 
            // buttonCancelar
            // 
            this.buttonCancelar.Location = new System.Drawing.Point(434, 98);
            this.buttonCancelar.Name = "buttonCancelar";
            this.buttonCancelar.Size = new System.Drawing.Size(87, 25);
            this.buttonCancelar.TabIndex = 5;
            this.buttonCancelar.Text = "Cancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            this.buttonCancelar.Click += new System.EventHandler(this.buttonCancelar_Click);
            // 
            // Posto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(572, 143);
            this.Controls.Add(this.labelProcesso);
            this.Controls.Add(this.labelTitulo);
            this.Controls.Add(this.pictureBoxLoading);
            this.Controls.Add(this.buttonCancelar);
            this.Name = "Posto";
            this.Text = "Iniciando o Posto";
            this.Load += new System.EventHandler(this.Posto_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelProcesso;
        private System.Windows.Forms.Label labelTitulo;
        private System.Windows.Forms.PictureBox pictureBoxLoading;
        private System.Windows.Forms.Button buttonCancelar;
    }
}