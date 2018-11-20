using Atualizador.Extensions;
using Atualizador.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atualizador.Structures
{
    class AbaConfiguracoes : INotifyPropertyChanged
    {
        #region Propriedades

        private ConfiguracaoModel _configuracoes;
        private bool _enableButtonConfiguracao;
        private string _mensagemlabel;

        #endregion

        #region Construtor

        public AbaConfiguracoes()
        {
            ConfiguracaoModel = ConfiguracaoXml.CarregarConfiguracao().ToModel();
            EnableButtonConfiguracao = true;
        }

        #endregion

        #region Objetos

        public ConfiguracaoModel ConfiguracaoModel
        {
            get
            {
                return _configuracoes;
            }
            set
            {
                if (_configuracoes != value)
                {
                    _configuracoes = value;
                }
            }
        }

        public bool EnableButtonConfiguracao
        {
            get { return _enableButtonConfiguracao; }
            set { SetField(ref _enableButtonConfiguracao, value); }
        }

        public string MensagemLabel
        {
            get { return _mensagemlabel; }
            set { SetField(ref _mensagemlabel, value); }
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

    }
}