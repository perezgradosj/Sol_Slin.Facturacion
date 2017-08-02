using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Reporting.WebForms;
using System.IO;

using Slin.Facturacion.Proxies.ServicioFacturacion;
using System.Globalization;

namespace Slin.Facturacion.Common.Util
{
    public class ParametersClass
    {
        #region Set Parameters
        //public ReportParameter[] GetArrayParametersCE(FacturaElectronica oFact, ListaFacturaElectronica listamonto)
        //{
        //    ReportParameter[] Dpr = new ReportParameter[45];
        //    try
        //    {       
        //        Dpr[0] = new ReportParameter("EmisorRuc", oFact.Empresa.RUC);
        //        Dpr[1] = new ReportParameter("EmisorRazonSocial", oFact.Empresa.RazonSocial);
        //        Dpr[2] = new ReportParameter("EmisorDireccion", oFact.Empresa.Direccion);
        //        Dpr[3] = new ReportParameter("SerieNumeroDocumento", oFact.NumeroSerie + " - " + oFact.NumeroDocumento);
        //        Dpr[4] = new ReportParameter("ClienteRazonSocial", oFact.Cliente.RazonSocial);
        //        Dpr[5] = new ReportParameter("ClienteDireccion", oFact.Cliente.Direccion);
        //        Dpr[6] = new ReportParameter("ClienteRuc", oFact.Cliente.ClienteRuc);
        //        Dpr[7] = new ReportParameter("FechaEmision", oFact.FechaEmision.ToString("dd/MM/yyyy"));
        //        Dpr[8] = new ReportParameter("Moneda", oFact.TipoMoneda);
        //        Dpr[9] = new ReportParameter("TextoNeto", oFact.MontoTotalLetras);//TextoNexto.Remove(0, 4)
        //        Dpr[10] = new ReportParameter("Importe", listamonto[5].MontoTotal + string.Empty);

        //        Dpr[11] = new ReportParameter("TotalGravado", listamonto[0].MontoTotal + string.Empty);
        //        Dpr[12] = new ReportParameter("TotalnoGravado", listamonto[1].MontoTotal + string.Empty);
        //        Dpr[13] = new ReportParameter("TotalExonerado", listamonto[2].MontoTotal + string.Empty);
        //        Dpr[14] = new ReportParameter("TotalDescuento", listamonto[3].MontoTotal + string.Empty);
        //        Dpr[15] = new ReportParameter("TotalIGV", listamonto[4].MontoTotal + string.Empty);
        //        Dpr[16] = new ReportParameter("ImporteTotal", listamonto[5].MontoTotal + string.Empty);

        //        if (oFact.TotalDetracciones == null)
        //        {
        //            Dpr[17] = new ReportParameter("Detraccion", "1");
        //        }
        //        else
        //        {
        //            Dpr[17] = new ReportParameter("Detraccion", oFact.TotalDetracciones);
        //        }

        //        Dpr[18] = new ReportParameter("NroOrdCompra", oFact.NroOrdCompra);


        //        if (oFact.Detraccion.CtaDetraccion != null)
        //        {
        //            if (oFact.Detraccion.CtaDetraccion.Length > Constantes.ValorCero)
        //            {
        //                Dpr[19] = new ReportParameter("CtaDetraccion", "Operación Sujeta al SPOT\nBanco de la Nación Cta. Cte. N° " + oFact.Detraccion.CtaDetraccion);
        //            }
        //        }
        //        else
        //        {
        //            Dpr[19] = new ReportParameter("CtaDetraccion", "000");
        //        }

        //        Dpr[20] = new ReportParameter("ValueTpoDocCli", Helper.ListClass.GetValue_TypeDocClie(oFact.Cliente.TipoDocumentoIdentidad.Codigo));



        //        Dpr[21] = new ReportParameter("TpoOperacion", oFact.TpoOperacion );
        //        Dpr[22] = new ReportParameter("TotalOtrosCargos", oFact.TotOtrosCargos + string.Empty);
        //        Dpr[23] = new ReportParameter("PorcentDetraccion", oFact.PorcentDetraccion);
        //        Dpr[24] = new ReportParameter("MontoDetraccion", oFact.MontoDetraccion + string.Empty);


        //        Dpr[25] = new ReportParameter("DirCliente", oFact.Campo10 + string.Empty);



        //        #region MANIPULACION DE LA LISTA EXTRAS

