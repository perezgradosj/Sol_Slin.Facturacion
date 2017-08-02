using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class ObjFacturaElectronico
    {
        [DataMember]
        public TipoDocumento TipoDocumento { get; set; }

        [DataMember]
        public String NumeroDocumento { get; set; }

        [DataMember]
        public DateTime FechaEmision { get; set; }

        [DataMember]
        public Emisor Emisor { get; set; }

        [DataMember]
        public Serie Serie { get; set; }

        [DataMember]
        public Int32 Nro { get; set; }

        [DataMember]
        public Cliente Cliente { get; set; }

        [DataMember]
        public Estado Estado { get; set; }

        [DataMember]
        public Decimal MontoTotal { get; set; }

        [DataMember]
        public string RutaImagen { get; set; }

        [DataMember]
        public Empresa Empresa { get; set; }

        [DataMember]
        public Moneda Moneda { get; set; }

        [DataMember]
        public EstadoEnvio EstadoEnvio { get; set; }
    }
}
