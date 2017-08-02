using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Serie
    {
        [DataMember]
        public int IdSerie { get; set; }

        [DataMember]
        public string CodigoSerie { get; set; }

        [DataMember]
        public TipoDocumento TipoDocumento { get; set;  }

        [DataMember]
        public string DescripcionSerie { get; set; }

        [DataMember]
        public string NumeroSerie { get; set; }

        [DataMember]
        public Empresa Empresa { get; set; }
    }
}
