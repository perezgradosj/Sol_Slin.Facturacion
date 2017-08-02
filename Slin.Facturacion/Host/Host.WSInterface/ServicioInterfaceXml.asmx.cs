using Slin.Facturacion.DataInterfaceXml.CE;
using Slin.Facturacion.DataInterfaceXml.CEN;
using Slin.Facturacion.DataInterfaceXml.CRE;
using Slin.Facturacion.DataInterfaceXml.LOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace Host.WSInterface
{
    /// <summary>
    /// Summary description for ServicioInterfaceXml
    /// </summary>
    [WebService(Namespace = "http://www.slin.com.pe/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ServicioInterfaceXml : System.Web.Services.WebService
    {

        //[SoapHeader ("Authentication", Required=true)] para consumir una web service con usuario y contraseña
        [WebMethod]
        public string GetObjInterfaceXML(string xmlLine)
        {
            return new InterfaceXmlCRE().GetObjInterfaceXML(xmlLine);
        }

        [WebMethod]
        public string GetObjInterfaceXML_CE(string xmlLine)
        {
            return new InterfaceXmlCE().GetObjInterfaceXML_CE(xmlLine);
        }

        [WebMethod]
        public string GetObjInterfaceXML_CENOTE(string xmlLine)
        {
            return new InterfaceXmlCENOTS().GetObjInterfaceXML_CENOTE(xmlLine);
        }

        [WebMethod]
        public string SendDocumentVoided(string xmlLine)
        {
            return new DocumentHelper().SendDocumentVoided(xmlLine);
        }

        //[WebMethod]
        //public string GetObjInterfaceXML_GUIA(string xmlInterface)
        //{
        //    return new InterfaceXmlCRE().GetObjInterfaceXML(xmlInterface);
        //}

        //[WebMethod]
        //public string GetObjInterfaceXML_CPE(string xmlInterface)
        //{
        //    return new InterfaceXmlCRE().GetObjInterfaceXML(xmlInterface);
        //}
    }
}
