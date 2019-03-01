using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PostoWeb;

namespace Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnStringToBase64_Click(object sender, RoutedEventArgs e)
        {
           textEntrada.Text = Base64Convert.convertStringToBase64(textSaida.Text);
        }

        private void BtnBase64ToString_Click(object sender, RoutedEventArgs e)
        {
            textSaida.Text = Base64Convert.convertBase64ToString(textEntrada.Text);
        }

        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Implementar.");
        }

        private void BtnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Implementar.");
        }

        private void BtnLimparCodificado_Click(object sender, RoutedEventArgs e)
        {
            textEntrada.Text = "";
        }

        private void BtnLimparDescodificado_Click(object sender, RoutedEventArgs e)
        {
            textSaida.Text = "";
        }

    }
}
