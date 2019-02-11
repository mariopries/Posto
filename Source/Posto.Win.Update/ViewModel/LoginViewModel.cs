using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posto.Win.Update.ViewModel
{
    public class LoginViewModel : NotificationObject
    {
        #region Propriedades

        private DelegateCommand _command, _command2;
        private DelegateCommand fInputSenhaIsValidCommand;
        private string _inputLogin;
        private string _mensagemStatus;

        #endregion

        public LoginViewModel(DelegateCommand command1, DelegateCommand command2) 
        {
            this.Comando1 = command1;
            this.Comando2 = command2;

            this.LogarCommand = new DelegateCommand(OnLogar);
            this.CancelarCommand = new DelegateCommand(OnCancelar);
        }

        #region Objetos

        public DelegateCommand Comando1
        {
            get
            {
                return _command;
            }
            set
            {
                if(_command != value)
                {
                    _command = value;
                    RaisePropertyChanged(() => Comando1);
                }
            }
        }
        public DelegateCommand Comando2
        {
            get
            {
                return _command2;
            }
            set
            {
                if (_command2 != value)
                {
                    _command2 = value;
                    RaisePropertyChanged(() => Comando2);
                }
            }
        }
        public DelegateCommand InputSenhaIsValidCommand
        {
            get
            {
                if (this.fInputSenhaIsValidCommand == null)
                {
                    this.fInputSenhaIsValidCommand = new DelegateCommand(this.InputSenhaIsValidExecuted);
                }

                return this.fInputSenhaIsValidCommand;
            }
        }

        #endregion

        #region Input

        public string InputLogin
        {
            get
            {
                return this._inputLogin;
            }
            set
            {
                if (this._inputLogin != value)
                {
                    this._inputLogin = value;
                    this.RaisePropertyChanged(() => this.InputLogin);
                }
            }
        }

        public string MensagemStatus
        {
            get
            {
                return this._mensagemStatus;
            }
            set
            {
                if (this._mensagemStatus != value)
                {
                    this._mensagemStatus = value;
                    this.RaisePropertyChanged(() => this.MensagemStatus);
                }
            }
        }

        #endregion

        #region Commands

        public DelegateCommand LogarCommand { get; set; }
        public DelegateCommand CancelarCommand { get; set; }
        
        #endregion

        #region Helpers

        private void InputSenhaIsValidExecuted()
        {
            if (!string.IsNullOrEmpty(this.InputLogin))
            {
                this.InputLogin = this.InputLogin;
            }
        }
        private void OnCancelar()
        {
            this.Comando2.Execute();
        }
        private void OnLogar()
        {
            InputSenhaIsValidExecuted();
            string senhaEsperada = null;

            var data = DateTime.Now;
            senhaEsperada = string.Format("{0}{1}{2}", data.Day + data.Hour, data.Day + data.Month, data.Day + int.Parse(data.Year.ToString().Substring(2, 2)));

            if (senhaEsperada == this.InputLogin)
            {
                this.Comando1.Execute();
            }
            else
            {
                this.MensagemStatus = "Login inválido!";
            } 
        }
        #endregion
    }
}
