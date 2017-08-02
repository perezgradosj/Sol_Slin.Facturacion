using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class TipoDocumento
    {
        [DataMember]
        public Int32 IdTipoDocumento { get; set; }

        [DataMember]
        public String CodigoDocumento { get; set; }

        [DataMember]
        public String Descripcion { get; set; }

        [DataMember]
        public int Padre { get; set; }

        [DataMember]
        public int Nivel { get; set; }

        [DataMember]
        public Estado Estado { get; set; }

        [DataMember]
        public Empresa Empresa { get; set; }

    }
}
