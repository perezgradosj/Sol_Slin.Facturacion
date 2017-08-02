using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class ExchangeRate
    {
        [DataMember]
        public string Ruc { get; set; }
        [DataMember]
        public DateTime Fecha { get; set; }
        [DataMember]
        public string Moneda { get; set; }
        [DataMember]
        public decimal Value { get; set; }

        [DataMember]
        public DateTime fech { get; set; }
        [DataMember]
        public string fech_str { get; set; }
        [DataMember]
        public decimal value { get; set; }

    }
    [CollectionDataContract]
    public class ListExchangeRate : List<ExchangeRate>
    {

    }



    
}
