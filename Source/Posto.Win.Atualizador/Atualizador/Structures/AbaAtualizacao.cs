using Atualizador.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atualizador.Structures
{
    public class AbaAtualizacao : INotifyPropertyChanged
    {
        #region Propriedades

        private AtualizarModel _atualizar;
        private bool _isVisibleButtonPausar;
        private bool _isEnableButtonAtualizar;
        private bool _botaoBloquear;
        private bool _botaoDesbloquear;
        private bool _isEnabledBarras;
        private bool _isVisibleBarras;
        private int _progressbarraValue1;
        private int _progressbarraValue2;
        private bool _isIndeterminateBarra1;
        private string _labelcontent;
        private Point _labelLocation;

        #endregion

        #region Construtor

        public AbaAtualizacao()
        {
            AtualizarModel = new AtualizarModel();
            IsVisibleButtonPausar = false;
            IsEnableButtonAtualizar = true;
            BotaoBloquear = false;
            BotaoDesbloquear = true;

            //--Barras de progresso
            IsEnabledBarras = false;
            IsVisibleBarras = false;
            ProgressoBarra1 = 0;
            ProgressoBarra2 = 0;
            IsIndeterminateBarra1 = false;

            //--Label
            LabelContent = "Testandos";
            LabelLocation = new Point(20, 296); //new Point(20, 264);
        }

        #endregion

        #region Objetos

        public AtualizarModel AtualizarModel
        {
            get { return _atualizar; }
            set { SetField(ref _atualizar, value); }
        }

        public bool IsVisibleButtonPausar
        {
            get { return _isVisibleButtonPausar; }
            set { SetField(ref _isVisibleButtonPausar, value); }
        }

        public bool IsEnableButtonAtualizar
        {
            get { return _isEnableButtonAtualizar; }
            set { SetField(ref _isEnableButtonAtualizar, value); }
        }

        public bool BotaoBloquear
        {
            get { return _botaoBloquear; }
            set { SetField(ref _botaoBloquear, value); }
        }

        public bool BotaoDesbloquear
        {
            get { return _botaoDesbloquear; }
            set { SetField(ref _botaoDesbloquear, value); }
        }

        public bool IsEnabledBarras
        {
            get { return _isEnabledBarras; }
            set { SetField(ref _isEnabledBarras, value); }
        }

        public bool IsVisibleBarras
        {
            get { return _isVisibleBarras; }
            set { SetField(ref _isVisibleBarras, value); }
        }
        public bool IsIndeterminateBarra1
        {
            get { return _isIndeterminateBarra1; }
            set { SetField(ref _isIndeterminateBarra1, value); }
        }
        public int ProgressoBarra1
        {
            get { return _progressbarraValue1; }
            set { SetField(ref _progressbarraValue1, value); }
        }
        public int ProgressoBarra2
        {
            get { return _progressbarraValue2; }
            set { SetField(ref _progressbarraValue2, value); }
        }
        public string LabelContent
        {
            get { return _labelcontent; }
            set { SetField(ref _labelcontent, value); }
        }
        public Point LabelLocation
        {
            get { return _labelLocation; }
            set { SetField(ref _labelLocation, value); }
        }

        #endregion

        #region Eventos

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Funções

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected bool SetField<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}
