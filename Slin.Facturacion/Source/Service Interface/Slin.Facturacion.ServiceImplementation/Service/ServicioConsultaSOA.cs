using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slin.Facturacion.DataAccess;

namespace Slin.Facturacion.ServiceImplementation
{
    public class ServicioConsultaSOA
    {
        //private static readonly ServicioConsultaSOA instance = new ServicioConsultaSOA();
        //static ServicioConsultaSOA() { }
        //private ServicioConsultaSOA() { }
        //public static ServicioConsultaSOA Instance { get { return instance; } }


        ServiceConsultaDataAccess objServiceConsultaDataAccess = new ServiceConsultaDataAccess();

        public string GetDocumentoXML(string NUM_CPE)
        {
            return objServiceConsultaDataAccess.GetDocumentoXML(NUM_CPE);
        }

        public string GetDocumentoPDF(string NUM_CPE)
        {
            return objServiceConsultaDataAccess.GetDocumentoPDF(NUM_CPE);
        }


        public string GetDocumentoXMLDir(string NUM_CPE)
        {
            return objServiceConsultaDataAccess.GetDocumentoXMLDir(NUM_CPE);
        }

        public string GetDocumentoPDFDir(string NUM_CPE)
        {
            return objServiceConsultaDataAccess.GetDocumentoPDFDir(NUM_CPE);
        }


        public string GetStatusDocument(string NUM_CE)
        {
            return objServiceConsultaDataAccess.GetStatusDocument(NUM_CE);
        }
    }
}
