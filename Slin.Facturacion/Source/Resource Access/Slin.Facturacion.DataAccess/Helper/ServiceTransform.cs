using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using xmlFac = Slin.Facturacion.Common.CE;
using xmlCre = Slin.Facturacion.Common.CRE;
using System.IO;
using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.Common;
using Microsoft.Reporting.WinForms;
using System.Configuration;
using iTextSharp.text.pdf;
using System.Drawing;

using xmlNotCred = Slin.Facturacion.Common.CENC;
using xmlNotDeb = Slin.Facturacion.Common.CEND;
using xmlRead = Slin.Facturacion.Common.Helper;
using System.Drawing.Imaging;
using Slin.Facturacion.Common.Method;

namespace Slin.Facturacion.DataAccess.Helper
{
    public class ServiceTransform
    {

        #region ENTITY
        string PathCompany = string.Empty;

        public string NombreArchivo = string.Empty;
        public string moneda = string.Empty;
        public string tpodocumento = string.Empty;
        //public string TextoNexto = string.Empty;

        XmlDocument XMLArchive = new XmlDocument();

        FacturaElectronica oFactura = new FacturaElectronica();
        ListaDetalleFacturaElectronica objlistadetalle = new ListaDetalleFacturaElectronica();
        DetalleFacturaElectronica objdetalle = new DetalleFacturaElectronica();
        ListaFacturaElectronica listamonto = new ListaFacturaElectronica();
        DocCRECPE objDetalleCRECPE = new DocCRECPE();
        ListaDocCRECPE objlistaDocCRECPE = new ListaDocCRECPE();

        private ReportViewer reportGR;
        public ReportViewer ReportGR
        {
            get { return reportGR; }
            set { reportGR = value; }
        }
        #endregion

