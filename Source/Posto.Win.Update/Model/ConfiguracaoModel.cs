using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posto.Win.Update.Model
{
    public class ConfiguracaoModel : NotificationObject
    {
        private string  _servidor;
        private int     _porta;
        private string  _banco;
        private string  _usuario;
        private string  _senha;
        private string  _localDiretorio;
        private string  _mensagem;
        private bool    _leitor;
        private bool    _web;

        public string Servidor 
        {
            get
            {
                return this._servidor;
            }
            set
            {
                if (this._servidor != value)
                {
                    this._servidor = value;
                    this.RaisePropertyChanged(() => this.Servidor);
                }
            }
        }

        public int Porta
        {
            get
            {
                return this._porta;
            }
            set
            {
                if (this._porta != value)
                {
                    this._porta = value;
                    this.RaisePropertyChanged(() => this.Porta);
                }
            }
        }

        public string Banco
        {
            get
            {
                return this._banco;
            }
            set
            {
                if (this._banco != value)
                {
                    this._banco = value;
                    this.RaisePropertyChanged(() => this.Banco);
                }
            }
        }

        public string Usuario
        {
            get
            {
                return this._usuario;
            }
            set
            {
                if (this._usuario != value)
                {
                    this._usuario = value;
                    this.RaisePropertyChanged(() => this.Usuario);
                }
            }
        }

        public string Senha
        {
            get
            {
                return this._senha;
            }
            set
            {
                if (this._senha != value)
                {
                    this._senha = value;
                    this.RaisePropertyChanged(() => this.Senha);
                }
            }
        }

        public string LocalDiretorio
        {
            get
            {
                return this._localDiretorio;
            }
            set
            {
                if (this._localDiretorio != value)
                {
                    this._localDiretorio = value;
                    this.RaisePropertyChanged(() => this.LocalDiretorio);
                }
            }
        }

        public string Mensagem
        {
            get
            {
                return this._mensagem;
            }
            set
            {
                if (this._mensagem != value)
                {
                    this._mensagem = value;
                    this.RaisePropertyChanged(() => this.Mensagem);
                }
            }
        }

        public bool LeitorBomba
        {
            get
            {
                return _leitor;
            }
            set
            {
                if (_leitor != value)
                {
                    _leitor = value;
                    RaisePropertyChanged(() => LeitorBomba);
                }
            }
        }

        public bool PostoWeb
        {
            get
            {
                return _web;
            }
            set
            {
                if (_web != value)
                {
                    _web = value;
                    RaisePropertyChanged(() => PostoWeb);
                }
            }
        }

        public string GetConnection 
        {
            get 
            {
                return string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};Encoding=utf-8;ClientEncoding=utf8;Pooling=False;CommandTimeout=0;",
                                    this.Servidor,
                                    this.Porta.ToString(),
                                    this.Usuario,
                                    this.Senha,
                                    this.Banco);
            }
        }
    }
}
