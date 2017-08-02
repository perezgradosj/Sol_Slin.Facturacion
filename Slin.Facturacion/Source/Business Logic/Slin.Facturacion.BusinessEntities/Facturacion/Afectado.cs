using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Afectado
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string CodigoTipoDocumento { get; set; }
    }

    [CollectionDataContract]
    public class ListaAfectado : List<Afectado>
    {

    }
}
