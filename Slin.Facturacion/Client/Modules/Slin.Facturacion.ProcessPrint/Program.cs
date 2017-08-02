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
using Microsoft.Reporting.WinForms;
using System.Diagnostics;
using Slin.Facturacion.Common.Method;

namespace Slin.Facturacion.ProcessPrint
{
    class Program
    {
        #region appsettings

        //static RefactoryClass Singleton.Instance = new RefactoryClass();

        //public string PathDocForPrint = ConfigurationManager.AppSettings["PathDocForPrint"].ToString(); //si va

        //static string PathLogSLINADE = ConfigurationManager.AppSettings["PathLogSLINADE"].ToString();
        //static string pathlog = PathLogSLINADE + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smp\";

        //static string PathCompany = ConfigurationManager.AppSettings["PathCompany"].ToString();
        //static string pathlog = PathCompany + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smp\Log_ProcessPrint.log";

        //static string pathlog = PathCompany + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smp\";

        //public string PathReporte = ConfigurationManager.AppSettings["PathReporte"].ToString();


        string NameService = ConfigurationManager.AppSettings["NameService"].ToString();//si va

        //public string PathPDF = ConfigurationManager.AppSettings["PathPDF"].ToString();
        //public string PathPDF417 = ConfigurationManager.AppSettings["PathPDF417"].ToString();
        //public string PathXML = ConfigurationManager.AppSettings["PathXML"].ToString();

        //public string PathReporte = PathCompany + @"Procesos\smp\Report\";

        //public string PathPDF = PathCompany + @"ProcesoCE\PDF\";
        //public string PathPDF417 = PathCompany + @"ProcesoCE\PDF417\";
        //public string PathXML = PathCompany + @"ProcesoCE\XML\";

        public string ValidateWithDataBase = ConfigurationManager.AppSettings["ValidateWithDataBase"].ToString();
        string ValidatePrintResume = ConfigurationManager.AppSettings["ValidatePrintResume"].ToString();

        public string Path_FoxitExe = ConfigurationManager.AppSettings["Path_FoxitExe"].ToString();


        static string pathlog = string.Empty;

        static string PathReporte = string.Empty;
        static string PathPDF = string.Empty;
        static string PathPDF417 = string.Empty;
        static string PathXML = string.Empty;

        static string PathFileOrderPrint = string.Empty;

        #endregion

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

                    PathFileOrderPrint = System.IO.Path.GetFullPath(System.IO.Path.Combine(pathFile_APP, "../../../../../")); //tiempo real path xml order file

                    Set_Path(PathCompany);
                }



