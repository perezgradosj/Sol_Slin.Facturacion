using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Common;
using Slin.Facturacion.Proxies.ServicioFacturacion;

using iTextSharp.text.pdf;
using System.Drawing;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Net.Mail;

using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;

using System.Net;
//using Org.BouncyCastle.X509;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Drawing.Imaging;

namespace Slin.Facturacion.Electronica.Web.Views.Envio
{
    public partial class UtilPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                RecibirFactura();
                RenderReport(ReportViewer1, Response);
            }
        }

        #region ENTITY
        FacturaElectronica oFact = new FacturaElectronica();
        ListaDetalleFacturaElectronica oListaDetalle = new ListaDetalleFacturaElectronica();
        ListaDocCRECPE olistaCRECPE = new ListaDocCRECPE();
        ListaFacturaElectronica listamonto = new ListaFacturaElectronica();

        public string TextoNexto = string.Empty;


        private Documento odocumentoenvio;
        public Documento oDocumentoEnvio
        {
            get { return odocumentoenvio; }
            set { odocumentoenvio = value; }
        }

        private WCFSeguridad.Empresa oempresa;
        public WCFSeguridad.Empresa oEmpresa
        {
            get { return oempresa; }
            set { oempresa = value; }
        }

        #endregion

        #region METHOD





        void RecibirFactura()
        {
            try
            {
                oFact = (FacturaElectronica)Session["EnvioFacturaPDF"];

                if (oFact == null)
                {
                    //Response.Write("<script language=javascript>javascript:window.history.back();</script>");
                    Session["MsjErrorSerializerXML"] = "Error al Serializar XML";
                    Response.Redirect("EnvioDocumento");
                }

                switch (oFact.TipoDocumento.CodigoDocumento)
                {
                    case Constantes.Factura:
                    case Constantes.Boleta:
                        {
                            oListaDetalle = (ListaDetalleFacturaElectronica)Session["EnvioFacturaDetallePDF"];
                            break;
                        }
                    case Constantes.NotaCredito:
                        {
                            oListaDetalle = (ListaDetalleFacturaElectronica)Session["EnvioFacturaDetallePDF"];
                            break;
                        }
                    case Constantes.NotaDebito:
                        {
                            oListaDetalle = (ListaDetalleFacturaElectronica)Session["EnvioFacturaDetallePDF"];
                            break;
                        }
                    case Constantes.Retencion:
                        {
                            olistaCRECPE = (ListaDocCRECPE)Session["EnvioFacturaDetallePDF"];
                            break;
                        }

                    case Constantes.Percepcion:
                        {
                            break;
                        }
                }
                GenerarPDF(oFact.TypeFormat + string.Empty);
            }
            catch (Exception ex)
            {
                Session["MsjErrorSerializerXML"] = ex.Message + ", " + ex.InnerException;
                Response.Redirect("EnvioDocumento");
            }
        }

        void GenerarPDF(string typeFormat)
        {
            try
            {
                CrearCodigoPDF417();
                LlenarListaMontos();
                EnviarParametrosRPT(typeFormat);

                Session.Remove("EnvioFacturaPDF");
                Session.Remove("EnvioFacturaDetallePDF");
            }
            catch (Exception ex)
            {
                //Session["MsjeErrorCreatedPDF"] = Constantes.msjPdfFailCreated;
                //Response.Write("<script language=javascript>javascript:window.history.back();</script>");
                Session["MsjErrorSerializerXML"] = ex.Message + ", " + ex.InnerException;
                Response.Redirect("EnvioDocumento");
            }
        }

        void CrearCodigoPDF417()
        {
            try
            {
                string contenidoCodigoPDF417 = string.Empty;
                contenidoCodigoPDF417 = new Slin.Facturacion.Common.Util.ParametersClass().GetValueForCodePDF417(oFact);

                BarcodePDF417 opdf417 = new BarcodePDF417();
                opdf417.Options = BarcodePDF417.PDF417_USE_ASPECT_RATIO;
                opdf417.ErrorLevel = 8;
                opdf417.SetText(contenidoCodigoPDF417);
                System.Drawing.Bitmap imagen = new System.Drawing.Bitmap(opdf417.CreateDrawingImage(Color.Black, Color.White));
                imagen.Save(Server.MapPath("~/DocumentoXML/CodigoQR.bmp"));

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
                bmp128.Save(Server.MapPath("~/DocumentoXML/CodigoQR_Barcode.bmp"));
            }
            catch (Exception ex)
            {
                //Session["MsjeErrorCreatedPDF"] = Constantes.msjPdfFailCreated;

                Session["MsjErrorSerializerXML"] = ex.Message + ", " + ex.InnerException;
                Response.Redirect("EnvioDocumento");
                //Response.Write("<script language=javascript>javascript:window.history.back();</script>");
            }

        }

        void LlenarListaMontos()
        {
            try
            {
                string RutaPDF417 = Path.Combine(System.Windows.Forms.Application.StartupPath, Server.MapPath("~/DocumentoXML/CodigoQR.bmp"));
                string RutaBarcode = Path.Combine(System.Windows.Forms.Application.StartupPath, Server.MapPath("~/DocumentoXML/CodigoQR_Barcode.bmp"));
                listamonto = new ListaFacturaElectronica();

                listamonto = new Slin.Facturacion.Common.Util.ParametersClass().GetListaMontos(oFact, RutaPDF417, RutaBarcode);

                this.ReportViewer1.LocalReport.EnableExternalImages = true;
                var file = Path.Combine(HttpContext.Current.Server.MapPath("~/DocumentoXML/CodigoQR.bmp"));
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);

                var file128 = Path.Combine(HttpContext.Current.Server.MapPath("~/DocumentoXML/CodigoQR_Barcode.bmp"));
                if (System.IO.File.Exists(file128))
                    System.IO.File.Delete(file128);
            }
            catch (Exception ex)
            {
                //Session["MsjeErrorCreatedPDF"] = Constantes.msjPdfFailCreated;
                //Response.Write("<script language=javascript>javascript:window.history.back();</script>");
                Session["MsjErrorSerializerXML"] = ex.Message + ", " + ex.InnerException;
                Response.Redirect("EnvioDocumento");
            }
        }

        void EnviarParametrosRPT(string typeFormat)
        {
            try
            {
                ReportViewer1.LocalReport.ReportPath = new Slin.Facturacion.Common.Util.ParametersClass().GetPathReportviewer(oFact, int.Parse(typeFormat));
                RPTParameterCE();
            }
            catch (Exception ex)
            {
                //Session["MsjeErrorCreatedPDF"] = Constantes.msjPdfFailCreated;
                //Response.Write("<script language=javascript>javascript:window.history.back();</script>");
                Session["MsjErrorSerializerXML"] = ex.Message + ", " + ex.InnerException;
                Response.Redirect("EnvioDocumento");
            }

        }

        #region PASAR PARAMETROS RPT SEGUN TPO DOC

        void RPTParameterCE()
        {
            try
            {
                ReportParameter[] Dpr = new ReportParameter[0];

                switch (oFact.TipoDocumento.CodigoDocumento)
                {
                    case Constantes.Factura:
                    case Constantes.Boleta:
                        {
                            Dpr = new Slin.Facturacion.Common.Util.ParametersClass().GetArrayParametersCE(oFact, listamonto);

                            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oListaDetalle));
                            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));

                            break;
                        }
                    case Constantes.NotaCredito:
                        {
                            Dpr = new Slin.Facturacion.Common.Util.ParametersClass().GetArrayParametersNotCred(oFact, listamonto);

                            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oListaDetalle));
                            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDocAfectado", oFact.ListaAfectado));
                            break;
                        }
                    case Constantes.NotaDebito:
                        {
                            Dpr = new Slin.Facturacion.Common.Util.ParametersClass().GetArrayParametersNotDebit(oFact, listamonto);

                            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oListaDetalle));
                            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));
                            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDocAfectado", oFact.ListaAfectado));

                            break;
                        }
                    case Constantes.Retencion:
                        {
                            Dpr = new Slin.Facturacion.Common.Util.ParametersClass().GetArrayParametersRetenc(oFact);

                            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaDetCRECPE", olistaCRECPE));
                            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));

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
                
                ReportViewer1.LocalReport.SetParameters(Dpr);
                //ReportViewer1.LocalReport.EnableHyperlinks = true;
                //ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                //Session["MsjeErrorCreatedPDF"] = Constantes.msjPdfFailCreated;
                //Response.Write("<script language=javascript>javascript:window.history.back();</script>");
                Session["MsjErrorSerializerXML"] = ex.Message + ", " + ex.InnerException;
                Response.Redirect("EnvioDocumento");
            }
        }



        #endregion

        public void RenderReport(ReportViewer reportViewer, HttpResponse response)
        {
            string msjeresult = string.Empty;
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string enconding;
                string extension;
                byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out enconding, out extension, out streamids, out warnings);

                //reportViewer.LocalReport.Refresh();
                FileStream fs = new FileStream(Server.MapPath("../../DocumentoXML/" + oFact.NombreArchivoXML + ".pdf"), FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

                msjeresult = EnviarEmail();

                if (msjeresult.Contains("Error"))
                {
                    var filexml = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + oEmail.NombreArchivo + ".xml"));
                    if (System.IO.File.Exists(filexml))
                        System.IO.File.Delete(filexml);

                    var filepdf = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + oEmail.NombreArchivo + ".pdf"));
                    if (System.IO.File.Exists(filepdf))
                        System.IO.File.Delete(filepdf);



                    var listSendAdjFiles = (List<string>)Session["listSendAdjFiles"];

                    foreach (var file in listSendAdjFiles)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/DocumentoXML/" + file)))
                        {
                            System.IO.File.Delete(Server.MapPath("~/DocumentoXML/" + file));
                        }
                    }


                    Session["RespuestaEnvio"] = msjeresult;

                    //InsertarDocumentoEnviado();
                    Response.Redirect("EnvioDocumento");
                }
                else
                {
                    var filexml = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + oEmail.NombreArchivo + ".xml"));
                    if (System.IO.File.Exists(filexml))
                        System.IO.File.Delete(filexml);

                    var filepdf = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + oEmail.NombreArchivo + ".pdf"));
                    if (System.IO.File.Exists(filepdf))
                        System.IO.File.Delete(filepdf);


                    var listSendAdjFiles = (List<string>)Session["listSendAdjFiles"];

                    foreach (var file in listSendAdjFiles)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/DocumentoXML/" + file)))
                        {
                            System.IO.File.Delete(Server.MapPath("~/DocumentoXML/" + file));
                        }
                    }

                    string res = InsertarDocumentoEnviado();

                    if (res != Constantes.msjRegistrado)
                    {
                        msjeresult += ", " + res + "en la BD.";
                    }

                    Session["RespuestaEnvio"] = msjeresult;
                    Response.Redirect("EnvioDocumento");
                }
            }
            catch (Exception ex)
            {
                //Session["MsjeErrorCreatedPDF"] = Constantes.msjPdfFailCreated;
                //Response.Write("<script language=javascript>javascript:window.history.back();</script>");
                Session["MsjErrorSerializerXML"] = ex.Message + ", " + ex.InnerException;
                Response.Redirect("EnvioDocumento");
            }

        }

        private string InsertarDocumentoEnviado()
        {
            string result = string.Empty;
            try
            {
                oDocumentoEnvio = new Documento();
                oDocumentoEnvio = (Documento)Session["ObjDocumentoEnviado"];
                string msjeInsertado = ServiceFacturacionController.Instance.InsertarDocumentoEnviado(oDocumentoEnvio);
                Session.Remove("ObjDocumentoEnviado");


                if (msjeInsertado == Constantes.msjRegistrado)
                {
                    result = Constantes.msjRegistrado;
                }
                else
                {
                    result = Constantes.msjErrorAlInsertar;
                }
            }
            catch (Exception ex)
            {
                result = Constantes.msjErrorAlInsertar;
            }
            return result;
        }


        Documento oEmail = new Documento();

        WCFSeguridad.Usuario userlog = new WCFSeguridad.Usuario();
        void ObtenerSessionEmail()
        {
            try
            {
                oEmail = (Documento)Session["DatosEmail"];

                userlog = (WCFSeguridad.Usuario)Session["UsuarioLogueadoFact"];
            }
            catch (Exception ex)
            {
                //Session["MsjeErrorCreatedPDF"] = Constantes.msjPdfFailCreated;
                //Response.Write("<script language=javascript>javascript:window.history.back();</script>");
                Session["MsjErrorSerializerXML"] = ex.Message + ", " + ex.InnerException;
                Response.Redirect("EnvioDocumento");
            }
        }

        public string EnviarEmail()
        {
            string msjresult = string.Empty;
            try
            {
                ObtenerSessionEmail();

                oEmpresa = new WCFSeguridad.Empresa();
                oEmpresa = ServiceSeguridadController.Instance.GetCredentialEntity(oEmail.Empresa.RUC);

                oEmpresa.Password = new Helper.Encrypt().DecryptKey(oEmpresa.Password);
                //int porto = 587;
                msjresult = SMTPMail(oEmail, oEmpresa, oFact);
            }
            catch (Exception ex)
            {
                msjresult = "Error al Enviar - " + ex.Message;
            }
            return msjresult;
        }

        public string SMTPMail(Documento oDocEmail, WCFSeguridad.Empresa objEmpresa, FacturaElectronica oDoc)
        {
            string msjresult = string.Empty;
            try
            {
                ObtenerSessionEmail();
                // Crear el Mail
                using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
                {
                    //mail.To.Add(new System.Net.Mail.MailAddress(pDestino));
                    String[] AMailto = oDocEmail.Destino.Split(';');

                    foreach (String email in AMailto)
                    {
                        mail.To.Add(new MailAddress(email));
                    }

                    

                    //mail.From = new System.Net.Mail.MailAddress(objEmpresa.Email, oDocEmail.Referente.ToUpper(), System.Text.Encoding.UTF8);
                    mail.From = new System.Net.Mail.MailAddress(objEmpresa.Email, objEmpresa.Email, System.Text.Encoding.UTF8);

                    mail.Subject = oDocEmail.Asunto.ToUpper();
                    mail.SubjectEncoding = System.Text.Encoding.UTF8;

                    mail.Body += oDocEmail.Mensaje;
                    mail.Body += TemplateMail(oDoc, objEmpresa);


                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    mail.IsBodyHtml = true;


                    mail.Priority = MailPriority.High;


                    //MailAddress reply = new MailAddress(objEmpresa.Email, objEmpresa.Email, System.Text.Encoding.GetEncoding("ISO-8859-1"));
                    //mail.ReplyTo = reply;



                    // Agregar el Adjunto si deseamos hacerlo
                    mail.Attachments.Add(new Attachment(Server.MapPath("../../DocumentoXML/" + oEmail.NombreArchivo + ".xml")));
                    mail.Attachments.Add(new Attachment(Server.MapPath("../../DocumentoXML/" + oEmail.NombreArchivo + ".pdf")));


                    //ADJUNTO SEGUN DATABASE
                    if (oFact.Campo1.Length > 0)
                    {
                        string[] Files = oFact.Campo1.Split(';');
                        foreach (string file in Files)
                        {
                            var file_in = Path.Combine(file);
                            if (System.IO.File.Exists(file_in))
                            {
                                mail.Attachments.Add(new Attachment(file_in));
                            }
                        }
                    }

                    //ADJUNTOS AÑADIDOS FOR USER WITH VIEW SEND DOCUMENT
                    #region

                    var listSendAdjFiles = (List<string>)Session["listSendAdjFiles"];
                    foreach (var file in listSendAdjFiles)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/DocumentoXML/" + file)))
                        {
                            mail.Attachments.Add(new Attachment(Server.MapPath("~/DocumentoXML/" + file)));
                        }
                    }
                    #endregion

                    // Configuración SMTP
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();


                    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    { return true; };

                    if(objEmpresa.Dominio != Constantes.Guion && objEmpresa.Dominio.Length > Constantes.ValorUno)
                    {
                        smtp = new SmtpClient(objEmpresa.Dominio, objEmpresa.Port);
                    }
                    else { smtp = new SmtpClient(objEmpresa.IP, objEmpresa.Port); }
                    
                    smtp.EnableSsl = objEmpresa.UseSSL == Constantes.ValorUno ? true : false;

                    // Crear Credencial de Autenticacion
                    smtp.Credentials = new System.Net.NetworkCredential(objEmpresa.Email, objEmpresa.Password);
                    //smtp.EnableSsl = false;
                    //smtp.UseDefaultCredentials = false;

                    try
                    {
                        smtp.Send(mail);
                        msjresult = Constantes.msjEnviadoCorrectamente;
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                        msjresult = "Error al Enviar - " + ex.Message + ex.InnerException;
                    }
                } //end using mail
            }
            catch (Exception ex)
            {
                msjresult = "Error al Enviar - " + ex.Message;
            }

            return msjresult;
        }// end SMTPMail


        //ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
        //public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)        
        //{
        //    return true;     
        //}

        private string TemplateMail(FacturaElectronica oDoc, WCFSeguridad.Empresa objEmpresa)
        {
            string body = string.Empty;
            //body = Constantes.HtmlLineSend_Company;

            string path = Server.MapPath("~/Util/Html/" + objEmpresa.RUC + ".html");

            body = File.ReadAllText(path, System.Text.Encoding.GetEncoding("ISO-8859-1"));

            body = body.Replace("{URL_COMPANY_LOGO}", objEmpresa.Url_CompanyLogo);
            body = body.Replace("{URL_COMPANY_CONSULT}", objEmpresa.Url_CompanyConsult);
            body = body.Replace("URL_COMPANY_CONSULT", objEmpresa.Url_CompanyConsult);


            #region HTML TEXT

            body = body.Replace("{RazonSocialEmisor}", oDoc.Empresa.RazonSocial.ToUpper());

            body = body.Replace("{ClienteRazonSocial}", oDoc.Cliente.RazonSocial);
            body = body.Replace("{SerieCorrelativo}", oDoc.SerieCorrelativo);

            #region case type document
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
                        body = body.Replace("{TipoDocumento}", "GUIA DE REMISIÓN");
                        break;
                    }
            }
            #endregion

            body = body.Replace("{FechaEmision}", oDoc.FechaEmision.ToString("dd/MM/yyyy"));

            #endregion
            return body;
        }
        #endregion
    }
}