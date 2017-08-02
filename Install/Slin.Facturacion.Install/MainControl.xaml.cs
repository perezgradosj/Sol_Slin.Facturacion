using Slin.Facturacion.Common;
using Slin.Facturacion.InstallCommon;
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
using WPF.MDI;

namespace Slin.Facturacion.Install
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : Window
    {
        public MainControl(int param_process)
        {

            InitializeComponent();
            LoadInstaller_Or_Registr(param_process);
        }



        #region method

        public void LoadInstaller_Or_Registr(int param)
        {
            if (param == Constantes.ValorCero)
            {
                //Views.Control.ViewCompanyData frmControl = new Views.Control.ViewCompanyData();
                //frmControl.ShowDialog();

                //MainMdiContainer.Children.Add(new MdiChild()
                //{
                //    Title = "Company Information",
                //    Height = 445,
                //    Width = 750,
                //    WindowState = System.Windows.WindowState.Normal,
                //    Content = new Views.Control.ViewCompanyData(),
                //});


                //MainMdiContainer = new MdiContainer();
                //MainMdiContainer.Children.Add(new MdiChild()
                //{
                //    Title = "Mail Configuration",
                //    Height = 445,
                //    Width = 750,
                //    WindowState = System.Windows.WindowState.Normal,
                //    Content = new Views.Control.ViewMailSettings(oEmpresa),
                //});
            }
            else if (param == Constantes.ValorUno)
            {
                //Views.Control.ViewCompanyData frmControl = new Views.Control.ViewCompanyData();
                ////this.Close();
                //frmControl.ShowDialog();

                CredentialCertificate obj = new CredentialCertificate();

                MainMdiContainer = new MdiContainer();

                //MainMdiContainer.Children = new System.Collections.ObjectModel.ObservableCollection<MdiChild>();

                MainMdiContainer.Children.Add(new MdiChild()
                {
                    Title = "Mail Configuration",
                    Height = 445,
                    Width = 750,
                    WindowState = System.Windows.WindowState.Normal,
                    Content = new Views.Control.ViewMailSettings(oEmpresa, obj),
                });
            }
        }

        public Company oEmpresa = new Company();

        //public MdiContainer MainMdiContainer = new MdiContainer();

        #endregion









































        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void userRegistration_Click(object sender, RoutedEventArgs e)
        {
            //MainWindow popup = new MainWindow();
            //popup.ShowDialog();







            //MainMdiContainer.Children.Clear();
            MainMdiContainer.Children.Add(new MdiChild()
            {

                Title = " User Registration",
                Height = (System.Windows.SystemParameters.PrimaryScreenHeight - 35),
                Width = (System.Windows.SystemParameters.PrimaryScreenWidth - 35),
                Style = null,
                //Here UserRegistration is the class that you have created for mainWindow.xaml user control.
                Content = new Children_One()



                //Content = new MainWindow()



            });
        }

        private void compRegistration_Click(object sender, RoutedEventArgs e)
        {
            //MainMdiContainer.Children.Clear();
            //MainMdiContainer.Children.Add(new MdiChild()
            //{
            //    Title = " Company Registration",
            //    Height = (System.Windows.SystemParameters.PrimaryScreenHeight - 15),
            //    Width = (System.Windows.SystemParameters.PrimaryScreenWidth - 15),
            //    Style = null,
            //    //Here compRegistration is the class that you have created for mainWindow.xaml user control.
            //    Content = new MainWindow()
            //});
        }

        

        private void MenuOne_Click(object sender, RoutedEventArgs e)
        {
            //ChildWindowContent.Content = new Children_One();


            //MainMdiContainer.Children.Add(new MdiChild()
            //{
            //    Title = " Información de la Compañia",
            //    Height = 300,
            //    Width = 300,
            //    WindowState = System.Windows.WindowState.Normal,
            //    Content = new Children_One()
            //});



            MainMdiContainer.Children.Add(new MdiChild()
            {
                Title = "Company Information",
                Height = 445,
                Width = 750,
                WindowState = System.Windows.WindowState.Normal,
                Content = new Views.Control.ViewCompanyData(),
            });

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Install_Click(object sender, RoutedEventArgs e)
        {

        } 
    }
}