                if (args.Length > 0)
                {
                    Singleton.Instance.WriteLog_Service_Comp(pathlog, " ");
                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjStart);
                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjStartProcessDoc + args[0].Replace("smp", " "));

                    Console.WriteLine(" ");
                    Console.WriteLine(Constantes.MsjStart);
                    Console.WriteLine(Constantes.MsjStartProcessDoc + args[0].Replace("smp", " "));

                    new Program().Execute_PrintDoc(args[0].Replace("smp", " "));


                    

                }

                //Console.WriteLine(" ");
                //Console.WriteLine(Constantes.MsjStart);
                //Console.WriteLine(Constantes.MsjStartProcessDoc + "20547025319-01-FF11-00000001*01*Foxit Reader PDF Printer*1*2");


                //Singleton.Instance.WriteLog_Service_Comp(pathlog, " ");
                //Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjStart);
                //Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjStartProcessDoc + ("20216414056-07-BC03-00000001*03*Foxit Reader PDF Printer*1*1").Replace("*"," "));

                //new Program().Execute_PrintDoc("20216414056-07-BC03-00000001*07*adepos*1*1");
                //Console.ReadLine();
                //Console.Read();
            }
            catch (Exception ex)
            {
                //Console.WriteLine(Constantes.Value_DateToLog + ex.Message);
                //Console.WriteLine(Constantes.MsjEndProcessDoc);
                //Console.WriteLine(Constantes.MsjEnd);

                //Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Value_DateToLog + ex.Message);
                //Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjEndProcessDoc);
                //Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjEnd);
            }
        }

        #region entity
        //string entityIdRucDesencrypt = string.Empty;
        string param_num_ce = string.Empty;
        string param_ruccompany = string.Empty;
        string param_type_document = string.Empty;
        string param_printer_name = string.Empty;
        string param_type_format = string.Empty;
        int param_copies = Constantes.ValorCero;

        string param_pathxml = string.Empty;
        string param_pathpdf = string.Empty;

        private ListaFacturaElectronica olistdocumento;
        public ListaFacturaElectronica oListDocumento
        {
            get { return olistdocumento; }
            set { olistdocumento = value; }
        }

        FacturaElectronica oFactura = new FacturaElectronica();
        ListaFacturaElectronica listamonto = new ListaFacturaElectronica();

        private static ReportViewer reportGR;
        public static ReportViewer ReportGR
        {
            get { return reportGR; }
            set { reportGR = value; }
        }
        #endregion



        #region charger path

        //static string PathCompany = ConfigurationManager.AppSettings["PathCompany"].ToString();
        //static string pathlog = PathCompany + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smp\Log_ProcessPrint.log";

        //public string PathReporte = PathCompany + @"Procesos\smp\Report\";

        //public string PathPDF = PathCompany + @"ProcesoCE\PDF\";
        //public string PathPDF417 = PathCompany + @"ProcesoCE\PDF417\";
        //public string PathXML = PathCompany + @"ProcesoCE\XML\";

        //string PathReporte = string.Empty;

        //string PathPDF = string.Empty;
        //string PathPDF417 = string.Empty;
        //string PathXML = string.Empty;

        //static string pathlog = string.Empty;

        //private void Set_Path(string path)
        //{
        //    PathReporte = path + @"Procesos\smp\Report\";

        //    PathPDF = path + @"ProcesoCE\PDF\";
        //    PathPDF417 = path + @"ProcesoCE\PDF417\";
        //    PathXML = path + @"ProcesoCE\XML\";

        //    pathlog = path + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smp\";
        //}


        #endregion



        #region method

        private static void Set_Path(string path)
        {
            PathReporte = path + @"Procesos\smp\Report\";

            PathPDF = path + @"ProcesoCE\PDF\";
            PathPDF417 = path + @"ProcesoCE\PDF417\";
            PathXML = path + @"ProcesoCE\XML\";

            pathlog = path + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smp\";
            Singleton.Instance.CreateDirectory(pathlog);
            pathlog = path + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smp\Log_ProcessPrint.log";




            //pathlog = path + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smc\Log_ProcessSend.log";

        }


        public void Execute_PrintDoc(string num_ce_codes)
        {
            Validate_CreateFileToPrint = Constantes.ValorCero;
            //Singleton.Instance.CreateDirectory(pathlog);

            //estructura del array 
            //[0] = num_ce
            //[1] = tipo_documento
            //[2] = printername
            //[3] = copies
            //[4] = tipo_formato

            param_pathxml = string.Empty;
            param_num_ce = string.Empty;
            param_ruccompany = string.Empty;
            param_type_document = string.Empty;
            param_printer_name = string.Empty;
            param_copies = Constantes.ValorCero;
            param_type_format = string.Empty;
            param_pathpdf = string.Empty;

            //num_ce_codes = num_ce_codes.Replace("smp", " ");

            string[] array = num_ce_codes.Split('*');
            int len = array.Length;

            param_num_ce = array[0];
            param_type_document = array[1];
            param_printer_name = array[2];
            param_copies = int.Parse(array[3]);

            param_ruccompany = param_num_ce.Substring(0, 11);
            param_type_format = array[4];

            param_pathxml = PathXML + param_num_ce + ".xml";//ruta donde se encuentra el xml firmado
            param_pathpdf = PathPDF + param_num_ce + ".pdf";

            #region

            //entityIdRucDesencrypt = new Common.Helper.Encrypt().DecryptKey(new BusinessSecurity.Entity.EntityClass().EntityId);

            var list = new BusinessSecurity.Entity.EntityClass().getsListEntity();

            if (list.Contains(param_ruccompany))
            {
                PrintDocument(param_pathxml);
            }
            else
            {
                Console.WriteLine(Constantes.MsjRucInvalid + param_num_ce);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjRucInvalid + param_num_ce);
            }
            #endregion

            var result = Singleton.Instance.Validate_Exists_File(PathFileOrderPrint);
            if (result == true) { System.IO.File.Delete(PathFileOrderPrint + param_num_ce + ".xml"); }

            //Console.WriteLine(Constantes.MsjFileDelete + PathDocForPrint + param_num_ce + ".xml");
            Console.WriteLine(Constantes.MsjFileDelete + param_num_ce + ".xml, file print order");
            Console.WriteLine(Constantes.MsjEndProcessDoc + param_num_ce);
            Console.WriteLine(Constantes.MsjEnd);

            //Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjFileDelete + PathDocForPrint + param_num_ce + ".xml");
            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjFileDelete + param_num_ce + ".xml, file print order");
            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjEndProcessDoc + param_num_ce);
            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjEnd);
        }

        private void PrintDocument(string _pathxml)
        {
            try

            {
                if (ValidateWithDataBase == Constantes.ValorSI)
                {
                    #region with validattion database
                    var result = new ServicioSeguridadSOA().GetStatus_WService(param_ruccompany, NameService);

                    if (result.Count > Constantes.ValorCero)
                    {
                        #region with validation database

                        oListDocumento = new ListaFacturaElectronica();
                        oListDocumento = new ServicioFacturacionSOA().GetListDocumentoPrint_Parameter(param_num_ce);

                        if (oListDocumento.Count == Constantes.ValorCero)
                        {
                            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjDocNoObtenido_BD + param_num_ce);
                            Console.WriteLine(Constantes.MsjDocNoObtenido_BD + param_num_ce);
                        }
                        else
                        {
                            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjDocObtenido_BD + param_num_ce);

                            Console.WriteLine(Constantes.MsjDocObtenido_BD + param_num_ce);

                            foreach (var xmldoc in oListDocumento)
                            {
                                #region recorremos la lista que siempre tendrá un documento por imprimir

                                if (!System.IO.File.Exists(_pathxml))
                                {
                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjXmlNoExiste_inPath + param_num_ce);
                                    Console.WriteLine(Constantes.MsjXmlNoExiste_inPath + param_num_ce);
                                }
                                else
                                {
                                    #region

                                    bool resultPrint = false;
                                    bool resultReadXml = false;
                                    bool resultcreadtePDF = false;
                                    //validamos por el formato del documento
                                    if (int.Parse(param_type_format) == Constantes.ValorUno)
                                    {
                                        Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjFormatPrint_Termic);

                                        Console.WriteLine(Constantes.MsjFormatPrint_Termic);

                                        #region
                                        resultReadXml = Read_File_Xml(_pathxml);
                                        if (resultReadXml == false)
                                        {
                                            //errores al leer el xml
                                            Console.WriteLine(Constantes.MsjErrorReadXml);
                                            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjErrorReadXml);
                                        }
                                        else
                                        {
                                            Console.WriteLine(Constantes.PrinterName + param_printer_name);
                                            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.PrinterName + param_printer_name);

                                            resultPrint = CrearPdfSegunTpoDoc(oFactura, param_type_format, param_copies);

                                            if (resultPrint == true)
                                            {
                                                if (Validate_CreateFileToPrint == Constantes.ValorCero)
                                                {
                                                    Console.WriteLine(Constantes.MsjPrinted_Ok);
                                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjPrinted_Ok);
                                                }
                                                else
                                                {
                                                    Console.WriteLine(Constantes.MsjNot_Printed);
                                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjNot_Printed);
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine(Constantes.MsjNot_Printed);
                                                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjNot_Printed);
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (int.Parse(param_type_format) == Constantes.ValorDos)
                                    {
                                        Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjFormatPrint_Pdf);

                                        Console.WriteLine(Constantes.MsjFormatPrint_Pdf);

                                        if (!System.IO.File.Exists(param_pathpdf))
                                        {
                                            #region 
                                            resultReadXml = Read_File_Xml(_pathxml);
                                            if (resultReadXml == false)
                                            {
                                                //errores al leer el xml
                                                Console.WriteLine(Constantes.MsjErrorReadXml);
                                                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjErrorReadXml);
                                            }
                                            else
                                            {

                                                resultcreadtePDF = CrearPdfSegunTpoDoc(oFactura, param_type_format, param_copies);

                                                if (resultcreadtePDF == true)
                                                {
                                                    Console.WriteLine(Constantes.PrinterName + param_printer_name);
                                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.PrinterName + param_printer_name);

                                                    resultPrint = ImprimirDocument(param_pathpdf, param_printer_name, param_copies);
                                                    if (resultPrint == true)
                                                    {
                                                        if (Validate_CreateFileToPrint == Constantes.ValorCero)
                                                        {
                                                            Console.WriteLine(Constantes.MsjPrinted_Ok);
                                                            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjPrinted_Ok);
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine(Constantes.MsjNot_Printed);
                                                            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjNot_Printed);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine(Constantes.MsjNot_Printed);
                                                        Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjNot_Printed);
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine(Constantes.Msj_PdfNotCreated);
                                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_PdfNotCreated);
                                                }
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region si el archivo pdf existe en la ruta

                                            Console.WriteLine(Constantes.Msj_FilePdfExists);
                                            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_FilePdfExists);

                                            Console.WriteLine(Constantes.PrinterName + param_printer_name);
                                            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.PrinterName + param_printer_name);

                                            resultPrint = ImprimirDocument(param_pathpdf, param_printer_name, param_copies);

                                            if (resultPrint == true)
                                            {
                                                if (Validate_CreateFileToPrint == Constantes.ValorCero)
                                                {
                                                    Console.WriteLine(Constantes.MsjPrinted_Ok);
                                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjPrinted_Ok);
                                                }
                                                else
                                                {
                                                    Console.WriteLine(Constantes.MsjNot_Printed);
                                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjNot_Printed);
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine(Constantes.MsjNot_Printed);
                                                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjNot_Printed);
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        Console.WriteLine(Constantes.MsjRucInvalid.Replace("RUCREPLACE", param_ruccompany));
                        Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjRucInvalid.Replace("RUCREPLACE", param_ruccompany));
                    }
                    #endregion
                    //with validation database
                }
                else
                {
                    #region with validation directory

                    if (System.IO.File.Exists(_pathxml))
                    {
                        #region
                        bool resultPrint = false;
                        bool resultReadXml = false;
                        bool resultCreatePDF = false;

                        if (int.Parse(param_type_format) == Constantes.ValorUno)
                        {
                            //termic
                            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjFormatPrint_Termic);
                            Console.WriteLine(Constantes.MsjFormatPrint_Termic);

                            #region
                            resultReadXml = Read_File_Xml(_pathxml);
                            if (resultReadXml == false)
                            {
                                //errores al leer el xml
                                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjErrorReadXml);
                                Console.WriteLine(Constantes.MsjErrorReadXml);
                            }
                            else
                            {
                                resultPrint = CrearPdfSegunTpoDoc(oFactura, param_type_format, param_copies);

                                if (resultPrint == true)
                                {
                                    if (Validate_CreateFileToPrint == Constantes.ValorCero)
                                    {
                                        Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.PrinterName + param_printer_name);
                                        Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjPrinted_Ok);

                                        Console.WriteLine(Constantes.PrinterName + param_printer_name);
                                        Console.WriteLine(Constantes.MsjPrinted_Ok);
                                    }
                                    else
                                    {
                                        Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjNot_Printed);
                                        Console.WriteLine(Constantes.MsjNot_Printed);
                                    }
                                }
                                else
                                {
                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjNot_Printed);
                                    Console.WriteLine(Constantes.MsjNot_Printed);
                                }
                            }
                            #endregion
                        }
                        else if (int.Parse(param_type_format) == Constantes.ValorDos)
                        {
                            //high pdf
                            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjFormatPrint_Pdf);
                            Console.WriteLine(Constantes.MsjFormatPrint_Pdf);

                            if (!System.IO.File.Exists(param_pathpdf))
                            {
                                #region

                                resultReadXml = Read_File_Xml(_pathxml);
                                if (resultReadXml == false)
                                {
                                    //errores al leer el xml
                                    Console.WriteLine(Constantes.MsjErrorReadXml);
                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjErrorReadXml);
                                }
                                else
                                {
                                    resultCreatePDF = CrearPdfSegunTpoDoc(oFactura, param_type_format, param_copies);

                                    if (resultCreatePDF == true)
                                    {
                                        Console.WriteLine(Constantes.PrinterName + param_printer_name);
                                        Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.PrinterName + param_printer_name);

                                        resultPrint = ImprimirDocument(param_pathpdf, param_printer_name, param_copies);

                                        if (resultPrint == true)
                                        {
                                            if (Validate_CreateFileToPrint == Constantes.ValorCero)
                                            {
                                                Console.WriteLine(Constantes.MsjPrinted_Ok);
                                                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjPrinted_Ok);
                                            }
                                            else
                                            {
                                                Console.WriteLine(Constantes.MsjNot_Printed);
                                                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjNot_Printed);
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine(Constantes.MsjNot_Printed);
                                            Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjNot_Printed);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(Constantes.Msj_PdfNotCreated);
                                        Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_PdfNotCreated);
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region si el archivo pdf existe en la ruta

                                Console.WriteLine(Constantes.Msj_FilePdfExists);
                                Console.WriteLine(Constantes.PrinterName + param_printer_name);
                                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_FilePdfExists);
                                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.PrinterName + param_printer_name);

                                resultPrint = ImprimirDocument(param_pathpdf, param_printer_name, param_copies);

                                if (resultPrint == true)
                                {
                                    if (Validate_CreateFileToPrint == Constantes.ValorCero)
                                    {
                                        Console.WriteLine(Constantes.MsjPrinted_Ok);
                                        Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjPrinted_Ok);
                                    }
                                    else
                                    {
                                        Console.WriteLine(Constantes.MsjNot_Printed);
                                        Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjNot_Printed);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(Constantes.MsjNot_Printed);
                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjNot_Printed);
                                }
                                #endregion
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        Console.WriteLine(Constantes.MsjXmlNoExiste_inPath + param_pathxml);
                        Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.MsjXmlNoExiste_inPath + param_pathxml);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
            }
        }

        #endregion

        #region serialize xml

        private bool Read_File_Xml(string pathxmlfile)
        {
            try
            {
                #region case type document
                switch (param_type_document)
                {
                    case Constantes.Factura:
                    case Constantes.Boleta:
                        { ObtenerValoresXMLCE(pathxmlfile); break; }
                    case Constantes.NotaCredito: { ObtenerValoresXMLCENotCredit(pathxmlfile); break; }
                    case Constantes.NotaDebito: { ObtenerValoresXMLCENotDebit(pathxmlfile); break; }
                    case Constantes.Retencion: { ObtenerValoresXMLCRE(pathxmlfile); break; }
                    case Constantes.Percepcion: { break; }
                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
                return false;
            }
        }

        #region OBTENER CE FROM XML FACTURA, BOLETA

        //void ObtenerValoresXMLCE(string pathxml)
        private bool ObtenerValoresXMLCE(string pathxmlfile)
        {
            bool result = false;
            try
            {
                oFactura = new FacturaElectronica();
                var XMLArchive = new XmlDocument();
                XMLArchive.Load(pathxmlfile);

                var res = Singleton.Instance.GetDataXml_AllTypeDocument(XMLArchive.InnerXml, Constantes.Factura);
                var inv = (xmlFac.InvoiceType)(res);

                oFactura = new Common.UtilCE.UsefullClassWS().GetDataFromXMLCE(inv, XMLArchive, param_num_ce);
                //oFactura = Singleton.Instance.Get_Document(string.Empty, Constantes.Factura, pathxmlfile, param_num_ce);

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
                result = false;
            }
            return result;
        }

        #endregion END CE

        #region NOTA DE CREDITO
        private bool ObtenerValoresXMLCENotCredit(string pathxmlfile)
        {
            bool result = false;
            try
            {
                var XMLArchive = new XmlDocument();
                XMLArchive.Load(pathxmlfile);

                var res = Singleton.Instance.GetDataXml_AllTypeDocument(XMLArchive.InnerXml, Constantes.NotaCredito);
                var nc = (xmlNotCred.CreditNoteType)(res);

                oFactura = new Slin.Facturacion.Common.UtilCE.UsefullClassWS().GetDataFromCNOTECredit(nc, XMLArchive, param_num_ce, param_type_document);
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
                result = false;
            }
            return result;
        }

        #endregion END NOTA DE CREDITO

        #region NOTA DE DEBITO
        private bool ObtenerValoresXMLCENotDebit(string pathxmlfile)
        {
            bool result = false;
            try
            {
                var XMLArchive = new XmlDocument();
                XMLArchive.Load(pathxmlfile);

                var res = Singleton.Instance.GetDataXml_AllTypeDocument(XMLArchive.InnerXml, Constantes.NotaDebito);
                var nd = (xmlNotDeb.DebitNoteType)(res);

                oFactura = new Common.UtilCE.UsefullClassWS().GetDataFromCNOTEDebit(nd, XMLArchive, param_num_ce, param_type_document);
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
                result = false;
            }
            return result;
        }
        #endregion END NOTA DE DEBITO

        #region OBTENER CRE FROM XML

        private bool ObtenerValoresXMLCRE(string pathxmlfile)
        {
            bool result = false;
            try
            {
                var XMLArchive = new XmlDocument();
                XMLArchive.Load(pathxmlfile);

                var res = Singleton.Instance.GetDataXml_AllTypeDocument(XMLArchive.InnerXml, Constantes.Retencion);
                var ret = (xmlCre.RetentionType)(res);

                oFactura = new Common.UtilCE.UsefullClassWS().GetDataFromXMLCRE(ret, XMLArchive, param_num_ce, param_type_document);
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
                result = false;
            }
            return result;
        }

        #endregion

        #endregion

        #region CREAR PDF

        //private void CrearPdfSegunTpoDoc(FacturaElectronica oFact, string typeFormat)
        private bool CrearPdfSegunTpoDoc(FacturaElectronica oFact, string typeFormat, int copies)
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
                new Common.UtilCE.ParametersClassWS().CrearCodigoPDF417(oFact, PathPDF417, param_num_ce);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
            }
        }

        #endregion END CODIGO PDF417

        #region LLENAR LISTA MONTOS

        void LlenarListaMontos(FacturaElectronica oFact)
        {
            try
            {
                string RutaPDF417 = Path.Combine(PathPDF417 + param_num_ce + ".bmp");
                string RutaBarcode = Path.Combine(PathPDF417 + param_num_ce + "_Barcode.bmp");

                listamonto = new ListaFacturaElectronica();
                listamonto = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetListaMontos(oFact, RutaPDF417, RutaBarcode);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
            }
        }

        #endregion END LLENAR LISTA MONTOS

        #region ENVIAR PARAMETRO REPORTVIEWER
        public string PathReportResumentTermic = string.Empty;
        private bool EnviarParametrosRPT(FacturaElectronica oFact, string typeFormat, int copies)
        {
            bool resultPrint = false;
            try
            {
                ReportGR = new ReportViewer();
                //ReportGR2 = new ReportViewer();

                string pathReporteLocal = PathReporte;
                PathReportResumentTermic = PathReporte;

                pathReporteLocal += new Common.UtilCE.ParametersClassWS().GetPathReportviewer(oFact, int.Parse(typeFormat)); //obtiene el nombre del informe
                PathReportResumentTermic += new Common.UtilCE.ParametersClassWS().GetPathReportviewerResumen(oFact, int.Parse(typeFormat)); //obtiene el nombre del informe;

                ReportGR.LocalReport.ReportPath = pathReporteLocal;
                resultPrint = RPTParameterCE(oFact, pathReporteLocal, int.Parse(typeFormat), param_copies, PathReportResumentTermic);
                //resultPrint = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
                resultPrint = false;
            }

            return resultPrint;
        }

        #endregion END ENVIAR PARAMETROS REPORTVIEWER

        #region PASAR PARAMETROS RPT SEGUN TPO DOC

        //void RPTParameterCE(FacturaElectronica oFact, string pathReporte, int typeFormat)

        int Validate_CreateFileToPrint = Constantes.ValorCero;
        private bool RPTParameterCE(FacturaElectronica oFact, string pathReporte, int typeFormat, int copies, string PathReporteResumenTermic)
        {
            bool result = false;
            try
            {
                #region CASE TYPE DOCUMENT
                var ImporteTotal_Res = listamonto[5].MontoTotal;

                switch (oFact.TipoDocumento.CodigoDocumento)
                {
                    case Constantes.Factura:
                    case Constantes.Boleta:
                        {
                            ReportParameter[] Dpr = new Common.UtilCE.ParametersClassWS().GetArrayParametersCE(oFact, listamonto);
                            //ReportParameter[] Dpr2 = new Common.UtilCE.ParametersClassWS().GetArrayParametersCE_Resumen(oFact, listamonto, ImporteTotal_Res);
                            ReportParameter[] Dpr2 = new Common.UtilCE.ParametersClassWS().GetArrayParametersCE(oFact, listamonto);

                            if (typeFormat == Constantes.ValorUno)
                            {
                                if (System.IO.File.Exists(pathReporte))
                                {
                                    for (int i = 1; i <= param_copies; i++)
                                    {
                                        new Helper.PrintClass().PrintRDLC(pathReporte, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr, param_printer_name);
                                    }
                                }
                                else
                                {
                                    Validate_CreateFileToPrint++;
                                    Console.WriteLine(Constantes.Msj_FileNotExists + pathReporte);
                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_FileNotExists + pathReporte);
                                }

                                if (System.IO.File.Exists(PathReporteResumenTermic))
                                {
                                    new Helper.PrintClass().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, param_printer_name);
                                }
                                else
                                {
                                    Validate_CreateFileToPrint++;
                                    Console.WriteLine(Constantes.Msj_FileNotExists + PathReporteResumenTermic);
                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_FileNotExists + PathReporteResumenTermic);
                                }
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
                                var resultCreatePDF = CrearPDFandSave(PathPDF + param_num_ce);
                                if (resultCreatePDF == true)
                                {
                                    //TICKET RESUMEN
                                    //new Helper.PrintClass().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, param_printer_name);

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
                            ReportParameter[] Dpr = new Common.UtilCE.ParametersClassWS().GetArrayParametersNotCred(oFact, listamonto);
                            //ReportParameter[] Dpr2 = new Common.UtilCE.ParametersClassWS().GetArrayParametersNotCred_Resumen(oFact, listamonto, ImporteTotal_Res);
                            ReportParameter[] Dpr2 = new Common.UtilCE.ParametersClassWS().GetArrayParametersNotCred(oFact, listamonto);

                            if (typeFormat == Constantes.ValorUno)
                            {
                                if (System.IO.File.Exists(pathReporte))
                                {
                                    for (int i = 1; i <= param_copies; i++)
                                    {
                                        new Helper.PrintClass().PrintRDLC(pathReporte, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, "DS_ListaDocAfectado", oFact.ListaAfectado, Dpr, param_printer_name);
                                    }
                                }
                                else
                                {
                                    Validate_CreateFileToPrint++;
                                    Console.WriteLine(Constantes.Msj_FileNotExists + pathReporte);
                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_FileNotExists + pathReporte);
                                }

                                if (System.IO.File.Exists(PathReporteResumenTermic))
                                {
                                    new Helper.PrintClass().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, param_printer_name);
                                }
                                else
                                {
                                    Validate_CreateFileToPrint++;
                                    Console.WriteLine(Constantes.Msj_FileNotExists + PathReporteResumenTermic);
                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_FileNotExists + PathReporteResumenTermic);
                                }
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
                                var resultCreatePDF = CrearPDFandSave(PathPDF + param_num_ce);
                                if (resultCreatePDF == true)
                                {
                                    //TICKET RESUMEN
                                    //new Helper.PrintClass().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, param_printer_name);
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
                            ReportParameter[] Dpr = new Common.UtilCE.ParametersClassWS().GetArrayParametersNotDebit(oFact, listamonto);
                            //ReportParameter[] Dpr2 = new Common.UtilCE.ParametersClassWS().GetArrayParametersNotDebit_Resumen(oFact, listamonto, ImporteTotal_Res);
                            ReportParameter[] Dpr2 = new Common.UtilCE.ParametersClassWS().GetArrayParametersNotDebit(oFact, listamonto);

                            if (typeFormat == Constantes.ValorUno)
                            {
                                if (System.IO.File.Exists(pathReporte))
                                {
                                    for (int i = 1; i <= param_copies; i++)
                                    {
                                        new Helper.PrintClass().PrintRDLC(pathReporte, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, "DS_ListaDocAfectado", oFact.ListaAfectado, Dpr, param_printer_name);
                                    }
                                }
                                else
                                {
                                    Validate_CreateFileToPrint++;
                                    Console.WriteLine(Constantes.Msj_FileNotExists + pathReporte);
                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_FileNotExists + pathReporte);
                                }

                                if (System.IO.File.Exists(PathReporteResumenTermic))
                                {
                                    new Helper.PrintClass().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, param_printer_name);
                                }
                                else
                                {
                                    Validate_CreateFileToPrint++;
                                    Console.WriteLine(Constantes.Msj_FileNotExists + PathReporteResumenTermic);
                                    Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_FileNotExists + PathReporteResumenTermic);
                                }
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
                                var resultCreatePDF = CrearPDFandSave(PathPDF + param_num_ce);
                                if (resultCreatePDF == true)
                                {
                                    //new Helper.PrintClass().PrintRDLC_Resumen(PathReporteResumenTermic, "DS_FacturaDet", oFactura.ListaDetalleFacturaElectronica, "DS_ListaMontosCab", listamonto, Dpr2, param_printer_name);
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
                            ReportParameter[] Dpr = new Common.UtilCE.ParametersClassWS().GetArrayParametersRetenc(oFact);
                            ReportParameter[] Dp2 = new Common.UtilCE.ParametersClassWS().GetArrayParametersRetenc_Resumen(oFact);

                            if (typeFormat == Constantes.ValorUno)
                            {
                                for (int i = 1; i <= param_copies; i++)
                                {
                                    //new Helper.PrintClass().PrintRDLC(pathReporte, "DS_FacturaDet", objlistadetalle, "DS_ListaMontosCab", listamonto, "DS_ListaDocAfectado", oFact.ListaAfectado, Dpr, param_printer_name);
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
                                var resultCreatePDF = CrearPDFandSave(PathPDF + param_num_ce);
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
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
                result = false;
            }
            return result;
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
                Validate_CreateFileToPrint++;
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
                result = false;
            }
            return result;
        }

        #endregion

        #endregion

        #region print high document 

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
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.FileName = Path_FoxitExe;
                    //startInfo.FileName = @"D:\SLIN-ADE\DEPOSEG\Procesos\smp\bin\Foxit Reader.exe";
                    //startInfo.FileName = @"C:\Program Files (x86)\Foxit Software\Foxit Reader\Foxit Reader.exe";
                    //startInfo.FileName = @"C:\Program Files (x86)\Foxit Software\PDF Creator\Registration.exe";
                    startInfo.Arguments = sArgs;
                    startInfo.CreateNoWindow = true;
                    startInfo.UseShellExecute = false;
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    System.Diagnostics.Process proc = System.Diagnostics.Process.Start(startInfo);
                    proc.WaitForExit(10000); // Wait a maximum of 10 sec for the process to finish
                    if (!proc.HasExited)
                    {
                        proc.Kill();
                        proc.Dispose();
                        rpta = false;
                    }
                    else { rpta = true; }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constantes.Msj_Error + ex.Message);
                Singleton.Instance.WriteLog_Service_Comp(pathlog, Constantes.Msj_Error + ex.Message);
                rpta = false;
            }
            return rpta;
        }
        #endregion
    }
}