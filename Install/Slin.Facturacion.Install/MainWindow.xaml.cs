using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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

namespace Slin.Facturacion.Install
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


        #region

        private void ReadScriptExecute(string path)
        {

        }


        #endregion

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            //Views.Control.ViewCompanyData frmControl = new Views.Control.ViewCompanyData();
            //this.Close();
            //frmControl.ShowDialog();


            //Ping pings = new Ping();
            //int timeout = 10;

            //if (pings.Send("http://www.sunat.gob.pe", timeout).Status == IPStatus.Success)
            //{
            //    MessageBox.Show("Exito");

            //}
            //else
            //{
            //    MessageBox.Show("Error");
            //}

            //CreateWB();
        }







        void CreateWB()
        {
            string soap = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
                    xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <soap:Body>
                        <PaymentSettings xmlns=""http://tempuri.org/"">
                            <name1>" + "S1ST3M4S" + "</name1>" +
                            "<name2>" + "ALEMANIA123" + "</name2>" +
                        "</PaymentSettings>" +
                    "</soap:Body>" +
            "</soap:Envelope>";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.sunat.gob.pe/ol-ad-itseida-ws/ReceptorService.htm");

            req.Headers.Add("SOAPAction" + "https://www.sunat.gob.pe/ol-ad-itseida-ws/ReceptorService.htm\"");
            req.ContentType = "text/xml;charset=\"utf-8\"";
            req.Accept = "text/xml";
            req.Method = "POST";

            using (Stream stm = req.GetRequestStream())
            {
                using (StreamWriter stmw = new StreamWriter(stm))
                {
                    stmw.Write(soap);
                }
            }

            WebResponse response = req.GetResponse();

        }


        void otherWEB()
        {
            HttpWebRequest obj;
            string url = "https://www.sunat.gob.pe/ol-ad-itseida-ws/ReceptorService.htm";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Install_Click(object sender, RoutedEventArgs e)
        {
            //MainControl mdiparent = new MainControl(0);
            //mdiparent.ShowDialog();

            Views.Control.ViewCompanyData frmcomp = new Views.Control.ViewCompanyData();
            frmcomp.ShowDialog();
        }

        private void btn_Install_registr_Click(object sender, RoutedEventArgs e)
        {
            Views.Control.ViewCompanyData frmcomp = new Views.Control.ViewCompanyData();
            frmcomp.ShowDialog();
        }
    }
}
