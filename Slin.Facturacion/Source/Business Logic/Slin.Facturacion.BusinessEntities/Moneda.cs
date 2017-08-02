using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Moneda
    {
        [DataMember]
        public Int32 IdMoneda { get; set; }

        [DataMember]
        public String Simbolo { get; set; }

        [DataMember]
        public String Descripcion { get; set; }
    }
}
