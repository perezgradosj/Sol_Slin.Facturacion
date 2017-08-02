using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class SistemaEstado
    {
        [DataMember]
        public string Criterio { get; set; }

        [DataMember]
        public int Ayer { get; set; }

        [DataMember]
        public int UltimaSemana { get; set; }

        [DataMember]
        public int TotalMes { get; set; }

        [DataMember]
        public int MesPasado { get; set; }

        [DataMember]
        public int MesActual { get; set; }

        [DataMember]
        public int Diferencia { get; set; }

        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public int Hoy { get; set; }

        [DataMember]
        public int SemanaActual { get; set; }
    }
}
