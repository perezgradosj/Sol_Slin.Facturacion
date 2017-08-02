using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Common;
using Slin.Facturacion.Proxies.ServicioFacturacion;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.ComponentModel;

using ExcelLibrary.SpreadSheet;

namespace Slin.Facturacion.Electronica.Web.Views.Informes
{
    public partial class ExportarExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                RecibirLista();
                //RenderReport(ReportViewer1, Response);
            }
        }


        #region ENTITY 

        private ListaDocumento listaReporteDocumento;
        public ListaDocumento ListaReporteDocumento
        {
            get { return listaReporteDocumento; }
            set { listaReporteDocumento = value; }
        }

        #endregion

        #region METHOD

        void RecibirLista()
        {
            try
            {
                ListaReporteDocumento = (ListaDocumento)(Session)["Excel"];

                if (ListaReporteDocumento == null)
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
                ReportViewer1.LocalReport.ReportPath = ("Report/Excel/ReporteDocumento.rdlc");
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_DocumentoCab", ListaReporteDocumento));
                ReportViewer1.LocalReport.Refresh();
                ExportarToExcel();
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
                string extension = ".xls";

                byte[] bytes = ReportViewer1.LocalReport.Render(
                   "Excel", null, out mimeType, out encoding,
                    out extension,
                   out streamids, out warnings);

                FileStream fs = new FileStream(Server.MapPath("~/DocumentoXML/" + "ReporteDocumento.xls"),
                   FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

                //Excel();

                mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = mimeType;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + "ReporteDocumento" + "." + "xls");
                //HttpContext.Current.Response.AddHeader("Content-Length", "attachment; filename=" + "ReporteDocumento" + "." + "xlsx");

                //HttpContext.Current.Response.AddHeader("Content-Length", "attachment; filename="+"FileName.xlsx".Length);
                HttpContext.Current.Response.BinaryWrite(bytes);
                HttpContext.Current.Response.Flush();

                //Response.Clear();
                //Response.ContentType = "application/octet-stream";
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + "ReporteDocumento.xls");
                //Response.WriteFile("../../DocumentoXML/" + "ReporteDocumento.xls");
                //Response.Flush();

                var file = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + "ReporteDocumento" + ".xls"));
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);

                Session.Remove("Excel");
                HttpContext.Current.Response.End();
            }
            catch (Exception ex) { }
        }

        private void Excel()
        {
            var lista = (ListaDocumento)Session["Excel"];
            System.Data.DataTable dt = ConvertToDataTable(lista);
            System.Data.DataSet ds = new System.Data.DataSet();
            ds.Tables.Add(dt);
            ExcelLibrary.DataSetHelper.CreateWorkbook(Server.MapPath("~/DocumentoXML/" + "ReporteDocumento.xlsx"), ds);

            //Workbook book = new Workbook();
            //Worksheet sheet = book.Worksheets.Add("Sample");
            //WorksheetRow row = sheet.Table.Rows.Add();
            //row.Cells.Add("Hello World");
            //book.Save(@"c:\test.xls");
        }



        public System.Data.DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            System.Data.DataTable table = new System.Data.DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                System.Data.DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
        #endregion
    }
}