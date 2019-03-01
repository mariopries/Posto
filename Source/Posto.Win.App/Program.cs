using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Posto.Win.UpdateLocal
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        //static void Main()
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            #if DEBUG
                Application.Run(new Posto(Aplicativo.Retaguarda));
            #else
                try
                {
                    var aplicativo = (Aplicativo)Enum.Parse(typeof(Aplicativo), args.FirstOrDefault().ToString());
                    Application.Run(new Posto(aplicativo));
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            #endif
        }
    }
}
