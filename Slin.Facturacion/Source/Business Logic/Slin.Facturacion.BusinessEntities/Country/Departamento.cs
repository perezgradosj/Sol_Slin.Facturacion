using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Departamento
    {
        [DataMember]
        public int IdDepartamento { get; set; }

        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public Pais Pais { get; set; }


        [DataMember]
        public ListaProvincia ListaProvincia { get; set; }

        [DataMember]
        public Provincia Provincia { get; set; }
    }
}
