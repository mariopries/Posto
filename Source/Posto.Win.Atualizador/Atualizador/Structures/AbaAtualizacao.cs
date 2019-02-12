using Atualizador.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atualizador.Structures
{
    class AbaAtualizacao : INotifyPropertyChanged
    {
        #region Propriedades

        private AtualizarModel _atualizar;
        private Status _status;
        private bool _isVisibleButtonPausar;
        private bool _isEnableButtonAtualizar;
        private bool _botaoBloquear;
        private bool _botaoDesbloquear;

        #endregion

        #region Construtor

        public AbaAtualizacao()
        {
            AtualizarModel = new AtualizarModel();
            Status = new Status();
            IsVisibleButtonPausar = false;
            IsEnableButtonAtualizar = true;
            BotaoBloquear = false;
            BotaoDesbloquear = true;
        }

        #endregion

        #region Objetos

        public AtualizarModel AtualizarModel
        {
            get { return _atualizar; }
            set { SetField(ref _atualizar, value); }
        }

        public Status Status
        {
            get { return _status; }
            set { SetField(ref _status, value); }
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
