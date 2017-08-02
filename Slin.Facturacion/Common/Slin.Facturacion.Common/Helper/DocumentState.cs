using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace Slin.Facturacion.Common.Helper
{
    [XmlRoot("DocumentState")]
    public class DocumentState
    {
        [XmlElement("ID_CE")]
        public string ID_CE { get; set; }

        [XmlElement("Tipo_CE")]
        public string Tipo_CE { get; set; }

        [XmlElement("Estado_Doc")]
        public string Estado_Doc { get; set; }

        [XmlElement("Estado_Desc")]
        public string Estado_Desc { get; set; }

        [XmlElement("RucEmisor")]
        public string RucEmisor { get; set; }

        [XmlElement("MontoLiteral")]
        public string MontoLiteral { get; set; }

        [XmlElement("PrintName")]
        public string PrintName { get; set; }

        [XmlElement("ImpNetPagSoles")]
        public string ImpNetPagSoles { get; set; }

        [XmlElement("Copies")]
        public string Copies { get; set; }

        [XmlElement("TypeFormat")]
        public string TypeFormat { get; set; }
    }
}
