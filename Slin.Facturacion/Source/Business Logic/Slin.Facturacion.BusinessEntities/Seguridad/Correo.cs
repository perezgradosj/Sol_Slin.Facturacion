using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Correo
    {
        [DataMember]
        public int IdEmail { get; set; }
        [DataMember]
        public string Mensaje { get; set; }
        [DataMember]
        public string Asunto { get; set; }
        [DataMember]
        public string EmailDestino { get; set; }
        [DataMember]
        public string EmailSalida { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public Empresa Empresa { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Domain { get; set; }
        [DataMember]
        public string IP { get; set; }
        [DataMember]
        public int Port { get; set; }
        [DataMember]
        public int Port_Alternative { get; set; }
        [DataMember]
        public Estado Estado { get; set; }
        [DataMember]
        public string TypeMail { get; set; }
        [DataMember]
        public SSL SSL { get; set; }
        [DataMember]
        public int UseAuthenticate { get; set; }
        [DataMember]
        public string ContactName { get; set; }
    }
}
