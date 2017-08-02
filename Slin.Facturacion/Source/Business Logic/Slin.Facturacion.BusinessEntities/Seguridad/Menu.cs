using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Menu
    {
        [DataMember]
        public int IdMenu { get; set; }

        [DataMember]
        public string NombreMenu { get; set; }

        [DataMember]
        public string CodigoMenu { get; set; }

        [DataMember]
        public Estado Estado { get; set; }

        [DataMember]
        public Perfil Perfil { get; set; }

        [DataMember]
        public Usuario Usuario { get; set; }

        [DataMember]
        public int PadreMenu { get; set; }

        [DataMember]
        public int NivelMenu { get; set; }

        [DataMember]
        public Empresa Empresa { get; set; }
    }
}
