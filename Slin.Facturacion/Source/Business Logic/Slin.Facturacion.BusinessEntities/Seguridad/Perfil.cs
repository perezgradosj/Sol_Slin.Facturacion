using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Perfil
    {
        [DataMember]
        public int IdPerfil { get; set; }

        [DataMember]
        public string NombrePerfil { get; set; }

        [DataMember]
        public Estado Estado { get; set; }

        [DataMember]
        public int IdUsuarioPerfil { get; set; }

        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public int IdMenu { get; set; }

        [DataMember]
        public Empresa Empresa { get; set; }

    }
}
