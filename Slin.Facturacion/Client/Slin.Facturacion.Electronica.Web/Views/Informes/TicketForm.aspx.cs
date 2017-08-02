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

using System.Drawing.Printing;
//using Microsoft.Reporting.WinForms;

using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using System.Data;

namespace Slin.Facturacion.Electronica.Web.Views.Informes
{
    public partial class TicketForm : System.Web.UI.Page
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
        ListaFacturaElectronica oListaMontos = new ListaFacturaElectronica();
        ListaDetalleFacturaElectronica oListaDetalle = new ListaDetalleFacturaElectronica();

        ListaFacturaElectronica listamonto = new ListaFacturaElectronica();

        public string TextoNexto = string.Empty;


        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        //public ReportViewer reportGR;
        //public ReportViewer ReportGR = new ReportViewer();

        private ReportViewer reportGR;
        public ReportViewer ReportGR
        {
            get { return reportGR; }
            set { reportGR = value; }
        }

        #endregion

        #region METHOD

        void ObtenerUsuarioLogueado()
        {
            oUsuarioLogeado = new WCFSeguridad.Usuario();
            oUsuarioLogeado.Empresa = new WCFSeguridad.Empresa();
            

            oUsuarioLogeado = (WCFSeguridad.Usuario)Session["UsuarioLogueadoFact"];
            oUsuarioLogeado.Empresa.Direccion = "San Isidro 240";
            oUsuarioLogeado.Empresa.PaginaWeb = "www.slin.com.pe";
        }

        void RecibirFactura()
        {
            oFact.TipoDocumento = new TipoDocumento();
            oFact = (FacturaElectronica)Session["FacturaPDF"];

            oFact.TipoDocumento.Descripcion = "Factura";

            oListaDetalle = (ListaDetalleFacturaElectronica)Session["FacturaDetallePDF"];
            oListaMontos = (ListaFacturaElectronica)Session["FacturaMontos"];

            GenerarPDF();
        }

        void GenerarPDF()
        {
            CrearCodigoPDF417();
            LlenarListaMontos();
            EnviarParametrosRPT();

            Session.Remove("FacturaPDF");
            Session.Remove("FacturaDetallePDF");
            Session.Remove("FacturaMontos");
        }

        void CrearCodigoPDF417()
        {
            string contenidoCodigoPDF417;

            string val1 = oFact.Empresa.RUC + "|" + oFact.TipoDocumento.CodigoDocumento + "|" + oFact.Serie.NumeroSerie + "|" + oFact.NumeroDocumento;
            string val2 = Convert.ToString(oListaMontos[0].MontoIgvCad + "|" + oListaMontos[0].MontoTotalCad);
            string val3 = oFact.FirmaDigital;//fecha para el codigo de barra pdf417
            string val4 = oFact.Cliente.TipoDocumentoIdentidad.Codigo + "|" + oFact.Cliente.ClienteRuc;
            string val5 = oFact.ValorResumen + "|" + oFact.ValorFirma;
            contenidoCodigoPDF417 = val1 + "|" + val2 + "|" + val3 + "|" + val4 + "|" + val5;

            BarcodePDF417 opdf417 = new BarcodePDF417();
            opdf417.Options = BarcodePDF417.PDF417_USE_ASPECT_RATIO;
            opdf417.ErrorLevel = 8;
            opdf417.SetText(contenidoCodigoPDF417);
            System.Drawing.Bitmap imagen = new System.Drawing.Bitmap(opdf417.CreateDrawingImage(Color.Black, Color.White));
            imagen.Save(Server.MapPath("~/DocumentoXML/CodigoQR.bmp"));
        }

