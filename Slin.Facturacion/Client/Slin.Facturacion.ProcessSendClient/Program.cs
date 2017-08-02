using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.ServiceImplementation;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
//using System.Drawing;

using Slin.Facturacion.Common;

using Microsoft.Reporting.WinForms;
//using iTextSharp.text.pdf;
using System.Configuration;
using System.Net.Mail;
using System.Windows.Forms;

using System.Net;
//using Org.BouncyCastle.X509;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;


using xmlFac = Slin.Facturacion.Common.CE;
using xmlCre = Slin.Facturacion.Common.CRE;
using xmlNotCred = Slin.Facturacion.Common.CENC;
using xmlNotDeb = Slin.Facturacion.Common.CEND;
using xmlRead = Slin.Facturacion.Common.Helper;

using System.Threading;
//using System.Drawing.Imaging;
using System.Globalization;

namespace Slin.Facturacion.ProcessSendClient
{
    class Program
    {
        public string PathReporte = ConfigurationManager.AppSettings["PathReporte"].ToString();
        public string PathHtml = ConfigurationManager.AppSettings["PathHtml"].ToString();

        public string PathListDocNotSend = ConfigurationManager.AppSettings["PathListDocNotSend"].ToString();
        public string PathListDocSend = ConfigurationManager.AppSettings["PathListDocSend"].ToString();
        public string PathErroLog = ConfigurationManager.AppSettings["PathErroLog"].ToString();

        public string PathPDF = ConfigurationManager.AppSettings["PathPDF"].ToString();
        public string PathPDF417 = ConfigurationManager.AppSettings["PathPDF417"].ToString();
        public string PathXML = ConfigurationManager.AppSettings["PathXML"].ToString();

        public string PathLogSLINADE = ConfigurationManager.AppSettings["PathLogSLINADE"].ToString();
        public string PathDocForSend = ConfigurationManager.AppSettings["PathDocForSend"].ToString();


        //StringBuilder logNsend = new StringBuilder();
        //StringBuilder logsend = new StringBuilder();
        //StringBuilder logError = new StringBuilder();

        List<string> logNsend = new List<string>();
        List<string> logsend = new List<string>();
        List<string> logError = new List<string>();

        string entityIdRucDesencrypt = string.Empty;//all entities

