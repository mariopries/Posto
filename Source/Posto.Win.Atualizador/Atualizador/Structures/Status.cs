using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atualizador.Structures
{
    public class Status : INotifyPropertyChanged
    {
        #region Propriedades

        private Barra _barraprogresso;
        private Label _statuslabel;

        #endregion

        #region Construtor

        public Status()
        {
            BarraProgresso = new Barra();
            StatusLabel = new Label();
        }

        #endregion

        #region Objetos

        public Barra BarraProgresso
        {
            get { return _barraprogresso; }
            set { SetField(ref _barraprogresso, value); }
        }
        public Label StatusLabel
        {
            get { return _statuslabel; }
            set { SetField(ref _statuslabel, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        #region Classes

        public class Barra : INotifyPropertyChanged
        {
            public Barra()
            {
                IsEnable = false;
                Visao = false;
                ProgressoBarra1 = 0;
                ProgressoBarra2 = 0;
                IsIndeterminateBarra1 = false;
            }

            #region Propriedades

            private bool _isEnable;
            private bool _visao;
            private double _progressbarra1;
            private double _progressbarra2;
            private bool _isIndeterminateBarra1;

            #endregion

            public bool IsEnable
            {
                get { return _isEnable; }
                set { SetField(ref _isEnable, value); }
            }

            public bool Visao
            {
                get { return _visao; }
                set { SetField(ref _visao, value); }
            }
            public bool IsIndeterminateBarra1
            {
                get { return _isIndeterminateBarra1; }
                set { SetField(ref _isIndeterminateBarra1, value); }
            }
            public double ProgressoBarra1
            {
                get { return _progressbarra1; }
                set { SetField(ref _progressbarra1, value); }
            }
            public double ProgressoBarra2
            {
                get { return _progressbarra2; }
                set { SetField(ref _progressbarra2, value); }
            }

            public event PropertyChangedEventHandler PropertyChanged;

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
        }

        public class Label : INotifyPropertyChanged
        {
            public Label()
            {
                //LabelMargin = new System.Windows.Thickness(10, 25, 10, 0);
                LabelContent = "";
            }

            #region Propriedades

            //private System.Windows.Thickness _labelmargin;
            private string _labelcontent;

            #endregion

            //public System.Windows.Thickness LabelMargin
            //{
            //    get
            //    {
            //        return _labelmargin;
            //    }
            //    set
            //    {
            //        if (_labelmargin != value)
            //        {
            //            _labelmargin = value;
            //            RaisePropertyChanged(() => LabelMargin);
            //        }
            //    }
            //}
            public string LabelContent
            {
                get { return _labelcontent; }
                set { SetField(ref _labelcontent, value); }
            }

            public event PropertyChangedEventHandler PropertyChanged;

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
        }

        #endregion
    }
}
