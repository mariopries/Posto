using Atualizador.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atualizador.Extensions
{
    public static class ModelToModelExtension
    {
        internal static ConfiguracaoModel ToModel(this ConfiguracaoXml model)
        {
            return new ConfiguracaoModel()
            {
                Servidor = model.Servidor,
                Porta = !string.IsNullOrWhiteSpace(model.Porta) ? Convert.ToInt32(model.Porta) : 0,
                Banco = model.Banco,
                Usuario = model.Usuario,
                Senha = model.Senha,
                LocalDiretorio = model.LocalDiretorio,
                VersaoArquivo = model.VersaoArquivo,
                LeitorBomba = model.LeitorBomba,
                PostoWeb = model.PostoWeb,
            };
        }

        internal static ConfiguracaoXml ToModel(this ConfiguracaoModel model)
        {
            return new ConfiguracaoXml()
            {
                Servidor = model.Servidor,
                Porta = model.Porta.ToString(),
                Banco = model.Banco,
                Usuario = model.Usuario,
                Senha = model.Senha,
                LocalDiretorio = model.LocalDiretorio,
                VersaoArquivo = model.VersaoArquivo,
                LeitorBomba = model.LeitorBomba,
                PostoWeb = model.PostoWeb,
            };
        }
    }
}