        static void Main(string[] args)
        {
            //new Program().GetListaDocumento();
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-PE");
            //Thread.CurrentThread.CurrentCulture
            //CultureInfo cultura1 = new CultureInfo("es-PE");
            //cultura1.NumberFormat.CurrencyDecimalSeparator = ".";

            try
            {
                if (args != null && args.Length > Constantes.ValorCero)
                {
                    for (int i = 0; i <= args.Length; i++)
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        //new Program().MethodWithParam(ID_CE);
                        new Program().SendDocument(args[i]);
                    }
                }
                else
                {
                    new Program().FileInfoList();
                }
                
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message + ", " + ex.InnerException);
            }
        }

        //public void MethodWithParam(string ID_CE)
        //{
        //    SendDocument(ID_CE);
        //}


        #region OTHERS
        void CrearNuevaCarpeta()
        {
            string newfolder = PathXML;
            //Console.WriteLine("New Folder:" + newfolder);
            if (!Directory.Exists(newfolder))
            {
                Directory.CreateDirectory(newfolder);
                //Console.WriteLine("Folder created: " + newfolder);
            }
        }


        private void CreateDirectoryLog(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }


        #endregion



        #region ENTITY

        private xmlRead.DocumentSend xmldoctype;
        public xmlRead.DocumentSend XmlDocType
        {
            get { return xmldoctype; }
            set { xmldoctype = value; }
        }

        private FacturaElectronica odocumento;
        public FacturaElectronica oDocumento
        {
            get { return odocumento; }
            set { odocumento = value; }
        }

        private ListaFacturaElectronica olistdocumento;
        public ListaFacturaElectronica oListDocumento
        {
            get { return olistdocumento; }
            set { olistdocumento = value; }
        }

        private ListaFacturaElectronica olistdocumentoupdate;
        public ListaFacturaElectronica oListDocumentoUpdate
        {
            get { return olistdocumentoupdate; }
            set { olistdocumentoupdate = value; }
        }

        private static ReportViewer reportGR;
        public static ReportViewer ReportGR
        {
            get { return reportGR; }
            set { reportGR = value; }
        }

        private List<Documento> olistDocSend;
        public List<Documento> oListDocSend
        {
            get { return olistDocSend; }
            set { olistDocSend = value; }
        }

        private Documento odocsend;
        public Documento oDocSend
        {
            get { return odocsend; }
            set { odocsend = value; }
        }

        //private bool resultSendDoc;
        //public bool ResultSendDoc
        //{
        //    get { return resultSendDoc; }
        //    set
        //    {
        //        resultSendDoc = value;
        //    }
        //}

        #endregion




        #region DETECTA AL INICIAR

        void FileInfoList()
        {
            CreateDirectoryLog(PathLogSLINADE + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\");
            CreateDirectoryLog(PathListDocNotSend + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\");
            CreateDirectoryLog(PathListDocSend + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\");
            CreateDirectoryLog(PathErroLog + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\");

            try
            {
                List<string> listaDetected = new List<string>();

                listaDetected = new List<string>();

                DirectoryInfo di = new DirectoryInfo(PathDocForSend);
                foreach (var fi in di.GetFiles())
                {
                    listaDetected.Add(fi.FullName);
                    Console.WriteLine("[" + DateTime.Now + "] Archivo Encontrado: " + fi.FullName);
                    //Console.ReadLine();
                }

                foreach (var li in listaDetected)
                {
                    Console.WriteLine("[" + DateTime.Now + "] -------------------------INICIO---------------------------");
                    Console.WriteLine("[" + DateTime.Now + "] Archivo en Proceso: " + li);
                    new Program().IniciaProcesoWith_Xml_Small(li);
                    Console.WriteLine("[" + DateTime.Now + "] ---------------------------FIN----------------------------");
                    //Console.ReadLine();
                }

                foreach (var del in listaDetected)
                {
                    //Console.WriteLine("[" + DateTime.Now + "]  | Eliminando Archivo: " + del);
                    //Console.ReadLine();
                    var file = Path.Combine(del);
                    if (System.IO.File.Exists(file))
                    {
                        System.IO.File.Delete(file);
                        //if (System.IO.File.Exists(file))
                        //{
                        //    System.IO.File.Delete(file);
                        //}
                    }
                }
                new Program().IniciarWatcher();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error al Inicio de Iniciar el Servicio: " + ex.Message + ", " + ex.InnerException);
                //Console.ReadLine();
            }

        }

        #endregion

        #region WATCHER



        public void IniciarWatcher()
        {
            try
            {
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = PathDocForSend;

                watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;

                // eventos
                //watcher.Changed += new FileSystemEventHandler(OnChanged);
                watcher.Created += new FileSystemEventHandler(OnChanged);

                watcher.EnableRaisingEvents = true;
                while (Console.Read() != 'q') ;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error el Iniciar el Watcher: " + ex.Message + ", " + ex.InnerException);
                //Console.ReadLine();
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("[" + DateTime.Now + "] ------------------------------------------------------------");
            Console.WriteLine("[" + DateTime.Now + "] Archivo Detectado: " + e.FullPath);
            try
            {
                string PathRutaXmlState = e.FullPath;

                if (GetExclusiveFileLock(e.FullPath))
                {
                    Console.WriteLine("[" + DateTime.Now + "] -------------------------INICIO---------------------------");
                    Console.WriteLine("[" + DateTime.Now + "] Archivo en Proceso: " + e.FullPath);
                    new Program().IniciaProcesoWith_Xml_Small(PathRutaXmlState);
                    Console.WriteLine("[" + DateTime.Now + "] ---------------------------FIN----------------------------");
                    //Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Evento Changed: " + e.FullPath + ", " + ex.Message + ", " + ex.InnerException);
                //Console.ReadLine();
            }
        }

        private static bool GetExclusiveFileLock(string path)
        {
            var fileReady = false;
            const int MaximumAttemptsAllowed = 15;
            var attemptsMade = 0;

            while (!fileReady && attemptsMade <= MaximumAttemptsAllowed)
            {
                try
                {
                    using (File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        //Log.WriteLine($"Archivo {path} está listo para ser procesado.");

                        Console.WriteLine("[" + DateTime.Now + "] Termino de copiar el Archivo, " + path);
                        return true;
                    }

                }
                catch (IOException ioex)
                {
                    attemptsMade++;
                    Thread.Sleep(1000);
                    //Log.WriteLine($"Archivo {path} aún no termina de ser copiado.");
                    Console.WriteLine("[" + DateTime.Now + "] Error al Detectar Archivo: " + path + ", " + ioex.Message + ", " + ioex.InnerException);
                    //Console.ReadLine();
                }
            }
            return fileReady;
        }

        private void Inicializador()
        {
            logsend = new List<string>();
            logNsend = new List<string>();
            logError = new List<string>();
            oListDocumentoUpdate = new ListaFacturaElectronica();
            oListDocSend = new List<Documento>();
            XmlDocType = new xmlRead.DocumentSend();
        }

        private void IniciaProcesoWith_Xml_Small(string PathRutaXmlState)
        {
            try
            {
                Inicializador();

                var objeto = new xmlRead.DocumentSend();
                objeto = LeerDataXml(PathRutaXmlState);


                if (objeto.ID_CE != null)
                {
                    Console.WriteLine("[" + DateTime.Now + "] Se a Leido Correctamente, " + PathRutaXmlState);
                }
                else
                {
                    Console.WriteLine("[" + DateTime.Now + "] Error al Leer el archivo, " + PathRutaXmlState);
                }


                //entityIdRucDesencrypt = new Helper.Encrypt().DecryptKey(entityIdRucEncrypt);
                entityIdRucDesencrypt = new Helper.Encrypt().DecryptKey(new Slin.Facturacion.BusinessSecurity.Entity.EntityClass().EntityId);
                
                


                if (objeto.RucEmisor == entityIdRucDesencrypt)
                {
                    SendDocument(objeto.ID_CE);
                }
                else if (objeto.RucEmisor != null && objeto.RucEmisor.Length == 11)
                {
                    string[] matrix = PathRutaXmlState.Split('\\');
                    string value = matrix[6];
                    logError.Add("[" + DateTime.Now + "] El Ruc de Empresa del Documento Nro:  " + objeto.ID_CE + " no corresponde o no tiene Licencia para este Proceso.");
                    logNsend.Add("[" + DateTime.Now + "] El Documento Nro: " + objeto.ID_CE + ", No se a Enviado, Revise el Error en la sgte Ubicación: " + PathErroLog + value);
                }
                else
                {
                    string[] matrix = PathRutaXmlState.Split('\\');

                    string value = matrix[6];

                    logNsend.Add("[" + DateTime.Now + "] El Documento Nro: " + objeto.ID_CE + ", No se a Enviado, Revise el Error en la Sgte Ubicación: " + PathErroLog + value);
                    //logNsend.Add("[" + DateTime.Now + "] El Documento Nro: " + PathRutaXmlState + ", No se a Enviado, Revise el Error en la Sgte Ubicación: " + PathErroLog + value);
                }


                if (logNsend.Count > 0)
                {
                    foreach (var line in logNsend)
                    {
                        using (StreamWriter sw = new StreamWriter(PathListDocNotSend + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\ListaDocNotSend.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }

                if (logsend.Count > 0)
                {
                    foreach (var line in logsend)
                    {
                        using (StreamWriter sw = new StreamWriter(PathListDocSend + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\ListaDocSend.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }

                if (logError.Count > 0)
                {
                    foreach (var line in logError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathErroLog + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\ListError.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }

                if (oListDocumentoUpdate.Count > 0)
                {
                    string msjUpdate = new ServicioFacturacionSOA().UpdateDocCabStatuSend(oListDocumentoUpdate);
                }

                if (oListDocSend.Count > 0)
                {
                    string msjInserted = new ServicioFacturacionSOA().InsertarListDocEnviado(oListDocSend);
                }

                var file = Path.Combine(PathRutaXmlState);
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);

                Console.WriteLine("[" + DateTime.Now + "] Eliminando Archivo: " + PathRutaXmlState);
                Console.WriteLine("[" + DateTime.Now + "] Fin del Proceso de Envio para el Documento: " + objeto.ID_CE);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error el Procesar el Archivo: " + PathRutaXmlState + ", " + ex.Message + ", " + ex.InnerException);
                //Console.ReadLine();
            }
        }

        private xmlRead.DocumentSend LeerDataXml(string PathRutaXmlState)
        {

            try
            {
                Console.WriteLine("[" + DateTime.Now + "] Inicio de Lectura de Archivo: " + PathRutaXmlState);
                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlRead.DocumentSend));
                sr = new StreamReader(PathRutaXmlState);
                xmlRead.DocumentSend xmlread = (xmlRead.DocumentSend)xmlSerial.Deserialize(sr);

                XmlDocType = new xmlRead.DocumentSend();

                XmlDocType.ID_CE = xmlread.ID_CE;
                XmlDocType.Tipo_CE = xmlread.Tipo_CE;
                XmlDocType.Estado_Doc = xmlread.Estado_Doc;
                XmlDocType.RucEmisor = xmlread.RucEmisor;
                XmlDocType.TypeFormat = xmlread.TypeFormat;

                sr.Close();
                Console.WriteLine("[" + DateTime.Now + "] Fin de Lectura del Archivo Ok: " + PathRutaXmlState);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error al Procesar el Documento Nro: " + PathRutaXmlState + ", El Formato Xml no es Correcto, " + ex.Message);
                logError.Add("[" + DateTime.Now + "] Error al Procesar el Documento Nro: " + PathRutaXmlState + ", El Formato Xml no es Correcto, " + ex.Message);
                sr.Close();
                //Console.ReadLine();
            }

            return XmlDocType;
        }

        #endregion




        #region FOR ENVIO

        private ListaFacturaElectronica SendDocument(string ID_NUM_CE)
        {
            oListDocSend = new List<Documento>();

            logsend = new List<string>();
            logNsend = new List<string>();
            logError = new List<string>();

            oListDocumentoUpdate = new ListaFacturaElectronica();
            CrearNuevaCarpeta();

            pathxmltemp = string.Empty;
            string rutaAchivosOutput = string.Empty;
            oListDocumento = new ListaFacturaElectronica();

            oListDocumento = new ServicioFacturacionSOA().GetListDocumentoSendMail_Parameter(ID_NUM_CE);


            if (oListDocumento.Count == Constantes.ValorCero)
            {
                Console.WriteLine("[" + DateTime.Now + "] No se Obtuvo ningún Documento de la Base de Datos con ID: " + ID_NUM_CE);
            }
            else
            {
                Console.WriteLine("[" + DateTime.Now + "] Se Obtuvo un Documento de la BD con ID: " + ID_NUM_CE);

                #region se ejecuta si obtuvo algun registro de la database

                //entityIdRucDesencrypt = new Helper.Encrypt().DecryptKey(entityIdRucEncrypt);
                entityIdRucDesencrypt = new Helper.Encrypt().DecryptKey(new Slin.Facturacion.BusinessSecurity.Entity.EntityClass().EntityId);




                foreach (var xmldoc in oListDocumento)
                {
                    Console.WriteLine("[" + DateTime.Now + "] Iniciando el Proceso del Documento: " + ID_NUM_CE);
                    oEmpresa = new ServicioSeguridadSOA().GetCredentialEntity(xmldoc.Empresa.RUC);

                    #region valida si la empresa esta habilitada para usar el servicio

                    if (xmldoc.Empresa.RUC == entityIdRucDesencrypt)
                    {
                        #region EMPIEZA A GENERAR LOS ARCHIVOS XML AND PDF

                        NombreArchivo = xmldoc.NombreArchivoXML;
                        tpodocumento = xmldoc.TipoDocumento.CodigoDocumento;
                        //moneda = xmldoc.Moneda.Descripcion;

                        if (xmldoc.REF_FILES == null)
                        {
                            REF_FILES = string.Empty;
                        }
                        else
                        {
                            REF_FILES = xmldoc.REF_FILES;
                        }

                        if (xmldoc.Campo1.Length < Constantes.ValorUno)
                        {
                            montoLiteral = "-";
                        }
                        else
                        {
                            montoLiteral = xmldoc.Campo1;
                        }
                        pathxmltemp = PathXML + xmldoc.NombreArchivoXML;


                        //AQUI VALIDA SI EXISTE EL ARCHIVO XML EN EL DIRECTORIO
                        Console.WriteLine("[" + DateTime.Now + "] Verificando si Existe el Archivo: " + pathxmltemp + ".xml");
                        var XmlExists = Path.Combine(pathxmltemp + ".xml");
                        if (System.IO.File.Exists(XmlExists))
                        {
                            Console.WriteLine("[" + DateTime.Now + "] Archivo XML del Doc.: " + pathxmltemp + ".xml" + ", Existe!.");

                            //VERIFICA SI EL ARCHIVO PDF EXISTE EN EL DIRECTORIO
                            Console.WriteLine("[" + DateTime.Now + "] Verificando si Existe el Archivo: " + PathPDF + NombreArchivo + ".pdf");
                            var PdfExists = Path.Combine(PathPDF + NombreArchivo + ".pdf");


                            if (!System.IO.File.Exists(PdfExists))
                            {
                                Console.WriteLine("[" + DateTime.Now + "] Archivo PDF del Doc.: " + xmldoc.NombreArchivoXML + " No Existe!. ");

                                Console.WriteLine("[" + DateTime.Now + "] Inicio de Creación del PDF del Doc.: " + xmldoc.NombreArchivoXML + ".pdf");

                                #region CASE TPO DOCUMENTO FOR PDF
                                Console.WriteLine("[" + DateTime.Now + "] Leyendo Archivo: " + pathxmltemp + ".xml");
                                switch (xmldoc.TipoDocumento.CodigoDocumento)
                                {

                                    case Constantes.Factura:
                                    case Constantes.Boleta:
                                        {
                                            ObtenerValoresXMLCE(pathxmltemp);
                                            Console.WriteLine("[" + DateTime.Now + "] Fin de Lectura del Archivo: " + pathxmltemp + ".xml");
                                            //CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
                                            CrearPdfSegunTpoDoc(oFactura, xmldoc.TypeFormat + string.Empty);
                                            break;
                                        }

                                    case Constantes.NotaCredito:
                                        {
                                            ObtenerValoresXMLCENotCredit(pathxmltemp);
                                            Console.WriteLine("[" + DateTime.Now + "] Fin de Lectura del Archivo: " + pathxmltemp + ".xml");
                                            //CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
                                            CrearPdfSegunTpoDoc(oFactura, xmldoc.TypeFormat + string.Empty);
                                            break;
                                        }
                                    case Constantes.NotaDebito:
                                        {
                                            ObtenerValoresXMLCENotDebit(pathxmltemp);
                                            Console.WriteLine("[" + DateTime.Now + "] Fin de Lectura del Archivo: " + pathxmltemp + ".xml");
                                            //CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
                                            CrearPdfSegunTpoDoc(oFactura, xmldoc.TypeFormat + string.Empty);
                                            break;
                                        }

                                    case Constantes.Retencion:
                                        {
                                            ObtenerValoresXMLCRE(pathxmltemp); // PARA EL PDF
                                            Console.WriteLine("[" + DateTime.Now + "] Fin de Lectura del Archivo: " + pathxmltemp + ".xml");
                                            //CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
                                            CrearPdfSegunTpoDoc(oFactura, xmldoc.TypeFormat + string.Empty);
                                            break;
                                        }
                                    case Constantes.Percepcion:
                                        {
                                            Console.WriteLine("[" + DateTime.Now + "] Fin de Lectura del Archivo: " + pathxmltemp + ".xml");
                                            break;
                                        }
                                }

                                #endregion

                                Console.WriteLine("[" + DateTime.Now + "] Se a creado el archivo: " + xmldoc.NombreArchivoXML + ".pdf");
                            }
                            else
                            {
                                Console.WriteLine("[" + DateTime.Now + "] Archivo PDF: " + PathPDF + xmldoc.NombreArchivoXML + ".pdf, Existe!");
                            }




                        }
                        else
                        {
                            Console.WriteLine("[" + DateTime.Now + "] Archivo XML del Doc.: " + xmldoc.NombreArchivoXML + " No Existe!. ");

                            Console.WriteLine("[" + DateTime.Now + "] Inicio de Cración del Archivo XML del Doc.: " + xmldoc.NombreArchivoXML);

                            xml = new XmlDocument();
                            xml.InnerXml = xmldoc.XML;
                            xml.Save(pathxmltemp + ".xml"); // HASTA AQUI YA ESTA CREADO EL ARCHIVO XML

                            Console.WriteLine("[" + DateTime.Now + "] Se a creado el archivo: " + xmldoc.NombreArchivoXML + ".xml");


                            var PdfExists = Path.Combine(PathPDF + NombreArchivo + ".pdf");
                            if (!System.IO.File.Exists(PdfExists))
                            {
                                #region CASE TPO DOCUMENTO FOR PDF
                                switch (xmldoc.TipoDocumento.CodigoDocumento)
                                {
                                    case Constantes.Factura:
                                    case Constantes.Boleta:
                                        {
                                            ObtenerValoresXMLCE(pathxmltemp);
                                            //CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
                                            CrearPdfSegunTpoDoc(oFactura, xmldoc.TypeFormat + string.Empty);
                                            break;
                                        }

                                    case Constantes.NotaCredito:
                                        {
                                            ObtenerValoresXMLCENotCredit(pathxmltemp);
                                            //CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
                                            CrearPdfSegunTpoDoc(oFactura, xmldoc.TypeFormat + string.Empty);
                                            break;
                                        }
                                    case Constantes.NotaDebito:
                                        {
                                            ObtenerValoresXMLCENotDebit(pathxmltemp);
                                            //CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
                                            CrearPdfSegunTpoDoc(oFactura, xmldoc.TypeFormat + string.Empty);
                                            break;
                                        }

                                    case Constantes.Retencion:
                                        {
                                            ObtenerValoresXMLCRE(pathxmltemp); // PARA EL PDF
                                            //CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
                                            CrearPdfSegunTpoDoc(oFactura, xmldoc.TypeFormat + string.Empty);

                                            break;
                                        }
                                    case Constantes.Percepcion:
                                        {
                                            break;
                                        }
                                }

                                #endregion
                            }
                        }




                        #endregion

                        if (oFactura.Empresa == null)
                        {
                            Console.WriteLine("[" + DateTime.Now + "] Leyendo Archivo XML: " + pathxmltemp + ".xml");
                            #region SERIALIZA EL ARCHIVO XML PARA OBTENER DATOS DE LA EMPRESA Y EL CLIENTE SOLO PARA EL ENVIO DEL CORREO

                            switch (xmldoc.TipoDocumento.CodigoDocumento)
                            {
                                case Constantes.Boleta:
                                case Constantes.Factura:
                                    {
                                        ObtenerValoresXMLCE(pathxmltemp);
                                        break;
                                    }
                                case Constantes.NotaCredito:
                                    {
                                        ObtenerValoresXMLCENotCredit(pathxmltemp);
                                        break;
                                    }
                                case Constantes.NotaDebito:
                                    {
                                        ObtenerValoresXMLCENotDebit(pathxmltemp);
                                        break;
                                    }
                                case Constantes.Retencion:
                                    {
                                        ObtenerValoresXMLCRE(pathxmltemp);
                                        break;
                                    }
                                case Constantes.Percepcion:
                                    {
                                        break;
                                    }
                            }
                            #endregion END SERIALIZA EL ARCHIVO XML PARA OBTENER DATOS DE LA EMPRESA Y EL CLIENTE SOLO PARA EL ENVIO DEL CORREO
                        }



                        #region valida si hay un email para hacer el envio de documentos
                        if (oEmpresa.Email != null && oEmpresa.Email.Length > Constantes.ValorCero)
                        {
                            #region TRY ENVIO MAIL
                            try
                            {
                                // AQUI SE HACE EL PROCESO DE CARGA DE ARCHIVOS DE ENVIO
                                if (!xmldoc.Email.Para.Contains("@"))
                                {
                                    oClient = new Cliente();
                                    oClient = new ServicioFacturacionSOA().GetEmailClient(xmldoc.Cliente.ClienteRuc);

                                    // SE ENVIA LOS DOCUMENTOS
                                    if (oClient.EmailClient != null)
                                    {
                                        if (oClient.EmailClient.Para.Contains("@"))
                                        {
                                            bool result = false;
                                            result = SendArchiveClient(oClient.EmailClient, oEmpresa, oFactura);



                                            if (result == true)
                                            {
                                                Console.WriteLine("[" + DateTime.Now + "] El Documento: " + xmldoc.NombreArchivoXML + " Se a Enviado Correctamente!");
                                                oListDocumentoUpdate.Insert(0, new FacturaElectronica() { IdFactura = xmldoc.IdFactura, NombreArchivoXML = xmldoc.NombreArchivoXML });

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

                                                logsend.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " Se a Enviado Correctamente!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("[" + DateTime.Now + "] El Documento: " + xmldoc.NombreArchivoXML + " No se a Enviado.");
                                                logNsend.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " No se a Enviado.");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("[" + DateTime.Now + "] El Documento: " + oFactura.NombreArchivoXML + " No Contiene un Mail Destino, tampoco en la base de datos.");
                                        logNsend.Add("[" + DateTime.Now + "] El Documento: " + oFactura.NombreArchivoXML + " No Contiene un Mail Destino, tampoco en la base de datos.");
                                    }
                                }
                                else if (xmldoc.Email.Para.Contains("@"))
                                {
                                    bool result = false;

                                    result = SendArchiveClient(xmldoc.Email, oEmpresa, oFactura);

                                    if (result == true)
                                    {
                                        Console.WriteLine("[" + DateTime.Now + "] El Documento: " + xmldoc.NombreArchivoXML + " Se a Enviado Correctamente!");
                                        oListDocumentoUpdate.Insert(0, new FacturaElectronica() { IdFactura = xmldoc.IdFactura, NombreArchivoXML = xmldoc.NombreArchivoXML });

                                        oDocSend = new Documento();
                                        oDocSend.Empresa = new Empresa();
                                        oDocSend.TipoDocumento = new TipoDocumento();
                                        oDocSend.TipoDocumento.CodigoDocumento = oFactura.TipoDocumento.CodigoDocumento;
                                        oDocSend.Serie = oFactura.NumeroSerie;
                                        oDocSend.NumeroDocumento = oFactura.NumeroDocumento;
                                        //oDocSend.Destino = para;
                                        oDocSend.Destino = xmldoc.Email.Para;
                                        oDocSend.Asunto = asunto;
                                        oDocSend.Mensaje = string.Empty;
                                        oDocSend.Remitente = oFactura.Empresa.RazonSocial;
                                        oDocSend.Fecha_Cad = Convert.ToDateTime(DateTime.Now);
                                        oDocSend.FechaEnvio = DateTime.Now.ToString();
                                        oDocSend.Empresa.RUC = oFactura.Empresa.RUC;
                                        oListDocSend.Add(oDocSend);

                                        logsend.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " Se a Enviado Correctamente!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("[" + DateTime.Now + "] El Documento: " + xmldoc.NombreArchivoXML + " No se a Enviado.");
                                        logNsend.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " No se a Enviado.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("[" + DateTime.Now + "] El Documento: " + oFactura.NombreArchivoXML + " No Contiene un Mail Destino, tampoco en la base de datos.");
                                    logNsend.Add("[" + DateTime.Now + "] El Documento: " + oFactura.NombreArchivoXML + " No Contiene un Mail Destino, tampoco en la base de datos.");
                                }

                                tpodocumento = string.Empty;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("[" + DateTime.Now + "] Error en el Doc. Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                                logError.Add("[" + DateTime.Now + "] Error en el Doc. Nro: " + NombreArchivo + ": " + ex.Message);
                            }
                            NombreArchivo = string.Empty;
                            REF_FILES = string.Empty;
                            #endregion END TRY ENVIO MAIL
                        }
                        else
                        {
                            Console.WriteLine("[" + DateTime.Now + "] No se encontro un correo o no hay ninguno con estado Activo, el Documento: " + xmldoc.NombreArchivoXML + " no se envió.");
                            logError.Add("[" + DateTime.Now + "] No se encontro un correo o no hay ninguno con estado Activo, El Documento: " + xmldoc.NombreArchivoXML + " no se envió.");
                            Console.WriteLine("[" + DateTime.Now + "] Fin del Proceso del Documento: " + ID_NUM_CE);
                        }

                        #endregion



                    }
                    else
                    {
                        Console.WriteLine("[" + DateTime.Now + "] El Ruc de Empresa del Documento Nro:  " + xmldoc.NombreArchivoXML + " no corresponde o no tiene Licencia para este Proceso.");
                        logError.Add("[" + DateTime.Now + "] El Ruc de Empresa del Documento Nro:  " + xmldoc.NombreArchivoXML + " no corresponde o no tiene Licencia para este Proceso.");
                    }
                    //Console.WriteLine("[" + DateTime.Now + "] Fin del Proceso del Documento: " + ID_NUM_CE);

                    #endregion
                }
                #endregion
            }
            var listaResultado = oListDocumento;
            return listaResultado;
        }

        #endregion


        #region SERIALIZAR XML

        XmlDocument xml = new XmlDocument();

        string pathxmltemp = string.Empty;
        public string NombreArchivo = string.Empty;
        //public string moneda = string.Empty;
        public string tpodocumento = string.Empty;
        public string TextoNexto = string.Empty;
        public string montoLiteral = string.Empty;

        public string REF_FILES = string.Empty;

        string nombrearchivoFile = string.Empty;

        XmlDocument XMLArchive = new XmlDocument();

        FacturaElectronica oFactura = new FacturaElectronica();
        ListaFacturaElectronica listamonto = new ListaFacturaElectronica();

        Cliente oClient = new Cliente();
        Empresa oEmpresa = new Empresa();

        StreamReader sr;

        #region OBTENER CE FROM XML FACTURA, BOLETA

        void ObtenerValoresXMLCE(string pathxml)
        {
            oFactura = new FacturaElectronica();
            try
            {
                XMLArchive.Load(pathxml + ".xml");

                //XmlNodeList xmlnodelist; //ALL
                //Inicializa();

                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlFac.InvoiceType));
                sr = new StreamReader(pathxml + ".xml");
                xmlFac.InvoiceType inv = (xmlFac.InvoiceType)xmlSerial.Deserialize(sr);

                oFactura = new Slin.Facturacion.Common.UtilCE.UsefullClassWS().GetDataFromXMLCE(inv, XMLArchive, NombreArchivo);

                sr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message);
            }
        }




        #endregion END CE

        #region NOTA DE CREDITO
        void ObtenerValoresXMLCENotCredit(string pathxml)
        {
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-PE");

            oFactura = new FacturaElectronica();

            try
            {
                XMLArchive.Load(pathxml + ".xml");//ALL

                //XMLArchive.InnerXml = XMLArchive.InnerXml.Replace("","");

                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlNotCred.CreditNoteType));
                sr = new StreamReader(pathxml + ".xml");
                xmlNotCred.CreditNoteType nc = (xmlNotCred.CreditNoteType)xmlSerial.Deserialize(sr);

                oFactura = new Slin.Facturacion.Common.UtilCE.UsefullClassWS().GetDataFromCNOTECredit(nc, XMLArchive, NombreArchivo, tpodocumento);

                sr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message);
                sr.Close();
            }
        }
        #endregion END NOTA DE CREDITO

        #region NOTA DE DEBITO
        void ObtenerValoresXMLCENotDebit(string pathxml)
        {
            oFactura = new FacturaElectronica();
            try
            {
                XMLArchive.Load(pathxml + ".xml");//ALL

                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlNotDeb.DebitNoteType));
                sr = new StreamReader(pathxml + ".xml");
                xmlNotDeb.DebitNoteType nd = (xmlNotDeb.DebitNoteType)xmlSerial.Deserialize(sr);

                oFactura = new Slin.Facturacion.Common.UtilCE.UsefullClassWS().GetDataFromCNOTEDebit(nd, XMLArchive, NombreArchivo, tpodocumento);

                sr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message);
                sr.Close();
            }
        }
        #endregion END NOTA DE DEBITO


        #region OBTENER CRE FROM XML

        void ObtenerValoresXMLCRE(string pathxml)
        {
            try
            {
                oFactura = new FacturaElectronica();

                XMLArchive.Load(pathxml + ".xml"); //XML

                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlCre.RetentionType));
                sr = new StreamReader(pathxml + ".xml");
                xmlCre.RetentionType ret = (xmlCre.RetentionType)xmlSerial.Deserialize(sr);

                oFactura = new Slin.Facturacion.Common.UtilCE.UsefullClassWS().GetDataFromXMLCRE(ret, XMLArchive, NombreArchivo, tpodocumento);
                sr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message);
                //logError.Add("[" + DateTime.Now + "] Error en el Nro: " + NombreArchivo + ": " + ex.Message);
                sr.Close();//
            }
        }

        #endregion


        #endregion END SERIALIZAR XML

        #region CREAR PDF

        private void CrearPdfSegunTpoDoc(FacturaElectronica oFact, string typeFormat)
        {
            CrearCodigoPDF417(oFact);
            LlenarListaMontos(oFact);
            EnviarParametrosRPT(oFact, typeFormat);
            CrearPDFandSave(PathPDF + NombreArchivo);
        }

        #region CODIGO PDF417

        void CrearCodigoPDF417(FacturaElectronica oFact)
        {
            try
            {

                new Slin.Facturacion.Common.UtilCE.ParametersClassWS().CrearCodigoPDF417(oFact, PathPDF417, NombreArchivo);

                #region

                //string contenidoCodigoPDF417 = string.Empty;
                //contenidoCodigoPDF417 = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetValueForCodePDF417(oFact);

                //BarcodePDF417 opdf417 = new BarcodePDF417();
                //opdf417.Options = BarcodePDF417.PDF417_USE_ASPECT_RATIO;
                //opdf417.ErrorLevel = 8;
                //opdf417.SetText(contenidoCodigoPDF417);
                //System.Drawing.Bitmap imagen = new System.Drawing.Bitmap(opdf417.CreateDrawingImage(Color.Black, Color.White));
                //imagen.Save(@PathPDF417 + NombreArchivo + ".bmp");

                ////Slin.Facturacion.Common.Helper.BarcodeClass code = new Slin.Facturacion.Common.Helper.BarcodeClass();
                ////System.Drawing.Graphics g = Graphics.FromImage(new Bitmap(1, 1));
                //////g.FillRectangle(new SolidBrush(Color.White), 0, 0, 20, 6 + 14);
                ////System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(1, 1, PixelFormat.Format32bppArgb);
                ////g = Graphics.FromImage(bmp);
                ////code.DrawCode128(g, (oFact.ListaExtra[8].ExDato.Length > 0 ? oFact.ListaExtra[8].ExDato : "-"), 0, 0).Save(@PathPDF417 + NombreArchivo + "_Barcode.bmp", ImageFormat.Bmp);


                //string valueCodeBar128 = string.Empty;
                //#region val for codebar128
                //try
                //{
                //    //valueCodeBar128 = oFact.ListaExtra[8].ExDato;
                //    if (oFact.ListaExtra[8].ExDato != null && oFact.ListaExtra[8].ExDato.Length > Constantes.ValorCero)
                //    {
                //        valueCodeBar128 = oFact.ListaExtra[8].ExDato;
                //    }
                //}
                //catch (Exception ex) { valueCodeBar128 = "000"; }
                //#endregion

                //if (valueCodeBar128.Length > 15)
                //{
                //    valueCodeBar128 = valueCodeBar128.Substring(0, 15);
                //}

                //BarcodeLib.Barcode Codigo = new BarcodeLib.Barcode();
                //Codigo.IncludeLabel = true;
                //System.Drawing.Bitmap bmp128 = new System.Drawing.Bitmap(Codigo.Encode(BarcodeLib.TYPE.CODE128, valueCodeBar128, Color.Black, Color.White, 400, 110));
                ////System.Drawing.Bitmap bmp128 = new System.Drawing.Bitmap(Codigo.Encode(BarcodeLib.TYPE.CODE128, (oFact.ListaExtra[8].ExDato.Length > 0 ? oFact.ListaExtra[8].ExDato : "-"), Color.Black, Color.White, 400, 110));
                //bmp128.Save(@PathPDF417 + NombreArchivo + "_Barcode.bmp");


                #endregion
            }
            catch (Exception ex)
            {
                //BarcodeLib.Barcode Codigo = new BarcodeLib.Barcode();
                //Codigo.IncludeLabel = true;
                //System.Drawing.Bitmap bmp128 = new System.Drawing.Bitmap(Codigo.Encode(BarcodeLib.TYPE.CODE128, "000", Color.Black, Color.White, 400, 110));
                //bmp128.Save(@PathPDF417 + NombreArchivo + "_Barcode.bmp");
            }
        }

        #endregion END CODIGO PDF417

        #region LLENAR LISTA MONTOS

        void LlenarListaMontos(FacturaElectronica oFact)
        {
            try
            {
                string RutaPDF417 = Path.Combine(PathPDF417 + NombreArchivo + ".bmp");
                string RutaBarcode = Path.Combine(PathPDF417 + NombreArchivo + "_Barcode.bmp");

                listamonto = new ListaFacturaElectronica();
                listamonto = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetListaMontos(oFact, RutaPDF417, RutaBarcode);
            }
            catch (Exception ex)
            {
                logError.Add("[" + DateTime.Now + "] Error en el Nro: " + NombreArchivo + ": " + ex.Message);
            }
        }

        #endregion END LLENAR LISTA MONTOS

        #region ENVIAR PARAMETRO REPORTVIEWER

        void EnviarParametrosRPT(FacturaElectronica oFact, string typeFormat)
        {
            try
            {
                ReportGR = new ReportViewer();
                #region SEGUN TYPE FORMAT

                //if (int.Parse(typeFormat) == Constantes.ValorUno)
                //{
                //    #region TYPE TICKET
                //    switch (oFact.TipoDocumento.CodigoDocumento)
                //    {
                //        case Constantes.Factura:
                //        case Constantes.Boleta:
                //            {
                //                if (oFact.TipoDocumento.CodigoDocumento == Constantes.Factura)
                //                {
                //                    ReportGR.LocalReport.ReportPath = PathReporte + "Fact_T_" + oFact.Empresa.RUC + ".rdlc";
                //                }
                //                else
                //                {
                //                    ReportGR.LocalReport.ReportPath = PathReporte + "Bol_T_" + oFact.Empresa.RUC + ".rdlc";
                //                    if (oFact.Cliente.ClienteRuc.Length == 0)
                //                    {
                //                        oFact.Cliente.ClienteRuc = oFact.Cliente.RazonSocial;
                //                    }
                //                }
                //                RPTParameterCE(oFact);
                //                break;
                //            }
                //        case Constantes.NotaCredito:
                //            {
                //                ReportGR.LocalReport.ReportPath = PathReporte + "NotC_T_" + oFact.Empresa.RUC + ".rdlc";
                //                RPTParameterNOTCRED(oFact);
                //                break;
                //            }
                //        case Constantes.NotaDebito:
                //            {
                //                ReportGR.LocalReport.ReportPath = PathReporte + "NotD_T_" + oFact.Empresa.RUC + ".rdlc";
                //                RPTParameterNOTDEB(oFact);
                //                break;
                //            }
                //        case Constantes.Retencion:
                //            {
                //                ReportGR.LocalReport.ReportPath = PathReporte + "Ret_T_" + oFact.Empresa.RUC + ".rdlc";
                //                RPTParameterCRE(oFact);
                //                break;
                //            }

                //        case Constantes.Percepcion:
                //            {
                //                ReportGR.LocalReport.ReportPath = PathReporte + "Per_T_" + oFact.Empresa.RUC + ".rdlc";
                //                RPTParameterCPE(oFact);
                //                break;
                //            }
                //    }
                //    #endregion END TYPE FORMAT TICKET
                //}
                //else if (int.Parse(typeFormat) == Constantes.ValorDos)
                //{
                //    #region FORMAT PDF HIGH
                //    switch (oFact.TipoDocumento.CodigoDocumento)
                //    {
                //        case Constantes.Factura:
                //        case Constantes.Boleta:
                //            {
                //                if (oFact.TipoDocumento.CodigoDocumento == Constantes.Factura)
                //                {
                //                    ReportGR.LocalReport.ReportPath = PathReporte + "Fact_" + oFact.Empresa.RUC + ".rdlc";
                //                }
                //                else
                //                {
                //                    ReportGR.LocalReport.ReportPath = PathReporte + "Bol_" + oFact.Empresa.RUC + ".rdlc";
                //                    if (oFact.Cliente.ClienteRuc.Length == 0)
                //                    {
                //                        oFact.Cliente.ClienteRuc = oFact.Cliente.RazonSocial;
                //                    }
                //                }
                //                RPTParameterCE(oFact);
                //                break;
                //            }
                //        case Constantes.NotaCredito:
                //            {
                //                ReportGR.LocalReport.ReportPath = PathReporte + "NotC_" + oFact.Empresa.RUC + ".rdlc";
                //                RPTParameterNOTCRED(oFact);
                //                break;
                //            }
                //        case Constantes.NotaDebito:
                //            {
                //                ReportGR.LocalReport.ReportPath = PathReporte + "NotD_" + oFact.Empresa.RUC + ".rdlc";
                //                RPTParameterNOTDEB(oFact);
                //                break;
                //            }
                //        case Constantes.Retencion:
                //            {
                //                ReportGR.LocalReport.ReportPath = PathReporte + "Ret_" + oFact.Empresa.RUC + ".rdlc";
                //                RPTParameterCRE(oFact);
                //                break;
                //            }

                //        case Constantes.Percepcion:
                //            {
                //                ReportGR.LocalReport.ReportPath = PathReporte + "Per_" + oFact.Empresa.RUC + ".rdlc";
                //                RPTParameterCPE(oFact);
                //                break;
                //            }
                //    }
                //    #endregion
                //}
                #endregion
                //ReportGR.LocalReport.ReportPath = PathReporte;
                ReportGR.LocalReport.ReportPath = PathReporte + new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetPathReportviewer(oFact, int.Parse(typeFormat));
                RPTParameterCE(oFact);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Nro: " + NombreArchivo + ": " + ex.Message);
            }
        }

        #endregion END ENVIAR PARAMETROS REPORTVIEWER

        #region PASAR PARAMETROS RPT SEGUN TPO DOC

        void RPTParameterCE(FacturaElectronica oFact)
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
                            Dpr = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersCE(oFact, listamonto);

                            ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oFact.ListaDetalleFacturaElectronica));
                            ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                            break;
                        }
                    case Constantes.NotaCredito:
                        {
                            Dpr = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersNotCred(oFact, listamonto);

                            ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oFact.ListaDetalleFacturaElectronica));
                            ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                            ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDocAfectado", oFact.ListaAfectado));
                            break;
                        }
                    case Constantes.NotaDebito:
                        {
                            Dpr = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersNotDebit(oFact, listamonto);

                            ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oFact.ListaDetalleFacturaElectronica));
                            ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                            ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDocAfectado", oFact.ListaAfectado));
                            break;
                        }
                    case Constantes.Retencion:
                        {
                            Dpr = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersRetenc(oFact);

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

                //ReportParameter[] Dpr = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersCE(oFact, listamonto);

                //ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oFact.ListaDetalleFacturaElectronica));
                //ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                //ReportGR.LocalReport.SetParameters(Dpr);
                //ReportGR.LocalReport.Refresh();

                //ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", objlistadetalle));
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message);
            }

        }

        void RPTParameterNOTCRED(FacturaElectronica oFact)
        {
            try
            {
                ReportParameter[] Dpr = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersNotCred(oFact, listamonto);

                ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oFact.ListaDetalleFacturaElectronica));
                ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDocAfectado", oFact.ListaAfectado));
                ReportGR.LocalReport.SetParameters(Dpr);
                ReportGR.LocalReport.Refresh();

                //ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", objlistadetalle));
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message);
            }
        }

        void RPTParameterNOTDEB(FacturaElectronica oFact)
        {
            try
            {
                ReportParameter[] Dpr = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersNotDebit(oFact, listamonto);

                ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oFact.ListaDetalleFacturaElectronica));
                ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDocAfectado", oFact.ListaAfectado));
                ReportGR.LocalReport.SetParameters(Dpr);
                ReportGR.LocalReport.Refresh();

                //ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", objlistadetalle));
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message);
            }
        }

        void RPTParameterCRE(FacturaElectronica oFact)
        {
            try
            {
                ReportParameter[] Dpr = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersRetenc(oFact);

                ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDetCRECPE", oFact.ListaDocCRECPE));
                ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                ReportGR.LocalReport.SetParameters(Dpr);
                ReportGR.LocalReport.Refresh();

                //ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDetCRECPE", objlistaDocCRECPE));
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message);
            }
        }

        void RPTParameterCPE(FacturaElectronica oFact)
        {

        }

        void RPTParameterGUIAREM(FacturaElectronica oFact)
        {

        }

        #endregion


        private void CrearPDFandSave(string pathNombreArchivo)
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
                Console.WriteLine("[" + DateTime.Now + "] Error en el Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Nro: " + NombreArchivo + ": " + ex.Message);
            }
        }

        #endregion

        #region SEND EMAIL WITH ARCHIVES

        private bool SendArchiveClient(Email objEmail, Empresa objEmpresa, FacturaElectronica oDoc)
        {
            bool result = false;
            result = EnviarEmail(objEmail, objEmpresa, oDoc);
            return result;
        }

        //public string para, cc, cco, asunto, usuario, password, remite;
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
                case Constantes.Factura:
                    {
                        asunto += "FACTURA - ";
                        break;
                    }
                case Constantes.Boleta:
                    {
                        asunto += "BOLETA DE VENTA ELECTRÓNICA - ";
                        break;
                    }
                case Constantes.NotaCredito:
                    {
                        asunto += "NOTA DE CRÉDITO ELECTRÓNICA - ";
                        break;
                    }
                case Constantes.NotaDebito:
                    {
                        asunto += "NOTA DE DÉBITO ELECTRÓNICA- ";
                        break;
                    }
                case Constantes.Retencion:
                    {
                        asunto += "COMPROBANTE DE RETENCIÓN ELECTRÓNICA - ";
                        break;
                    }
                case Constantes.Percepcion:
                    {
                        asunto += "COMPROBANTE DE PERCEPCIÓN ELECTRÓNICA - ";
                        break;
                    }
                case Constantes.GuiaRemision:
                    {
                        asunto += "GUIA DE REMISIÓN ELECTRÓNICA - ";
                        break;
                    }
            }
            #endregion

            asunto += oDoc.SerieCorrelativo + " - " + oDoc.Empresa.RazonSocial;

            //usuario = objEmp.Email;
            objEmp.Password = new Helper.Encrypt().DecryptKey(objEmp.Password);


            //remite = oFactura.Empresa.RazonSocial;

            //int porto = 587;
            //result = SMTPMail(para, cc, cco, asunto, usuario, password, remite, porto, oDoc);
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
                String[] AMailto = objEmail.Para.Split(';');

                foreach (String email in AMailto)
                {
                    mail.To.Add(new MailAddress(email));
                }


                if (objEmail.CC.Length > 0 && objEmail.CC.Contains("@"))
                {
                    string[] CCMail = objEmail.CC.Split(';');

                    foreach (string cc in CCMail)
                    {
                        mail.CC.Add(new MailAddress(cc.TrimStart().TrimEnd()));
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

                //mail.Bcc.Add(new MailAddress("slin@slin.com.pe"));




                mail.From = new System.Net.Mail.MailAddress(objEmpresa.Email, oDocF.Empresa.RazonSocial, System.Text.Encoding.UTF8);
                mail.Subject = pAsunto;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;

                mail.Body = TemplateMail(oFactura);

                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;


                // Agregar el Adjunto si deseamos hacerlo
                //mail.Attachments.Add(new Attachment(@pathxmltemp + ".xml", System.Net.Mime.MediaTypeNames.Application.Soap));
                //mail.Attachments.Add(new Attachment(@pathxmltemp + ".pdf", System.Net.Mime.MediaTypeNames.Application.Pdf));

                mail.Attachments.Add(new Attachment(PathXML + NombreArchivo + ".xml"));
                mail.Attachments.Add(new Attachment(PathPDF + NombreArchivo + ".pdf"));

                if (REF_FILES.Length > 0)
                {
                    string[] Files = REF_FILES.Split(';');
                    foreach (string file in Files)
                    {
                        var file_in = Path.Combine(file);
                        if (System.IO.File.Exists(file_in))
                        {
                            mail.Attachments.Add(new Attachment(file_in));
                        }
                    }
                }

                // Configuración SMTP
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };


                #region OTHER METHOD MAIL

                //if (objEmpresa.Email.Contains("hotmail") || objEmpresa.Email.Contains("outlook"))
                //{
                //    //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp-mail.outlook.com", 587);
                //    smtp = new System.Net.Mail.SmtpClient("smtp-mail.outlook.com", objEmpresa.Port);//587
                //    smtp.EnableSsl = true;
                //}
                //else if (objEmpresa.Email.Contains("gmail"))
                //{
                //    //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                //    smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", objEmpresa.Port);
                //    smtp.EnableSsl = true;
                //}
                //else if (objEmpresa.Email.Contains("slin"))
                //{
                //    smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", objEmpresa.Port);
                //    smtp.EnableSsl = true;
                //}
                //else if (objEmpresa.Email.Contains("ancro"))
                //{
                //    smtp = new System.Net.Mail.SmtpClient("190.107.180.68", 25);
                //    smtp.EnableSsl = false;
                //}
                //else
                //{
                //    smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", objEmpresa.Port);
                //    smtp.EnableSsl = true;
                //}
                #endregion END OTHER METHOD MAIL


                if (objEmpresa.Dominio.Contains("gmail"))
                {
                    smtp = new System.Net.Mail.SmtpClient(objEmpresa.Dominio, objEmpresa.Port);//port 587
                    smtp.EnableSsl = true;
                }
                else
                {
                    //smtp = new System.Net.Mail.SmtpClient(objEmpresa.IP, objEmpresa.Port);
                    smtp = new System.Net.Mail.SmtpClient(objEmpresa.Dominio, objEmpresa.Port);
                    //smtp.EnableSsl = true;

                    smtp.EnableSsl = objEmpresa.UseSSL == Constantes.ValorUno ? true : false;
                }


                // Crear Credencial de Autenticacion
                smtp.Credentials = new System.Net.NetworkCredential(objEmpresa.Email, objEmpresa.Password);
                //smtp.EnableSsl = false;

                try
                {
                    smtp.Send(mail);
                    result = true;
                }
                catch (Exception ex)
                {
                    //throw ex;
                    Console.WriteLine("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                    logError.Add("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                    result = false;
                }
            } // end using mail

            return result;

        }// end SMTPMail



        private string TemplateMail(FacturaElectronica oDoc)
        {
            string body = string.Empty;
            //using (StreamReader reader = new StreamReader(PathHtml))
            //{
            //    body = reader.ReadToEnd();
            //}


            var filehtml = Path.Combine(PathHtml + oDoc.Empresa.RUC + ".html");
            if (System.IO.File.Exists(filehtml))
            {
                using (StreamReader reader = new StreamReader(PathHtml + oDoc.Empresa.RUC + ".html"))
                {
                    body = reader.ReadToEnd();
                }


                body = body.Replace("{RazonSocialEmisor}", oDoc.Empresa.RazonSocial.ToUpper());
                body = body.Replace("{ClienteRazonSocial}", oDoc.Cliente.RazonSocial);
                body = body.Replace("{SerieCorrelativo}", oDoc.SerieCorrelativo);

                switch (oDoc.TipoDocumento.CodigoDocumento)
                {
                    case Constantes.Factura:
                        {
                            body = body.Replace("{TipoDocumento}", "FACTURA ELECTRÓNICA");
                            body = body.Replace("{Moneda}", oDoc.TipoMoneda);
                            body = body.Replace("{MontoTotal}", oDoc.MontoTotalCad);
                            //body = body.Replace("{MontoTotal}", string.Empty + Convert.ToDecimal(oDoc.MontoTotalCad, CultureInfo.CreateSpecificCulture("es-PE")));
                            break;
                        }
                    case Constantes.Boleta:
                        {
                            body = body.Replace("{TipoDocumento}", "BOLETA DE VENTA ELECTRÓNICA");
                            body = body.Replace("{Moneda}", oDoc.TipoMoneda);
                            body = body.Replace("{MontoTotal}", oDoc.MontoTotalCad);
                            break;
                        }
                    case Constantes.NotaCredito:
                        {
                            body = body.Replace("{TipoDocumento}", "NOTA DE CRÉDITO ELECTRÓNICA");
                            body = body.Replace("{Moneda}", oDoc.TipoMoneda);
                            body = body.Replace("{MontoTotal}", oDoc.MontoTotalCad);
                            //body = body.Replace("{MontoTotal}", decimal.Parse(oDoc.MontoTotalCad, CultureInfo.CreateSpecificCulture("es-PE")) + string.Empty);
                            break;
                        }
                    case Constantes.NotaDebito:
                        {
                            body = body.Replace("{TipoDocumento}", "NOTA DE DÉBITO ELECTRÓNICA");
                            body = body.Replace("{Moneda}", oDoc.TipoMoneda);
                            body = body.Replace("{MontoTotal}", oDoc.MontoTotalCad);
                            break;
                        }
                    case Constantes.Retencion:
                        {
                            body = body.Replace("{TipoDocumento}", "COMPROBANTE DE RETENCIÓN ELECTRÓNICA");
                            body = body.Replace("{Moneda}", "Soles");
                            body = body.Replace("{MontoTotal}", oDoc.DocCRECPE.ImporteTotalPagado + string.Empty);
                            break;
                        }
                    case Constantes.Percepcion:
                        {
                            body = body.Replace("{TipoDocumento}", "COMPROBANTE DE PERCEPCIÓN ELECTRÓNICA");
                            body = body.Replace("{Moneda}", "Soles");
                            body = body.Replace("{MontoTotal}", oDoc.DocCRECPE.ImporteTotalPagado + string.Empty);
                            break;
                        }
                    case Constantes.GuiaRemision:
                        {
                            body = body.Replace("{TipoDocumento}", "GUIA DE REMISIÓN ELECTRÓNICA");
                            break;
                        }
                }
                body = body.Replace("{FechaEmision}", oDoc.FechaEmision.ToString("dd/MM/yyyy"));

            }
            else
            {
                using (StreamReader reader = new StreamReader(PathHtml + "Default.html"))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{RazonSocialEmisor}", oDoc.Empresa.RazonSocial.ToUpper());
                body = body.Replace("{ClienteRazonSocial}", oDoc.Cliente.RazonSocial);
                body = body.Replace("{SerieCorrelativo}", oDoc.SerieCorrelativo);

                switch (oDoc.TipoDocumento.CodigoDocumento)
                {
                    case Constantes.Factura:
                        {
                            body = body.Replace("{TipoDocumento}", "FACTURA ELECTRÓNICA");
                            break;
                        }
                    case Constantes.Boleta:
                        {
                            body = body.Replace("{TipoDocumento}", "BOLETA DE VENTA ELECTRÓNICA");
                            break;
                        }
                    case Constantes.NotaCredito:
                        {
                            body = body.Replace("{TipoDocumento}", "NOTA DE CRÉDITO ELECTRÓNICA");
                            break;
                        }
                    case Constantes.NotaDebito:
                        {
                            body = body.Replace("{TipoDocumento}", "NOTA DE DÉBITO ELECTRÓNICA");
                            break;
                        }
                    case Constantes.Retencion:
                        {
                            body = body.Replace("{TipoDocumento}", "COMPROBANTE DE RETENCIÓN ELECTRÓNICA");
                            break;
                        }
                    case Constantes.Percepcion:
                        {
                            body = body.Replace("{TipoDocumento}", "COMPROBANTE DE PERCEPCIÓN ELECTRÓNICA");
                            break;
                        }
                    case Constantes.GuiaRemision:
                        {
                            body = body.Replace("{TipoDocumento}", "GUIA DE REMISIÓN ELECTRÓNICA");
                            break;
                        }
                }

                body = body.Replace("{FechaEmision}", oDoc.FechaEmision.ToString("dd/MM/yyyy"));
                //body = body.Replace("{Moneda}", oDoc.TipoMoneda);
                //body = body.Replace("{MontoTotal}", oDoc.MontoTotalCad);

            }




            //var filehtml = Path.Combine(PathHtml + oDoc.Empresa.RUC + ".html");

            //if (System.IO.File.Exists(filehtml))
            //{
            //    body = body.Replace("{Moneda}", oDoc.TipoMoneda);
            //    body = body.Replace("{MontoTotal}", oDoc.MontoTotalCad);
            //}
            //else
            //{
            //    //body = body.Replace("{FechaEmision}", oDoc.FechaEmision.ToString("dd/MM/yyyy"));
            //    //body = body.Replace("{Moneda}", oDoc.TipoMoneda);
            //    //body = body.Replace("{MontoTotal}", oDoc.MontoTotalCad);
            //}

            return body;
        }

        #endregion
    }
}
