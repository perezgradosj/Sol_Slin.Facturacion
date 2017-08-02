using Microsoft.Reporting.WinForms;
using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.Common;
using Slin.Facturacion.ServiceImplementation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using xmlFac = Slin.Facturacion.Common.CE;
using xmlCre = Slin.Facturacion.Common.CRE;
using xmlNotCred = Slin.Facturacion.Common.CENC;
using xmlNotDeb = Slin.Facturacion.Common.CEND;

using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using Slin.Facturacion.Common.Method;

namespace Slin.Facturacion.ProcessSend
{
    class Program
    {

        public string NameService = ConfigurationManager.AppSettings["NameService"].ToString();

        static string pathlog = string.Empty;

        static string PathReporte = string.Empty;
        static string PathHtml = string.Empty;

        static string PathPDF = string.Empty;
        static string PathPDF417 = string.Empty;
        static string PathXML = string.Empty;

        static void Main(string[] args)
        {
            try
            {
                {
                    string pathFile_APP = string.Empty;
                    pathFile_APP = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    string PathCompany = System.IO.Path.GetFullPath(System.IO.Path.Combine(pathFile_APP, "../../../../")); //tiempo real

                    //string pathFile_APP = string.Empty;
                    //pathFile_APP = @"J:\SLIN-ADE\DEPOSEG\";
                    //string PathCompany = pathFile_APP;

                    //string PathCompany = System.IO.Path.GetFullPath(System.IO.Path.Combine(pathFile_APP, "../../../../../../../")); //debug

                    Set_Path(PathCompany);
                }

                if (args.Length > 0)
                {
                    Singleton.Instance.WriteLog_Service_Comp(pathlog, " ");
                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjStart);
                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Doc_inProcess + args[0]);

                    Console.WriteLine(" ");
                    Console.WriteLine(Constantes.MsjStart);
                    Console.WriteLine(Constantes.Doc_inProcess + args[0]);

                    if (args.Length == 3)
                    {

                    }

                    new Program().Execute_SendDoc(args[0]);
                }

                //Singleton.Instance.WriteLog_Service_Comp(pathlog, " ");
                //Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjStart);
                //Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Doc_inProcess + "20547025319-01-FF11-00000001");

