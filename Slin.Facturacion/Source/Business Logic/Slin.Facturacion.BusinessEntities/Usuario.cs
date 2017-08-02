using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Usuario
    {
        [DataMember]
        public Int32 IdUsuario { get; set; }

        [DataMember]
        public String CodUsuario { get; set; }

        [DataMember]
        public Empleado Empleado { get; set; }

        [DataMember]
        public String Username { get; set; }

        [DataMember]
        public String Password { get; set; }

        [DataMember]
        public Estado Estado { get; set; }

        [DataMember]
        public Empresa Empresa { get; set; }

        [DataMember]
        public DateTime FechaRegistro { get; set; }

        [DataMember]
        public string CodEmpleado { get; set; }

        [DataMember]
        public int Nro { get; set; }
        //public string NombreApellidos { get; set; }


        [DataMember]
        public Perfil Perfil { get; set; }

        [DataMember]
        public Rol Rol { get; set; }

        [DataMember]
        public DateTime FechaExpiracion { get; set; }


        [DataMember]
        public String NuevoPassword { get; set; }

        [DataMember]
        public string DNI { get; set; }

        [DataMember]
        public string NombresApellidos { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public string NombrePerfil { get; set; }



        [DataMember]
        public Sede Sede { get; set; }

        [DataMember]
        public int IdSede { get; set; }

        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class Authenticate
    {
        [DataMember]
        public int IdUseAuthenticate { get; set; }
        [DataMember]
        public string Desc { get; set; }
    }

    [CollectionDataContract]
    public class ListAuthenticate : List<Authenticate>
    {

    }
}
