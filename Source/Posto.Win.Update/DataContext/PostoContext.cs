using System;
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
        private NpgsqlConnection  _conexao;
        private NpgsqlTransaction _transaction;

        public PostoContext(ConfiguracaoModel configuracao = null)
        {
            this._conexao = new NpgsqlConnection(configuracao.GetConnection);
            this._conexao.Open();
        }

        public NpgsqlCommand Query(string sql)
        {
            return new NpgsqlCommand(sql, this._conexao);
        }

        public void BeginTransaction()
        {
            this._transaction = this._conexao.BeginTransaction();
        }

        public void Commit()
        {
            this._transaction.Commit();
        }

        public void RollBack()
        {
            this._transaction.Rollback();
        }

        public void Close() 
        {
            this._conexao.Close();
        }
    }
}