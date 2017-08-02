using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Ubigeo
    {
        [DataMember]
        public int IdUbigeo { get; set; }

        [DataMember]
        public string CodigoUbigeo { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public Distrito Distrito { get; set; }
    }
}
