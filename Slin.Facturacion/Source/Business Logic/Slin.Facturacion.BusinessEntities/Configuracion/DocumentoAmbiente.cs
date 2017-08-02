using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class DocumentoAmbiente
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int ID_TPO_CE { get; set; }

        [DataMember]
        public string TIPO_CE { get; set; }

        [DataMember]
        public string DESCRIPCION_TPO_CE { get; set; }

        [DataMember]
        public AmbienteSunat AmbienteSunat { get; set; }

        [DataMember]
        public Estado Estado { get; set; }

        [DataMember]
        public Empresa Empresa { get; set; }
    }


    [DataContract]
    public class SecondaryUser
    {
        [DataMember]
        public int IdAmb { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string RucEntity { get; set; }
    }

    [CollectionDataContract]
    public class ListSecondaryUser : List<SecondaryUser>
    {

    }
}
