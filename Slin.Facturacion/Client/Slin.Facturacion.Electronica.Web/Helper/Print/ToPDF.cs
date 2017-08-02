using Slin.Facturacion.Proxies.ServicioFacturacion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace Slin.Facturacion.Electronica.Web.Helper.Print
{
    [Serializable()]
    public class ToPDF
    {
        public List<DetalleFacturaElectronica> ListaDetalle { get; set; }

        public ToPDF()
        {
            ListaDetalle = new List<DetalleFacturaElectronica>();
        }


        public void TOPDF_FROM_XML()
        {
            DetalleFacturaElectronica odetalle = new DetalleFacturaElectronica();

            XmlSerializer x = new XmlSerializer(odetalle.GetType());
            System.IO.StringWriter writer = new System.IO.StringWriter();
            x.Serialize(writer, odetalle);
            ConvierteFOP(writer);
        }


        private void ConvierteFOP(System.IO.StringWriter Xmlaux)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Xmlaux.ToString());
            string XSLFO = null;
            dynamic xslt = new XslCompiledTransform();
            xslt.Load("PlantillaAgenda.xsl");// Aqui va el path de la plantilla
            using (System.IO.StringWriter tmp = new System.IO.StringWriter())
            {
                using (XmlTextWriter res = new XmlTextWriter(tmp))
                {
                    xslt.Transform(xmlDoc, null, res);
                    XSLFO = tmp.ToString();
                }
            }
            //NOTA: La generacion de PDF en depuracion tarda bastante, en ejecucion es practicamente inmediata
            //GeneraPDF(XSLFO);
        }

        //public void GeneraPDF(string XSLFO)
        //{
        //    FopFactory fopFactory = new FopFactory.newInstance();
        //    OutPutStream salida = new BufferedOutPutStream(new FileOutPutStream(new File("Ejemplo.pdf")));
        //    try
        //    {
        //        Fop fop = fopFactory.newFop("application/pdf", salida);
        //        TransformerFactory factory = TransformerFactory.newInstance();
        //        Transformer transformer = factory.newTransformer();
        //        Source src = new StreamSource(new java.io.StringReader(XSLFO));
        //        Result res = SAXResult(fop.getDefaultHandler());
        //        transformer.transform(src, res);
        //    }
        //    finally
        //    {
        //        salida.close();
        //    }
        //}


        //private void GeneraPDF(string XSLFO)
        //{
        //    FopFactory fopFactory = FopFactory.newInstance();
        //    OutputStream salida = new BufferedOutputStream(new FileOutputStream(new File("EjemploAgenda1.pdf")));
        //    try
        //    {
        //        Fop fop = fopFactory.newFop("application/pdf", salida);
        //        TransformerFactory factory = TransformerFactory.newInstance();
        //        Transformer transformer = factory.newTransformer(); // Identificador
        //        Source src = new StreamSource(new java.io.StringReader(XSLFO));
        //        Result res = new SAXResult(fop.getDefaultHandler());
        //        transformer.transform(src, res);
        //    }
        //    finally
        //    {
        //        salida.close(); //Cerramos la salida.
        //    }
        //}
    }
}