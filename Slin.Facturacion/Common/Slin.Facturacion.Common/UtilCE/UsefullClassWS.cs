using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WCFFacturacion = Slin.Facturacion.BusinessEntities;

using xmlFac = Slin.Facturacion.Common.CE;
using xmlCre = Slin.Facturacion.Common.CRE;
using xmlNotCred = Slin.Facturacion.Common.CENC;
using xmlNotDeb = Slin.Facturacion.Common.CEND;
using xmlRead = Slin.Facturacion.Common.Helper;
using xmlReten = Slin.Facturacion.Common.CRE;
using System.Xml;
using System.Globalization;
using Slin.Facturacion.Common.Method;

namespace Slin.Facturacion.Common.UtilCE
{
    public class UsefullClassWS
    {
        #region ENTITY

        private WCFFacturacion.FacturaElectronica ofactura;
        public WCFFacturacion.FacturaElectronica oFactura
        {
            get { return ofactura; }
            set
            {
                ofactura = value;
            }
        }


        private void Inicializa()
        {
            oFactura.ListaDetalleFacturaElectronica = new WCFFacturacion.ListaDetalleFacturaElectronica();
            oFactura.DetalleFacturaElectronica = new WCFFacturacion.DetalleFacturaElectronica();
            oFactura = new WCFFacturacion.FacturaElectronica();
            oFactura.Empresa = new WCFFacturacion.Empresa();
            oFactura.Cliente = new WCFFacturacion.Cliente();
            oFactura.TipoDocumento = new WCFFacturacion.TipoDocumento();
            oFactura.Empresa.TipoDocumentiIdentidad = new WCFFacturacion.TipoDocumentoIdentidad();
            oFactura.Cliente.TipoDocumentoIdentidad = new WCFFacturacion.TipoDocumentoIdentidad();

            oFactura.DocCRECPE = new WCFFacturacion.DocCRECPE();

            oFactura.Extra = new WCFFacturacion.Extra();
            oFactura.ListaExtra = new WCFFacturacion.ListaExtra();

            oFactura.DocCRECPE = new WCFFacturacion.DocCRECPE();
            oFactura.ListaDocCRECPE = new WCFFacturacion.ListaDocCRECPE();

            oFactura.Referencia = new WCFFacturacion.Referencia();
            oFactura.ListaReferencia = new WCFFacturacion.ListaReferencia();
            oFactura.Detraccion = new WCFFacturacion.Detraccion();
        }

        #endregion

        #region CE

