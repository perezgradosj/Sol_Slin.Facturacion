using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.ServiceImplementation;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;

using iTextSharp.text.pdf;

using Slin.Facturacion.Common;
//using Slin.Facturacion.Common.Serializer;
using xmlFac = Slin.Facturacion.Common.CE;
using xmlCre = Slin.Facturacion.Common.CRE;
using Microsoft.Reporting.WinForms;

using xmlNotCred = Slin.Facturacion.Common.CENC;
using xmlNotDeb = Slin.Facturacion.Common.CEND;
using xmlRead = Slin.Facturacion.Common.Helper;

using System.Threading;

//QUITAR
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Diagnostics;
using System.Timers;


using System.Windows;
using System.Drawing.Imaging;

namespace Slin.Facturacion.PrintDoc
{
    class Program
    {
        public string PathReporte = ConfigurationManager.AppSettings["PathReporte"].ToString();

        public string PathListDocNotPrint = ConfigurationManager.AppSettings["PathListDocNotPrint"].ToString();
        public string PathListDocPrint = ConfigurationManager.AppSettings["PathListDocPrint"].ToString();
        public string PathErroLog = ConfigurationManager.AppSettings["PathErroLog"].ToString();

        public string PathPDF = ConfigurationManager.AppSettings["PathPDF"].ToString();
        public string PathPDF417 = ConfigurationManager.AppSettings["PathPDF417"].ToString();
        public string PathXML = ConfigurationManager.AppSettings["PathXML"].ToString();

        public string PathLogSLINADE = ConfigurationManager.AppSettings["PathLogSLINADE"].ToString();
        public string PathDocForPrint = ConfigurationManager.AppSettings["PathDocForPrint"].ToString();

        public string ValidateWithDataBase = ConfigurationManager.AppSettings["ValidateWithDataBase"].ToString();
        private string ValidatePrintResume = ConfigurationManager.AppSettings["ValidatePrintResume"].ToString();

        //StringBuilder logNsend = new StringBuilder();
        //StringBuilder logsend = new StringBuilder();
        //StringBuilder logError = new StringBuilder();

        List<string> logNprint = new List<string>();
        List<string> logPrint = new List<string>();
        List<string> logError = new List<string>();


        //string entityIdRucEncrypt = "t6U5yI/3RJUlfJIc1RVpSg==";//ancro
        //string entityIdRucEncrypt = "+j4hnjICO0lWKUW5T4zeVQ==";//slin
        //string entityIdRucEncrypt = "GKut1XliNKJ1uHZpF8LchA==";//tecni services
        //string entityIdRucEncrypt = "e+xtM8QKrNB1sZLqkpO5LQ==";//PWC
        //string entityIdRucEncrypt = "1q7ew7gL0RtiuizQVmG8jw==";//GAVEGLIO

        string entityIdRucDesencrypt = string.Empty;//all entities

        static void Main(string[] args)
        {
            new Program().FileInfoList();
            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message + ", " + ex.InnerException);
            //}
        }


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

        private xmlRead.DocumentState xmldoctype;
        public xmlRead.DocumentState XmlDocType
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

        //private static ReportViewer reportGR2;
        //public static ReportViewer ReportGR2
        //{
        //    get { return reportGR2; }
        //    set { reportGR2 = value; }
        //}

        //private List<Documento> olistDocprint;
        //public List<Documento> oListDocPrint
        //{
        //    get { return olistDocprint; }
        //    set { olistDocprint = value; }
        //}

        private Documento odocsend;
        public Documento oDocSend
        {
            get { return odocsend; }
            set { odocsend = value; }
        }

        #endregion

        #region DETECTA AL INICIAR

        void FileInfoList()
        {
            CreateDirectoryLog(PathLogSLINADE + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\");
            CreateDirectoryLog(PathListDocNotPrint + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\");
            CreateDirectoryLog(PathListDocPrint + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\");
            CreateDirectoryLog(PathErroLog + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\");

            try
            {
                List<string> listaDetected = new List<string>();

                listaDetected = new List<string>();

                DirectoryInfo di = new DirectoryInfo(PathDocForPrint);
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
                    //Console.WriteLine("[" + DateTime.Now + "]  Eliminando Archivo: " + del);
                    //Console.ReadLine();
                    var file = Path.Combine(del);
                    if (System.IO.File.Exists(file))
                        System.IO.File.Delete(file);
                }
                new Program().IniciarWatcher();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "]  Error al Inicio de Iniciar el Servicio: " + ex.Message + ", " + ex.InnerException);
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
                watcher.Path = PathDocForPrint;

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

                        Console.WriteLine("[" + DateTime.Now + "] Se a copiado el Archivo, " + path);
                        return true;
                    }

                }
                catch (IOException ioex)
                {
                    attemptsMade++;
                    Thread.Sleep(5000);
                    //Log.WriteLine($"Archivo {path} aún no termina de ser copiado.");
                    Console.WriteLine("[" + DateTime.Now + "] Error al Detectar Archivo: " + path + ", " + ioex.Message + ", " + ioex.InnerException);
                    //Console.ReadLine();
                }
            }
            return fileReady;
        }

        private void Inicializador()
        {
            logPrint = new List<string>();
            logNprint = new List<string>();
            logError = new List<string>();
            oListDocumentoUpdate = new ListaFacturaElectronica();
            //oListDocPrint = new List<Documento>();
            XmlDocType = new xmlRead.DocumentState();
        }

