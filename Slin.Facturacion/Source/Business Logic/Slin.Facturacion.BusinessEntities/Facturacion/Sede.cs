using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Sede
    {
        [DataMember]
        public int IdSede { get; set; }

        [DataMember]
        public string Cod { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Empresa Empresa { get; set; }
    }

    [CollectionDataContract]
    public class ListaSede : List<Sede>
    {

    }
}
