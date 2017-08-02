using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using Slin.Facturacion.Proxies.ServicioFacturacion;

//using Microsoft.Reporting.WinForms;

namespace Slin.Facturacion.Electronica.Web.Helper
{
    public class Impresion : IDisposable
    {

        private int m_currentPageIndex;
        private IList<Stream> m_streams;
        private DataTable dt { get; set; }
        private string reportName { get; set; }

        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding,
                              string mimeType, bool willSeek)
        {
            Stream stream = new FileStream(name + "." + fileNameExtension, FileMode.Create);
            m_streams.Add(stream);
            return stream;
        }


        //private void Export(LocalReport report)
        private void Export(ReportViewer report)
        {
            string deviceInfo =
              "<DeviceInfo>" +
              "  <OutputFormat>EMF</OutputFormat>" +
              "  <PageWidth>8,27in</PageWidth>" +
              "  <PageHeight>11,69in</PageHeight>" +
              "  <MarginTop>0.00in</MarginTop>" +
              "  <MarginLeft>0.00in</MarginLeft>" +
              "  <MarginRight>0.00in</MarginRight>" +
              "  <MarginBottom>0.00in</MarginBottom>" +
              "</DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            //report.Render("Image", deviceInfo, CreateStream, out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }


        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            //Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            //ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }


        private void Print()
        {
            //string printerName = ConfigurationManager.AppSettings["Impresora"].ToString();
            //string printerName = ConfigurationManager.AppSettings["Microsoft XPS Document Writer"].ToString();

            string printerName = "Microsoft XPS Document Writer";

            //if (m_streams == null || m_streams.Count == 0)
            //    return;

            PrintDocument printDoc = new PrintDocument();
            printDoc.PrinterSettings.PrinterName = printerName;

            if (!printDoc.PrinterSettings.IsValid)
            {
                string msg = String.Format("No se puede encontrar la impresora \"{0}\".", printerName);
                Console.WriteLine(msg);
                return;
            }
            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);





            //printDoc.PrinterSettings.Copies = 3;

            //m_currentPageIndex = 0;
            printDoc.Print();
        }

        //private void Run(string rs, string _datasource, DataTable _dt)
        //{
        //    LocalReport report = new LocalReport();
        //    report.ReportPath = rs;
        //    report.DataSources.Add(new ReportDataSource(_datasource, _dt));
        //    Export(report);
        //    m_currentPageIndex = 0;
        //    Print();
        //}

        public void Run(ReportViewer rs, ListaDetalleFacturaElectronica _dt, ListaFacturaElectronica _dt2)
        {
            //LocalReport report = new LocalReport();

            ReportViewer report = new ReportViewer();

            report = rs;



            //report.ReportPath = rs;
            //report.DataSources.Add(new ReportDataSource("DS_TicketDetalle", _dt));
            //report.DataSources.Add(new ReportDataSource("DS_MontosCabTicket", _dt2));
            Export(report);
            m_currentPageIndex = 0;
            Print();
        }


        //public void Run(string rs)
        //{
        //    LocalReport report = new LocalReport();
        //    report.ReportPath = rs;
        //    //report.DataSources.Add(new ReportDataSource(_datasource, _dt));


        //    Export(report);
        //    m_currentPageIndex = 0;
        //    Print();
        //}


        private void RunDataSources(string rs, IList<DataTable> _dt)
        {
            LocalReport report = new LocalReport();
            report.ReportPath = rs;
            foreach (DataTable st in _dt)
            {
                report.DataSources.Add(new ReportDataSource(st.TableName, st));
            }

            //Export(report);
            m_currentPageIndex = 0;
            Print();
        }


        private void RunDataSourcesParametros(string rs, IList<DataTable> _dt, IList<ReportParameter> param)
        {
            LocalReport report = new LocalReport();
            report.ReportPath = rs;
            foreach (DataTable st in _dt)
            {
                report.DataSources.Add(new ReportDataSource(st.TableName, st));
            }
            foreach (ReportParameter rparam in param)
            {
                report.SetParameters(param);
            }
            //Export(report);
            m_currentPageIndex = 0;
            Print();
        }

        private void RunConParametros(string rs, string _datasource, IList<ReportParameter> parame, DataTable _dt)
        {
            LocalReport report = new LocalReport();
            report.ReportPath = rs;
            report.DataSources.Add(new ReportDataSource(_datasource, _dt));
            foreach (ReportParameter param in parame)
            {
                report.SetParameters(param);
            }
            //Export(report);
            m_currentPageIndex = 0;
            Print();
        }

        private void RunConParametro(string rs, string _datasource, ReportParameter param, DataTable _dt)
        {
            LocalReport report = new LocalReport();
            report.ReportPath = rs;
            report.DataSources.Add(new ReportDataSource(_datasource, _dt));

            report.SetParameters(param);

            //Export(report);
            m_currentPageIndex = 0;
            Print();
        }

        public void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
            }
        }

        public void ImprimirReporteConParametros(string _reportName, string _source, DataTable _dt, IList<ReportParameter> parametros)
        {
            reportName = _reportName;
            dt = _dt;
            using (Impresion imp = new Impresion())
            {
                imp.RunConParametros(reportName, _source, parametros, _dt);
            }

        }

        public void ImprimirReporteConDataSources(string _reportName, IList<DataTable> _dt)
        {

            using (Impresion imp = new Impresion())
            {
                imp.RunDataSources(_reportName, _dt);
            }

        }


        public void ImprimirReporteConDataSourcesConParametros(string _reportName, IList<DataTable> _dt, IList<ReportParameter> rparam)
        {

            using (Impresion imp = new Impresion())
            {
                imp.RunDataSourcesParametros(_reportName, _dt, rparam);
            }

        }


        public void ImprimirReporteConParametro(string _reportName, string _source, DataTable _dt, ReportParameter parametro)
        {
            reportName = _reportName;
            dt = _dt;
            using (Impresion imp = new Impresion())
            {
                imp.RunConParametro(reportName, _source, parametro, _dt);
            }

        }

        public void ImprimirReporte(string _reportName, string _source, DataTable _dt)
        {
            reportName = _reportName;
            dt = _dt;
            using (Impresion imp = new Impresion())
            {
                //imp.Run(reportName, _source, _dt);

                //imp.Run(reportName);
            }

        }


        public static ReportParameter crearParametro(string nombre, DbType tipo, string valor)
        {
            try
            {
                ReportParameter param = new ReportParameter();
                param.Name = nombre;
                param.Values.Add(valor);

                return param;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


    }
}