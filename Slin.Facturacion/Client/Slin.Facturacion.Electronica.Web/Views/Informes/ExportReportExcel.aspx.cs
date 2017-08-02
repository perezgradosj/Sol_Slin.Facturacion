using Microsoft.Reporting.WebForms;
using Slin.Facturacion.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;

namespace Slin.Facturacion.Electronica.Web.Views.Informes
{
    public partial class ExportReportExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                RecibirLista();
            }
        }

        #region ENTITY

        private WCFSeguridad.ListaLogueoClass olistlogueo;
        public WCFSeguridad.ListaLogueoClass oListLogueo
        {
            get { return olistlogueo; }
            set { olistlogueo = value; }
        }

        #endregion

        #region METHOD

        void RecibirLista()
        {
            try
            {
                oListLogueo = (WCFSeguridad.ListaLogueoClass)(Session)["ExcelListLogueoClass"];

                if (oListLogueo == null)
                {
                    Response.Write("<script language=javascript>javascript:window.history.back();</script>");
                }
                PasarParametrosRPT();
            }
            catch (Exception ex)
            {

            }
        }

        void PasarParametrosRPT()
        {
            try
            {
                ReportViewer1.LocalReport.ReportPath = ("Report/Excel/RPT_RegistroLogueo.rdlc");

                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_ExcelReport", oListLogueo));
                ReportViewer1.LocalReport.Refresh();

                var TipoExpor = (string)Session["TipoExport"];

                if (TipoExpor == Constantes.ValorExportPDF)
                {
                    ExportarToPDF();
                }
                else
                {
                    ExportarToExcel();
                }
            }
            catch (Exception ex)
            {

            }

        }

        void ExportarToExcel()
        {
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = ReportViewer1.LocalReport.Render(
                   "Excel", null, out mimeType, out encoding,
                    out extension,
                   out streamids, out warnings);

                FileStream fs = new FileStream(Server.MapPath("~/DocumentoXML/" + "RPT_RegistroLogueo.xls"),
                   FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();


                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = mimeType;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + "RPT_RegistroLogueo" + "." + extension);
                HttpContext.Current.Response.BinaryWrite(bytes);
                HttpContext.Current.Response.Flush();

                //Response.Clear();
                //Response.ContentType = "application/octet-stream";
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + "ReporteDocumento.xls");
                //Response.WriteFile("../../DocumentoXML/" + "ReporteDocumento.xls");
                //Response.Flush();

                var file = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + "RPT_RegistroLogueo" + ".xls"));
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);

                Session.Remove("ExcelListLogueoClass");
                Session.Remove("TipoExport");
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {

            }

        }


        void ExportarToPDF()
        {
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = ReportViewer1.LocalReport.Render(
                   "PDF", null, out mimeType, out encoding,
                    out extension,
                   out streamids, out warnings);

                FileStream fs = new FileStream(Server.MapPath("~/DocumentoXML/" + "RPT_RegistroLogueo.pdf"),
                   FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();


                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = mimeType;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + "RPT_RegistroLogueo" + "." + extension);
                HttpContext.Current.Response.BinaryWrite(bytes);
                HttpContext.Current.Response.Flush();

                //Response.Clear();
                //Response.ContentType = "application/octet-stream";
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + "ReporteDocumento.xls");
                //Response.WriteFile("../../DocumentoXML/" + "ReporteDocumento.xls");
                //Response.Flush();

                var file = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + "RPT_RegistroLogueo" + ".pdf"));
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);

                Session.Remove("ExcelListLogueoClass");
                Session.Remove("TipoExport");
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {

            }

        }
        #endregion
    }
}