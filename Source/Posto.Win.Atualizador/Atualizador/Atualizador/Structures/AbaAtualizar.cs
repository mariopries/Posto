using Atualizador.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atualizador.Structures
{
    class AbaAtualizar : INotifyPropertyChanged
    {
        #region Propriedades

        private AtualizarModel _atualizar;
        //private Status _status;
        private bool _isVisibleButtonPausar;
        private bool _isEnableButtonAtualizar;

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #region Construtor

        public AbaAtualizar()
        {
            AtualizarModel = new AtualizarModel();
            //Status = new Status();
            IsVisibleButtonPausar = false;
            IsEnableButtonAtualizar = true;
        }

        #endregion

        #region Objetos

        public AtualizarModel AtualizarModel
        {
            get
            {
                return _atualizar;
            }
            set
            {
                if (_atualizar != value)
                {
                    _atualizar = value;
                }
            }
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

        #endregion
    }
}
