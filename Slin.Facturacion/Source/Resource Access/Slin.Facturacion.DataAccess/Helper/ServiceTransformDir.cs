using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using XML = Slin.Facturacion.Common.Helper;

namespace Slin.Facturacion.DataAccess.Helper
{
    public class ServiceTransformDir
    {
        public string PathDirectory = ConfigurationManager.ConnectionStrings["PathDirectory"].ToString();
        public string PathXml = ConfigurationManager.ConnectionStrings["PathXml"].ToString();


        StreamReader sr;


        public XML.Document GetListDocumentConsult(string num_cpe)
        {
            XML.Document objDoc = new XML.Document();

            XmlSerializer xmlSerial = new XmlSerializer(typeof(XML.DocumenType));
            sr = new StreamReader(PathDirectory + ".xml");
            XML.DocumenType doct = (XML.DocumenType)xmlSerial.Deserialize(sr);

            foreach (var doc in doct.Documents.ListDocumento)
            {
                if (doc.Num_CPE == num_cpe)
                {
                    objDoc = new XML.Document();
                    objDoc.IdEstado = doc.IdEstado;
                    objDoc.Descripcion = doc.Descripcion;
                    objDoc.Num_CPE = doc.Num_CPE;
                    objDoc.Tpo_CPE = doc.Tpo_CPE;
                    break;
                }
            }
            return objDoc;
        }

        //public XML.Documents GetListDocumentAccept()
        //{
        //    XML.Document objDoc = new XML.Document();
        //    XML.Documents objListDoc = new XML.Documents();

        //    XmlSerializer xmlSerial = new XmlSerializer(typeof(XML.DocumenType));
        //    sr = new StreamReader(PathDirectory + ".xml");
        //    XML.DocumenType doct = (XML.DocumenType)xmlSerial.Deserialize(sr);

        //    foreach (var doc in doct.Documents.ListDocumento)
        //    {
        //        if (doc.IdEstado == 7.ToString())
        //        {
        //            objDoc = new XML.Document();
        //            objDoc.IdEstado = doc.IdEstado;
        //            objDoc.Descripcion = doc.Descripcion;
        //            objDoc.Num_CPE = doc.Num_CPE;

        //            objDoc.Extra = doc.IdEstado;

        //            objListDoc.ListDocumento.Add(objDoc);
        //        }
        //    }
        //    return objListDoc;
        //}



        public string GetXmlFromDir(string num_cpe)
        {
            var xmldoc = new XmlDocument();
            xmldoc.Load(PathXml + num_cpe +".xml");

            string response = xmldoc.InnerXml;

            return response;
        }
    }
}
