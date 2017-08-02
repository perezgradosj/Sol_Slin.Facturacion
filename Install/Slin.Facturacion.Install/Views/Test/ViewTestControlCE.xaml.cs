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
using System.Windows.Shapes;

namespace Slin.Facturacion.Install.Views.Test
{
    /// <summary>
    /// Interaction logic for TestControlCE.xaml
    /// </summary>
    public partial class ViewTestControlCE : Window
    {
        public ViewTestControlCE()
        {
            InitializeComponent();
        }

        private void btnExaminar_Click(object sender, RoutedEventArgs e)
        {
            PreviewValidate();
        }


        private void PreviewValidate()
        {
            Microsoft.Win32.OpenFileDialog openfile = new Microsoft.Win32.OpenFileDialog();
            openfile.Filter = "Zip |*.zip";

            if (openfile.ShowDialog().Value == true)
            {
                if (openfile.FileName.Length > 0)
                {
                    MessageBox.Show("Path: " + openfile.FileName);
                }
            }
        }

        private void btnCreateBD_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRegistrCompany_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //Views.Control.DBSettings frmbd = new Control.DBSettings();
            //if (frmbd.IsActive)
            //{
            //    frmbd.ShowDialog();
            //}


            //for (int i = 0; i <= Application.Current.MainWindow.Count; i++)
            //{
            //    string nameform = Application.OpenForms[i].ToString();
            //    if (nameform.Contains(Views.Control.DBSettings.NameProperty.Name))
            //    {
            //        Application.OpenForms[i].ShowDialog();
            //    }
            //}
        }

    }
}