        public WCFFacturacion.FacturaElectronica GetDataFromXMLCE(xmlFac.InvoiceType inv, XmlDocument XMLArchive, string NUM_CE)
        {
            oFactura = new WCFFacturacion.FacturaElectronica();
            Inicializa();
            XmlNodeList xmlnodelist;

            try
            {
                #region READ XML

                xmlnodelist = XMLArchive.GetElementsByTagName("ext:ExtensionContent");
                oFactura.NombreArchivoXML = NUM_CE;

                oFactura.TpoOperacion = inv.InvoiceTypeCode.Value == null ? string.Empty : inv.InvoiceTypeCode.Value;//TIPO OPERACION

                oFactura.SerieCorrelativo = inv.ID.Value; //SERIE CORRELATIVO

                string[] array = oFactura.SerieCorrelativo.Split('-');
                oFactura.NumeroSerie = array[0];
                oFactura.NumeroDocumento = array[1];

                oFactura.FechaEmision2 = inv.IssueDate.Value.ToString("yyyy-MM-dd");
                oFactura.FechaEmision = inv.IssueDate.Value;

                oFactura.TipoDocumento.CodigoDocumento = inv.InvoiceTypeCode.Value; //TIPO CE
                oFactura.TipoMoneda = inv.DocumentCurrencyCode.Value; //TIPO MONEDA

                switch (oFactura.TipoMoneda)
                {
                    case "USD": { oFactura.TipoMoneda = "DÓLARES AMERICANOS"; break; }
                    case "PEN": { oFactura.TipoMoneda = "SOLES"; break; }
                }

                oFactura.Empresa.RUC = inv.AccountingSupplierParty.CustomerAssignedAccountID.Value; //RUC EMISOR
                oFactura.Empresa.TipoDocumentiIdentidad.Codigo = inv.AccountingSupplierParty.AdditionalAccountID.First().Value; //TPO DOC EMISOR
                oFactura.Empresa.RazonSocial = inv.AccountingSupplierParty.Party.PartyLegalEntity.ToList().First().RegistrationName.Value; // RAZON SOCIAL EMISOR
                oFactura.Empresa.RazonComercial = inv.AccountingSupplierParty.Party.PartyName.First().Name.Value; //RAZON COMERCIAL
                oFactura.Empresa.CodigoUbigeo = inv.AccountingSupplierParty.Party.PostalAddress.ID.Value; //UBIGEO EMISOR
                oFactura.Empresa.Direccion = inv.AccountingSupplierParty.Party.PostalAddress.StreetName.Value; //DIRECCION EMISOR

                oFactura.Cliente.ClienteRuc = string.Empty + inv.AccountingCustomerParty.CustomerAssignedAccountID.Value; //NRO DOC CLIENTE
                oFactura.Cliente.TipoDocumentoIdentidad.Codigo = inv.AccountingCustomerParty.AdditionalAccountID.First().Value; // TPO DOC CLIENTE

                #region DIRECCION CLIENTE

                string dir = inv.AccountingCustomerParty.Party.PostalAddress.StreetName.Value;

                if (dir == null || dir == string.Empty)
                { oFactura.Cliente.Direccion = string.Empty; }
                else
                { oFactura.Cliente.Direccion = string.Empty + dir; }

                #region concatena direccion cliente
                string depart, prov, distr, urb;

                urb = inv.AccountingCustomerParty.Party.PostalAddress.CitySubdivisionName.Value;
                depart = inv.AccountingCustomerParty.Party.PostalAddress.CityName.Value;
                prov = inv.AccountingCustomerParty.Party.PostalAddress.CountrySubentity.Value;
                distr = inv.AccountingCustomerParty.Party.PostalAddress.District.Value;

                if (urb == null || urb == string.Empty) { }
                else
                { oFactura.Campo10 += " " + urb; }

                if (depart == null || depart == string.Empty) { }
                else
                { oFactura.Campo10 += ", " + depart; }

                if (prov == null || prov == string.Empty) { }
                else
                { oFactura.Campo10 += ", " + prov; }


                if (distr == null || distr == string.Empty) { }
                else
                { oFactura.Campo10 += ", " + distr; }


                oFactura.Campo10 = oFactura.Cliente.Direccion + oFactura.Campo10;

                #endregion end concatena direccion

                #endregion END DIRECCION CLIENTE

                oFactura.Cliente.RazonSocial = string.Empty + inv.AccountingCustomerParty.Party.PartyLegalEntity.First().RegistrationName.Value; //RAZON SOCIAL CLIENTE

                #region ORDEN DE COMPRA
                string orderRef = string.Empty;
                if (inv.OrderReference == null)
                { orderRef = string.Empty; }
                else
                {
                    if (inv.OrderReference.ID == null)
                    { orderRef = string.Empty; }
                    else
                    {
                        if (inv.OrderReference.ID.Value == null)
                        { orderRef = string.Empty; }
                        else
                        { orderRef = inv.OrderReference.ID.Value; }
                    }
                    if (orderRef.Length == Constantes.ValorCero)
                    { orderRef = string.Empty; }
                }
                oFactura.NroOrdCompra = orderRef;
                #endregion END ORDEN DE COMPRA

                //TOTALES
                if (inv.TaxTotal == null)
                {
                    oFactura.MontoIgvCad = string.Empty + 0.00m; //TOTAL IGV
                    oFactura.MontoTotalCad = string.Empty + 0.00m; //IMPORTE TOTAL
                    oFactura.MontoTotal = 0.00m;
                }
                else
                {
                    oFactura.MontoIgvCad = string.Empty + inv.TaxTotal.First().TaxAmount.Value; //TOTAL IGV
                    oFactura.MontoIGV = inv.TaxTotal.First().TaxAmount.Value; //TOTAL IGV


                    oFactura.MontoTotalCad = string.Empty + inv.LegalMonetaryTotal.PayableAmount.Value; //IMPORTE TOTAL
                    oFactura.MontoTotal = inv.LegalMonetaryTotal.PayableAmount.Value;
                }

                xmlnodelist = XMLArchive.GetElementsByTagName("sac:AdditionalMonetaryTotal"); //FOR SUB TOTAL AND OTHER TOTALES
                for (int i = 0; i <= xmlnodelist.Count - 1; i++)
                {
                    switch (xmlnodelist.Item(i).ChildNodes[0].InnerText)
                    {
                        #region CASE
                        //case "1001": { oFactura.TotalGravadoSinIGV = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        //case "1002": { oFactura.TotalInafecto = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "1003": { oFactura.TotalExonerado = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "1004": { oFactura.TotalGratuito = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "1005": { oFactura.SubTotalVenta = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "2001": { oFactura.TotalPercepciones = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "2002": { oFactura.TotalRetenciones = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "2003":
                        //    {
                        //        oFactura.TotalDetracciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;

                        //        oFactura.MontoDetraccion = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE"));
                        //        oFactura.PorcentDetraccion = xmlnodelist.Item(i).ChildNodes[2].InnerText;

                        //        break;
                        //    }
                        //case "2004": { oFactura.TotalBonificaciones = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "2005": { oFactura.TotalDescuento = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }

                        case "1001": { oFactura.TotalGravadoSinIGV = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "1002": { oFactura.TotalInafecto_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "1003": { oFactura.TotalExonerado_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "1004": { oFactura.TotalGratuito_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "1005": { oFactura.SubTotalVenta_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "2001": { oFactura.TotalPercepciones_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "2002": { oFactura.TotalRetenciones_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "2003":
                            {
                                oFactura.TotalDetracciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;

                                oFactura.MontoDetraccion = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE"));
                                oFactura.PorcentDetraccion = xmlnodelist.Item(i).ChildNodes[2].InnerText;

                                break;
                            }
                        case "2004": { oFactura.TotalBonificaciones_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "2005": { oFactura.TotalDescuento_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                            #endregion
                    }
                }

                if (oFactura.TotalDescuento == null || oFactura.TotalDescuento.Length == Constantes.ValorCero)
                    oFactura.TotalDescuento = Constantes.ValorCeroMonto;

                xmlnodelist = XMLArchive.GetElementsByTagName("DigestValue"); //VALOR RESUMEN 
                oFactura.ValorResumen = xmlnodelist.Item(0).InnerText.Length == 0 ? string.Empty : xmlnodelist.Item(0).InnerText; ;

                xmlnodelist = XMLArchive.GetElementsByTagName("SignatureValue"); //VALOR FIRMA
                oFactura.ValorFirma = xmlnodelist.Item(0).InnerText.Length == 0 ? string.Empty : xmlnodelist.Item(0).InnerText;

                #region total texto, detraccion

                xmlnodelist = XMLArchive.GetElementsByTagName("sac:AdditionalProperty");
                for (int j = 0; j <= xmlnodelist.Count - 1; j++)
                {
                    switch (xmlnodelist.Item(j).ChildNodes[0].InnerText)
                    {
                        case "1000": { oFactura.MontoTotalLetras = xmlnodelist.Item(j).ChildNodes[1].InnerText; break; }
                        case "3000": { oFactura.Detraccion.Codigo = xmlnodelist.Item(j).ChildNodes[1].InnerText; break; }
                        case "3001": { oFactura.Detraccion.CtaDetraccion = xmlnodelist.Item(j).ChildNodes[1].InnerText; break; }
                    }
                }

                if (inv.LegalMonetaryTotal == null)
                { oFactura.TotOtrosCargos = 0.00m; }
                else
                {
                    if (inv.LegalMonetaryTotal.ChargeTotalAmount == null)
                    { oFactura.TotOtrosCargos = 0.00m; }
                    else if (inv.LegalMonetaryTotal.ChargeTotalAmount != null)
                    {
                        oFactura.TotOtrosCargos = Convert.ToDecimal(inv.LegalMonetaryTotal.ChargeTotalAmount.Value, CultureInfo.CreateSpecificCulture("es-PE"));
                    }
                }

                #endregion


                oFactura.ListaDetalleFacturaElectronica = new WCFFacturacion.ListaDetalleFacturaElectronica();
                foreach (var line in inv.InvoiceLine)
                {
                    oFactura.DetalleFacturaElectronica = new WCFFacturacion.DetalleFacturaElectronica();


                    oFactura.DetalleFacturaElectronica.NroOrden = int.Parse(line.ID.Value); //NRO DE ORDEN
                    oFactura.DetalleFacturaElectronica.Cantidad = line.InvoicedQuantity.Value; //CANTIDAD
                    oFactura.DetalleFacturaElectronica.ValorVenta = line.LineExtensionAmount.Value; //VALOR VENTA

                    var ListtypeCodeDet = line.PricingReference.AlternativeConditionPrice.ToList();
                    if (ListtypeCodeDet.Count == 2)
                    {
                        oFactura.DetalleFacturaElectronica.PrecioVenta = line.PricingReference.AlternativeConditionPrice[1].PriceAmount.Value; //PRECIO VENTA
                    }
                    else
                    {
                        oFactura.DetalleFacturaElectronica.PrecioVenta = line.PricingReference.AlternativeConditionPrice.First().PriceAmount.Value; //PRECIO VENTA
                    }

                    oFactura.DetalleFacturaElectronica.SubTotalTexto = string.Empty + oFactura.DetalleFacturaElectronica.PrecioVenta * oFactura.DetalleFacturaElectronica.Cantidad; // SUB TOTAL X DETALLE

                    oFactura.DetalleFacturaElectronica.IGV = line.TaxTotal.First().TaxAmount.Value; // TOTAL IGV * DETALLE
                    oFactura.DetalleFacturaElectronica.ValorUnitario = line.Price.PriceAmount.Value; //VALOR UNITARIO

                    var detail_description = line.Item.Description.First().Value;
                    detail_description = detail_description.Replace(";;", "\n");
                    oFactura.DetalleFacturaElectronica.Descripcion = detail_description;

                    oFactura.DetalleFacturaElectronica.CodigoProducto = line.Item.SellersItemIdentification.ID.Value; //CODIGO PRODUCTO
                    oFactura.DetalleFacturaElectronica.Unidad = line.InvoicedQuantity.unitCode.ToString();// UNIDAD PRODUCTO

                    oFactura.DetalleFacturaElectronica.Unidad = Equivalentes.Instance.Get_Equivalente(oFactura.DetalleFacturaElectronica.Unidad);

                    oFactura.DetalleFacturaElectronica.NombreArchivoXML = oFactura.NombreArchivoXML;

                    oFactura.DetalleFacturaElectronica.NumeroSerie = oFactura.NumeroSerie;
                    oFactura.DetalleFacturaElectronica.NumeroDocumento = oFactura.NumeroDocumento;

                    oFactura.DetalleFacturaElectronica.Importe = line.LineExtensionAmount.Value;

                    if (line.AllowanceCharge != null)
                    { oFactura.DetalleFacturaElectronica.Dscto = line.AllowanceCharge.First().Amount.Value; }
                    else
                    { oFactura.DetalleFacturaElectronica.Dscto = 0.00m; }

                    var moneda = inv.DocumentCurrencyCode.Value;
                    switch (moneda)
                    {
                        case "PEN": { oFactura.DetalleFacturaElectronica.Simbolo = "S/"; break; }
                        case "USD": { oFactura.DetalleFacturaElectronica.Simbolo = "US $"; break; }
                    }

                    oFactura.ListaDetalleFacturaElectronica.Add(oFactura.DetalleFacturaElectronica);
                }
                xmlnodelist = XMLArchive.GetElementsByTagName("Extra");

                oFactura.ListaExtra = new WCFFacturacion.ListaExtra();
                for (int i = 0; i <= xmlnodelist.Count - 1; i++)
                {
                    oFactura.Extra = new WCFFacturacion.Extra();
                    oFactura.Extra.ExLinea = xmlnodelist.Item(i).ChildNodes[0].InnerText;
                    oFactura.Extra.ExDato = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                    oFactura.Extra.ExTipo = xmlnodelist.Item(i).ChildNodes[2].InnerText;
                    oFactura.ListaExtra.Add(oFactura.Extra);
                }
                #endregion
            }
            catch (Exception ex) { return oFactura; }
            return oFactura;
        }

        #endregion END CE

        #region NOTE CREDIT

        public WCFFacturacion.FacturaElectronica GetDataFromCNOTECredit(xmlNotCred.CreditNoteType nc, XmlDocument XMLArchive, string NUM_CE, string TPO_CE)
        {
            oFactura = new WCFFacturacion.FacturaElectronica();

            Inicializa();

            XmlNodeList xmlnodelist;

            try
            {
                #region READ XML

                oFactura.MontoTotalCad = "0.00";
                oFactura.MontoIgvCad = "0.00";
                oFactura.TotalDescuento = "0.00";
                oFactura.TotalExonerado = "0.00";
                oFactura.TotalInafecto = "0.00";
                oFactura.TotalGravado = "0.00";
                oFactura.TotalDetracciones = "0.00";
                oFactura.NroOrdCompra = string.Empty;

                oFactura.NombreArchivoXML = NUM_CE;

                oFactura.CodTpoNC = nc.DiscrepancyResponse.First().ResponseCode.Value;
                oFactura.DescTpoNC = Singleton.Instance.Get_Desc_TypeNC(oFactura.CodTpoNC);

                oFactura.SerieCorrelativo = nc.ID.Value;

                string[] array = oFactura.SerieCorrelativo.Split('-');

                oFactura.NumeroSerie = array[0];
                oFactura.NumeroDocumento = array[1];

                oFactura.FechaEmision2 = nc.IssueDate.Value.ToString("yyyy-MM-dd");//FECHA DEL DOCUMENTO
                oFactura.FechaEmision = nc.IssueDate.Value;//FECHA EMISION

                oFactura.TipoDocumento.CodigoDocumento = TPO_CE;//TIPO DOCUMENTO
                oFactura.TipoMoneda = nc.DocumentCurrencyCode.Value;// TIPO MONEDA

                switch (oFactura.TipoMoneda)
                {
                    case "USD": { oFactura.TipoMoneda = "DÓLARES AMERICANOS"; break; }
                    case "PEN": { oFactura.TipoMoneda = "SOLES"; break; }
                }
                oFactura.Empresa.RUC = nc.AccountingSupplierParty.CustomerAssignedAccountID.Value;
                oFactura.Empresa.TipoDocumentiIdentidad.Codigo = nc.AccountingSupplierParty.AdditionalAccountID.First().Value;

                oFactura.Empresa.RazonSocial = nc.AccountingSupplierParty.Party.PartyLegalEntity.ToList().First().RegistrationName.Value; // RAZON SOCIAL EMISOR
                oFactura.Empresa.RazonComercial = nc.AccountingSupplierParty.Party.PartyName.First().Name.Value; //RAZON COMERCIAL

                oFactura.Empresa.CodigoUbigeo = nc.AccountingSupplierParty.Party.PostalAddress.ID.Value;
                oFactura.Empresa.Direccion = nc.AccountingSupplierParty.Party.PostalAddress.StreetName.Value;

                oFactura.Cliente.ClienteRuc = string.Empty + nc.AccountingCustomerParty.CustomerAssignedAccountID.Value; //NRO DOC CLIENTE
                oFactura.Cliente.TipoDocumentoIdentidad.Codigo = nc.AccountingCustomerParty.AdditionalAccountID.First().Value;

                #region DIRECCION CLIENTE
                string dir = nc.AccountingCustomerParty.Party.PostalAddress.StreetName.Value;
                if (dir == null || dir == string.Empty)
                { oFactura.Cliente.Direccion = string.Empty; }
                else
                { oFactura.Cliente.Direccion = string.Empty + dir; }

                #endregion

                oFactura.Cliente.RazonSocial = string.Empty + nc.AccountingCustomerParty.Party.PartyLegalEntity.First().RegistrationName.Value; //RAZON SOCIAL CLIENTE

                #region ORDEN DE COMPRA
                string orderRef = string.Empty;
                if (nc.OrderReference == null)
                { orderRef = string.Empty; }
                else
                {
                    if (nc.OrderReference.ID == null)
                    { orderRef = string.Empty; }
                    else
                    {
                        if (nc.OrderReference.ID.Value == null)
                        { orderRef = string.Empty; }
                        else
                        { orderRef = nc.OrderReference.ID.Value; }
                    }
                    if (orderRef.Length == Constantes.ValorCero)
                    { orderRef = string.Empty; }
                }
                oFactura.NroOrdCompra = orderRef;
                #endregion END ORDEN DE COMPRA

                #region TOTALES
                xmlnodelist = XMLArchive.GetElementsByTagName("sac:AdditionalMonetaryTotal"); //FOR SUB TOTAL AND OTHER TOTALES
                for (int i = 0; i <= xmlnodelist.Count - 1; i++)
                {
                    switch (xmlnodelist.Item(i).ChildNodes[0].InnerText)
                    {
                        //#region CASE
                        //case "1001": { oFactura.TotalGravadoSinIGV = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        //case "1002": { oFactura.TotalInafecto = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "1003": { oFactura.TotalExonerado = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "1004": { oFactura.TotalGratuito = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "1005": { oFactura.SubTotalVenta = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "2001": { oFactura.TotalPercepciones = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "2002": { oFactura.TotalRetenciones = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "2003": { oFactura.TotalDetracciones = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "2004": { oFactura.TotalBonificaciones = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "2005": { oFactura.TotalDescuento = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //#endregion

                        #region CASE
                        case "1001": { oFactura.TotalGravadoSinIGV = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "1002": { oFactura.TotalInafecto_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "1003": { oFactura.TotalExonerado_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "1004": { oFactura.TotalGratuito_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "1005": { oFactura.SubTotalVenta_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "2001": { oFactura.TotalPercepciones_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "2002": { oFactura.TotalRetenciones_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "2003": { oFactura.TotalDetracciones_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "2004": { oFactura.TotalBonificaciones_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "2005": { oFactura.TotalDescuento_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        #endregion
                    }
                }
                #endregion

                #region OTHER TOTALES

                if (nc.TaxTotal == null)
                {
                    oFactura.MontoIgvCad = string.Empty + 0.00m; //TOTAL IGV
                }
                else
                {
                    oFactura.MontoIgvCad = string.Empty + nc.TaxTotal.First().TaxAmount.Value; //TOTAL IGV
                    oFactura.MontoIGV = nc.TaxTotal.First().TaxAmount.Value; //TOTAL IGV
                }

                if (nc.LegalMonetaryTotal != null)
                {
                    oFactura.MontoTotalCad = string.Empty + nc.LegalMonetaryTotal.PayableAmount.Value; //IMPORTE TOTAL
                    oFactura.MontoTotal = nc.LegalMonetaryTotal.PayableAmount.Value;
                }
                else
                {
                    oFactura.MontoTotalCad = string.Empty + 0.00m; //IMPORTE TOTAL
                    oFactura.MontoTotal = 0.00m;
                }

                if (nc.TaxTotal == null)
                {
                    oFactura.MontoIGV = 0.00m;
                    oFactura.MontoIGVText = 0.00m + string.Empty;
                    oFactura.MontoISC = 0.00m;
                    oFactura.MontoISCText = 0.00m + string.Empty;
                    oFactura.MontoOtros = 0.00m;
                    oFactura.MontoOtrosText = 0.00m + string.Empty;
                }
                else
                {
                    #region

                    for (int j = 0; j <= nc.TaxTotal.ToList().Count - 1; j++)
                    {
                        switch (nc.TaxTotal[j].TaxSubtotal.First().TaxCategory.TaxScheme.ID.Value)
                        {
                            case "1000":
                                {
                                    oFactura.MontoIGV = nc.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value;
                                    oFactura.MontoIGV = nc.TaxTotal[j].TaxAmount.Value;
                                    oFactura.MontoIGVText = nc.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value + string.Empty;
                                    oFactura.MontoIGVText = nc.TaxTotal[j].TaxAmount.Value + string.Empty;
                                    break;
                                }
                            case "2000":
                                {
                                    oFactura.MontoISC = nc.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value;
                                    oFactura.MontoISC = nc.TaxTotal[j].TaxAmount.Value;
                                    oFactura.MontoISCText = nc.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value + string.Empty;
                                    oFactura.MontoISCText = nc.TaxTotal[j].TaxAmount.Value + string.Empty;
                                    break;
                                }
                            case "9999":
                                {
                                    oFactura.MontoOtros = nc.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value;
                                    oFactura.MontoOtros = nc.TaxTotal[j].TaxAmount.Value;
                                    oFactura.MontoOtrosText = nc.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value + string.Empty;
                                    oFactura.MontoOtrosText = nc.TaxTotal[j].TaxAmount.Value + string.Empty;
                                    break;
                                }
                        }
                    }
                    #endregion
                }

                if (oFactura.TotalDescuento == null || oFactura.TotalDescuento.Length == Constantes.ValorCero)
                    oFactura.TotalDescuento = Constantes.ValorCeroMonto;


                if (nc.LegalMonetaryTotal == null)
                { oFactura.TotOtrosCargos = 0.00m; }
                else
                {
                    if (nc.LegalMonetaryTotal.ChargeTotalAmount == null)
                    { oFactura.TotOtrosCargos = 0.00m; }
                    else if (nc.LegalMonetaryTotal.ChargeTotalAmount != null)
                    {
                        oFactura.TotOtrosCargos = Convert.ToDecimal(nc.LegalMonetaryTotal.ChargeTotalAmount.Value, CultureInfo.CreateSpecificCulture("es-PE"));
                    }
                }

                #endregion END TOTALES

                xmlnodelist = XMLArchive.GetElementsByTagName("DigestValue"); //VALOR RESUMEN 
                oFactura.ValorResumen = xmlnodelist.Item(0).InnerText.Length == 0 ? string.Empty : xmlnodelist.Item(0).InnerText; ;

                xmlnodelist = XMLArchive.GetElementsByTagName("SignatureValue"); //VALOR FIRMA
                oFactura.ValorFirma = xmlnodelist.Item(0).InnerText.Length == 0 ? string.Empty : xmlnodelist.Item(0).InnerText;

                xmlnodelist = XMLArchive.GetElementsByTagName("sac:AdditionalProperty"); // MONTO LITERAL
                oFactura.MontoTotalLetras = xmlnodelist.Item(0).ChildNodes[1].InnerText;

                #region DETALLE
                oFactura.ListaDetalleFacturaElectronica = new WCFFacturacion.ListaDetalleFacturaElectronica();

                foreach (var line in nc.CreditNoteLine)
                {
                    oFactura.DetalleFacturaElectronica = new WCFFacturacion.DetalleFacturaElectronica();
                    oFactura.DetalleFacturaElectronica.NroOrden = int.Parse(line.ID.Value);

                    oFactura.DetalleFacturaElectronica.Unidad = line.CreditedQuantity.unitCode + string.Empty;
                    oFactura.DetalleFacturaElectronica.Cantidad = line.CreditedQuantity.Value;

                    oFactura.DetalleFacturaElectronica.SubTotal = line.LineExtensionAmount.Value; //SUB TOTAL SIN IGV

                    var ListtypeCodeDet = line.PricingReference.AlternativeConditionPrice.ToList();
                    if (ListtypeCodeDet.Count == 2)
                    {
                        oFactura.DetalleFacturaElectronica.PrecioVenta = line.PricingReference.AlternativeConditionPrice[1].PriceAmount.Value; //PRECIO VENTA
                    }
                    else
                    {
                        oFactura.DetalleFacturaElectronica.PrecioVenta = line.PricingReference.AlternativeConditionPrice.First().PriceAmount.Value; //PRECIO VENTA
                    }

                    oFactura.DetalleFacturaElectronica.SubTotalTexto = string.Empty + decimal.Round(oFactura.DetalleFacturaElectronica.PrecioVenta * oFactura.DetalleFacturaElectronica.Cantidad, 2); //SUB TOTAL + IGV 

                    oFactura.DetalleFacturaElectronica.DetMontoIGV = line.TaxTotal.First().TaxAmount.Value; // TOTAL IGV 

                    oFactura.DetalleFacturaElectronica.ValorUnitario = line.Price.PriceAmount.Value;

                    var detail_description = line.Item.Description.First().Value;
                    detail_description = detail_description.Replace(";;", "\n");
                    oFactura.DetalleFacturaElectronica.Descripcion = detail_description;

                    oFactura.DetalleFacturaElectronica.CodigoProducto = line.Item.SellersItemIdentification.ID.Value;

                    oFactura.DetalleFacturaElectronica.Importe = line.LineExtensionAmount.Value;

                    oFactura.DetalleFacturaElectronica.Unidad = Equivalentes.Instance.Get_Equivalente(oFactura.DetalleFacturaElectronica.Unidad);

                    oFactura.DetalleFacturaElectronica.NombreArchivoXML = oFactura.NombreArchivoXML;

                    oFactura.DetalleFacturaElectronica.NumeroSerie = oFactura.NumeroSerie;
                    oFactura.DetalleFacturaElectronica.NumeroDocumento = oFactura.NumeroDocumento;

                    var moneda = nc.DocumentCurrencyCode.Value;
                    switch (moneda)
                    {
                        case "PEN": { oFactura.DetalleFacturaElectronica.Simbolo = "S/"; break; }
                        case "USD": { oFactura.DetalleFacturaElectronica.Simbolo = "US $"; break; }
                    }

                    oFactura.ListaDetalleFacturaElectronica.Add(oFactura.DetalleFacturaElectronica);
                }
                #endregion END DETALLE

                #region DOCUMENTO AFECTADO
                oFactura.ListaAfectado = new WCFFacturacion.ListaAfectado();
                foreach (var afec in nc.BillingReference)
                {
                    oFactura.Afectado = new WCFFacturacion.Afectado();

                    oFactura.Afectado.ID = afec.InvoiceDocumentReference.ID.Value;
                    oFactura.Afectado.CodigoTipoDocumento = afec.InvoiceDocumentReference.DocumentTypeCode.Value;
                    oFactura.ListaAfectado.Add(oFactura.Afectado);
                }
                #endregion

                oFactura.MotivoAnulado = nc.DiscrepancyResponse.First().Description.First().Value;

                xmlnodelist = XMLArchive.GetElementsByTagName("Extra");

                oFactura.ListaExtra = new WCFFacturacion.ListaExtra();
                for (int i = 0; i <= xmlnodelist.Count - 1; i++)
                {
                    oFactura.Extra = new WCFFacturacion.Extra();
                    oFactura.Extra.ExLinea = xmlnodelist.Item(i).ChildNodes[0].InnerText;
                    oFactura.Extra.ExDato = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                    oFactura.Extra.ExTipo = xmlnodelist.Item(i).ChildNodes[2].InnerText;
                    oFactura.ListaExtra.Add(oFactura.Extra);
                }

                #endregion
            }
            catch (Exception ex) { return oFactura; }

            return oFactura;
        }

        #endregion END NOTE CREDIT

        #region NOTE DEBIT

        public WCFFacturacion.FacturaElectronica GetDataFromCNOTEDebit(xmlNotDeb.DebitNoteType nd, XmlDocument XMLArchive, string NUM_CE, string TPO_CE)
        {
            oFactura = new WCFFacturacion.FacturaElectronica();
            Inicializa();
            XmlNodeList xmlnodelist;

            try
            {
                #region READ XML

                oFactura.MontoTotalCad = "0.00";
                oFactura.MontoIgvCad = "0.00";
                oFactura.TotalDescuento = "0.00";
                oFactura.TotalExonerado = "0.00";
                oFactura.TotalInafecto = "0.00";
                oFactura.TotalGravado = "0.00";
                oFactura.TotalDetracciones = "0.00";
                oFactura.NroOrdCompra = string.Empty;


                oFactura.NombreArchivoXML = NUM_CE;

                oFactura.CodTpoND = nd.DiscrepancyResponse.First().ResponseCode.Value;
                oFactura.DescTpoND = Singleton.Instance.Get_Desc_TypeND(oFactura.CodTpoND);

                oFactura.SerieCorrelativo = nd.ID.Value;

                string[] array = oFactura.SerieCorrelativo.Split('-');

                oFactura.NumeroSerie = array[0];
                oFactura.NumeroDocumento = array[1];

                oFactura.FechaEmision2 = nd.IssueDate.Value.ToString("yyyy-MM-dd");//FECHA DEL DOCUMENTO
                oFactura.FechaEmision = nd.IssueDate.Value;//FECHA EMISION

                oFactura.TipoDocumento.CodigoDocumento = TPO_CE;//TIPO DOCUMENTO
                oFactura.TipoMoneda = nd.DocumentCurrencyCode.Value;// TIPO MONEDA

                switch (oFactura.TipoMoneda)
                {
                    case "USD": { oFactura.TipoMoneda = "DÓLARES AMERICANOS"; break; }
                    case "PEN": { oFactura.TipoMoneda = "SOLES"; break; }
                }

                oFactura.Empresa.RUC = nd.AccountingSupplierParty.CustomerAssignedAccountID.Value;
                oFactura.Empresa.TipoDocumentiIdentidad.Codigo = nd.AccountingSupplierParty.AdditionalAccountID.First().Value;

                oFactura.Empresa.RazonSocial = nd.AccountingSupplierParty.Party.PartyLegalEntity.ToList().First().RegistrationName.Value; // RAZON SOCIAL EMISOR
                oFactura.Empresa.RazonComercial = nd.AccountingSupplierParty.Party.PartyName.First().Name.Value; //RAZON COMERCIAL

                oFactura.Empresa.CodigoUbigeo = nd.AccountingSupplierParty.Party.PostalAddress.ID.Value;
                oFactura.Empresa.Direccion = nd.AccountingSupplierParty.Party.PostalAddress.StreetName.Value;

                oFactura.Cliente.ClienteRuc = string.Empty + nd.AccountingCustomerParty.CustomerAssignedAccountID.Value; //NRO DOC CLIENTE
                oFactura.Cliente.TipoDocumentoIdentidad.Codigo = nd.AccountingCustomerParty.AdditionalAccountID.First().Value;

                #region DIRECCION CLIENTE
                string dir = nd.AccountingCustomerParty.Party.PostalAddress.StreetName.Value;
                if (dir == null || dir == string.Empty)
                { oFactura.Cliente.Direccion = string.Empty; }
                else
                { oFactura.Cliente.Direccion = string.Empty + dir; }
                #endregion

                oFactura.Cliente.RazonSocial = string.Empty + nd.AccountingCustomerParty.Party.PartyLegalEntity.First().RegistrationName.Value; //RAZON SOCIAL CLIENTE

                #region ORDEN DE COMPRA
                string orderRef = string.Empty;
                if (nd.OrderReference == null)
                { orderRef = string.Empty; }
                else
                {
                    if (nd.OrderReference.ID == null)
                    { orderRef = string.Empty; }
                    else
                    {
                        if (nd.OrderReference.ID.Value == null)
                        { orderRef = string.Empty; }
                        else
                        { orderRef = nd.OrderReference.ID.Value; }
                    }
                    if (orderRef.Length == Constantes.ValorCero)
                    { orderRef = string.Empty; }
                }
                oFactura.NroOrdCompra = orderRef;
                #endregion END ORDEN DE COMPRA

                #region TOTALES
                xmlnodelist = XMLArchive.GetElementsByTagName("sac:AdditionalMonetaryTotal"); //FOR SUB TOTAL AND OTHER TOTALES
                for (int i = 0; i <= xmlnodelist.Count - 1; i++)
                {
                    switch (xmlnodelist.Item(i).ChildNodes[0].InnerText)
                    {
                        //#region CASE
                        //case "1001": { oFactura.TotalGravadoSinIGV = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        //case "1002": { oFactura.TotalInafecto = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "1003": { oFactura.TotalExonerado = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "1004": { oFactura.TotalGratuito = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "1005": { oFactura.SubTotalVenta = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "2001": { oFactura.TotalPercepciones = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "2002": { oFactura.TotalRetenciones = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "2003":
                        //    {
                        //        oFactura.TotalDetracciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                        //        oFactura.MontoDetraccion = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE"));
                        //        oFactura.PorcentDetraccion = xmlnodelist.Item(i).ChildNodes[2].InnerText;
                        //        break;
                        //    }
                        //case "2004": { oFactura.TotalBonificaciones = xmlnodelist.Item(i).ChildNodes[1].InnerText; break; }
                        //case "2005": { oFactura.TotalDescuento = xmlnodelist.Item(i).ChildNodes[1].InnerText; ; break; }
                        //    #endregion


                        #region CASE
                        case "1001": { oFactura.TotalGravadoSinIGV = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "1002": { oFactura.TotalInafecto_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "1003": { oFactura.TotalExonerado_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "1004": { oFactura.TotalGratuito_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "1005": { oFactura.SubTotalVenta_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "2001": { oFactura.TotalPercepciones_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "2002": { oFactura.TotalRetenciones_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "2003":
                            {
                                oFactura.TotalDetracciones_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE"));
                                oFactura.MontoDetraccion = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE"));
                                oFactura.PorcentDetraccion = xmlnodelist.Item(i).ChildNodes[2].InnerText;
                                break;
                            }
                        case "2004": { oFactura.TotalBonificaciones_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                        case "2005": { oFactura.TotalDescuento_mon = Convert.ToDecimal(xmlnodelist.Item(i).ChildNodes[1].InnerText, CultureInfo.CreateSpecificCulture("es-PE")); break; }
                            #endregion
                    }
                }
                #endregion

                #region OTHER TOTALES

                if (nd.TaxTotal == null)
                {
                    oFactura.MontoIgvCad = string.Empty + 0.00m; //TOTAL IGV
                    oFactura.MontoIGV = 0.00m; //TOTAL IGV
                }
                else
                {
                    oFactura.MontoIgvCad = string.Empty + nd.TaxTotal.First().TaxAmount.Value; //TOTAL IGV
                    oFactura.MontoIGV = nd.TaxTotal.First().TaxAmount.Value; //TOTAL IGV
                }

                if (nd.RequestedMonetaryTotal == null)
                {
                    oFactura.MontoTotalCad = string.Empty + 0.00m;
                    oFactura.MontoTotal = 0.00m;
                }
                else
                {
                    oFactura.MontoTotalCad = string.Empty + nd.RequestedMonetaryTotal.PayableAmount.Value;
                    oFactura.MontoTotal = nd.RequestedMonetaryTotal.PayableAmount.Value;
                }


                if (nd.TaxTotal == null) { }
                else
                {
                    for (int j = 0; j <= nd.TaxTotal.ToList().Count - 1; j++)
                    {
                        switch (nd.TaxTotal[j].TaxSubtotal.First().TaxCategory.TaxScheme.ID.Value)
                        {
                            case "1000":
                                {
                                    oFactura.MontoIGV = nd.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value;
                                    oFactura.MontoIGV = nd.TaxTotal[j].TaxAmount.Value;
                                    oFactura.MontoIGVText = nd.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value + string.Empty;
                                    oFactura.MontoIGVText = nd.TaxTotal[j].TaxAmount.Value + string.Empty;
                                    break;
                                }
                            case "2000":
                                {
                                    oFactura.MontoISC = nd.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value;
                                    oFactura.MontoISC = nd.TaxTotal[j].TaxAmount.Value;
                                    oFactura.MontoISCText = nd.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value + string.Empty;
                                    oFactura.MontoISCText = nd.TaxTotal[j].TaxAmount.Value + string.Empty;
                                    break;
                                }
                            case "9999":
                                {
                                    oFactura.MontoOtros = nd.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value;
                                    oFactura.MontoOtros = nd.TaxTotal[j].TaxAmount.Value;
                                    oFactura.MontoOtrosText = nd.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value + string.Empty;
                                    oFactura.MontoOtrosText = nd.TaxTotal[j].TaxAmount.Value + string.Empty;
                                    break;
                                }

                        }
                    }
                }



                if (oFactura.TotalDescuento == null || oFactura.TotalDescuento.Length == Constantes.ValorCero)
                    oFactura.TotalDescuento = Constantes.ValorCeroMonto;

                if (nd.RequestedMonetaryTotal == null)
                { oFactura.TotOtrosCargos = 0.00m; }
                else
                {
                    if (nd.RequestedMonetaryTotal.ChargeTotalAmount == null)
                    { oFactura.TotOtrosCargos = 0.00m; }
                    else if (nd.RequestedMonetaryTotal.ChargeTotalAmount != null)
                    {
                        oFactura.TotOtrosCargos = Convert.ToDecimal(nd.RequestedMonetaryTotal.ChargeTotalAmount.Value, CultureInfo.CreateSpecificCulture("es-PE"));
                    }
                }

                #endregion END TOTALES

                xmlnodelist = XMLArchive.GetElementsByTagName("DigestValue"); //VALOR RESUMEN 
                oFactura.ValorResumen = xmlnodelist.Item(0).InnerText.Length == 0 ? string.Empty : xmlnodelist.Item(0).InnerText; ;

                xmlnodelist = XMLArchive.GetElementsByTagName("SignatureValue"); //VALOR FIRMA
                oFactura.ValorFirma = xmlnodelist.Item(0).InnerText.Length == 0 ? string.Empty : xmlnodelist.Item(0).InnerText;

                #region total texto, detraccion

                xmlnodelist = XMLArchive.GetElementsByTagName("sac:AdditionalProperty");
                for (int j = 0; j <= xmlnodelist.Count - 1; j++)
                {
                    switch (xmlnodelist.Item(j).ChildNodes[0].InnerText)
                    {
                        case "1000": { oFactura.MontoTotalLetras = xmlnodelist.Item(j).ChildNodes[1].InnerText; break; }
                        case "3000": { oFactura.Detraccion.Codigo = xmlnodelist.Item(j).ChildNodes[1].InnerText; break; }
                        case "3001": { oFactura.Detraccion.CtaDetraccion = xmlnodelist.Item(j).ChildNodes[1].InnerText; break; }
                    }
                }
                #endregion



                #region DETALLE

                oFactura.ListaDetalleFacturaElectronica = new WCFFacturacion.ListaDetalleFacturaElectronica();

                foreach (var line in nd.DebitNoteLine)
                {
                    oFactura.DetalleFacturaElectronica = new WCFFacturacion.DetalleFacturaElectronica();
                    oFactura.DetalleFacturaElectronica.NroOrden = int.Parse(line.ID.Value);

                    oFactura.DetalleFacturaElectronica.Unidad = line.DebitedQuantity.unitCode + string.Empty;
                    oFactura.DetalleFacturaElectronica.Cantidad = line.DebitedQuantity.Value;

                    oFactura.DetalleFacturaElectronica.SubTotal = line.LineExtensionAmount.Value; //SUB TOTAL SIN IGV

                    var ListtypeCodeDet = line.PricingReference.AlternativeConditionPrice.ToList();
                    if (ListtypeCodeDet.Count == 2)
                    {
                        oFactura.DetalleFacturaElectronica.PrecioVenta = line.PricingReference.AlternativeConditionPrice[1].PriceAmount.Value; //PRECIO VENTA
                    }
                    else
                    {
                        oFactura.DetalleFacturaElectronica.PrecioVenta = line.PricingReference.AlternativeConditionPrice.First().PriceAmount.Value; //PRECIO VENTA
                    }

                    oFactura.DetalleFacturaElectronica.SubTotalTexto = string.Empty + decimal.Round(oFactura.DetalleFacturaElectronica.PrecioVenta * oFactura.DetalleFacturaElectronica.Cantidad, 2); //SUB TOTAL + IGV 

                    oFactura.DetalleFacturaElectronica.DetMontoIGV = line.TaxTotal.First().TaxAmount.Value; // TOTAL IGV 

                    oFactura.DetalleFacturaElectronica.ValorUnitario = line.Price.PriceAmount.Value;

                    var detail_description = line.Item.Description.First().Value;
                    detail_description = detail_description.Replace(";;", "\n");
                    oFactura.DetalleFacturaElectronica.Descripcion = detail_description;

                    oFactura.DetalleFacturaElectronica.CodigoProducto = line.Item.SellersItemIdentification.ID.Value;

                    oFactura.DetalleFacturaElectronica.Importe = line.LineExtensionAmount.Value;

                    oFactura.DetalleFacturaElectronica.Unidad = Equivalentes.Instance.Get_Equivalente(oFactura.DetalleFacturaElectronica.Unidad);

                    oFactura.DetalleFacturaElectronica.NombreArchivoXML = oFactura.NombreArchivoXML;

                    oFactura.DetalleFacturaElectronica.NumeroSerie = oFactura.NumeroSerie;
                    oFactura.DetalleFacturaElectronica.NumeroDocumento = oFactura.NumeroDocumento;

                    var moneda = nd.DocumentCurrencyCode.Value;
                    switch (moneda)
                    {
                        case "PEN": { oFactura.DetalleFacturaElectronica.Simbolo = "S/"; break; }
                        case "USD": { oFactura.DetalleFacturaElectronica.Simbolo = "US $"; break; }
                    }

                    oFactura.ListaDetalleFacturaElectronica.Add(oFactura.DetalleFacturaElectronica);
                }

                #endregion END DETALLE

                #region DOCUMENTO AFECTADO
                oFactura.ListaAfectado = new WCFFacturacion.ListaAfectado();
                foreach (var afec in nd.BillingReference)
                {
                    oFactura.Afectado = new WCFFacturacion.Afectado();
                    oFactura.Afectado.ID = afec.InvoiceDocumentReference.ID.Value;
                    oFactura.Afectado.CodigoTipoDocumento = afec.InvoiceDocumentReference.DocumentTypeCode.Value;
                    oFactura.ListaAfectado.Add(oFactura.Afectado);
                }
                #endregion

                oFactura.MotivoAnulado = nd.DiscrepancyResponse.First().Description.First().Value;

                xmlnodelist = XMLArchive.GetElementsByTagName("Extra");

                oFactura.ListaExtra = new WCFFacturacion.ListaExtra();
                for (int i = 0; i <= xmlnodelist.Count - 1; i++)
                {
                    oFactura.Extra = new WCFFacturacion.Extra();
                    oFactura.Extra.ExLinea = xmlnodelist.Item(i).ChildNodes[0].InnerText;
                    oFactura.Extra.ExDato = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                    oFactura.Extra.ExTipo = xmlnodelist.Item(i).ChildNodes[2].InnerText;
                    oFactura.ListaExtra.Add(oFactura.Extra);
                }

                #endregion
            }
            catch (Exception ex) { return oFactura; }

            return oFactura;
        }


        #endregion END NOTE DEBIT

        #region CRE

        public WCFFacturacion.FacturaElectronica GetDataFromXMLCRE(xmlReten.RetentionType ret, XmlDocument XMLArchive, string NUM_CE, string TPO_CE)
        {
            oFactura = new WCFFacturacion.FacturaElectronica();
            Inicializa();
            XmlNodeList xmlnodelist;

            try
            {
                #region READ

                oFactura.NombreArchivoXML = NUM_CE;
                oFactura.SerieCorrelativo = ret.ID.Value; // SERIE Y CORRELATIVO
                oFactura.FechaEmision2 = ret.IssueDate.Value.ToString("yyyy-MM-dd"); // FECHA DE DOCUMENTO
                oFactura.FechaEmision = ret.IssueDate.Value;

                oFactura.TipoDocumento.CodigoDocumento = TPO_CE; //TIPO CE

                string[] array = oFactura.SerieCorrelativo.Split('-');
                oFactura.NumeroSerie = array[0]; // SERIE 
                oFactura.NumeroDocumento = array[1]; // CORRELATIVO

                oFactura.Empresa.TipoDocumentiIdentidad.Codigo = ret.AgentParty.PartyIdentification.First().ID.schemeID; //TPO DOCUMENTO EMISOR
                oFactura.Empresa.RUC = ret.AgentParty.PartyIdentification.First().ID.Value; // NRO DOC EMISOR
                oFactura.Empresa.RazonComercial = ret.AgentParty.PartyName.First().Name.Value; // RAZON COMERCIAL EMISOR
                oFactura.Empresa.RazonSocial = ret.AgentParty.PartyLegalEntity.First().RegistrationName.Value; // RAZON SOCIAL EMISOR
                oFactura.Empresa.CodigoUbigeo = ret.AgentParty.PostalAddress.ID.Value; // COD UBIGEO EMISOR
                oFactura.Empresa.Direccion = ret.AgentParty.PostalAddress.StreetName.Value; // DIRECCION EMISOR

                oFactura.Cliente.TipoDocumentoIdentidad.Codigo = ret.ReceiverParty.PartyIdentification.First().ID.schemeID; // TPO DOC CLIENTE
                oFactura.Cliente.ClienteRuc = ret.ReceiverParty.PartyIdentification.First().ID.Value; // NRO DOC CLIENTE
                oFactura.Cliente.RazonSocial = ret.ReceiverParty.PartyLegalEntity.First().RegistrationName.Value; // RAZON SOCIAL CLIENTE
                oFactura.Cliente.Nombres = ret.ReceiverParty.PartyName.First().Name.Value; // RAZON COMERCIAL CLIENTE
                oFactura.Cliente.Direccion = ret.ReceiverParty.PostalAddress.StreetName.Value; // DIRECCION CLIENTE

                oFactura.Cliente.Departamento = ret.ReceiverParty.PostalAddress.CityName.Value;
                oFactura.Cliente.Provincia = ret.ReceiverParty.PostalAddress.CountrySubentity.Value;
                oFactura.Cliente.Distrito = ret.ReceiverParty.PostalAddress.District.Value;
                oFactura.Cliente.CodPais = ret.ReceiverParty.PostalAddress.Country.IdentificationCode.Value;

                oFactura.Cliente.Direccion += "";
                //oFactura.Cliente.Direccion += ", " + oFactura.Cliente.Departamento + ", " + oFactura.Cliente.Provincia + ", " + oFactura.Cliente.Distrito;

                oFactura.Nota = ret.Note.First().Value; // NOTAS DE LA RETENCION


                oFactura.DocCRECPE.MonedaImpTotalRetenido = string.Empty + ret.TotalInvoiceAmount.currencyID; //MONEDA IMPTOTRET
                oFactura.DocCRECPE.ImporteTotalRetenido = ret.TotalInvoiceAmount.Value; // IMP TOT RET
                oFactura.DocCRECPE.MonedaImpTotalPagado = string.Empty + ret.SUNATTotalPaid.currencyID; // MON TOT PAG
                oFactura.DocCRECPE.ImporteTotalPagado = ret.SUNATTotalPaid.Value; // IMP TOT PAG
                oFactura.DocCRECPE.TasaRetencion = string.Empty + ret.SUNATRetentionPercent.Value;


                oFactura.MonedaImpTotalRetenido = oFactura.DocCRECPE.MonedaImpTotalRetenido;
                oFactura.ImporteTotalRetenido = oFactura.DocCRECPE.ImporteTotalRetenido + string.Empty;
                oFactura.MonedaImpTotalPagado = oFactura.DocCRECPE.MonedaImpTotalPagado;
                oFactura.ImporteTotalPagado = oFactura.DocCRECPE.ImporteTotalPagado + string.Empty;
                oFactura.TasaRetencion = oFactura.DocCRECPE.TasaRetencion;

                xmlnodelist = XMLArchive.GetElementsByTagName("DigestValue"); //VALOR RESUMEN 
                oFactura.ValorResumen = xmlnodelist.Item(0).InnerText.Length == 0 ? string.Empty : xmlnodelist.Item(0).InnerText; ;

                xmlnodelist = XMLArchive.GetElementsByTagName("SignatureValue"); //VALOR FIRMA
                oFactura.ValorFirma = xmlnodelist.Item(0).InnerText.Length == 0 ? string.Empty : xmlnodelist.Item(0).InnerText;

                decimal tipoCambio = 0.00m;
                string fechaCambio = "-";

                oFactura.ListaDocCRECPE = new WCFFacturacion.ListaDocCRECPE();

                foreach (var reten in ret.SUNATRetentionDocumentReference)
                {
                    oFactura.DocCRECPE = new WCFFacturacion.DocCRECPE();

                    oFactura.DocCRECPE.TipoDocRelac = reten.ID.schemeID; // TPO DOC RELAC
                    oFactura.DocCRECPE.NroDocRelac = reten.ID.Value;  // NRO DOC RELAC
                    oFactura.DocCRECPE.FechaEmisionDocRelac = reten.IssueDate.Value.ToString("yyyy-MM-dd"); // FECHA EMI DOC RELAC
                    oFactura.DocCRECPE.ImporteTotDocRelac = reten.TotalInvoiceAmount.Value; // IMP TOT DOC RELAC
                    oFactura.DocCRECPE.MonedaImpTotDocRelac = string.Empty + reten.TotalInvoiceAmount.currencyID; // MON IMP TOT RELAC

                    oFactura.DocCRECPE.FechaPago = string.Empty + reten.Payment.PaidDate.Value.ToString("yyyy-MM-dd"); // FECHA PAGO
                    oFactura.DocCRECPE.NumeroPago = reten.Payment.ID.Value; // NUMERO DE PAGO
                    oFactura.DocCRECPE.ImportePagoSinReten = reten.Payment.PaidAmount.Value; // IMP PAGO SIN RETENCION
                    oFactura.DocCRECPE.MonedaPago = string.Empty + reten.Payment.PaidAmount.currencyID; // MONEDA PAGO

                    oFactura.DocCRECPE.ImporteRetenido = reten.SUNATRetentionInformation.SUNATRetentionAmount.Value; //IMP RETENIDO
                    oFactura.DocCRECPE.MonedaImpRetenido = string.Empty + reten.SUNATRetentionInformation.SUNATRetentionAmount.currencyID; // MONEDA IMP RET

                    oFactura.DocCRECPE.FechaRetencion = ret.SUNATRetentionDocumentReference.First().SUNATRetentionInformation.SUNATRetentionDate.Value.ToString("yyyy-MM-dd"); // FECHA DE RETENCION

                    oFactura.DocCRECPE.ImporteTotxPagoNeto = reten.SUNATRetentionInformation.SUNATNetTotalPaid.Value; // IMP TOT A PAGAR(NETO)
                    oFactura.DocCRECPE.MonedaTotxPagoNeto = string.Empty + reten.SUNATRetentionInformation.SUNATNetTotalPaid.currencyID; // MONEDA TOT A PAGAR (NETO)

                    if (oFactura.DocCRECPE.MonedaImpTotDocRelac == Constantes.USD)
                    {
                        oFactura.DocCRECPE.Simbolo = Constantes.DOLAR;
                    }
                    else if (oFactura.DocCRECPE.MonedaImpTotDocRelac == Constantes.PEN)
                    {
                        oFactura.DocCRECPE.Simbolo = Constantes.SOLES;
                    }

                    oFactura.DocCRECPE.SimboloSol = Constantes.SOLES;

                    if (reten.SUNATRetentionInformation.ExchangeRate == null) { }
                    else
                    {
                        if (reten.SUNATRetentionInformation.ExchangeRate.CalculationRate.Value > tipoCambio)
                        {
                            tipoCambio = reten.SUNATRetentionInformation.ExchangeRate.CalculationRate.Value;
                            fechaCambio = reten.SUNATRetentionInformation.ExchangeRate.Date.Value.ToString("yyyy-MM-dd");
                        }

                        tipoCambio = reten.SUNATRetentionInformation.ExchangeRate.CalculationRate.Value;
                        fechaCambio = reten.SUNATRetentionInformation.ExchangeRate.Date.Value.ToString("yyyy-MM-dd");
                    }
                    oFactura.ListaDocCRECPE.Add(oFactura.DocCRECPE);
                }

                oFactura.TipoCambio = tipoCambio + string.Empty;
                oFactura.FechaCambio = fechaCambio;

                xmlnodelist = XMLArchive.GetElementsByTagName("Extra");

                if (xmlnodelist == null)
                {
                    oFactura.ImpGlobSoles = oFactura.DocCRECPE.ImporteTotalRetenido + oFactura.DocCRECPE.ImporteTotalPagado;
                }
                else if (xmlnodelist.Count > 0)
                {
                    oFactura.ListaExtra = new WCFFacturacion.ListaExtra();
                    for (int i = 0; i <= xmlnodelist.Count - 1; i++)
                    {
                        oFactura.Extra = new WCFFacturacion.Extra();
                        oFactura.Extra.ExLinea = xmlnodelist.Item(i).ChildNodes[0].InnerText;
                        oFactura.Extra.ExDato = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                        oFactura.Extra.ExTipo = xmlnodelist.Item(i).ChildNodes[2].InnerText;
                        oFactura.ListaExtra.Add(oFactura.Extra);
                    }
                    oFactura.MontoTotalLetras = oFactura.ListaExtra[0].ExDato;
                    oFactura.ImpGlobSoles = decimal.Parse(oFactura.ListaExtra[1].ExDato);
                }
                else { oFactura.ImpGlobSoles = oFactura.DocCRECPE.ImporteTotalRetenido + oFactura.DocCRECPE.ImporteTotalPagado; }
                #endregion
            }
            catch (Exception ex)
            {
                return oFactura;
            }
            return oFactura;
        }

        #endregion

    }
}
