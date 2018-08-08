using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posto.Win.Update.Model
{
    public class AtualizarModel : NotificationObject
    {
        private int _dia;
        private int _hora;
        private int _minuto;
        private DateTime? _ultimaData;
        private int? _versao;
        private DateTime _dataAtual;
        private string _mensagemStatus;
        

        public int Dia
        {
            get
            {
                return _dia;
            }
            set
            {
                if (_dia != value)
                {
                    _dia = value;
                    RaisePropertyChanged(() => Dia);
                    RaisePropertyChanged(() => GetDataProximaAtualizacao);
                }
            }
        }

        public int Hora
        {
            get
            {
                return _hora;
            }
            set
            {
                if (_hora != value)
                {
                    _hora = value;
                    RaisePropertyChanged(() => Hora);
                    RaisePropertyChanged(() => GetDataProximaAtualizacao);
                }
            }
        }

        public int Minuto
        {
            get
            {
                return _minuto;
            }
            set
            {
                if (_minuto != value)
                {
                    _minuto = value;
                    RaisePropertyChanged(() => Minuto);
                    RaisePropertyChanged(() => GetDataProximaAtualizacao);
                }
            }
        }

        public int? Versao
        {
            get
            {
                return _versao;
            }
            set
            {
                if (_versao != value)
                {
                    _versao = value;
                    RaisePropertyChanged(() => Versao);
                }
            }
        }

        public string MensagemStatus
        {
            get
            {
                return _mensagemStatus;
            }
            set
            {
                if (_mensagemStatus != value)
                {
                    _mensagemStatus = value;
                    RaisePropertyChanged(() => MensagemStatus);
                }
            }
        }



        public DateTime DataAtual
        {
            get
            {
                return _dataAtual;
            }
            set
            {
                if (_dataAtual != value)
                {
                    _dataAtual = value;
                    RaisePropertyChanged(() => DataAtual);
                    RaisePropertyChanged(() => GetDataProximaAtualizacao);
                }
            }
        }

        public DateTime? UltimaData
        {
            get
            {
                return _ultimaData;
            }
            set
            {
                if (_ultimaData != value)
                {
                    _ultimaData = value;
                    RaisePropertyChanged(() => UltimaData);
                }
            }
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
    }
}
