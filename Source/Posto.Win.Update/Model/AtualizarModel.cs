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
                return this._dia;
            }
            set
            {
                if (this._dia != value)
                {
                    this._dia = value;
                    this.RaisePropertyChanged(() => this.Dia);
                    this.RaisePropertyChanged(() => this.GetDataProximaAtualizacao);
                }
            }
        }

        public int Hora
        {
            get
            {
                return this._hora;
            }
            set
            {
                if (this._hora != value)
                {
                    this._hora = value;
                    this.RaisePropertyChanged(() => this.Hora);
                    this.RaisePropertyChanged(() => this.GetDataProximaAtualizacao);
                }
            }
        }

        public int Minuto
        {
            get
            {
                return this._minuto;
            }
            set
            {
                if (this._minuto != value)
                {
                    this._minuto = value;
                    this.RaisePropertyChanged(() => this.Minuto);
                    this.RaisePropertyChanged(() => this.GetDataProximaAtualizacao);
                }
            }
        }

        public DateTime? UltimaData
        {
            get
            {
                return this._ultimaData;
            }
            set
            {
                if (this._ultimaData != value)
                {
                    this._ultimaData = value;
                    this.RaisePropertyChanged(() => this.UltimaData);
                }
            }
        }
        
        public int? Versao
        {
            get
            {
                return this._versao;
            }
            set
            {
                if (this._versao != value)
                {
                    this._versao = value;
                    this.RaisePropertyChanged(() => this.Versao);
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

        public DateTime DataAtual
        {
            get
            {
                return this._dataAtual;
            }
            set
            {
                if (this._dataAtual != value)
                {
                    this._dataAtual = value;
                    this.RaisePropertyChanged(() => this.DataAtual);
                    this.RaisePropertyChanged(() => this.GetDataProximaAtualizacao);
                }
            }
        }

        public DateTime? GetDataProximaAtualizacao
        {
            get
            {
                if (this.DataAtual == DateTime.MinValue) 
                {
                    return (DateTime?)null;
                }

                var dataSemana = this.DataAtual
                                     .AddDays(this.Dia - ((int)this.DataAtual.DayOfWeek + 1))
                                     .Date
                                     .AddHours(this.Hora)
                                     .AddMinutes(this.Minuto);

                if (dataSemana < this.DataAtual)
                {
                    dataSemana = dataSemana.AddDays(7);
                }

                return dataSemana;
            }
        }
    }
}
