using Aspose.Pdf;
using Fonet;
using iTextSharp.text;
using Microsoft.Reporting.WebForms;
using NReco.PdfGenerator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;



namespace Pdf_Xslt
{
    class Program
    {
        static void Main(string[] args)
        {
            //documentoXml = new XPathDocument("D:\\20508622326-01-F001-00000334.xml");
            //pagexslt = new XslTransform();
            //pagexslt.Load("D:\\FEE.xslt");
            //HtmlTextWriter writer;

            //pagexslt.Transform(documentoXml, null, writer);
            new Program().Execute();
        }


        //static XPathDocument documentoXml;
        //static XslTransform pagexslt;

        //protected override void Render(HtmlTextWriter writer)
        //{
        //    pagexslt.Transform(documentoXml, null, writer);
        //}


        XPathDocument documentoXml;
        XslTransform pagexslt;

        private void Execute()
        {
            //var htmlContent = String.Format("<body>Hello world: {0}</body>", DateTime.Now);
            //var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            //var pdfBytes = htmlToPdf.GeneratePdf(htmlContent);
            //htmlToPdf.GeneratePdfFromFile("http://www.nrecosite.com/", null, "D:\\export.pdf");



            //XmlReaderSettings booksSettings = new XmlReaderSettings();
            //booksSettings.Schemas.Add("http://tempuri.org/books.xsd", "D:\\VALIDATE\\books.xsd");
            //booksSettings.ValidationType = ValidationType.Schema;
            //booksSettings.ValidationEventHandler += new ValidationEventHandler(booksSettingsValidationEventHandler);

            //XmlReader books = XmlReader.Create("D:\\VALIDATE\\books.xml", booksSettings);

            //while (books.Read()) { }



            //Aspose.Pdf.Generator.Pdf pdf1 = new Aspose.Pdf.Generator.Pdf();


            ////Aspose.Pdf.License license = new Aspose.Pdf.License();
            ////license.SetLicense("D:\\Aspose.pdf.lic");
            //pdf1.BindXML("D:\\FEE.xml", "D:\\FEE.xslt");

            ////pdf1.BindHTML("<?xml version='1.0' encoding='utf-8'?><xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform' xmlns:msxsl='urn:schemas-microsoft-com:xslt' exclude-result-prefixes='msxsl'  xmlns:ds='http://www.w3.org/2000/09/xmldsig#' xmlns:qdt='urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2' xmlns:cbc='urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:udt='urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2' xmlns:cac='urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2' xmlns:ext='urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2' xmlns:sac='urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1' xmlns:ccts='urn:un:unece:uncefact:documentation:2' xmlns='urn:oasis:names:specification:ubl:schema:xsd:Invoice-2'  ><xsl:output method='html' indent='yes'/><xsl:template match='@* | node()'><!--<xsl:copy><xsl:apply-templates select='@* | node()'/></xsl:copy>--><html xmlns='http://www.w3.org/1999/xhtml'><body>         <table><tr><td></td></tr></table><table><tr><td><xsl:value-of select='cac:AccountingSupplierParty/cbc:CustomerAssignedAccountID'/></td></tr></table><table border='1'><tr><table border='1'><tr><td>FACTURA ELECTRÓNICA</td></tr><tr><td>R.U.C.:<xsl:value-of select='cac:AccountingSupplierParty/cbc:CustomerAssignedAccountID'/></td></tr><tr></tr><tr><td><xsl:value-of select='cbc:ID'/></td></tr></table></tr></table><table><tr><td colspan='3'></td><td></td><td></td><td></td><td></td></tr></table></body></html></xsl:template></xsl:stylesheet>");
            //pdf1.Save("D:\\helloword.pdf");


            //Aspose.Pdf.Generator.Pdf pdf1 = new Aspose.Pdf.Generator.Pdf();
            //Aspose.Pdf.Generator.Section section1 = pdf1.Sections.Add();
            //Aspose.Pdf.Generator.Text text1 = new Aspose.Pdf.Generator.Text("<?xml version='1.0' encoding='utf-8'?><xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform' xmlns:msxsl='urn:schemas-microsoft-com:xslt' exclude-result-prefixes='msxsl'  xmlns:ds='http://www.w3.org/2000/09/xmldsig#' xmlns:qdt='urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2' xmlns:cbc='urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:udt='urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2' xmlns:cac='urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2' xmlns:ext='urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2' xmlns:sac='urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1' xmlns:ccts='urn:un:unece:uncefact:documentation:2' xmlns='urn:oasis:names:specification:ubl:schema:xsd:Invoice-2'  ><xsl:output method='html' indent='yes'/><xsl:template match='@* | node()'><!--<xsl:copy><xsl:apply-templates select='@* | node()'/></xsl:copy>--><html xmlns='http://www.w3.org/1999/xhtml'><body>         <table><tr><td></td></tr></table><table><tr><td><xsl:value-of select='cac:AccountingSupplierParty/cbc:CustomerAssignedAccountID'/></td></tr></table><table border='1'><tr><table border='1'><tr><td>FACTURA ELECTRÓNICA</td></tr><tr><td>R.U.C.:<xsl:value-of select='cac:AccountingSupplierParty/cbc:CustomerAssignedAccountID'/></td></tr><tr></tr><tr><td><xsl:value-of select='cbc:ID'/></td></tr></table></tr></table><table><tr><td colspan='3'></td><td></td><td></td><td></td><td></td></tr></table></body></html></xsl:template></xsl:stylesheet>");

            //pdf1.BindXML("D:\\20508622326-01-F001-00000334.xml", "D:\\FEE.xslt");

            //section1.Paragraphs.Add(text1);

            //pdf1.Save("D:\\pdf_aspose.pdf");



            //documentoXml = new XPathDocument("D:\\20508622326-01-F001-00000334.xml");
            //pagexslt = new XslTransform();
            //pagexslt.Load("D:\\FEE.xslt");

            string num_cpe = "20508622326-01-F001-00000368";

            var htmlToPdf = new HtmlToPdfConverter();

            
            HtmlToPdfConverter html = new HtmlToPdfConverter();


            //html.GeneratePdfFromFile("","");

            //Renderx(writer);

            //htmlToPdf.GeneratePdf(pagexslt.XmlResolver.ToString(), null, "D:\\pdf.pdf");




            //XDocument fileXML;
            //String xmlString;
            //using (StreamReader sr = new StreamReader("D:\\VALIDATE\\SLIN\\Procesos\\smc\\Report\\FEE.xslt", Encoding.GetEncoding("iso-8859-1")))
            //{
            //    xmlString = sr.ReadToEnd();
            //    StringReader reader = new StringReader(xmlString);
            //    fileXML = XDocument.Load(reader, System.Xml.Linq.LoadOptions.None);
            //}
            //


            var xslt_file = File.ReadAllText("D:\\VALIDATE\\SLIN\\Procesos\\smc\\Report\\FEE.xslt", Encoding.UTF8);


            var xslt = File.ReadAllText("D:\\FEE.xslt", Encoding.GetEncoding("ISO-8859-1"));

            XslCompiledTransform transform = new XslCompiledTransform();
            using (XmlReader reader = XmlReader.Create(new StringReader(xslt_file)))
            {
                transform.Load(reader);
            }

            //XmlDocument xml = new XmlDocument();
            //xml.Load("D:\\20508622326-01-F001-00000334.xml");


            var xml = File.ReadAllText("D:\\VALIDATE\\SLIN\\ProcesoCE\\XML\\" + num_cpe + ".xml", Encoding.GetEncoding("ISO-8859-1"));


            StringWriter results = new StringWriter();
            using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
            {
                transform.Transform(reader, null, results);
            }






            //return results.ToString();

            //htmlToPdf.GeneratePdf("<?xml version='1.0' encoding='utf-8'?><xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform' xmlns:msxsl='urn:schemas-microsoft-com:xslt' exclude-result-prefixes='msxsl'  xmlns:ds='http://www.w3.org/2000/09/xmldsig#' xmlns:qdt='urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2' xmlns:cbc='urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:udt='urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2' xmlns:cac='urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2' xmlns:ext='urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2' xmlns:sac='urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1' xmlns:ccts='urn:un:unece:uncefact:documentation:2' xmlns='urn:oasis:names:specification:ubl:schema:xsd:Invoice-2'  ><xsl:output method='html' indent='yes'/><xsl:template match='@* | node()'><!--<xsl:copy><xsl:apply-templates select='@* | node()'/></xsl:copy>--><html xmlns='http://www.w3.org/1999/xhtml'><body>         <table><tr><td></td></tr></table><table><tr><td><xsl:value-of select='cac:AccountingSupplierParty/cbc:CustomerAssignedAccountID'/></td></tr></table><table border='1'><tr><table border='1'><tr><td>FACTURA ELECTRÓNICA</td></tr><tr><td>R.U.C.:<xsl:value-of select='cac:AccountingSupplierParty/cbc:CustomerAssignedAccountID'/></td></tr><tr></tr><tr><td><xsl:value-of select='cbc:ID'/></td></tr></table></tr></table><table><tr><td colspan='3'></td><td></td><td></td><td></td><td></td></tr></table></body></html></xsl:template></xsl:stylesheet>", null, "D:\\pdf_withhtml.pdf");


            if (System.IO.File.Exists("D:\\pdf_withhtml.pdf"))
                System.IO.File.Delete("D:\\pdf_withhtml.pdf");


            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = utf8.GetBytes(results.ToString());
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            string msg = iso.GetString(isoBytes);


            msg = msg.Replace("REPLACECODE", "20100103738");


            //htmlToPdf.GeneratePdf((msg.ToString(CultureInfo.CurrentCulture)), null, "D:\\pdf_withhtml.pdf");



            //htmlToPdf.GeneratePdf(msg, null, "D:\\pdf_withhtml.pdf");
            //html.GeneratePdfFromFile(msg, null);


            //var rex = html;
            //html.GeneratePdf(msg, null, "D:\\pdf_withhtml_2.pdf");


            if (System.IO.File.Exists("D:\\VALIDATE\\SLIN\\ProcesoCE\\PDF\\" + num_cpe + ".html"))
                System.IO.File.Delete("D:\\VALIDATE\\SLIN\\ProcesoCE\\PDF\\" + num_cpe + ".html");

            using (StreamWriter sw = new StreamWriter("D:\\VALIDATE\\SLIN\\ProcesoCE\\PDF\\" + num_cpe + ".html", true, Encoding.GetEncoding("ISO-8859-1")))
            {
                sw.WriteLine(msg);
            }




            


            if (System.IO.File.Exists("D:\\VALIDATE\\SLIN\\ProcesoCE\\PDF\\" + num_cpe + ".pdf"))
                System.IO.File.Delete("D:\\VALIDATE\\SLIN\\ProcesoCE\\PDF\\" + num_cpe + ".pdf");



            var procStartInfo = new ProcessStartInfo("D:\\VALIDATE\\SLIN\\Procesos\\smc\\bin\\wkhtmltopdf.exe", "D:\\VALIDATE\\SLIN\\ProcesoCE\\PDF\\" + num_cpe +  ".html D:\\VALIDATE\\SLIN\\ProcesoCE\\PDF\\" + num_cpe + ".pdf");
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            Process proc = new Process();
            proc.StartInfo = procStartInfo;
            proc.Start();















            //Warning[] warnings;
            //string[] streamIds;
            //string mimeType = string.Empty;
            //string encoding = string.Empty;
            //string extension = string.Empty;

            //byte[] bytes = ReportGR.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            //var bytes = System.Text.Encoding.Unicode.GetBytes(msg);
            //using (System.IO.FileStream fs = new System.IO.FileStream("D:\\pdf_withhtml.pdf", System.IO.FileMode.Create))
            //{
            //    fs.Write(bytes, 0, bytes.Length);
            //}



            //iTextSharp.text.Document document = new iTextSharp.text.Document();

            //iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream("D:\\pdf_withhtml.pdf", FileMode.OpenOrCreate));

            //document.Open();

            //document.Add(new Paragraph(msg));
            //document.Close();


        }








