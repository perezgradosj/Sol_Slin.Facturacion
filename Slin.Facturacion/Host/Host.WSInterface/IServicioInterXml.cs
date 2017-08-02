using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Host.WSInterface
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServicioInterXml" in both code and config file together.
    [ServiceContract]
    public interface IServicioInterXml
    {
        //[OperationContract]
        //void DoWork();

        [OperationContract]
        string GetObjInterfaceXML(string xmlLine);

        [OperationContract]
        string GetObjInterfaceXML_CE(string xmlLine);

        [OperationContract]
        string GetObjInterfaceXML_CENOTE(string xmlLine);

        [OperationContract]
        string SendDocumentVoided(string xmlLine);
    }
}
