using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using TestWebService.ServiceController;

namespace TestWebService
{
    public partial class TestForm_CE : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnConsume_Click(object sender, EventArgs e)
        {
            //string msje = new ServiceInterfaceController().GetObjInterfaceXML(txtxml.Value);
            //string msje = new ServiceInterfaceController().GetObjInterfaceXML_CE(txtxml.Value);
            //string msje = new ServiceInterfaceController().GetObjInterfaceXML_CENOTE(txtxml.Value);
            //string msje = new ServiceInterfaceController().SendDocumentVoided(txtxml.Value);

            //string msje = new ServiceInterfaceController().GetStatusDocument(txtxml.Value);






            




            var msje = REST(txtxml.Value);



            lblreturn.InnerText = msje;
            //Response.Write("<script language=javascript>alert('" + msje + "');</script>");
            //PDF();
            //Response.Write("<script language=javascript>alert('" + "OK" + "');</script>");
            //XML();
            //Response.Write("<script language=javascript>alert('" + "OK" + "');</script>");
            //CallWebService();
            //Response.Write("<script language=javascript>alert('" + msje + "');</script>");


        }

        private void PDF()
        {
            string num_cpe = txtxml.Value.TrimEnd();

            string result = new ServiceInterfaceController().GetDocumentoPDF(num_cpe);

            byte[] res = Convert.FromBase64String(result);
            string respuesta = Encoding.GetEncoding("iso-8859-1").GetString(res);


            if (System.IO.File.Exists(@"J:\Host_ws\pdf\" + num_cpe + ".pdf"))
                System.IO.File.Delete(@"J:\Host_ws\pdf\" + num_cpe + ".pdf");

            System.IO.FileStream stream = new FileStream(@"J:\Host_ws\pdf\" + num_cpe + ".pdf", FileMode.CreateNew);
            System.IO.BinaryWriter writer =
                new BinaryWriter(stream);
            writer.Write(res, 0, res.Length);
            writer.Close();
        }

        private void XML()
        {
            string num_cpe = txtxml.Value.TrimEnd();

            string result = new ServiceInterfaceController().GetDocumentoXML(num_cpe);

            //byte[] res = Encoding.GetEncoding("").GetBytes(result);
            byte[] res = Convert.FromBase64String(result);

            string respuesta = Encoding.GetEncoding("iso-8859-1").GetString(res);

            var xmldoc = new XmlDocument();
            xmldoc.InnerXml = respuesta;
            xmldoc.Save(@"J:\Host_ws\xml\" + num_cpe + ".xml");
        }


        #region 

        public void CallWebService()
        {
            var _url = "http://localhost:61056/ServicioInterfaceXml.asmx";
            var _action = "http://localhost:61056/ServicioInterfaceXml.asmx?op=GetObjInterfaceXML_CE";

            XmlDocument soapEnvelopeXml = CreateSoapEnvelope();
            HttpWebRequest webRequest = CreateWebRequest(_url, _action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
                Console.Write(soapResult);
            }
            Response.Write("<script language=javascript>alert('" + soapResult + "');</script>");
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private XmlDocument CreateSoapEnvelope()
        {
            string value = txtxml.Value.Replace("<![CDATA[", "");
            value = value.Replace("]]>", "");
            XmlDocument soapEnvelop = new XmlDocument();
            soapEnvelop.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?><soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Body><GetObjInterfaceXML_CE xmlns=""http://www.slin.com.pe/""><xmlLine>"+ value +"</xmlLine></GetObjInterfaceXML_CE></soap:Body></soap:Envelope>");
            return soapEnvelop;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

        #endregion









        #region rest


        private string REST(string num_cpe)
        {
            string res = string.Empty;
            //var url = "http://localhost:54403/WServiceGetDocument.asmx/GetStatusDocument/" + num_cpe;
            //var webrequest = (HttpWebRequest)System.Net.WebRequest.Create(url);

            //using (var response = webrequest.GetResponse())
            //using (var reader = new StreamReader(response.GetResponseStream()))
            //{
            //    var result = reader.ReadToEnd();
            //    res = Convert.ToString(result);
            //}

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:54403/WServiceGetDocument.asmx/getnumcpe?num=" + num_cpe);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return "Error: " + response.StatusCode + ", " + response.StatusDescription;
                }

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            return res;
        }

        private string SOAP(string num_cpe)
        {
            //var poststring = new { clave1:valor1, clave2:valor2 };
            //byte[] data = UTF8Encoding.UTF8.GetBytes(poststring);

            //HttpWebRequest request;
            //request = WebRequest.Create("https://www.sunat.gob.pe/ol-it-wsconscpegem/billConsultService?wsdl") as HttpWebRequest;

            string soap =
            @"<?xml version=""1.0""encoding=""utf-8""?>
            <soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
            xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
            xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
            <soap12:Body>
              <GetStatusDocumentResponse xmlns=""http://www.slin.com.pe/"">
                 <GetStatusDocumentResult>" + num_cpe + "</GetStatusDocumentResult>"
               +"</GetStatusDocumentResponse>"
             +"</soap12:Body>"
            +"</soap12:Envelope>";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://localhost:54403/WServiceGetDocument.asmx");
            req.Headers.Add("SOAPAction"+ "http://localhost:54403/WServiceGetDocument.asmx/GetStatusDocument");
            req.ContentType = "text/xml;charset=\"utf-8\"";
            req.Accept = "text/xml";
            req.Method = "POST";

            using (Stream stm = req.GetRequestStream())
            {
                using (StreamWriter stmw = new StreamWriter(stm))
                {
                    stmw.Write(soap);
                }
            }

            //var response = (HttpWebRequest)req.GetResponse();
            var response = (HttpWebResponse)req.GetResponse();

            Stream responseStream = response.GetResponseStream();

            return "";

        }


        #endregion
















    }
}