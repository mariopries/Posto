using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posto.Win.Update.Infraestrutura
{
    public class Status : NotificationObject
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
            get
            {
                return _barraprogresso;
            }
            set
            {
                if (_barraprogresso != value)
                {
                    _barraprogresso = value;
                    RaisePropertyChanged(() => BarraProgresso);
                }
            }
        }
        public Label StatusLabel
        {
            get
            {
                return _statuslabel;
            }
            set
            {
                if (_statuslabel != value)
                {
                    _statuslabel = value;
                    RaisePropertyChanged(() => StatusLabel);
                }
            }
        }

        #endregion

        #region Classes

        public class Barra : NotificationObject
        {
            public Barra()
            {
                IsEnable = false;
                Visao = System.Windows.Visibility.Hidden;
                ProgressoBarra1 = 0;
                ProgressoBarra2 = 0;
            }

            #region Propriedades

            private bool _isEnable;
            private System.Windows.Visibility _visao;
            private double _progressbarra1;
            private double _progressbarra2;

            #endregion

            public bool IsEnable
            {
                get
                {
                    return _isEnable;
                }
                set
                {
                    if (_isEnable != value)
                    {
                        _isEnable = value;
                        RaisePropertyChanged(() => IsEnable);
                    }
                }
            }
            public System.Windows.Visibility Visao
            {
                get
                {
                    return _visao;
                }
                set
                {
                    if (_visao != value)
                    {
                        _visao = value;
                        RaisePropertyChanged(() => Visao);
                    }
                }
            }
            public double ProgressoBarra1
            {
                get
                {
                    return _progressbarra1;
                }
                set
                {
                    if (_progressbarra1 != value)
                    {
                        _progressbarra1 = value;
                        RaisePropertyChanged(() => ProgressoBarra1);
                    }
                }
            }
            public double ProgressoBarra2
            {
                get
                {
                    return _progressbarra2;
                }
                set
                {
                    if (_progressbarra2 != value)
                    {
                        _progressbarra2 = value;
                        RaisePropertyChanged(() => ProgressoBarra2);
                    }
                }
            }

        }
        public class Label : NotificationObject
        {
            public Label()
            {
                LabelMargin = new System.Windows.Thickness(10, 25, 10, 0);
                LabelContent = "";
            }

            #region Propriedades

            private System.Windows.Thickness _labelmargin;
            private string _labelcontent;

            #endregion

            public System.Windows.Thickness LabelMargin
            {
                get
                {
                    return _labelmargin;
                }
                set
                {
                    if (_labelmargin != value)
                    {
                        _labelmargin = value;
                        RaisePropertyChanged(() => LabelMargin);
                    }
                }
            }
            public string LabelContent
            {
                get
                {
                    return _labelcontent;
                }
                set
                {
                    if (_labelcontent != value)
                    {
                        _labelcontent = value;
                        RaisePropertyChanged(() => LabelContent);
                    }
                }
            }
        }

        #endregion
    }
}
