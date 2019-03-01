using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Posto.Win.App.Structure;

namespace Posto.Win.App
{
    class Program
    {
        static void Main(string[] args)
        {
            #if DEBUG
            new Atualizador("1");
            #else
            try
            {
                new Atualizador(args.FirstOrDefault().ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            #endif
        }
    }
}
