using Atualizador.DataContext;
using Atualizador.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atualizador.Objetos
{
    public class IndicadoresManutencao
    {
        #region Gerenciador de log

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Propriedades

        private bool _manutencao;
        private bool _fimmanutencao;
        private bool _atualizoudb;
        private bool _atualizouexe;
        private ConfiguracaoModel _configuracaoModel;

        #endregion

        #region Constantes

        private const string TabAtualizacao = "atualiz";
        private const string ColAtuCodigo = "atucod";
        private const string ColManutencao = "atumanuten";
        private const string ColFimManutencao = "atufimmanu";
        private const string ColAtualizaDB = "atuatuaban";
        private const string ColAtualizaExe = "atuatuaexe";

        #endregion

        #region Construtor

        public IndicadoresManutencao(ConfiguracaoModel configuracao)
        {
            Configuracao = configuracao;
            CarregaIndicadores();
        }

        #endregion

        #region Funções

        public ConfiguracaoModel Configuracao
        {
            get
            {
                return _configuracaoModel;
            }
            set
            {
                if (_configuracaoModel != value)
                {
                    _configuracaoModel = value;
                }
            }
        }
        public bool EmManutencao
        {
            get
            {
                return _manutencao;
            }
            set
            {
                if (_manutencao != value)
                {
                    try
                    {
                        _manutencao = value;
                        var context = new PostoContext(Configuracao);
                        context.Query("UPDATE " + TabAtualizacao + " SET " + ColManutencao + " = '" + ((value == true) ? "S" : "N") + "'").ExecuteNonQuery();
                        context.Close();
                    }
                    catch (Exception e)
                    {
                        log.Error(e);
                    }
                }
            }
        }
        public bool FimManutencao
        {
            get
            {
                return _fimmanutencao == true;
            }
            set
            {
                if (_fimmanutencao != value)
                {
                    try
                    {
                        _fimmanutencao = value;
                        var context = new PostoContext(Configuracao);
                        context.Query("UPDATE " + TabAtualizacao + " SET " + ColFimManutencao + " = '" + ((value == true) ? "S" : "N") + "'").ExecuteNonQuery();
                        context.Close();
                    }
                    catch (Exception e)
                    {
                        log.Error(e);
                    }
                }
            }
        }
        public bool AtualizouBanco
        {
            get
            {
                return _atualizoudb;
            }
            set
            {
                if (_atualizoudb != value)
                {
                    try
                    {
                        _atualizoudb = value;
                        var context = new PostoContext(Configuracao);
                        context.Query("UPDATE " + TabAtualizacao + " SET " + ColAtualizaDB + " = '" + ((value == true) ? "S" : "N") + "'").ExecuteNonQuery();
                        context.Close();
                    }
                    catch (Exception e)
                    {
                        log.Error(e);
                    }
                }
            }
        }
        public bool AtualizouExe
        {
            get
            {
                return _atualizouexe;
            }
            set
            {
                if (_atualizouexe != value)
                {
                    try
                    {
                        _atualizouexe = value;
                        var context = new PostoContext(Configuracao);
                        context.Query("UPDATE " + TabAtualizacao + " SET " + ColAtualizaExe + " = '" + ((value == true) ? "S" : "N") + "'").ExecuteNonQuery();
                        context.Close();
                    }
                    catch (Exception e)
                    {
                        log.Error(e);
                    }
                }
            }
        }

        #endregion

        #region Helpers

        public void CarregaIndicadores()
        {
            try
            {
                var context = new PostoContext(Configuracao);
                var query = context.Query("SELECT * FROM " + TabAtualizacao + " WHERE " + ColAtuCodigo + " = 1 LIMIT 1");

                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EmManutencao = (reader.GetString(reader.GetOrdinal(ColManutencao)) == "S") ? true : false;
                        FimManutencao = (reader.GetString(reader.GetOrdinal(ColFimManutencao)) == "S") ? true : false;
                        AtualizouBanco = (reader.GetString(reader.GetOrdinal(ColAtualizaDB)) == "S") ? true : false;
                        AtualizouExe = (reader.GetString(reader.GetOrdinal(ColAtualizaExe)) == "S") ? true : false;
                    }
                }
                context.Close();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
        }
        public void DerrubaConexoes()
        {
            var context = new PostoContext(Configuracao);
            context.Query("SELECT pg_terminate_backend(PID) FROM pg_stat_activity WHERE PID <> pg_backend_pid()").ExecuteNonQuery();
            context.Close();
        }

        #endregion
    }
}