        #region METHOD
        private bool VerificarExistxPDF(string path, string num_ce)
        {
            var file = Path.Combine(path + num_ce + ".pdf");
            if (System.IO.File.Exists(file))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        string Path_TologConsult = string.Empty;
        public byte[] GetPDFDocFromXmlLine(string XMLine, string tpodoc, string num_cpe, string typeformat)
        {
            byte[] pdfBytes = null;
            try
            {
                string[] array = num_cpe.Split('-');

                PathCompany = ConfigurationManager.AppSettings[array[0]].ToString();

                var PathXml_Company = PathCompany + @"ProcesoCE\XML\";
                var PathPdf_Company = PathCompany + @"ProcesoCE\PDF\";

                Path_TologConsult = PathCompany + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\ser_consult\";
                CreateDirectory(Path_TologConsult);
                Path_TologConsult = PathCompany + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\ser_consult\ws_consult.log";

                NombreArchivo = num_cpe;
                tpodocumento = tpodoc;

                bool PdfCreateOk = false;

                #region CASE TPO DOCUMENTO FOR PDF

                switch (tpodoc)
                {
                    case Constantes.Factura:
                    case Constantes.Boleta:
                        {
                            ObtenerValoresXMLCE(XMLine);
                            var exists = VerificarExistxPDF(PathPdf_Company, num_cpe);
                            if (exists == false)
                            {
                                PdfCreateOk = CrearPdfSegunTpoDoc(oFactura, typeformat, num_cpe);
                            }
                            else
                            {
                                PdfCreateOk = true;
                            }
                            break;
                        }
                    case Constantes.NotaCredito:
                        {
                            ObtenerValoresXMLCENotCredit(XMLine);
                            var exists = VerificarExistxPDF(PathPdf_Company, num_cpe);
                            if (exists == false)
                            {
                                PdfCreateOk = CrearPdfSegunTpoDoc(oFactura, typeformat, num_cpe);
                            }
                            else { PdfCreateOk = true; }
                            break;
                        }
                    case Constantes.NotaDebito:
                        {
                            ObtenerValoresXMLCENotDebit(XMLine);
                            var exists = VerificarExistxPDF(PathPdf_Company, num_cpe);
                            if (exists == false)
                            {
                                PdfCreateOk = CrearPdfSegunTpoDoc(oFactura, typeformat, num_cpe);
                            }
                            else { PdfCreateOk = true; }
                            break;
                        }

                    case Constantes.Retencion:
                        {
                            ObtenerValoresXMLCRE(XMLine);
                            var exists = VerificarExistxPDF(PathPdf_Company, num_cpe);
                            if (exists == false)
                            {
                                PdfCreateOk = CrearPdfSegunTpoDoc(oFactura, typeformat, num_cpe);
                            }
                            else { PdfCreateOk = true; }
                            break;
                        }
                    case Constantes.Percepcion:
                        {
                            break;
                        }
                }

                #endregion

                if (PdfCreateOk == true)
                {
                    pdfBytes = File.ReadAllBytes(PathPdf_Company + NombreArchivo + ".pdf");
                    WriteLog_Service(Constantes.Msj_Pdf_Procesing);
                    WriteLog_Service(Constantes.Msj_Response_PdfBase64);
                }
                else
                {
                    WriteLog_Service(Constantes.Msj_PdfFail);
                }
            }
            catch (Exception ex)
            {
                WriteLog_Service(Constantes.Msj_Error + ex.Message);
                return pdfBytes = File.ReadAllBytes("<Document><Error>La Representación impresa no se puedo crear correctamente.</Error></Document>");
            }

            return pdfBytes;
        }

        private void CreateDirectory(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        #endregion

        #region METHOD SERIALIZE XML

        private bool CreateXmlPath(string xmline, string num_cpe, string pathxml)
        {
            try
            {
                var file = Path.Combine(pathxml + num_cpe + ".xml");
                if (System.IO.File.Exists(file))
                {
                    return true;
                }
                else
                {
                    var xmldoc = new XmlDocument();
                    xmldoc.InnerXml = xmline;
                    xmldoc.Save(pathxml + num_cpe + ".xml");
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        void Inicializa()
        {

            objlistadetalle = new ListaDetalleFacturaElectronica();
            objdetalle = new DetalleFacturaElectronica();
            oFactura = new FacturaElectronica();
            oFactura.Empresa = new Empresa();
            oFactura.Cliente = new Cliente();
            oFactura.TipoDocumento = new TipoDocumento();
            oFactura.Empresa.TipoDocumentiIdentidad = new TipoDocumentoIdentidad();
            oFactura.Cliente.TipoDocumentoIdentidad = new TipoDocumentoIdentidad();

            oFactura.DocCRECPE = new DocCRECPE();

            oFactura.Extra = new Extra();
            oFactura.ListaExtra = new ListaExtra();

            objDetalleCRECPE = new DocCRECPE();
            objlistaDocCRECPE = new ListaDocCRECPE();
        }

        #endregion

        #region OBTENER CE FROM XML
        void ObtenerValoresXMLCE(string xml_line)
        {
            oFactura = new FacturaElectronica();
            try
            {
                #region SERIALIZER

                XMLArchive.LoadXml(xml_line);

                //XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlFac.InvoiceType));
                //sr = new StreamReader(pathxml + ".xml");
                //xmlFac.InvoiceType inv = (xmlFac.InvoiceType)xmlSerial.Deserialize(sr);

                //var inv = new xmlFac.InvoiceType();
                //XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlFac.InvoiceType));
                //using (TextReader reader = new StringReader(xml_line))
                //{
                //    inv = (xmlFac.InvoiceType)xmlSerial.Deserialize(reader);
                //}

                var res = Singleton.Instance.GetDataXml_AllTypeDocument(xml_line, Constantes.Factura);
                var inv = (xmlFac.InvoiceType)(res);

                oFactura = new Slin.Facturacion.Common.UtilCE.UsefullClassWS().GetDataFromXMLCE(inv, XMLArchive, NombreArchivo);
                #endregion END SERIALIZER
            }
            catch (Exception ex)
            {
                WriteLog_Service(Constantes.Msj_Error + ex.Message);
            }
        }

        #endregion END CE

        #region NOTA DE CREDITO
        void ObtenerValoresXMLCENotCredit(string xml_line)
        {
            oFactura = new FacturaElectronica();
            try
            {
                XMLArchive.LoadXml(xml_line);//ALL

                //XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlNotCred.CreditNoteType));
                //sr = new StreamReader(pathxml + ".xml");
                //xmlNotCred.CreditNoteType nc = (xmlNotCred.CreditNoteType)xmlSerial.Deserialize(sr);


                //var nc = new xmlNotCred.CreditNoteType();
                //XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlNotCred.CreditNoteType));
                //using (TextReader reader = new StringReader(xml_line))
                //{
                //    nc = (xmlNotCred.CreditNoteType)xmlSerial.Deserialize(reader);
                //}

                var res = Singleton.Instance.GetDataXml_AllTypeDocument(xml_line, Constantes.NotaCredito);
                var nc = (xmlNotCred.CreditNoteType)(res);

                oFactura = new Slin.Facturacion.Common.UtilCE.UsefullClassWS().GetDataFromCNOTECredit(nc, XMLArchive, NombreArchivo, tpodocumento);
            }
            catch (Exception ex)
            {
                WriteLog_Service(Constantes.Msj_Error + ex.Message);
            }
        }
        #endregion END NOTA DE CREDITO

        #region NOTA DE DEBITO
        void ObtenerValoresXMLCENotDebit(string xml_line)
        {
            oFactura = new FacturaElectronica();
            try
            {
                XMLArchive.LoadXml(xml_line);//ALL
                //XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlNotDeb.DebitNoteType));
                //sr = new StreamReader(pathxml + ".xml");
                //xmlNotDeb.DebitNoteType nd = (xmlNotDeb.DebitNoteType)xmlSerial.Deserialize(sr);

                //var nd = new xmlNotDeb.DebitNoteType();
                //XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlNotDeb.DebitNoteType));
                //using (TextReader reader = new StringReader(xml_line))
                //{
                //    nd = (xmlNotDeb.DebitNoteType)xmlSerial.Deserialize(reader);
                //}

                var res = Singleton.Instance.GetDataXml_AllTypeDocument(xml_line, Constantes.NotaDebito);
                var nd = (xmlNotDeb.DebitNoteType)(res);

                oFactura = new Slin.Facturacion.Common.UtilCE.UsefullClassWS().GetDataFromCNOTEDebit(nd, XMLArchive, NombreArchivo, tpodocumento);
            }
            catch (Exception ex)
            {
                WriteLog_Service(Constantes.Msj_Error + ex.Message);
            }
        }
        #endregion END NOTA DE DEBITO

        #region OBTENER CRE FROM XML

        void ObtenerValoresXMLCRE(string xml_line)
        {
            XMLArchive = new XmlDocument();
            oFactura = new FacturaElectronica();
            try
            {
                XMLArchive.LoadXml(xml_line); //XML
                //XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlCre.RetentionType));
                //sr = new StreamReader(pathxml + ".xml");
                //xmlCre.RetentionType ret = (xmlCre.RetentionType)xmlSerial.Deserialize(sr);

                //var ret = new xmlCre.RetentionType();
                //XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlCre.RetentionType));
                //using (TextReader reader = new StringReader(xml_line))
                //{
                //    ret = (xmlCre.RetentionType)xmlSerial.Deserialize(reader);
                //}

                var res = Singleton.Instance.GetDataXml_AllTypeDocument(xml_line, Constantes.Retencion);
                var ret = (xmlCre.RetentionType)(res);

                oFactura = new Slin.Facturacion.Common.UtilCE.UsefullClassWS().GetDataFromXMLCRE(ret, XMLArchive, NombreArchivo, tpodocumento);
            }
            catch (Exception ex)
            {
                WriteLog_Service(Constantes.Msj_Error + ex.Message);
            }
        }

        #endregion

        #region CREAR PDF

        private bool CrearPdfSegunTpoDoc(FacturaElectronica oFact, string typeFormat, string num_ce)
        {
            CrearCodigoPDF417(oFact, num_ce);
            LlenarListaMontos(oFact, num_ce);
            EnviarParametrosRPT(oFact, typeFormat, num_ce);
            var result = CrearPDFandSave(num_ce);
            return result;
        }


        #region CODIGO PDF417

        void CrearCodigoPDF417(FacturaElectronica oFact, string num_ce)
        {
            try
            {              
                //var PathPdf417_Company = ReturnPathAccordingCompany(num_ce, PathPDF417);
                var PathPdf417_Company = PathCompany + @"ProcesoCE\PDF417\";
                string contenidoCodigoPDF417 = string.Empty;

                contenidoCodigoPDF417 = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetValueForCodePDF417(oFact);
                BarcodePDF417 opdf417 = new BarcodePDF417();
                opdf417.Options = BarcodePDF417.PDF417_USE_ASPECT_RATIO;
                opdf417.ErrorLevel = 8;
                opdf417.SetText(contenidoCodigoPDF417);
                System.Drawing.Bitmap imagen = new System.Drawing.Bitmap(opdf417.CreateDrawingImage(Color.Black, Color.White));
                imagen.Save(PathPdf417_Company + NombreArchivo + ".bmp");


                string valueCodeBar128 = string.Empty;
                #region val for codebar128
                try
                {
                    if (oFact.ListaExtra[8].ExDato != null && oFact.ListaExtra[8].ExDato.Length > Constantes.ValorCero)
                    {
                        valueCodeBar128 = oFact.ListaExtra[8].ExDato;
                    }
                }
                catch (Exception ex) { valueCodeBar128 = "000"; }

                #endregion

                if (valueCodeBar128.Length > 15)
                {
                    valueCodeBar128 = valueCodeBar128.Substring(0, 15);
                }

                BarcodeLib.Barcode Codigo = new BarcodeLib.Barcode();
                Codigo.IncludeLabel = true;
                System.Drawing.Bitmap bmp128 = new System.Drawing.Bitmap(Codigo.Encode(BarcodeLib.TYPE.CODE128, valueCodeBar128, Color.Black, Color.White, 400, 110));              
                bmp128.Save(PathPdf417_Company + NombreArchivo + "_Barcode.bmp");
            }
            catch (Exception ex)
            {
                WriteLog_Service(Constantes.Msj_Error + ex.Message);
            }
        }

        #endregion END CODIGO PDF417

        #region LLENAR LISTA MONTOS

        void LlenarListaMontos(FacturaElectronica oFact, string num_ce)
        {
            try
            {
                var PathPdf417_Company = PathCompany + @"ProcesoCE\PDF417\";

                string RutaPDF417 = Path.Combine(PathPdf417_Company + NombreArchivo + ".bmp");
                string RutaBarcode = Path.Combine(PathPdf417_Company + NombreArchivo + "_Barcode.bmp");

                listamonto = new ListaFacturaElectronica();
                listamonto = new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetListaMontos(oFact, RutaPDF417, RutaBarcode);
            }
            catch (Exception ex)
            {
                WriteLog_Service(Constantes.Msj_Error + ex.Message);
            }
        }

        #endregion END LLENAR LISTA MONTOS

        #region ENVIAR PARAMETRO REPORTVIEWER
        void EnviarParametrosRPT(FacturaElectronica oFact, string typeFormat, string num_ce)
        {
            try
            {
                ReportGR = new ReportViewer();
                var PathReporte_Company = PathCompany + @"Procesos\Report\";

                ReportGR.LocalReport.ReportPath = PathReporte_Company + new Slin.Facturacion.Common.UtilCE.ParametersClassWS().GetPathReportviewer(oFact, int.Parse(typeFormat));
                RPTParameterCE(oFact); // PARA TODOSLOS TIPOS DE DOCUMENTO
                
            }
            catch (Exception ex)
            {
                WriteLog_Service(Constantes.Msj_Error + ex.Message);
            }
        }

        private void WriteLog_Service(string msje)
        {
            using (StreamWriter sw = new StreamWriter(Path_TologConsult, true, Encoding.UTF8))
            {
                sw.WriteLine(msje);
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
            }
            catch (Exception ex)
            {
                WriteLog_Service(Constantes.Msj_Error + ex.Message);
            }
        }

        #endregion

        private bool CrearPDFandSave(string num_ce)
        {
            try
            {
                var PathPdf_Company = PathCompany + @"ProcesoCE\PDF\";

                var file = Path.Combine(PathPdf_Company + NombreArchivo + ".pdf");

                if (System.IO.File.Exists(file))
                {
                    //System.IO.File.Delete(file);
                    return true;
                }
                else
                {
                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = string.Empty;
                    ReportGR.ProcessingMode = ProcessingMode.Remote;
                    byte[] bytes = ReportGR.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                    using (System.IO.FileStream fs = new System.IO.FileStream(PathPdf_Company + NombreArchivo + ".pdf", System.IO.FileMode.Create))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                WriteLog_Service(Constantes.Msj_Error + ex.Message);
                return false;
            }
        }

        #endregion
    }
}
