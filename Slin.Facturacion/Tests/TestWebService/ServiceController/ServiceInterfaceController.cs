using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using TestWebService.ServiceReferenceXml;

//using TestWebService.ServiceReferencePWC;

namespace TestWebService.ServiceController
{
    public class ServiceInterfaceController
    {
        public string GetObjInterfaceXML_CE(string xmlInterface)
        {
            using (ServicioInterfaceXmlSoapClient client = new ServicioInterfaceXmlSoapClient())
            {
                return client.GetObjInterfaceXML_CE(xmlInterface);
            }

        }

        public string GetObjInterfaceXML(string xmlLine)
        {
            using (ServicioInterfaceXmlSoapClient client = new ServicioInterfaceXmlSoapClient())
            {
                return client.GetObjInterfaceXML(xmlLine);
            }
        }

        public string GetObjInterfaceXML_CENOTE(string xmlLine)
        {
            using (ServicioInterfaceXmlSoapClient client = new ServicioInterfaceXmlSoapClient())
            {
                return client.GetObjInterfaceXML_CENOTE(xmlLine);
            }
        }

        public string SendDocumentVoided(string xmlLine)
        {
            using (ServicioInterfaceXmlSoapClient client = new ServicioInterfaceXmlSoapClient())
            {
                return client.SendDocumentVoided(xmlLine);
            }
        }






        public string GetDocumentoPDF(string NUM_CE)
        {

            using (ServiceReferencePWC.WServiceGetDocumentSoapClient client = new ServiceReferencePWC.WServiceGetDocumentSoapClient())
            {
                return client.GetDocumentoPDF(NUM_CE);
            }
        }

        public string GetDocumentoXML(string NUM_CE)
        {
            using (ServiceReferencePWC.WServiceGetDocumentSoapClient client = new ServiceReferencePWC.WServiceGetDocumentSoapClient())
            {
                return client.GetDocumentoXML(NUM_CE);
            }
        }

        public string GetStatusDocument(string NUM_CE)
        {
            using (ServiceReferencePWC.WServiceGetDocumentSoapClient client = new ServiceReferencePWC.WServiceGetDocumentSoapClient())
            {
                return client.GetStatusDocument(NUM_CE);
            }
        }

        //public string GetStatusDocument(string NUM_CE)
        //{
        //    using (ServicioInterfaceXmlSoapClient client = new ServicioInterfaceXmlSoapClient())
        //    {
        //        return client.GetStatusDocument(NUM_CE);
        //    }
        //}
    }
}