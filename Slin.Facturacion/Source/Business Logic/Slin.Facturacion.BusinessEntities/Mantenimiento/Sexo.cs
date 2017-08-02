using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WcfSerialization = System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [WcfSerialization::DataContract(Namespace = "http://www.slin.com.pe", Name = "Sexo")]
    public class Sexo
    {
        [WcfSerialization::DataMember(Name = "IdSexo", IsRequired = false, Order = 0)]
        public Int32 IdSexo { get; set; }

        [WcfSerialization::DataMember(Name = "Codigo", IsRequired = false, Order = 0)]
        public String Codigo { get; set; }

        [WcfSerialization::DataMember(Name = "Descripcion", IsRequired = false, Order = 0)]
        public String Descripcion { get; set; }
    }
}
