﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.ViewModel;
using Posto.Win.Update.Model;
using Posto.Win.Update.Infraestrutura;
using System.Windows;

namespace Posto.Win.Update.Abas
{
    public class AbaAtualizar : NotificationObject
    {
        #region Propriedades

        private AtualizarModel _atualizar;
        private Status _status;
        private bool _isVisibleButtonPausar;
        private bool _isEnableButtonAtualizar;
        private Visibility _botaoBloquear;
        private Visibility _botaoDesbloquear;

        #endregion

        #region Construtor

        public AbaAtualizar()
        {
            AtualizarModel = new AtualizarModel();
            Status = new Status();
            IsVisibleButtonPausar = false;
            IsEnableButtonAtualizar = true;
            BotaoBloquear = Visibility.Hidden;
            BotaoDesbloquear = Visibility.Visible;
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
                    RaisePropertyChanged(() => AtualizarModel);
                }
            }
        }
        public Status Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    RaisePropertyChanged(() => Status);
                }
            }
        }

        #endregion

        #region Funções

        public bool IsVisibleButtonPausar
        {
            get
            {
                return _isVisibleButtonPausar;
            }
            set
            {
                if (_isVisibleButtonPausar != value)
                {
                    _isVisibleButtonPausar = value;
                    RaisePropertyChanged(() => IsVisibleButtonPausar);
                }
            }
        }
        public bool IsEnableButtonAtualizar
        {
            get
            {
                return _isEnableButtonAtualizar;
            }
            set
            {
                if (_isEnableButtonAtualizar != value)
                {
                    _isEnableButtonAtualizar = value;
                    RaisePropertyChanged(() => IsEnableButtonAtualizar);
                }
            }
        }

        public Visibility BotaoBloquear
        {
            get
            {
                return _botaoBloquear;
            }
            set
            {
                if (_botaoBloquear != value)
                {
                    _botaoBloquear = value;
                    RaisePropertyChanged(() => BotaoBloquear);
                }
            }
        }

        public Visibility BotaoDesbloquear
        {
            get
            {
                return _botaoDesbloquear;
            }
            set
            {
                if (_botaoDesbloquear != value)
                {
                    _botaoDesbloquear = value;
                    RaisePropertyChanged(() => BotaoDesbloquear);
                }
            }
        }

        #endregion

    }
}