        //        int constante = 26;
        //        for (int i = 0; i <= 18; i++)
        //        {
        //            try
        //            {
        //                if (oFact.ListaExtra[i] == null)
        //                {
        //                    Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
        //                    constante++;
        //                }
        //                else
        //                {
        //                    if (oFact.ListaExtra[i].ExDato.Contains("*b*") || oFact.ListaExtra[i].ExDato.Contains("*B*")) 
        //                    {
        //                        var linejump = oFact.ListaExtra[i].ExDato.Replace("*b*", "\n");
        //                        linejump = linejump.Replace("*B*", "\n");

        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        constante++;
        //                    }
        //                    else if (oFact.ListaExtra[i].ExDato.Contains("*p*") || oFact.ListaExtra[i].ExDato.Contains("*P*"))
        //                    {
        //                        var linejump = oFact.ListaExtra[i].ExDato.Replace("*p*", "\n");
        //                        linejump = linejump.Replace("*P*", "\n");

        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        constante++;
        //                    }
        //                    else
        //                    {
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), oFact.ListaExtra[i].ExDato);
        //                        constante++;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
        //                constante++;
        //            }
        //        }
        //        #endregion END MANIPULACION DE LA LISTA EXTRAS
        //    }
        //    catch (Exception ex)
        //    {
        //        return Dpr;
        //    }
        //    return Dpr;
        //}

        //public ReportParameter[] GetArrayParametersNotCred(FacturaElectronica oFact, ListaFacturaElectronica listamonto)
        //{
        //    ReportParameter[] Dpr = new ReportParameter[46];
        //    try
        //    {
        //        Dpr[0] = new ReportParameter("EmisorRuc", oFact.Empresa.RUC);
        //        Dpr[1] = new ReportParameter("EmisorRazonSocial", oFact.Empresa.RazonSocial);
        //        Dpr[2] = new ReportParameter("EmisorDireccion", oFact.Empresa.Direccion);
        //        Dpr[3] = new ReportParameter("SerieNumeroDocumento", oFact.NumeroSerie + " - " + oFact.NumeroDocumento);
        //        Dpr[4] = new ReportParameter("ClienteRazonSocial", oFact.Cliente.RazonSocial);
        //        Dpr[5] = new ReportParameter("ClienteDireccion", oFact.Cliente.Direccion);
        //        Dpr[6] = new ReportParameter("ClienteRuc", oFact.Cliente.ClienteRuc);
        //        Dpr[7] = new ReportParameter("FechaEmision", oFact.FechaEmision.ToString("dd/MM/yyyy"));
        //        Dpr[8] = new ReportParameter("Moneda", oFact.TipoMoneda);
        //        Dpr[9] = new ReportParameter("TextoNeto", oFact.MontoTotalLetras);//TextoNexto.Remove(0, 4)
        //        Dpr[10] = new ReportParameter("Importe", listamonto[5].MontoTotalCad);

        //        Dpr[11] = new ReportParameter("TotalGravado", listamonto[0].MontoTotalCad);
        //        Dpr[12] = new ReportParameter("TotalnoGravado", listamonto[1].MontoTotalCad);
        //        Dpr[13] = new ReportParameter("TotalExonerado", listamonto[2].MontoTotalCad);
        //        Dpr[14] = new ReportParameter("TotalDescuento", listamonto[3].MontoTotalCad);
        //        Dpr[15] = new ReportParameter("TotalIGV", listamonto[4].MontoTotalCad);
        //        Dpr[16] = new ReportParameter("ImporteTotal", listamonto[5].MontoTotalCad);

        //        //Dpr[10] = new ReportParameter("Importe", listamonto[5].MontoTotal + string.Empty);
        //        //Dpr[11] = new ReportParameter("TotalGravado", listamonto[0].MontoTotal + string.Empty);
        //        //Dpr[12] = new ReportParameter("TotalnoGravado", listamonto[1].MontoTotal + string.Empty);
        //        //Dpr[13] = new ReportParameter("TotalExonerado", listamonto[2].MontoTotal + string.Empty);
        //        //Dpr[14] = new ReportParameter("TotalDescuento", listamonto[3].MontoTotal + string.Empty);
        //        //Dpr[15] = new ReportParameter("TotalIGV", listamonto[4].MontoTotal + string.Empty);
        //        //Dpr[16] = new ReportParameter("ImporteTotal", listamonto[5].MontoTotal + string.Empty);

        //        if (oFact.TotalDetracciones == null)
        //        {
        //            Dpr[17] = new ReportParameter("Detraccion", "1");
        //        }
        //        else
        //        {
        //            Dpr[17] = new ReportParameter("Detraccion", oFact.TotalDetracciones);
        //        }

        //        Dpr[18] = new ReportParameter("NroOrdCompra", oFact.NroOrdCompra);

        //        //FOR HIGH PDF
        //        string tipodocafectado = string.Empty;
        //        string nrodocafectado = string.Empty;
        //        string saltolinea = string.Empty;
        //        foreach (var tpodocafec in oFact.ListaAfectado)
        //        {
        //            if (tpodocafec.CodigoTipoDocumento == Constantes.Factura)
        //            {
        //                tipodocafectado += saltolinea + Constantes.FacturaDesc;
        //            }
        //            else if (tpodocafec.CodigoTipoDocumento == Constantes.Boleta)
        //            {
        //                tipodocafectado += saltolinea + Constantes.BoletaDesc;
        //            }
        //            else if (tpodocafec.CodigoTipoDocumento == Constantes.TicketMaqRegistradora)
        //            {
        //                tipodocafectado += saltolinea + Constantes.TicketMaqRegistradoraDesc;
        //            }
        //            else
        //            {
        //                tipodocafectado += saltolinea + tpodocafec.CodigoTipoDocumento;
        //            }
        //            nrodocafectado += saltolinea + tpodocafec.ID;
        //            saltolinea = Constantes.SaltoLinea;
        //        }

        //        //FOR SMALL PDF
        //        for (int j = 0; j <= oFact.ListaAfectado.Count - 1; j++)
        //        {
        //            switch (oFact.ListaAfectado[j].CodigoTipoDocumento)
        //            {
        //                case Constantes.Boleta:
        //                    {
        //                        oFact.ListaAfectado[j].ID = oFact.ListaAfectado[j].ID;
        //                        oFact.ListaAfectado[j].CodigoTipoDocumento = Constantes.BoletaDesc;
        //                        break;
        //                    }
        //                case Constantes.Factura:
        //                    {
        //                        oFact.ListaAfectado[j].ID = oFact.ListaAfectado[j].ID;
        //                        oFact.ListaAfectado[j].CodigoTipoDocumento = Constantes.FacturaDesc;
        //                        break;
        //                    }
        //                case Constantes.TicketMaqRegistradora:
        //                    {
        //                        oFact.ListaAfectado[j].ID = oFact.ListaAfectado[j].ID;
        //                        oFact.ListaAfectado[j].CodigoTipoDocumento = Constantes.TicketMaqRegistradoraDesc;
        //                        break;
        //                    }
        //            }
        //        }

        //        Dpr[19] = new ReportParameter("TipoDocAfectado", tipodocafectado);
        //        Dpr[20] = new ReportParameter("NroDocAfectado", nrodocafectado);
        //        Dpr[21] = new ReportParameter("MotivoAnulado", oFact.MotivoAnulado);

        //        Dpr[22] = new ReportParameter("ValueTpoDocCli", Helper.ListClass.GetValue_TypeDocClie(oFact.Cliente.TipoDocumentoIdentidad.Codigo));


        //        Dpr[23] = new ReportParameter("CodTpoNC", oFact.CodTpoNC);
        //        Dpr[24] = new ReportParameter("DescTpoNC", oFact.DescTpoNC);
        //        Dpr[25] = new ReportParameter("TotalOtrosCargos", oFact.TotOtrosCargos + string.Empty);

        //        Dpr[26] = new ReportParameter("DirCliente", oFact.Campo10 + string.Empty);

        //        #region MANIPULACION DE LA LISTA EXTRAS

        //        int constante = 27;
        //        for (int i = 0; i <= 18; i++)
        //        {
        //            try
        //            {
        //                if (oFact.ListaExtra[i] == null)
        //                {
        //                    Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
        //                    constante++;
        //                }
        //                else
        //                {
        //                    if (oFact.ListaExtra[i].ExDato.Contains("*b*") || oFact.ListaExtra[i].ExDato.Contains("*B*"))
        //                    {
        //                        var linejump = oFact.ListaExtra[i].ExDato.Replace("*b*", "\n");
        //                        linejump = linejump.Replace("*B*", "\n");

        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        constante++;
        //                    }
        //                    else if (oFact.ListaExtra[i].ExDato.Contains("*p*") || oFact.ListaExtra[i].ExDato.Contains("*P*"))
        //                    {
        //                        var linejump = oFact.ListaExtra[i].ExDato.Replace("*p*", "\n");
        //                        linejump = linejump.Replace("*P*", "\n");

        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        constante++;
        //                    }
        //                    else
        //                    {
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), oFact.ListaExtra[i].ExDato);
        //                        constante++;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
        //                constante++;
        //            }
        //        }

        //        #endregion END MANIPULACION DE LA LISTA EXTRAS
        //    }
        //    catch (Exception ex)
        //    {
        //        return Dpr;
        //    }
        //    return Dpr;
        //}

        //public ReportParameter[] GetArrayParametersNotDebit(FacturaElectronica oFact, ListaFacturaElectronica listamonto)
        //{
        //    ReportParameter[] Dpr = new ReportParameter[49];
        //    try
        //    {
        //        Dpr[0] = new ReportParameter("EmisorRuc", oFact.Empresa.RUC);
        //        Dpr[1] = new ReportParameter("EmisorRazonSocial", oFact.Empresa.RazonSocial);
        //        Dpr[2] = new ReportParameter("EmisorDireccion", oFact.Empresa.Direccion);
        //        Dpr[3] = new ReportParameter("SerieNumeroDocumento", oFact.NumeroSerie + " - " + oFact.NumeroDocumento);
        //        Dpr[4] = new ReportParameter("ClienteRazonSocial", oFact.Cliente.RazonSocial);
        //        Dpr[5] = new ReportParameter("ClienteDireccion", oFact.Cliente.Direccion);
        //        Dpr[6] = new ReportParameter("ClienteRuc", oFact.Cliente.ClienteRuc);
        //        Dpr[7] = new ReportParameter("FechaEmision", oFact.FechaEmision.ToString("dd/MM/yyyy"));
        //        Dpr[8] = new ReportParameter("Moneda", oFact.TipoMoneda);
        //        Dpr[9] = new ReportParameter("TextoNeto", oFact.MontoTotalLetras.Remove(0, 4));//TextoNexto.Remove(0, 4)
        //        Dpr[9] = new ReportParameter("TextoNeto", oFact.MontoTotalLetras);//TextoNexto.Remove(0, 4)
        //        Dpr[10] = new ReportParameter("Importe", listamonto[5].MontoTotalCad);

        //        Dpr[11] = new ReportParameter("TotalGravado", listamonto[0].MontoTotalCad);
        //        Dpr[12] = new ReportParameter("TotalnoGravado", listamonto[1].MontoTotalCad);
        //        Dpr[13] = new ReportParameter("TotalExonerado", listamonto[2].MontoTotalCad);
        //        Dpr[14] = new ReportParameter("TotalDescuento", listamonto[3].MontoTotalCad);
        //        Dpr[15] = new ReportParameter("TotalIGV", listamonto[4].MontoTotalCad);
        //        Dpr[16] = new ReportParameter("ImporteTotal", listamonto[5].MontoTotalCad);

        //        //Dpr[10] = new ReportParameter("Importe", listamonto[5].MontoTotal + string.Empty);
        //        //Dpr[11] = new ReportParameter("TotalGravado", listamonto[0].MontoTotal + string.Empty);
        //        //Dpr[12] = new ReportParameter("TotalnoGravado", listamonto[1].MontoTotal + string.Empty);
        //        //Dpr[13] = new ReportParameter("TotalExonerado", listamonto[2].MontoTotal + string.Empty);
        //        //Dpr[14] = new ReportParameter("TotalDescuento", listamonto[3].MontoTotal + string.Empty);
        //        //Dpr[15] = new ReportParameter("TotalIGV", listamonto[4].MontoTotal + string.Empty);
        //        //Dpr[16] = new ReportParameter("ImporteTotal", listamonto[5].MontoTotal + string.Empty);

        //        if (oFact.TotalDetracciones == null)
        //        {
        //            Dpr[17] = new ReportParameter("Detraccion", "1");
        //        }
        //        else
        //        {
        //            Dpr[17] = new ReportParameter("Detraccion", oFact.TotalDetracciones);
        //        }

        //        Dpr[18] = new ReportParameter("NroOrdCompra", oFact.NroOrdCompra);

        //        //FOR HIGH PDF
        //        string tipodocafectado = string.Empty;
        //        string nrodocafectado = string.Empty;
        //        string saltolinea = string.Empty;
        //        foreach (var tpodocafec in oFact.ListaAfectado)
        //        {
        //            if (tpodocafec.CodigoTipoDocumento == Constantes.Factura)
        //            {
        //                tipodocafectado += saltolinea + Constantes.FacturaDesc;
        //            }
        //            else if (tpodocafec.CodigoTipoDocumento == Constantes.Boleta)
        //            {
        //                tipodocafectado += saltolinea + Constantes.BoletaDesc;
        //            }
        //            else if (tpodocafec.CodigoTipoDocumento == Constantes.TicketMaqRegistradora)
        //            {
        //                tipodocafectado += saltolinea + Constantes.TicketMaqRegistradoraDesc;
        //            }
        //            else
        //            {
        //                tipodocafectado += saltolinea + tpodocafec.CodigoTipoDocumento;
        //            }
        //            nrodocafectado += saltolinea + tpodocafec.ID;
        //            saltolinea = Constantes.SaltoLinea;
        //        }

        //        //FOR SMALL PDF
        //        for (int j = 0; j <= oFact.ListaAfectado.Count - 1; j++)
        //        {
        //            switch (oFact.ListaAfectado[j].CodigoTipoDocumento)
        //            {
        //                case Constantes.Boleta:
        //                    {
        //                        oFact.ListaAfectado[j].ID = oFact.ListaAfectado[j].ID;
        //                        oFact.ListaAfectado[j].CodigoTipoDocumento = Constantes.BoletaDesc;
        //                        break;
        //                    }
        //                case Constantes.Factura:
        //                    {
        //                        oFact.ListaAfectado[j].ID = oFact.ListaAfectado[j].ID;
        //                        oFact.ListaAfectado[j].CodigoTipoDocumento = Constantes.FacturaDesc;
        //                        break;
        //                    }
        //                case Constantes.TicketMaqRegistradora:
        //                    {
        //                        oFact.ListaAfectado[j].ID = oFact.ListaAfectado[j].ID;
        //                        oFact.ListaAfectado[j].CodigoTipoDocumento = Constantes.TicketMaqRegistradoraDesc;
        //                        break;
        //                    }
        //            }
        //        }

        //        Dpr[19] = new ReportParameter("TipoDocAfectado", tipodocafectado);
        //        Dpr[20] = new ReportParameter("NroDocAfectado", nrodocafectado);
        //        Dpr[21] = new ReportParameter("MotivoAnulado", oFact.MotivoAnulado);

        //        //detraccion
        //        if (oFact.Detraccion.CtaDetraccion != null)
        //        {
        //            if (oFact.Detraccion.CtaDetraccion.Length > Constantes.ValorCero)
        //            {
        //                Dpr[22] = new ReportParameter("CtaDetraccion", "Operación Sujeta al SPOT\nBanco de la Nación Cta. Cte. N° " + oFact.Detraccion.CtaDetraccion);
        //            }
        //        }
        //        else
        //        {
        //            Dpr[22] = new ReportParameter("CtaDetraccion", "000");
        //        }

        //        Dpr[23] = new ReportParameter("ValueTpoDocCli", Helper.ListClass.GetValue_TypeDocClie(oFact.Cliente.TipoDocumentoIdentidad.Codigo));



        //        Dpr[24] = new ReportParameter("CodTpoND", oFact.CodTpoND);
        //        Dpr[25] = new ReportParameter("DescTpoND", oFact.DescTpoND);
        //        Dpr[26] = new ReportParameter("TotalOtrosCargos", oFact.TotOtrosCargos + string.Empty);
        //        Dpr[27] = new ReportParameter("PorcentDetraccion", oFact.PorcentDetraccion);
        //        Dpr[28] = new ReportParameter("MontoDetraccion", oFact.MontoDetraccion + string.Empty);

        //        Dpr[29] = new ReportParameter("DirCliente", oFact.Campo10 + string.Empty);

        //        #region MANIPULACION DE LA LISTA EXTRAS

        //        int constante = 30;
        //        for (int i = 0; i <= 18; i++)
        //        {
        //            try
        //            {
        //                if (oFact.ListaExtra[i] == null)
        //                {
        //                    Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
        //                    constante++;
        //                }
        //                else
        //                {
        //                    if (oFact.ListaExtra[i].ExDato.Contains("*b*") || oFact.ListaExtra[i].ExDato.Contains("*B*"))
        //                    {
        //                        var linejump = oFact.ListaExtra[i].ExDato.Replace("*b*", "\n");
        //                        linejump = linejump.Replace("*B*", "\n");

        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        constante++;
        //                    }
        //                    else if (oFact.ListaExtra[i].ExDato.Contains("*p*") || oFact.ListaExtra[i].ExDato.Contains("*P*"))
        //                    {
        //                        var linejump = oFact.ListaExtra[i].ExDato.Replace("*p*", "\n");
        //                        linejump = linejump.Replace("*P*", "\n");

        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        constante++;
        //                    }
        //                    else
        //                    {
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), oFact.ListaExtra[i].ExDato);
        //                        constante++;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
        //                constante++;
        //            }
        //        }

        //        #endregion END MANIPULACION DE LA LISTA EXTRAS
        //    }
        //    catch (Exception ex)
        //    {
        //        return Dpr;
        //    }
        //    return Dpr;
        //}

        //public ReportParameter[] GetArrayParametersRetenc(FacturaElectronica oFact)
        //{
        //    ReportParameter[] Dpr = new ReportParameter[26];
        //    try
        //    {
        //        Dpr[0] = new ReportParameter("RucEmisor", oFact.Empresa.RUC);

        //        Dpr[1] = new ReportParameter("EmisorRazonSocial", oFact.Empresa.RazonSocial);
        //        Dpr[2] = new ReportParameter("EmisorDireccion", oFact.Empresa.Direccion);

        //        Dpr[3] = new ReportParameter("SerieCorrelativo", oFact.SerieCorrelativo);
        //        Dpr[4] = new ReportParameter("RazonSocialCliente", oFact.Cliente.RazonSocial);
        //        Dpr[5] = new ReportParameter("RucCliente", oFact.Cliente.ClienteRuc);
        //        Dpr[6] = new ReportParameter("DireccionCliente", oFact.Cliente.Direccion);
        //        Dpr[7] = new ReportParameter("FechaEmision", oFact.FechaEmision2);
        //        //Dpr[6] = new ReportParameter("TasaRetencion", oFact.DocCRECPE.TasaRetencion);
        //        //Dpr[7] = new ReportParameter("ImpTotRetenido", oFact.DocCRECPE.ImporteTotalRetenido + string.Empty);
        //        //Dpr[8] = new ReportParameter("ImpTotPagado", oFact.DocCRECPE.ImporteTotalPagado + string.Empty);
        //        Dpr[8] = new ReportParameter("TasaRetencion", oFact.TasaRetencion);
        //        Dpr[9] = new ReportParameter("ImpTotRetenido", oFact.ImporteTotalRetenido + string.Empty);
        //        Dpr[10] = new ReportParameter("ImpTotPagado", oFact.ImporteTotalPagado + string.Empty);
        //        Dpr[11] = new ReportParameter("TipoCambio", oFact.TipoCambio + string.Empty);
        //        Dpr[12] = new ReportParameter("FechaTipoCambio", oFact.FechaCambio);
        //        Dpr[13] = new ReportParameter("TextoNeto", oFact.MontoTotalLetras);
        //        Dpr[14] = new ReportParameter("ImpGlobSoles", oFact.ImpGlobSoles + string.Empty);

        //        #region MANIPULACION DE LA LISTA EXTRAS

        //        int constante = 15;
        //        for (int i = 0; i <= 10; i++)
        //        {
        //            try
        //            {
        //                if (oFact.ListaExtra[i] == null)
        //                {
        //                    Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
        //                    constante++;
        //                }
        //                else
        //                {
        //                    if (oFact.ListaExtra[i].ExDato.Contains("*b*") || oFact.ListaExtra[i].ExDato.Contains("*B*"))
        //                    {
        //                        var linejump = oFact.ListaExtra[i].ExDato.Replace("*b*", "\n");
        //                        linejump = linejump.Replace("*B*", "\n");

        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        constante++;
        //                    }
        //                    else if (oFact.ListaExtra[i].ExDato.Contains("*p*") || oFact.ListaExtra[i].ExDato.Contains("*P*"))
        //                    {
        //                        var linejump = oFact.ListaExtra[i].ExDato.Replace("*p*", "\n");
        //                        linejump = linejump.Replace("*P*", "\n");

        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        constante++;
        //                    }
        //                    else
        //                    {
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), oFact.ListaExtra[i].ExDato);
        //                        constante++;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
        //                constante++;
        //            }
        //        }

        //        #endregion END MANIPULACION DE LA LISTA EXTRAS
        //    }
        //    catch (Exception ex)
        //    {
        //        return Dpr;
        //    }
        //    return Dpr;
        //}

        #endregion



        #region Set Parameters
        public ReportParameter[] GetArrayParametersCE(FacturaElectronica oFact, ListaFacturaElectronica listamonto)
        {
            ReportParameter[] Dpr = new ReportParameter[45];
            try
            {
                Dpr[0] = new ReportParameter("EmisorRuc", oFact.Empresa.RUC);
                Dpr[1] = new ReportParameter("EmisorRazonSocial", oFact.Empresa.RazonSocial);
                Dpr[2] = new ReportParameter("EmisorDireccion", oFact.Empresa.Direccion);
                Dpr[3] = new ReportParameter("SerieNumeroDocumento", oFact.NumeroSerie + " - " + oFact.NumeroDocumento);
                Dpr[4] = new ReportParameter("ClienteRazonSocial", oFact.Cliente.RazonSocial);
                Dpr[5] = new ReportParameter("ClienteDireccion", oFact.Cliente.Direccion);
                Dpr[6] = new ReportParameter("ClienteRuc", oFact.Cliente.ClienteRuc);
                Dpr[7] = new ReportParameter("FechaEmision", oFact.FechaEmision.ToShortDateString());
                Dpr[8] = new ReportParameter("Moneda", oFact.TipoMoneda);
                Dpr[9] = new ReportParameter("TextoNeto", oFact.MontoTotalLetras);
                //Dpr[10] = new ReportParameter("Importe", listamonto[5].MontoTotal + string.Empty);

                //Dpr[11] = new ReportParameter("TotalGravado", listamonto[0].MontoTotal + string.Empty);
                //Dpr[12] = new ReportParameter("TotalnoGravado", listamonto[1].MontoTotal + string.Empty);
                //Dpr[13] = new ReportParameter("TotalExonerado", listamonto[2].MontoTotal + string.Empty);
                //Dpr[14] = new ReportParameter("TotalDescuento", listamonto[3].MontoTotal + string.Empty);
                //Dpr[15] = new ReportParameter("TotalIGV", listamonto[4].MontoTotal + string.Empty);
                //Dpr[16] = new ReportParameter("ImporteTotal", listamonto[5].MontoTotal + string.Empty);

                Dpr[10] = new ReportParameter("Importe", oFact.MontoTotal + string.Empty);

                Dpr[11] = new ReportParameter("TotalGravado", oFact.TotalGravadoSinIGV + string.Empty);
                Dpr[12] = new ReportParameter("TotalnoGravado", oFact.TotalInafecto_mon + string.Empty);
                Dpr[13] = new ReportParameter("TotalExonerado", oFact.TotalExonerado_mon + string.Empty);
                Dpr[14] = new ReportParameter("TotalDescuento", oFact.TotalDescuento_mon + string.Empty);
                Dpr[15] = new ReportParameter("TotalIGV", oFact.MontoIGV + string.Empty);
                Dpr[16] = new ReportParameter("ImporteTotal", oFact.MontoTotal + string.Empty);

                if (oFact.TotalDetracciones == null)
                {
                    Dpr[17] = new ReportParameter("Detraccion", "1");
                }
                else
                {
                    //Dpr[17] = new ReportParameter("Detraccion", oFact.TotalDetracciones);
                    Dpr[17] = new ReportParameter("Detraccion", oFact.MontoDetraccion + string.Empty);
                }

                Dpr[18] = new ReportParameter("NroOrdCompra", oFact.NroOrdCompra);


                if (oFact.Detraccion.CtaDetraccion != null)
                {
                    if (oFact.Detraccion.CtaDetraccion.Length > Constantes.ValorCero)
                    {
                        Dpr[19] = new ReportParameter("CtaDetraccion", "Operación Sujeta al SPOT\nBanco de la Nación Cta. Cte. N° " + oFact.Detraccion.CtaDetraccion);
                    }
                }
                else
                {
                    Dpr[19] = new ReportParameter("CtaDetraccion", "000");
                }

                Dpr[20] = new ReportParameter("ValueTpoDocCli", Helper.ListClass.GetValue_TypeDocClie(oFact.Cliente.TipoDocumentoIdentidad.Codigo));

                Dpr[21] = new ReportParameter("TpoOperacion", oFact.TpoOperacion);
                Dpr[22] = new ReportParameter("TotalOtrosCargos", oFact.TotOtrosCargos + string.Empty);
                Dpr[23] = new ReportParameter("PorcentDetraccion", oFact.PorcentDetraccion);
                Dpr[24] = new ReportParameter("MontoDetraccion", oFact.MontoDetraccion + string.Empty);


                Dpr[25] = new ReportParameter("DirCliente", oFact.Campo10 + string.Empty);

                #region MANIPULACION DE LA LISTA EXTRAS

                int constante = 26;
                for (int i = 0; i <= 18; i++)
                {
                    try
                    {
                        if (oFact.ListaExtra[i] == null)
                        {
                            Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
                            constante++;
                        }
                        else
                        {
                            if (oFact.ListaExtra[i].ExDato.Contains("*b*") || oFact.ListaExtra[i].ExDato.Contains("*B*"))
                            {
                                var linejump = oFact.ListaExtra[i].ExDato.Replace("*b*", "\n");
                                linejump = linejump.Replace("*B*", "\n");

                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
                                constante++;
                            }
                            else if (oFact.ListaExtra[i].ExDato.Contains("*p*") || oFact.ListaExtra[i].ExDato.Contains("*P*"))
                            {
                                var linejump = oFact.ListaExtra[i].ExDato.Replace("*p*", "\n");
                                linejump = linejump.Replace("*P*", "\n");

                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
                                constante++;
                            }
                            else
                            {
                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), oFact.ListaExtra[i].ExDato);
                                constante++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
                        constante++;
                    }
                }
                #endregion END MANIPULACION DE LA LISTA EXTRAS
            }
            catch (Exception ex)
            {
                return Dpr;
            }
            return Dpr;
        }

        #region RESUMEN

        //public ReportParameter[] GetArrayParametersCE_Resumen(FacturaElectronica oFact, ListaFacturaElectronica listamonto, decimal ImporteTotal_Res)
        //{
        //    ReportParameter[] Dpr = new ReportParameter[41];
        //    try
        //    {
        //        Dpr[0] = new ReportParameter("EmisorRuc", oFact.Empresa.RUC);
        //        Dpr[1] = new ReportParameter("EmisorRazonSocial", oFact.Empresa.RazonSocial);
        //        Dpr[2] = new ReportParameter("EmisorDireccion", oFact.Empresa.Direccion);
        //        Dpr[3] = new ReportParameter("SerieNumeroDocumento", oFact.NumeroSerie + " - " + oFact.NumeroDocumento);
        //        Dpr[4] = new ReportParameter("ClienteRazonSocial", oFact.Cliente.RazonSocial);
        //        Dpr[5] = new ReportParameter("ClienteDireccion", oFact.Cliente.Direccion);
        //        Dpr[6] = new ReportParameter("ClienteRuc", oFact.Cliente.ClienteRuc);
        //        Dpr[7] = new ReportParameter("FechaEmision", oFact.FechaEmision.ToShortDateString());
        //        Dpr[8] = new ReportParameter("Moneda", oFact.TipoMoneda);
        //        Dpr[9] = new ReportParameter("TextoNeto", oFact.MontoTotalLetras.Remove(0, 4));
        //        Dpr[10] = new ReportParameter("Importe", listamonto[5].MontoTotal + string.Empty);

        //        Dpr[11] = new ReportParameter("TotalGravado", listamonto[0].MontoTotal + string.Empty);
        //        Dpr[12] = new ReportParameter("TotalnoGravado", listamonto[1].MontoTotal + string.Empty);
        //        Dpr[13] = new ReportParameter("TotalExonerado", listamonto[2].MontoTotal + string.Empty);
        //        Dpr[14] = new ReportParameter("TotalDescuento", listamonto[3].MontoTotal + string.Empty);
        //        Dpr[15] = new ReportParameter("TotalIGV", listamonto[4].MontoTotal + string.Empty);
        //        Dpr[16] = new ReportParameter("ImporteTotal", listamonto[5].MontoTotal + string.Empty);

        //        if (oFact.TotalDetracciones == null)
        //        {
        //            Dpr[17] = new ReportParameter("Detraccion", "1");
        //        }
        //        else
        //        {
        //            Dpr[17] = new ReportParameter("Detraccion", oFact.TotalDetracciones);
        //        }

        //        Dpr[18] = new ReportParameter("NroOrdCompra", oFact.NroOrdCompra);


        //        if (oFact.Detraccion.CtaDetraccion != null)
        //        {
        //            if (oFact.Detraccion.CtaDetraccion.Length > Constantes.ValorCero)
        //            {
        //                Dpr[19] = new ReportParameter("CtaDetraccion", "Operación Sujeta al SPOT\nBanco de la Nación Cta. Cte. N° " + oFact.Detraccion.CtaDetraccion);
        //            }
        //        }
        //        else
        //        {
        //            Dpr[19] = new ReportParameter("CtaDetraccion", "000");
        //        }

        //        Dpr[20] = new ReportParameter("ValueTpoDocCli", Helper.ListClass.GetValue_TypeDocClie(oFact.Cliente.TipoDocumentoIdentidad.Codigo));

        //        #region MANIPULACION DE LA LISTA EXTRAS

        //        int constante = 21;
        //        for (int i = 0; i <= 18; i++)
        //        {
        //            try
        //            {
        //                if (oFact.ListaExtra[i] == null)
        //                {
        //                    Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
        //                    constante++;
        //                }
        //                else
        //                {
        //                    if (oFact.ListaExtra[i].ExDato.Contains("*b*") || oFact.ListaExtra[i].ExDato.Contains("*B*"))
        //                    {
        //                        var linejump = oFact.ListaExtra[i].ExDato.Replace("*b*", "\n");
        //                        linejump = linejump.Replace("*B*", "\n");

        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        constante++;
        //                    }
        //                    else if (oFact.ListaExtra[i].ExDato.Contains("*p*") || oFact.ListaExtra[i].ExDato.Contains("*P*"))
        //                    {
        //                        var linejump = oFact.ListaExtra[i].ExDato.Replace("*p*", "\n");
        //                        linejump = linejump.Replace("*P*", "\n");

        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        constante++;
        //                    }
        //                    else
        //                    {
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), oFact.ListaExtra[i].ExDato);
        //                        constante++;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
        //                constante++;
        //            }
        //        }
        //        #endregion END MANIPULACION DE LA LISTA EXTRAS

        //        Dpr[40] = new ReportParameter("ImporteTotal_Res", ImporteTotal_Res + string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Dpr;
        //    }
        //    return Dpr;
        //}

        #endregion

        public ReportParameter[] GetArrayParametersNotCred(FacturaElectronica oFact, ListaFacturaElectronica listamonto)
        {
            ReportParameter[] Dpr = new ReportParameter[46];
            try
            {
                Dpr[0] = new ReportParameter("EmisorRuc", oFact.Empresa.RUC);
                Dpr[1] = new ReportParameter("EmisorRazonSocial", oFact.Empresa.RazonSocial);
                Dpr[2] = new ReportParameter("EmisorDireccion", oFact.Empresa.Direccion);
                Dpr[3] = new ReportParameter("SerieNumeroDocumento", oFact.NumeroSerie + " - " + oFact.NumeroDocumento);
                Dpr[4] = new ReportParameter("ClienteRazonSocial", oFact.Cliente.RazonSocial);
                Dpr[5] = new ReportParameter("ClienteDireccion", oFact.Cliente.Direccion);
                Dpr[6] = new ReportParameter("ClienteRuc", oFact.Cliente.ClienteRuc);
                Dpr[7] = new ReportParameter("FechaEmision", oFact.FechaEmision.ToString("dd/MM/yyyy"));
                Dpr[8] = new ReportParameter("Moneda", oFact.TipoMoneda);
                Dpr[9] = new ReportParameter("TextoNeto", oFact.MontoTotalLetras);
                //Dpr[10] = new ReportParameter("Importe", listamonto[5].MontoTotalCad);
                //Dpr[11] = new ReportParameter("TotalGravado", listamonto[0].MontoTotalCad);
                //Dpr[12] = new ReportParameter("TotalnoGravado", listamonto[1].MontoTotalCad);
                //Dpr[13] = new ReportParameter("TotalExonerado", listamonto[2].MontoTotalCad);
                //Dpr[14] = new ReportParameter("TotalDescuento", listamonto[3].MontoTotalCad);
                //Dpr[15] = new ReportParameter("TotalIGV", listamonto[4].MontoTotalCad);
                //Dpr[16] = new ReportParameter("ImporteTotal", listamonto[5].MontoTotalCad);


                //Dpr[10] = new ReportParameter("Importe", listamonto[5].MontoTotal + string.Empty);
                //Dpr[11] = new ReportParameter("TotalGravado", listamonto[0].MontoTotal + string.Empty);
                //Dpr[12] = new ReportParameter("TotalnoGravado", listamonto[1].MontoTotal + string.Empty);
                //Dpr[13] = new ReportParameter("TotalExonerado", listamonto[2].MontoTotal + string.Empty);
                //Dpr[14] = new ReportParameter("TotalDescuento", listamonto[3].MontoTotal + string.Empty);
                //Dpr[15] = new ReportParameter("TotalIGV", listamonto[4].MontoTotal + string.Empty);
                //Dpr[16] = new ReportParameter("ImporteTotal", Convert.ToDecimal(listamonto[5].MontoTotal + string.Empty, CultureInfo.CreateSpecificCulture("es-PE")) + string.Empty);



                Dpr[10] = new ReportParameter("Importe", oFact.MontoTotal + string.Empty);
                Dpr[11] = new ReportParameter("TotalGravado", oFact.TotalGravadoSinIGV + string.Empty);
                Dpr[12] = new ReportParameter("TotalnoGravado", oFact.TotalInafecto_mon + string.Empty);
                Dpr[13] = new ReportParameter("TotalExonerado", oFact.TotalExonerado_mon + string.Empty);
                Dpr[14] = new ReportParameter("TotalDescuento", oFact.TotalDescuento_mon + string.Empty);
                Dpr[15] = new ReportParameter("TotalIGV", oFact.MontoIGV + string.Empty);
                Dpr[16] = new ReportParameter("ImporteTotal", oFact.MontoTotal + string.Empty);




                if (oFact.TotalDetracciones == null)
                {
                    Dpr[17] = new ReportParameter("Detraccion", "1");
                }
                else
                {
                    //Dpr[17] = new ReportParameter("Detraccion", oFact.TotalDetracciones);
                    Dpr[17] = new ReportParameter("Detraccion", oFact.MontoDetraccion + string.Empty);
                }

                Dpr[18] = new ReportParameter("NroOrdCompra", oFact.NroOrdCompra);

                //FOR HIGH PDF
                string tipodocafectado = string.Empty;
                string nrodocafectado = string.Empty;
                string saltolinea = string.Empty;

                foreach (var tpodocafec in oFact.ListaAfectado)
                {
                    if (tpodocafec.CodigoTipoDocumento == Constantes.Factura)
                    { tipodocafectado += saltolinea + Constantes.FacturaDesc; }
                    else if (tpodocafec.CodigoTipoDocumento == Constantes.Boleta)
                    { tipodocafectado += saltolinea + Constantes.BoletaDesc; }
                    else if (tpodocafec.CodigoTipoDocumento == Constantes.TicketMaqRegistradora)
                    { tipodocafectado += saltolinea + Constantes.TicketMaqRegistradoraDesc; }
                    else
                    { tipodocafectado += saltolinea + tpodocafec.CodigoTipoDocumento; }
                    nrodocafectado += saltolinea + tpodocafec.ID;
                    saltolinea = Constantes.SaltoLinea;
                }


                //FOR SMALL PDF
                for (int j = 0; j <= oFact.ListaAfectado.Count - 1; j++)
                {
                    switch (oFact.ListaAfectado[j].CodigoTipoDocumento)
                    {
                        case Constantes.Boleta:
                            {
                                oFact.ListaAfectado[j].ID = oFact.ListaAfectado[j].ID;
                                oFact.ListaAfectado[j].CodigoTipoDocumento = Constantes.BoletaDesc;
                                break;
                            }
                        case Constantes.Factura:
                            {
                                oFact.ListaAfectado[j].ID = oFact.ListaAfectado[j].ID;
                                oFact.ListaAfectado[j].CodigoTipoDocumento = Constantes.FacturaDesc;
                                break;
                            }
                        case Constantes.TicketMaqRegistradora:
                            {
                                oFact.ListaAfectado[j].ID = oFact.ListaAfectado[j].ID;
                                oFact.ListaAfectado[j].CodigoTipoDocumento = Constantes.TicketMaqRegistradoraDesc;
                                break;
                            }
                    }
                }

                Dpr[19] = new ReportParameter("TipoDocAfectado", tipodocafectado);
                Dpr[20] = new ReportParameter("NroDocAfectado", nrodocafectado);
                Dpr[21] = new ReportParameter("MotivoAnulado", oFact.MotivoAnulado);

                Dpr[22] = new ReportParameter("ValueTpoDocCli", Helper.ListClass.GetValue_TypeDocClie(oFact.Cliente.TipoDocumentoIdentidad.Codigo));

                Dpr[23] = new ReportParameter("CodTpoNC", oFact.CodTpoNC);
                Dpr[24] = new ReportParameter("DescTpoNC", oFact.DescTpoNC);
                Dpr[25] = new ReportParameter("TotalOtrosCargos", oFact.TotOtrosCargos + string.Empty);

                Dpr[26] = new ReportParameter("DirCliente", oFact.Campo10 + string.Empty);

                #region MANIPULACION DE LA LISTA EXTRAS

                int constante = 27;
                for (int i = 0; i <= 18; i++)
                {
                    try
                    {
                        if (oFact.ListaExtra[i] == null)
                        {
                            Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
                            constante++;
                        }
                        else
                        {
                            if (oFact.ListaExtra[i].ExDato.Contains("*b*") || oFact.ListaExtra[i].ExDato.Contains("*B*"))
                            {
                                var linejump = oFact.ListaExtra[i].ExDato.Replace("*b*", "\n");
                                linejump = linejump.Replace("*B*", "\n");

                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
                                constante++;
                            }
                            else if (oFact.ListaExtra[i].ExDato.Contains("*p*") || oFact.ListaExtra[i].ExDato.Contains("*P*"))
                            {
                                var linejump = oFact.ListaExtra[i].ExDato.Replace("*p*", "\n");
                                linejump = linejump.Replace("*P*", "\n");

                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
                                constante++;
                            }
                            else
                            {
                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), oFact.ListaExtra[i].ExDato);
                                constante++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
                        constante++;
                    }
                }

                #endregion END MANIPULACION DE LA LISTA EXTRAS
            }
            catch (Exception ex)
            {
                return Dpr;
            }
            return Dpr;
        }

        #region RESUMEN

        //public ReportParameter[] GetArrayParametersNotCred_Resumen(FacturaElectronica oFact, ListaFacturaElectronica listamonto, decimal ImporteTotal_Res)
        //{
        //    ReportParameter[] Dpr = new ReportParameter[42];
        //    try
        //    {
        //        Dpr[0] = new ReportParameter("EmisorRuc", oFact.Empresa.RUC);
        //        Dpr[1] = new ReportParameter("EmisorRazonSocial", oFact.Empresa.RazonSocial);
        //        Dpr[2] = new ReportParameter("EmisorDireccion", oFact.Empresa.Direccion);
        //        Dpr[3] = new ReportParameter("SerieNumeroDocumento", oFact.NumeroSerie + " - " + oFact.NumeroDocumento);
        //        Dpr[4] = new ReportParameter("ClienteRazonSocial", oFact.Cliente.RazonSocial);
        //        Dpr[5] = new ReportParameter("ClienteDireccion", oFact.Cliente.Direccion);
        //        Dpr[6] = new ReportParameter("ClienteRuc", oFact.Cliente.ClienteRuc);
        //        Dpr[7] = new ReportParameter("FechaEmision", oFact.FechaEmision.ToString("dd/MM/yyyy"));
        //        Dpr[8] = new ReportParameter("Moneda", oFact.TipoMoneda);
        //        Dpr[9] = new ReportParameter("TextoNeto", oFact.MontoTotalLetras.Remove(0, 4));//TextoNexto.Remove(0, 4)
        //        Dpr[10] = new ReportParameter("Importe", listamonto[5].MontoTotalCad);

        //        Dpr[11] = new ReportParameter("TotalGravado", listamonto[0].MontoTotalCad);
        //        Dpr[12] = new ReportParameter("TotalnoGravado", listamonto[1].MontoTotalCad);
        //        Dpr[13] = new ReportParameter("TotalExonerado", listamonto[2].MontoTotalCad);
        //        Dpr[14] = new ReportParameter("TotalDescuento", listamonto[3].MontoTotalCad);
        //        Dpr[15] = new ReportParameter("TotalIGV", listamonto[4].MontoTotalCad);
        //        Dpr[16] = new ReportParameter("ImporteTotal", listamonto[5].MontoTotalCad);

        //        if (oFact.TotalDetracciones == null)
        //        {
        //            Dpr[17] = new ReportParameter("Detraccion", "1");
        //        }
        //        else
        //        {
        //            Dpr[17] = new ReportParameter("Detraccion", oFact.TotalDetracciones);
        //        }

        //        Dpr[18] = new ReportParameter("NroOrdCompra", oFact.NroOrdCompra);

        //        //FOR HIGH PDF
        //        string tipodocafectado = string.Empty;
        //        string nrodocafectado = string.Empty;
        //        string saltolinea = string.Empty;

        //        foreach (var tpodocafec in oFact.ListaAfectado)
        //        {
        //            if (tpodocafec.CodigoTipoDocumento == Constantes.Factura)
        //            { tipodocafectado += saltolinea + Constantes.FacturaDesc; }
        //            else if (tpodocafec.CodigoTipoDocumento == Constantes.Boleta)
        //            { tipodocafectado += saltolinea + Constantes.BoletaDesc; }
        //            else if (tpodocafec.CodigoTipoDocumento == Constantes.TicketMaqRegistradora)
        //            { tipodocafectado += saltolinea + Constantes.TicketMaqRegistradoraDesc; }
        //            else
        //            { tipodocafectado += saltolinea + tpodocafec.CodigoTipoDocumento; }
        //            nrodocafectado += saltolinea + tpodocafec.ID;
        //            saltolinea = Constantes.SaltoLinea;
        //        }


        //        //FOR SMALL PDF
        //        for (int j = 0; j <= oFact.ListaAfectado.Count - 1; j++)
        //        {
        //            switch (oFact.ListaAfectado[j].CodigoTipoDocumento)
        //            {
        //                case Constantes.Boleta:
        //                    {
        //                        oFact.ListaAfectado[j].ID = oFact.ListaAfectado[j].ID;
        //                        oFact.ListaAfectado[j].CodigoTipoDocumento = Constantes.BoletaDesc;
        //                        break;
        //                    }
        //                case Constantes.Factura:
        //                    {
        //                        oFact.ListaAfectado[j].ID = oFact.ListaAfectado[j].ID;
        //                        oFact.ListaAfectado[j].CodigoTipoDocumento = Constantes.FacturaDesc;
        //                        break;
        //                    }
        //                case Constantes.TicketMaqRegistradora:
        //                    {
        //                        oFact.ListaAfectado[j].ID = oFact.ListaAfectado[j].ID;
        //                        oFact.ListaAfectado[j].CodigoTipoDocumento = Constantes.TicketMaqRegistradoraDesc;
        //                        break;
        //                    }
        //            }
        //        }

        //        Dpr[19] = new ReportParameter("TipoDocAfectado", tipodocafectado);
        //        Dpr[20] = new ReportParameter("NroDocAfectado", nrodocafectado);
        //        Dpr[21] = new ReportParameter("MotivoAnulado", oFact.MotivoAnulado);
        //        Dpr[22] = new ReportParameter("ValueTpoDocCli", Helper.ListClass.GetValue_TypeDocClie(oFact.Cliente.TipoDocumentoIdentidad.Codigo));

        //        #region MANIPULACION DE LA LISTA EXTRAS

        //        int constante = 23;
        //        for (int i = 0; i <= 17; i++)
        //        {
        //            try
        //            {
        //                if (oFact.ListaExtra[i] == null)
        //                {
        //                    Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
        //                    constante++;
        //                }
        //                else
        //                {
        //                    if (oFact.ListaExtra[i].ExDato.Contains("*b*") || oFact.ListaExtra[i].ExDato.Contains("*B*"))
        //                    {
        //                        var linejump = oFact.ListaExtra[i].ExDato.Replace("*b*", "\n");
        //                        linejump = linejump.Replace("*B*", "\n");

        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        constante++;
        //                    }
        //                    else if (oFact.ListaExtra[i].ExDato.Contains("*p*") || oFact.ListaExtra[i].ExDato.Contains("*P*"))
        //                    {
        //                        var linejump = oFact.ListaExtra[i].ExDato.Replace("*p*", "\n");
        //                        linejump = linejump.Replace("*P*", "\n");

        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        constante++;
        //                    }
        //                    else
        //                    {
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), oFact.ListaExtra[i].ExDato);
        //                        constante++;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
        //                constante++;
        //            }
        //        }

        //        #endregion END MANIPULACION DE LA LISTA EXTRAS

        //        Dpr[41] = new ReportParameter("ImporteTotal_Res", ImporteTotal_Res + string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Dpr;
        //    }
        //    return Dpr;
        //}

        #endregion

        public ReportParameter[] GetArrayParametersNotDebit(FacturaElectronica oFact, ListaFacturaElectronica listamonto)
        {
            ReportParameter[] Dpr = new ReportParameter[49];
            try
            {
                Dpr[0] = new ReportParameter("EmisorRuc", oFact.Empresa.RUC);
                Dpr[1] = new ReportParameter("EmisorRazonSocial", oFact.Empresa.RazonSocial);
                Dpr[2] = new ReportParameter("EmisorDireccion", oFact.Empresa.Direccion);
                Dpr[3] = new ReportParameter("SerieNumeroDocumento", oFact.NumeroSerie + " - " + oFact.NumeroDocumento);
                Dpr[4] = new ReportParameter("ClienteRazonSocial", oFact.Cliente.RazonSocial);
                Dpr[5] = new ReportParameter("ClienteDireccion", oFact.Cliente.Direccion);
                Dpr[6] = new ReportParameter("ClienteRuc", oFact.Cliente.ClienteRuc);
                Dpr[7] = new ReportParameter("FechaEmision", oFact.FechaEmision.ToString("dd/MM/yyyy"));
                Dpr[8] = new ReportParameter("Moneda", oFact.TipoMoneda);
                Dpr[9] = new ReportParameter("TextoNeto", oFact.MontoTotalLetras);//TextoNexto.Remove(0, 4)
                //Dpr[10] = new ReportParameter("Importe", listamonto[5].MontoTotalCad);
                //Dpr[11] = new ReportParameter("TotalGravado", listamonto[0].MontoTotalCad);
                //Dpr[12] = new ReportParameter("TotalnoGravado", listamonto[1].MontoTotalCad);
                //Dpr[13] = new ReportParameter("TotalExonerado", listamonto[2].MontoTotalCad);
                //Dpr[14] = new ReportParameter("TotalDescuento", listamonto[3].MontoTotalCad);
                //Dpr[15] = new ReportParameter("TotalIGV", listamonto[4].MontoTotalCad);
                //Dpr[16] = new ReportParameter("ImporteTotal", listamonto[5].MontoTotalCad);

                //Dpr[10] = new ReportParameter("Importe", listamonto[5].MontoTotal + string.Empty);
                //Dpr[11] = new ReportParameter("TotalGravado", listamonto[0].MontoTotal + string.Empty);
                //Dpr[12] = new ReportParameter("TotalnoGravado", listamonto[1].MontoTotal + string.Empty);
                //Dpr[13] = new ReportParameter("TotalExonerado", listamonto[2].MontoTotal + string.Empty);
                //Dpr[14] = new ReportParameter("TotalDescuento", listamonto[3].MontoTotal + string.Empty);
                //Dpr[15] = new ReportParameter("TotalIGV", listamonto[4].MontoTotal + string.Empty);
                //Dpr[16] = new ReportParameter("ImporteTotal", Convert.ToDecimal(listamonto[5].MontoTotal + string.Empty, CultureInfo.CreateSpecificCulture("es-PE")) + string.Empty);

                Dpr[10] = new ReportParameter("Importe", oFact.MontoTotal + string.Empty);
                Dpr[11] = new ReportParameter("TotalGravado", oFact.TotalGravadoSinIGV + string.Empty);
                Dpr[12] = new ReportParameter("TotalnoGravado", oFact.TotalInafecto_mon + string.Empty);
                Dpr[13] = new ReportParameter("TotalExonerado", oFact.TotalExonerado_mon + string.Empty);
                Dpr[14] = new ReportParameter("TotalDescuento", oFact.TotalDescuento_mon + string.Empty);
                Dpr[15] = new ReportParameter("TotalIGV", oFact.MontoIGV + string.Empty);
                Dpr[16] = new ReportParameter("ImporteTotal", oFact.MontoTotal + string.Empty);


                if (oFact.TotalDetracciones == null)
                {
                    Dpr[17] = new ReportParameter("Detraccion", "1");
                }
                else
                {
                    //Dpr[17] = new ReportParameter("Detraccion", oFact.TotalDetracciones);
                    Dpr[17] = new ReportParameter("Detraccion", oFact.MontoDetraccion + string.Empty);
                }

                Dpr[18] = new ReportParameter("NroOrdCompra", oFact.NroOrdCompra);

                string tipodocafectado = string.Empty;
                string nrodocafectado = string.Empty;
                string saltolinea = string.Empty;

                foreach (var tpodocafec in oFact.ListaAfectado)
                {
                    if (tpodocafec.CodigoTipoDocumento == Constantes.Factura)
                    { tipodocafectado += saltolinea + Constantes.FacturaDesc; }
                    else if (tpodocafec.CodigoTipoDocumento == Constantes.Boleta)
                    { tipodocafectado += saltolinea + Constantes.BoletaDesc; }
                    else if (tpodocafec.CodigoTipoDocumento == Constantes.TicketMaqRegistradora)
                    { tipodocafectado += saltolinea + Constantes.TicketMaqRegistradoraDesc; }
                    else
                    { tipodocafectado += saltolinea + tpodocafec.CodigoTipoDocumento; }
                    nrodocafectado += saltolinea + tpodocafec.ID;
                    saltolinea = Constantes.SaltoLinea;
                }

                for (int j = 0; j <= oFact.ListaAfectado.Count - 1; j++)
                {
                    switch (oFact.ListaAfectado[j].CodigoTipoDocumento)
                    {
                        case Constantes.Boleta:
                            {
                                oFact.ListaAfectado[j].ID = oFact.ListaAfectado[j].ID;
                                oFact.ListaAfectado[j].CodigoTipoDocumento = Constantes.BoletaDesc;
                                break;
                            }
                        case Constantes.Factura:
                            {
                                oFact.ListaAfectado[j].ID = oFact.ListaAfectado[j].ID;
                                oFact.ListaAfectado[j].CodigoTipoDocumento = Constantes.FacturaDesc;
                                break;
                            }
                        case Constantes.TicketMaqRegistradora:
                            {
                                oFact.ListaAfectado[j].ID = oFact.ListaAfectado[j].ID;
                                oFact.ListaAfectado[j].CodigoTipoDocumento = Constantes.TicketMaqRegistradoraDesc;
                                break;
                            }
                    }
                }

                Dpr[19] = new ReportParameter("TipoDocAfectado", tipodocafectado);
                Dpr[20] = new ReportParameter("NroDocAfectado", nrodocafectado);
                Dpr[21] = new ReportParameter("MotivoAnulado", oFact.MotivoAnulado);

                //detraccion
                if (oFact.Detraccion.CtaDetraccion != null)
                {
                    if (oFact.Detraccion.CtaDetraccion.Length > Constantes.ValorCero)
                    {
                        Dpr[22] = new ReportParameter("CtaDetraccion", "Operación Sujeta al SPOT\nBanco de la Nación Cta. Cte. N° " + oFact.Detraccion.CtaDetraccion);
                    }
                }
                else
                {
                    Dpr[22] = new ReportParameter("CtaDetraccion", "000");
                }

                Dpr[23] = new ReportParameter("ValueTpoDocCli", Helper.ListClass.GetValue_TypeDocClie(oFact.Cliente.TipoDocumentoIdentidad.Codigo));

                Dpr[24] = new ReportParameter("CodTpoND", oFact.CodTpoND);
                Dpr[25] = new ReportParameter("DescTpoND", oFact.DescTpoND);
                Dpr[26] = new ReportParameter("TotalOtrosCargos", oFact.TotOtrosCargos + string.Empty);
                Dpr[27] = new ReportParameter("PorcentDetraccion", oFact.PorcentDetraccion);
                Dpr[28] = new ReportParameter("MontoDetraccion", oFact.MontoDetraccion + string.Empty);

                Dpr[29] = new ReportParameter("DirCliente", oFact.Campo10 + string.Empty);

                #region MANIPULACION DE LA LISTA EXTRAS

                int constante = 30;
                for (int i = 0; i <= 18; i++)
                {
                    try
                    {
                        if (oFact.ListaExtra[i] == null)
                        {
                            Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
                            constante++;
                        }
                        else
                        {
                            if (oFact.ListaExtra[i].ExDato.Contains("*b*") || oFact.ListaExtra[i].ExDato.Contains("*B*"))
                            {
                                var linejump = oFact.ListaExtra[i].ExDato.Replace("*b*", "\n");
                                linejump = linejump.Replace("*B*", "\n");

                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
                                constante++;
                            }
                            else if (oFact.ListaExtra[i].ExDato.Contains("*p*") || oFact.ListaExtra[i].ExDato.Contains("*P*"))
                            {
                                var linejump = oFact.ListaExtra[i].ExDato.Replace("*p*", "\n");
                                linejump = linejump.Replace("*P*", "\n");

                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
                                constante++;
                            }
                            else
                            {
                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), oFact.ListaExtra[i].ExDato);
                                constante++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
                        constante++;
                    }
                }

                #endregion END MANIPULACION DE LA LISTA EXTRAS
            }
            catch (Exception ex)
            {
                return Dpr;
            }
            return Dpr;
        }

        #region RESUMEN
        //chekear to resumen()
        //public ReportParameter[] GetArrayParametersNotDebit_Resumen(FacturaElectronica oFact, ListaFacturaElectronica listamonto, decimal ImporteTotal_Res)
        //{
        //    ReportParameter[] Dpr = new ReportParameter[43];
        //    try
        //    {
        //        Dpr[0] = new ReportParameter("EmisorRuc", oFact.Empresa.RUC);
        //        Dpr[1] = new ReportParameter("EmisorRazonSocial", oFact.Empresa.RazonSocial);
        //        Dpr[2] = new ReportParameter("EmisorDireccion", oFact.Empresa.Direccion);
        //        Dpr[3] = new ReportParameter("SerieNumeroDocumento", oFact.NumeroSerie + " - " + oFact.NumeroDocumento);
        //        Dpr[4] = new ReportParameter("ClienteRazonSocial", oFact.Cliente.RazonSocial);
        //        Dpr[5] = new ReportParameter("ClienteDireccion", oFact.Cliente.Direccion);
        //        Dpr[6] = new ReportParameter("ClienteRuc", oFact.Cliente.ClienteRuc);
        //        Dpr[7] = new ReportParameter("FechaEmision", oFact.FechaEmision.ToString("dd/MM/yyyy"));
        //        Dpr[8] = new ReportParameter("Moneda", oFact.TipoMoneda);
        //        Dpr[9] = new ReportParameter("TextoNeto", oFact.MontoTotalLetras.Remove(0, 4));//TextoNexto.Remove(0, 4)
        //        Dpr[10] = new ReportParameter("Importe", listamonto[5].MontoTotalCad);

        //        Dpr[11] = new ReportParameter("TotalGravado", listamonto[0].MontoTotalCad);
        //        Dpr[12] = new ReportParameter("TotalnoGravado", listamonto[1].MontoTotalCad);
        //        Dpr[13] = new ReportParameter("TotalExonerado", listamonto[2].MontoTotalCad);
        //        Dpr[14] = new ReportParameter("TotalDescuento", listamonto[3].MontoTotalCad);
        //        Dpr[15] = new ReportParameter("TotalIGV", listamonto[4].MontoTotalCad);
        //        Dpr[16] = new ReportParameter("ImporteTotal", listamonto[5].MontoTotalCad);


        //        if (oFact.TotalDetracciones == null)
        //        {
        //            Dpr[17] = new ReportParameter("Detraccion", "1");
        //        }
        //        else
        //        {
        //            Dpr[17] = new ReportParameter("Detraccion", oFact.TotalDetracciones);
        //        }

        //        Dpr[18] = new ReportParameter("NroOrdCompra", oFact.NroOrdCompra);

        //        string tipodocafectado = string.Empty;
        //        string nrodocafectado = string.Empty;
        //        string saltolinea = string.Empty;

        //        foreach (var tpodocafec in oFact.ListaAfectado)
        //        {
        //            if (tpodocafec.CodigoTipoDocumento == Constantes.Factura)
        //            {
        //                tipodocafectado += saltolinea + Constantes.FacturaDesc;
        //            }
        //            else if (tpodocafec.CodigoTipoDocumento == Constantes.Boleta)
        //            {
        //                tipodocafectado += saltolinea + Constantes.BoletaDesc;
        //            }
        //            else if (tpodocafec.CodigoTipoDocumento == Constantes.TicketMaqRegistradora)
        //            {
        //                tipodocafectado += saltolinea + Constantes.TicketMaqRegistradoraDesc;
        //            }
        //            else
        //            {
        //                tipodocafectado += saltolinea + tpodocafec.CodigoTipoDocumento;
        //            }
        //            nrodocafectado += saltolinea + tpodocafec.ID;
        //            saltolinea = Constantes.SaltoLinea;
        //        }

        //        for (int j = 0; j <= oFact.ListaAfectado.Count - 1; j++)
        //        {
        //            switch (oFact.ListaAfectado[j].CodigoTipoDocumento)
        //            {
        //                case Constantes.Boleta:
        //                    {
        //                        oFact.ListaAfectado[j].ID = oFact.ListaAfectado[j].ID;
        //                        oFact.ListaAfectado[j].CodigoTipoDocumento = Constantes.BoletaDesc;
        //                        break;
        //                    }
        //                case Constantes.Factura:
        //                    {
        //                        oFact.ListaAfectado[j].ID = oFact.ListaAfectado[j].ID;
        //                        oFact.ListaAfectado[j].CodigoTipoDocumento = Constantes.FacturaDesc;
        //                        break;
        //                    }
        //                case Constantes.TicketMaqRegistradora:
        //                    {
        //                        oFact.ListaAfectado[j].ID = oFact.ListaAfectado[j].ID;
        //                        oFact.ListaAfectado[j].CodigoTipoDocumento = Constantes.TicketMaqRegistradoraDesc;
        //                        break;
        //                    }
        //            }
        //        }

        //        Dpr[19] = new ReportParameter("TipoDocAfectado", tipodocafectado);
        //        Dpr[20] = new ReportParameter("NroDocAfectado", nrodocafectado);
        //        Dpr[21] = new ReportParameter("MotivoAnulado", oFact.MotivoAnulado);

        //        //detraccion
        //        if (oFact.Detraccion.CtaDetraccion != null)
        //        {
        //            if (oFact.Detraccion.CtaDetraccion.Length > Constantes.ValorCero)
        //            {
        //                Dpr[22] = new ReportParameter("CtaDetraccion", "Operación Sujeta al SPOT\nBanco de la Nación Cta. Cte. N° " + oFact.Detraccion.CtaDetraccion);
        //            }
        //        }
        //        else
        //        {
        //            Dpr[22] = new ReportParameter("CtaDetraccion", "000");
        //        }

        //        Dpr[23] = new ReportParameter("ValueTpoDocCli", Helper.ListClass.GetValue_TypeDocClie(oFact.Cliente.TipoDocumentoIdentidad.Codigo));

        //        #region MANIPULACION DE LA LISTA EXTRAS

        //        int constante = 24;
        //        for (int i = 0; i <= 17; i++)
        //        {
        //            try
        //            {
        //                if (oFact.ListaExtra[i] == null)
        //                {
        //                    Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
        //                    constante++;
        //                }
        //                else
        //                {
        //                    if (oFact.ListaExtra[i].ExDato.Contains("*b*") || oFact.ListaExtra[i].ExDato.Contains("*B*"))
        //                    {
        //                        var linejump = oFact.ListaExtra[i].ExDato.Replace("*b*", "\n");
        //                        linejump = linejump.Replace("*B*", "\n");

        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        constante++;
        //                    }
        //                    else if (oFact.ListaExtra[i].ExDato.Contains("*p*") || oFact.ListaExtra[i].ExDato.Contains("*P*"))
        //                    {
        //                        var linejump = oFact.ListaExtra[i].ExDato.Replace("*p*", "\n");
        //                        linejump = linejump.Replace("*P*", "\n");

        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
        //                        constante++;
        //                    }
        //                    else
        //                    {
        //                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), oFact.ListaExtra[i].ExDato);
        //                        constante++;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
        //                constante++;
        //            }
        //        }

        //        #endregion END MANIPULACION DE LA LISTA EXTRAS

        //        Dpr[42] = new ReportParameter("ImporteTotal_Res", ImporteTotal_Res + string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Dpr;
        //    }
        //    return Dpr;
        //}

        #endregion

        public ReportParameter[] GetArrayParametersRetenc(FacturaElectronica oFact)
        {
            ReportParameter[] Dpr = new ReportParameter[26];
            try
            {
                Dpr[0] = new ReportParameter("RucEmisor", oFact.Empresa.RUC);
                Dpr[1] = new ReportParameter("EmisorRazonSocial", oFact.Empresa.RazonSocial);
                Dpr[2] = new ReportParameter("EmisorDireccion", oFact.Empresa.Direccion);
                Dpr[3] = new ReportParameter("SerieCorrelativo", oFact.SerieCorrelativo);
                Dpr[4] = new ReportParameter("RazonSocialCliente", oFact.Cliente.RazonSocial);
                Dpr[5] = new ReportParameter("RucCliente", oFact.Cliente.ClienteRuc);
                Dpr[6] = new ReportParameter("DireccionCliente", oFact.Cliente.Direccion);
                Dpr[7] = new ReportParameter("FechaEmision", oFact.FechaEmision2);
                Dpr[8] = new ReportParameter("TasaRetencion", oFact.TasaRetencion);
                Dpr[9] = new ReportParameter("ImpTotRetenido", oFact.ImporteTotalRetenido + string.Empty);
                Dpr[10] = new ReportParameter("ImpTotPagado", oFact.ImporteTotalPagado + string.Empty);

                Dpr[11] = new ReportParameter("TipoCambio", oFact.TipoCambio + string.Empty);
                Dpr[12] = new ReportParameter("FechaTipoCambio", oFact.FechaCambio);

                Dpr[13] = new ReportParameter("TextoNeto", oFact.MontoTotalLetras.Remove(0, 4));

                Dpr[14] = new ReportParameter("ImpGlobSoles", oFact.ImpGlobSoles + string.Empty);

                #region MANIPULACION DE LA LISTA EXTRAS

                int constante = 15;
                for (int i = 0; i <= 10; i++)
                {
                    try
                    {
                        if (oFact.ListaExtra[i] == null)
                        {
                            Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
                            constante++;
                        }
                        else
                        {
                            if (oFact.ListaExtra[i].ExDato.Contains("*b*") || oFact.ListaExtra[i].ExDato.Contains("*B*"))
                            {
                                var linejump = oFact.ListaExtra[i].ExDato.Replace("*b*", "\n");
                                linejump = linejump.Replace("*B*", "\n");

                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
                                constante++;
                            }
                            else if (oFact.ListaExtra[i].ExDato.Contains("*p*") || oFact.ListaExtra[i].ExDato.Contains("*P*"))
                            {
                                var linejump = oFact.ListaExtra[i].ExDato.Replace("*p*", "\n");
                                linejump = linejump.Replace("*P*", "\n");

                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), linejump);
                                constante++;
                            }
                            else
                            {
                                Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), oFact.ListaExtra[i].ExDato);
                                constante++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Dpr[constante] = new ReportParameter("Extra" + (i + 1).ToString(), string.Empty);
                        constante++;
                    }
                }

                #endregion END MANIPULACION DE LA LISTA EXTRAS
            }
            catch (Exception ex)
            {
                return Dpr;
            }
            return Dpr;
        }

        #endregion


        #region Set Montos, CodeBar

        public ListaFacturaElectronica GetListaMontos(FacturaElectronica oFact, string RutaPDF417, string RutaBarcode)
        {
            ListaFacturaElectronica listamonto = new ListaFacturaElectronica();
            try
            {
                #region CASE 

                switch (oFact.TipoDocumento.CodigoDocumento)
                {
                    case Constantes.Factura:
                    case Constantes.Boleta:
                        {
                            {
                                listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Importe Total", MontoTotal = oFact.MontoTotal, CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                                listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total IGV(%18)", MontoTotal = Convert.ToDecimal(oFact.MontoIgvCad, CultureInfo.CreateSpecificCulture("es-PE")), CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                                listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Total Dscto", MontoTotal = Convert.ToDecimal(oFact.TotalDescuento, CultureInfo.CreateSpecificCulture("es-PE")), CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                                listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Exonerado", MontoTotal = Convert.ToDecimal(oFact.TotalExonerado, CultureInfo.CreateSpecificCulture("es-PE")), CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                                listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total no Gravado", MontoTotal = Convert.ToDecimal(oFact.TotalInafecto, CultureInfo.CreateSpecificCulture("es-PE")), CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                                listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Gravado", MontoTotal = Convert.ToDecimal(oFact.TotalGravado, CultureInfo.CreateSpecificCulture("es-PE")), CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });



                                //listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Importe Total", MontoTotal = decimal.Parse(oFact.MontoTotalCad), CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                                //listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total IGV(%18)", MontoTotal = decimal.Parse(oFact.MontoIgvCad), CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                                //listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Total Dscto", MontoTotal = decimal.Parse(oFact.TotalDescuento), CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                                //listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Exonerado", MontoTotal = decimal.Parse(oFact.TotalExonerado), CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                                //listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total no Gravado", MontoTotal = decimal.Parse(oFact.TotalInafecto), CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                                ////listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Gravado", MontoTotal = decimal.Parse(oFact.TotalGravado), CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                                //listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Gravado", MontoTotal = oFact.TotalGravadoSinIGV, CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            }
                            break;
                        }
                    case Constantes.NotaCredito:
                        {
                            listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Importe Total", MontoTotalCad = oFact.MontoTotalCad, CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total IGV(%18)", MontoTotalCad = oFact.MontoIgvCad, CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Total Dscto", MontoTotalCad = oFact.TotalDescuento, CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Exonerado", MontoTotalCad = oFact.TotalExonerado, CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total no Gravado", MontoTotalCad = oFact.TotalInafecto, CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Gravado", MontoTotalCad = Convert.ToDecimal(oFact.TotalGravado, CultureInfo.CreateSpecificCulture("es-PE")) + string.Empty, CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            //listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Importe Total", MontoTotal = decimal.Parse(oFact.MontoTotalCad, CultureInfo.CreateSpecificCulture("es-PE")), CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            //listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total IGV(%18)", MontoTotal = decimal.Parse(oFact.MontoIgvCad, CultureInfo.CreateSpecificCulture("es-PE")), CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            //listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Total Dscto", MontoTotal = decimal.Parse(oFact.TotalDescuento, CultureInfo.CreateSpecificCulture("es-PE")), CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            //listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Exonerado", MontoTotal = decimal.Parse(oFact.TotalExonerado, CultureInfo.CreateSpecificCulture("es-PE")), CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            //listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total no Gravado", MontoTotal = decimal.Parse(oFact.TotalInafecto, CultureInfo.CreateSpecificCulture("es-PE")), CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            ////listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Gravado", MontoTotal = decimal.Parse(oFact.TotalGravado), CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            //listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Gravado", MontoTotal = oFact.TotalGravadoSinIGV, CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });

                            break;
                        }
                    case Constantes.NotaDebito:
                        {
                            listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Importe Total", MontoTotalCad = oFact.MontoTotalCad, CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total IGV(%18)", MontoTotalCad = oFact.MontoIgvCad, CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Total Dscto", MontoTotalCad = oFact.TotalDescuento, CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Exonerado", MontoTotalCad = oFact.TotalExonerado, CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total no Gravado", MontoTotalCad = oFact.TotalInafecto, CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Total Gravado", MontoTotalCad = Convert.ToDecimal(oFact.TotalGravado, CultureInfo.CreateSpecificCulture("es-PE")) + string.Empty, CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            break;
                        }
                    case Constantes.Retencion:
                        {
                            listamonto.Insert(0, new FacturaElectronica { TipoMonto = "Importe Total", MontoTotalCad = oFact.DocCRECPE.ImporteTotalPagado + string.Empty, CodigoPDF417 = File.ReadAllBytes(RutaPDF417), LogoEmpresa = File.ReadAllBytes(RutaBarcode) });
                            break;
                        }

                    case Constantes.Percepcion:
                        {
                            break;
                        }
                }
                #endregion END CASE
            }
            catch (Exception ex)
            {
                return listamonto;
            }
            return listamonto;
        }

        public string GetValueForCodePDF417(FacturaElectronica oFact)
        {
            string contenidoCodigoPDF417 = string.Empty;
            try
            {
                string val1 = string.Empty; string val2 = string.Empty; string val3 = string.Empty; string val4 = string.Empty; string val5 = string.Empty;

                #region CASE

                switch (oFact.TipoDocumento.CodigoDocumento)
                {
                    case Constantes.Factura:
                    case Constantes.Boleta:
                        {
                            val1 = oFact.Empresa.RUC + "|" + oFact.TipoDocumento.CodigoDocumento + "|" + oFact.NumeroSerie + "|" + oFact.NumeroDocumento;
                            val2 = oFact.MontoIgvCad + "|" + oFact.MontoTotalCad;
                            val3 = oFact.FechaEmision2; //fecha para el codigo de barra pdf417
                            val4 = oFact.Cliente.TipoDocumentoIdentidad.Codigo + "|" + oFact.Cliente.ClienteRuc;
                            val5 = oFact.ValorResumen + "|" + oFact.ValorFirma;
                            break;
                        }
                    case Constantes.NotaCredito:
                        {
                            val1 = oFact.Empresa.RUC + "|" + oFact.TipoDocumento.CodigoDocumento + "|" + oFact.NumeroSerie + "|" + oFact.NumeroDocumento;
                            val2 = oFact.MontoIgvCad + "|" + oFact.MontoTotalCad;
                            val3 = oFact.FechaEmision2; //fecha para el codigo de barra pdf417
                            val4 = oFact.Cliente.TipoDocumentoIdentidad.Codigo + "|" + oFact.Cliente.ClienteRuc;
                            val5 = oFact.ValorResumen + "|" + oFact.ValorFirma;
                            break;
                        }
                    case Constantes.NotaDebito:
                        {
                            val1 = oFact.Empresa.RUC + "|" + oFact.TipoDocumento.CodigoDocumento + "|" + oFact.NumeroSerie + "|" + oFact.NumeroDocumento;
                            val2 = oFact.MontoIgvCad + "|" + oFact.MontoTotalCad;
                            val3 = oFact.FechaEmision2; //fecha para el codigo de barra pdf417
                            val4 = oFact.Cliente.TipoDocumentoIdentidad.Codigo + "|" + oFact.Cliente.ClienteRuc;
                            val5 = oFact.ValorResumen + "|" + oFact.ValorFirma;
                            break;
                        }
                    case Constantes.Retencion:
                        {
                            val1 = oFact.Empresa.RUC + "|" + oFact.TipoDocumento.CodigoDocumento + "|" + oFact.NumeroSerie + "|" + oFact.NumeroDocumento;
                            //val2 = oFact.DocCRECPE.ImporteTotalRetenido + "|" + oFact.DocCRECPE.ImporteTotalPagado;
                            val2 = oFact.ImporteTotalRetenido + "|" + oFact.ImporteTotalPagado;
                            val3 = oFact.FechaEmision2; //fecha para el codigo de barra pdf417
                            val4 = oFact.Cliente.TipoDocumentoIdentidad.Codigo + "|" + oFact.Cliente.ClienteRuc;
                            val5 = oFact.ValorResumen + "|" + oFact.ValorFirma;
                            break;
                        }

                    case Constantes.Percepcion:
                        {
                            val1 = oFact.Empresa.RUC + "|" + oFact.TipoDocumento.CodigoDocumento + "|" + oFact.NumeroSerie + "|" + oFact.NumeroDocumento;
                            val2 = oFact.MontoIgvCad + "|" + oFact.MontoTotalCad;
                            val3 = oFact.FechaEmision2; //fecha para el codigo de barra pdf417
                            val4 = oFact.Cliente.TipoDocumentoIdentidad.Codigo + "|" + oFact.Cliente.ClienteRuc;
                            val5 = oFact.ValorResumen + "|" + oFact.ValorFirma;
                            break;
                        }
                }

                #endregion END CASE

                contenidoCodigoPDF417 = val1 + "|" + val2 + "|" + val3 + "|" + val4 + "|" + val5;
            }
            catch (Exception ex)
            {
                return contenidoCodigoPDF417;
            }
            return contenidoCodigoPDF417;
        }

        #endregion


        #region PATH INFORME
        public string GetPathReportviewer(FacturaElectronica oFact, int typeFormat)
        {
            string resultPath = string.Empty;

            try
            {
                switch (typeFormat)
                {
                    case Constantes.ValorUno:
                        {
                            #region TERMIC
                            switch (oFact.TipoDocumento.CodigoDocumento)
                            {
                                case Constantes.Factura:
                                    {
                                        resultPath = "Report/Reporte/Fact_T_" + oFact.Empresa.RUC + ".rdlc";
                                        break;
                                    }
                                case Constantes.Boleta:
                                    {
                                        resultPath = "Report/Reporte/Bol_T_" + oFact.Empresa.RUC + ".rdlc";
                                        break;
                                    }
                                case Constantes.NotaCredito:
                                    {
                                        resultPath = "Report/Reporte/NotC_T_" + oFact.Empresa.RUC + ".rdlc";
                                        break;
                                    }
                                case Constantes.NotaDebito:
                                    {
                                        resultPath = "Report/Reporte/NotD_T_" + oFact.Empresa.RUC + ".rdlc";
                                        break;
                                    }
                                case Constantes.Retencion:
                                    {
                                        resultPath = "Report/Reporte/Ret_T_" + oFact.Empresa.RUC + ".rdlc";
                                        break;
                                    }
                                case Constantes.Percepcion:
                                    {
                                        resultPath = "Report/Reporte/Per_T_" + oFact.Empresa.RUC + ".rdlc";
                                        break;
                                    }
                            }
                            #endregion

                            break;
                        }
                    case Constantes.ValorDos:
                        {
                            #region PDF

                            switch (oFact.TipoDocumento.CodigoDocumento)
                            {
                                case Constantes.Factura:
                                    {
                                        resultPath = "Report/Reporte/Fact_" + oFact.Empresa.RUC + ".rdlc";
                                        break;
                                    }
                                case Constantes.Boleta:
                                    {
                                        resultPath = "Report/Reporte/Bol_" + oFact.Empresa.RUC + ".rdlc";
                                        break;
                                    }
                                case Constantes.NotaCredito:
                                    {
                                        resultPath = "Report/Reporte/NotC_" + oFact.Empresa.RUC + ".rdlc";
                                        break;
                                    }
                                case Constantes.NotaDebito:
                                    {
                                        resultPath = "Report/Reporte/NotD_" + oFact.Empresa.RUC + ".rdlc";
                                        break;
                                    }
                                case Constantes.Retencion:
                                    {
                                        resultPath = "Report/Reporte/Ret_" + oFact.Empresa.RUC + ".rdlc";
                                        break;
                                    }
                                case Constantes.Percepcion:
                                    {
                                        resultPath = "Report/Reporte/Per_" + oFact.Empresa.RUC + ".rdlc";
                                        break;
                                    }
                            }

                            #endregion
                            break;
                        }
                    case Constantes.ValorCero:
                        {
                            #region PDF

                            switch (oFact.TipoDocumento.CodigoDocumento)
                            {
                                case Constantes.Factura:
                                    {
                                        resultPath = "Report/Reporte/Fact_" + oFact.Empresa.RUC + ".rdlc";
                                        break;
                                    }
                                case Constantes.Boleta:
                                    {
                                        resultPath = "Report/Reporte/Bol_" + oFact.Empresa.RUC + ".rdlc";
                                        break;
                                    }
                                case Constantes.NotaCredito:
                                    {
                                        resultPath = "Report/Reporte/NotC_" + oFact.Empresa.RUC + ".rdlc";
                                        break;
                                    }
                                case Constantes.NotaDebito:
                                    {
                                        resultPath = "Report/Reporte/NotD_" + oFact.Empresa.RUC + ".rdlc";
                                        break;
                                    }
                                case Constantes.Retencion:
                                    {
                                        resultPath = "Report/Reporte/Ret_" + oFact.Empresa.RUC + ".rdlc";
                                        break;
                                    }
                                case Constantes.Percepcion:
                                    {
                                        resultPath = "Report/Reporte/Per_" + oFact.Empresa.RUC + ".rdlc";
                                        break;
                                    }
                            }

                            #endregion
                            break;
                        }
                }
            }
            catch (Exception ex) { }
            return resultPath;
        }
        #endregion
    }
}