        private void IniciaProcesoWith_Xml_Small(string PathRutaXmlState)
        {
            try
            {
                Inicializador();

                var objeto = new xmlRead.DocumentState();
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
                    //PrintDocument(XmlDocType.ID_CE);
                    PrintDocument(objeto.ID_CE, objeto.Copies);
                }
                else if (objeto.RucEmisor != null && objeto.RucEmisor.Length == 11)
                {
                    string[] matrix = PathRutaXmlState.Split('\\');
                    string value = matrix[6];
                    logError.Add("[" + DateTime.Now + "] El Ruc de Empresa del Documento Nro:  " + objeto.ID_CE + " no corresponde o no tiene Licencia para este Proceso.");
                    logNprint.Add("[" + DateTime.Now + "] El Documento Nro: " + objeto.ID_CE + ", No se a Impreso, Revise el Error en la sgte Ubicación: " + PathErroLog + value);
                    Console.WriteLine("[" + DateTime.Now + "] El Ruc de Empresa del Documento Nro:  " + objeto.ID_CE + " no corresponde o no tiene Licencia para este Proceso.");
                }
                else
                {
                    string[] matrix = PathRutaXmlState.Split('\\');

                    string value = matrix[6];

                    logNprint.Add("[" + DateTime.Now + "] El Documento Nro: " + PathRutaXmlState + ", No se a Impreso, Revise el Error en la Sgte Ubicación: " + PathErroLog + value);
                }


                if (logNprint.Count > 0)
                {
                    foreach (var line in logNprint)
                    {
                        using (StreamWriter sw = new StreamWriter(PathListDocNotPrint + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\ListaDocNotPrint.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }

                if (logPrint.Count > 0)
                {
                    foreach (var line in logPrint)
                    {
                        using (StreamWriter sw = new StreamWriter(PathListDocPrint + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\ListaDocPrint.log", true, Encoding.UTF8))
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
                    string msjUpdate = new ServicioFacturacionSOA().UpdateDocCabStatusPrint(oListDocumentoUpdate);
                }

                //if (oListDocPrint.Count > 0)
                //{
                //    string msjInserted = ServicioFacturacionSOA.Instance).InsertarListDocEnviado(oListDocPrint);
                //}

                var file = Path.Combine(PathRutaXmlState);
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);

                Console.WriteLine("[" + DateTime.Now + "] Eliminando Archivo: " + PathRutaXmlState);
                Console.WriteLine("[" + DateTime.Now + "] Fin del Proceso de Impresión para el Documento: " + objeto.ID_CE);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error el Procesar el Archivo: " + PathRutaXmlState + ", " + ex.Message + ", " + ex.InnerException);
                //Console.ReadLine();
            }
        }

        private xmlRead.DocumentState LeerDataXml(string PathRutaXmlState)
        {
            try
            {
                Console.WriteLine("[" + DateTime.Now + "] Inicio de Lectura de Archivo: " + PathRutaXmlState);
                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlRead.DocumentState));
                sr = new StreamReader(PathRutaXmlState);
                xmlRead.DocumentState xmlread = (xmlRead.DocumentState)xmlSerial.Deserialize(sr);

                XmlDocType = new xmlRead.DocumentState();

                XmlDocType.ID_CE = xmlread.ID_CE;
                XmlDocType.Tipo_CE = xmlread.Tipo_CE;
                XmlDocType.Estado_Doc = xmlread.Estado_Doc;
                XmlDocType.RucEmisor = xmlread.RucEmisor;
                XmlDocType.PrintName = xmlread.PrintName;
                XmlDocType.TypeFormat = xmlread.TypeFormat;
                XmlDocType.Copies = xmlread.Copies;

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

        #region FOR PRINT

        //private ListaFacturaElectronica PrintDocument(string ID_NUM_CE)
        private void PrintDocument(string ID_NUM_CE, string copies)
        {


            pathxmltemp = string.Empty;
            try
            {
                logPrint = new List<string>();
                logNprint = new List<string>();
                logError = new List<string>();

                oListDocumentoUpdate = new ListaFacturaElectronica();
                CrearNuevaCarpeta();

                pathxmltemp = PathXML + ID_NUM_CE;
                //string rutaAchivosOutput = string.Empty;

                if (ValidateWithDataBase == Constantes.ValorSI)
                {
                    #region METHOD PRINT WITH VALIDATE DATABASE

                    oListDocumento = new ListaFacturaElectronica();
                    oListDocumento = new ServicioFacturacionSOA().GetListDocumentoPrint_Parameter(ID_NUM_CE);


                    if (oListDocumento.Count == Constantes.ValorUno)
                    {
                        Console.WriteLine("[" + DateTime.Now + "] Se Obtuvo un Documento de la BD con ID: " + ID_NUM_CE);

                        Console.WriteLine("[" + DateTime.Now + "] Iniciando el Proceso de Impresion del Documento: " + ID_NUM_CE);
                        Console.WriteLine("[" + DateTime.Now + "] Verificando si Existe el Archivo: " + pathxmltemp + ".xml");

                        if (System.IO.File.Exists(PathXML + XmlDocType.ID_CE + ".xml"))
                        {
                            Console.WriteLine("[" + DateTime.Now + "] Archivo XML del Doc.: " + pathxmltemp + ".xml" + ", Existe!.");

                            //SI EXISTE EL XML EB LA RUTA
                            #region IF FILE XML EXISTS

                            //VALIDA EL TIPO DE FORMATO A PROCESAR
                            if (int.Parse(XmlDocType.TypeFormat) == Constantes.ValorUno)
                            {

                                #region FORMATO TERMICO

                                Console.WriteLine("[" + DateTime.Now + "] El Tipo de Formato a Procesar es para la Impresora Termica.");
                                NombreArchivo = XmlDocType.ID_CE;
                                tpodocumento = XmlDocType.Tipo_CE;
                                pathxmltemp = PathXML + XmlDocType.ID_CE;

                                bool resultPrint = false;
                                bool resultReadXml = false;

                                Console.WriteLine("[" + DateTime.Now + "]  Leyendo Archivo: " + pathxmltemp + ".xml");

                                #region CASE TPO DOCUMENTO FOR READ XML
                                switch (XmlDocType.Tipo_CE)
                                {
                                    case Constantes.Factura:
                                    case Constantes.Boleta:
                                        {
                                            resultReadXml = ObtenerValoresXMLCE(pathxmltemp);
                                            break;
                                        }

                                    case Constantes.NotaCredito:
                                        {
                                            resultReadXml = ObtenerValoresXMLCENotCredit(pathxmltemp);
                                            break;
                                        }
                                    case Constantes.NotaDebito:
                                        {
                                            resultReadXml = ObtenerValoresXMLCENotDebit(pathxmltemp);
                                            break;
                                        }

                                    case Constantes.Retencion:
                                        {
                                            resultReadXml = ObtenerValoresXMLCRE(pathxmltemp); // PARA EL PDF
                                            //Console.WriteLine("[" + DateTime.Now + "]  Fin de Lectura del Archivo: " + pathxmltemp + ".xml");
                                            //CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
                                            //resultPrint = CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
                                            break;
                                        }
                                    case Constantes.Percepcion:
                                        {
                                            //Console.WriteLine("[" + DateTime.Now + "]  Fin de Lectura del Archivo: " + pathxmltemp + ".xml");
                                            break;
                                        }
                                }

                                #endregion


                                #region VALIDA RESULTADOS (READ XML, PRINT DOCUMENT)
                                if (resultReadXml == true)
                                {
                                    resultPrint = CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat, copies);
                                    Console.WriteLine("[" + DateTime.Now + "]  Fin de Lectura del Archivo: " + pathxmltemp + ".xml = OK!");
                                }
                                else
                                {
                                    Console.WriteLine("[" + DateTime.Now + "] Error al Leer el Documento Nro: " + oFactura.NombreArchivoXML);
                                    logError.Add("[" + DateTime.Now + "] Error al Leer el Documento Nro: " + oFactura.NombreArchivoXML);
                                }

                                if (resultPrint == true)
                                {
                                    Console.WriteLine("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " Se a Impreso Correctamente!, Nombre Impresora: " + XmlDocType.PrintName);
                                    logPrint.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " Se a Impreso Correctamente!, Nombre Impresora: " + XmlDocType.PrintName);
                                }
                                else
                                {
                                    Console.WriteLine("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " No se a Impreso!, Nomb Imp: " + XmlDocType.PrintName);
                                    logNprint.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " No se a Impreso.");
                                }

                                #endregion

                                #endregion END FORMATO TERMICO
                            }
                            else if (int.Parse(XmlDocType.TypeFormat) == Constantes.ValorDos)
                            {
                                #region FORMATO PDF

                                Console.WriteLine("[" + DateTime.Now + "] El Tipo de Formato a Procesar es PDF");
                                Console.WriteLine("[" + DateTime.Now + "] Verificando si Existe el Archivo: " + PathPDF + NombreArchivo + ".pdf");

                                var PdfExists = Path.Combine(PathPDF + NombreArchivo + ".pdf");

                                bool resultPrint = false;
                                bool resultReadXml = false;
                                int Copies = Constantes.ValorUno;

                                if (!System.IO.File.Exists(PdfExists))
                                {
                                    #region SI EXISTENO EXISTE EL PDF
                                    bool resultCreatePDF = false;

                                    Console.WriteLine("[" + DateTime.Now + "] Archivo PDF del Doc.: " + XmlDocType.ID_CE + " No Existe!. ");

                                    NombreArchivo = XmlDocType.ID_CE;
                                    tpodocumento = XmlDocType.Tipo_CE;
                                    pathxmltemp = PathXML + XmlDocType.ID_CE;

                                    Console.WriteLine("[" + DateTime.Now + "]  Leyendo Archivo: " + pathxmltemp + ".xml");

                                    #region CASE TPO DOCUMENTO FOR PDF

                                    switch (XmlDocType.Tipo_CE)
                                    {

                                        case Constantes.Factura:
                                        case Constantes.Boleta:
                                            {
                                                resultReadXml = ObtenerValoresXMLCE(pathxmltemp);
                                                break;
                                            }
                                        case Constantes.NotaCredito:
                                            {
                                                resultReadXml = ObtenerValoresXMLCENotCredit(pathxmltemp);
                                                break;
                                            }
                                        case Constantes.NotaDebito:
                                            {
                                                resultReadXml = ObtenerValoresXMLCENotDebit(pathxmltemp);
                                                break;
                                            }

                                        case Constantes.Retencion:
                                            {
                                                resultReadXml = ObtenerValoresXMLCRE(pathxmltemp); // PARA EL PDF
                                                //Console.WriteLine("[" + DateTime.Now + "]  Fin de Lectura del Archivo: " + pathxmltemp + ".xml");
                                                //CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
                                                //resultPrint = CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
                                                break;
                                            }
                                        case Constantes.Percepcion:
                                            {
                                                break;
                                            }
                                    }

                                    #endregion

                                    if (resultReadXml == true)
                                    {
                                        resultCreatePDF = CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat, copies);
                                        Console.WriteLine("[" + DateTime.Now + "]  Fin de Lectura del Archivo: " + pathxmltemp + ".xml = OK!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("[" + DateTime.Now + "] Error al Leer el Documento Nro: " + oFactura.NombreArchivoXML);
                                        logError.Add("[" + DateTime.Now + "] Error al Leer el Documento Nro: " + oFactura.NombreArchivoXML);
                                    }


                                    if (resultCreatePDF == true)
                                    {
                                        //Si el archivo PDF se a Creado Correctamente manda a imprimir con foxit
                                        resultPrint = ImprimirDocument(PathPDF + XmlDocType.ID_CE + ".pdf", XmlDocType.PrintName, Copies);
                                        Console.WriteLine("[" + DateTime.Now + "] Se a Creado Correctamente el Pdf del Documento Nro: " + XmlDocType.ID_CE);
                                    }
                                    else
                                    {
                                        Console.WriteLine("[" + DateTime.Now + "] Error al Crear el Pdf del Documento Nro: " + XmlDocType.ID_CE);
                                        logError.Add("[" + DateTime.Now + "] Error al Crear el Pdf del Documento Nro: " + XmlDocType.ID_CE);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    Console.WriteLine("[" + DateTime.Now + "] Archivo PDF del Doc.: " + XmlDocType.ID_CE + " Existe!. ");
                                    resultPrint = ImprimirDocument(PathPDF + XmlDocType.ID_CE + ".pdf", XmlDocType.PrintName, Copies);
                                }

                                // VALIDA SI SE A REALIZADO LA IMPRESION
                                if (resultPrint == true)
                                {
                                    Console.WriteLine("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " Se a Impreso Correctamente!, Nombre Impresora: " + XmlDocType.PrintName);
                                    logPrint.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " Se a Impreso Correctamente!, Nombre Impresora: " + XmlDocType.PrintName);
                                }
                                else
                                {
                                    Console.WriteLine("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " No se a Impreso!, Nomb Imp: " + XmlDocType.PrintName);
                                    logNprint.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " No se a Impreso.");
                                }

                                #endregion
                            }

                            #endregion
                        }
                        else
                        {
                            // SI NO EXISTE EL XML EN LA RUTA
                            Console.WriteLine("[" + DateTime.Now + "] El Archivo XML del Documento: " + ID_NUM_CE + " no Existe!, " + PathXML + XmlDocType.ID_CE + ".xml");
                            logNprint.Add("[" + DateTime.Now + "] El Archivo XML del Documento: " + ID_NUM_CE + " no Existe!, " + PathXML + XmlDocType.ID_CE + ".xml");
                        }
                    }
                    else
                    {
                        Console.WriteLine("[" + DateTime.Now + "] El Documento Nro: " + ID_NUM_CE + " no existe en la Base de Datos: ");
                        logNprint.Add("[" + DateTime.Now + "] El Documento Nro: " + ID_NUM_CE + " no existe en la Base de Datos: ");
                        logNprint.Add("[" + DateTime.Now + "] El Documento Nro: " + ID_NUM_CE + " No se a Impreso!, Nomb Imp: " + XmlDocType.PrintName);
                        Console.WriteLine("[" + DateTime.Now + "] El Documento Nro: " + ID_NUM_CE + " No se a Impreso!, Nomb Imp: " + XmlDocType.PrintName);
                    }

                    #endregion END METHOD WITH VALIDATE DATABASE
                }
                else
                {
                    #region METHOD WITH FILE IN DIRECTORY

                    Console.WriteLine("[" + DateTime.Now + "] Iniciando el Proceso de Impresion del Documento: " + ID_NUM_CE);
                    Console.WriteLine("[" + DateTime.Now + "] Verificando si Existe el Archivo: " + pathxmltemp + ".xml");

                    if (System.IO.File.Exists(PathXML + XmlDocType.ID_CE + ".xml"))
                    {
                        Console.WriteLine("[" + DateTime.Now + "] Archivo XML del Doc.: " + pathxmltemp + ".xml" + ", Existe!.");

                        //SI EXISTE EL DOCUMENTO XML EN LA RUTA 
                        #region IF FILE XML EXISTS

                        //VALIDA EL TIPO DE FORMATO A PROCESAR
                        if (int.Parse(XmlDocType.TypeFormat) == Constantes.ValorUno)
                        {
                            #region FORMATO TERMICO
                            Console.WriteLine("[" + DateTime.Now + "] El Tipo de Formato a Procesar es para la Impresora Termica.");
                            NombreArchivo = XmlDocType.ID_CE;
                            tpodocumento = XmlDocType.Tipo_CE;
                            pathxmltemp = PathXML + XmlDocType.ID_CE;

                            bool resultPrint = false;
                            bool resultReadXml = false;

                            Console.WriteLine("[" + DateTime.Now + "]  Leyendo Archivo: " + pathxmltemp + ".xml");

                            #region CASE TPO DOCUMENTO FOR READ XML
                            switch (XmlDocType.Tipo_CE)
                            {
                                case Constantes.Factura:
                                case Constantes.Boleta:
                                    {
                                        resultReadXml = ObtenerValoresXMLCE(pathxmltemp);
                                        break;
                                    }

                                case Constantes.NotaCredito:
                                    {
                                        resultReadXml = ObtenerValoresXMLCENotCredit(pathxmltemp);
                                        break;
                                    }
                                case Constantes.NotaDebito:
                                    {
                                        resultReadXml = ObtenerValoresXMLCENotDebit(pathxmltemp);
                                        break;
                                    }
                                case Constantes.Retencion:
                                    {
                                        resultReadXml = ObtenerValoresXMLCRE(pathxmltemp); // PARA EL PDF
                                        //Console.WriteLine("[" + DateTime.Now + "]  Fin de Lectura del Archivo: " + pathxmltemp + ".xml");
                                        //CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
                                        //resultPrint = CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
                                        break;
                                    }
                                case Constantes.Percepcion:
                                    {
                                        //Console.WriteLine("[" + DateTime.Now + "]  Fin de Lectura del Archivo: " + pathxmltemp + ".xml");
                                        break;
                                    }
                            }
                            #endregion

                            if (resultReadXml == true)
                            {
                                resultPrint = CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat, copies);
                                Console.WriteLine("[" + DateTime.Now + "]  Fin de Lectura del Archivo: " + pathxmltemp + ".xml = OK!");
                            }
                            else
                            {
                                Console.WriteLine("[" + DateTime.Now + "] Error al Leer el Documento Nro: " + oFactura.NombreArchivoXML);
                                logError.Add("[" + DateTime.Now + "] Error al Leer el Documento Nro: " + oFactura.NombreArchivoXML);
                            }

                            if (resultPrint == true)
                            {
                                Console.WriteLine("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " Se a Impreso Correctamente!, Nombre Impresora: " + XmlDocType.PrintName);
                                logPrint.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " Se a Impreso Correctamente!, Nombre Impresora: " + XmlDocType.PrintName);
                            }
                            else
                            {
                                Console.WriteLine("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " No se a Impreso!, Nomb Imp: " + XmlDocType.PrintName);
                                logNprint.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " No se a Impreso.");
                            }

                            #endregion
                        }
                        else if (int.Parse(XmlDocType.TypeFormat) == Constantes.ValorDos)
                        {
                            #region FORMATO PDF
                            Console.WriteLine("[" + DateTime.Now + "] El Tipo de Formato a Procesar es PDF");
                            Console.WriteLine("[" + DateTime.Now + "] Verificando si Existe el Archivo: " + PathPDF + NombreArchivo + ".pdf");

                            var PdfExists = Path.Combine(PathPDF + NombreArchivo + ".pdf");
                            bool resultPrint = false;
                            bool resultReadXml = false;

                            int Copies = Constantes.ValorUno;
                            if (!System.IO.File.Exists(PdfExists))
                            {
                                bool resultCreatePDF = false;
                                Console.WriteLine("[" + DateTime.Now + "] Archivo PDF del Doc.: " + XmlDocType.ID_CE + " No Existe!. ");

                                NombreArchivo = XmlDocType.ID_CE;
                                tpodocumento = XmlDocType.Tipo_CE;
                                pathxmltemp = PathXML + XmlDocType.ID_CE;

                                Console.WriteLine("[" + DateTime.Now + "]  Leyendo Archivo: " + pathxmltemp + ".xml");

                                #region CASE TPO DOCUMENTO FOR PDF

                                switch (XmlDocType.Tipo_CE)
                                {

                                    case Constantes.Factura:
                                    case Constantes.Boleta:
                                        {
                                            resultReadXml = ObtenerValoresXMLCE(pathxmltemp);
                                            break;
                                        }
                                    case Constantes.NotaCredito:
                                        {
                                            resultReadXml = ObtenerValoresXMLCENotCredit(pathxmltemp);
                                            break;
                                        }
                                    case Constantes.NotaDebito:
                                        {
                                            resultReadXml = ObtenerValoresXMLCENotDebit(pathxmltemp);
                                            break;
                                        }

                                    case Constantes.Retencion:
                                        {
                                            resultReadXml = ObtenerValoresXMLCRE(pathxmltemp); // PARA EL PDF
                                            //Console.WriteLine("[" + DateTime.Now + "]  Fin de Lectura del Archivo: " + pathxmltemp + ".xml");
                                            //CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
                                            //resultPrint = CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
                                            break;
                                        }
                                    case Constantes.Percepcion:
                                        {
                                            break;
                                        }
                                }

                                #endregion

                                if (resultReadXml == true)
                                {
                                    //resultPrint = CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
                                    resultCreatePDF = CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat, copies);
                                    Console.WriteLine("[" + DateTime.Now + "]  Fin de Lectura del Archivo: " + pathxmltemp + ".xml = OK!");
                                }
                                else
                                {
                                    Console.WriteLine("[" + DateTime.Now + "] Error al Leer el Documento Nro: " + oFactura.NombreArchivoXML);
                                    logError.Add("[" + DateTime.Now + "] Error al Leer el Documento Nro: " + oFactura.NombreArchivoXML);
                                }

                                if (resultCreatePDF == true)
                                {
                                    //Si el archivo PDF se a Creado Correctamente manda a imprimir con foxit
                                    resultPrint = ImprimirDocument(PathPDF + XmlDocType.ID_CE + ".pdf", XmlDocType.PrintName, Copies);
                                    Console.WriteLine("[" + DateTime.Now + "] Se a Creado Correctamente el Pdf del Documento Nro: " + XmlDocType.ID_CE);
                                }
                                else
                                {
                                    Console.WriteLine("[" + DateTime.Now + "] Error al Crear el Pdf del Documento Nro: " + XmlDocType.ID_CE);
                                    logError.Add("[" + DateTime.Now + "] Error al Crear el Pdf del Documento Nro: " + XmlDocType.ID_CE);
                                }
                            }
                            else
                            {
                                Console.WriteLine("[" + DateTime.Now + "] Archivo PDF del Doc.: " + XmlDocType.ID_CE + " Existe!. ");
                                resultPrint = ImprimirDocument(PathPDF + XmlDocType.ID_CE + ".pdf", XmlDocType.PrintName, Copies);
                            }

                            // VALIDA SI SE A IMPRESO 
                            if (resultPrint == true)
                            {
                                Console.WriteLine("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " Se a Impreso Correctamente!, Nombre Impresora: " + XmlDocType.PrintName);
                                logPrint.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " Se a Impreso Correctamente!, Nombre Impresora: " + XmlDocType.PrintName);
                            }
                            else
                            {
                                Console.WriteLine("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " No se a Impreso!, Nomb Imp: " + XmlDocType.PrintName);
                                logNprint.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " No se a Impreso.");
                            }

                            #endregion END FORMATO PDF
                        }


                        #endregion
                    }
                    else
                    {
                        // SI NO EXISTE EL XML EN LA RUTA
                        Console.WriteLine("[" + DateTime.Now + "] No se encontro el XML del Documento Nro: " + ID_NUM_CE);
                        logNprint.Add("[" + DateTime.Now + "] No se encontro el XML del Documento Nro: " + ID_NUM_CE);
                    }

                    #endregion END METHOD WITH FLE IN DIRECTORY
                }
                Console.WriteLine("[" + DateTime.Now + "] Fin del Proceso del Documento: " + ID_NUM_CE);
            }
            catch (Exception ex)
            {
                logError.Add("[" + DateTime.Now + "] Error " + ex.Message + ", " + ex.InnerException + ",  en el Documento Nro: " + XmlDocType.ID_CE + ".");
            }
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
        //ListaDetalleFacturaElectronica objlistadetalle = new ListaDetalleFacturaElectronica();
        //DetalleFacturaElectronica objdetalle = new DetalleFacturaElectronica();
        ListaFacturaElectronica listamonto = new ListaFacturaElectronica();

        //DocCRECPE objDetalleCRECPE = new DocCRECPE();
        //ListaDocCRECPE objlistaDocCRECPE = new ListaDocCRECPE();

        Cliente oClient = new Cliente();
        Empresa oEmpresa = new Empresa();

        StreamReader sr;

        #region OBTENER CE FROM XML FACTURA, BOLETA

        //void ObtenerValoresXMLCE(string pathxml)
        private bool ObtenerValoresXMLCE(string pathxml)
        {
            bool result = false;
            try
            {
                #region READ XML

                XMLArchive.Load(pathxml + ".xml");

                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlFac.InvoiceType));
                sr = new StreamReader(pathxml + ".xml");
                xmlFac.InvoiceType inv = (xmlFac.InvoiceType)xmlSerial.Deserialize(sr);

                oFactura = new Slin.Facturacion.Common.UtilCE.UsefullClassWS().GetDataFromXMLCE(inv, XMLArchive, nombrearchivoFile);

                sr.Close();

                #endregion END READ XML

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message);
                result = false;
            }
            return result;
        }




