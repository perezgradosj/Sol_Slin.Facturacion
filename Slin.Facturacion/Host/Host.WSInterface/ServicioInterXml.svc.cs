using Slin.Facturacion.DataInterfaceXml.CE;
using Slin.Facturacion.DataInterfaceXml.CEN;
using Slin.Facturacion.DataInterfaceXml.CRE;
using Slin.Facturacion.DataInterfaceXml.LOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Host.WSInterface
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServicioInterXml" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServicioInterXml.svc or ServicioInterXml.svc.cs at the Solution Explorer and start debugging.
    public class ServicioInterXml : IServicioInterXml
    {
        //public void DoWork()
        //{
        //}

        public string GetObjInterfaceXML_CE(string xmlLine)
        {
            return new InterfaceXmlCE().GetObjInterfaceXML_CE(xmlLine);
        }

        public string GetObjInterfaceXML(string xmlLine)
        {
            return new InterfaceXmlCRE().GetObjInterfaceXML(xmlLine);
        }

        public string GetObjInterfaceXML_CENOTE(string xmlLine)
        {
            return new InterfaceXmlCENOTS().GetObjInterfaceXML_CENOTE(xmlLine);
        }

        public string SendDocumentVoided(string xmlLine)
        {
            return new DocumentHelper().SendDocumentVoided(xmlLine);
        }
    }
}
