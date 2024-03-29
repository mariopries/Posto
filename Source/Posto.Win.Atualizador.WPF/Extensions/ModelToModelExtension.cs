﻿using Posto.Win.Update.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posto.Win.Update.Extensions
{
    public static class ModelToModelExtension
    {
        public static ConfiguracaoModel ToModel(this ConfiguracaoXml model)
        {
            return new ConfiguracaoModel()
            {
                Servidor = model.Servidor,
                Porta = !string.IsNullOrWhiteSpace(model.Porta) ? Convert.ToInt32(model.Porta) : 0,
                Banco = model.Banco,
                Usuario = model.Usuario,
                Senha = model.Senha,
                LocalDiretorio = model.LocalDiretorio,
                LocalPostgres = model.LocalPostgres,
                VersaoArquivo = model.VersaoArquivo,
                LeitorBomba = model.LeitorBomba,
                PostoWeb = model.PostoWeb,
                Backup = model.Backup,
                Vacuum = model.Vacuum,
                Reindex = model.Reindex
            };
        }

        public static ConfiguracaoXml ToModel(this ConfiguracaoModel model)
        {
            return new ConfiguracaoXml()
            {
                Servidor = model.Servidor,
                Porta = model.Porta.ToString(),
                Banco = model.Banco,
                Usuario = model.Usuario,
                Senha = model.Senha,
                LocalDiretorio = model.LocalDiretorio,
                LocalPostgres = model.LocalPostgres,
                VersaoArquivo = model.VersaoArquivo,
                LeitorBomba = model.LeitorBomba,
                PostoWeb = model.PostoWeb,
                Backup = model.Backup,
                Vacuum = model.Vacuum,
                Reindex = model.Reindex
            };
        }

    }
}
