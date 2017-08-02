using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class LogueoClass
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public Empleado Empleado { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public Perfil Perfil { get; set; }

        [DataMember]
        public DateTime FechaIngreso { get; set; }

        [DataMember]
        public DateTime FechaSalida { get; set; }

        [DataMember]
        public int IdPerfil { get; set; }

        [DataMember]
        public string NombrePerfil { get; set; }

        [DataMember]
        public string NombresApellidos { get; set; }

        [DataMember]
        public string Username_log { get; set; }

        [DataMember]
        public TipoLog TipoLog { get; set; }


        [DataMember]
        public string FechaDesde { get; set; }

        [DataMember]
        public string FechaHasta { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public string HostName { get; set; }

        [DataMember]
        public string sIP { get; set; }

        [DataMember]
        public string RucEntity { get; set; }

        [DataMember]
        public Empresa Empresa { get; set; }

        [DataMember]
        public string DNI { get; set; }
    }

    [CollectionDataContract]
    public class ListaLogueoClass : List<LogueoClass>
    {

    }


    [DataContract]
    public class TipoLog
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public string Descripcion { get; set; }



    }

    [CollectionDataContract]
    public class ListaTipoLog : List<TipoLog>
    {

    }


    [DataContract]
    public class LogAde
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string MenuAmbiente { get; set; }

        [DataMember]
        public string MessageLog { get; set; }

        [DataMember]
        public string InnerException { get; set; }

        [DataMember]
        public DateTime FechaLog { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public string Correction { get; set; }

        [DataMember]
        public int IdMenu { get; set; }

        [DataMember]
        public Menu Menu { get; set; }

        [DataMember]
        public string FechaDesde { get; set; }

        [DataMember]
        public string FechaHasta { get; set; }

        [DataMember]
        public string NombreMenu { get; set; }
    }

    [CollectionDataContract]
    public class ListaLogAde : List<LogAde>
    {

    }
}
