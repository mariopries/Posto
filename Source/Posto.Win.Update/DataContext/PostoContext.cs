using System;
using log4net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Npgsql;
using System.Data;
using Posto.Win.Update.Model;
using Posto.Win.Update.Extensions;

namespace Posto.Win.Update.DataContext
{
    public class PostoContext
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
            catch (Exception e)
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
            catch (Exception e)
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
            catch (Exception e)
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
            catch (Exception e)
            {
                log.Error(e);
            }
        }
    }
}