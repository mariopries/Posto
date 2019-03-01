using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posto.Win.UpdateLocal
{
    public enum Aplicativo : byte
    {
        [Description("Retaguarda"), Category("uGsw002.exe")]
        Retaguarda = 1,

        [Description("Pré-venda"), Category("aPreVend.exe")]
        PreVenda = 2,

        [Description("Contagem de Estoque"), Category("uMenuEst.exe")]
        Estoque = 3,

        [Description("Frente de Caixa"), Category("uSGM.exe")]
        FrenteCaixa = 4,

        [Description("Troca de Óleo"), Category("uTroOleo.exe")]
        TrocaOleo = 5,

        [Description("Leitor de Bomba"), Category("uLeitor.exe")]
        LeitorBomba = 6,

        [Description("Posto Web"), Category("uPostoWe.exe")]
        PostoWeb = 7
    }
}
