using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [DataContract]
    public class DetalleFacturaElectronica
    {
        [DataMember]
        public FacturaElectronica FacturaElectronica { get; set; }

        [DataMember]
        public Int32 NroOrden { get; set; }

        [DataMember]
        public String NumOrden { get; set; }

        [DataMember]
        public Decimal Cantidad { get; set; }

        [DataMember]
        public Decimal ValorUnitario { get; set; }

        [DataMember]
        public Decimal IGV { get; set; }

        [DataMember]
        public Decimal SubTotal { get; set; }

        [DataMember]
        public string SubTotalTexto { get; set; }

        [DataMember]
        public Producto Producto { get; set; }

        [DataMember]
        public Decimal MontoPrecioVenta { get; set; }

        [DataMember]
        public Decimal ValorVenta { get; set; }

        [DataMember]
        public string ValorVentaTexto { get; set; }

        [DataMember]
        public String CodigoAfectoIGV { get; set; }

        //[DataMember]
        //public Decimal MontoISC { get; set; }

        [DataMember]
        public String CodigoSisISC { get; set; }

        [DataMember]
        public String DescripcionMonto { get; set; }

        [DataMember]
        public string Unidad { get; set; }

        [DataMember]
        public String CodigoProducto { get; set; }

        [DataMember]
        public String Descripcion { get; set; }

        [DataMember]
        public decimal PrecioVenta { get; set; }

        [DataMember]
        public string PrecioVentaTexto { get; set; }

        [DataMember]
        public decimal Importe { get; set; }

        [DataMember]
        public string Nro { get; set; }

        [DataMember]
        public string MontoExonerado { get; set; }

        [DataMember]
        public string MontoGravado { get; set; }

        [DataMember]
        public string MontoIGV { get; set; }

        [DataMember]
        public string MontoTotal { get; set; }

        [DataMember]
        public int Emitidas { get; set; }

        [DataMember]
        public int Anuladas { get; set; }

        [DataMember]
        public int Linea { get; set; }

        [DataMember]
        public int NroInicio { get; set; }

        [DataMember]
        public int NroFin { get; set; }

        [DataMember]
        public TipoDocumento TipoDocumento { get; set; }

        [DataMember]
        public Serie Serie { get; set; }

        [DataMember]
        public int TotalDocEmitido { get; set; }

        [DataMember]
        public string MontoInafecto { get; set; }

        [DataMember]
        public string TotalOtrosCargos { get; set; }

        [DataMember]
        public string MontoISC { get; set; }

        [DataMember]
        public string TotalOtrosTributos { get; set; }

        [DataMember]
        public string NombreArchivoXML { get; set; }

        [DataMember]
        public decimal ValorVentaSinIGV { get; set; }

        [DataMember]
        public string NumeroSerie { get; set; }

        [DataMember]
        public string NumeroDocumento { get; set; }

        [DataMember]
        public string CodigoAfecto { get; set; }


        [DataMember]
        public decimal DetMontoIGV { get; set; }

        [DataMember]
        public decimal DetMontoISC { get; set; }

        [DataMember]
        public decimal DetMontoOtros { get; set; }

        [DataMember]
        public string DetMontoIGVText { get; set; }

        [DataMember]
        public string DetMontoISCText { get; set; }

        [DataMember]
        public string DetMontoOtrosText { get; set; }


        [DataMember]
        public decimal Dscto { get; set; }

        [DataMember]
        public string DsctoText { get; set; }

        [DataMember]
        public decimal TotDscto { get; set; }

        [DataMember]
        public string TotDsctoText { get; set; }

        [DataMember]
        public string Simbolo { get; set; }

        [DataMember]
        public string SimboloSol { get; set; }
    }
}
