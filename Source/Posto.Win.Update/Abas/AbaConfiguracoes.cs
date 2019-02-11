using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.ViewModel;
using Posto.Win.Update.Model;
using Posto.Win.Update.Extensions;
using Posto.Win.Update.DataContext;
using System.Windows;

namespace Posto.Win.Update.Abas
{
    public class AbaConfiguracoes : NotificationObject
    {
        #region Propriedades

        private ConfiguracaoModel _configuracoes;
        private Visibility _visibilidade;
        private bool _enableButtonConfiguracao;        
        private string _mensagemlabel;

        #endregion

        #region Construtor

        public AbaConfiguracoes()
        {
            ConfiguracaoModel = ConfiguracaoXml.CarregarConfiguracao().ToModel();
            EnableButtonConfiguracao = true;
            Visibilidade = Visibility.Hidden;
        }

        #endregion

        #region Objetos

        public ConfiguracaoModel ConfiguracaoModel
        {
            get
            {
                return _configuracoes;
            }
            set
            {
                if (_configuracoes != value)
                {
                    _configuracoes = value;
                    RaisePropertyChanged(() => ConfiguracaoModel);
                }
            }
        }

        public Visibility Visibilidade
        {
            get
            {
                return _visibilidade;
            }
            set
            {
                if (_visibilidade != value)
                {
                    _visibilidade = value;
                    RaisePropertyChanged(() => Visibilidade);
                }
            }
        }

        #endregion

        #region Funções

        public bool EnableButtonConfiguracao
        {
            get
            {
                return _enableButtonConfiguracao;
            }
            set
            {
                if (_enableButtonConfiguracao != value)
                {
                    _enableButtonConfiguracao = value;
                    RaisePropertyChanged(() => EnableButtonConfiguracao);
                }
            }
        }
        public string MensagemLabel
        {
            get
            {
                return _mensagemlabel;
            }
            set
            {
                if (_mensagemlabel != value)
                {
                    _mensagemlabel = value;
                    RaisePropertyChanged(() => MensagemLabel);
                }
            }
        }

        #endregion
        
    }
}
