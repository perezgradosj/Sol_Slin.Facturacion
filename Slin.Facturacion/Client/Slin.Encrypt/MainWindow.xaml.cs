using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Slin.Encrypt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            txttext.Clear();
            txttextencrypt.Clear();
            txttext.Focus();
        }

        private void btnEncryptar_Click(object sender, RoutedEventArgs e)
        {
            if (txttext.Text.Length > 0)
            {
                txttextencrypt.Text = new Helper.Encrypt().EncryptKey(txttext.Text.Trim());
            }
            else
            {
                MessageBox.Show("Ingrese texto a encryptar.", "Info");
                txttext.Focus();
                txttextencrypt.Clear();
            }

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txttext.Clear();
            txttextencrypt.Clear();
            txttext.Focus();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
