using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atualizador.Models
{
    class AtualizarModel : INotifyPropertyChanged
    {
        #region Propriedades

        private int _dia;
        private int _hora;
        private int _minuto;
        private DateTime? _ultimaData;
        private int? _versao;
        private DateTime _dataAtual;

        #endregion

        #region Objetos

        public int Dia
        {
            get { return _dia; }
            set { SetField(ref _dia, value); }
        }

        public int Hora
        {
            get { return _hora; }
            set { SetField(ref _hora, value); }
        }

        public int Minuto
        {
            get { return _minuto; }
            set { SetField(ref _minuto, value); }
        }

        public int? Versao
        {
            get { return _versao; }
            set { SetField(ref _versao, value); }
        }

        public DateTime DataAtual
        {
            get { return _dataAtual; }
            set { SetField(ref _dataAtual, value); }
        }

        public DateTime? UltimaData
        {
            get { return _ultimaData; }
            set { SetField(ref _ultimaData, value); }
        }

        public DateTime? GetDataProximaAtualizacao
        {
            get
            {
                if (DataAtual == DateTime.MinValue)
                {
                    return (DateTime?)null;
                }

                var dataSemana = DataAtual
                                     .AddDays(Dia - ((int)DataAtual.DayOfWeek + 1))
                                     .Date
                                     .AddHours(Hora)
                                     .AddMinutes(Minuto);

                if (dataSemana < DataAtual)
                {
                    dataSemana = dataSemana.AddDays(7);
                }

                return dataSemana;
            }
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
