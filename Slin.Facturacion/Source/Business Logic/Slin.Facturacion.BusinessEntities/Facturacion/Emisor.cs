using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Emisor
    {
        [DataMember]
        public String NombreEmisor { get; set; }

        [DataMember]
        public String NombreComercio { get; set; }

        [DataMember]
        public String Ubigeo { get; set; }

        [DataMember]
        public String DomicilioFiscal { get; set; }

        [DataMember]
        public String Urbanizacion { get; set; }

        [DataMember]
        public String Provincia { get; set; }

        [DataMember]
        public String Departamento { get; set; }

        [DataMember]
        public String Distrito { get; set; }

        [DataMember]
        public String Pais { get; set; }

        [DataMember]
        public TipoDocumentoIdentidad TipoDocumentoIdentidad { get; set; }

        [DataMember]
        public String NumeroDocumentoIdentidad { get; set; }
    }
}
