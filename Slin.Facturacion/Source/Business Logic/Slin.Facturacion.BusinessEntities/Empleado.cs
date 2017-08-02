using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Empleado
    {
        [DataMember]
        public int IdEmpleado { get; set; }

        [DataMember]
        public string CodEmpleado { get; set; }

        [DataMember]
        public string Nombres { get; set; }

        [DataMember]
        public string ApePaterno { get; set; }

        [DataMember]
        public string ApeMaterno { get; set; }

        [DataMember]
        public string DNI { get; set; }

        [DataMember]
        public Sexo Sexo { get; set; }

        [DataMember]
        public string Direccion { get; set; }

        [DataMember]
        public string Telefono { get; set; }

        [DataMember]
        public string Celular { get; set; }

        [DataMember]
        public DateTime FechaRegistro { get; set; }

        [DataMember]
        public DateTime FechaNacimiento { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public Estado Estado { get; set; }

        [DataMember]
        public string NombresApellidos { get; set; }

        [DataMember]
        public int Nro { get; set; }

        [DataMember]
        public Empresa Empresa { get; set; }

        [DataMember]
        public TipoDocumentoIdentidad TipoDocumentoIdentidad { get; set; }

    }
}