        public static bool XMLToPDF(string pXmlFile, string pXslFile, string pFoFile, string pPdfFile)
        {
            string lBaseDir = System.IO.Path.GetDirectoryName(pXslFile);
            XslCompiledTransform lXslt = new XslCompiledTransform();
            lXslt.Load(pXslFile);
            lXslt.Transform(pXmlFile, pFoFile);
            FileStream lFileInputStreamFo = new FileStream(pFoFile, FileMode.Open);
            FileStream lFileOutputStreamPDF = new FileStream(pPdfFile, FileMode.Create);
            FonetDriver lDriver = FonetDriver.Make();
            lDriver.BaseDirectory = new DirectoryInfo(lBaseDir);
            lDriver.CloseOnExit = true;
            lDriver.Render(lFileInputStreamFo, lFileOutputStreamPDF);
            lFileInputStreamFo.Close();
            lFileOutputStreamPDF.Close();
            return System.IO.File.Exists(pPdfFile);
        }
































        HtmlTextWriter writer;
        public void Renderx(HtmlTextWriter writer)
        {
            pagexslt.Transform(documentoXml, null, writer);
        }



        static void booksSettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                Console.Write("WARNING: ");
                Console.WriteLine(e.Message);
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                Console.Write("ERROR: ");
                Console.WriteLine(e.Message);
            }
        }
    }
}
