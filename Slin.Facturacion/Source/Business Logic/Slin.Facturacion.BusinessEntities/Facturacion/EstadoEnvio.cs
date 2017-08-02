using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class EstadoEnvio
    {
        [DataMember]
        public int IdEstadoEnvio { get; set; }

        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public TipoDocumento TipoDocumento { get; set; }

        [DataMember]
        public Empresa Empresa { get; set; }
    }


    [DataContract]
    public class EstadoPrint
    {
        [DataMember]
        public int IdEstadoPrint { get; set; }

        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public TipoDocumento TipoDocumento { get; set; }

        [DataMember]
        public Empresa Empresa { get; set; }
    }

    [CollectionDataContract]
    public class ListaEstadoPrint : List<EstadoPrint>
    {

    }
}
