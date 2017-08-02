using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Slin.Facturacion.DataInterfaceXml.Serialize
{
    [XmlRoot(ElementName="DocumentVoided")]
	public class DocumentVoided 
    {
		[XmlElement(ElementName="ID")]
		public string ID { get; set; }
		[XmlElement(ElementName="TipoCE")]
		public string TipoCE { get; set; }
		[XmlElement(ElementName="EmiNumDoc")]
		public string EmiNumDoc { get; set; }
        [XmlElement(ElementName = "Serie")]
        public string Serie { get; set; }
        [XmlElement(ElementName = "Correlativo")]
        public string Correlativo { get; set; }
		[XmlElement(ElementName="Motivo")]
		public string Motivo { get; set; }
	}

}