        #endregion END CE

        #region NOTA DE CREDITO
        private bool ObtenerValoresXMLCENotCredit(string pathxml)
        {
            bool result = false;
            try
            {
                #region READ XML
                XMLArchive.Load(pathxml + ".xml");//ALL

                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlNotCred.CreditNoteType));
                sr = new StreamReader(pathxml + ".xml");
                xmlNotCred.CreditNoteType nc = (xmlNotCred.CreditNoteType)xmlSerial.Deserialize(sr);

                oFactura = new Slin.Facturacion.Common.UtilCE.UsefullClassWS().GetDataFromCNOTECredit(nc, XMLArchive, NombreArchivo, tpodocumento);

                sr.Close();

                #endregion
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message);
                sr.Close();
                result = false;
            }
            return result;
        }

        #endregion END NOTA DE CREDITO

        #region NOTA DE DEBITO
        private bool ObtenerValoresXMLCENotDebit(string pathxml)
        {
            bool result = false;
            try
            {
                #region READ XML
                XMLArchive.Load(pathxml + ".xml");//ALL

                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlNotDeb.DebitNoteType));
                sr = new StreamReader(pathxml + ".xml");
                xmlNotDeb.DebitNoteType nd = (xmlNotDeb.DebitNoteType)xmlSerial.Deserialize(sr);

                oFactura = new Slin.Facturacion.Common.UtilCE.UsefullClassWS().GetDataFromCNOTEDebit(nd, XMLArchive, NombreArchivo, tpodocumento);

                sr.Close();

                #endregion
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message);
                sr.Close();
                result = false;
            }

            return result;
        }
        #endregion END NOTA DE DEBITO

        #region OBTENER CRE FROM XML

        private bool ObtenerValoresXMLCRE(string pathxml)
        {
            bool result = false;
            try
            {
                #region READ XML
                XMLArchive.Load(pathxml + ".xml"); //XML

                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlCre.RetentionType));
                sr = new StreamReader(pathxml + ".xml");
                xmlCre.RetentionType ret = (xmlCre.RetentionType)xmlSerial.Deserialize(sr);
                oFactura = new Slin.Facturacion.Common.UtilCE.UsefullClassWS().GetDataFromXMLCRE(ret, XMLArchive, NombreArchivo, tpodocumento);

                sr.Close();
                #endregion

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message);
                sr.Close();// SR A TOMAR EN CUENTA
                result = false;
            }
            return result;
        }

        #endregion

        #endregion END SERIALIZAR XML

        #region CREAR PDF

        //private void CrearPdfSegunTpoDoc(FacturaElectronica oFact, string typeFormat)
        private bool CrearPdfSegunTpoDoc(FacturaElectronica oFact, string typeFormat, string copies)
        {
            CrearCodigoPDF417(oFact);
            LlenarListaMontos(oFact);
            bool result = EnviarParametrosRPT(oFact, typeFormat, copies);
            //CrearPDFandSave(PathPDF + NombreArchivo);
            return result;
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


                //BarcodeLib.Barcode Codigo = new BarcodeLib.Barcode();
                //Codigo.IncludeLabel = true;
                //System.Drawing.Bitmap bmp128 = new System.Drawing.Bitmap(Codigo.Encode(BarcodeLib.TYPE.CODE128, (oFact.ListaExtra[8].ExDato.Length > 0 ? oFact.ListaExtra[8].ExDato : "-"), Color.Black, Color.White, 400, 110));
                //bmp128.Save(@PathPDF417 + NombreArchivo + "_Barcode.bmp");

                #endregion

                
            }
            catch (Exception ex)
            {

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
        public string PathReportResumentTermic = string.Empty;
        private bool EnviarParametrosRPT(FacturaElectronica oFact, string typeFormat, string copies)
        {
            bool resultPrint = false;
            try
            {
                ReportGR = new ReportViewer();
                //ReportGR2 = new ReportViewer();

                #region SEGUN TYPE FORMAT

                //if (int.Parse(typeFormat) == Constantes.ValorUno)
                //{
                //    #region TYPE TICKET
                //    string pathReporteLocal = string.Empty;
                //    switch (oFact.TipoDocumento.CodigoDocumento)
                //    {
                //        case Constantes.Factura:
                //        case Constantes.Boleta:
                //            {
                //                if (oFact.TipoDocumento.CodigoDocumento == Constantes.Factura)
                //                {
                //                    //ReportGR.LocalReport.ReportPath = PathReporte + "Fact_T_" + oFact.Empresa.RUC + ".rdlc";
                //                    //ReportGR2.LocalReport.ReportEmbeddedResource = "Slin.Facturacion.PrintDoc.Reporte.Fact_T_" + oFact.Empresa.RUC + ".rdlc";
                //                    pathReporteLocal = PathReporte + "Fact_T_" + oFact.Empresa.RUC + ".rdlc";
                //                }
                //                else
                //                {
                //                    //ReportGR.LocalReport.ReportPath = PathReporte + "Bol_T_" + oFact.Empresa.RUC + ".rdlc";
                //                    //ReportGR2.LocalReport.ReportEmbeddedResource = "Slin.Facturacion.PrintDoc.Reporte.Bol_T_" + oFact.Empresa.RUC + ".rdlc";
                //                    pathReporteLocal = PathReporte + "Bol_T_" + oFact.Empresa.RUC + ".rdlc";
                //                    if (oFact.Cliente.ClienteRuc.Length == 0)
                //                    {
                //                        oFact.Cliente.ClienteRuc = oFact.Cliente.RazonSocial;
                //                    }
                //                }
                //                RPTParameterCE(oFact, pathReporteLocal, int.Parse(typeFormat));
                //                break;
                //            }
                //        case Constantes.NotaCredito:
                //            {

                //                //ReportGR.LocalReport.ReportPath = PathReporte + "NotC_T_" + oFact.Empresa.RUC + ".rdlc";
                //                //pathReporteLocal = PathReporte + "NotC_T_" + oFact.Empresa.RUC + ".rdlc";
                //                RPTParameterNOTCRED(oFact, PathReporte + "NotC_T_" + oFact.Empresa.RUC + ".rdlc", int.Parse(typeFormat));
                //                break;
                //            }
                //        case Constantes.NotaDebito:
                //            {
                //                //ReportGR.LocalReport.ReportPath = PathReporte + "NotD_T_" + oFact.Empresa.RUC + ".rdlc";
                //                RPTParameterNOTDEB(oFact, PathReporte + "NotD_T_" + oFact.Empresa.RUC + ".rdlc", int.Parse(typeFormat));
                //                break;
                //            }
                //        case Constantes.Retencion:
                //            {
                //                //ReportGR.LocalReport.ReportPath = PathReporte + "Ret_T_" + oFact.Empresa.RUC + ".rdlc";
                //                RPTParameterCRE(oFact, int.Parse(typeFormat));
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
                //    #region TYPE FORMAT PDF HIGH

                //    string pathReporteLocal = string.Empty;
                //    switch (oFact.TipoDocumento.CodigoDocumento)
                //    {
                //        case Constantes.Factura:
                //        case Constantes.Boleta:
                //            {
                //                if (oFact.TipoDocumento.CodigoDocumento == Constantes.Factura)
                //                {
                //                    ReportGR.LocalReport.ReportPath = PathReporte + "Fact_" + oFact.Empresa.RUC + ".rdlc";
                //                    //pathReporteLocal = PathReporte + "Fact_" + oFact.Empresa.RUC + ".rdlc";
                //                }
                //                //else if(oFact.TipoDocumento.CodigoDocumento == Constantes.Boleta)
                //                else
                //                {
                //                    ReportGR.LocalReport.ReportPath = PathReporte + "Bol_" + oFact.Empresa.RUC + ".rdlc";
                //                    //pathReporteLocal = PathReporte + "Bol_" + oFact.Empresa.RUC + ".rdlc";

                //                    if (oFact.Cliente.ClienteRuc.Length == 0)
                //                    {
                //                        oFact.Cliente.ClienteRuc = oFact.Cliente.RazonSocial;
                //                    }
                //                }
                //                RPTParameterCE(oFact, pathReporteLocal, int.Parse(typeFormat));
                //                break;
                //            }
                //        case Constantes.NotaCredito:
                //            {
                //                ReportGR.LocalReport.ReportPath = PathReporte + "NotC_" + oFact.Empresa.RUC + ".rdlc";
                //                //RPTParameterNOTCRED(oFact, PathReporte + "NotC_" + oFact.Empresa.RUC + ".rdlc", int.Parse(typeFormat));
                //                break;
                //            }
                //        case Constantes.NotaDebito:
                //            {
                //                ReportGR.LocalReport.ReportPath = PathReporte + "NotD_" + oFact.Empresa.RUC + ".rdlc";
                //                //RPTParameterNOTDEB(oFact, PathReporte + "NotD_" + oFact.Empresa.RUC + ".rdlc", int.Parse(typeFormat));
                //                break;
                //            }
                //        case Constantes.Retencion:
                //            {
                //                ReportGR.LocalReport.ReportPath = PathReporte + "Ret_" + oFact.Empresa.RUC + ".rdlc";
                //                RPTParameterCRE(oFact, int.Parse(typeFormat));
                //                break;
                //            }

                //        case Constantes.Percepcion:
                //            {
                //                ReportGR.LocalReport.ReportPath = PathReporte + "Per_" + oFact.Empresa.RUC + ".rdlc";
                //                RPTParameterCPE(oFact);
                //                break;
                //            }
                //    }

                //    #endregion END TYPE FORMAT PDF HIGH
                //}


                #endregion

                string pathReporteLocal = PathReporte;
                PathReportResumentTermic = PathReporte;

                pathReporteLocal += new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetPathReportviewer(oFact, int.Parse(typeFormat)); //obtiene el nombre del informe
                PathReportResumentTermic += new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetPathReportviewerResumen(oFact, int.Parse(typeFormat)); //obtiene el nombre del informe;

                ReportGR.LocalReport.ReportPath = pathReporteLocal;

                resultPrint = RPTParameterCE(oFact, pathReporteLocal, int.Parse(typeFormat), copies, PathReportResumentTermic);

                //resultPrint = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Nro: " + NombreArchivo + ": " + ex.Message);
                resultPrint = false;
            }

            return resultPrint;
        }

        #endregion END ENVIAR PARAMETROS REPORTVIEWER

        #region PASAR PARAMETROS RPT SEGUN TPO DOC

        //void RPTParameterCE(FacturaElectronica oFact, string pathReporte, int typeFormat)

        
        private bool RPTParameterCE(FacturaElectronica oFact, string pathReporte, int typeFormat, string copies, string PathReporteResumenTermic)
        {
            bool result = false;

            #region COPIES
            int CopiasforPrint = Constantes.ValorCero;

            if (copies.Length == Constantes.ValorCero)
            {
                CopiasforPrint = Constantes.ValorUno;
            }
            else if (int.Parse(copies) <= Constantes.ValorCero)
            {
                CopiasforPrint = Constantes.ValorUno;
            }
            else if(int.Parse(copies) > Constantes.ValorCero)
            {
                CopiasforPrint = int.Parse(copies);
            }
            else
            {
                CopiasforPrint = Constantes.ValorUno;
            }
            #endregion

            try
            {
                #region CASE TYPE DOCUMENT

                var ImporteTotal_Res = listamonto[5].MontoTotal;

                switch (oFact.TipoDocumento.CodigoDocumento)
                {
                    case Constantes.Factura:
                    case Constantes.Boleta:
                        {
                            ReportParameter[] Dpr = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersCE(oFact, listamonto);
                            //ReportParameter[] Dpr2 = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersCE_Resumen(oFact, listamonto, ImporteTotal_Res);
                            ReportParameter[] Dpr2 = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersCE(oFact, listamonto);
                            if (typeFormat == Constantes.ValorUno)
                            {
                                
                                for (int i = 1; i <= CopiasforPrint; i++)
                                {
                                    new Helper.Printed().PrintRDLC(pathReporte, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr, XmlDocType.PrintName);
                                }

                                new Helper.Printed().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, XmlDocType.PrintName);

                                //if (ValidatePrintResume == Constantes.ValorSI)
                                //{
                                //    new Helper.Printed().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, XmlDocType.PrintName);
                                //}
                            }
                            else if (typeFormat == Constantes.ValorDos)
                            {
                                #region
                                ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica));
                                ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                                ReportGR.LocalReport.SetParameters(Dpr);
                                ReportGR.LocalReport.Refresh();

                                //CrearPDFandSave(PathPDF + NombreArchivo);
                                var resultCreatePDF = CrearPDFandSave(PathPDF + NombreArchivo);
                                if (resultCreatePDF == true)
                                {
                                    //TICKET RESUMEN
                                    new Helper.Printed().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, XmlDocType.PrintName);

                                    //if (ValidatePrintResume == Constantes.ValorSI)
                                    //{
                                    //    new Helper.Printed().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, XmlDocType.PrintName);
                                    //}
                                }
                                #endregion
                            }
                            break;
                        }
                    case Constantes.NotaCredito:
                        {
                            ReportParameter[] Dpr = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersNotCred(oFact, listamonto);
                            //ReportParameter[] Dpr2 = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersNotCred_Resumen(oFact, listamonto, ImporteTotal_Res);
                            ReportParameter[] Dpr2 = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersNotCred(oFact, listamonto);

                            if (typeFormat == Constantes.ValorUno)
                            {
                                for (int i = 1; i <= CopiasforPrint; i++)
                                {
                                    new Helper.Printed().PrintRDLC(pathReporte, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, "DS_ListaDocAfectado", oFact.ListaAfectado, Dpr, XmlDocType.PrintName);
                                }

                                new Helper.Printed().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, XmlDocType.PrintName);
                                //new Helper.Printed().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, XmlDocType.PrintName);

                                //if (ValidatePrintResume == Constantes.ValorSI)
                                //{
                                //    new Helper.Printed().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, XmlDocType.PrintName);
                                //}
                            }
                            else if (typeFormat == Constantes.ValorDos)
                            {
                                #region
                                ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica));
                                ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                                ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDocAfectado", oFact.ListaAfectado));
                                ReportGR.LocalReport.SetParameters(Dpr);
                                ReportGR.LocalReport.Refresh();

                                //CrearPDFandSave(PathPDF + NombreArchivo);
                                var resultCreatePDF = CrearPDFandSave(PathPDF + NombreArchivo);
                                if (resultCreatePDF == true)
                                {
                                    //TICKET RESUMEN
                                    new Helper.Printed().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, XmlDocType.PrintName);

                                    //if (ValidatePrintResume == Constantes.ValorSI)
                                    //{
                                    //    new Helper.Printed().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, XmlDocType.PrintName);
                                    //}
                                }
                                #endregion
                            }
                            break;
                        }
                    case Constantes.NotaDebito:
                        {
                            ReportParameter[] Dpr = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersNotDebit(oFact, listamonto);
                            //ReportParameter[] Dpr2 = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersNotDebit_Resumen(oFact, listamonto, ImporteTotal_Res);
                            ReportParameter[] Dpr2 = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersNotDebit(oFact, listamonto);

                            if (typeFormat == Constantes.ValorUno)
                            {
                                for (int i = 1; i <= CopiasforPrint; i++)
                                {
                                    new Helper.Printed().PrintRDLC(pathReporte, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, "DS_ListaDocAfectado", oFact.ListaAfectado, Dpr, XmlDocType.PrintName);
                                }

                                new Helper.Printed().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, XmlDocType.PrintName);

                                //if (ValidatePrintResume == Constantes.ValorSI)
                                //{
                                //    //TICKET RESUMEN
                                //    new Helper.Printed().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, XmlDocType.PrintName);
                                //}
                            }
                            else if (typeFormat == Constantes.ValorDos)
                            {
                                #region
                                ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica));
                                ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                                ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDocAfectado", oFact.ListaAfectado));
                                ReportGR.LocalReport.SetParameters(Dpr);
                                ReportGR.LocalReport.Refresh();

                                //CrearPDFandSave(PathPDF + NombreArchivo);
                                var resultCreatePDF = CrearPDFandSave(PathPDF + NombreArchivo);
                                if (resultCreatePDF == true)
                                {
                                    new Helper.Printed().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, XmlDocType.PrintName);

                                    //if (ValidatePrintResume == Constantes.ValorSI)
                                    //{
                                    //    //TICKET RESUMEN
                                    //    new Helper.Printed().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, XmlDocType.PrintName);
                                    //}
                                }
                                #endregion
                            }
                            break;
                        }
                    case Constantes.Retencion:
                        {
                            ReportParameter[] Dpr = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersRetenc(oFact);
                            ReportParameter[] Dp2 = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersRetenc_Resumen(oFact);

                            if (typeFormat == Constantes.ValorUno)
                            {
                                for (int i = 1; i <= CopiasforPrint; i++)
                                {
                                    //new Helper.Printed().PrintRDLC(pathReporte, "DS_FacturaDet", objlistadetalle, "DS_ListaMontosCab", listamonto, "DS_ListaDocAfectado", oFact.ListaAfectado, Dpr, XmlDocType.PrintName);
                                }

                                if (ValidatePrintResume == Constantes.ValorSI)
                                {
                                    //TICKET RESUMEN
                                    //new Helper.Printed().PrintRDLC(pathReporte, "DS_FacturaDet", objlistadetalle, "DS_ListaMontosCab", listamonto, "DS_ListaDocAfectado", oFact.ListaAfectado, Dpr, XmlDocType.PrintName);
                                }
                            }
                            else if (typeFormat == Constantes.ValorDos)
                            {
                                #region
                                ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDetCRECPE", oFactura.ListaDocCRECPE));
                                ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                                ReportGR.LocalReport.SetParameters(Dpr);
                                ReportGR.LocalReport.Refresh();

                                //CrearPDFandSave(PathPDF + NombreArchivo);
                                var resultCreatePDF = CrearPDFandSave(PathPDF + NombreArchivo);
                                if (resultCreatePDF == true)
                                {
                                    if (ValidatePrintResume == Constantes.ValorSI)
                                    {

                                    }
                                    //new Helper.Printed().PrintRDLC(pathReporte, "DS_FacturaDet", objlistadetalle, "DS_ListaMontosCab", listamonto, "DS_ListaDocAfectado", oFact.ListaAfectado, Dpr, XmlDocType.PrintName);
                                }
                                #endregion
                            }
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

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                Console.WriteLine("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message);
            }
            return result;













            #region ANT
            //try
            //{
            //    ReportParameter[] Dpr = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersCE(oFact, listamonto);

            //    if (typeFormat == Constantes.ValorUno)
            //    {
                    
            //        //new Helper.Printed().PrintRDLC(pathReporte, "DS_FacturaDet", objlistadetalle, "DS_ListaMontosCab", listamonto, Dpr, XmlDocType.PrintName);
            //        new Helper.Printed().PrintRDLC(pathReporte, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr, XmlDocType.PrintName);
            //    }
            //    else if (typeFormat == Constantes.ValorDos)
            //    {
            //        //ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", objlistadetalle));
            //        ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica));
            //        ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
            //        ReportGR.LocalReport.SetParameters(Dpr);
            //        ReportGR.LocalReport.Refresh();

            //        CrearPDFandSave(PathPDF + NombreArchivo);

            //        //ReportGR2.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", objlistadetalle));
            //        //ReportGR2.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
            //        //ReportGR2.LocalReport.SetParameters(Dpr);
            //        //ReportGR2.LocalReport.Refresh();
            //        //ReportGR2.RefreshReport();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
            //    logError.Add("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message);
            //}

            #endregion
        }

        void RPTParameterNOTCRED(FacturaElectronica oFact, string pathReporte, int typeFormat)
        {
            try
            {
                ReportParameter[] Dpr = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersNotCred(oFact, listamonto);

                if (typeFormat == Constantes.ValorUno)
                {
                    new Helper.Printed().PrintRDLC(pathReporte, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, "DS_ListaDocAfectado", oFact.ListaAfectado, Dpr, XmlDocType.PrintName);
                    //new Helper.Printed().PrintRDLC(pathReporte, "DS_FacturaDet", objlistadetalle, "DS_ListaMontosCab", listamonto, "DS_ListaDocAfectado", oFact.ListaAfectado, Dpr, XmlDocType.PrintName);
                }
                else if (typeFormat == Constantes.ValorDos)
                {
                    //ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", objlistadetalle));
                    ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica));
                    ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                    ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDocAfectado", oFact.ListaAfectado));
                    ReportGR.LocalReport.SetParameters(Dpr);
                    ReportGR.LocalReport.Refresh();

                    CrearPDFandSave(PathPDF + NombreArchivo);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message);
            }
            
        }

        void RPTParameterNOTDEB(FacturaElectronica oFact, string pathReporte, int typeFormat)
        {
            try
            {
                ReportParameter[] Dpr = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersNotDebit(oFact, listamonto);
                if (typeFormat == Constantes.ValorUno)
                {
                    //new Helper.Printed().PrintRDLC(pathReporte, "DS_FacturaDet", objlistadetalle, "DS_ListaMontosCab", listamonto, "DS_ListaDocAfectado", oFact.ListaAfectado, Dpr, XmlDocType.PrintName);
                    new Helper.Printed().PrintRDLC(pathReporte, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, "DS_ListaDocAfectado", oFact.ListaAfectado, Dpr, XmlDocType.PrintName);
                }
                else if (typeFormat == Constantes.ValorDos)
                {
                    //ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", objlistadetalle));
                    ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica));
                    ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                    ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDocAfectado", oFact.ListaAfectado));
                    ReportGR.LocalReport.SetParameters(Dpr);
                    ReportGR.LocalReport.Refresh();
                    CrearPDFandSave(PathPDF + NombreArchivo);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Documento Nro: " + NombreArchivo + ": " + ex.Message);
            }
        }

        void RPTParameterCRE(FacturaElectronica oFact, int typeFormat)
        {
            try
            {
                ReportParameter[] Dpr = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetArrayParametersRetenc(oFact);

                if (typeFormat == Constantes.ValorUno)
                {
                    //new Helper.Printed().PrintRDLC(pathReporte, "DS_FacturaDet", objlistadetalle, "DS_ListaMontosCab", listamonto, "DS_ListaDocAfectado", oFact.ListaAfectado, Dpr, XmlDocType.PrintName);
                }
                else if (typeFormat == Constantes.ValorDos)
                {
                    //ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDetCRECPE", objlistaDocCRECPE));
                    ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDetCRECPE", oFactura.ListaDocCRECPE));
                    ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                    ReportGR.LocalReport.SetParameters(Dpr);
                    ReportGR.LocalReport.Refresh();
                    CrearPDFandSave(PathPDF + NombreArchivo);

                    //ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDetCRECPE", objlistaDocCRECPE));
                    //ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                    //ReportGR.LocalReport.SetParameters(Dpr);
                    //ReportGR.LocalReport.Refresh();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Nro: " + NombreArchivo + ": " + ex.Message);
            }
        }

        void RPTParameterCPE(FacturaElectronica oFact)
        {

        }


        void RPTParameterGUIAREM(FacturaElectronica oFact)
        {

        }

        #endregion

        #region CREATE PDF AND SAVE

        private bool CrearPDFandSave(string pathNombreArchivo)
        {
            bool result = false;
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
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] Error en el Nro: " + NombreArchivo + ": " + ex.Message + ", " + ex.InnerException);
                logError.Add("[" + DateTime.Now + "] Error en el Nro: " + NombreArchivo + ": " + ex.Message);
                result = false;
            }
            return result;
        }

        #endregion

        #endregion

        #region METHOD IMPRIME DOC

        private bool ImprimirDocument(string pathDoc, string printer, int copies)
        {
            return ImprimirFoxit(pathDoc, printer, copies);
            //return result;
        }

        private bool ImprimirFoxit(string file, string printer, int copies)
        {
            //copies = 1;
            bool rpta = false;
            try
            {
                for (int i = 0; i < copies; i++)
                {
                    //string sArgs = " /t \"" + file + "\" \"" + printer + "\"";
                    string sArgs = " /t \"" + file + "\" \"" + printer + "\" \"" + "{2}" + "\"";
                    System.Diagnostics.ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = @"C:\Program Files (x86)\Foxit Software\Foxit Reader\Foxit Reader.exe";
                    //startInfo.FileName = @"C:\Program Files (x86)\Foxit Software\PDF Creator\Registration.exe";
                    startInfo.Arguments = sArgs;
                    startInfo.CreateNoWindow = true;
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    System.Diagnostics.Process proc = Process.Start(startInfo);
                    proc.WaitForExit(10000); // Wait a maximum of 10 sec for the process to finish
                    if (!proc.HasExited)
                    {
                        proc.Kill();
                        proc.Dispose();
                        rpta = false;
                    }
                    else
                    {
                        rpta = true;
                    }
                }
            }
            catch (Exception ex)
            {
                rpta = false;
                Console.WriteLine("[" + DateTime.Now + "] Error en el Doc. Nro: " + NombreArchivo + ": " + ex.Message);
                logError.Add("[" + DateTime.Now + "] Error en el Doc. Nro: " + NombreArchivo + ": " + ex.Message);
            }
            return rpta;
        }








        //public bool ImprimirFoxit(string filename, string printer)
        //{
        //    bool rpta = false;

        //    try
        //    {
        //        if (printer == null)
        //        {
        //            string sArgs = " /t \"" + filename + "\" \"" + printer + "\"";
        //            System.Diagnostics.ProcessStartInfo startInfo = new ProcessStartInfo();
        //            startInfo.FileName = @"C:\Program Files (x86)\Foxit Software\Foxit Reader\Foxit Reader.exe";
        //            startInfo.Arguments = sArgs;
        //            startInfo.CreateNoWindow = true;
        //            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //            System.Diagnostics.Process proc = Process.Start(startInfo);
        //            proc.WaitForExit(10000); // Wait a maximum of 10 sec for the process to finish
        //            if (!proc.HasExited)
        //            {
        //                proc.Kill();
        //                proc.Dispose();
        //                rpta = false;
        //            }
        //        }
        //        else
        //        {
        //            ProcessStartInfo info = new ProcessStartInfo();
        //            info.Verb = printer;
        //            info.FileName = filename;
        //            info.CreateNoWindow = true;
        //            info.WindowStyle = ProcessWindowStyle.Hidden;

        //            Process p = new Process();
        //            p.StartInfo = info;
        //            p.Start();

        //            p.WaitForInputIdle();
        //            System.Threading.Thread.Sleep(3000);
        //            if (false == p.CloseMainWindow())
        //                p.Kill();
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        logError.Add("[" + DateTime.Now + "]  Error al Procesar el Documento Nro: " + NombreArchivo + ex.Message);
        //        rpta = false;
        //    }
        //    return rpta;
        //}

        #endregion




        #region XSLT TEST

        private void Generar()
        {
            XmlSerializer x = new XmlSerializer(oFactura.GetType());
            System.IO.StringWriter writer = new System.IO.StringWriter();
            x.Serialize(writer, oFactura.ListaDetalleFacturaElectronica);
            ConvierteFOP(writer);
            MessageBox.Show("PDF CREADO...");
        }
        private void ConvierteFOP(System.IO.StringWriter Xmlaux)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Xmlaux.ToString());
            string XSLFO = null;
            dynamic xslt = new System.Xml.Xsl.XslCompiledTransform();
            xslt.Load("PlantillaAgenda.xsl");// Aqui va el path de la plantilla
            using (System.IO.StringWriter tmp = new System.IO.StringWriter())
            {
                using (XmlTextWriter res = new XmlTextWriter(tmp))
                {
                    xslt.Transform(xmlDoc, null, res);
                    XSLFO = tmp.ToString();
                }
            }
            GeneraPDF(XSLFO);
        }

        private void GeneraPDF(string XSLFO)
        {
            //FopFactory fopFactory = FopFactory.newInstance();
            //OutputStream salida = new BufferedOutputStream(new FileOutputStream(new File("EjemploAgenda1.pdf")));
            //try
            //{
            //    Fop fop = fopFactory.newFop("application/pdf", salida);
            //    TransformerFactory factory = TransformerFactory.newInstance();
            //    Transformer transformer = factory.newTransformer(); // Identificador
            //    Source src = new StreamSource(new java.io.StringReader(XSLFO));
            //    Result res = new SAXResult(fop.getDefaultHandler());
            //    transformer.transform(src, res);
            //}
            //finally
            //{
            //    salida.close(); //Cerramos la salida.
            //}
        }

        #endregion




















        #region LAST METHOD VALIDATE WITH DATABASEE

        //#region METHOD PRINT WITH VALIDATE DATABASE

        //            oListDocumento = new ListaFacturaElectronica();
        //            oListDocumento = ServicioFacturacionSOA.Instance).GetListDocumentoPrint_Parameter(ID_NUM_CE);
        //            if (oListDocumento.Count == Constantes.ValorCero)
        //            {
        //                Console.WriteLine("[" + DateTime.Now + "] No se Obtuvo ningún Documento de la Base de Datos con ID: " + ID_NUM_CE);
        //            }
        //            else
        //            {
        //                Console.WriteLine("[" + DateTime.Now + "] Se Obtuvo un Documento de la BD con ID: " + ID_NUM_CE);
        //            }
        //            #region METHOD TWO

        //            entityIdRucDesencrypt = new Helper.Encrypt().DecryptKey(entityIdRucEncrypt);

        //            foreach (var xmldoc in oListDocumento)
        //            {
        //                Console.WriteLine("[" + DateTime.Now + "] Iniciando el Proceso del Documento: " + ID_NUM_CE);
        //                //oEmpresa = new ServicioSeguridadSOA().GetCredentialEntity(xmldoc.Empresa.RUC);

        //                if (xmldoc.Empresa.RUC == entityIdRucDesencrypt)
        //                {
        //                    #region IF ENTITY OK

        //                    NombreArchivo = xmldoc.NombreArchivoXML;
        //                    tpodocumento = xmldoc.TipoDocumento.CodigoDocumento;
        //                    //moneda = xmldoc.Moneda.Descripcion;

        //                    if (xmldoc.REF_FILES == null)
        //                    {
        //                        REF_FILES = string.Empty;
        //                    }
        //                    else
        //                    {
        //                        REF_FILES = xmldoc.REF_FILES;
        //                    }

        //                    if (xmldoc.Campo1.Length < Constantes.ValorUno)
        //                    {
        //                        montoLiteral = "-";
        //                    }
        //                    else
        //                    {
        //                        montoLiteral = xmldoc.Campo1;
        //                    }

        //                    pathxmltemp = PathXML + xmldoc.NombreArchivoXML;




        //                    //AQUI VALIDAR SI EXISTE EL ARCHIVO XML EN EL DIRECTORIO

        //                    Console.WriteLine("[" + DateTime.Now + "] Verificando si Existe el Archivo: " + pathxmltemp + ".xml");
        //                    var XmlExists = Path.Combine(pathxmltemp + ".xml");
        //                    if (System.IO.File.Exists(XmlExists))
        //                    {
        //                        Console.WriteLine("[" + DateTime.Now + "] Archivo XML del Doc.: " + pathxmltemp + ".xml" + ", Existe!.");


        //                        //VERIFICA SI EL ARCHIVO PDF EXISTE EN EL DIRECTORIO
        //                        Console.WriteLine("[" + DateTime.Now + "] Verificando si Existe el Archivo: " + PathPDF + NombreArchivo + ".pdf");
        //                        var PdfExists = Path.Combine(PathPDF + NombreArchivo + ".pdf");
        //                        if (!System.IO.File.Exists(PdfExists))
        //                        {
        //                            Console.WriteLine("[" + DateTime.Now + "] Archivo PDF del Doc.: " + xmldoc.NombreArchivoXML + " No Existe!. ");


        //                            Console.WriteLine("[" + DateTime.Now + "] Inicio de Creación del PDF del Doc.: " + xmldoc.NombreArchivoXML + ".pdf");
        //                            #region CASE TPO DOCUMENTO FOR PDF
        //                            Console.WriteLine("[" + DateTime.Now + "]  Leyendo Archivo: " + pathxmltemp + ".xml");

        //                            bool resultPrint = false;
        //                            switch (xmldoc.TipoDocumento.CodigoDocumento)
        //                            {

        //                                case Constantes.Factura:
        //                                case Constantes.Boleta:
        //                                    {
        //                                        ObtenerValoresXMLCE(pathxmltemp);
        //                                        Console.WriteLine("[" + DateTime.Now + "]  Fin de Lectura del Archivo: " + pathxmltemp + ".xml");
        //                                        //CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);

        //                                        resultPrint = CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
        //                                        break;
        //                                    }

        //                                case Constantes.NotaCredito:
        //                                    {
        //                                        ObtenerValoresXMLCENotCredit(pathxmltemp);
        //                                        Console.WriteLine("[" + DateTime.Now + "]  Fin de Lectura del Archivo: " + pathxmltemp + ".xml");
        //                                        //CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);

        //                                        resultPrint = CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
        //                                        break;
        //                                    }
        //                                case Constantes.NotaDebito:
        //                                    {
        //                                        ObtenerValoresXMLCENotDebit(pathxmltemp);
        //                                        Console.WriteLine("[" + DateTime.Now + "]  Fin de Lectura del Archivo: " + pathxmltemp + ".xml");
        //                                        //CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);

        //                                        resultPrint = CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
        //                                        break;
        //                                    }

        //                                case Constantes.Retencion:
        //                                    {
        //                                        ObtenerValoresXMLCRE(pathxmltemp); // PARA EL PDF
        //                                        Console.WriteLine("[" + DateTime.Now + "]  Fin de Lectura del Archivo: " + pathxmltemp + ".xml");
        //                                        //CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);

        //                                        resultPrint = CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
        //                                        break;
        //                                    }
        //                                case Constantes.Percepcion:
        //                                    {
        //                                        Console.WriteLine("[" + DateTime.Now + "]  Fin de Lectura del Archivo: " + pathxmltemp + ".xml");
        //                                        break;
        //                                    }
        //                            }

        //                            #endregion

        //                            if (resultPrint == true)
        //                            {
        //                                Console.WriteLine("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " Se a Impreso Correctamente!, Nombre Impresora: " + XmlDocType.PrintName);
        //                                logPrint.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " Se a Impreso Correctamente!, Nombre Impresora: " + XmlDocType.PrintName);
        //                            }
        //                            else
        //                            {
        //                                Console.WriteLine("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " No se a Impreso!, Nomb Imp: " + XmlDocType.PrintName);
        //                                logNprint.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " No se a Impreso.");
        //                            }


        //                            Console.WriteLine("[" + DateTime.Now + "] Fin de Creación del Archivo: " + xmldoc.NombreArchivoXML + ".pdf, Creado Correctamente!");
        //                        }
        //                        else
        //                        {
        //                            Console.WriteLine("[" + DateTime.Now + "] Archivo PDF: " + PathPDF + xmldoc.NombreArchivoXML + ".pdf, Existe!");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine("[" + DateTime.Now + "] Archivo XML del Doc.: " + xmldoc.NombreArchivoXML + " No Existe!. ");

        //                        Console.WriteLine("[" + DateTime.Now + "] Inicio de Cración del Archivo XML del Doc.: " + xmldoc.NombreArchivoXML);

        //                        xml = new XmlDocument();
        //                        xml.InnerXml = xmldoc.XML;
        //                        xml.Save(pathxmltemp + ".xml"); // HASTA AQUI YA ESTA CREADO EL ARCHIVO XML

        //                        Console.WriteLine("[" + DateTime.Now + "] Fin de Creación del Archivo XML del Doc.: " + xmldoc.NombreArchivoXML + " Creado Correctamente!");


        //                        var PdfExists = Path.Combine(PathPDF + NombreArchivo + ".pdf");
        //                        if (!System.IO.File.Exists(PdfExists))
        //                        {
        //                            #region CASE TPO DOCUMENTO FOR PDF
        //                            switch (xmldoc.TipoDocumento.CodigoDocumento)
        //                            {
        //                                case Constantes.Factura:
        //                                case Constantes.Boleta:
        //                                    {
        //                                        ObtenerValoresXMLCE(pathxmltemp);
        //                                        CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
        //                                        break;
        //                                    }

        //                                case Constantes.NotaCredito:
        //                                    {
        //                                        ObtenerValoresXMLCENotCredit(pathxmltemp);
        //                                        CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
        //                                        break;
        //                                    }
        //                                case Constantes.NotaDebito:
        //                                    {
        //                                        ObtenerValoresXMLCENotDebit(pathxmltemp);
        //                                        CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);
        //                                        break;
        //                                    }

        //                                case Constantes.Retencion:
        //                                    {
        //                                        ObtenerValoresXMLCRE(pathxmltemp); // PARA EL PDF
        //                                        CrearPdfSegunTpoDoc(oFactura, XmlDocType.TypeFormat);

        //                                        break;
        //                                    }
        //                                case Constantes.Percepcion:
        //                                    {
        //                                        break;
        //                                    }
        //                            }

        //                            #endregion
        //                        }
        //                    }

        //                    #region OBSOLETO

        //                    //if (oFactura.Empresa == null)
        //                    //{
        //                    //    Console.WriteLine("[" + DateTime.Now + "]  Leyendo Archivo XML: " + pathxmltemp + ".xml");
        //                    //    //EN CASO QUE YA EXISTE EL XML Y EL PDF, SE SERIALIZA EL XML PARA OBTENER INFORMACION NECESARIO PARA ENVIAR EL CORREO
        //                    //    #region SERIALIZA EL ARCHIVO XML PARA OBTENER DATOS DE LA EMPRESA Y EL CLIENTE SOLO PARA EL ENVIO DEL CORREO

        //                    //    switch (xmldoc.TipoDocumento.CodigoDocumento)
        //                    //    {
        //                    //        case Constantes.Boleta:
        //                    //        case Constantes.Factura:
        //                    //            {
        //                    //                ObtenerValoresXMLCE(pathxmltemp);
        //                    //                break;
        //                    //            }
        //                    //        case Constantes.NotaCredito:
        //                    //            {
        //                    //                ObtenerValoresXMLCENotCredit(pathxmltemp);
        //                    //                break;
        //                    //            }
        //                    //        case Constantes.NotaDebito:
        //                    //            {
        //                    //                ObtenerValoresXMLCENotDebit(pathxmltemp);
        //                    //                break;
        //                    //            }
        //                    //        case Constantes.Retencion:
        //                    //            {
        //                    //                ObtenerValoresXMLCRE(pathxmltemp);
        //                    //                break;
        //                    //            }
        //                    //        case Constantes.Percepcion:
        //                    //            {
        //                    //                break;
        //                    //            }
        //                    //    }
        //                    //    #endregion END SERIALIZA EL ARCHIVO XML PARA OBTENER DATOS DE LA EMPRESA Y EL CLIENTE SOLO PARA EL ENVIO DEL CORREO
        //                    //}

        //                    #endregion END OBSOLETO



        //                    #region PRINT DOCUMENT PDF
        //                    //int copies = 0;//REVISAR AQUI QUE SE TIENE QUE ENVIAR EL VALOR DEL DOCUMENTO

        //                    ////copies = int.Parse(XmlDocType.Copies);
        //                    //copies = 1;
        //                    //try
        //                    //{
        //                    //    if (XmlDocType.PrintName != null && XmlDocType.PrintName.Length > 0)
        //                    //    {
        //                    //        var res = ImprimirDocument(PathPDF + NombreArchivo + ".pdf", XmlDocType.PrintName, copies);

        //                    //        if (res == true)
        //                    //        {
        //                    //            Console.WriteLine("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " Se a Impreso Correctamente!, Nombre Impresora: " + XmlDocType.PrintName);
        //                    //            logPrint.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " Se a Impreso Correctamente!, Nombre Impresora: " + XmlDocType.PrintName);
        //                    //        }
        //                    //        else
        //                    //        {
        //                    //            Console.WriteLine("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " No se a Impreso!, Nomb Imp: " + XmlDocType.PrintName);
        //                    //            logNprint.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " No se a Impreso.");
        //                    //        }
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        logNprint.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " No se a Impreso.");
        //                    //    }
        //                    //}
        //                    //catch (Exception ex)
        //                    //{
        //                    //    logError.Add("[" + DateTime.Now + "] Error en el Doc. Nro: " + NombreArchivo + ": " + ex.Message);
        //                    //}


        //                    #endregion END PRINT DOCUMENT

        //                    #endregion
        //                }
        //                else
        //                {
        //                    Console.WriteLine("[" + DateTime.Now + "] El Ruc de Empresa del Documento Nro:  " + xmldoc.NombreArchivoXML + " no corresponde o no tiene Licencia para este Proceso.");
        //                    logError.Add("[" + DateTime.Now + "] El Ruc de Empresa del Documento Nro:  " + xmldoc.NombreArchivoXML + " no corresponde o no tiene Licencia para este Proceso.");
        //                }
        //                Console.WriteLine("[" + DateTime.Now + "] Fin del Proceso del Documento: " + ID_NUM_CE);
        //            }

        //            #endregion END METHOD TWO
        //            var listaResultado = oListDocumento;

        //            #endregion END METHOD WITH VALIDATE DATABASE

        #endregion

        #region PRINT DOCUMENT PDF
        //int copies = 0;//REVISAR AQUI QUE SE TIENE QUE ENVIAR EL VALOR DEL DOCUMENTO

        ////copies = int.Parse(XmlDocType.Copies);
        //copies = 1;
        //try
        //{
        //    if (XmlDocType.PrintName != null && XmlDocType.PrintName.Length > 0)
        //    {
        //        var res = ImprimirDocument(PathPDF + NombreArchivo + ".pdf", XmlDocType.PrintName, copies);

        //        if (res == true)
        //        {
        //            Console.WriteLine("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " Se a Impreso Correctamente!, Nombre Impresora: " + XmlDocType.PrintName);
        //            logPrint.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " Se a Impreso Correctamente!, Nombre Impresora: " + XmlDocType.PrintName);
        //        }
        //        else
        //        {
        //            Console.WriteLine("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " No se a Impreso!, Nomb Imp: " + XmlDocType.PrintName);
        //            logNprint.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " No se a Impreso.");
        //        }
        //    }
        //    else
        //    {
        //        logNprint.Add("[" + DateTime.Now + "] El Documento Nro: " + oFactura.NombreArchivoXML + " No se a Impreso.");
        //    }
        //}
        //catch (Exception ex)
        //{
        //    logError.Add("[" + DateTime.Now + "] Error en el Doc. Nro: " + NombreArchivo + ": " + ex.Message);
        //}


        #endregion END PRINT DOCUMENT
    }
}
