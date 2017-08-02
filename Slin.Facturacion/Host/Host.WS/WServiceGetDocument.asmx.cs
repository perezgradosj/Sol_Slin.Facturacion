using Slin.Facturacion.ServiceImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace Host.WS
{
    /// <summary>
    /// Summary description for WServiceGetDocument
    /// </summary>
    [WebService(Namespace = "http://www.slin.com.pe/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    //[System.Web.Script.Services.ScriptService]
    public class WServiceGetDocument : System.Web.Services.WebService
    {
        ServicioConsultaSOA objMethod = new ServicioConsultaSOA();

        [WebMethod]
        public string GetDocumentoPDF(string NUM_CPE)
        {
            return objMethod.GetDocumentoPDF(NUM_CPE);
        }

        [WebMethod]
        public string GetDocumentoXML(string NUM_CPE)
        {
            return objMethod.GetDocumentoXML(NUM_CPE);
        }

        [WebMethod]
        public string GetStatusDocument(string NUM_CE)
        {
            return objMethod.GetStatusDocument(NUM_CE);
        }

        [WebMethod]
        public string getnumcpe(string num)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize("Enviaste: " + num);
        }
    }
}
