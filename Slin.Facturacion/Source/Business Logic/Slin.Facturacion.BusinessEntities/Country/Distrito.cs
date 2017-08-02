using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Distrito
    {
        [DataMember]
        public int IdDistrito { get; set; }

        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public string CodigoUbigeo { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public Provincia Provincia { get; set; }
    }
}
