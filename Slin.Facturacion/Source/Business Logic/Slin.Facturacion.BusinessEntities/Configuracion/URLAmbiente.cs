using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class URLAmbiente
    {
        [DataMember]
        public int IdUrl { get; set; }

        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public string URL { get; set; }

        [DataMember]
        public Estado Estado { get; set; }

        [DataMember]
        public AmbienteSunat AmbienteSunat { get; set; }
    }
}
