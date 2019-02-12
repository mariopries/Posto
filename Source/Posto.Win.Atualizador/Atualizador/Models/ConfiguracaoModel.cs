using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atualizador.Models
{
    public class ConfiguracaoModel : INotifyPropertyChanged
    {
        private string _servidor;
        private int _porta;
        private string _banco;
        private string _usuario;
        private string _senha;
        private string _dirSistema;
        private string _dirPostgreSql;
        private string _versaoArquivo;
        private bool _leitor;
        private bool _web;
        private bool _backup;
        private bool _vacuum;
        private bool _reindex;

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

        public string Servidor
        {
            get { return _servidor; }
            set { SetField(ref _servidor, value); }
        }

        public int Porta
        {
            get { return _porta; }
            set { SetField(ref _porta, value); }
        }

        public string Banco
        {
            get { return _banco; }
            set { SetField(ref _banco, value); }
        }

        public string Usuario
        {
            get { return _usuario; }
            set { SetField(ref _usuario, value); }
        }

        public string Senha
        {
            get { return _senha; }
            set { SetField(ref _senha, value); }
        }

        public string DiretorioSistema
        {
            get { return _dirSistema; }
            set { SetField(ref _dirSistema, value); }
        }

        public string DiretorioPostgreSql
        {
            get { return _dirPostgreSql; }
            set { SetField(ref _dirPostgreSql, value); }
        }

        public string VersaoArquivo
        {
            get { return _versaoArquivo; }
            set { SetField(ref _versaoArquivo, value); }
        }

        public bool LeitorBomba
        {
            get { return _leitor; }
            set { SetField(ref _leitor, value); }
        }

        public bool PostoWeb
        {
            get { return _web; }
            set { SetField(ref _web, value); }
        }

        public bool Backup
        {
            get { return _backup; }
            set { SetField(ref _backup, value); }
        }

        public bool Vacuum
        {
            get { return _vacuum; }
            set { SetField(ref _vacuum, value); }
        }

        public bool Reindex
        {
            get { return _reindex; }
            set { SetField(ref _reindex, value); }
        }

        public string GetConnection
        {
            get
            {
                return string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};Encoding=utf-8;ClientEncoding=utf8;Pooling=False;CommandTimeout=0;",
                                    Servidor,
                                    Porta.ToString(),
                                    Usuario,
                                    Senha,
                                    Banco);
            }
        }        
    }
}
