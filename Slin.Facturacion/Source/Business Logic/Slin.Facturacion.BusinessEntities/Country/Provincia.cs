using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Provincia
    {
        [DataMember]
        public int IdProvincia { get; set; }

        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public Departamento Departamento { get; set; }


        [DataMember]
        public ListaDistrito ListaDistrito { get; set; }

        [DataMember]
        public Distrito Distrito { get; set; }
    }
}
