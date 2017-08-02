using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Cliente
    {
        [DataMember]
        public Int32 IdCliente { get; set; }

        [DataMember]
        public String CodigoCliente { get; set; }

        [DataMember]
        public String Nombres { get; set; }

        [DataMember]
        public String Apellidos { get; set; }

        [DataMember]
        public TipoDocumentoIdentidad TipoDocumentoIdentidad { get; set; }

        [DataMember]
        public String NumeroDocumentoIdentidad { get; set; }

        [DataMember]
        public String ClienteRuc { get; set; }

        [DataMember]
        public String Direccion { get; set; }

        [DataMember]
        public string RazonSocial { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public Estado Estado { get; set; }

        [DataMember]
        public Empresa Empresa { get; set; }

        [DataMember]
        public string NroDocumento { get; set; }

        [DataMember]
        public string Telefono { get; set; }

        [DataMember]
        public string CodUbigeo { get; set; }

        [DataMember]
        public Email EmailClient { get; set; }

        [DataMember]
        public string Distrito { get; set; }

        [DataMember]
        public string Provincia { get; set; }

        [DataMember]
        public string Departamento { get; set; }

        [DataMember]
        public string CodPais { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public string Urbanizacion { get; set; }
    }
}
