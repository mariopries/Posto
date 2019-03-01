using Atualizador.Models;
using log4net;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atualizador.DataContext
{
    class PostoContext
    {
        #region Gerenciador de log

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        private NpgsqlConnection _conexao;
        private NpgsqlTransaction _transaction;

        public PostoContext(ConfiguracaoModel configuracao = null)
        {
            _conexao = new NpgsqlConnection(configuracao.GetConnection);
            _conexao.Open();
        }

        public NpgsqlCommand Query(string sql)
        {
            return new NpgsqlCommand(sql, _conexao);
        }

        public void BeginTransaction()
        {
            try
            {
                _transaction = _conexao.BeginTransaction();
            }
            catch (NpgsqlException e)
            {                
                log.Error(e);
            }
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch (NpgsqlException e)
            {
                log.Error(e);
            }
        }

        public void RollBack()
        {
            try
            {
                _transaction.Rollback();
            }
            catch (NpgsqlException e)
            {
                log.Error(e);
            }
        }

        public void Close()
        {
            try
            {
                _conexao.Close();
            }
            catch (NpgsqlException e)
            {
                log.Error(e);
            }
        }
    }
}
