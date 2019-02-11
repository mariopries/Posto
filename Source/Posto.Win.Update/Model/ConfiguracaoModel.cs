using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
        private string  _versaoArquivo;
        private string  _localPostgres;
        private bool    _leitor;
        private bool    _web;
        private bool    _backup;
        private bool    _reindex;
        private bool    _vacuum;

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

        public string LocalPostgres
        {
            get
            {
                return this._localPostgres;
            }
            set
            {
                if (this._localPostgres != value)
                {
                    this._localPostgres = value;
                    this.RaisePropertyChanged(() => this.LocalPostgres);
                }
            }
        }

        public string VersaoArquivo
        {
            get
            {
                return this._versaoArquivo;
            }
            set
            {
                if (this._versaoArquivo != value)
                {
                    this._versaoArquivo = value;
                    this.RaisePropertyChanged(() => this.VersaoArquivo);
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

        public bool Backup
        {
            get
            {
                return _backup;
            }
            set
            {
                if (_backup != value)
                {
                    _backup = value;
                    RaisePropertyChanged(() => Backup);
                }
            }
        }
        public bool Reindex
        {
            get
            {
                return _reindex;
            }
            set
            {
                if (_reindex != value)
                {
                    _reindex = value;
                    RaisePropertyChanged(() => Reindex);
                }
            }
        }
        public bool Vacuum
        {
            get
            {
                return _vacuum;
            }
            set
            {
                if (_vacuum != value)
                {
                    _vacuum = value;
                    RaisePropertyChanged(() => Vacuum);
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
