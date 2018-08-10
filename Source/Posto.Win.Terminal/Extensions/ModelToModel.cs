using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalUpdate.Estrutura;

namespace TerminalUpdate.Extensoes
{
    public static class ModelToModelExtension
    {
        public static Configuracoes ToModel(this ConfiguracoesXml model)
        {
            return new Configuracoes()
            {
                Local = model.Local,
                Servidor = model.Servidor,
            };
        }
        public static ConfiguracoesXml ToModel(this Configuracoes model)
        {
            return new ConfiguracoesXml()
            {
                Local = model.Local,
                Servidor = model.Servidor,
            };
        }
    }
}
