using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Empresa
    {
        [DataMember]
        public int IdEmpresa { get; set; }

        [DataMember]
        public string CodEmpresa { get; set; }

        [DataMember]
        public Ubigeo Ubigeo { get; set; }

        [DataMember]
        public string CodigoUbigeo { get; set; }

        [DataMember]
        public string RUC { get; set; }

        [DataMember]
        public string RazonSocial { get; set; }

        [DataMember]
        public string RazonComercial { get; set; }

        [DataMember]
        public string Telefono { get; set; }

        [DataMember]
        public string Fax { get; set; }

        [DataMember]
        public string Direccion { get; set; }

        [DataMember]
        public string DomicilioFiscal { get; set; }

        [DataMember]
        public string Urbanizacion { get; set; }

        [DataMember]
        public DateTime FechaRegistro { get; set; }

        [DataMember]
        public Estado Estado { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string PaginaWeb { get; set; }

        [DataMember]
        public TipoDocumentoIdentidad TipoDocumentiIdentidad { get; set; }

        [DataMember]
        public int Nro { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Distrito { get; set; }

        [DataMember]
        public string Provincia { get; set; }

        [DataMember]
        public string Departamento { get; set; }

        [DataMember]
        public string CodPais { get; set; }

        [DataMember]
        public string Dominio { get; set; }

        [DataMember]
        public string IP { get; set; }

        [DataMember]
        public int Port { get; set; }

        [DataMember]
        public string TipoLogin { get; set; }

        [DataMember]
        public int UseSSL { get; set; }

        [DataMember]
        public string Url_CompanyConsult { get; set; }
        [DataMember]
        public string Url_CompanyLogo { get; set; }
        [DataMember]
        public int IdGrp { get; set; }
        [DataMember]
        public string Group { get; set; }
    }
}