        void LlenarListaMontos()
        {
            string RutaPDF417 = Path.Combine(System.Windows.Forms.Application.StartupPath, Server.MapPath("~/DocumentoXML/CodigoQR.bmp"));

            for (int i = 0; i <= oListaMontos.Count - 1; i++)
            {
                listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Importe Total", MontoTotalCad = oListaMontos[i].MontoTotalCad, CodigoPDF417 = File.ReadAllBytes(RutaPDF417) });
                listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total IGV(%18)", MontoTotalCad = oListaMontos[i].MontoIgvCad, CodigoPDF417 = File.ReadAllBytes(RutaPDF417) });

                if (Convert.ToDecimal(oListaMontos[i].TotalDescuento) > 0)
                {
                    listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Total Dscto", MontoTotalCad = oListaMontos[i].TotalDescuento, CodigoPDF417 = File.ReadAllBytes(RutaPDF417) });
                }

                //if (Convert.ToDecimal(oListaMontos[i].TotalExonerado) > 0)
                //{
                //    listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Exonerado", MontoTotalCad = oListaMontos[i].TotalExonerado, CodigoPDF417 = File.ReadAllBytes(RutaPDF417) });
                //}

                listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Exonerado", MontoTotalCad = oListaMontos[i].TotalExonerado, CodigoPDF417 = File.ReadAllBytes(RutaPDF417) });
                listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total no Gravado", MontoTotalCad = oListaMontos[i].TotalnoGravado, CodigoPDF417 = File.ReadAllBytes(RutaPDF417) });
                listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Gravado", MontoTotalCad = oListaMontos[i].TotalGravado, CodigoPDF417 = File.ReadAllBytes(RutaPDF417) });

                TextoNexto = oListaMontos[i].MontoTotalLetras;
            }

            //this.ReportViewer1.LocalReport.EnableExternalImages = true;
            var file = Path.Combine(HttpContext.Current.Server.MapPath("~/DocumentoXML/CodigoQR.bmp"));
            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);
        }

        void EnviarParametrosRPT()
        {
            ObtenerUsuarioLogueado();
            //ReportGR = new ReportViewer();

            if (oFact.TipoDocumento.CodigoDocumento == Constantes.Factura)
            {
                ReportViewer1.LocalReport.ReportEmbeddedResource = "Slin.Facturacion.Electronica.Web.Report.Impresion.TicketFactura.rdlc";
            }
            else if (oFact.TipoDocumento.CodigoDocumento == Constantes.Boleta)
            {
                ReportViewer1.LocalReport.ReportEmbeddedResource = "Slin.Facturacion.Electronica.Web.Report.Impresion.TicketFactura.rdlc";

                if (oFact.Cliente.ClienteRuc.Length == 0)
                {
                    oFact.Cliente.ClienteRuc = oFact.Cliente.Nombres;
                }
            }

            ReportParameter[] Dpr = new ReportParameter[8];
            Dpr[0] = new ReportParameter("RazonSocial", oUsuarioLogeado.Empresa.RazonSocial.ToUpper());
            Dpr[1] = new ReportParameter("RucEmpresa", oUsuarioLogeado.Empresa.RUC);
            Dpr[2] = new ReportParameter("DireccionEmpresa", oUsuarioLogeado.Empresa.Direccion);
            Dpr[3] = new ReportParameter("TipoDocumentoDesc", oFact.TipoDocumento.Descripcion);
            Dpr[4] = new ReportParameter("NumeroSerie", oFact.Serie.NumeroSerie);
            Dpr[5] = new ReportParameter("Correlativo", oFact.NumeroDocumento);
            Dpr[6] = new ReportParameter("FechaEmision", oFact.FechaEmision.ToShortDateString());
            Dpr[7] = new ReportParameter("PaginaWebEmpresa", oUsuarioLogeado.Empresa.PaginaWeb);

            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_TicketDetalle", oListaDetalle));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_MontosCabTicket", listamonto));

            //ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_TicketDetalle", oListaDetalle));
            //ReportGR.LocalReport.DataSources.Add(new ReportDataSource("DS_MontosCabTicket", listamonto));

            ReportViewer1.LocalReport.SetParameters(Dpr);
            ReportViewer1.LocalReport.Refresh();

            //ReportGR.LocalReport.SetParameters(Dpr);
            //ReportGR.LocalReport.Refresh();
            //ReportGR.RefreshReport();
        }


        public void RenderReport(ReportViewer reportViewer, HttpResponse response)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string enconding;
            string extension;
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out enconding, out extension, out streamids, out warnings);

            //para que aparezca el pdf en el navegador
            //response.Clear();
            //response.ContentType = mimeType;
            //response.AppendHeader("content-Disposition", "inline: filename=TrainingOfficialRecord." + extension);
            //response.BinaryWrite(bytes);
            //response.End();
            //Response.Clear();

            //para que se descargue
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = mimeType;
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + oFact.NombreArchivoXML + "." + extension);
            HttpContext.Current.Response.BinaryWrite(bytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        #endregion

















    }
}