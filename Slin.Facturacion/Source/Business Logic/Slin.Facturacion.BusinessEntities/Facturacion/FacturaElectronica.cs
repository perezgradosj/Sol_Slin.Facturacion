using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Runtime.Serialization;
using System.Xml;
using WcfSerialization = global::System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [WcfSerialization::DataContract(Namespace = "http://www.slin.com.pe", Name = "FacturaElectronica")]
    public class FacturaElectronica
    {
        [WcfSerialization::DataMember(Name = "IdFactura", IsRequired = false, Order = 0)]
        public Int32 IdFactura { get; set; }

        [WcfSerialization::DataMember(Name = "TipoDocumento", IsRequired = false, Order = 0)]
        public TipoDocumento TipoDocumento { get; set; }

        [WcfSerialization::DataMember(Name = "NumeroSerie", IsRequired = false, Order = 0)]
        public String NumeroSerie { get; set; }

        [WcfSerialization::DataMember(Name = "NumeroDocumento", IsRequired = false, Order = 0)]
        public String NumeroDocumento { get; set; }

        [WcfSerialization::DataMember(Name = "FechaEmision", IsRequired = false, Order = 0)]
        public DateTime FechaEmision { get; set; }

        [WcfSerialization::DataMember(Name = "FirmaDigital", IsRequired = false, Order = 0)]
        public String FirmaDigital { get; set; }

        [WcfSerialization::DataMember(Name = "Emisor", IsRequired = false, Order = 0)]
        public Emisor Emisor { get; set; }

        [WcfSerialization::DataMember(Name = "TipoMoneda", IsRequired = false, Order = 0)]
        public String TipoMoneda { get; set; }

        [WcfSerialization::DataMember(Name = "IGV", IsRequired = false, Order = 0)]
        public Decimal IGV { get; set; }

        [WcfSerialization::DataMember(Name = "FechaInicio", IsRequired = false, Order = 0)]
        public string FechaInicio { get; set; }

        [WcfSerialization::DataMember(Name = "FechaFin", IsRequired = false, Order = 0)]
        public string FechaFin { get; set; }

        [WcfSerialization::DataMember(Name = "Serie", IsRequired = false, Order = 0)]
        public Serie Serie { get; set; }

        [WcfSerialization::DataMember(Name = "NumeroDocumentoInicio", IsRequired = false, Order = 0)]
        public String NumeroDocumentoInicio { get; set; }

        [WcfSerialization::DataMember(Name = "NumeroDocumentoFin", IsRequired = false, Order = 0)]
        public String NumeroDocumentoFin { get; set; }

        [WcfSerialization::DataMember(Name = "Nro", IsRequired = false, Order = 0)]
        public Int32 Nro { get; set; }

        [WcfSerialization::DataMember(Name = "Cliente", IsRequired = false, Order = 0)]
        public Cliente Cliente { get; set; }

        [WcfSerialization::DataMember(Name = "Estado", IsRequired = false, Order = 0)]
        public Estado Estado { get; set; }

        [WcfSerialization::DataMember(Name = "MontoTotal", IsRequired = false, Order = 0)]
        public Decimal MontoTotal { get; set; }

        [WcfSerialization::DataMember(Name = "RutaImagen", IsRequired = false, Order = 0)]
        public string RutaImagen { get; set; }

        [WcfSerialization::DataMember(Name = "NombreArchivoXML", IsRequired = false, Order = 0)]
        public String NombreArchivoXML { get; set; }

        [WcfSerialization::DataMember(Name = "Empresa", IsRequired = false, Order = 0)]
        public Empresa Empresa { get; set; }

        [WcfSerialization::DataMember(Name = "Moneda", IsRequired = false, Order = 0)]
        public Moneda Moneda { get; set; }

        [WcfSerialization::DataMember(Name = "TotalGravado", IsRequired = false, Order = 0)]
        public string TotalGravado { get; set; }

        [WcfSerialization::DataMember(Name = "TotalnoGravado", IsRequired = false, Order = 0)]
        public string TotalnoGravado { get; set; }

        [WcfSerialization::DataMember(Name = "TotalExonerado", IsRequired = false, Order = 0)]
        public string TotalExonerado { get; set; }

        [WcfSerialization::DataMember(Name = "TotalExonerado_mon", IsRequired = false, Order = 0)]
        public decimal TotalExonerado_mon { get; set; }

        [WcfSerialization::DataMember(Name = "TotalDescuento", IsRequired = false, Order = 0)]
        public string TotalDescuento { get; set; }

        [WcfSerialization::DataMember(Name = "TotalDescuento_mon", IsRequired = false, Order = 0)]
        public decimal TotalDescuento_mon { get; set; }

        [WcfSerialization::DataMember(Name = "MontoTotalLetras", IsRequired = false, Order = 0)]
        public string MontoTotalLetras { get; set; }

        [WcfSerialization::DataMember(Name = "TipoMonto", IsRequired = false, Order = 0)]
        public string TipoMonto { get; set; }

        [WcfSerialization::DataMember(Name = "TotalTipoMonto", IsRequired = false, Order = 0)]
        public string TotalTipoMonto { get; set; }

        [WcfSerialization::DataMember(Name = "CodigoPDF417", IsRequired = false, Order = 0)]
        public Byte[] CodigoPDF417 { get; set; }

        [WcfSerialization::DataMember(Name = "LogoEmpresa", IsRequired = false, Order = 0)]
        public Byte[] LogoEmpresa { get; set; }

        [WcfSerialization::DataMember(Name = "ValorResumen", IsRequired = false, Order = 0)]
        public string ValorResumen { get; set; }

        [WcfSerialization::DataMember(Name = "ValorFirma", IsRequired = false, Order = 0)]
        public string ValorFirma { get; set; }

        [WcfSerialization::DataMember(Name = "MontoIgvCad", IsRequired = false, Order = 0)]
        public string MontoIgvCad { get; set; }

        [WcfSerialization::DataMember(Name = "MontoTotalCad", IsRequired = false, Order = 0)]
        public string MontoTotalCad { get; set; }

        [WcfSerialization::DataMember(Name = "FechaAnulado", IsRequired = false, Order = 0)]
        public DateTime FechaAnulado { get; set; }

        [WcfSerialization::DataMember(Name = "MotivoAnulado", IsRequired = false, Order = 0)]
        public string MotivoAnulado { get; set; }

        [WcfSerialization::DataMember(Name = "EstadoEnvio", IsRequired = false, Order = 0)]
        public EstadoEnvio EstadoEnvio { get; set; }

        [WcfSerialization::DataMember(Name = "MensajeAnulado", IsRequired = false, Order = 0)]
        public string MensajeAnulado { get; set; }

        [WcfSerialization::DataMember(Name = "MensajeEnvio", IsRequired = false, Order = 0)]
        public string MensajeEnvio { get; set; }

        [WcfSerialization::DataMember(Name = "NumeroAtencion", IsRequired = false, Order = 0)]
        public string NumeroAtencion { get; set; }

        [WcfSerialization::DataMember(Name = "FechaEnvio", IsRequired = false, Order = 0)]
        public string FechaEnvio { get; set; }

        [WcfSerialization::DataMember(Name = "Secuencia", IsRequired = false, Order = 0)]
        public string Secuencia { get; set; }

        [WcfSerialization::DataMember(Name = "FechaInicio2", IsRequired = false, Order = 0)]
        public DateTime FechaInicio2 { get; set; }

        [WcfSerialization::DataMember(Name = "FechaFin2", IsRequired = false, Order = 0)]
        public DateTime FechaFin2 { get; set; }

        [WcfSerialization::DataMember(Name = "FechaEnvio2", IsRequired = false, Order = 0)]
        public DateTime FechaEnvio2 { get; set; }

        [WcfSerialization::DataMember(Name = "FechaCarga", IsRequired = false, Order = 0)]
        public string FechaCarga { get; set; }

        [WcfSerialization::DataMember(Name = "XML", IsRequired = false, Order = 0)]
        public string XML { get; set; }

        [WcfSerialization::DataMember(Name = "CDR", IsRequired = false, Order = 0)]
        public string CDR { get; set; }

        [WcfSerialization::DataMember(Name = "TipoFecha", IsRequired = false, Order = 0)]
        public int TipoFecha { get; set; }

        [WcfSerialization::DataMember(Name = "SerieCorrelativo", IsRequired = false, Order = 0)]
        public string SerieCorrelativo { get; set; }

        [WcfSerialization::DataMember(Name = "SumaIGV", IsRequired = false, Order = 0)]
        public decimal SumaIGV { get; set; }

        [WcfSerialization::DataMember(Name = "TotalGravadoSinIGV", IsRequired = false, Order = 0)]
        public decimal TotalGravadoSinIGV { get; set; }

        [WcfSerialization::DataMember(Name = "FechaEmision2", IsRequired = false, Order = 0)]
        public string FechaEmision2 { get; set; }

        [WcfSerialization::DataMember(Name = "MensajeEnvioDetalle", IsRequired = false, Order = 0)]
        public string MensajeEnvioDetalle { get; set; }

        [WcfSerialization::DataMember(Name = "TotalInafecto", IsRequired = false, Order = 0)]
        public string TotalInafecto { get; set; }


        [WcfSerialization::DataMember(Name = "TotalInafecto_mon", IsRequired = false, Order = 0)]
        public decimal TotalInafecto_mon { get; set; }


        [WcfSerialization::DataMember(Name = "TotalGratuito", IsRequired = false, Order = 0)]
        public string TotalGratuito { get; set; }

        [WcfSerialization::DataMember(Name = "TotalGratuito_mon", IsRequired = false, Order = 0)]
        public decimal TotalGratuito_mon { get; set; }

        [WcfSerialization::DataMember(Name = "SubTotalVenta", IsRequired = false, Order = 0)]
        public string SubTotalVenta { get; set; }


        [WcfSerialization::DataMember(Name = "SubTotalVenta_mon", IsRequired = false, Order = 0)]
        public decimal SubTotalVenta_mon { get; set; }


        [WcfSerialization::DataMember(Name = "TotalPercepciones", IsRequired = false, Order = 0)]
        public string TotalPercepciones { get; set; }



        [WcfSerialization::DataMember(Name = "TotalPercepciones_mon", IsRequired = false, Order = 0)]
        public decimal TotalPercepciones_mon { get; set; }



        [WcfSerialization::DataMember(Name = "TotalRetenciones", IsRequired = false, Order = 0)]
        public string TotalRetenciones { get; set; }

        [WcfSerialization::DataMember(Name = "TotalRetenciones_mon", IsRequired = false, Order = 0)]
        public decimal TotalRetenciones_mon { get; set; }


        [WcfSerialization::DataMember(Name = "TotalDetracciones", IsRequired = false, Order = 0)]
        public string TotalDetracciones { get; set; }


        [WcfSerialization::DataMember(Name = "TotalDetracciones_mon", IsRequired = false, Order = 0)]
        public decimal TotalDetracciones_mon { get; set; }


        [WcfSerialization::DataMember(Name = "TotalBonificaciones", IsRequired = false, Order = 0)]
        public string TotalBonificaciones { get; set; }

        [WcfSerialization::DataMember(Name = "TotalBonificaciones_mon", IsRequired = false, Order = 0)]
        public decimal TotalBonificaciones_mon { get; set; }

        [WcfSerialization::DataMember(Name = "Adicionales", IsRequired = false, Order = 0)]
        public Adicionales Adicionales { get; set; }

        [WcfSerialization::DataMember(Name = "Detraccion", IsRequired = false, Order = 0)]
        public Detraccion Detraccion { get; set; }

        [WcfSerialization::DataMember(Name = "BeneficioHospedaje", IsRequired = false, Order = 0)]
        public BeneficioHospedaje BeneficioHospedaje { get; set; }

        [WcfSerialization::DataMember(Name = "Leyenda", IsRequired = false, Order = 0)]
        public Leyenda Leyenda { get; set; }

        [WcfSerialization::DataMember(Name = "Nota", IsRequired = false, Order = 0)]
        public string Nota { get; set; }

        [WcfSerialization::DataMember(Name = "DocCRECPE", IsRequired = false, Order = 0)]
        public DocCRECPE DocCRECPE { get; set; }

        [WcfSerialization::DataMember(Name = "ListaDocCRECPE", IsRequired = false, Order = 0)]
        public ListaDocCRECPE ListaDocCRECPE { get; set; }

        [WcfSerialization::DataMember(Name = "Extra", IsRequired = false, Order = 0)]
        public Extra Extra { get; set; }

        [WcfSerialization::DataMember(Name = "ListaExtra", IsRequired = false, Order = 0)]
        public ListaExtra ListaExtra { get; set; }

        [WcfSerialization::DataMember(Name = "Cantidad", IsRequired = false, Order = 0)]
        public int Cantidad { get; set; }

        [WcfSerialization::DataMember(Name = "IdEstadoDoc", IsRequired = false, Order = 0)]
        public int IdEstadoDoc { get; set; }

        [WcfSerialization::DataMember(Name = "EstadoDesc", IsRequired = false, Order = 0)]
        public string EstadoDesc { get; set; }

        [WcfSerialization::DataMember(Name = "TotOtrosCargos", IsRequired = false, Order = 0)]
        public decimal TotOtrosCargos { get; set; }




        #region RETENCION

        [WcfSerialization::DataMember(Name = "ImporteTotalRetenido", IsRequired = false, Order = 0)]
        public string ImporteTotalRetenido { get; set; }

        [WcfSerialization::DataMember(Name = "MonedaImpTotalRetenido", IsRequired = false, Order = 0)]
        public string MonedaImpTotalRetenido { get; set; }

        [WcfSerialization::DataMember(Name = "RegimenRetencion", IsRequired = false, Order = 0)]
        public string RegimenRetencion { get; set; }

        [WcfSerialization::DataMember(Name = "TasaRetencion", IsRequired = false, Order = 0)]
        public string TasaRetencion { get; set; }

        [WcfSerialization::DataMember(Name = "ImporteTotalPagado", IsRequired = false, Order = 0)]
        public string ImporteTotalPagado { get; set; }

        [WcfSerialization::DataMember(Name = "MonedaImpTotalPagado", IsRequired = false, Order = 0)]
        public string MonedaImpTotalPagado { get; set; }

        [WcfSerialization::DataMember(Name = "TipoDocRelac", IsRequired = false, Order = 0)]
        public string TipoDocRelac { get; set; }

        [WcfSerialization::DataMember(Name = "NroDocRelac", IsRequired = false, Order = 0)]
        public string NroDocRelac { get; set; }

        [WcfSerialization::DataMember(Name = "FechaEmisionDocRelac", IsRequired = false, Order = 0)]
        public string FechaEmisionDocRelac { get; set; }

        [WcfSerialization::DataMember(Name = "ImporteTotDocRelac", IsRequired = false, Order = 0)]
        public string ImporteTotDocRelac { get; set; }

        [WcfSerialization::DataMember(Name = "MonedaImpTotDocRelac", IsRequired = false, Order = 0)]
        public string MonedaImpTotDocRelac { get; set; }

        [WcfSerialization::DataMember(Name = "FechaPago", IsRequired = false, Order = 0)]
        public string FechaPago { get; set; }

        [WcfSerialization::DataMember(Name = "NumeroPago", IsRequired = false, Order = 0)]
        public string NumeroPago { get; set; }

        [WcfSerialization::DataMember(Name = "ImportePagoSinReten", IsRequired = false, Order = 0)]
        public string ImportePagoSinReten { get; set; }

        [WcfSerialization::DataMember(Name = "MonedaPago", IsRequired = false, Order = 0)]
        public string MonedaPago { get; set; }

        [WcfSerialization::DataMember(Name = "ImporteReteneido", IsRequired = false, Order = 0)]
        public string ImporteReteneido { get; set; }

        [WcfSerialization::DataMember(Name = "MonedaImpRetenido", IsRequired = false, Order = 0)]
        public string MonedaImpRetenido { get; set; }

        [WcfSerialization::DataMember(Name = "FechaRetencion", IsRequired = false, Order = 0)]
        public string FechaRetencion { get; set; }

        [WcfSerialization::DataMember(Name = "ImportTotxPagoNeto", IsRequired = false, Order = 0)]
        public string ImportTotxPagoNeto { get; set; }

        [WcfSerialization::DataMember(Name = "MonedaTotxPagoNeto", IsRequired = false, Order = 0)]
        public string MonedaTotxPagoNeto { get; set; }

        [WcfSerialization::DataMember(Name = "MonedaRefTpoCambio", IsRequired = false, Order = 0)]
        public string MonedaRefTpoCambio { get; set; }

        [WcfSerialization::DataMember(Name = "MonObjetivoTasaCambio", IsRequired = false, Order = 0)]
        public string MonObjetivoTasaCambio { get; set; }

        [WcfSerialization::DataMember(Name = "TipoCambio", IsRequired = false, Order = 0)]
        public string TipoCambio { get; set; }

        [WcfSerialization::DataMember(Name = "FechaCambio", IsRequired = false, Order = 0)]
        public string FechaCambio { get; set; }


        [WcfSerialization::DataMember(Name = "FechaCambioDate", IsRequired = false, Order = 0)]
        public DateTime FechaCambioDate { get; set; }


        [WcfSerialization::DataMember(Name = "ImpPagGlobSoles", IsRequired = false, Order = 0)]
        public decimal ImpPagGlobSoles { get; set; }

        [WcfSerialization::DataMember(Name = "ImpGlobSoles", IsRequired = false, Order = 0)]
        public decimal ImpGlobSoles { get; set; }


        [WcfSerialization::DataMember(Name = "Sede", IsRequired = false, Order = 0)]
        public string Sede { get; set; }

        [WcfSerialization::DataMember(Name = "IdSede", IsRequired = false, Order = 0)]
        public int IdSede { get; set; }

        [WcfSerialization::DataMember(Name = "Impresora", IsRequired = false, Order = 0)]
        public string Impresora { get; set; }

        [WcfSerialization::DataMember(Name = "Usuario", IsRequired = false, Order = 0)]
        public string Usuario { get; set; }

        [WcfSerialization::DataMember(Name = "Campo1", IsRequired = false, Order = 0)]
        public string Campo1 { get; set; }

        [WcfSerialization::DataMember(Name = "Campo2", IsRequired = false, Order = 0)]
        public string Campo2 { get; set; }

        [WcfSerialization::DataMember(Name = "Campo3", IsRequired = false, Order = 0)]
        public string Campo3 { get; set; }

        [WcfSerialization::DataMember(Name = "Campo4", IsRequired = false, Order = 0)]
        public string Campo4 { get; set; }

        [WcfSerialization::DataMember(Name = "Campo5", IsRequired = false, Order = 0)]
        public string Campo5 { get; set; }

        [WcfSerialization::DataMember(Name = "Campo6", IsRequired = false, Order = 0)]
        public string Campo6 { get; set; }

        [WcfSerialization::DataMember(Name = "Campo7", IsRequired = false, Order = 0)]
        public string Campo7 { get; set; }

        [WcfSerialization::DataMember(Name = "Campo8", IsRequired = false, Order = 0)]
        public string Campo8 { get; set; }

        [WcfSerialization::DataMember(Name = "Campo9", IsRequired = false, Order = 0)]
        public string Campo9 { get; set; }

        [WcfSerialization::DataMember(Name = "Campo10", IsRequired = false, Order = 0)]
        public string Campo10 { get; set; }


        [WcfSerialization::DataMember(Name = "REF_FILES", IsRequired = false, Order = 0)]
        public string REF_FILES { get; set; }

        [WcfSerialization::DataMember(Name = "NroOrdCompra", IsRequired = false, Order = 0)]
        public string NroOrdCompra { get; set; }

        [WcfSerialization::DataMember(Name = "CodeMessage", IsRequired = false, Order = 0)]
        public string CodeMessage { get; set; }

        [WcfSerialization::DataMember(Name = "DocMessage", IsRequired = false, Order = 0)]
        public string DocMessage { get; set; }

        [WcfSerialization::DataMember(Name = "CodeResponse", IsRequired = false, Order = 0)]
        public string CodeResponse { get; set; }

        [WcfSerialization::DataMember(Name = "NoteResponse", IsRequired = false, Order = 0)]
        public string NoteResponse { get; set; }


        [WcfSerialization::DataMember(Name = "TypeFormat", IsRequired = false, Order = 0)]
        public int TypeFormat { get; set; }


        #endregion


        #region SEND MAIL

        [WcfSerialization::DataMember(Name = "Email", IsRequired = false, Order = 0)]
        public Email Email { get; set; }

        [WcfSerialization::DataMember(Name = "Para", IsRequired = false, Order = 0)]
        public string Para { get; set; }

        [WcfSerialization::DataMember(Name = "CC", IsRequired = false, Order = 0)]
        public string CC { get; set; }

        [WcfSerialization::DataMember(Name = "CCO", IsRequired = false, Order = 0)]
        public string CCO { get; set; }

        #endregion


        #region OTHER NOT CRED AND DEB

        [WcfSerialization::DataMember(Name = "Discr_ReferenceID", IsRequired = false, Order = 0)]
        public string Discr_ReferenceID { get; set; }

        [WcfSerialization::DataMember(Name = "Discr_ResponseCode", IsRequired = false, Order = 0)]
        public string Discr_ResponseCode { get; set; }

        [WcfSerialization::DataMember(Name = "Discr_Description", IsRequired = false, Order = 0)]
        public string Discr_Description { get; set; }

        #endregion END OTHERS


        [WcfSerialization::DataMember(Name = "Referencia", IsRequired = false, Order = 0)]
        public Referencia Referencia { get; set; }

        [WcfSerialization::DataMember(Name = "ListaReferencia", IsRequired = false, Order = 0)]
        public ListaReferencia ListaReferencia { get; set; }



        [WcfSerialization::DataMember(Name = "ListaDetalleFacturaElectronica", IsRequired = false, Order = 0)]
        public ListaDetalleFacturaElectronica ListaDetalleFacturaElectronica { get; set; }

        [WcfSerialization::DataMember(Name = "DetalleFacturaElectronica", IsRequired = false, Order = 0)]
        public DetalleFacturaElectronica DetalleFacturaElectronica { get; set; }

        [WcfSerialization::DataMember(Name = "MontoIGV", IsRequired = false, Order = 0)]
        public decimal MontoIGV { get; set; }

        [WcfSerialization::DataMember(Name = "MontoISC", IsRequired = false, Order = 0)]
        public decimal MontoISC { get; set; }

        [WcfSerialization::DataMember(Name = "MontoOtros", IsRequired = false, Order = 0)]
        public decimal MontoOtros { get; set; }


        [WcfSerialization::DataMember(Name = "MontoIGVText", IsRequired = false, Order = 0)]
        public string MontoIGVText { get; set; }

        [WcfSerialization::DataMember(Name = "MontoISCText", IsRequired = false, Order = 0)]
        public string MontoISCText { get; set; }

        [WcfSerialization::DataMember(Name = "MontoOtrosText", IsRequired = false, Order = 0)]
        public string MontoOtrosText { get; set; }






        [WcfSerialization::DataMember(Name = "TpoOperacion", IsRequired = false, Order = 0)]
        public string TpoOperacion { get; set; }

        [WcfSerialization::DataMember(Name = "PorcentDetraccion", IsRequired = false, Order = 0)]
        public string PorcentDetraccion { get; set; }

        [WcfSerialization::DataMember(Name = "MontoDetraccion", IsRequired = false, Order = 0)]
        public decimal MontoDetraccion { get; set; }

        [WcfSerialization::DataMember(Name = "CodTpoNC", IsRequired = false, Order = 0)]
        public string CodTpoNC { get; set; }

        [WcfSerialization::DataMember(Name = "DescTpoNC", IsRequired = false, Order = 0)]
        public string DescTpoNC { get; set; }

        [WcfSerialization::DataMember(Name = "CodTpoND", IsRequired = false, Order = 0)]
        public string CodTpoND { get; set; }

        [WcfSerialization::DataMember(Name = "DescTpoND", IsRequired = false, Order = 0)]
        public string DescTpoND { get; set; }




        [WcfSerialization::DataMember(Name = "Afectado", IsRequired = false, Order = 0)]
        public Afectado Afectado { get; set; }

        [WcfSerialization::DataMember(Name = "ListaAfectado", IsRequired = false, Order = 0)]
        public ListaAfectado ListaAfectado { get; set; }


        public void Inicializa()
        {
            IdFactura = 0;
            NumeroSerie = string.Empty;
            NumeroDocumento = string.Empty;
            FechaEmision = DateTime.Now;
            FirmaDigital = string.Empty;
            TipoMoneda = string.Empty;
            IGV = 0.00m;
            FechaInicio = string.Empty;
            FechaFin = string.Empty;
            NumeroDocumentoInicio = string.Empty;
            NumeroDocumentoFin = string.Empty;
            Nro = 0;
            MontoTotal = 0.00m;
            RutaImagen = string.Empty;
            NombreArchivoXML = string.Empty;
            TotalGravado = string.Empty;
            TotalnoGravado = string.Empty;
            TotalExonerado = string.Empty;
            TotalDescuento = string.Empty;
            MontoTotalLetras = string.Empty;
            TipoMonto = string.Empty;
            TotalTipoMonto = string.Empty;
            ValorResumen = string.Empty;
            ValorFirma = string.Empty;
            MontoIgvCad = string.Empty;
            MontoTotalCad = string.Empty;
            FechaAnulado = DateTime.Now;
            MotivoAnulado = string.Empty;
            MensajeAnulado = string.Empty;
            MensajeEnvio = string.Empty;
            NumeroAtencion = string.Empty;
            FechaEnvio = string.Empty;
            Secuencia = string.Empty;
            FechaInicio2 = DateTime.Now;
            FechaFin2 = DateTime.Now;
            FechaEnvio2 = DateTime.Now;
            FechaCarga = string.Empty;
            XML = string.Empty;
            TipoFecha = 0;
            SerieCorrelativo = string.Empty;
            SumaIGV = 0.00m;
            TotalGravadoSinIGV = 0.00m;
            FechaEmision2 = string.Empty;
            MensajeEnvioDetalle = string.Empty;
            TotalInafecto = string.Empty;
            TotalGratuito = string.Empty;
            SubTotalVenta = string.Empty;


            TotalPercepciones = string.Empty;
            TotalRetenciones = string.Empty;
            TotalDetracciones = string.Empty;
            TotalBonificaciones = string.Empty;
            Nota = string.Empty;
            ImporteTotalRetenido = string.Empty;
            MonedaImpTotalRetenido = string.Empty;
            RegimenRetencion = string.Empty;
            TasaRetencion = string.Empty;
            ImporteTotalPagado = string.Empty;
            MonedaImpTotalPagado = string.Empty;

            TipoDocRelac = string.Empty;
            NroDocRelac = string.Empty;
            FechaEmisionDocRelac = string.Empty;
            ImporteTotDocRelac = string.Empty;
            MonedaImpTotDocRelac = string.Empty;
            FechaPago = string.Empty;
            NumeroPago = string.Empty;
            ImportePagoSinReten = string.Empty;
            MonedaPago = string.Empty;
            ImporteReteneido = string.Empty;
            MonedaImpRetenido = string.Empty;
            FechaRetencion = string.Empty;
            ImportTotxPagoNeto = string.Empty;
            MonedaTotxPagoNeto = string.Empty;
            MonedaRefTpoCambio = string.Empty;
            MonObjetivoTasaCambio = string.Empty;
            TipoCambio = string.Empty;
            FechaCambio = string.Empty;
            ImpPagGlobSoles = 0.00m;
            ImpGlobSoles = 0.00m;
            Sede = string.Empty;
            Impresora = string.Empty;
            Usuario = string.Empty;
            Campo1 = string.Empty;
            Campo2 = string.Empty;
            Campo3 = string.Empty;
            Campo4 = string.Empty;
            Campo5 = string.Empty;
            Campo6 = string.Empty;
            Campo7 = string.Empty;
            Campo8 = string.Empty;
            Campo9 = string.Empty;
            Campo10 = string.Empty;
            REF_FILES = string.Empty;
            NroOrdCompra = string.Empty;
            Para = string.Empty;
            CC = string.Empty;
            CCO = string.Empty;

            Discr_ReferenceID = string.Empty;
            Discr_ResponseCode = string.Empty;
            Discr_Description = string.Empty;

            MontoIGV = 0.00m;
            MontoISC = 0.00m;
            MontoOtros = 0.00m;
            MontoIGVText = string.Empty;
            MontoISCText = string.Empty;
            MontoOtrosText = string.Empty;
        }
    }
}
