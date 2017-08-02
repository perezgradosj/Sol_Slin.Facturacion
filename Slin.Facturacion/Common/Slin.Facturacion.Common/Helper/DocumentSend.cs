using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
namespace Slin.Facturacion.Common.Helper
{
    [XmlRoot(ElementName = "DocumentSend")]
    public class DocumentSend
    {
        [XmlElement(ElementName = "ID_CE")]
        public string ID_CE { get; set; }
        [XmlElement(ElementName = "Tipo_CE")]
        public string Tipo_CE { get; set; }
        [XmlElement(ElementName = "Estado_Doc")]
        public string Estado_Doc { get; set; }
        [XmlElement(ElementName = "RucEmisor")]
        public string RucEmisor { get; set; }
        [XmlElement(ElementName = "TypeFormat")]
        public string TypeFormat { get; set; }
    }
}
