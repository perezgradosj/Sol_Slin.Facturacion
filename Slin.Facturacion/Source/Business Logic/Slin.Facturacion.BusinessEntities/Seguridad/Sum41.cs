using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Sum41
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string HName { get; set; }

        [DataMember]
        public string Cod_HD { get; set; }

        [DataMember]
        public string Cod_MB { get; set; }

        [DataMember]
        public string Cod_Ky { get; set; }

        [DataMember]
        public string Cod_Enky { get; set; }
    }
}
