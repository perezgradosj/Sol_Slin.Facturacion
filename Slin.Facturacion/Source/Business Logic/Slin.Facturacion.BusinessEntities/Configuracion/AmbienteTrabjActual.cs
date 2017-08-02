using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class AmbienteTrabjActual
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string COD { get; set; }

        [DataMember]
        public string DESCRIPCION { get; set; }

        [DataMember]
        public int IDAmbiente { get; set; }

        [DataMember]
        public Empresa Empresa { get; set; }

    }
}
