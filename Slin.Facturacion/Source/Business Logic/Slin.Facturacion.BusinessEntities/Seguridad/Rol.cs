using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Rol
    {
        [DataMember]
        public int IdRol { get; set; }

        [DataMember]
        public string NombreRol { get; set; }

        [DataMember]
        public Estado Estado { get; set; }

        [DataMember]
        public string CodigoRol { get; set; }

        [DataMember]
        public int Padre { get; set; }
    }
}
