using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atualizador.Models
{
    class ConfiguracaoModel : INotifyPropertyChanged
    {
        private string _servidor;
        private int _porta;
        private string _banco;
        private string _usuario;
        private string _senha;
        private string _localDiretorio;
        private string _versaoArquivo;
        private bool _leitor;
        private bool _web;

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

        public string LocalDiretorio
        {
            get { return _localDiretorio; }
            set { SetField(ref _localDiretorio, value); }
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
