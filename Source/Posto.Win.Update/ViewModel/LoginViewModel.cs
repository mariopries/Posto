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
        private DelegateCommand _command;
        private string _inputLogin;
        private string _mensagemStatus;

        public LoginViewModel(DelegateCommand command) 
        {
            this._command = command;

            this.LogarCommand = new DelegateCommand(OnLogar);
        }

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

        #endregion

        #region Helpers

        private void OnLogar()
        {
            string senhaEsperada = null;

            var data = DateTime.Now;
            senhaEsperada = string.Format("{0}{1}{2}", data.Day + data.Hour, data.Day + data.Month, data.Day + int.Parse(data.Year.ToString().Substring(2, 2)));

            if (senhaEsperada == this.InputLogin)
            {
                this._command.Execute();
            }
            else
            {
                this.MensagemStatus = "Login inválido!";
            }
 
        }
        #endregion
    }
}
