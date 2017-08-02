using Slin.Facturacion.Common;
using Slin.Facturacion.InstallCommon;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;
using System.Xml.Serialization;

namespace Slin.Facturacion.RegisterCompany.Views.Control
{
    /// <summary>
    /// Interaction logic for ViewValidateServicesAdq.xaml
    /// </summary>
    public partial class ViewValidateServicesAdq : Window
    {
        private ConectionClass obj_DB = new ConectionClass();
        private Company oCompany = new Company();

        public ViewValidateServicesAdq(ConectionClass objDB, Company oempresa)
        {
            InitializeComponent();
            VerificateServices();
            obj_DB = objDB;
            oCompany = oempresa;
        }


        #region entity

        //public List<string> list_ws = new List<string>();
        //public List<string> list_app = new List<string>();


        public ListSevices list_ws = new ListSevices();
        public ListSevices list_app = new ListSevices();

        #endregion

        #region method

        private void ReadObjectService()
        {
            try
            {
                string pathFileXml = string.Empty;
                pathFileXml = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string ret = @"..\..\..\Files\Xml\Services.xml";
                pathFileXml = System.IO.Path.GetFullPath(System.IO.Path.Combine(pathFileXml, ret));

                xmldoc = new XmlDocument();
                xmldoc.Load(pathFileXml);
            }
            catch (Exception ex)
            {

            }
        }

        private void VerificateServices()
        {
            ReadObjectService();

            list_ws = new ListSevices();
            list_app = new ListSevices();

            //XmlNodeList nodetype;
            //XmlNodeList nodename;
            //XmlNodeList nodehability;
            //XmlNodeList nodedescription;

            var list_table = new Services();

            if (xmldoc.InnerXml.Length > 10)
            {

                XmlSerializer xmlSerial = new XmlSerializer(typeof(Services));
                using (TextReader reader = new StringReader(xmldoc.InnerXml))
                {
                    list_table = (Services)xmlSerial.Deserialize(reader);
                }

                //XmlSerializer xmlSerial = new XmlSerializer(typeof(Services));
                //sr = new StreamReader(Server.MapPath("~/DocumentoXML/" + NombreArchivo + ".xml"));
                //xmlFac.InvoiceType inv = (xmlFac.InvoiceType)xmlSerial.Deserialize(sr);


                //nodetype = xmldoc.GetElementsByTagName("Type");
                //nodename = xmldoc.GetElementsByTagName("Name");
                //nodehability = xmldoc.GetElementsByTagName("Hability");
                //nodedescription = xmldoc.GetElementsByTagName("Description");

                TreeViewItem item_WS = new TreeViewItem() { Header = "Servicios Windows" };
                TreeViewItem item_APP = new TreeViewItem() { Header = "Aplicaciones Web" };

                for (int i = 0; i <= list_table.Service.Count - 1; i++)
                {
                    if (list_table.Service[i].Type == Constantes.ValorWS && list_table.Service[i].Hability == Constantes.ValorUno + string.Empty)
                    {
                        //item = new TreeViewItem() { Header = "* " + nodename[i].InnerText.ToUpper() + "-" + nodedescription[i].InnerText };
                        //item_WS.Items.Insert(0, item);
                        item_WS.Items.Insert(0, new TreeViewItem() { Header = "* " + list_table.Service[i].Name.ToUpper() + "-" + list_table.Service[i].Description });
                        list_ws.Add(list_table.Service[i]);
                    }
                    else if (list_table.Service[i].Type == Constantes.ValorAPP && list_table.Service[i].Hability == Constantes.ValorUno + string.Empty)
                    {
                        item_APP.Items.Insert(0, new TreeViewItem() { Header = "* " + list_table.Service[i].Name.ToUpper() + "-" + list_table.Service[i].Description });
                        list_app.Add(list_table.Service[i]);
                    }
                }
                Trw_SW.Items.Add(item_WS);
                Trw_APP.Items.Add(item_APP);
            }
        }

        XmlDocument xmldoc = new XmlDocument();

        #endregion


        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            ViewCreateDirectories frmCreFolder = new ViewCreateDirectories(list_ws, list_app, obj_DB, oCompany);
            frmCreFolder.ShowDialog();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
