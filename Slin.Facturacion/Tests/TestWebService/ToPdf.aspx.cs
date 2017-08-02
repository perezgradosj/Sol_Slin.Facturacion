using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.XPath;
using System.Xml.Xsl;
using iTextSharp.text.html.simpleparser;

namespace TestWebService
{
    public partial class ToPdf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            documentoXml = new XPathDocument(Server.MapPath("20508622326-01-F001-00000334.xml"));
            pagexslt = new XslTransform();
            pagexslt.Load(Server.MapPath("FEE.xslt"));
        }

        XPathDocument documentoXml;
        XslTransform pagexslt;

        protected override void Render(HtmlTextWriter writer)
        {
            pagexslt.Transform(documentoXml, null, writer);
        }

        protected void btnConsume_Click(object sender, EventArgs e)
        {
            
        }


        private void GeneratePdf()
        {
            

            //Document document = new Document(PageSize.A4, 80, 50, 30, 65);
            //FileStream stream = new FileStream(Server.MapPath("result.pdf"), FileMode.Create);
            //PdfWriter PDFWriter = PdfWriter.GetInstance(document, stream);
            ////HtmlParser.Parse(document, Server.MapPath("origin.html"));
            //stream.Close();
            //PDFWriter.Close();



            //iTextSharp.text.pdf.PdfDocument opdf = new PdfDocument();





            MemoryStream msOutput = new MemoryStream();
            TextReader reader = new StringReader("20508622326-01-F001-00000334.xml");

            // step 1: creation of a document-object
            Document document = new Document(PageSize.A4, 30, 30, 30, 30);

            // step 2:
            // we create a writer that listens to the document
            // and directs a XML-stream to a file
            PdfWriter writer = PdfWriter.GetInstance(document, msOutput);

            // step 3: we create a worker parse the document
            HTMLWorker worker = new HTMLWorker(document);

            // step 4: we open document and start the worker on the document
            document.Open();
            worker.StartDocument();

            // step 5: parse the html into the document
            worker.Parse(reader);

            // step 6: close the document and the worker
            worker.EndDocument();
            worker.Close();
            document.Close();

            //return msOutput;
        }
    }
}