using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace Slin.Facturacion.Common.Helper
{
    [XmlRoot("DocumenType")]
    public class DocumenType
    {
        [XmlElement("Document")]
        public Document Document { get; set; }

        [XmlElement("Documents")]
        public Documents Documents { get; set; }
    }


    [XmlRoot("Document")]
    public class Document
    {
        [XmlElement("Num_CPE")]
        public string Num_CPE { get; set; }

        [XmlElement("IdEstado")]
        public string IdEstado { get; set; }

        [XmlElement("Descripcion")]
        public string Descripcion { get; set; }

        [XmlElement("Tpo_CPE")]
        public string Tpo_CPE { get; set; }
    }

    public class Documents
    {
        [XmlElement("Document")]
        public List<Document> ListDocumento = new List<Document>();
    }

}
