using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class Adicionales
    {
        public string MontoLiteral { get; set; }
    }

    [DataContract]
    public class Leyenda
    {
        [DataMember]
        public string CodigoLeyenda { get; set; }

        [DataMember]
        public string DescripcionLeyenda { get; set; }
    }

    [DataContract]
    public class Detraccion
    {
        [DataMember]
        public string CodigoDetraccion { get; set; }

        [DataMember]
        public string DescripcionDetraccion { get; set; }

        [DataMember]
        public string Codigo { get; set; }
        [DataMember]
        public string CtaDetraccion { get; set; }

    }

    [DataContract]
    public class BeneficioHospedaje
    {
        [DataMember]
        public string CodigoBeneficio { get; set; }

        [DataMember]
        public string DescripcionBeneficio { get; set; }
    }

    [DataContract]
    public class DocCRECPE
    {
        [DataMember]
        public decimal ImporteTotalRetenido { get; set; }

        [DataMember]
        public string MonedaImpTotalRetenido { get; set; }

        [DataMember]
        public string RegimenRetencion { get; set; }

        [DataMember]
        public string TasaRetencion { get; set; }

        [DataMember]
        public decimal ImporteTotalPagado { get; set; }

        [DataMember]
        public string MonedaImpTotalPagado {get; set; }

        [DataMember]
        public string TipoDocRelac { get; set; }

        [DataMember]
        public string NroDocRelac { get; set; }

        [DataMember]
        public string FechaEmisionDocRelac { get; set; }


        [DataMember]
        public DateTime FechaEmiDocRelac { get; set; }

        [DataMember]
        public decimal ImporteTotDocRelac { get; set; }

        [DataMember]
        public string MonedaImpTotDocRelac { get; set; }

        [DataMember]
        public string FechaPago { get; set; }

        [DataMember]
        public DateTime FechaPagoDate { get; set; }

        [DataMember]
        public string NumeroPago { get; set; }

        [DataMember]
        public decimal ImportePagoSinReten { get; set; }

        [DataMember]
        public string MonedaPago { get; set; }

        [DataMember]
        public decimal ImporteRetenido { get; set; }

        [DataMember]
        public string MonedaImpRetenido { get; set; }

        [DataMember]
        public string FechaRetencion { get; set; }

        [DataMember]
        public DateTime FechaRetencionDate { get; set; }

        [DataMember]
        public decimal ImporteTotxPagoNeto { get; set; }//alterado

        [DataMember]
        public string MonedaTotxPagoNeto { get; set; }

        [DataMember]
        public string MonedaRefTpoCambio { get; set; }

        [DataMember]
        public string MonObjetivoTasaCambio { get; set; }

        [DataMember]
        public string TipoCambio { get; set; }

        [DataMember]
        public string FechaCambio { get; set; }

        [DataMember]
        public DateTime FechaTipoCambio { get; set; }

        [DataMember]
        public string Simbolo { get; set; }

        [DataMember]
        public string SimboloSol { get; set; }

        [DataMember]
        public string NombreArchivoXML { get; set; }

        [DataMember]
        public int NroOrden { get; set; }

        [DataMember]
        public string NumeroSerie { get; set; }

        [DataMember]
        public string NumeroDocumento { get; set; }


    }


    [CollectionDataContract]
    public class ListaDocCRECPE : List<DocCRECPE>
    {

    }


    [DataContract]
    public class Extra
    {
        [DataMember]
        public string ExLinea { get; set; }

        [DataMember]
        public string ExDato { get; set; }

        [DataMember]
        public string ExTipo { get; set; }
    }


    [CollectionDataContract]
    public class ListaExtra : List<Extra>
    {

    }


    [DataContract]
    public class Referencia
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string CodigoTipoDocumento { get; set; }


        [DataMember]
        public string ReferenceID { get; set; }

        [DataMember]
        public string ResponseCode { get; set; }

        [DataMember]
        public string Description { get; set; }
    }


    [CollectionDataContract]
    public class ListaReferencia : List<Referencia>
    {

    }



}
