using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posto
{
    public enum Aplicativo : byte
    {
        [Description("Retaguarda")]
        Retaguarda = 1,

        [Description("Pré-venda")]
        PreVenda = 2,

        [Description("Contagem de Estoque")]
        Estoque = 3,

        [Description("Frente de Caixa")]
        FrenteCaixa = 4,

        [Description("Troca de Óleo")]
        TrocaOleo = 5
    }
}
