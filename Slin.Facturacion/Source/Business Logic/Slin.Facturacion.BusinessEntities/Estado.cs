using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Estado
    {
        [DataMember]
        public Int32 IdEstado { get; set; }

        [DataMember]
        public String Descripcion { get; set; }

        [DataMember]
        public String RutaImagen { get; set; }

        [DataMember]
        public int IdEstadoSUNAT { get; set; }
    }
}
