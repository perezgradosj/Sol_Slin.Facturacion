using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;




using Microsoft.Reporting.WinForms;
using System.Windows.Documents;
using System.IO;
using System.Drawing.Printing;
using System.Drawing.Imaging;


namespace Slin.Facturacion.PrintDoc.Helper
{
    public class Printed
    {
        public void PrintRDLC(string FileName, string DataSourceName, Object DataSource, string datasourceName2, Object DataSource2, ReportParameter[] Dpr, string printname)
        {
            try
            {
                ReportDataSource rds = new ReportDataSource(DataSourceName, DataSource);
                ReportDataSource rds2 = new ReportDataSource(datasourceName2, DataSource2);


                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;

                ReportViewer viewer = new ReportViewer();
                viewer.ProcessingMode = ProcessingMode.Local;
                //viewer.LocalReport.ReportPath = "Report/" + FileName + ".rdlc";

                viewer.LocalReport.ReportPath = FileName;

                //List<ReportParameter> lstParameter = new List<ReportParameter>();
                //foreach (var item in ArrayReportParameter)
                //{
                //    ReportParameter parameter = new ReportParameter(item.Split(',')[0].ToString(), item.Split(',')[1].ToString());
                //    lstParameter.Add(parameter);
                //}

                viewer.LocalReport.SetParameters(Dpr);

                viewer.LocalReport.DataSources.Add(rds);
                viewer.LocalReport.DataSources.Add(rds2);




                Imprimir(viewer.LocalReport, printname);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ", " + ex.InnerException);
            }
        }

        public void PrintRDLC(string FileName, string DataSourceName, Object DataSource, string DataSourceName2, Object DataSource2, string DataSourceName3, Object DataSource3, ReportParameter[] Dpr, string printname)
        {
            try
            {
                ReportDataSource rds = new ReportDataSource(DataSourceName, DataSource);
                ReportDataSource rds2 = new ReportDataSource(DataSourceName2, DataSource2);
                ReportDataSource rds3 = new ReportDataSource(DataSourceName3, DataSource3);


                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;

                ReportViewer viewer = new ReportViewer();
                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.ReportPath = FileName;

                viewer.LocalReport.SetParameters(Dpr);

                viewer.LocalReport.DataSources.Add(rds);
                viewer.LocalReport.DataSources.Add(rds2);
                viewer.LocalReport.DataSources.Add(rds3);

                Imprimir(viewer.LocalReport, printname);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ", " + ex.InnerException);
            }

            
        }





        public void PrintRDLC_Resumen(string FileName, string DataSourceName, Object DataSource, string datasourceName2, Object DataSource2, ReportParameter[] Dpr, string printname)
        {
            try
            {
                ReportDataSource rds = new ReportDataSource(DataSourceName, DataSource);
                ReportDataSource rds2 = new ReportDataSource(datasourceName2, DataSource2);

                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;

                ReportViewer viewer = new ReportViewer();
                viewer.ProcessingMode = ProcessingMode.Local;
                //viewer.LocalReport.ReportPath = "Report/" + FileName + ".rdlc";

                viewer.LocalReport.ReportPath = FileName;
                viewer.LocalReport.SetParameters(Dpr);

                viewer.LocalReport.DataSources.Add(rds);
                viewer.LocalReport.DataSources.Add(rds2);

                Imprimir(viewer.LocalReport, printname);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ", " + ex.InnerException);
            }
        }

















        private int m_currentPageIndex;
        private IList<Stream> m_streams;

        public void Imprimir(LocalReport argReporte, string printname)
        {
            //
            Export(argReporte);
            m_currentPageIndex = 0;
            Print(printname);
        }

        private void Export(LocalReport report)
        {
            //Export the given report as an EMF (Enhanced Metafile) file.
            //string deviceInfo =
            //  "<DeviceInfo>" +
            //  "  <OutputFormat>EMF</OutputFormat>" +
            //  "  <PageWidth>8.5in</PageWidth>" +
            //  "  <PageHeight>11in</PageHeight>" +
            //  "  <MarginTop>0.25in</MarginTop>" +
            //  "  <MarginLeft>0.25in</MarginLeft>" +
            //  "  <MarginRight>0in</MarginRight>" +
            //  "  <MarginBottom>0.25in</MarginBottom>" +
            //  "</DeviceInfo>";
            string deviceInfo =
             "<DeviceInfo>" +
             //"  <OutputFormat>PDF</OutputFormat>" +
             "  <OutputFormat>EMF</OutputFormat>" +
             "  <PageWidth>8in</PageWidth>" +
             "  <PageHeight>11in</PageHeight>" +
             "  <MarginTop>0in</MarginTop>" +
             "  <MarginLeft>0in</MarginLeft>" +
             "  <MarginRight>0in</MarginRight>" +
             "  <MarginBottom>0in</MarginBottom>" +
             "  <DeviceOrientation>landscape</DeviceOrientation>" +
             "</DeviceInfo>";

            Warning[] warnings;
            m_streams = new List<Stream>();

            try
            {
                report.Render("Image", deviceInfo, CreateStream, out warnings);
            }
            catch (Exception ex)
            {

                throw;
            }

            foreach (Stream stream in m_streams)
            { stream.Position = 0; }
        }

        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            //Routine to provide to the report renderer, in order to save an image for each page of the report.
            String Date = String.Format("{0:dMyyyyHHmmss.FFF}", DateTime.Now);

            Stream stream = new FileStream(@"..\..\" + name + Date + "." + fileNameExtension, FileMode.Create);
            m_streams.Add(stream);
            return stream;
        }

        private void Print(string printname)
        {
            //
            try
            {
                PrintDocument printDoc;

                String printerName = printname; //ImpresoraPredeterminada();

                if (m_streams == null || m_streams.Count == 0)
                { return; }

                printDoc = new PrintDocument();
                printDoc.PrinterSettings.PrinterName = printerName;
                if (!printDoc.PrinterSettings.IsValid)
                {
                    //string msg = String.Format("Can't find printer \"{0}\".", printerName);
                    return;
                }
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                printDoc.Print();
            }
            catch (Exception)
            {
                throw;
            }

        }


        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            //Handler for PrintPageEvents
            Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);
            ev.Graphics.DrawImage(pageImage, ev.PageBounds);
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }







    }
}