                //Console.WriteLine(" ");
                //Console.WriteLine(Constantes.MsjStart);
                //Console.WriteLine(Constantes.Doc_inProcess + "20547025319-01-FF11-00000001");
                //new Program().Execute_SendDoc("20216414056-07-BC03-00000001");
                //Console.ReadLine();
                //Console.Read();
            }
            catch (Exception ex) { }
        }


        #region ENTITY

        private List<Documento> olistDocSend;
        public List<Documento> oListDocSend
        {
            get { return olistDocSend; }
            set { olistDocSend = value; }
        }

        private ListaFacturaElectronica olistdocumentoupdate;
        public ListaFacturaElectronica oListDocumentoUpdate
        {
            get { return olistdocumentoupdate; }
            set { olistdocumentoupdate = value; }
        }

        private ListaFacturaElectronica olistdocumento;
        public ListaFacturaElectronica oListDocumento
        {
            get { return olistdocumento; }
            set { olistdocumento = value; }
        }

        private Documento odocsend;
        public Documento oDocSend
        {
            get { return odocsend; }
            set { odocsend = value; }
        }

        private static ReportViewer reportGR;
        public static ReportViewer ReportGR
        {
            get { return reportGR; }
            set { reportGR = value; }
        }
        #endregion
        private static void Set_Path(string path)
        {
            PathReporte = path + @"Procesos\smc\Report\";

            PathHtml = path + @"Procesos\smc\Html\";

            PathPDF = path + @"ProcesoCE\PDF\";
            PathPDF417 = path + @"ProcesoCE\PDF417\";
            PathXML = path + @"ProcesoCE\XML\";

            //pathlog = path + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smp\";

            pathlog = path + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smc\";
            Singleton.Instance.CreateDirectory(pathlog);
            pathlog = path + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smc\Log_ProcessSend.log";
        }

        #region METHOD

        private void Initialize()
        {
            oListDocSend = new List<Documento>();
            oListDocumentoUpdate = new ListaFacturaElectronica();
            oListDocumento = new ListaFacturaElectronica();
            oDocSend = new Documento();
        }


        private void Execute_SendDoc(string num_ce)
        {
            Initialize();
            //Singleton.Instance.CreateDirectory(pathlog);

            var result = new ServicioSeguridadSOA().GetStatus_WService(num_ce.Substring(0, 11), NameService);

            if (result.Count > Constantes.ValorCero)
            {
                SendDocument(num_ce);
            }
            else
            {
                Console.WriteLine(Constantes.MsjRucInactivo_Send.Replace("RUCREPLACE", num_ce.Substring(0, 11)));
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjRucInactivo_Send.Replace("RUCREPLACE", num_ce.Substring(0, 11)));
            }

            if (oListDocumentoUpdate.Count > Constantes.ValorCero)
            {
                string msjUpdate = new ServicioFacturacionSOA().UpdateDocCabStatuSend(oListDocumentoUpdate);
                Console.WriteLine(Constantes.Msj_DocUpdate + num_ce);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_DocUpdate + num_ce);
            }

            if (oListDocSend.Count > Constantes.ValorCero)
            {
                string msjInserted = new ServicioFacturacionSOA().InsertarListDocEnviado(oListDocSend);
                Console.WriteLine(Constantes.Msj_DocEnvRegistr + num_ce);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_DocEnvRegistr + num_ce);
            }

            //Console.WriteLine(Constantes.MsjFileDelete_Send + PathDocForSend + num_ce + ".xml");
            Console.WriteLine(Constantes.MsjFileDelete_Send + num_ce + ".xml, file send order");
            Console.WriteLine(Constantes.MsjEndProcess_Send + num_ce);
            Console.WriteLine(Constantes.MsjEnd);

            //Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjFileDelete_Send + PathDocForSend + num_ce + ".xml");
            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjFileDelete_Send + num_ce + ".xml, file send order");
            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjEndProcess_Send + num_ce);
            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjEnd);

            //Console.ReadLine();
            //Console.Read();
        }

        #endregion

        #region FOR ENVIO

        private ListaFacturaElectronica SendDocument(string ID_NUM_CE)
        {
            oListDocSend = new List<Documento>();

            oListDocumentoUpdate = new ListaFacturaElectronica();

            pathxmltemp = string.Empty;
            //string rutaAchivosOutput = string.Empty;
            oListDocumento = new ListaFacturaElectronica();

            try
            {
                //entityIdRucDesencrypt = new Common.Helper.Encrypt().DecryptKey(new BusinessSecurity.Entity.EntityClass().EntityId);
                var list = new BusinessSecurity.Entity.EntityClass().getsListEntity();

                string[] array = ID_NUM_CE.Split('-');
                //if (array[0].ToString() != entityIdRucDesencrypt)

                var ruccomp = array[0];

                if (!list.Contains(ruccomp))
                {
                    Console.WriteLine(Constantes.MsjRucInvalid_Send + ID_NUM_CE);
                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjRucInvalid_Send + ID_NUM_CE);
                }
                else
                {
                    oListDocumento = new ServicioFacturacionSOA().GetListDocumentoSendMail_Parameter(ID_NUM_CE);
                    oEmpresa = new ServicioSeguridadSOA().GetCredentialEntity(array[0].ToString());

                    var xmldoc_re = new FacturaElectronica();

                    #region si esta habilitado para el uso del servicio  

                    if (oListDocumento.Count == Constantes.ValorCero)
                    {
                        Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjDocNoObtenido_BD + ID_NUM_CE);
                        Console.WriteLine(Constantes.MsjDocNoObtenido_BD + ID_NUM_CE);
                    }
                    else
                    {
                        Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjDocObtenido_BD + ID_NUM_CE);
                        Console.WriteLine(Constantes.MsjDocObtenido_BD + ID_NUM_CE);

                        foreach (var xmldoc in oListDocumento)
                        {
                            #region recorre la lista que siempre contendra un item

                            NombreArchivo = ID_NUM_CE;
                            tpodocumento = xmldoc.TipoDocumento.CodigoDocumento;

                            if (xmldoc.REF_FILES == null)
                            { REF_FILES = string.Empty; }
                            else
                            { REF_FILES = xmldoc.REF_FILES; }

                            if (xmldoc.Campo1.Length < Constantes.ValorUno)
                            { montoLiteral = "-"; }
                            else
                            { montoLiteral = xmldoc.Campo1; }

                            pathxmltemp = PathXML + ID_NUM_CE;

                            bool exists_file_xml = Singleton.Instance.Validate_Exists_File(pathxmltemp + ".xml");

                            if (exists_file_xml == true)
                            {
                                #region
                                Console.WriteLine(Constantes.Msj_FileProccesing + pathxmltemp + ".xml");
                                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_FileProccesing + pathxmltemp + ".xml");

                                //crear el archivo pdf
                                bool exists_file_pdf = Singleton.Instance.Validate_Exists_File(PathPDF + ID_NUM_CE + ".pdf");
                                if (exists_file_pdf == true)
                                {
                                    //si el xml y el pdf existen serializamos para obtener algunos datos necesarios
                                    var result_bool = Read_File_Xml(tpodocumento, ID_NUM_CE, xmldoc.XML);
                                    if (result_bool == true) { }

                                    //debo mejorar esta parete de arriba
                                    Console.WriteLine(Constantes.Msj_FilePdfExists + PathPDF + ID_NUM_CE + ".pdf");
                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_FilePdfExists + PathPDF + ID_NUM_CE + ".pdf");
                                }
                                else
                                {
                                    #region si no existe el pdf                                 
                                    Cant_Error = Constantes.ValorCero;// reiniciamos el contador de errores al crear el pdf

                                    var result_bool = Process_To_FilePDF(tpodocumento, ID_NUM_CE, xmldoc.XML);

                                    Console.WriteLine(Constantes.Msj_CreatingPdfFile + PathPDF + ID_NUM_CE + ".pdf");
                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_CreatingPdfFile + PathPDF + ID_NUM_CE + ".pdf");
                                    if (Cant_Error == Constantes.ValorCero)
                                    {
                                        Console.WriteLine(Constantes.Msj_PdfCreatedSuccess + PathPDF + ID_NUM_CE + ".pdf");
                                        Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_PdfCreatedSuccess + PathPDF + ID_NUM_CE + ".pdf");
                                    }
                                    else
                                    {
                                        Console.WriteLine(Constantes.Msj_ErrorCreatedPdf + ID_NUM_CE + ".pdf");
                                        Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_ErrorCreatedPdf + ID_NUM_CE + ".pdf");
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            else
                            {
                                #region files
                                //crear el archivo xml

                                #region si no existe el xml
                                Create_File_Xml(pathxmltemp + ".xml", xmldoc.XML);

                                Console.WriteLine(Constantes.Msj_CreatingXmlDoc + pathxmltemp + ".xml");
                                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_CreatingXmlDoc + pathxmltemp + ".xml");

                                #endregion
                                bool exists_file_pdf = Singleton.Instance.Validate_Exists_File(PathPDF + ID_NUM_CE + ".pdf");

                                if (exists_file_pdf == true)
                                {
                                    Console.WriteLine(Constantes.Msj_FilePdfExists + PathPDF + ID_NUM_CE + ".pdf");
                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_FilePdfExists + PathPDF + ID_NUM_CE + ".pdf");
                                }
                                else
                                {
                                    //crear el archivo pdf
                                    #region si no existe el pdf

                                    Cant_Error = Constantes.ValorCero;// reiniciamos el contador de errores al crear el pdf
                                    var result_bool = Process_To_FilePDF(tpodocumento, ID_NUM_CE, xmldoc.XML);

                                    Console.WriteLine(Constantes.Msj_CreatingPdfFile + PathPDF + ID_NUM_CE + ".pdf");
                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_CreatingPdfFile + PathPDF + ID_NUM_CE + ".pdf");
                                    if (Cant_Error == Constantes.ValorCero)
                                    {
                                        Console.WriteLine(Constantes.Msj_PdfCreatedSuccess + PathPDF + ID_NUM_CE + ".pdf");
                                        Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_PdfCreatedSuccess + PathPDF + ID_NUM_CE + ".pdf");
                                    }
                                    else
                                    {
                                        Console.WriteLine(Constantes.Msj_ErrorCreatedPdf + ID_NUM_CE + ".pdf");
                                        Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_ErrorCreatedPdf + ID_NUM_CE + ".pdf");
                                    }
                                    #endregion
                                }
                                #endregion
                            }

                            #endregion
                            xmldoc_re = xmldoc;

                            #region envio
                            if (Cant_Error == Constantes.ValorCero)
                            {
                                if (oEmpresa.Email != null && oEmpresa.Email.Length > Constantes.ValorCero)
                                {
                                    #region si la empresa tiene un mail configurado para el envio

                                    if (!xmldoc_re.Email.Para.Contains("@"))
                                    {
                                        //si al traer el documento desde la bd no se tiene un correo de cliente, se buscara en base de datos con el ruc o dni del cliente
                                        #region

                                        oClient = new Cliente();
                                        oClient = new ServicioFacturacionSOA().GetEmailClient(xmldoc_re.Cliente.ClienteRuc);

                                        if (oClient.EmailClient != null)
                                        {
                                            #region

                                            if (oClient.EmailClient.Para.Contains("@"))
                                            {
                                                bool result = false;
                                                result = SendArchiveClient(oClient.EmailClient, oEmpresa, oFactura);

                                                #region
                                                if (result == true)
                                                {
                                                    oListDocumentoUpdate.Insert(0, new FacturaElectronica() { IdFactura = xmldoc_re.IdFactura, NombreArchivoXML = xmldoc_re.NombreArchivoXML });

                                                    oDocSend = new Documento();
                                                    oDocSend.Empresa = new Empresa();
                                                    oDocSend.TipoDocumento = new TipoDocumento();
                                                    oDocSend.TipoDocumento.CodigoDocumento = oFactura.TipoDocumento.CodigoDocumento;
                                                    oDocSend.Serie = oFactura.NumeroSerie;
                                                    oDocSend.NumeroDocumento = oFactura.NumeroDocumento;
                                                    //oDocSend.Destino = para;
                                                    oDocSend.Destino = oClient.EmailClient.Para;
                                                    oDocSend.Asunto = asunto;
                                                    oDocSend.Mensaje = string.Empty;
                                                    oDocSend.Remitente = oFactura.Empresa.RazonSocial;
                                                    oDocSend.Fecha_Cad = Convert.ToDateTime(DateTime.Now);
                                                    oDocSend.FechaEnvio = DateTime.Now.ToString();
                                                    oDocSend.Empresa.RUC = oFactura.Empresa.RUC;
                                                    oListDocSend.Add(oDocSend);

                                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_DocSendSuccess + ID_NUM_CE);
                                                    Console.WriteLine(Constantes.Msj_DocSendSuccess + ID_NUM_CE);
                                                }
                                                else
                                                {
                                                    Console.WriteLine(Constantes.Msj_DocNotSend + ID_NUM_CE);
                                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_DocNotSend + ID_NUM_CE);
                                                }
                                                #endregion
                                            }

                                            #endregion
                                        }
                                        else
                                        {
                                            Console.WriteLine(Constantes.Msj_NoEmailWasReceived);
                                            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_NoEmailWasReceived);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        //si al traer el documento desde la bd el cliente contiene un correo electronico de destino
                                        #region
                                        bool result = false;
                                        result = SendArchiveClient(xmldoc_re.Email, oEmpresa, oFactura);

                                        if (result == true)
                                        {
                                            oListDocumentoUpdate.Insert(0, new FacturaElectronica() { IdFactura = xmldoc_re.IdFactura, NombreArchivoXML = xmldoc_re.NombreArchivoXML });

                                            oDocSend = new Documento();
                                            oDocSend.Empresa = new Empresa();
                                            oDocSend.TipoDocumento = new TipoDocumento();
                                            oDocSend.TipoDocumento.CodigoDocumento = oFactura.TipoDocumento.CodigoDocumento;
                                            oDocSend.Serie = oFactura.NumeroSerie;
                                            oDocSend.NumeroDocumento = oFactura.NumeroDocumento;
                                            //oDocSend.Destino = para;
                                            oDocSend.Destino = xmldoc_re.Email.Para;
                                            oDocSend.Asunto = asunto;
                                            oDocSend.Mensaje = string.Empty;
                                            oDocSend.Remitente = oFactura.Empresa.RazonSocial;
                                            oDocSend.Fecha_Cad = Convert.ToDateTime(DateTime.Now);
                                            oDocSend.FechaEnvio = DateTime.Now.ToString();
                                            oDocSend.Empresa.RUC = oFactura.Empresa.RUC;
                                            oListDocSend.Add(oDocSend);

                                            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_DocSendSuccess + ID_NUM_CE);
                                            Console.WriteLine(Constantes.Msj_DocSendSuccess + ID_NUM_CE);
                                        }
                                        else
                                        {
                                            Console.WriteLine(Constantes.Msj_DocNotSend + ID_NUM_CE);
                                            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_DocNotSend + ID_NUM_CE);
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                else
                                {
                                    Console.WriteLine(Constantes.Msj_OutgoingMailNull);
                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_OutgoingMailNull);
                                }
                            }
                            else
                            {
                                Console.WriteLine(Constantes.Msj_DocNotSend + ID_NUM_CE);
                                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_DocNotSend + ID_NUM_CE);
                            }
                            #endregion
                        }
                    }
                    #endregion
                    //envio
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
            }
            var listaResultado = oListDocumento;
            return listaResultado;
        }

        private void Create_File_Xml(string path_xml, string xml_line)
        {
            try
            {
                //xml = new XmlDocument();
                //xml.InnerXml = xml_line;
                //xml.Save(path_xml);
                Singleton.Instance.Create_FileXml(xml_line, path_xml);

            }
            catch (Exception ex)
            {
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
            }
        }


        private bool Read_File_Xml(string tpo_ce, string Num_CE, string xml_line)
        {
            try
            {
                #region case

                switch (tpo_ce)
                {
                    case Constantes.Factura:
                    case Constantes.Boleta:
                        { ObtenerValoresXMLCE(pathxmltemp, xml_line); break; }
                    case Constantes.NotaCredito: { ObtenerValoresXMLCENotCredit(pathxmltemp, xml_line); break; }
                    case Constantes.NotaDebito: { ObtenerValoresXMLCENotDebit(pathxmltemp, xml_line); break; }
                    case Constantes.Retencion: { ObtenerValoresXMLCRE(pathxmltemp, xml_line); break; }
                    case Constantes.Percepcion: { break; }
                }
                return true;
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
                return false;
            }
        }

        private bool Process_To_FilePDF(string tpo_ce, string Num_CE, string xml_line)
        {
            try
            {
                #region case

                switch (tpo_ce)
                {
                    case Constantes.Factura:
                    case Constantes.Boleta:
                        { ObtenerValoresXMLCE(pathxmltemp, xml_line); break; }
                    case Constantes.NotaCredito: { ObtenerValoresXMLCENotCredit(pathxmltemp, xml_line); break; }
                    case Constantes.NotaDebito: { ObtenerValoresXMLCENotDebit(pathxmltemp, xml_line); break; }
                    case Constantes.Retencion: { ObtenerValoresXMLCRE(pathxmltemp, xml_line); break; }
                    case Constantes.Percepcion: { break; }
                }

                CrearPdfSegunTpoDoc(oFactura, Constantes.ValorDos + string.Empty, Num_CE);
                return true;
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
                return false;
            }
        }

        #endregion

        #region SERIALIZAR XML

        XmlDocument xml = new XmlDocument();

        string pathxmltemp = string.Empty;
        public string NombreArchivo = string.Empty;
        public string tpodocumento = string.Empty;
        //public string TextoNexto = string.Empty;
        public string montoLiteral = string.Empty;
        public string REF_FILES = string.Empty;
        //string nombrearchivoFile = string.Empty;

        XmlDocument XMLArchive = new XmlDocument();

        FacturaElectronica oFactura = new FacturaElectronica();
        ListaFacturaElectronica listamonto = new ListaFacturaElectronica();

        Cliente oClient = new Cliente();
        Empresa oEmpresa = new Empresa();

        #region OBTENER CE FROM XML FACTURA, BOLETA

        bool ObtenerValoresXMLCE(string pathxml, string xml_line)
        {
            oFactura = new FacturaElectronica();
            try
            {
                XMLArchive = new XmlDocument();
                XMLArchive.InnerXml = xml_line;

                var res = Singleton.Instance.GetDataXml_AllTypeDocument(xml_line, Constantes.Factura);
                var inv = (xmlFac.InvoiceType)(res);

                oFactura = new Slin.Facturacion.Common.UtilCE.UsefullClassWS().GetDataFromXMLCE(inv, XMLArchive, NombreArchivo);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
                return false;
            }
        }

        #endregion END CE

        #region NOTA DE CREDITO
        bool ObtenerValoresXMLCENotCredit(string pathxml, string xml_line)
        {
            oFactura = new FacturaElectronica();
            try
            {
                XMLArchive = new XmlDocument();
                XMLArchive.InnerXml = xml_line;

                var res = Singleton.Instance.GetDataXml_AllTypeDocument(xml_line, Constantes.NotaCredito);
                var nc = (xmlNotCred.CreditNoteType)(res);

                oFactura = new Common.UtilCE.UsefullClassWS().GetDataFromCNOTECredit(nc, XMLArchive, NombreArchivo, tpodocumento);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
                return false;
            }
        }
        #endregion END NOTA DE CREDITO

        #region NOTA DE DEBITO
        bool ObtenerValoresXMLCENotDebit(string pathxml, string xml_line)
        {
            oFactura = new FacturaElectronica();
            try
            {
                XMLArchive = new XmlDocument();
                XMLArchive.InnerXml = xml_line;

                var res = Singleton.Instance.GetDataXml_AllTypeDocument(xml_line, Constantes.NotaDebito);
                var nd = (xmlNotDeb.DebitNoteType)(res);

                oFactura = new Common.UtilCE.UsefullClassWS().GetDataFromCNOTEDebit(nd, XMLArchive, NombreArchivo, tpodocumento);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
                return false;
            }
        }
        #endregion END NOTA DE DEBITO

        #region OBTENER CRE FROM XML

        bool ObtenerValoresXMLCRE(string pathxml, string xml_line)
        {
            try
            {
                oFactura = new FacturaElectronica();

                XMLArchive = new XmlDocument();
                XMLArchive.InnerXml = xml_line;

                var res = Singleton.Instance.GetDataXml_AllTypeDocument(xml_line, Constantes.Retencion);
                var ret = (xmlCre.RetentionType)(res);

                oFactura = new Common.UtilCE.UsefullClassWS().GetDataFromXMLCRE(ret, XMLArchive, NombreArchivo, tpodocumento);

                //oFactura = Singleton.Instance.Get_Document(xml_line, Constantes.Factura, pathxml, NombreArchivo);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
                return false;
            }
        }

        #endregion

        #endregion END SERIALIZAR XML

        #region CREAR PDF
        int Cant_Error = Constantes.ValorCero;
        private void CrearPdfSegunTpoDoc(FacturaElectronica oFact, string typeFormat, string num_ce)
        {
            Cant_Error = Constantes.ValorCero;

            Create_PDF417(oFact, num_ce);
            Set_ListMont(oFact);
            Send_ParametersRPT(oFact, typeFormat);
            Create_PDFandSave(PathPDF + num_ce);
        }

        #region CODIGO PDF417

        void Create_PDF417(FacturaElectronica oFact, string num_cpe)
        {
            try
            {
                string msje_Result = new Common.UtilCE.ParametersClassWS().CrearCodigoPDF417(oFact, PathPDF417, num_cpe);

                if (msje_Result != Constantes.msjBarcodeCreate_Ok)
                {
                    Cant_Error++;
                    Console.WriteLine(Constantes.Value_DateToLog + msje_Result + ", [Dll: Common]");
                    //Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Value_DateToLog + msje_Result + ", [Dll: Common]");
                }
            }
            catch (Exception ex)
            {
                Cant_Error++;
                Console.WriteLine(Constantes.Msj_Error + ex.Message + ", [Create_PDF417]");
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message + ", [Create_PDF417]");
            }
        }

        #endregion END CODIGO PDF417

        #region LLENAR LISTA MONTOS

        void Set_ListMont(FacturaElectronica oFact)
        {
            try
            {
                Common.UtilCE.ParametersClassWS.msj_Result_Set_MontCommon = string.Empty;

                string RutaPDF417 = Path.Combine(PathPDF417 + NombreArchivo + ".bmp");
                string RutaBarcode = Path.Combine(PathPDF417 + NombreArchivo + "_Barcode.bmp");

                listamonto = new ListaFacturaElectronica();
                listamonto = new Common.UtilCE.ParametersClassWS().GetListaMontos(oFact, RutaPDF417, RutaBarcode);

                string msje = Common.UtilCE.ParametersClassWS.msj_Result_Set_MontCommon;

                if (msje.Length > Constantes.ValorCero)
                {
                    Cant_Error++;
                    Console.WriteLine("[" + DateTime.Now + "] " + msje + ", [Dll: Common]");
                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Value_DateToLog + msje + ", [Dll: Common]");
                }
            }
            catch (Exception ex)
            {
                Cant_Error++;
                Console.WriteLine(Constantes.Msj_Error + ex.Message + ", [Set_ListMont]");
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message + ", [Set_ListMont]");
            }
        }

        #endregion END LLENAR LISTA MONTOS

        #region ENVIAR PARAMETRO REPORTVIEWER

        void Send_ParametersRPT(FacturaElectronica oFact, string typeFormat)
        {
            try
            {
                ReportGR = new ReportViewer();
                var path_result = PathReporte + new Common.UtilCE.ParametersClassWS().GetPathReportviewer(oFact, int.Parse(typeFormat));

                if (path_result.Contains("Error"))
                {
                    Cant_Error++;
                    Console.WriteLine(Constantes.Value_DateToLog + path_result + ", [Dll: Common]");
                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Value_DateToLog + path_result + ", [Dll: Common]");
                }
                else
                {
                    //ReportGR.LocalReport.ReportPath = PathReporte + new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetPathReportviewer(oFact, int.Parse(typeFormat));
                    ReportGR.LocalReport.ReportPath = path_result;
                }
                var bool_result = RPTParameterCE(oFact);
                //RPTParameterCE(oFact);
            }
            catch (Exception ex)
            {
                Cant_Error++;
                Console.WriteLine(Constantes.Msj_Error + ex.Message + ", [Send_ParametersRPT]");
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message + ", [Send_ParametersRPT]");
            }
        }

        #endregion END ENVIAR PARAMETROS REPORTVIEWER

        #region PASAR PARAMETROS RPT SEGUN TPO DOC

        bool RPTParameterCE(FacturaElectronica oFact)
        {
            ReportParameter[] Dpr = new ReportParameter[0];
            try
            {
                #region CASE TYPE DOCUMENT
                switch (oFact.TipoDocumento.CodigoDocumento)
                {
                    case Constantes.Factura:
                    case Constantes.Boleta:
                        {
                            Dpr = new Common.UtilCE.ParametersClassWS().GetArrayParametersCE(oFact, listamonto);

                            ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oFact.ListaDetalleFacturaElectronica));
                            ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                            break;
                        }
                    case Constantes.NotaCredito:
                        {
                            Dpr = new Common.UtilCE.ParametersClassWS().GetArrayParametersNotCred(oFact, listamonto);

                            ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oFact.ListaDetalleFacturaElectronica));
                            ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                            ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDocAfectado", oFact.ListaAfectado));
                            break;
                        }
                    case Constantes.NotaDebito:
                        {
                            Dpr = new Common.UtilCE.ParametersClassWS().GetArrayParametersNotDebit(oFact, listamonto);

                            ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oFact.ListaDetalleFacturaElectronica));
                            ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                            ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDocAfectado", oFact.ListaAfectado));
                            break;
                        }
                    case Constantes.Retencion:
                        {
                            Dpr = new Common.UtilCE.ParametersClassWS().GetArrayParametersRetenc(oFact);

                            ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDetCRECPE", oFact.ListaDocCRECPE));
                            ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                            break;
                        }
                    case Constantes.Percepcion:
                        {
                            break;
                        }
                    case Constantes.GuiaRemision:
                        {
                            break;
                        }
                }
                #endregion

                ReportGR.LocalReport.SetParameters(Dpr);
                ReportGR.LocalReport.Refresh();

                return true;
            }
            catch (Exception ex)
            {
                Cant_Error++;
                Console.WriteLine(Constantes.Msj_Error + ex.Message + ", [RPTParameterCE]");
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message + ", [RPTParameterCE]");
                return false;
            }
        }


        #endregion

        #region CREA EL PDF

        private void Create_PDFandSave(string pathNombreArchivo)
        {
            try
            {
                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                ReportGR.ProcessingMode = ProcessingMode.Remote;
                byte[] bytes = ReportGR.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                using (System.IO.FileStream fs = new System.IO.FileStream(pathNombreArchivo + ".pdf", System.IO.FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                Cant_Error++;
                Console.WriteLine(Constantes.Msj_Error + ex.Message + ", [Create_PDFandSave]");
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message + ", [Create_PDFandSave]");
            }
        }

        #endregion




        #endregion

        #region SEND EMAIL WITH ARCHIVES

        private bool SendArchiveClient(Email objEmail, Empresa objEmpresa, FacturaElectronica oDoc)
        {
            //bool result = false;
            //result = EnviarEmail(objEmail, objEmpresa, oDoc);
            //return result;
            return EnviarEmail(objEmail, objEmpresa, oDoc);
        }

        public string cc, cco, asunto;
        public bool EnviarEmail(Email objClien, Empresa objEmp, FacturaElectronica oDoc)
        {
            asunto = string.Empty;

            bool result = false;

            //para = objClien.Para;
            cc = objClien.CC.Contains("@") ? objClien.CC : string.Empty;
            cco = objClien.CCO.Contains("@") ? objClien.CCO : string.Empty;

            objClien.CC = cc;
            objClien.CCO = cco;

            #region SWITCH
            switch (oDoc.TipoDocumento.CodigoDocumento)
            {
                case Constantes.Factura: { asunto += Constantes.Subject_Fact; break; }
                case Constantes.Boleta: { asunto += Constantes.Subject_Bol; break; }
                case Constantes.NotaCredito: { asunto += Constantes.Subject_Nc; break; }
                case Constantes.NotaDebito: { asunto += Constantes.Subject_Nd; break; }
                case Constantes.Retencion: { asunto += Constantes.Subject_Ret; break; }
                case Constantes.Percepcion: { asunto += Constantes.Subject_Per; break; }
                case Constantes.GuiaRemision: { asunto += Constantes.Subject_Guia; break; }
            }
            #endregion

            asunto += oDoc.SerieCorrelativo + " - " + oDoc.Empresa.RazonSocial;
            objEmp.Password = new Common.Helper.Encrypt().DecryptKey(objEmp.Password);
            result = SMTPMail(objClien, asunto, objEmp, oDoc);
            return result;
        }

        public bool SMTPMail(Email objEmail, string pAsunto, Empresa objEmpresa, FacturaElectronica oDocF)
        {
            bool result = false;

            // Crear el Mail
            using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
            {
                //mail.To.Add(new System.Net.Mail.MailAddress(pDestino));
                string[] AMailto = objEmail.Para.Split(';');

                foreach (string email in AMailto)
                {
                    if (email.Length > 5 && email.Contains("@"))
                    {
                        mail.To.Add(new MailAddress(email));
                    }
                    //mail.To.Add(new MailAddress(email));
                }

                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_EmailDestino + objEmail.Para);
                Console.WriteLine(Constantes.Msj_EmailDestino + objEmail.Para);


                if (objEmail.CC.Length > 0 && objEmail.CC.Contains("@"))
                {
                    string[] CCMail = objEmail.CC.Split(';');

                    foreach (string cc in CCMail)
                    {
                        //mail.CC.Add(new MailAddress(cc.TrimStart().TrimEnd()));
                        mail.CC.Add(new MailAddress(cc.Trim()));
                    }
                }


                if (objEmail.CCO.Length > 0 && objEmail.CCO.Contains("@"))
                {
                    string[] CCOMail = objEmail.CCO.Split(';');
                    foreach (string cco in CCOMail)
                    {
                        mail.Bcc.Add(new MailAddress(cco));
                    }
                }

                //mail.From = new System.Net.Mail.MailAddress(objEmpresa.Email, oDocF.Empresa.RazonSocial, System.Text.Encoding.UTF8);
                mail.From = new System.Net.Mail.MailAddress(objEmpresa.Email, objEmpresa.Email, System.Text.Encoding.GetEncoding("ISO-8859-1"));
                mail.Subject = pAsunto;

                mail.SubjectEncoding = System.Text.Encoding.UTF8;

                mail.Body = TemplateMail(oFactura, objEmpresa);
                mail.Priority = MailPriority.High;


                //MailAddress reply = new MailAddress(objEmpresa.Email, objEmpresa.Email, System.Text.Encoding.GetEncoding("ISO-8859-1"));
                //mail.ReplyTo = reply;

                if (mail.Body.Length == Constantes.ValorCero)
                {

                }

                mail.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                mail.IsBodyHtml = true;

                mail.Attachments.Add(new Attachment(PathXML + NombreArchivo + ".xml"));
                mail.Attachments.Add(new Attachment(PathPDF + NombreArchivo + ".pdf"));

                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_FilesToSend + NombreArchivo + ".xml, " + NombreArchivo + ".pdf");

                Console.WriteLine(Constantes.Msj_FilesToSend + NombreArchivo + ".xml, " + NombreArchivo + ".pdf");

                if (REF_FILES.Length > 0)
                {
                    string[] Files = REF_FILES.Split(';');
                    foreach (string file in Files)
                    {
                        var file_in = Path.Combine(file);
                        if (System.IO.File.Exists(file_in))
                        {
                            mail.Attachments.Add(new Attachment(file_in));

                            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_AditionalsFiles + file_in);
                            Console.WriteLine(Constantes.Msj_AditionalsFiles + file_in);
                        }
                    }
                }
                // Configuración SMTP
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                //if (objEmpresa.Dominio.Contains("gmail"))
                //{
                //    smtp = new System.Net.Mail.SmtpClient(objEmpresa.Dominio, objEmpresa.Port);//port 587
                //    smtp.EnableSsl = true;
                //}
                //else
                //{
                //    smtp = new System.Net.Mail.SmtpClient(objEmpresa.Dominio, objEmpresa.Port);
                //    //smtp = new System.Net.Mail.SmtpClient(objEmpresa.Dominio, objEmpresa.Port);
                //    smtp.EnableSsl = objEmpresa.UseSSL == Constantes.ValorUno ? true : false;
                //}

                if (objEmpresa.Dominio != Constantes.Guion && objEmpresa.Dominio.Length > Constantes.ValorUno)
                {
                    smtp = new SmtpClient(objEmpresa.Dominio, objEmpresa.Port);
                }
                else { smtp = new SmtpClient(objEmpresa.IP, objEmpresa.Port); }

                //smtp = new SmtpClient(objEmpresa.Dominio, objEmpresa.Port);
                smtp.EnableSsl = objEmpresa.UseSSL == Constantes.ValorUno ? true : false;

                //smtp.Credentials = CredentialCache.DefaultNetworkCredentials;
                smtp.Credentials = new System.Net.NetworkCredential(objEmpresa.Email, objEmpresa.Password);

                //smtp.UseDefaultCredentials = false;
                //smtp.EnableSsl = false;
                try
                {
                    smtp.Send(mail);
                    result = true;
                }
                catch (Exception ex)
                {
                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
                    Console.WriteLine(Constantes.Msj_Error + ex.Message);
                    result = false;
                }
            } // end using mail
            return result;
        }// end SMTPMail

        private string TemplateMail(FacturaElectronica oDoc, Empresa objcompany)
        {
            string body = string.Empty;

            //string path = PathHtml + objcompany.RUC + ".html";
            //body = Constantes.HtmlLineSend_Company;
            body = File.ReadAllText(PathHtml + objcompany.RUC + ".html", System.Text.Encoding.GetEncoding("ISO-8859-1"));

            try
            {
                #region HTML TEXT
                body = body.Replace("{URL_COMPANY_LOGO}", objcompany.Url_CompanyLogo);
                body = body.Replace("{URL_COMPANY_CONSULT}", objcompany.Url_CompanyConsult);
                body = body.Replace("URL_COMPANY_CONSULT", objcompany.Url_CompanyConsult);

                body = body.Replace("{RazonSocialEmisor}", oDoc.Empresa.RazonSocial.ToUpper());
                body = body.Replace("{ClienteRazonSocial}", oDoc.Cliente.RazonSocial);
                body = body.Replace("{SerieCorrelativo}", oDoc.SerieCorrelativo);

                #region case type document
                switch (oDoc.TipoDocumento.CodigoDocumento)
                {
                    case Constantes.Factura: { body = body.Replace("{TipoDocumento}", "FACTURA ELECTRÓNICA"); break; }
                    case Constantes.Boleta: { body = body.Replace("{TipoDocumento}", "BOLETA DE VENTA ELECTRÓNICA"); break; }
                    case Constantes.NotaCredito: { body = body.Replace("{TipoDocumento}", "NOTA DE CRÉDITO ELECTRÓNICA"); break; }
                    case Constantes.NotaDebito: { body = body.Replace("{TipoDocumento}", "NOTA DE DÉBITO ELECTRÓNICA"); break; }
                    case Constantes.Retencion: { body = body.Replace("{TipoDocumento}", "COMPROBANTE DE RETENCIÓN ELECTRÓNICA"); break; }
                    case Constantes.Percepcion: { body = body.Replace("{TipoDocumento}", "COMPROBANTE DE PERCEPCIÓN ELECTRÓNICA"); break; }
                    case Constantes.GuiaRemision: { body = body.Replace("{TipoDocumento}", "GUIA DE REMISIÓN ELECTRÓNICA"); break; }
                }
                #endregion
                body = body.Replace("{FechaEmision}", oDoc.FechaEmision.ToString("dd/MM/yyyy"));
                #endregion
            }
            catch (Exception ex)
            {
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                body = string.Empty;
            }
            return body;
        }
        #endregion








        #region DOCUMENT CANCELED SEND

         

        #endregion END
    }
}
