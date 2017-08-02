using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Email
    {
        [DataMember]
        public string Para { get; set; }

        [DataMember]
        public string CC { get; set; }

        [DataMember]
        public string CCO { get; set; }

        [DataMember]
        public string TypeMail { get; set; }

        [DataMember]
        public string Description { get; set; }
    }

    [CollectionDataContract]
    public class ListaEmail : List<Email>
    {

    }


    [DataContract]
    public class SSL
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }
    }

    [CollectionDataContract]
    public class ListSSL : List<SSL>
    {

    }
}
