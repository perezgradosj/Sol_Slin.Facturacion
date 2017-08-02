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
using System.Drawing.Imaging;

namespace Slin.Facturacion.Electronica.Web.Views.Informes
{
    public partial class DocumentoPdf : System.Web.UI.Page
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
        #endregion

        #region METHOD
        void RecibirFactura()
        {
            try
            {
                oFact = (FacturaElectronica)Session["FacturaPDF"];

                if (oFact == null)
                {
                    Response.Write("<script language=javascript>javascript:window.history.back();</script>");
                }

                if (oFact.TipoDocumento == null)
                {
                    Response.Write("<script language=javascript>javascript:window.history.back();</script>");
                }

                switch (oFact.TipoDocumento.CodigoDocumento)
                {
                    case Constantes.Factura:
                    case Constantes.Boleta:
                        {
                            oListaDetalle = (ListaDetalleFacturaElectronica)Session["FacturaDetallePDF"];
                            break;
                        }
                    case Constantes.NotaCredito:
                        {
                            oListaDetalle = (ListaDetalleFacturaElectronica)Session["FacturaDetallePDF"];
                            break;
                        }
                    case Constantes.NotaDebito:
                        {
                            oListaDetalle = (ListaDetalleFacturaElectronica)Session["FacturaDetallePDF"];
                            break;
                        }
                    case Constantes.Retencion:
                        {
                            olistaCRECPE = (ListaDocCRECPE)Session["FacturaDetallePDF"];
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
                Session["MsjeErrorCreatedPDF"] = Constantes.msjPdfFailCreated;
                Response.Write("<script language=javascript>javascript:window.history.back();</script>");
            }
        }

        void GenerarPDF(string typeFormatPDF)
        {
            try
            {
                CrearCodigoPDF417();
                LlenarListaMontos();
                EnviarParametrosRPT(typeFormatPDF);

                Session.Remove("FacturaPDF");
                Session.Remove("FacturaDetallePDF");
            }
            catch (Exception ex)
            {
                Session["MsjeErrorCreatedPDF"] = Constantes.msjPdfFailCreated;
                Response.Write("<script language=javascript>javascript:window.history.back();</script>");
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

                if (valueCodeBar128.Length <= 0)
                {
                    valueCodeBar128 = "000";
                }

                BarcodeLib.Barcode Codigo = new BarcodeLib.Barcode();
                Codigo.IncludeLabel = true;
                System.Drawing.Bitmap bmp128 = new System.Drawing.Bitmap(Codigo.Encode(BarcodeLib.TYPE.CODE128, valueCodeBar128, Color.Black, Color.White, 400, 110));
                bmp128.Save(Server.MapPath("~/DocumentoXML/CodigoQR_Barcode.bmp"));
            }
            catch (Exception ex)
            {
                Session["MsjeErrorCreatedPDF"] = Constantes.msjPdfFailCreated;
                Response.Write("<script language=javascript>javascript:window.history.back();</script>");
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
                Session["MsjeErrorCreatedPDF"] = Constantes.msjPdfFailCreated;
                Response.Write("<script language=javascript>javascript:window.history.back();</script>");
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
                Session["MsjeErrorCreatedPDF"] = Constantes.msjPdfFailCreated;
                Response.Write("<script language=javascript>javascript:window.history.back();</script>");
            }
        }
        #region PASAR PARAMETROS RPT SEGUN TPO DOC

        void RPTParameterCE()
        {
            Session.Remove("ListErrors");

            ReportParameter[] Dpr = new ReportParameter[0];

            int rest = Constantes.ValorCero;
            try
            {
                switch (oFact.TipoDocumento.CodigoDocumento)
                {
                    case Constantes.Factura:
                    case Constantes.Boleta:
                        {
                            Dpr = new Common.Util.ParametersClass().GetArrayParametersCE(oFact, listamonto);

                            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_FacturaDet", oListaDetalle));
                            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_ListaMontosCab", listamonto));

                            break;
                        }
                    case Constantes.NotaCredito:
                        {
                            Dpr = new Common.Util.ParametersClass().GetArrayParametersNotCred(oFact, listamonto);

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
                //ReportViewer1.LocalReport.EnableHyperlinks = true;
                ReportViewer1.LocalReport.SetParameters(Dpr);
                ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                //List<string> listError = new List<string>();

                //if (Session["ListErrors"] != null)
                //{
                //    listError = (List<string>)Session["ListErrors"];
                //}
                //else
                //{
                //    listError = new List<string>();
                //}

                //string[] msje = ex.InnerException.Message.Split(' ');

                //string ms = "";

                //ms = msje[8].Replace("'", "");
                //ms = ms.Replace("'", "");
                //ms = ms.Replace(",", "");

                //if (!listError.Contains(ms))
                //{
                //    listError.Add(ms);
                //}

                //Session["ListErrors"] = listError;

                //ReportParameter[] Dpr2 = new ReportParameter[Dpr.ToList().Count - listError.Count];
                //int constante = Constantes.ValorCero;

                //for (int i = 0; i<= Dpr.ToList().Count - (1); i++)
                //{
                //    if (ms != Dpr[i].Name)
                //    {
                //        Dpr2[constante] = Dpr[i];
                //        constante++;
                //    }
                //}
                //ReportViewer1.LocalReport.SetParameters(Dpr2);
                //ReportViewer1.LocalReport.Refresh();

                //for (int i = 0; i<= Dpr.ToList().Count - 1; i++)
                //{
                //}
                //SetParameters(Dpr2, listError);
                //var obj = Dpr.ToList().Find(x => x.Name == ms);
                Session["MsjeErrorCreatedPDF"] = Constantes.msjPdfFailCreated;
                Response.Write("<script language=javascript>javascript:window.history.back();</script>");
            }
        }

        private void SetParameters(ReportParameter [] Dpr, List<string> list)
        {
            ReportParameter[] Dpr2 = new ReportParameter[Dpr.ToList().Count - list.Count];
            int constante = Constantes.ValorCero;

            for (int i = 0; i <= Dpr.ToList().Count - (1); i++)
            {
                if (!list.Contains(Dpr[i].Name))
                {
                    Dpr2[constante] = Dpr[i];
                    constante++;
                }
            }
        }

        #endregion



        public void RenderReport(ReportViewer reportViewer, HttpResponse response)
        {
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string enconding;
                string extension;
                byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out enconding, out extension, out streamids, out warnings);

                //para que aparezca el pdf en el navegador
                //Response.Buffer = true;
                //response.Clear();
                //response.ContentType = mimeType;
                ////response.AppendHeader("content-Disposition", "inline: filename=TrainingOfficialRecord." + extension);
                //response.AppendHeader("content-Disposition", "inline: filename=" + oFact.NombreArchivoXML + "." + extension);
                //response.BinaryWrite(bytes);
                //response.End();
                //Response.Clear();






                //string filename = Request.QueryString["filename"].ToString();

                //Response.Clear();
                //Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", oFact.NombreArchivoXML + "." + extension));
                //Response.ContentType = "application/pdf";
                //Response.WriteFile(Server.MapPath("~/DocumentoXML/" + oFact.NombreArchivoXML + ".xml"));
                //Response.End();



                //string filename = oFact.NombreArchivoXML.ToString();

                //Response.Clear();
                //Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", filename));
                //Response.ContentType = "application/pdf";
                //Response.WriteFile(Server.MapPath("~/DocumentoXML/" + filename +".pdf"));
                //Response.End();



                //para que se descargue
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = mimeType;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + oFact.NombreArchivoXML + "." + extension);
                HttpContext.Current.Response.BinaryWrite(bytes);
                HttpContext.Current.Response.Flush();
                //HttpContext.Current.Response.ContentType = "application/pdf";
                //HttpContext.Current.Response.OutputStream.Flush();




                //HttpContext.Current.Response.End();

                //Response.Write("<script>");
                //Response.Write("window.open('../../DocumentoXML/ " + oFact.NombreArchivoXML + ".pdf'" + ",'_blank')");
                //Response.Write("</script>");



            }
            catch (Exception ex)
            {
                //Session["MsjeErrorCreatedPDF"] = Constantes.msjPdfFailCreated;
                Response.Write("<script language=javascript>javascript:window.history.back();</script>");
            }
        }
        #endregion
    }
}