using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Bank
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string BankName { get; set; }

        [DataMember]
        public string CtaSoles { get; set; }

        [DataMember]
        public string CtaDolares { get; set; }

        [DataMember]
        public Empresa Empresa { get; set; }

        [DataMember]
        public int TypeBank { get; set; }

        [DataMember]
        public Estado Estado { get; set; }
    }

    [CollectionDataContract]
    public class ListBank : List<Bank>
    {

    }
}
