using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.BusinessEntities
{
    public class ArchivoXML
    {
        public String Nombre { get; set; }

        public String cbcID { get; set; }
        public String cbcCurrencyID { get; set; }

        public String cbcUBLVersionID { get; set; }
        public String cbcCustomizationID { get; set; }
        public String cbcIDDocu { get; set; }

        public string FechaEmision { get; set; }


        public String TipoDocumento { get; set; }


        public string cbcDocumentCurrencyCode { get; set; }

        public String cbcIdSing { get; set; }

        public string RucEmpresa { get; set; }

        public string cbcName { get; set; }

        public string cbcIDUbigeo { get; set; }
        public string EmpresaDireccion { get; set; }

        public string cbcCountry { get; set; }


        public string EmpresaRazonSocial { get; set; }


        public string cbcRazonSocial2 { get; set; }


        public string TipoDocIdentidadEmpresa { get; set; }
        public string TipoDocIdentidadCliente { get; set; }


        public string ClienteRazonSocial { get; set; }

        public string ClienteDireccion { get; set; }


        public string TaxCurrencyID { get; set; }
        public string TaxCurrencyID2 { get; set; }


        public string cbcTaxID { get; set; }
        public string cbcTaxName { get; set; }
        public string cbcTaxTypeCode { get; set; }

        public string cbcPayCurrencyID { get; set; }

        public string cbcInvID { get; set; }
        public string cbcInvUnitCode { get; set; }
        public string cbcInvCurrencyId { get; set; }

        public string cbcPriceCurrencyID { get; set; }
        public string cbcPriceTypeCode { get; set; }


        public decimal IGV { get; set; }

        public string cbcTaxAmountCurrencyID2 { get; set; }

        public string cbcTaxEmptionReasonCode { get; set; }

        public string cbcTaxSchemeID { get; set; }
        public string cbcTaxSchemeName { get; set; }
        public string cbcTaxSchemeTypeCode { get; set; }


        public string DescripcionProducto { get; set; }
        public string cbcIDProducto { get; set; }

        public string cbcPrecioProducto { get; set; }

        public string NroDocumentoCliente { get; set; }



        public string TotalGravado { get; set; }

        public string TotalNoGravado { get; set; }

        public string ValorResumen { get; set; }
        public string ValorFirma { get; set; }

        public string Importe { get; set; }

        public decimal TotalImporteGravado { get; set; }

        public string MontoLiteral { get; set; }

        public string Serie { get; set; }
        public string Correlativo { get; set; }
    }
}
