using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities.Configuracion
{
    [DataContract]
    public class Services
    {
        [DataMember]
        public int IdService { get; set; }
        [DataMember]
        public string CodeService { get; set; }
        [DataMember]
        public string NombreService { get; set; }
        [DataMember]
        public string ValueTime { get; set; }
        [DataMember]
        public string IntervaleValue { get; set; }
        [DataMember]
        public int MaxNumAttempts { get; set; }
        [DataMember]
        public Empresa Empresa { get; set; }
        [DataMember]
        public Estado Estado { get; set; }
        [DataMember]
        public string ServiceStatus { get; set; }
    }

    [CollectionDataContract]
    public class ListService : List<Services>
    {

    }


    //[DataContract]
    //public class Moneda
    //{
    //    [DataMember]
    //    public int IdMoneda { get; set; }

    //    [DataMember]
    //    public int Simbolo { get; set; }

    //    [DataMember]
    //    public string Descripcion { get; set; }

    //    [DataMember]
    //    public string CodeInternal { get; set; }
    //}

    //[CollectionDataContract]
    //public class ListaMoneda : List<Moneda>
    //{

    //}
    

    [DataContract]
    public class TipoCambio
    {
        [DataMember]
        public int IdTipoCambio { get; set; }
        [DataMember]
        public Moneda Moneda { get; set; }
        [DataMember]
        public decimal PurchaseValue { get; set; }
        [DataMember]
        public decimal SaleValue { get; set; }
        [DataMember]
        public Empresa Empresa { get; set; }
        [DataMember]
        public string DateValue { get; set; }//Fecha del tipo de Cambio
    }

    [CollectionDataContract]
    public class ListaTipoCambio : List<TipoCambio>
    {

    }

    [DataContract]
    public class CertificateDigital
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string NameCertificate { get; set; }
        [DataMember]
        public string Pwd { get; set; }
        [DataMember]
        public string ExpirationDate { get; set; }
        [DataMember]
        public string RucEntity { get; set; }
    }

    [CollectionDataContract]
    public class ListCertificateDigital : List<CertificateDigital>
    {

    }
}

