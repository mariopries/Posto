using Npgsql;
using Posto.Win.Update.Infraestrutura;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Posto.Win.Update.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool PrimeiraVez = false;
        public MainWindow()
        {

            if (VerificaProgramaEmExecucao())
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("O Programa já está em Execução!",
                                          "Atenção Operador.",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);

                if (result == MessageBoxResult.OK)
                {
                    System.Windows.Application.Current.Shutdown();
                }
            }
            else
            {
                InitializeComponent();
                MyNotifyIcon.TrayMouseDoubleClick += MyNotifyIcon_TrayMouseDoubleClick;
                MyNotifyIcon.ShowBalloonTip("Atualizador - WinSGM", "O atualizador foi iníciado.", Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);
            }


            //System.Threading.Mutex hSolution = null;
            //// verifica se programa já está rodando
            //try
            //{
            //    hSolution = System.Threading.Mutex.OpenExisting("Atualizador.exe");
            //}
            //catch (System.Threading.WaitHandleCannotBeOpenedException)
            //{
            //    // Não existe Instancia do programa aberto
            //}

            //if (hSolution == null)
            //{
            //    hSolution = new System.Threading.Mutex(true, "Atualizador.exe");
            //    InitializeComponent();
            //    MyNotifyIcon.TrayMouseDoubleClick += MyNotifyIcon_TrayMouseDoubleClick;
            //    MyNotifyIcon.ShowBalloonTip("Atualizador - WinSGM", "O atualizador foi iníciado.", Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);
            //}
            //else
            //{
            //    MessageBoxResult result = System.Windows.MessageBox.Show("O Programa já está em Execução!",
            //                              "Atenção Operador.",
            //                              MessageBoxButton.OK,
            //                              MessageBoxImage.Error);

            //    if (result == MessageBoxResult.OK)
            //    {
            //        System.Windows.Application.Current.Shutdown();
            //    }
            //}
        }
        public static bool VerificaProgramaEmExecucao()
        {
            return Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1;
        }

    private void MyNotifyIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ShowInTaskbar = true;
            Show();
            WindowState = WindowState.Normal;
            Activate();
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            ShowInTaskbar = false;
            WindowState = WindowState.Minimized;
            Visibility = Visibility.Collapsed;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {

            if (WindowState == WindowState.Minimized)
            {
                if (PrimeiraVez == true)
                    MyNotifyIcon.ShowBalloonTip("Atualizador - WinSGM", "O atualizador foi minimizado mas está rodando em segundo plano.", Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);
                PrimeiraVez = true;
                ShowInTaskbar = false;
                Hide();

            }
        }
    }
}
