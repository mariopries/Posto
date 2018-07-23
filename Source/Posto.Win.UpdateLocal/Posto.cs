using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Posto.Win.UpdateLocal
{
    public partial class Posto : Form
    {
        #region Propriedades privadas

        private BackgroundWorker _bgWInicializador = new BackgroundWorker();
        private Aplicativo _parm;

        #endregion

        #region Contrutor e Load

        public Posto(Aplicativo parm)
        {
            this._parm = parm;
            InitializeComponent();

            //Instalador.InstallFonts();
            _bgWInicializador.WorkerReportsProgress = true;
            _bgWInicializador.DoWork             += new DoWorkEventHandler(fBgWInicializador_DoWork);
            _bgWInicializador.ProgressChanged    += new ProgressChangedEventHandler(fBgWInicializador_ProgressChanged);
            _bgWInicializador.RunWorkerCompleted += new RunWorkerCompletedEventHandler(fBgWInicializador_RunWorkerCompleted);
        }

        private void Posto_Load(object sender, EventArgs e)
        {
            _bgWInicializador.RunWorkerAsync();
        }

        #endregion

        #region Eventos
        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            Posto.ActiveForm.Close();
        }
        #endregion

        #region Controle de inicialização - Background

        void fBgWInicializador_DoWork(object sender, DoWorkEventArgs e)
        {
            Inicializador inicializador = new Inicializador();
            inicializador.Start(_bgWInicializador, _parm);
        }

        void fBgWInicializador_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                buttonCancelar.Text = "Fechar";

                labelProcesso.ForeColor = Color.Red;
                labelProcesso.Text = e.Error.Message;
                labelProcesso.Refresh();
            }
            else
            {
                labelProcesso.Text = "Processo finalizado com sucesso.";
                this.WindowState = FormWindowState.Minimized;
                Application.Exit();
            }
        }

        void fBgWInicializador_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e != null && e.UserState != null)
            {
                labelProcesso.Text = ((string)e.UserState).Trim();
            }
        }

        #endregion

        private void labelTitulo_Click(object sender, EventArgs e)
        {

        }

        private void labelProcesso_Click(object sender, EventArgs e)
        {

        }

    }
}
