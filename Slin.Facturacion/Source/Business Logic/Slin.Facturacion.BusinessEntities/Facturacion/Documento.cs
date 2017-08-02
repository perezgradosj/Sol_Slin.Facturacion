using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Documento
    {
        [DataMember]
        public int Nro { get; set; }

        [DataMember]
        public string CodigoTipoDocumento { get; set; }

        [DataMember]
        public string TipoDocumentoDesc { get; set; }

        [DataMember]
        public string Serie { get; set; }

        [DataMember]
        public string NumeroDocumento { get; set; }

        [DataMember]
        public string NumeroFactura { get; set; }

        [DataMember]
        public DateTime FechaEmision { get; set; }

        [DataMember]
        public string NumDocCliente { get; set; }

        [DataMember]
        public string Cliente { get; set; }

        [DataMember]
        public decimal MontoTotal { get; set; }

        [DataMember]
        public string Estado { get; set; }

        [DataMember]
        public string Moneda { get; set; }

        //[DataMember]
        //public string FechaAnulado { get; set; }

        [DataMember]
        public DateTime FechaAnulado { get; set; }


        [DataMember]
        public string FechaDesde { get; set; }

        [DataMember]
        public string FechaHasta { get; set; }

        [DataMember]
        public TipoDocumento TipoDocumento { get; set; }

        [DataMember]
        public string Asunto { get; set; }

        [DataMember]
        public string Destino { get; set; }

        [DataMember]
        public string Remitente { get; set; }

        [DataMember]
        public string FechaEnvio { get; set; }

        [DataMember]
        public DateTime FechaEnviado { get; set; }


        [DataMember]
        public string Mensaje { get; set; }

        [DataMember]
        public string Referente { get; set; }

        [DataMember]
        public string NombreArchivo { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public Empresa Empresa { get; set; }

        [DataMember]
        public DateTime Fecha_Cad { get; set; }

        [DataMember]
        public decimal Total { get; set; }

        //[DataMember]
        //public string MotivoAnulado { get; set; }
    }
}
