using Npgsql;
using Posto.Win.Update.Infraestrutura;
using System;
using System.Collections.Generic;
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
        System.Windows.Forms.NotifyIcon icon = new System.Windows.Forms.NotifyIcon();
        public MainWindow()
        {
            InitializeComponent();
            this.MyNotifyIcon.TrayMouseDoubleClick += this.MyNotifyIcon_TrayMouseDoubleClick;
        }

        private void MyNotifyIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.Show();
            this.WindowState = System.Windows.WindowState.Normal;
            this.Activate();
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.WindowState = System.Windows.WindowState.Minimized;
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this.Hide();
            }
        }       
    }
}
