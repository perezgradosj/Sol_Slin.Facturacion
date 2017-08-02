using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Slin.Facturacion.DataAccess.Helper;

using System.Configuration;
using Slin.Facturacion.BusinessEntities;

using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

using System.Xml;
using System.Data.SqlTypes;
using System.Globalization;
using System.Xml.Serialization;
using System.IO;
using Slin.Facturacion.Common.Method;

namespace Slin.Facturacion.DataAccess
{
    public class FacturacionDataAccess
    {
        static string PathLogSLINADE = Conexion.Cadena;

        static string cadena = "Server=" + Conexion.Host + ";Database=" + Conexion.BD + ";User=" + Conexion.USER + ";pwd=" + Conexion.PWD;

        SqlConnection cnn = new SqlConnection(cadena);

        StringBuilder logError = new StringBuilder();
        List<string> listError = new List<string>();


        //#region OTHERS
        //void Singleton.Instance.CreateDirectory(string path)
        //{
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.Singleton.Instance.CreateDirectory(path);
        //    }
        //}
        //#endregion


        public ListaFacturaElectronica ListarDocumentoCabecera(FacturaElectronica oFacturaElectronica)
        {
            FacturaElectronica objFactura = new FacturaElectronica();
            ListaFacturaElectronica objListaFactura = new ListaFacturaElectronica();

            try
            {
                SqlCommand oDbCommand = new SqlCommand();

                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_ListaDocumentoCabecera;
                oDbCommand.Connection = cnn;

                oDbCommand.Parameters.AddWithValue("@FechaInicio", oFacturaElectronica.FechaInicio);
                oDbCommand.Parameters.AddWithValue("@FechaFin", oFacturaElectronica.FechaFin);
                oDbCommand.Parameters.AddWithValue("@TipoDocumento", oFacturaElectronica.TipoDocumento.CodigoDocumento);
                oDbCommand.Parameters.AddWithValue("@Serie", oFacturaElectronica.Serie.NumeroSerie);
                oDbCommand.Parameters.AddWithValue("@NumeroDocumentoInicio", oFacturaElectronica.NumeroDocumentoInicio);
                oDbCommand.Parameters.AddWithValue("@NumeroDocumentoFin", oFacturaElectronica.NumeroDocumentoFin);
                oDbCommand.Parameters.AddWithValue("@Estado", oFacturaElectronica.Estado.IdEstado);
                oDbCommand.Parameters.AddWithValue("@Ruc", oFacturaElectronica.Cliente.ClienteRuc);
                oDbCommand.Parameters.AddWithValue("@RazonSocial", oFacturaElectronica.Cliente.Nombres);
                oDbCommand.Parameters.AddWithValue("@RucEmpresaEmisor", oFacturaElectronica.Empresa.RUC);
                oDbCommand.Parameters.AddWithValue("@Id_ED_DOC", oFacturaElectronica.Estado.IdEstadoSUNAT);

                oDbCommand.Parameters.AddWithValue("@NameSede", oFacturaElectronica.Sede);

                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("Nro");
                    int indexCodigoDocumento = objReader.GetOrdinal("CodigoDocumento");
                    int indexDescripcionTipoDocumento = objReader.GetOrdinal("Descripcion");
                    int indexSerie = objReader.GetOrdinal("Serie");
                    int indexNombreArchivoXML = objReader.GetOrdinal("NumeroDocumento");
                    int indexNumeroDocumento = objReader.GetOrdinal("NumeroFactura");
                    int indexFechaEmision = objReader.GetOrdinal("FechaEmision");
                    int indexNroDocClient = objReader.GetOrdinal("NumDocCliente");
                    int indexClienteDireccion = objReader.GetOrdinal("ClienteDireccion");
                    int indexCliente = objReader.GetOrdinal("Cliente");
                    int indexMontoTotal = objReader.GetOrdinal("MontoTotal");
                    int indexEstado = objReader.GetOrdinal("Estado");
                    int indexRutaImagen = objReader.GetOrdinal("RutaImagen");
                    int indexMoneda = objReader.GetOrdinal("Moneda");

                    int indexEmpresaRuc = objReader.GetOrdinal("EmpresaRuc");
                    int indexRazonSocial = objReader.GetOrdinal("RazonSocial");
                    int indexEmpresaDireccion = objReader.GetOrdinal("EmpresaDireccion");
                    int indexEmpresaUbigeo = objReader.GetOrdinal("EmpresaUbigeo");
                    int indexEmpresaUbigeoDesc = objReader.GetOrdinal("EmpresaUbigeoDesc");

                    int indexIdTipoDocumento = objReader.GetOrdinal("IdTipoDocumento");

                    int indexXML = objReader.GetOrdinal("XML");

                    int indexCDR = objReader.GetOrdinal("CDR");

                    int indexCampo1 = objReader.GetOrdinal("Campo1");

                    int indexCodeMessage = objReader.GetOrdinal("CodeMessage");
                    int indexDocMessage = objReader.GetOrdinal("DocMessage");

                    int indexCodeResponse = objReader.GetOrdinal("CodeResponse");
                    int indexNoteResponse = objReader.GetOrdinal("NoteResponse");

                    int indextypeFormat = objReader.GetOrdinal("TypeFormat");

                    //int indexImpresora = objReader.GetOrdinal("TypeFormat");

                    while (objReader.Read())
                    {
                        objFactura = new FacturaElectronica();
                        objFactura.Nro = DataUtil.DbValueToDefault<Int32>(objReader[indexNro]);
                        objFactura.TipoDocumento = new TipoDocumento();
                        objFactura.TipoDocumento.IdTipoDocumento = DataUtil.DbValueToDefault<int>(objReader[indexIdTipoDocumento]);
                        objFactura.TipoDocumento.CodigoDocumento = DataUtil.DbValueToDefault<String>(objReader[indexCodigoDocumento]);
                        objFactura.TipoDocumento.Descripcion = DataUtil.DbValueToDefault<String>(objReader[indexDescripcionTipoDocumento]);

                        objFactura.Serie = new Serie();
                        objFactura.Serie.NumeroSerie = DataUtil.DbValueToDefault<String>(objReader[indexSerie]);
                        objFactura.NombreArchivoXML = DataUtil.DbValueToDefault<string>(objReader[indexNombreArchivoXML]);
                        objFactura.NumeroDocumento = DataUtil.DbValueToDefault<String>(objReader[indexNumeroDocumento]);
                        objFactura.FechaEmision = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEmision]);
                        objFactura.FechaEmision2 = (DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEmision])).ToString("yyyy/MM/dd");
                        objFactura.Cliente = new Cliente();
                        objFactura.Cliente.NumeroDocumentoIdentidad = DataUtil.DbValueToDefault<String>(objReader[indexNroDocClient]);
                        objFactura.Cliente.Nombres = DataUtil.DbValueToDefault<String>(objReader[indexCliente]);
                        objFactura.Cliente.Direccion = DataUtil.DbValueToDefault<String>(objReader[indexClienteDireccion]);

                        objFactura.MontoTotal = DataUtil.DbValueToDefault<Decimal>(objReader[indexMontoTotal]);
                        objFactura.Estado = new Estado();
                        objFactura.Estado.Descripcion = DataUtil.DbValueToDefault<String>(objReader[indexEstado]);
                        objFactura.RutaImagen = DataUtil.DbValueToDefault<String>(objReader[indexRutaImagen]);
                        objFactura.Moneda = new Moneda();
                        objFactura.Moneda.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexMoneda]);

                        objFactura.Empresa = new Empresa();
                        objFactura.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexEmpresaRuc]);
                        objFactura.Empresa.RazonSocial = DataUtil.DbValueToDefault<string>(objReader[indexRazonSocial]);
                        objFactura.Empresa.Direccion = DataUtil.DbValueToDefault<String>(objReader[indexEmpresaDireccion]);

                        objFactura.Empresa.Ubigeo = new Ubigeo();
                        objFactura.Empresa.Ubigeo.CodigoUbigeo = DataUtil.DbValueToDefault<String>(objReader[indexEmpresaUbigeo]);
                        objFactura.Empresa.Ubigeo.Descripcion = DataUtil.DbValueToDefault<String>(objReader[indexEmpresaUbigeoDesc]);

                        string strData = string.Empty;
                        byte[] byteData = (byte[])objReader[indexXML];
                        strData = Encoding.GetEncoding("iso-8859-1").GetString(byteData);
                        objFactura.XML = strData;


                        strData = string.Empty;
                        byteData = null;
                        byteData = (byte[])objReader[indexCDR];
                        strData = Encoding.GetEncoding("iso-8859-1").GetString(byteData);
                        objFactura.CDR = strData;


                        objFactura.Campo1 = DataUtil.DbValueToDefault<String>(objReader[indexCampo1]);

                        objFactura.CodeMessage = DataUtil.DbValueToDefault<string>(objReader[indexCodeMessage]);
                        objFactura.DocMessage = DataUtil.DbValueToDefault<string>(objReader[indexDocMessage]);

                        objFactura.CodeResponse = DataUtil.DbValueToDefault<string>(objReader[indexCodeResponse]);
                        objFactura.NoteResponse = DataUtil.DbValueToDefault<string>(objReader[indexNoteResponse]);
                        objFactura.TypeFormat = DataUtil.DbValueToDefault<int>(objReader[indextypeFormat]);
                        objListaFactura.Add(objFactura);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: ListarDocumentoCabecera ] " + ex.Message);


                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return objListaFactura;
        }


        public ListaDetalleFacturaElectronica ListarDocumentoDetalle(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        {
            DetalleFacturaElectronica objDetalleDocumento = new DetalleFacturaElectronica();
            ListaDetalleFacturaElectronica objListaDetalleDocumento = new ListaDetalleFacturaElectronica();

            try
            {
                SqlCommand oDbCommand = new SqlCommand();

                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_ListaDocumentoDetalle;
                oDbCommand.Connection = cnn;

                oDbCommand.Parameters.AddWithValue("@TipoDocumento", tipoDocumento);
                oDbCommand.Parameters.AddWithValue("@Serie", NumeroSerie);
                oDbCommand.Parameters.AddWithValue("@NumeroDocumento", NumeroDocumento);
                oDbCommand.Parameters.AddWithValue("@RucEmpresa", RucEmpresa);

                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("NroOrden");
                    int indexUnidad = objReader.GetOrdinal("Unidad");
                    int indexCantidad = objReader.GetOrdinal("Cantidad");
                    int indexCodigoProducto = objReader.GetOrdinal("CodigoProducto");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");
                    int indexValorVenta = objReader.GetOrdinal("ValorVenta");
                    int indexValorVentaTexto = objReader.GetOrdinal("ValorVentaTexto");

                    int indexPrecioVenta = objReader.GetOrdinal("PrecioVenta");
                    int indexPrecioVentaTexto = objReader.GetOrdinal("PrecioVentaTexto");
                    int indexIgv = objReader.GetOrdinal("Igv");

                    int indexSubtotal = objReader.GetOrdinal("SubTotal");
                    int indexSubtotalTexto = objReader.GetOrdinal("SubTotalTexto");

                    int indexImporte = objReader.GetOrdinal("Importe");

                    while (objReader.Read())
                    {
                        objDetalleDocumento = new DetalleFacturaElectronica();
                        objDetalleDocumento.NroOrden = DataUtil.DbValueToDefault<int>(objReader[indexNro]);
                        objDetalleDocumento.Producto = new Producto();
                        objDetalleDocumento.Cantidad = DataUtil.DbValueToDefault<decimal>(objReader[indexCantidad]);
                        objDetalleDocumento.Unidad = DataUtil.DbValueToDefault<string>(objReader[indexUnidad]);
                        objDetalleDocumento.CodigoProducto = DataUtil.DbValueToDefault<string>(objReader[indexCodigoProducto]);
                        objDetalleDocumento.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);

                        objDetalleDocumento.PrecioVenta = DataUtil.DbValueToDefault<decimal>(objReader[indexPrecioVenta]);
                        objDetalleDocumento.PrecioVentaTexto = DataUtil.DbValueToDefault<string>(objReader[indexPrecioVentaTexto]);
                        objDetalleDocumento.ValorVenta = DataUtil.DbValueToDefault<decimal>(objReader[indexValorVenta]);
                        objDetalleDocumento.ValorVentaTexto = DataUtil.DbValueToDefault<string>(objReader[indexValorVentaTexto]);

                        objDetalleDocumento.IGV = DataUtil.DbValueToDefault<decimal>(objReader[indexIgv]);

                        objDetalleDocumento.SubTotal = DataUtil.DbValueToDefault<decimal>(objReader[indexSubtotal]);
                        objDetalleDocumento.SubTotalTexto = DataUtil.DbValueToDefault<string>(objReader[indexSubtotalTexto]);

                        objDetalleDocumento.Importe = DataUtil.DbValueToDefault<decimal>(objReader[indexImporte]);
                        objListaDetalleDocumento.Add(objDetalleDocumento);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: ListarDocumentoDetalle ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return objListaDetalleDocumento;
        }




        #region CRE, CPE

        public ListaFacturaElectronica ListarDocumentoCabeceraCRECPE(FacturaElectronica oFacturaElectronica)
        {
            FacturaElectronica objFactura = new FacturaElectronica();
            ListaFacturaElectronica objListaFactura = new ListaFacturaElectronica();

            try
            {
                SqlCommand oDbCommand = new SqlCommand();

                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_ListaDocumentoCabeceraCRECPE;
                oDbCommand.Connection = cnn;

                oDbCommand.Parameters.AddWithValue("@FechaInicio", oFacturaElectronica.FechaInicio);
                oDbCommand.Parameters.AddWithValue("@FechaFin", oFacturaElectronica.FechaFin);
                oDbCommand.Parameters.AddWithValue("@TipoDocumento", oFacturaElectronica.TipoDocumento.CodigoDocumento);
                oDbCommand.Parameters.AddWithValue("@Serie", oFacturaElectronica.Serie.NumeroSerie);
                oDbCommand.Parameters.AddWithValue("@NumeroDocumentoInicio", oFacturaElectronica.NumeroDocumentoInicio);
                oDbCommand.Parameters.AddWithValue("@NumeroDocumentoFin", oFacturaElectronica.NumeroDocumentoFin);
                oDbCommand.Parameters.AddWithValue("@Estado", oFacturaElectronica.Estado.IdEstado);
                oDbCommand.Parameters.AddWithValue("@Ruc", oFacturaElectronica.Cliente.ClienteRuc);
                oDbCommand.Parameters.AddWithValue("@RazonSocial", oFacturaElectronica.Cliente.Nombres);
                oDbCommand.Parameters.AddWithValue("@RucEmpresaEmisor", oFacturaElectronica.Empresa.RUC);
                oDbCommand.Parameters.AddWithValue("@Id_ED_DOC", oFacturaElectronica.Estado.IdEstadoSUNAT);

                oDbCommand.Parameters.AddWithValue("@NameSede", oFacturaElectronica.Sede);

                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("Nro");
                    int indexCodigoDocumento = objReader.GetOrdinal("CodigoDocumento");
                    int indexDescripcionTipoDocumento = objReader.GetOrdinal("Descripcion");
                    int indexSerie = objReader.GetOrdinal("Serie");
                    int indexNombreArchivoXML = objReader.GetOrdinal("NumeroDocumento");
                    int indexNumeroDocumento = objReader.GetOrdinal("NumeroFactura");
                    int indexFechaEmision = objReader.GetOrdinal("FechaEmision");
                    int indexNroDocClient = objReader.GetOrdinal("NumDocCliente");
                    int indexClienteDireccion = objReader.GetOrdinal("ClienteDireccion");
                    int indexCliente = objReader.GetOrdinal("Cliente");
                    int indexMontoTotal = objReader.GetOrdinal("MontoTotal");
                    int indexEstado = objReader.GetOrdinal("Estado");
                    int indexRutaImagen = objReader.GetOrdinal("RutaImagen");
                    int indexMoneda = objReader.GetOrdinal("Moneda");

                    int indexEmpresaRuc = objReader.GetOrdinal("EmpresaRuc");
                    int indexRazonSocial = objReader.GetOrdinal("RazonSocial");
                    int indexEmpresaDireccion = objReader.GetOrdinal("EmpresaDireccion");
                    int indexEmpresaUbigeo = objReader.GetOrdinal("EmpresaUbigeo");
                    int indexEmpresaUbigeoDesc = objReader.GetOrdinal("EmpresaUbigeoDesc");

                    int indexIdTipoDocumento = objReader.GetOrdinal("IdTipoDocumento");

                    int indexXML = objReader.GetOrdinal("XML");

                    int indexCDR = objReader.GetOrdinal("CDR");

                    int indexCampo1 = objReader.GetOrdinal("Campo1");

                    int indexCodeMessage = objReader.GetOrdinal("CodeMessage");
                    int indexDocMessage = objReader.GetOrdinal("DocMessage");

                    int indexCodeResponse = objReader.GetOrdinal("CodeResponse");
                    int indexNoteResponse = objReader.GetOrdinal("NoteResponse");

                    int indextypeFormat = objReader.GetOrdinal("TypeFormat");

                    while (objReader.Read())
                    {
                        objFactura = new FacturaElectronica();
                        objFactura.Nro = DataUtil.DbValueToDefault<Int32>(objReader[indexNro]);
                        objFactura.TipoDocumento = new TipoDocumento();
                        objFactura.TipoDocumento.IdTipoDocumento = DataUtil.DbValueToDefault<int>(objReader[indexIdTipoDocumento]);
                        objFactura.TipoDocumento.CodigoDocumento = DataUtil.DbValueToDefault<String>(objReader[indexCodigoDocumento]);
                        objFactura.TipoDocumento.Descripcion = DataUtil.DbValueToDefault<String>(objReader[indexDescripcionTipoDocumento]);

                        objFactura.Serie = new Serie();
                        objFactura.Serie.NumeroSerie = DataUtil.DbValueToDefault<String>(objReader[indexSerie]);
                        objFactura.NombreArchivoXML = DataUtil.DbValueToDefault<string>(objReader[indexNombreArchivoXML]);
                        objFactura.NumeroDocumento = DataUtil.DbValueToDefault<String>(objReader[indexNumeroDocumento]);
                        objFactura.FechaEmision = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEmision]);
                        objFactura.FechaEmision2 = (DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEmision])).ToString("yyyy/MM/dd");

                        objFactura.Cliente = new Cliente();
                        objFactura.Cliente.NumeroDocumentoIdentidad = DataUtil.DbValueToDefault<String>(objReader[indexNroDocClient]);
                        objFactura.Cliente.Nombres = DataUtil.DbValueToDefault<String>(objReader[indexCliente]);
                        objFactura.Cliente.Direccion = DataUtil.DbValueToDefault<String>(objReader[indexClienteDireccion]);

                        objFactura.MontoTotal = DataUtil.DbValueToDefault<Decimal>(objReader[indexMontoTotal]);
                        objFactura.Estado = new Estado();
                        objFactura.Estado.Descripcion = DataUtil.DbValueToDefault<String>(objReader[indexEstado]);
                        objFactura.RutaImagen = DataUtil.DbValueToDefault<String>(objReader[indexRutaImagen]);
                        objFactura.Moneda = new Moneda();
                        objFactura.Moneda.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexMoneda]);

                        objFactura.Empresa = new Empresa();
                        objFactura.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexEmpresaRuc]);
                        objFactura.Empresa.RazonSocial = DataUtil.DbValueToDefault<string>(objReader[indexRazonSocial]);
                        objFactura.Empresa.Direccion = DataUtil.DbValueToDefault<string>(objReader[indexEmpresaDireccion]);

                        objFactura.Empresa.Ubigeo = new Ubigeo();
                        objFactura.Empresa.Ubigeo.CodigoUbigeo = DataUtil.DbValueToDefault<string>(objReader[indexEmpresaUbigeo]);
                        objFactura.Empresa.Ubigeo.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexEmpresaUbigeoDesc]);

                        

                        string strData = string.Empty;
                        byte[] byteData = (byte[])objReader[indexXML];
                        strData = Encoding.GetEncoding("iso-8859-1").GetString(byteData);
                        objFactura.XML = strData;

                        strData = string.Empty;
                        byteData = null;
                        byteData = (byte[])objReader[indexCDR];
                        strData = Encoding.GetEncoding("iso-8859-1").GetString(byteData);
                        objFactura.CDR = strData;

                        objFactura.Campo1 = DataUtil.DbValueToDefault<string>(objReader[indexCampo1]);

                        objFactura.CodeMessage = DataUtil.DbValueToDefault<string>(objReader[indexCodeMessage]);
                        objFactura.DocMessage = DataUtil.DbValueToDefault<string>(objReader[indexDocMessage]);

                        objFactura.CodeResponse = DataUtil.DbValueToDefault<string>(objReader[indexCodeResponse]);
                        objFactura.NoteResponse = DataUtil.DbValueToDefault<string>(objReader[indexNoteResponse]);
                        objFactura.TypeFormat = DataUtil.DbValueToDefault<int>(objReader[indextypeFormat]);
                        objListaFactura.Add(objFactura);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: ListarDocumentoCabeceraCRECPE ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return objListaFactura;
        }

        #endregion


        public ListaEstado ListarEstadoDocumento()
        {
            ListaEstado oListaEstado = new ListaEstado();
            Estado oEstado = new Estado();

            try
            {
                SqlCommand oDbCommand = new SqlCommand();

                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_ListaEstadoDocumento;
                oDbCommand.Connection = cnn;
                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexIdEstado = objReader.GetOrdinal("IdEstado");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");
                    while (objReader.Read())
                    {
                        oEstado = new Estado();
                        oEstado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        oEstado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);
                        oListaEstado.Add(oEstado);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: ListarEstadoDocumento ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaEstado;
        }

        public ListaTipoDocumento ListarTipoDocumento()
        {
            ListaTipoDocumento oListaTipoDocumento = new ListaTipoDocumento();
            TipoDocumento oTipoDocumento = new TipoDocumento();

            try
            {
                SqlCommand oDbCommand = new SqlCommand();
                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_ListaTipoDocumento;
                oDbCommand.Connection = cnn;

                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexIdTipoDocumento = objReader.GetOrdinal("IdTipoDocumento");
                    int indexCodigoDocumento = objReader.GetOrdinal("CodigoDocumento");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");
                    int indexPadre = objReader.GetOrdinal("Padre");
                    while (objReader.Read())
                    {
                        oTipoDocumento = new TipoDocumento();
                        oTipoDocumento.IdTipoDocumento = DataUtil.DbValueToDefault<int>(objReader[indexIdTipoDocumento]);
                        oTipoDocumento.CodigoDocumento = DataUtil.DbValueToDefault<string>(objReader[indexCodigoDocumento]);
                        oTipoDocumento.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);
                        oTipoDocumento.Padre = DataUtil.DbValueToDefault<int>(objReader[indexPadre]);
                        oListaTipoDocumento.Add(oTipoDocumento);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: ListarTipoDocumento ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaTipoDocumento;
        }


        public ListaSerie ListarSerie(Serie oSerie)
        {
            Serie objSerie = new Serie();
            ListaSerie oListaSerie = new ListaSerie();

            try
            {
                SqlCommand oDbCommand = new SqlCommand();

                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_ListaSerie;
                oDbCommand.Connection = cnn;

                oDbCommand.Parameters.AddWithValue("@TipoDocumento", oSerie.TipoDocumento.IdTipoDocumento);
                oDbCommand.Parameters.AddWithValue("@Empresa", oSerie.Empresa.IdEmpresa);

                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexIdSerie = objReader.GetOrdinal("IdSerie");
                    int indexNumeroSerie = objReader.GetOrdinal("NumeroSerie");

                    while (objReader.Read())
                    {
                        objSerie = new Serie();
                        objSerie.IdSerie = DataUtil.DbValueToDefault<int>(objReader[indexIdSerie]);
                        objSerie.NumeroSerie = DataUtil.DbValueToDefault<string>(objReader[indexNumeroSerie]);
                        oListaSerie.Add(objSerie);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: ListarSerie ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaSerie;
        }

        public string GenerarXML(String tipoDocumento, String Serie, String NroDocumento, String RucEmpresa)
        {
            XmlDocument DocXML = new XmlDocument();
            string msj = string.Empty;
            try
            {
                string response = LoadXml(cnn, tipoDocumento, Serie, NroDocumento, RucEmpresa);
                msj = response != String.Empty ? response : String.Empty;
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GenerarXML ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }

        public string LoadXml(SqlConnection cnn, String tipoDocumento, String Serie, String NumeroDocumento, String RucEmpresa)
        {
            XmlDocument DocXML = new XmlDocument();
            string respuesta = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GenerarXML;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@TipoDocumento", tipoDocumento);
                cmd.Parameters.AddWithValue("@Serie", Serie);
                cmd.Parameters.AddWithValue("@NumeroDocumento", NumeroDocumento);
                cmd.Parameters.AddWithValue("@RucEmpresa", RucEmpresa);

                using (SqlDataReader objReader = cmd.ExecuteReader())
                {
                    while (objReader.Read())
                    {
                        SqlXml minxml = objReader.GetSqlXml(objReader.GetOrdinal("XML_SIGN"));
                        DocXML.LoadXml(minxml.Value);
                        respuesta = minxml.Value;
                    }
                    cnn.Close();
                }
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: LoadXml ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return respuesta;
        }

        public ListaFacturaElectronica GetListaMontoCab(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        {
            FacturaElectronica oFactura = new FacturaElectronica();
            ListaFacturaElectronica oListaFacturaCab = new ListaFacturaElectronica();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaMontosCab;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@TipoDocumento", tipoDocumento);
                cmd.Parameters.AddWithValue("@Serie", NumeroSerie);
                cmd.Parameters.AddWithValue("@NumeroDocumento", NumeroDocumento);
                cmd.Parameters.AddWithValue("@RucEmpresa", RucEmpresa);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIGV = objReader.GetOrdinal("IGV");
                    int indexTotalGravado = objReader.GetOrdinal("TotalGravado");
                    int indexTotalnoGravado = objReader.GetOrdinal("TotalnoGravado");
                    int indexTotalExonerado = objReader.GetOrdinal("TotalExonerado");
                    int indexTotalDescuento = objReader.GetOrdinal("TotalDescuento");
                    int indexMontoTotal = objReader.GetOrdinal("MontoTotalCad");
                    int indexMontoTotalLetras = objReader.GetOrdinal("MontoTotalLetras");


                    while (objReader.Read())
                    {
                        oFactura = new FacturaElectronica();
                        oFactura.MontoIgvCad = DataUtil.DbValueToDefault<string>(objReader[indexIGV]);
                        oFactura.TotalGravado = DataUtil.DbValueToDefault<string>(objReader[indexTotalGravado]);
                        oFactura.TotalnoGravado = DataUtil.DbValueToDefault<string>(objReader[indexTotalnoGravado]);
                        oFactura.TotalExonerado = DataUtil.DbValueToDefault<string>(objReader[indexTotalExonerado]);
                        oFactura.TotalDescuento = DataUtil.DbValueToDefault<string>(objReader[indexTotalDescuento]);
                        oFactura.MontoTotalCad = DataUtil.DbValueToDefault<string>(objReader[indexMontoTotal]);
                        oFactura.MontoTotalLetras = DataUtil.DbValueToDefault<string>(objReader[indexMontoTotalLetras]);
                        oListaFacturaCab.Add(oFactura);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListaMontoCab ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaFacturaCab;
        }


        #region DOCUMENTO ANULADO

        public FacturaElectronica GetFechaDocumento(int tpodocumento, String serie, String nrodocumento, String rucempresa)
        {
            FacturaElectronica oDocFecha = new FacturaElectronica();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ObtenerFechaDocumento;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@TipoDocumento", tpodocumento);
                cmd.Parameters.AddWithValue("@Serie", serie);
                cmd.Parameters.AddWithValue("@NumeroDocumento", nrodocumento);
                cmd.Parameters.AddWithValue("@RucEmpresa", rucempresa);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexSerie = objReader.GetOrdinal("Serie");
                    int indexFechaEmision = objReader.GetOrdinal("FechaEmision");
                    int indexMontoTotal = objReader.GetOrdinal("MontoTotal");
                    int indexCodigoTipoDoc = objReader.GetOrdinal("CodigoDocumento");
                    int indexMoneda = objReader.GetOrdinal("Moneda");

                    while (objReader.Read())
                    {
                        oDocFecha = new FacturaElectronica();
                        oDocFecha.TipoDocumento = new TipoDocumento();
                        oDocFecha.Moneda = new Moneda();

                        oDocFecha.NumeroSerie = DataUtil.DbValueToDefault<string>(objReader[indexSerie]);
                        oDocFecha.FechaEmision = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEmision]);
                        oDocFecha.MontoTotalCad = DataUtil.DbValueToDefault<string>(objReader[indexMontoTotal]);
                        oDocFecha.TipoDocumento.CodigoDocumento = DataUtil.DbValueToDefault<string>(objReader[indexCodigoTipoDoc].ToString().TrimEnd());
                        oDocFecha.Moneda.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexMoneda]);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetFechaDocumento ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oDocFecha;
        }


        //VER AQUI
        public ListaFacturaElectronica GetValidarDocumentoExiste(FacturaElectronica oDocAnulado)
        {
            FacturaElectronica oDocumento = new FacturaElectronica();
            ListaFacturaElectronica oListaDocumento = new ListaFacturaElectronica();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ValidarDocumentoExiste;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdTipoDocumento", oDocAnulado.TipoDocumento.IdTipoDocumento);
                cmd.Parameters.AddWithValue("@Serie", oDocAnulado.Serie.NumeroSerie);
                cmd.Parameters.AddWithValue("@NumeroDocumento", oDocAnulado.NumeroDocumento);
                cmd.Parameters.AddWithValue("@RucEmpresa", oDocAnulado.Empresa.RUC);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexFechaEmision = objReader.GetOrdinal("FechaEmision");
                    int indexMontoTotal = objReader.GetOrdinal("MontoTotal");

                    while (objReader.Read())
                    {
                        oDocumento = new FacturaElectronica();
                        oDocumento.Estado = new Estado();

                        oDocumento.FechaEmision = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEmision]);
                        oDocumento.MontoTotal = DataUtil.DbValueToDefault<decimal>(objReader[indexMontoTotal]);
                        oListaDocumento.Add(oDocumento);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetValidarDocumentoExiste ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaDocumento;
        }

        public String InsertarDocumentoAnulado(FacturaElectronica oDocumentoAnulado)
        {
            string msje = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertarDocumentoAnulado;
                cmd.Connection = cnn;
                {
                    cmd.Parameters.AddWithValue("@IdTipoDocumento", oDocumentoAnulado.TipoDocumento.IdTipoDocumento);
                    cmd.Parameters.AddWithValue("@Serie", oDocumentoAnulado.Serie.NumeroSerie);
                    cmd.Parameters.AddWithValue("@Numerodocumento", oDocumentoAnulado.NumeroDocumento);
                    cmd.Parameters.AddWithValue("@FechaAnulado", oDocumentoAnulado.FechaAnulado);
                    cmd.Parameters.AddWithValue("@MotivoAnulado", oDocumentoAnulado.MotivoAnulado);
                    cmd.Parameters.AddWithValue("@EstadoAnulado", oDocumentoAnulado.Estado.IdEstado);
                    cmd.Parameters.AddWithValue("@RucEmpresa", oDocumentoAnulado.Empresa.RUC);
                    cmd.Parameters.AddWithValue("@CodigoTipoDocumento", oDocumentoAnulado.TipoDocumento.CodigoDocumento);
                    cmd.Parameters.AddWithValue("@User", oDocumentoAnulado.Usuario);
                }
                int result = cmd.ExecuteNonQuery();
                cnn.Close();
                msje = result > 0 ? "Registrado Correctamente" : "Error al Anular";
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: InsertarDocumentoAnulado ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msje;
        }

        public ListaDocumento ValidarExisteDocAnulado(FacturaElectronica oDocumentoAnulado)
        {
            Documento oDocAnulado = new Documento();
            ListaDocumento oListaDocAnulado = new ListaDocumento();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ExisteDocAnulado;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdTipoDocumento", oDocumentoAnulado.TipoDocumento.IdTipoDocumento);
                cmd.Parameters.AddWithValue("@Serie", oDocumentoAnulado.Serie.NumeroSerie);
                cmd.Parameters.AddWithValue("@Numerodocumento", oDocumentoAnulado.NumeroDocumento);
                cmd.Parameters.AddWithValue("@RucEmpresa", oDocumentoAnulado.Empresa.RUC);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("Nro");
                    int indexSerie = objReader.GetOrdinal("Serie");
                    int indexNroDocumento = objReader.GetOrdinal("NumeroDocumento");
                    int indexFechaAnulado = objReader.GetOrdinal("FechaAnulado");

                    while (objReader.Read())
                    {
                        oDocAnulado = new Documento();
                        oDocAnulado.Nro = DataUtil.DbValueToDefault<int>(objReader[indexNro]);
                        oDocAnulado.Serie = DataUtil.DbValueToDefault<string>(objReader[indexSerie]);
                        oDocAnulado.NumeroDocumento = DataUtil.DbValueToDefault<string>(objReader[indexNroDocumento]);
                        oDocAnulado.FechaAnulado = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaAnulado]);
                        oListaDocAnulado.Add(oDocAnulado);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: ValidarExisteDocAnulado ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaDocAnulado;
        }


        public Documento ValidarExisteDoc_WS(string NUM_CE, string TIPO_CE)
        {
            Documento oDocAnulado = new Documento();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ExisteDocWS;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@NUM_CE", NUM_CE);
                cmd.Parameters.AddWithValue("@TIPO_CE", TIPO_CE);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("IdTpoDoc");
                    int indexSerie = objReader.GetOrdinal("Serie");
                    int indexNroDocumento = objReader.GetOrdinal("NumeroDocumento");
                    int indexFechaAnulado = objReader.GetOrdinal("FechaEmision");

                    while (objReader.Read())
                    {
                        oDocAnulado = new Documento();
                        oDocAnulado.Nro = DataUtil.DbValueToDefault<int>(objReader[indexNro]);
                        oDocAnulado.Serie = DataUtil.DbValueToDefault<string>(objReader[indexSerie]);
                        oDocAnulado.NumeroDocumento = DataUtil.DbValueToDefault<string>(objReader[indexNroDocumento]);
                        oDocAnulado.FechaAnulado = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaAnulado]);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: ValidarExisteDoc_WS ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oDocAnulado;
        }


        public ListaFacturaElectronica GetListaDocumentoAnulado(FacturaElectronica oDocumentoAnulado)
        {
            FacturaElectronica oFactura = new FacturaElectronica();
            ListaFacturaElectronica oListaFacturaElectronica = new ListaFacturaElectronica();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaDocumentoAnulado;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@FechaDesde", oDocumentoAnulado.FechaInicio);
                cmd.Parameters.AddWithValue("@FechaHasta", oDocumentoAnulado.FechaFin);
                cmd.Parameters.AddWithValue("@Serie", oDocumentoAnulado.Serie.NumeroSerie);
                cmd.Parameters.AddWithValue("@IdTipoDocumento", oDocumentoAnulado.TipoDocumento.IdTipoDocumento);
                cmd.Parameters.AddWithValue("@RucEmpresa", oDocumentoAnulado.Empresa.RUC);


                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdDocumentoAnul = objReader.GetOrdinal("IdDocumentoAnulado");
                    int indexIdTipoDocumento = objReader.GetOrdinal("IdTipoDocumento");
                    int indexCodigoTpoDoc = objReader.GetOrdinal("CodigoTipoDocumento");
                    int indexSerie = objReader.GetOrdinal("Serie");
                    int indexNumeroDocumento = objReader.GetOrdinal("NumeroDocumento");
                    int indexFechaAnulado = objReader.GetOrdinal("FechaAnulado");
                    int indexMotivoAnulado = objReader.GetOrdinal("MotivoAnulado");
                    int indexEstadoAnulado = objReader.GetOrdinal("EstadoAnulado");
                    int indexMensajeAnulado = objReader.GetOrdinal("MensajeAnulado");
                    int indexEstadoEnvio = objReader.GetOrdinal("EstadoEnvio");
                    int indexMensajeEnvio = objReader.GetOrdinal("MensajeEnvio");
                    int indexNumeroAtencion = objReader.GetOrdinal("NumeroAtencion");
                    int indexIdEstado = objReader.GetOrdinal("IdEstadoDoc");
                    int indexEstadoDesc = objReader.GetOrdinal("EstadoDesc");

                    while (objReader.Read())
                    {
                        oFactura = new FacturaElectronica();
                        oFactura.TipoDocumento = new TipoDocumento();
                        oFactura.Serie = new Serie();
                        oFactura.Estado = new Estado();
                        oFactura.EstadoEnvio = new EstadoEnvio();

                        oFactura.Nro = DataUtil.DbValueToDefault<int>(objReader[indexIdDocumentoAnul]);
                        oFactura.TipoDocumento.IdTipoDocumento = DataUtil.DbValueToDefault<int>(objReader[indexIdTipoDocumento]);
                        oFactura.TipoDocumento.CodigoDocumento = DataUtil.DbValueToDefault<string>(objReader[indexCodigoTpoDoc]);
                        oFactura.Serie.NumeroSerie = DataUtil.DbValueToDefault<string>(objReader[indexSerie]);
                        oFactura.NumeroDocumento = DataUtil.DbValueToDefault<string>(objReader[indexNumeroDocumento]);
                        oFactura.FechaAnulado = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaAnulado]);

                        oFactura.FechaEmision2 = (DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaAnulado])).ToString("yyyy/MM/dd");

                        oFactura.MotivoAnulado = DataUtil.DbValueToDefault<string>(objReader[indexMotivoAnulado]);
                        oFactura.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexEstadoAnulado]);

                        oFactura.MensajeAnulado = DataUtil.DbValueToDefault<string>(objReader[indexMensajeAnulado]);
                        oFactura.EstadoEnvio.IdEstadoEnvio = DataUtil.DbValueToDefault<int>(objReader[indexEstadoEnvio]);

                        oFactura.MensajeEnvio = DataUtil.DbValueToDefault<string>(objReader[indexMensajeEnvio]);
                        oFactura.NumeroAtencion = DataUtil.DbValueToDefault<string>(objReader[indexNumeroAtencion]);

                        oFactura.IdEstadoDoc = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        oFactura.EstadoDesc = DataUtil.DbValueToDefault<string>(objReader[indexEstadoDesc]);

                        oListaFacturaElectronica.Add(oFactura);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListaDocumentoAnulado ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaFacturaElectronica;
        }
        #endregion



        #region UTIL 
        public ListaDocumento GetListaDocumentoCabExcel(FacturaElectronica oFactura)
        {
            Documento oDocumento = new Documento();
            ListaDocumento oListaDocumento = new ListaDocumento();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaDocumentoCabExcel;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@FechaInicio", oFactura.FechaInicio);
                cmd.Parameters.AddWithValue("@FechaFin", oFactura.FechaFin);
                cmd.Parameters.AddWithValue("@TipoDocumento", oFactura.TipoDocumento.CodigoDocumento);
                cmd.Parameters.AddWithValue("@Serie", oFactura.Serie.NumeroSerie);
                cmd.Parameters.AddWithValue("@NumeroDocumentoInicio", oFactura.NumeroDocumentoInicio);
                cmd.Parameters.AddWithValue("@NumeroDocumentoFin", oFactura.NumeroDocumentoFin);
                cmd.Parameters.AddWithValue("@Estado", oFactura.Estado.IdEstado);
                cmd.Parameters.AddWithValue("@Ruc", oFactura.Cliente.ClienteRuc);
                cmd.Parameters.AddWithValue("@RazonSocial", oFactura.Cliente.Nombres);
                cmd.Parameters.AddWithValue("@RucEmpresa", oFactura.Empresa.RUC);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("Nro");
                    int indexCodigoDocumento = objReader.GetOrdinal("CodigoTipoDocumento");
                    int indexTipoDocumento = objReader.GetOrdinal("TipoDocumentoDesc");
                    int indexSerie = objReader.GetOrdinal("Serie");
                    int indexNombreArchivoXML = objReader.GetOrdinal("NombreDocumento");
                    int indexNumeroDocumento = objReader.GetOrdinal("NumeroDocumento");
                    int indexFechaEmision = objReader.GetOrdinal("FechaEmision");
                    int indexNroDocClient = objReader.GetOrdinal("NumDocCliente");
                    int indexCliente = objReader.GetOrdinal("Cliente");
                    int indexMontoTotal = objReader.GetOrdinal("Total");
                    int indexEstado = objReader.GetOrdinal("Estado");
                    int indexMoneda = objReader.GetOrdinal("Moneda");

                    while (objReader.Read())
                    {
                        oDocumento = new Documento();

                        oDocumento.Nro = DataUtil.DbValueToDefault<int>(objReader[indexNro]);
                        oDocumento.CodigoTipoDocumento = DataUtil.DbValueToDefault<string>(objReader[indexCodigoDocumento]);
                        oDocumento.TipoDocumentoDesc = DataUtil.DbValueToDefault<string>(objReader[indexTipoDocumento]);
                        oDocumento.Serie = DataUtil.DbValueToDefault<string>(objReader[indexSerie]);
                        oDocumento.NumeroDocumento = DataUtil.DbValueToDefault<string>(objReader[indexNumeroDocumento]);
                        oDocumento.FechaEmision = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEmision]);

                        oDocumento.FechaEnvio = (DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEmision])).ToString("yyyy/MM/dd");

                        oDocumento.NumDocCliente = DataUtil.DbValueToDefault<string>(objReader[indexNroDocClient]);
                        oDocumento.Cliente = DataUtil.DbValueToDefault<string>(objReader[indexCliente]);
                        oDocumento.Total = DataUtil.DbValueToDefault<decimal>(objReader[indexMontoTotal]);
                        oDocumento.Estado = DataUtil.DbValueToDefault<string>(objReader[indexEstado]);
                        oDocumento.Moneda = DataUtil.DbValueToDefault<string>(objReader[indexMoneda]);
                        oListaDocumento.Add(oDocumento);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListaDocumentoCabExcel ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaDocumento;
        }

        public FacturaElectronica GetObtenerDocumentoUnico(FacturaElectronica oFacturaElectronica)
        {
            FacturaElectronica objFactura = new FacturaElectronica();

            try
            {
                SqlCommand oDbCommand = new SqlCommand();

                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_ObtenerDocumentoUnico;
                oDbCommand.Connection = cnn;

                oDbCommand.Parameters.AddWithValue("@TipoDocumento", oFacturaElectronica.TipoDocumento.CodigoDocumento);
                oDbCommand.Parameters.AddWithValue("@Serie", oFacturaElectronica.Serie.NumeroSerie);
                oDbCommand.Parameters.AddWithValue("@NroDocumento", oFacturaElectronica.NumeroDocumento);
                oDbCommand.Parameters.AddWithValue("@RucEmpresa", oFacturaElectronica.Empresa.RUC);

                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("Nro");
                    int indexCodigoDocumento = objReader.GetOrdinal("CodigoDocumento");
                    int indexDescripcionTipoDocumento = objReader.GetOrdinal("Descripcion");
                    int indexSerie = objReader.GetOrdinal("Serie");
                    int indexNombreArchivoXML = objReader.GetOrdinal("NumeroDocumento");
                    int indexNumeroDocumento = objReader.GetOrdinal("NumeroFactura");
                    int indexFechaEmision = objReader.GetOrdinal("FechaEmision");
                    int indexNroDocClient = objReader.GetOrdinal("NumDocCliente");
                    int indexClienteDireccion = objReader.GetOrdinal("ClienteDireccion");
                    int indexCliente = objReader.GetOrdinal("Cliente");
                    int indexMontoTotal = objReader.GetOrdinal("MontoTotal");
                    int indexEstado = objReader.GetOrdinal("Estado");
                    int indexRutaImagen = objReader.GetOrdinal("RutaImagen");
                    int indexMoneda = objReader.GetOrdinal("Moneda");

                    int indexEmpresaRuc = objReader.GetOrdinal("EmpresaRuc");
                    int indexRazonSocial = objReader.GetOrdinal("RazonSocial");
                    int indexEmpresaDireccion = objReader.GetOrdinal("EmpresaDireccion");
                    int indexEmpresaUbigeo = objReader.GetOrdinal("EmpresaUbigeo");
                    int indexEmpresaUbigeoDesc = objReader.GetOrdinal("EmpresaUbigeoDesc");
                    int indexIdTipoDocumento = objReader.GetOrdinal("IdTipoDocumento");
                    int indexXML = objReader.GetOrdinal("XML");

                    int indexCampo1 = objReader.GetOrdinal("Campo1");

                    int indextypeFormat = objReader.GetOrdinal("TypeFormat");

                    while (objReader.Read())
                    {
                        objFactura = new FacturaElectronica();
                        objFactura.Nro = DataUtil.DbValueToDefault<Int32>(objReader[indexNro]);
                        objFactura.TipoDocumento = new TipoDocumento();
                        objFactura.TipoDocumento.IdTipoDocumento = DataUtil.DbValueToDefault<int>(objReader[indexIdTipoDocumento]);
                        objFactura.TipoDocumento.CodigoDocumento = DataUtil.DbValueToDefault<String>(objReader[indexCodigoDocumento]);
                        objFactura.TipoDocumento.Descripcion = DataUtil.DbValueToDefault<String>(objReader[indexDescripcionTipoDocumento]);

                        objFactura.Serie = new Serie();
                        objFactura.Serie.NumeroSerie = DataUtil.DbValueToDefault<String>(objReader[indexSerie]);
                        objFactura.NombreArchivoXML = DataUtil.DbValueToDefault<string>(objReader[indexNombreArchivoXML]).TrimEnd(); ;
                        objFactura.NumeroDocumento = DataUtil.DbValueToDefault<String>(objReader[indexNumeroDocumento]);
                        objFactura.FechaEmision = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEmision]);

                        objFactura.FechaEmision2 = (DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEmision])).ToString("yyyy/MM/dd");

                        objFactura.Cliente = new Cliente();
                        objFactura.Cliente.NumeroDocumentoIdentidad = DataUtil.DbValueToDefault<String>(objReader[indexNroDocClient]);
                        objFactura.Cliente.Nombres = DataUtil.DbValueToDefault<String>(objReader[indexCliente]).TrimEnd();
                        objFactura.Cliente.Direccion = DataUtil.DbValueToDefault<String>(objReader[indexClienteDireccion]);

                        objFactura.MontoTotal = DataUtil.DbValueToDefault<Decimal>(objReader[indexMontoTotal]);
                        objFactura.Estado = new Estado();
                        objFactura.Estado.Descripcion = DataUtil.DbValueToDefault<String>(objReader[indexEstado]);
                        objFactura.RutaImagen = DataUtil.DbValueToDefault<String>(objReader[indexRutaImagen]);
                        objFactura.Moneda = new Moneda();
                        objFactura.Moneda.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexMoneda]);

                        objFactura.Empresa = new Empresa();
                        objFactura.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexEmpresaRuc]);
                        objFactura.Empresa.RazonSocial = DataUtil.DbValueToDefault<string>(objReader[indexRazonSocial]);
                        objFactura.Empresa.Direccion = DataUtil.DbValueToDefault<String>(objReader[indexEmpresaDireccion]);

                        objFactura.Empresa.Ubigeo = new Ubigeo();
                        objFactura.Empresa.Ubigeo.CodigoUbigeo = DataUtil.DbValueToDefault<String>(objReader[indexEmpresaUbigeo]);
                        objFactura.Empresa.Ubigeo.Descripcion = DataUtil.DbValueToDefault<String>(objReader[indexEmpresaUbigeoDesc]);
                        //objFactura.XML = DataUtil.DbValueToDefault<String>(objReader[indexXML]);

                        string strData = string.Empty;
                        byte[] byteData = (byte[])objReader[indexXML];
                        strData = Encoding.GetEncoding("iso-8859-1").GetString(byteData);

                        objFactura.Campo1 = DataUtil.DbValueToDefault<string>(objReader[indexCampo1]);

                        objFactura.TypeFormat = DataUtil.DbValueToDefault<int>(objReader[indextypeFormat]);
                        objFactura.XML = strData;
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetObtenerDocumentoUnico ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return objFactura;
        }


        
        #endregion


        #region RESUMEN RC AND RA

        public ListaEstado GetListaTipoFecha()
        {
            Estado oTipoFecha = new Estado();
            ListaEstado oListaTipoFecha = new ListaEstado();

            try
            {
                {
                    oTipoFecha.IdEstado = 1;
                    oTipoFecha.Descripcion = "Fecha Emisión";
                    oListaTipoFecha.Add(oTipoFecha);

                    oTipoFecha = new Estado();

                    oTipoFecha.IdEstado = 2;
                    oTipoFecha.Descripcion = "Fecha Envío";
                    oListaTipoFecha.Add(oTipoFecha);
                }
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListaTipoFecha ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaTipoFecha;
        }


        public ListaFacturaElectronica GetListaCabeceraRC(FacturaElectronica documentoRC)
        {
            FacturaElectronica oDocumentoRC = new FacturaElectronica();
            ListaFacturaElectronica oListaDocumentoRC = new ListaFacturaElectronica();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaCabeceraRC;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Fechadesde", documentoRC.FechaInicio);
                cmd.Parameters.AddWithValue("@Fechahasta", documentoRC.FechaFin);
                cmd.Parameters.AddWithValue("@IdEstado", documentoRC.Estado.IdEstado);
                cmd.Parameters.AddWithValue("@TipoFecha", documentoRC.TipoFecha);

                cmd.Parameters.AddWithValue("@RucEntity", documentoRC.Empresa.RUC);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("Nro");
                    int indexCorrelativo = objReader.GetOrdinal("Correlativo");
                    int indexFechaInicio = objReader.GetOrdinal("FechaInicio");
                    int indexFechaFin = objReader.GetOrdinal("FechaFin");
                    int indexFechaEnvio = objReader.GetOrdinal("FechaEnvio");
                    int indexSecuencia = objReader.GetOrdinal("Secuencia");
                    int indexIdEstado = objReader.GetOrdinal("IdEstado");
                    int indexEstado = objReader.GetOrdinal("Estado");
                    int indexMensajeEnvio = objReader.GetOrdinal("MensajeEnvio");
                    int indexMensajeEnvioDet = objReader.GetOrdinal("MensajeEnvioDetalle");
                    int indexNroAtencion = objReader.GetOrdinal("NumeroAtencion");
                    int indexXmlFirmado = objReader.GetOrdinal("XML");
                    int indexNombreArchivo = objReader.GetOrdinal("NombreArchivo");

                    while (objReader.Read())
                    {
                        string strData = string.Empty;

                        oDocumentoRC = new FacturaElectronica();
                        oDocumentoRC.Estado = new Estado();
                        oDocumentoRC.Empresa = new Empresa();

                        oDocumentoRC.Nro = DataUtil.DbValueToDefault<int>(objReader[indexNro]);
                        oDocumentoRC.IdFactura = DataUtil.DbValueToDefault<int>(objReader[indexCorrelativo]);

                        oDocumentoRC.FechaInicio2 = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaInicio]);
                        oDocumentoRC.FechaInicio = (DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaInicio])).ToString("yyyy/MM/dd");



                        oDocumentoRC.FechaFin2 = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaFin]);
                        oDocumentoRC.FechaFin = (DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaFin])).ToString("yyyy/MM/dd");

                        oDocumentoRC.FechaEnvio = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEnvio]).ToString("yyyy/MM/dd HH:mm:ss tt", CultureInfo.GetCultureInfo("es-PE"));



                        oDocumentoRC.Secuencia = DataUtil.DbValueToDefault<string>(objReader[indexSecuencia]);
                        oDocumentoRC.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        oDocumentoRC.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexEstado]);
                        oDocumentoRC.MensajeEnvio = DataUtil.DbValueToDefault<string>(objReader[indexMensajeEnvio]);
                        oDocumentoRC.MensajeEnvioDetalle = DataUtil.DbValueToDefault<string>(objReader[indexMensajeEnvioDet]);
                        oDocumentoRC.NumeroAtencion = DataUtil.DbValueToDefault<string>(objReader[indexNroAtencion]);

                        byte[] byteData = (byte[])objReader[indexXmlFirmado];
                        strData = Encoding.GetEncoding("iso-8859-1").GetString(byteData);
                        oDocumentoRC.XML = strData;

                        oDocumentoRC.NombreArchivoXML = DataUtil.DbValueToDefault<string>(objReader[indexNombreArchivo]).TrimEnd();
                        oListaDocumentoRC.Add(oDocumentoRC);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListaCabeceraRC ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaDocumentoRC;
        }


        public ListaFacturaElectronica GetListaCabeceraRA(FacturaElectronica documentoRA)
        {
            FacturaElectronica oDocumentoRA = new FacturaElectronica();
            ListaFacturaElectronica oListaDocumentoRA = new ListaFacturaElectronica();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaCabeceraRA;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Fechadesde", documentoRA.FechaInicio);
                cmd.Parameters.AddWithValue("@Fechahasta", documentoRA.FechaFin);
                cmd.Parameters.AddWithValue("@IdEstado", documentoRA.Estado.IdEstado);
                cmd.Parameters.AddWithValue("@TipoFecha", documentoRA.TipoFecha);

                cmd.Parameters.AddWithValue("@RucEntity", documentoRA.Empresa.RUC);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("Nro");
                    int indexCorrelativo = objReader.GetOrdinal("Correlativo");
                    int indexFechaInicio = objReader.GetOrdinal("FechaInicio");
                    int indexFechaFin = objReader.GetOrdinal("FechaFin");
                    int indexFechaEnvio = objReader.GetOrdinal("FechaEnvio");
                    int indexSecuencia = objReader.GetOrdinal("Secuencia");
                    int indexIdEstado = objReader.GetOrdinal("IdEstado");
                    int indexEstado = objReader.GetOrdinal("Estado");
                    //int indexEstadoDesc = objReader.GetOrdinal("EstadoDescripcion");
                    int indexMensajeEnvio = objReader.GetOrdinal("MensajeEnvio");
                    int indexMensajeEnvioDet = objReader.GetOrdinal("MensajeEnvioDetalle");
                    int indexNroAtencion = objReader.GetOrdinal("NumeroAtencion");

                    int indexXmlFirmado = objReader.GetOrdinal("XML");

                    int indexNombreArchivo = objReader.GetOrdinal("NombreArchivo");

                    string strData = string.Empty;

                    while (objReader.Read())
                    {
                        oDocumentoRA = new FacturaElectronica();
                        oDocumentoRA.Estado = new Estado();

                        oDocumentoRA.Nro = DataUtil.DbValueToDefault<int>(objReader[indexNro]);
                        oDocumentoRA.IdFactura = DataUtil.DbValueToDefault<int>(objReader[indexCorrelativo]);

                        oDocumentoRA.FechaInicio2 = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaInicio]);
                        oDocumentoRA.FechaInicio = (DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaInicio])).ToString("yyyy/MM/dd");

                        oDocumentoRA.FechaFin2 = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaFin]);
                        oDocumentoRA.FechaFin = (DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaFin])).ToString("yyyy/MM/dd");


                        //oDocumentoRA.FechaEnvio = DataUtil.DbValueToDefault<string>(objReader[indexFechaEnvio]);
                        oDocumentoRA.FechaEnvio = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEnvio]).ToString("yyyy/MM/dd HH:mm:ss tt", CultureInfo.GetCultureInfo("es-PE"));

                        oDocumentoRA.Secuencia = DataUtil.DbValueToDefault<string>(objReader[indexSecuencia]);
                        oDocumentoRA.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        oDocumentoRA.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexEstado]);
                        oDocumentoRA.MensajeEnvio = DataUtil.DbValueToDefault<string>(objReader[indexMensajeEnvio]);
                        oDocumentoRA.MensajeEnvioDetalle = DataUtil.DbValueToDefault<string>(objReader[indexMensajeEnvioDet]);
                        oDocumentoRA.NumeroAtencion = DataUtil.DbValueToDefault<string>(objReader[indexNroAtencion]);
                        oDocumentoRA.NombreArchivoXML = DataUtil.DbValueToDefault<string>(objReader[indexNombreArchivo]).TrimEnd();

                        //oDocumentoRA.XML = DataUtil.DbValueToDefault<string>(objReader[indexXmlFirmado]);

                        byte[] byteData = (byte[])objReader[indexXmlFirmado];
                        strData = Encoding.GetEncoding("iso-8859-1").GetString(byteData);
                        oDocumentoRA.XML = strData;

                        oListaDocumentoRA.Add(oDocumentoRA);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListaCabeceraRA ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaDocumentoRA;
        }


        public ListaFacturaElectronica GetListaCabeceraRR(FacturaElectronica documentoRR)
        {
            FacturaElectronica oDocumentoRR = new FacturaElectronica();
            ListaFacturaElectronica oListaDocumentoRR = new ListaFacturaElectronica();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaCabeceraRR;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Fechadesde", documentoRR.FechaInicio);
                cmd.Parameters.AddWithValue("@Fechahasta", documentoRR.FechaFin);
                cmd.Parameters.AddWithValue("@IdEstado", documentoRR.Estado.IdEstado);
                cmd.Parameters.AddWithValue("@TipoFecha", documentoRR.TipoFecha);

                cmd.Parameters.AddWithValue("@RucEntity", documentoRR.Empresa.RUC);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("Nro");
                    int indexCorrelativo = objReader.GetOrdinal("Correlativo");
                    int indexFechaInicio = objReader.GetOrdinal("FechaInicio");
                    int indexFechaFin = objReader.GetOrdinal("FechaFin");
                    int indexFechaEnvio = objReader.GetOrdinal("FechaEnvio");
                    int indexSecuencia = objReader.GetOrdinal("Secuencia");
                    int indexIdEstado = objReader.GetOrdinal("IdEstado");
                    int indexEstado = objReader.GetOrdinal("Estado");
                    //int indexEstadoDesc = objReader.GetOrdinal("EstadoDescripcion");
                    int indexMensajeEnvio = objReader.GetOrdinal("MensajeEnvio");
                    int indexMensajeEnvioDet = objReader.GetOrdinal("MensajeEnvioDetalle");
                    int indexNroAtencion = objReader.GetOrdinal("NumeroAtencion");

                    int indexXmlFirmado = objReader.GetOrdinal("XML");

                    int indexNombreArchivo = objReader.GetOrdinal("NombreArchivo");

                    string strData = string.Empty;

                    while (objReader.Read())
                    {
                        oDocumentoRR = new FacturaElectronica();
                        oDocumentoRR.Estado = new Estado();

                        oDocumentoRR.Nro = DataUtil.DbValueToDefault<int>(objReader[indexNro]);
                        oDocumentoRR.IdFactura = DataUtil.DbValueToDefault<int>(objReader[indexCorrelativo]);

                        oDocumentoRR.FechaInicio2 = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaInicio]);
                        oDocumentoRR.FechaInicio = (DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaInicio])).ToString("yyyy/MM/dd");

                        oDocumentoRR.FechaFin2 = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaFin]);
                        oDocumentoRR.FechaFin = (DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaFin])).ToString("yyyy/MM/dd");

                        //oDocumentoRR.FechaEnvio = DataUtil.DbValueToDefault<string>(objReader[indexFechaEnvio]);
                        oDocumentoRR.FechaEnvio = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEnvio]).ToString("yyyy/MM/dd HH:mm:ss tt", CultureInfo.GetCultureInfo("es-PE"));

                        oDocumentoRR.Secuencia = DataUtil.DbValueToDefault<string>(objReader[indexSecuencia]);
                        oDocumentoRR.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        oDocumentoRR.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexEstado]);
                        oDocumentoRR.MensajeEnvio = DataUtil.DbValueToDefault<string>(objReader[indexMensajeEnvio]);
                        oDocumentoRR.MensajeEnvioDetalle = DataUtil.DbValueToDefault<string>(objReader[indexMensajeEnvioDet]);
                        oDocumentoRR.NumeroAtencion = DataUtil.DbValueToDefault<string>(objReader[indexNroAtencion]);
                        oDocumentoRR.NombreArchivoXML = DataUtil.DbValueToDefault<string>(objReader[indexNombreArchivo]).TrimEnd();

                        //oDocumentoRA.XML = DataUtil.DbValueToDefault<string>(objReader[indexXmlFirmado]);

                        byte[] byteData = (byte[])objReader[indexXmlFirmado];
                        strData = Encoding.GetEncoding("iso-8859-1").GetString(byteData);
                        oDocumentoRR.XML = strData;

                        oListaDocumentoRR.Add(oDocumentoRR);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListaCabeceraRR ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaDocumentoRR;
        }





        public ListaDetalleFacturaElectronica GetListaDetalleRC(FacturaElectronica documentoRC)
        {
            DetalleFacturaElectronica oDetalleRC = new DetalleFacturaElectronica();
            ListaDetalleFacturaElectronica oListaDetalleRC = new ListaDetalleFacturaElectronica();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaDetalleRC;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Correlativo", documentoRC.IdFactura);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("Nro");
                    //int indexIdFactura = objReader.GetOrdinal("IdFactura");
                    int indexTipoDocumento = objReader.GetOrdinal("TipoDocumento");
                    int indexNumeroSerie = objReader.GetOrdinal("NumeroSerie");
                    int indexNroInicio = objReader.GetOrdinal("NroInicio");
                    int indexNroFin = objReader.GetOrdinal("NroFin");

                    int indexTotalDocEmi = objReader.GetOrdinal("TotalDocEmitido");

                    int indexMontoExonerado = objReader.GetOrdinal("MontoExonerado");
                    int indexMontoGravado = objReader.GetOrdinal("MontoGravado");
                    int indexMontoInafecto = objReader.GetOrdinal("MontoInafecto");
                    int indexTotalOtrosCargos = objReader.GetOrdinal("TotalOtrosCargos");

                    int indexISC = objReader.GetOrdinal("MontoISC");

                    int indexIGV = objReader.GetOrdinal("MontoIGV");
                    int indexTotalOtrosTributos = objReader.GetOrdinal("TotalOtrosTributos");

                    int indexMontoTotal = objReader.GetOrdinal("MontoTotal");

                    while (objReader.Read())
                    {
                        oDetalleRC = new DetalleFacturaElectronica();
                        oDetalleRC.FacturaElectronica = new FacturaElectronica();
                        oDetalleRC.TipoDocumento = new TipoDocumento();
                        oDetalleRC.Serie = new Serie();

                        oDetalleRC.NroOrden = DataUtil.DbValueToDefault<int>(objReader[indexNro]);
                        oDetalleRC.TipoDocumento.CodigoDocumento = DataUtil.DbValueToDefault<string>(objReader[indexTipoDocumento]);

                        oDetalleRC.Serie.NumeroSerie = DataUtil.DbValueToDefault<string>(objReader[indexNumeroSerie]);
                        oDetalleRC.NroInicio = DataUtil.DbValueToDefault<int>(objReader[indexNroInicio]);
                        oDetalleRC.NroFin = DataUtil.DbValueToDefault<int>(objReader[indexNroFin]);

                        oDetalleRC.TotalDocEmitido = DataUtil.DbValueToDefault<int>(objReader[indexTotalDocEmi]);

                        oDetalleRC.MontoExonerado = DataUtil.DbValueToDefault<string>(objReader[indexMontoExonerado]);
                        oDetalleRC.MontoGravado = DataUtil.DbValueToDefault<string>(objReader[indexMontoGravado]);
                        oDetalleRC.MontoInafecto = DataUtil.DbValueToDefault<string>(objReader[indexMontoInafecto]);
                        oDetalleRC.TotalOtrosCargos = DataUtil.DbValueToDefault<string>(objReader[indexTotalOtrosCargos]);
                        oDetalleRC.MontoISC = DataUtil.DbValueToDefault<string>(objReader[indexISC]);
                        oDetalleRC.MontoIGV = DataUtil.DbValueToDefault<string>(objReader[indexIGV]);
                        oDetalleRC.TotalOtrosTributos = DataUtil.DbValueToDefault<string>(objReader[indexTotalOtrosTributos]);
                        oDetalleRC.MontoTotal = DataUtil.DbValueToDefault<string>(objReader[indexMontoTotal]);
                        oListaDetalleRC.Add(oDetalleRC);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListaDetalleRC ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaDetalleRC;
        }

        public ListaDetalleFacturaElectronica GetListaDetalleRA(FacturaElectronica documentoRA)
        {
            DetalleFacturaElectronica oDetalleRA = new DetalleFacturaElectronica();
            ListaDetalleFacturaElectronica oListaDetalleRA = new ListaDetalleFacturaElectronica();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaDetalleRA;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Correlativo", documentoRA.IdFactura);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("NroOrden");
                    int indexTipoDocumento = objReader.GetOrdinal("TipoDocumento");
                    int indexNumeroSerie = objReader.GetOrdinal("NumeroSerie");
                    int indexNumeroDocumento = objReader.GetOrdinal("NumeroDocumento");
                    int indexFechaCarga = objReader.GetOrdinal("FechaCarga");
                    int indexMotivoAnulacion = objReader.GetOrdinal("MotivoAnulacion");
                    int indexMensajeDocumento = objReader.GetOrdinal("MensajeDocumento");

                    while (objReader.Read())
                    {
                        oDetalleRA = new DetalleFacturaElectronica();
                        oDetalleRA.FacturaElectronica = new FacturaElectronica();
                        oDetalleRA.TipoDocumento = new TipoDocumento();
                        oDetalleRA.Serie = new Serie();

                        oDetalleRA.NroOrden = DataUtil.DbValueToDefault<int>(objReader[indexNro]);
                        oDetalleRA.TipoDocumento.CodigoDocumento = DataUtil.DbValueToDefault<string>(objReader[indexTipoDocumento]);
                        oDetalleRA.Serie.NumeroSerie = DataUtil.DbValueToDefault<string>(objReader[indexNumeroSerie]);
                        oDetalleRA.FacturaElectronica.NumeroDocumento = DataUtil.DbValueToDefault<string>(objReader[indexNumeroDocumento]);
                        oDetalleRA.FacturaElectronica.FechaCarga = DataUtil.DbValueToDefault<string>(objReader[indexFechaCarga]);
                        oDetalleRA.FacturaElectronica.MotivoAnulado = DataUtil.DbValueToDefault<string>(objReader[indexMotivoAnulacion]);
                        oDetalleRA.FacturaElectronica.MensajeAnulado = DataUtil.DbValueToDefault<string>(objReader[indexMensajeDocumento]);
                        oListaDetalleRA.Add(oDetalleRA);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListaDetalleRA ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaDetalleRA;
        }


        public ListaDetalleFacturaElectronica GetListaDetalleRR(FacturaElectronica documentoRR)
        {
            DetalleFacturaElectronica oDetalleRR = new DetalleFacturaElectronica();
            ListaDetalleFacturaElectronica oListaDetalleRR = new ListaDetalleFacturaElectronica();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaDetalleRR;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Correlativo", documentoRR.IdFactura);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("NroOrden");
                    int indexTipoDocumento = objReader.GetOrdinal("TipoDocumento");
                    int indexNumeroSerie = objReader.GetOrdinal("NumeroSerie");
                    int indexNumeroDocumento = objReader.GetOrdinal("NumeroDocumento");
                    int indexFechaCarga = objReader.GetOrdinal("FechaCarga");
                    int indexMotivoAnulacion = objReader.GetOrdinal("MotivoAnulacion");
                    int indexMensajeDocumento = objReader.GetOrdinal("MensajeDocumento");

                    while (objReader.Read())
                    {
                        oDetalleRR = new DetalleFacturaElectronica();
                        oDetalleRR.FacturaElectronica = new FacturaElectronica();
                        oDetalleRR.TipoDocumento = new TipoDocumento();
                        oDetalleRR.Serie = new Serie();

                        oDetalleRR.NroOrden = DataUtil.DbValueToDefault<int>(objReader[indexNro]);
                        oDetalleRR.TipoDocumento.CodigoDocumento = DataUtil.DbValueToDefault<string>(objReader[indexTipoDocumento]);
                        oDetalleRR.Serie.NumeroSerie = DataUtil.DbValueToDefault<string>(objReader[indexNumeroSerie]);
                        oDetalleRR.FacturaElectronica.NumeroDocumento = DataUtil.DbValueToDefault<string>(objReader[indexNumeroDocumento]);
                        oDetalleRR.FacturaElectronica.FechaCarga = DataUtil.DbValueToDefault<string>(objReader[indexFechaCarga]);
                        oDetalleRR.FacturaElectronica.MotivoAnulado = DataUtil.DbValueToDefault<string>(objReader[indexMotivoAnulacion]);
                        oDetalleRR.FacturaElectronica.MensajeAnulado = DataUtil.DbValueToDefault<string>(objReader[indexMensajeDocumento]);
                        oListaDetalleRR.Add(oDetalleRR);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListaDetalleRR ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaDetalleRR;
        }

        #endregion


        #region ESTADO SISTEMA DOCUMENTO

        public ListaSistemaEstado GetListaEstadoSistema_Ok(string RucEntity)
        {
            SistemaEstado oEstadoSistema = new SistemaEstado();
            ListaSistemaEstado oListaSistemaEstado = new ListaSistemaEstado();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_EstadoSistemaOK;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@RucEntity", RucEntity);
                //cmd.Parameters.AddWithValue("@fechtoday", Convert.ToDateTime(DateTime.Now.ToString("dd-MM-yyyy")));
                cmd.Parameters.AddWithValue("@fechtoday", Convert.ToDateTime(DateTime.Now.ToShortDateString()));

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexCriterio = objReader.GetOrdinal("Criterio");
                    int indexHoy = objReader.GetOrdinal("Hoy");
                    int indexAyer = objReader.GetOrdinal("Ayer");
                    int indexSemanaActual = objReader.GetOrdinal("SemanaActual");
                    int indexUltimaSemana = objReader.GetOrdinal("UltimaSemana");
                    int indexTotalMes = objReader.GetOrdinal("TotalMes");
                    int indexMesPasado = objReader.GetOrdinal("MesPasado");
                    int indexCodigo = objReader.GetOrdinal("Codigo");

                    while (objReader.Read())
                    {
                        oEstadoSistema = new SistemaEstado();

                        oEstadoSistema.Criterio = DataUtil.DbValueToDefault<string>(objReader[indexCriterio]);
                        oEstadoSistema.Hoy = DataUtil.DbValueToDefault<int>(objReader[indexHoy]);
                        oEstadoSistema.Ayer = DataUtil.DbValueToDefault<int>(objReader[indexAyer]);
                        oEstadoSistema.SemanaActual = DataUtil.DbValueToDefault<int>(objReader[indexSemanaActual]);
                        oEstadoSistema.UltimaSemana = DataUtil.DbValueToDefault<int>(objReader[indexUltimaSemana]);
                        oEstadoSistema.TotalMes = DataUtil.DbValueToDefault<int>(objReader[indexTotalMes]);
                        oEstadoSistema.MesPasado = DataUtil.DbValueToDefault<int>(objReader[indexMesPasado]);
                        oEstadoSistema.Codigo = DataUtil.DbValueToDefault<string>(objReader[indexCodigo]);
                        oListaSistemaEstado.Add(oEstadoSistema);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListaEstadoSistema_Ok ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaSistemaEstado;
        }


        public ListaSistemaEstado GetListaEstadoSistema_Error(string RucEntity)
        {
            SistemaEstado oEstadoSistema = new SistemaEstado();
            ListaSistemaEstado oListaSistemaEstado = new ListaSistemaEstado();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_EstadoSistemaError;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@RucEntity", RucEntity);
                //cmd.Parameters.AddWithValue("@fechtoday", DateTime.Now.ToString("dd-MM-yyyy"));
                cmd.Parameters.AddWithValue("@fechtoday", Convert.ToDateTime(DateTime.Now.ToShortDateString()));

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexCriterio = objReader.GetOrdinal("Criterio");
                    int indexHoy = objReader.GetOrdinal("Hoy");
                    int indexAyer = objReader.GetOrdinal("Ayer");
                    int indexSemanaActual = objReader.GetOrdinal("SemanaActual");
                    int indexUltimaSemana = objReader.GetOrdinal("UltimaSemana");
                    int indexTotalMes = objReader.GetOrdinal("TotalMes");
                    int indexMesPasado = objReader.GetOrdinal("MesPasado");
                    int indexCodigo = objReader.GetOrdinal("Codigo");

                    while (objReader.Read())
                    {
                        oEstadoSistema = new SistemaEstado();

                        oEstadoSistema.Criterio = DataUtil.DbValueToDefault<string>(objReader[indexCriterio]);
                        oEstadoSistema.Hoy = DataUtil.DbValueToDefault<int>(objReader[indexHoy]);
                        oEstadoSistema.Ayer = DataUtil.DbValueToDefault<int>(objReader[indexAyer]);
                        oEstadoSistema.SemanaActual = DataUtil.DbValueToDefault<int>(objReader[indexSemanaActual]);
                        oEstadoSistema.UltimaSemana = DataUtil.DbValueToDefault<int>(objReader[indexUltimaSemana]);
                        oEstadoSistema.TotalMes = DataUtil.DbValueToDefault<int>(objReader[indexTotalMes]);
                        oEstadoSistema.MesPasado = DataUtil.DbValueToDefault<int>(objReader[indexMesPasado]);
                        oEstadoSistema.Codigo = DataUtil.DbValueToDefault<string>(objReader[indexCodigo]);
                        oListaSistemaEstado.Add(oEstadoSistema);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListaEstadoSistema_Error ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaSistemaEstado;
        }
        #endregion


        #region CONSULTA PORTAL 

        public ListaFacturaElectronica GetListaHistorialCliente(FacturaElectronica oFacturaElectronica)
        {
            FacturaElectronica oDocumento = new FacturaElectronica();
            ListaFacturaElectronica oListaHistorial = new ListaFacturaElectronica();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ObtenerHistorialCliente_EmpresaPort;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@TipoDocumento", oFacturaElectronica.TipoDocumento.CodigoDocumento);
                cmd.Parameters.AddWithValue("@RucEmpresa", oFacturaElectronica.Empresa.RUC);
                cmd.Parameters.AddWithValue("@ClienteRuc_Dni", oFacturaElectronica.Cliente.NumeroDocumentoIdentidad);
                cmd.Parameters.AddWithValue("@FechaInicio", oFacturaElectronica.FechaInicio);
                cmd.Parameters.AddWithValue("@FechaFin", oFacturaElectronica.FechaFin);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("Nro");
                    int indexCodigoDocumento = objReader.GetOrdinal("CodigoDocumento");
                    int indexDescripcionTipoDocumento = objReader.GetOrdinal("Descripcion");
                    int indexSerie = objReader.GetOrdinal("Serie");
                    int indexNombreArchivoXML = objReader.GetOrdinal("NumeroDocumento");
                    int indexNumeroDocumento = objReader.GetOrdinal("NumeroFactura");
                    int indexFechaEmision = objReader.GetOrdinal("FechaEmision");
                    int indexNroDocClient = objReader.GetOrdinal("NumDocCliente");
                    int indexClienteDireccion = objReader.GetOrdinal("ClienteDireccion");
                    int indexCliente = objReader.GetOrdinal("Cliente");
                    int indexMontoTotal = objReader.GetOrdinal("MontoTotal");
                    int indexEstado = objReader.GetOrdinal("Estado");
                    int indexRutaImagen = objReader.GetOrdinal("RutaImagen");
                    int indexMoneda = objReader.GetOrdinal("Moneda");

                    int indexEmpresaRuc = objReader.GetOrdinal("EmpresaRuc");
                    int indexRazonSocial = objReader.GetOrdinal("RazonSocial");
                    int indexEmpresaDireccion = objReader.GetOrdinal("EmpresaDireccion");
                    int indexEmpresaUbigeo = objReader.GetOrdinal("EmpresaUbigeo");
                    int indexEmpresaUbigeoDesc = objReader.GetOrdinal("EmpresaUbigeoDesc");

                    int indexIdTipoDocumento = objReader.GetOrdinal("IdTipoDocumento");
                    int indexXML = objReader.GetOrdinal("XML");
                    int indexCDR = objReader.GetOrdinal("CDR");



                    //int indexCampo1 = objReader.GetOrdinal("Campo1");

                    int indextypeFormat = objReader.GetOrdinal("TypeFormat");

                    while (objReader.Read())
                    {
                        oDocumento = new FacturaElectronica();
                        oDocumento.Nro = DataUtil.DbValueToDefault<Int32>(objReader[indexNro]);
                        oDocumento.TipoDocumento = new TipoDocumento();
                        
                        oDocumento.TipoDocumento.CodigoDocumento = DataUtil.DbValueToDefault<String>(objReader[indexCodigoDocumento]);
                        oDocumento.TipoDocumento.Descripcion = DataUtil.DbValueToDefault<String>(objReader[indexDescripcionTipoDocumento]);

                        oDocumento.Serie = new Serie();
                        oDocumento.Serie.NumeroSerie = DataUtil.DbValueToDefault<String>(objReader[indexSerie]);
                        oDocumento.NombreArchivoXML = DataUtil.DbValueToDefault<string>(objReader[indexNombreArchivoXML]);
                        oDocumento.NumeroDocumento = DataUtil.DbValueToDefault<String>(objReader[indexNumeroDocumento]);
                        oDocumento.FechaEmision = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEmision]);
                        oDocumento.FechaEmision2 = (DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEmision])).ToString("yyyy/MM/dd");

                        oDocumento.Cliente = new Cliente();
                        oDocumento.Cliente.NumeroDocumentoIdentidad = DataUtil.DbValueToDefault<String>(objReader[indexNroDocClient]);
                        oDocumento.Cliente.Nombres = DataUtil.DbValueToDefault<String>(objReader[indexCliente]);
                        oDocumento.Cliente.Direccion = DataUtil.DbValueToDefault<String>(objReader[indexClienteDireccion]);

                        oDocumento.MontoTotal = DataUtil.DbValueToDefault<Decimal>(objReader[indexMontoTotal]);

                        oDocumento.Estado = new Estado();
                        oDocumento.Estado.Descripcion = DataUtil.DbValueToDefault<String>(objReader[indexEstado]);

                        oDocumento.RutaImagen = DataUtil.DbValueToDefault<String>(objReader[indexRutaImagen]);

                        oDocumento.Moneda = new Moneda();
                        oDocumento.Moneda.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexMoneda]);

                        oDocumento.Empresa = new Empresa();
                        oDocumento.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexEmpresaRuc]);
                        oDocumento.Empresa.RazonSocial = DataUtil.DbValueToDefault<string>(objReader[indexRazonSocial]);
                        oDocumento.Empresa.Direccion = DataUtil.DbValueToDefault<String>(objReader[indexEmpresaDireccion]);

                        oDocumento.Empresa.Ubigeo = new Ubigeo();
                        oDocumento.Empresa.Ubigeo.CodigoUbigeo = DataUtil.DbValueToDefault<String>(objReader[indexEmpresaUbigeo]);
                        oDocumento.Empresa.Ubigeo.Descripcion = DataUtil.DbValueToDefault<String>(objReader[indexEmpresaUbigeoDesc]);

                        oDocumento.TipoDocumento.IdTipoDocumento = DataUtil.DbValueToDefault<int>(objReader[indexIdTipoDocumento]);

                        string strData = string.Empty;

                        byte[] byteData = (byte[])objReader[indexXML];
                        strData = Encoding.GetEncoding("iso-8859-1").GetString(byteData);
                        oDocumento.XML = strData;

                        strData = string.Empty;
                        byteData = null;
                        byteData = (byte[])objReader[indexCDR];
                        strData = Encoding.GetEncoding("iso-8859-1").GetString(byteData);
                        oDocumento.CDR = strData;

                        //oDocumento.XML = DataUtil.DbValueToDefault<String>(objReader[indexXML]);

                        //oDocumento.Campo1 = DataUtil.DbValueToDefault<String>(objReader[indexCampo1]);
                        oDocumento.TypeFormat = DataUtil.DbValueToDefault<int>(objReader[indextypeFormat]);
                        oListaHistorial.Add(oDocumento);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListaHistorialCliente ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaHistorial;
        }



        public FacturaElectronica GetDocumentoPortalWeb(FacturaElectronica oFacturaElectronica)
        {
            FacturaElectronica objFactura = new FacturaElectronica();
            try
            {
                SqlCommand oDbCommand = new SqlCommand();

                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_ObtenerDocumentoPortalWeb;
                oDbCommand.Connection = cnn;

                oDbCommand.Parameters.AddWithValue("@TipoDocumento", oFacturaElectronica.TipoDocumento.CodigoDocumento);
                oDbCommand.Parameters.AddWithValue("@Serie", oFacturaElectronica.Serie.NumeroSerie);
                oDbCommand.Parameters.AddWithValue("@NroDocumento", oFacturaElectronica.NumeroDocumento);
                oDbCommand.Parameters.AddWithValue("@RucEmpresa", oFacturaElectronica.Empresa.RUC);
                oDbCommand.Parameters.AddWithValue("@FechaEmision", Convert.ToDateTime(oFacturaElectronica.FechaCarga));
                oDbCommand.Parameters.AddWithValue("@MontoTotal", oFacturaElectronica.MontoTotal);
                oDbCommand.Parameters.AddWithValue("@NroDocumentoCliente", oFacturaElectronica.Cliente.NumeroDocumentoIdentidad);

                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("Nro");
                    int indexCodigoDocumento = objReader.GetOrdinal("CodigoDocumento");
                    int indexDescripcionTipoDocumento = objReader.GetOrdinal("Descripcion");
                    int indexSerie = objReader.GetOrdinal("Serie");
                    int indexNombreArchivoXML = objReader.GetOrdinal("NumeroDocumento");
                    int indexNumeroDocumento = objReader.GetOrdinal("NumeroFactura");
                    int indexFechaEmision = objReader.GetOrdinal("FechaEmision");
                    int indexNroDocClient = objReader.GetOrdinal("NumDocCliente");
                    int indexClienteDireccion = objReader.GetOrdinal("ClienteDireccion");
                    int indexCliente = objReader.GetOrdinal("Cliente");
                    int indexMontoTotal = objReader.GetOrdinal("MontoTotal");
                    int indexEstado = objReader.GetOrdinal("Estado");
                    int indexRutaImagen = objReader.GetOrdinal("RutaImagen");
                    int indexMoneda = objReader.GetOrdinal("Moneda");

                    int indexEmpresaRuc = objReader.GetOrdinal("EmpresaRuc");
                    int indexRazonSocial = objReader.GetOrdinal("RazonSocial");
                    int indexEmpresaDireccion = objReader.GetOrdinal("EmpresaDireccion");
                    int indexEmpresaUbigeo = objReader.GetOrdinal("EmpresaUbigeo");
                    int indexEmpresaUbigeoDesc = objReader.GetOrdinal("EmpresaUbigeoDesc");
                    int indexIdTipoDocumento = objReader.GetOrdinal("IdTipoDocumento");
                    int indexXML = objReader.GetOrdinal("XML");

                    int indexCampo1 = objReader.GetOrdinal("Campo1");

                    int indextypeFormat = objReader.GetOrdinal("TypeFormat");

                    int indexCDR = objReader.GetOrdinal("CDR");

                    while (objReader.Read())
                    {
                        objFactura = new FacturaElectronica();
                        objFactura.Nro = DataUtil.DbValueToDefault<Int32>(objReader[indexNro]);
                        objFactura.TipoDocumento = new TipoDocumento();
                        objFactura.TipoDocumento.IdTipoDocumento = DataUtil.DbValueToDefault<int>(objReader[indexIdTipoDocumento]);
                        objFactura.TipoDocumento.CodigoDocumento = DataUtil.DbValueToDefault<String>(objReader[indexCodigoDocumento]);
                        objFactura.TipoDocumento.Descripcion = DataUtil.DbValueToDefault<String>(objReader[indexDescripcionTipoDocumento]);

                        objFactura.Serie = new Serie();
                        objFactura.Serie.NumeroSerie = DataUtil.DbValueToDefault<String>(objReader[indexSerie]);
                        objFactura.NombreArchivoXML = DataUtil.DbValueToDefault<string>(objReader[indexNombreArchivoXML]);
                        objFactura.NumeroDocumento = DataUtil.DbValueToDefault<String>(objReader[indexNumeroDocumento]);
                        objFactura.FechaEmision = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEmision]);
                        objFactura.FechaEmision2 = (DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEmision])).ToString("yyyy/MM/dd");

                        objFactura.Cliente = new Cliente();
                        objFactura.Cliente.NumeroDocumentoIdentidad = DataUtil.DbValueToDefault<String>(objReader[indexNroDocClient]);
                        objFactura.Cliente.Nombres = DataUtil.DbValueToDefault<String>(objReader[indexCliente]);
                        objFactura.Cliente.Direccion = DataUtil.DbValueToDefault<String>(objReader[indexClienteDireccion]);

                        objFactura.MontoTotal = DataUtil.DbValueToDefault<Decimal>(objReader[indexMontoTotal]);
                        objFactura.Estado = new Estado();
                        objFactura.Estado.Descripcion = DataUtil.DbValueToDefault<String>(objReader[indexEstado]);
                        objFactura.RutaImagen = DataUtil.DbValueToDefault<String>(objReader[indexRutaImagen]);
                        objFactura.Moneda = new Moneda();
                        objFactura.Moneda.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexMoneda]);

                        objFactura.Empresa = new Empresa();
                        objFactura.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexEmpresaRuc]);
                        objFactura.Empresa.RazonSocial = DataUtil.DbValueToDefault<string>(objReader[indexRazonSocial]);
                        objFactura.Empresa.Direccion = DataUtil.DbValueToDefault<String>(objReader[indexEmpresaDireccion]);

                        objFactura.Empresa.Ubigeo = new Ubigeo();
                        objFactura.Empresa.Ubigeo.CodigoUbigeo = DataUtil.DbValueToDefault<String>(objReader[indexEmpresaUbigeo]);
                        objFactura.Empresa.Ubigeo.Descripcion = DataUtil.DbValueToDefault<String>(objReader[indexEmpresaUbigeoDesc]);

                        //objFactura.XML = DataUtil.DbValueToDefault<String>(objReader[indexXML]);

                        string strData = string.Empty;
                        byte[] byteData = (byte[])objReader[indexXML];
                        strData = Encoding.GetEncoding("iso-8859-1").GetString(byteData);
                        objFactura.XML = strData;


                        strData = string.Empty;
                        byteData = null;
                        byteData = (byte[])objReader[indexCDR];
                        strData = Encoding.GetEncoding("iso-8859-1").GetString(byteData);
                        objFactura.CDR = strData;

                        objFactura.Campo1 = DataUtil.DbValueToDefault<String>(objReader[indexCampo1]);
                        objFactura.TypeFormat = DataUtil.DbValueToDefault<int>(objReader[indextypeFormat]);
                        
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetDocumentoPortalWeb ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return objFactura;
        }
 
        #endregion



        #region DOCUMENTO ENVIADO

        public String InsertarDocumentoEnviado(Documento odocumento)
        {
            string msj = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertarDocumentoEnviado;
                cmd.Connection = cnn;

                {
                    cmd.Parameters.AddWithValue("@IdTipoDocumento", odocumento.TipoDocumento.IdTipoDocumento);
                    cmd.Parameters.AddWithValue("@Serie", odocumento.Serie);
                    cmd.Parameters.AddWithValue("@Correlativo", odocumento.NumeroDocumento);
                    cmd.Parameters.AddWithValue("@Destino", odocumento.Destino);
                    cmd.Parameters.AddWithValue("@Asunto", odocumento.Asunto);
                    cmd.Parameters.AddWithValue("@Mensaje", odocumento.Mensaje);
                    cmd.Parameters.AddWithValue("@Remitente", odocumento.Remitente);
                    cmd.Parameters.AddWithValue("@FechaEnvio", odocumento.Fecha_Cad.ToShortDateString());
                    //cmd.Parameters.AddWithValue("@FechaEnvio", odocumento.Fecha_Cad);
                    cmd.Parameters.AddWithValue("@Fecha_Cad", odocumento.FechaEnvio);
                    cmd.Parameters.AddWithValue("@RucEmpresa", odocumento.Empresa.RUC);
                    cmd.Parameters.AddWithValue("@TPO_CE", odocumento.TipoDocumento.CodigoDocumento);

                    cmd.Parameters.AddWithValue("@FechaEnviado", odocumento.Fecha_Cad);

                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Registrar" : "Registrado Correctamente";
                    cnn.Close();
                }
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: InsertarDocumentoEnviado ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }


        public String InsertarListDocEnviado(List<Documento> listDocSend)
        {
            string msj = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertarDocumentoEnviado;
                cmd.Connection = cnn;


                foreach (var doc in listDocSend)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdTipoDocumento", doc.TipoDocumento.IdTipoDocumento);
                    cmd.Parameters.AddWithValue("@Serie", doc.Serie);
                    cmd.Parameters.AddWithValue("@Correlativo", doc.NumeroDocumento);
                    cmd.Parameters.AddWithValue("@Destino", doc.Destino);
                    cmd.Parameters.AddWithValue("@Asunto", doc.Asunto);
                    cmd.Parameters.AddWithValue("@Mensaje", doc.Mensaje);
                    cmd.Parameters.AddWithValue("@Remitente", doc.Remitente);
                    cmd.Parameters.AddWithValue("@FechaEnvio", doc.Fecha_Cad.ToShortDateString());
                    cmd.Parameters.AddWithValue("@Fecha_Cad", doc.FechaEnvio);
                    cmd.Parameters.AddWithValue("@RucEmpresa", doc.Empresa.RUC);
                    cmd.Parameters.AddWithValue("@TPO_CE", doc.TipoDocumento.CodigoDocumento);

                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Registrar" : "Registrado Correctamente";
                }

                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: InsertarListDocEnviado ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }

        public ListaDocumento GetListaDocumentoEnviado(Documento odocumento)
        {
            Documento objdocumento = new Documento();
            ListaDocumento olistaDocumento = new ListaDocumento();
            try
            {
                SqlCommand oDbCommand = new SqlCommand();

                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_ListaDocumentoEnviado;
                oDbCommand.Connection = cnn;

                oDbCommand.Parameters.AddWithValue("@FechaDesde", odocumento.FechaDesde);
                oDbCommand.Parameters.AddWithValue("@FechaHasta", odocumento.FechaHasta);
                oDbCommand.Parameters.AddWithValue("@Serie", odocumento.Serie);
                oDbCommand.Parameters.AddWithValue("@TipoDocumento", odocumento.TipoDocumento.IdTipoDocumento);
                oDbCommand.Parameters.AddWithValue("@RucEmpresa", odocumento.Empresa.RUC);

                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("Nro");
                    int indexIdTipoDocumento = objReader.GetOrdinal("IdTipoDocumento");
                    int indexTipoDocumento = objReader.GetOrdinal("TipoDocumento");
                    int indexDescripcionTipoDocumento = objReader.GetOrdinal("Descripcion");
                    int indexSerie = objReader.GetOrdinal("Serie");
                    int indexNumeroDocumento = objReader.GetOrdinal("NumeroDocumento");
                    int indexAsunto = objReader.GetOrdinal("Asunto");
                    int indexFechaEnvio = objReader.GetOrdinal("FechaEnvio");
                    int indexDestino = objReader.GetOrdinal("Destino");
                    int indexRemitente = objReader.GetOrdinal("Remitente");

                    while (objReader.Read())
                    {
                        objdocumento = new Documento();
                        objdocumento.TipoDocumento = new TipoDocumento();

                        objdocumento.Nro = DataUtil.DbValueToDefault<int>(objReader[indexNro]);
                        objdocumento.TipoDocumento.IdTipoDocumento = DataUtil.DbValueToDefault<int>(objReader[indexIdTipoDocumento]);
                        objdocumento.TipoDocumento.CodigoDocumento = DataUtil.DbValueToDefault<string>(objReader[indexTipoDocumento]);
                        objdocumento.TipoDocumento.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcionTipoDocumento]);
                        objdocumento.Serie = DataUtil.DbValueToDefault<string>(objReader[indexSerie]);
                        objdocumento.NumeroDocumento = DataUtil.DbValueToDefault<string>(objReader[indexNumeroDocumento]);
                        objdocumento.Asunto = DataUtil.DbValueToDefault<string>(objReader[indexAsunto]);

                        objdocumento.FechaEnvio = (DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEnvio])).ToString("yyyy/MM/dd HH:mm:ss tt", System.Globalization.CultureInfo.GetCultureInfo("es-PE"));
                        objdocumento.FechaEnviado = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEnvio]);

                        objdocumento.Destino = DataUtil.DbValueToDefault<string>(objReader[indexDestino]);
                        objdocumento.Remitente = DataUtil.DbValueToDefault<string>(objReader[indexRemitente]);
                        olistaDocumento.Add(objdocumento);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListaDocumentoEnviado ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return olistaDocumento;
        }

        #endregion


        #region DOCUMENTO XML RECIBIDO DE SUNAT

        public ListaDocumento GetListaIfExistsDocumentoCabecera(string NUM_CPE)
        {
            Documento oDocumento = new Documento();
            ListaDocumento oListaDocumento = new ListaDocumento();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_IfExistsDocument;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@NUM_CPE", NUM_CPE);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("Nro");

                    while (objReader.Read())
                    {
                        oDocumento = new Documento();
                        oDocumento.Nro = DataUtil.DbValueToDefault<int>(objReader[indexNro]);
                        oListaDocumento.Add(oDocumento);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListaIfExistsDocumentoCabecera ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaDocumento;
        }


        



        #endregion


        #region METHOD IDOCUMENT RECEIVED

        public int InsertarDocumentoCabecera_Rec(FacturaElectronica oFactura, byte[] XmlVarBinary)
        {
            //string msj = string.Empty;
            int result = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertaDocumentoCabecera_Rec;
                cmd.Connection = cnn;

                {
                    cmd.Parameters.AddWithValue("@NUM_CPE", oFactura.NombreArchivoXML);
                    cmd.Parameters.AddWithValue("@ID_TPO_CPE", oFactura.TipoDocumento.CodigoDocumento);
                    cmd.Parameters.AddWithValue("@ID_CPE", oFactura.SerieCorrelativo);
                    cmd.Parameters.AddWithValue("@ID_TPO_OPERACION", oFactura.TipoDocumento.CodigoDocumento);
                    cmd.Parameters.AddWithValue("@FEC_EMIS", oFactura.FechaEmision);
                    cmd.Parameters.AddWithValue("@TPO_MONEDA", oFactura.TipoMoneda);
                    cmd.Parameters.AddWithValue("@EM_TPO_DOC", oFactura.Empresa.TipoDocumentiIdentidad.Codigo);
                    cmd.Parameters.AddWithValue("@EM_NUM_DOCU", oFactura.Empresa.RUC);
                    cmd.Parameters.AddWithValue("@EM_NOMBRE", oFactura.Empresa.RazonSocial);
                    cmd.Parameters.AddWithValue("@EM_NCOMER", oFactura.Empresa.RazonComercial);
                    cmd.Parameters.AddWithValue("@EM_UBIGEO", oFactura.Empresa.CodigoUbigeo);
                    cmd.Parameters.AddWithValue("@EM_DFISCAL", oFactura.Empresa.Direccion);

                    cmd.Parameters.AddWithValue("@EM_DURBANIZ", oFactura.Empresa.Urbanizacion);
                    cmd.Parameters.AddWithValue("@EM_DIR_DPTO", oFactura.Empresa.Departamento);
                    cmd.Parameters.AddWithValue("@EM_DIR_PROV", oFactura.Empresa.Provincia);
                    cmd.Parameters.AddWithValue("@EM_DIR_DIST", oFactura.Empresa.Distrito);
                    cmd.Parameters.AddWithValue("@EM_COD_PAIS", oFactura.Empresa.CodPais);

                    cmd.Parameters.AddWithValue("@RE_TPODOC", oFactura.Cliente.TipoDocumentoIdentidad.Codigo);
                    cmd.Parameters.AddWithValue("@RE_NUMDOC", oFactura.Cliente.ClienteRuc);
                    cmd.Parameters.AddWithValue("@RE_NOMBRE", oFactura.Cliente.RazonSocial);
                    cmd.Parameters.AddWithValue("@RE_DIRECCION", oFactura.Cliente.Direccion);
                    cmd.Parameters.AddWithValue("@TOT_GRAV_MTO", decimal.Parse(oFactura.TotalGravado));
                    cmd.Parameters.AddWithValue("@TOT_SUMA_IGV", decimal.Parse(oFactura.MontoIgvCad));
                    cmd.Parameters.AddWithValue("@TOT_IMPOR_TOTAL", oFactura.MontoTotal);
                    cmd.Parameters.AddWithValue("@MONTOLITERAL", oFactura.MontoTotalLetras);
                    cmd.Parameters.AddWithValue("@SERIE", oFactura.NumeroSerie);
                    cmd.Parameters.AddWithValue("@NUM_DOCUMENTO", oFactura.NumeroDocumento);
                    //cmd.Parameters.AddWithValue("@Id_ED", oFactura);
                    //cmd.Parameters.AddWithValue("@XmlVarBinary", XmlVarBinary);

                    SqlParameter XML = new SqlParameter("@VAR_FIR", SqlDbType.VarBinary);
                    XML.Value = XmlVarBinary;
                    cmd.Parameters.Add(XML);

                    SqlDataReader dr;
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        int Id = int.Parse(dr[0].ToString().Trim());
                        if (Id > 0)
                        {
                            cnn.Close();
                            return Id;
                        }
                        else { cnn.Close(); }
                    }
                    else { cnn.Close(); }
                    //int result = cmd.ExecuteNonQuery();
                    //msj = result == 0 ? "Error al Registrar" : "Registrado Correctamente";
                    //cnn.Close();
                }
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: InsertarDocumentoCabecera_Rec ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return result;
        }
        public string InsertarDocumentoDetalle_Rec(ListaDetalleFacturaElectronica olistadetalle, int ID_DC)
        {
            string msj = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertaDocumentoDetalle_Rec;
                cmd.Connection = cnn;

                for (int i = 0; i <= olistadetalle.Count - 1; i++)
                {
                    cmd.Parameters.Clear();

                    cmd.Parameters.AddWithValue("@ID_DC", ID_DC);
                    cmd.Parameters.AddWithValue("@NUM_CPE", olistadetalle[i].NombreArchivoXML);
                    cmd.Parameters.AddWithValue("@IT_NRO_ORD", olistadetalle[i].NroOrden);
                    cmd.Parameters.AddWithValue("@IT_UND_MED", olistadetalle[i].Unidad);
                    cmd.Parameters.AddWithValue("@IT_CANT_ITEM", olistadetalle[i].Cantidad);
                    cmd.Parameters.AddWithValue("@IT_COD_PROD", olistadetalle[i].CodigoProducto);
                    cmd.Parameters.AddWithValue("@IT_DESCRIP", olistadetalle[i].Descripcion);
                    cmd.Parameters.AddWithValue("@IT_VAL_UNIT", olistadetalle[i].ValorUnitario);
                    cmd.Parameters.AddWithValue("@IT_MNT_PVTA", olistadetalle[i].PrecioVenta);
                    cmd.Parameters.AddWithValue("@IT_VAL_VTA", olistadetalle[i].ValorVenta);
                    cmd.Parameters.AddWithValue("@IT_MTO_IGV", olistadetalle[i].IGV);
                    cmd.Parameters.AddWithValue("@IT_COD_AFE_IGV", olistadetalle[i].CodigoAfectoIGV);
                    cmd.Parameters.AddWithValue("@SERIE", olistadetalle[i].NumeroSerie);
                    cmd.Parameters.AddWithValue("@NUM_DOCUMENTO", olistadetalle[i].NumeroDocumento);

                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Registrar" : "Registrado Correctamente";
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: InsertarDocumentoDetalle_Rec ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }
        public string InsertarDocumentoDetalle_CRE_CPE(ListaDocCRECPE olistadetalle, int ID_DC)
        {
            string msj = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertDocumentoDetalle_Rec_rp;
                cmd.Connection = cnn;

                for (int i = 0; i <= olistadetalle.Count - 1; i++)
                {
                    cmd.Parameters.Clear();

                    cmd.Parameters.AddWithValue("@ID_DC", ID_DC);
                    cmd.Parameters.AddWithValue("@NUM_CPE", olistadetalle[i].NombreArchivoXML);
                    cmd.Parameters.AddWithValue("@IT_NRO_ORD", olistadetalle[i].NroOrden);
                    cmd.Parameters.AddWithValue("@SERIE", olistadetalle[i].NumeroSerie);
                    cmd.Parameters.AddWithValue("@NUM_DOCUMENTO", olistadetalle[i].NumeroDocumento);

                    cmd.Parameters.AddWithValue("@TPODOCRELAC", olistadetalle[i].TipoDocRelac);
                    cmd.Parameters.AddWithValue("@NUMDOCRELAC", olistadetalle[i].NroDocRelac);
                    cmd.Parameters.AddWithValue("@FEMISDOCRELAC", olistadetalle[i].FechaEmiDocRelac);
                    cmd.Parameters.AddWithValue("@ITOTDOCRELAC", olistadetalle[i].ImporteTotDocRelac);
                    cmd.Parameters.AddWithValue("@MDOCRELAC", olistadetalle[i].MonedaImpTotDocRelac);
                    cmd.Parameters.AddWithValue("@FECMOVI", olistadetalle[i].FechaPagoDate);
                    cmd.Parameters.AddWithValue("@NUMMOVI", olistadetalle[i].NumeroPago);
                    cmd.Parameters.AddWithValue("@IMPSOPERMOV", olistadetalle[i].ImporteTotDocRelac);
                    cmd.Parameters.AddWithValue("@MONMOVI", olistadetalle[i].MonedaImpTotDocRelac);
                    cmd.Parameters.AddWithValue("@IMPOPER", olistadetalle[i].ImporteRetenido);
                    cmd.Parameters.AddWithValue("@MONIMPOPER", olistadetalle[i].MonedaImpRetenido);
                    cmd.Parameters.AddWithValue("@FECOPER", olistadetalle[i].FechaRetencionDate);
                    cmd.Parameters.AddWithValue("@IMPTOTOPER", olistadetalle[i].ImporteTotxPagoNeto);
                    cmd.Parameters.AddWithValue("@MONOPER", olistadetalle[i].MonedaTotxPagoNeto);
                    cmd.Parameters.AddWithValue("@MONREFETC", olistadetalle[i].MonedaRefTpoCambio);
                    cmd.Parameters.AddWithValue("@MONDESTTC", olistadetalle[i].MonedaRefTpoCambio);
                    cmd.Parameters.AddWithValue("@FACTORTC", olistadetalle[i].TasaRetencion);
                    cmd.Parameters.AddWithValue("@FECHATC", olistadetalle[i].FechaTipoCambio);


                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Registrar" : "Registrado Correctamente";
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: InsertarDocumentoDetalle_CRE_CPE ] " + ex.Message);



                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }


        public ListaFacturaElectronica GetListaDocumentReceived(FacturaElectronica oFacturaElectronica)
        {
            FacturaElectronica objFactura = new FacturaElectronica();
            ListaFacturaElectronica objListaFactura = new ListaFacturaElectronica();

            try
            {
                SqlCommand oDbCommand = new SqlCommand();

                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_GetListDocumentoCabeceraReceived;
                oDbCommand.Connection = cnn;

                oDbCommand.Parameters.AddWithValue("@FechaInicio", oFacturaElectronica.FechaInicio);
                oDbCommand.Parameters.AddWithValue("@FechaFin", oFacturaElectronica.FechaFin);
                oDbCommand.Parameters.AddWithValue("@TipoDocumento", oFacturaElectronica.TipoDocumento.CodigoDocumento);
                //oDbCommand.Parameters.AddWithValue("@Serie", oFacturaElectronica.Serie.NumeroSerie);
                oDbCommand.Parameters.AddWithValue("@NumeroDocumentoInicio", oFacturaElectronica.NumeroDocumentoInicio);
                oDbCommand.Parameters.AddWithValue("@NumeroDocumentoFin", oFacturaElectronica.NumeroDocumentoFin);
                //oDbCommand.Parameters.AddWithValue("@Estado", oFacturaElectronica.Estado.IdEstado);
                oDbCommand.Parameters.AddWithValue("@RucEmisor", oFacturaElectronica.Empresa.RUC);
                oDbCommand.Parameters.AddWithValue("@RazonSocialEmisor", oFacturaElectronica.Empresa.RazonSocial);
                oDbCommand.Parameters.AddWithValue("@RucCliente", oFacturaElectronica.Cliente.ClienteRuc);
                //oDbCommand.Parameters.AddWithValue("@Id_ED_DOC", oFacturaElectronica.Estado.IdEstadoSUNAT);

                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("Nro");
                    int indexCodigoDocumento = objReader.GetOrdinal("CodigoDocumento");
                    int indexDescripcionTipoDocumento = objReader.GetOrdinal("Descripcion");
                    int indexSerie = objReader.GetOrdinal("Serie");
                    int indexNombreArchivoXML = objReader.GetOrdinal("NumeroDocumento");
                    int indexNumeroDocumento = objReader.GetOrdinal("NumeroFactura");
                    int indexFechaEmision = objReader.GetOrdinal("FechaEmision");
                    int indexNumDocEmpresa = objReader.GetOrdinal("NumDocEmpresa");
                    int indexEmisorDireccion = objReader.GetOrdinal("EmisorDireccion");
                    int indexEmpresa = objReader.GetOrdinal("Empresa");
                    int indexMontoTotal = objReader.GetOrdinal("MontoTotal");
                    //int indexEstado = objReader.GetOrdinal("Estado");
                    //int indexRutaImagen = objReader.GetOrdinal("RutaImagen");
                    int indexMoneda = objReader.GetOrdinal("Moneda");

                    //int indexEmpresaRuc = objReader.GetOrdinal("EmpresaRuc");
                    //int indexRazonSocial = objReader.GetOrdinal("RazonSocial");
                    //int indexEmpresaDireccion = objReader.GetOrdinal("EmpresaDireccion");
                    //int indexEmpresaUbigeo = objReader.GetOrdinal("EmpresaUbigeo");
                    //int indexEmpresaUbigeoDesc = objReader.GetOrdinal("EmpresaUbigeoDesc");

                    int indexIdTipoDocumento = objReader.GetOrdinal("IdTipoDocumento");

                    int indexXML = objReader.GetOrdinal("XML");

                    //int indexCampo1 = objReader.GetOrdinal("Campo1");

                    //int indexCodeMessage = objReader.GetOrdinal("CodeMessage");
                    //int indexDocMessage = objReader.GetOrdinal("DocMessage");

                    //int indexCodeResponse = objReader.GetOrdinal("CodeResponse");
                    //int indexNoteResponse = objReader.GetOrdinal("NoteResponse");

                    //int indextypeFormat = objReader.GetOrdinal("TypeFormat");

                    while (objReader.Read())
                    {
                        objFactura = new FacturaElectronica();
                        objFactura.Empresa = new Empresa();
                        objFactura.TipoDocumento = new TipoDocumento();
                        objFactura.Cliente = new Cliente();
                        objFactura.Moneda = new Moneda();
                        objFactura.Serie = new Serie();
                        //objFactura.Estado = new Estado();

                        objFactura.Nro = DataUtil.DbValueToDefault<Int32>(objReader[indexNro]);

                        objFactura.TipoDocumento.IdTipoDocumento = DataUtil.DbValueToDefault<int>(objReader[indexIdTipoDocumento]);
                        objFactura.TipoDocumento.CodigoDocumento = DataUtil.DbValueToDefault<String>(objReader[indexCodigoDocumento]);
                        objFactura.TipoDocumento.Descripcion = DataUtil.DbValueToDefault<String>(objReader[indexDescripcionTipoDocumento]);

                        objFactura.Empresa.RUC = DataUtil.DbValueToDefault<String>(objReader[indexNumDocEmpresa]);
                        objFactura.Empresa.RazonSocial = DataUtil.DbValueToDefault<String>(objReader[indexEmpresa]);
                        objFactura.Empresa.Direccion = DataUtil.DbValueToDefault<String>(objReader[indexEmisorDireccion]);

                        objFactura.Serie.NumeroSerie = DataUtil.DbValueToDefault<String>(objReader[indexSerie]);

                        objFactura.NombreArchivoXML = DataUtil.DbValueToDefault<string>(objReader[indexNombreArchivoXML]);
                        objFactura.NumeroDocumento = DataUtil.DbValueToDefault<String>(objReader[indexNumeroDocumento]);

                        objFactura.FechaEmision = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEmision]);
                        objFactura.FechaEmision2 = (DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaEmision])).ToString("yyyy/MM/dd");

                        objFactura.MontoTotal = DataUtil.DbValueToDefault<Decimal>(objReader[indexMontoTotal]);
                        objFactura.Moneda.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexMoneda]);

                        string strData = string.Empty;
                        byte[] byteData = (byte[])objReader[indexXML];
                        strData = Encoding.GetEncoding("iso-8859-1").GetString(byteData);
                        objFactura.XML = strData;

                        objListaFactura.Add(objFactura);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListaDocumentReceived ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return objListaFactura;
        }


        #endregion END INSERT I DOCUMENT RECEIVED



        #region INTERFACE XML

        public ListaFacturaElectronica GetListDocumentoSendMail()
        {
            FacturaElectronica oDocumento = new FacturaElectronica();
            ListaFacturaElectronica oListaDocumento = new ListaFacturaElectronica();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_VerificarStatusDocxSend;
                cmd.Connection = cnn;

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdCab = objReader.GetOrdinal("Id");
                    int indexNomb_Doc = objReader.GetOrdinal("NombreArchivo");
                    int indexTipoDoc = objReader.GetOrdinal("TipoDocumento");
                    int indexSerieCorre = objReader.GetOrdinal("SerieCorrelativo");
                    int indexIdEstado = objReader.GetOrdinal("IdEstado");
                    int indexDescEstado = objReader.GetOrdinal("DescEstado");
                    int indexXml = objReader.GetOrdinal("XML");
                    int indexEmpresaRuc = objReader.GetOrdinal("RucEmpresa");
                    int indexClientRuc = objReader.GetOrdinal("ClienteRuc");
                    int indexPara = objReader.GetOrdinal("Para");
                    int indexCC = objReader.GetOrdinal("CC");
                    int indexCCO = objReader.GetOrdinal("CCO");

                    int indexCampo1 = objReader.GetOrdinal("Campo1");

                    int indexRefFiles = objReader.GetOrdinal("REF_FILES");
                    int indextypeFormat = objReader.GetOrdinal("TypeFormat");

                    while (objReader.Read())
                    {
                        oDocumento = new FacturaElectronica();
                        oDocumento.Estado = new Estado();
                        oDocumento.TipoDocumento = new TipoDocumento();
                        oDocumento.Cliente = new Cliente();
                        oDocumento.Email = new Email();
                        oDocumento.Empresa = new Empresa();

                        oDocumento.IdFactura = DataUtil.DbValueToDefault<int>(objReader[indexIdCab]);
                        oDocumento.NombreArchivoXML = DataUtil.DbValueToDefault<string>(objReader[indexNomb_Doc]).TrimEnd();
                        oDocumento.TipoDocumento.CodigoDocumento = DataUtil.DbValueToDefault<string>(objReader[indexTipoDoc]);
                        oDocumento.SerieCorrelativo = DataUtil.DbValueToDefault<string>(objReader[indexSerieCorre]);
                        oDocumento.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        oDocumento.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescEstado]);

                        string strData = string.Empty;
                        byte[] byteData = (byte[])objReader[indexXml];
                        strData = Encoding.GetEncoding("iso-8859-1").GetString(byteData);
                        oDocumento.XML = strData;

                        oDocumento.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexEmpresaRuc]);
                        oDocumento.Cliente.ClienteRuc = DataUtil.DbValueToDefault<string>(objReader[indexClientRuc]).TrimEnd();
                        oDocumento.Email.Para = DataUtil.DbValueToDefault<string>(objReader[indexPara]);
                        oDocumento.Email.CC = DataUtil.DbValueToDefault<string>(objReader[indexCC]);
                        oDocumento.Email.CCO = DataUtil.DbValueToDefault<string>(objReader[indexCCO]);

                        oDocumento.Campo1 = DataUtil.DbValueToDefault<string>(objReader[indexCampo1]);
                        oDocumento.REF_FILES = DataUtil.DbValueToDefault<string>(objReader[indexRefFiles]);
                        oDocumento.TypeFormat = DataUtil.DbValueToDefault<int>(objReader[indextypeFormat]);
                        oListaDocumento.Add(oDocumento);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListDocumentoSendMail ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaDocumento;
        }


        public ListaFacturaElectronica GetListDocumentoSendMail_Parameter(string NUM_CE)
        {
            FacturaElectronica oDocumento = new FacturaElectronica();
            ListaFacturaElectronica oListaDocumento = new ListaFacturaElectronica();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_VerificarStatusDocxSend_Parameter;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@NUM_CE", NUM_CE);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdCab = objReader.GetOrdinal("Id");
                    int indexNomb_Doc = objReader.GetOrdinal("NombreArchivo");
                    int indexTipoDoc = objReader.GetOrdinal("TipoDocumento");
                    int indexSerieCorre = objReader.GetOrdinal("SerieCorrelativo");
                    int indexIdEstado = objReader.GetOrdinal("IdEstado");
                    int indexDescEstado = objReader.GetOrdinal("DescEstado");
                    int indexXml = objReader.GetOrdinal("XML");
                    int indexEmpresaRuc = objReader.GetOrdinal("RucEmpresa");
                    int indexClientRuc = objReader.GetOrdinal("ClienteRuc");
                    int indexPara = objReader.GetOrdinal("Para");
                    int indexCC = objReader.GetOrdinal("CC");
                    int indexCCO = objReader.GetOrdinal("CCO");

                    int indexCampo1 = objReader.GetOrdinal("Campo1");

                    int indexRefFiles = objReader.GetOrdinal("REF_FILES");

                    int indextypeFormat = objReader.GetOrdinal("TypeFormat");

                    while (objReader.Read())
                    {
                        oDocumento = new FacturaElectronica();
                        oDocumento.Estado = new Estado();
                        oDocumento.TipoDocumento = new TipoDocumento();
                        oDocumento.Cliente = new Cliente();
                        oDocumento.Email = new Email();
                        oDocumento.Empresa = new Empresa();

                        oDocumento.IdFactura = DataUtil.DbValueToDefault<int>(objReader[indexIdCab]);
                        oDocumento.NombreArchivoXML = DataUtil.DbValueToDefault<string>(objReader[indexNomb_Doc]).TrimEnd();
                        oDocumento.TipoDocumento.CodigoDocumento = DataUtil.DbValueToDefault<string>(objReader[indexTipoDoc]);
                        oDocumento.SerieCorrelativo = DataUtil.DbValueToDefault<string>(objReader[indexSerieCorre]);
                        oDocumento.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        oDocumento.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescEstado]);

                        string strData = string.Empty;
                        byte[] byteData = (byte[])objReader[indexXml];
                        strData = Encoding.GetEncoding("iso-8859-1").GetString(byteData);
                        oDocumento.XML = strData;

                        oDocumento.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexEmpresaRuc]);
                        oDocumento.Cliente.ClienteRuc = DataUtil.DbValueToDefault<string>(objReader[indexClientRuc]).TrimEnd();
                        oDocumento.Email.Para = DataUtil.DbValueToDefault<string>(objReader[indexPara]);
                        oDocumento.Email.CC = DataUtil.DbValueToDefault<string>(objReader[indexCC]);
                        oDocumento.Email.CCO = DataUtil.DbValueToDefault<string>(objReader[indexCCO]);

                        oDocumento.Campo1 = DataUtil.DbValueToDefault<string>(objReader[indexCampo1]);
                        oDocumento.REF_FILES = DataUtil.DbValueToDefault<string>(objReader[indexRefFiles]);
                        oDocumento.TypeFormat = DataUtil.DbValueToDefault<int>(objReader[indextypeFormat]);
                        oListaDocumento.Add(oDocumento);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListDocumentoSendMail_Parameter ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaDocumento;
        }


        public Cliente GetEmailClient(string NroDocumento)
        {
            Cliente oClient = new Cliente();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetEmailClient;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@NroDocumento", NroDocumento);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexNroDocumento = objReader.GetOrdinal("NroDocumento");
                    int indexRazonSocial = objReader.GetOrdinal("RazonSocial");
                    int indexEmail = objReader.GetOrdinal("Email");

                    while (objReader.Read())
                    {
                        oClient = new Cliente();
                        oClient.EmailClient = new Email();
                        oClient.NroDocumento = DataUtil.DbValueToDefault<string>(objReader[indexNroDocumento]);
                        oClient.RazonSocial = DataUtil.DbValueToDefault<string>(objReader[indexRazonSocial]);
                        oClient.EmailClient.Para = DataUtil.DbValueToDefault<string>(objReader[indexEmail]);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetEmailClient ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oClient;
        }


        public string UpdateDocCabStatuSend(ListaFacturaElectronica olistaUpdate)
        {
            string msj = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_UpdateStatusDocSend;
                cmd.Connection = cnn;

                for (int i = 0; i <= olistaUpdate.Count - 1; i++)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID_DC", olistaUpdate[i].IdFactura);
                    cmd.Parameters.AddWithValue("@NUM_CPE", olistaUpdate[i].NombreArchivoXML);
                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Registrar" : "Registrado Correctamente";
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: UpdateDocCabStatuSend ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }


















        public ListaFacturaElectronica GetListDocumentoPrint_Parameter(string NUM_CE)
        {
            FacturaElectronica oDocumento = new FacturaElectronica();
            ListaFacturaElectronica oListaDocumento = new ListaFacturaElectronica();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_VerificarStatusDocxPrint_Parameter;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@NUM_CE", NUM_CE);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdCab = objReader.GetOrdinal("Id");
                    int indexNomb_Doc = objReader.GetOrdinal("NombreArchivo");
                    int indexTipoDoc = objReader.GetOrdinal("TipoDocumento");
                    int indexSerieCorre = objReader.GetOrdinal("SerieCorrelativo");
                    int indexIdEstado = objReader.GetOrdinal("IdEstado");
                    int indexDescEstado = objReader.GetOrdinal("DescEstado");
                    int indexXml = objReader.GetOrdinal("XML");
                    int indexEmpresaRuc = objReader.GetOrdinal("RucEmpresa");
                    //int indexClientRuc = objReader.GetOrdinal("ClienteRuc");
                    //int indexPara = objReader.GetOrdinal("Para");
                    //int indexCC = objReader.GetOrdinal("CC");
                    //int indexCCO = objReader.GetOrdinal("CCO");

                    int indexCampo1 = objReader.GetOrdinal("Campo1");

                    //int indexRefFiles = objReader.GetOrdinal("REF_FILES");
                    int indexImpresora = objReader.GetOrdinal("Impresora");

                    int indextypeFormat = objReader.GetOrdinal("TypeFormat");

                    while (objReader.Read())
                    {
                        oDocumento = new FacturaElectronica();
                        oDocumento.Estado = new Estado();
                        oDocumento.TipoDocumento = new TipoDocumento();
                        oDocumento.Cliente = new Cliente();
                        oDocumento.Email = new Email();
                        oDocumento.Empresa = new Empresa();

                        oDocumento.IdFactura = DataUtil.DbValueToDefault<int>(objReader[indexIdCab]);
                        oDocumento.NombreArchivoXML = DataUtil.DbValueToDefault<string>(objReader[indexNomb_Doc]).TrimEnd();
                        oDocumento.TipoDocumento.CodigoDocumento = DataUtil.DbValueToDefault<string>(objReader[indexTipoDoc]);
                        oDocumento.SerieCorrelativo = DataUtil.DbValueToDefault<string>(objReader[indexSerieCorre]);
                        oDocumento.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        oDocumento.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescEstado]);

                        string strData = string.Empty;
                        byte[] byteData = (byte[])objReader[indexXml];
                        strData = Encoding.GetEncoding("iso-8859-1").GetString(byteData);
                        oDocumento.XML = strData;

                        oDocumento.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexEmpresaRuc]);
                        //oDocumento.Cliente.ClienteRuc = DataUtil.DbValueToDefault<string>(objReader[indexClientRuc]).TrimEnd();
                        //oDocumento.Email.Para = DataUtil.DbValueToDefault<string>(objReader[indexPara]);
                        //oDocumento.Email.CC = DataUtil.DbValueToDefault<string>(objReader[indexCC]);
                        //oDocumento.Email.CCO = DataUtil.DbValueToDefault<string>(objReader[indexCCO]);

                        oDocumento.Campo1 = DataUtil.DbValueToDefault<string>(objReader[indexCampo1]);
                        //oDocumento.REF_FILES = DataUtil.DbValueToDefault<string>(objReader[indexRefFiles]);

                        oDocumento.Impresora = DataUtil.DbValueToDefault<string>(objReader[indexImpresora]);
                        oDocumento.TypeFormat = DataUtil.DbValueToDefault<int>(objReader[indextypeFormat]);
                        oListaDocumento.Add(oDocumento);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListDocumentoPrint_Parameter ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaDocumento;
        }



        public string UpdateDocCabStatusPrint(ListaFacturaElectronica olistaUpdate)
        {
            string msj = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_UpdateStatusDocPrint;
                cmd.Connection = cnn;

                for (int i = 0; i <= olistaUpdate.Count - 1; i++)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID_DC", olistaUpdate[i].IdFactura);
                    cmd.Parameters.AddWithValue("@NUM_CPE", olistaUpdate[i].NombreArchivoXML);
                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Registrar" : "Registrado Correctamente";
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: UpdateDocCabStatusPrint ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }


        #endregion

        #region CATALOGO

        public String InsertTipoDocumentSend(ListaEstadoEnvio ListSend)
        {
            string msj = string.Empty;
            int IdRet = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertTipoDoc_Send;
                cmd.Connection = cnn;

                foreach (var obj in ListSend)
                {
                    IdRet = 0;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Tipo_ce", obj.TipoDocumento.CodigoDocumento);
                    cmd.Parameters.AddWithValue("@IdEstado", obj.IdEstadoEnvio);
                    cmd.Parameters.AddWithValue("@RucEntity", obj.Empresa.RUC);
                    IdRet = cmd.ExecuteNonQuery();

                    msj = IdRet > 0 ? "Registrado Correctamente" : "Error al Registrar";
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                msj = ex.Message;

                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: InsertTipoDocumentSend ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }

        public String InsertTipoDocumentPrint(ListaEstadoPrint ListPrint)
        {
            string msj = string.Empty;
            int IdRet = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertTipoDoc_Print;
                cmd.Connection = cnn;

                foreach (var obj in ListPrint)
                {
                    IdRet = 0;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Tipo_ce", obj.TipoDocumento.CodigoDocumento);
                    cmd.Parameters.AddWithValue("@IdEstado", obj.IdEstadoPrint);
                    cmd.Parameters.AddWithValue("@RucEntity", obj.Empresa.RUC);
                    IdRet = cmd.ExecuteNonQuery();

                    msj = IdRet > 0 ? "Registrado Correctamente" : "Error al Registrar";
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: InsertTipoDocumentPrint ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }



        public String DeleteTipoDocumentSend(ListaEstadoEnvio ListSend)
        {
            string msj = string.Empty;
            int IdRet = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_DeteleTipoDoc_Send;
                cmd.Connection = cnn;

                foreach (var obj in ListSend)
                {
                    IdRet = 0;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Tipo_ce", obj.TipoDocumento.CodigoDocumento);
                    cmd.Parameters.AddWithValue("@IdEstado", obj.IdEstadoEnvio);
                    cmd.Parameters.AddWithValue("@RucEntity", obj.Empresa.RUC);
                    IdRet = cmd.ExecuteNonQuery();

                    msj = IdRet > 0 ? "Actualizado Correctamente" : "Error al Quitar";
                }
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: DeleteTipoDocumentSend ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }


        public String DeleteTipoDocumentPrint(ListaEstadoPrint ListPrint)
        {
            string msj = string.Empty;
            int IdRet = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_DeleteTipoDoc_Print;
                cmd.Connection = cnn;

                foreach (var obj in ListPrint)
                {
                    IdRet = 0;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Tipo_ce", obj.TipoDocumento.CodigoDocumento);
                    cmd.Parameters.AddWithValue("@IdEstado", obj.IdEstadoPrint);
                    cmd.Parameters.AddWithValue("@RucEntity", obj.Empresa.RUC);
                    IdRet = cmd.ExecuteNonQuery();

                    msj = IdRet > 0 ? "Quitado Correctamente" : "Error al Quitar";
                }
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: DeleteTipoDocumentPrint ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }
        

        public ListaEstadoEnvio GetListEstadoEnvio(int IdEstadoEnvio, string RucEmpresa)
        {
            EstadoEnvio Objeto = new EstadoEnvio();
            ListaEstadoEnvio oListaObjeto = new ListaEstadoEnvio();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetstatusSend_TPO_CE;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdEstadoDocumento", IdEstadoEnvio);
                cmd.Parameters.AddWithValue("@RucEntity", RucEmpresa);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdEstadoEnvio = objReader.GetOrdinal("IdEstadoEnvio");
                    int indexCodigoDocumento = objReader.GetOrdinal("CodigoDocumento");
                    int indexIdTipoDocumento = objReader.GetOrdinal("IdTipoDocumento");

                    while (objReader.Read())
                    {
                        Objeto = new EstadoEnvio();
                        Objeto.TipoDocumento = new TipoDocumento();

                        Objeto.IdEstadoEnvio = DataUtil.DbValueToDefault<int>(objReader[indexIdEstadoEnvio]);
                        Objeto.TipoDocumento.CodigoDocumento = DataUtil.DbValueToDefault<string>(objReader[indexCodigoDocumento]);
                        Objeto.TipoDocumento.IdTipoDocumento = DataUtil.DbValueToDefault<int>(objReader[indexIdTipoDocumento]);
                        oListaObjeto.Add(Objeto);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListEstadoEnvio ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaObjeto;
        }



        public ListaEstadoPrint GetListEstadoPrint(int IdEstadoPrint, string RucEmpresa)
        {
            EstadoPrint Objeto = new EstadoPrint();
            ListaEstadoPrint oListaObjeto = new ListaEstadoPrint();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetstatusPrint_TPO_CE;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdEstadoDocumento", IdEstadoPrint);
                cmd.Parameters.AddWithValue("@RucEntity", RucEmpresa);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdEstadoEnvio = objReader.GetOrdinal("IdEstadoEnvio");
                    int indexCodigoDocumento = objReader.GetOrdinal("CodigoDocumento");
                    int indexIdTipoDocumento = objReader.GetOrdinal("IdTipoDocumento");

                    while (objReader.Read())
                    {
                        Objeto = new EstadoPrint();
                        Objeto.TipoDocumento = new TipoDocumento();

                        Objeto.IdEstadoPrint = DataUtil.DbValueToDefault<int>(objReader[indexIdEstadoEnvio]);
                        Objeto.TipoDocumento.CodigoDocumento = DataUtil.DbValueToDefault<string>(objReader[indexCodigoDocumento]);
                        Objeto.TipoDocumento.IdTipoDocumento = DataUtil.DbValueToDefault<int>(objReader[indexIdTipoDocumento]);
                        oListaObjeto.Add(Objeto);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListEstadoPrint ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaObjeto;
        }

        #endregion






        #region exchangerate

        public ListExchangeRate Get_ExchangeRate_Today(DateTime fecha)
        {
            ExchangeRate Objeto = new ExchangeRate();
            ListExchangeRate oListaObjeto = new ListExchangeRate();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetExchangeRate_TD;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Fecha", fecha.ToString("yyyy-MM-dd"));
                //cmd.Parameters.AddWithValue("@RucEntity", RucEmpresa);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexFecha = objReader.GetOrdinal("Fecha");
                    int indexMoneda = objReader.GetOrdinal("Moneda");
                    int indexValue = objReader.GetOrdinal("Value");

                    while (objReader.Read())
                    {
                        Objeto = new ExchangeRate();

                        Objeto.Fecha = DataUtil.DbValueToDefault<DateTime>(objReader[indexFecha]);
                        Objeto.Moneda = DataUtil.DbValueToDefault<string>(objReader[indexMoneda]);
                        Objeto.Value = DataUtil.DbValueToDefault<decimal>(objReader[indexValue]);
                        oListaObjeto.Add(Objeto);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: Get_ExchangeRate_Today ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaObjeto;
        }

        #endregion





        #region Exchange Rate
        
        public string Insert_ExchangeRate_Value(ExchangeRate objtype)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_Ins_ExchangeRate_Today;
                cmd.Connection = cnn;

                {
                    cmd.Parameters.AddWithValue("@fech", objtype.fech);
                    cmd.Parameters.AddWithValue("@fecf_str", objtype.fech_str);
                    cmd.Parameters.AddWithValue("@cambio", objtype.value);

                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Registrar" : "Registrado Correctamente";
                    cnn.Close();
                }
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: Insert_ExchangeRate_Value ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }

        #endregion


        #region list pendings document

        public ListaFacturaElectronica GetList_PendingsDocuments_RA(string ruccomp, string type)
        {
            FacturaElectronica Objeto = new FacturaElectronica();
            ListaFacturaElectronica oListaObjeto = new ListaFacturaElectronica();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetList_PendingDocuments_RA;
                cmd.Connection = cnn;

                //cmd.Parameters.AddWithValue("@Fecha", fecha.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ruccomp", ruccomp);
                cmd.Parameters.AddWithValue("@type", type);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexFecha = objReader.GetOrdinal("FechaEmision");
                    int indexEstado = objReader.GetOrdinal("Estado");
                    int indexCantidad = objReader.GetOrdinal("Cantidad");

                    while (objReader.Read())
                    {
                        Objeto = new FacturaElectronica();
                        Objeto.Estado = new Estado();

                        Objeto.FechaEmision = DataUtil.DbValueToDefault<DateTime>(objReader[indexFecha]);
                        Objeto.FechaEmision2 = (DataUtil.DbValueToDefault<DateTime>(objReader[indexFecha])).ToString("yyyy/MM/dd");

                        Objeto.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexEstado]);
                        Objeto.Cantidad = DataUtil.DbValueToDefault<int>(objReader[indexCantidad]);
                        oListaObjeto.Add(Objeto);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetList_PendingsDocuments_RA ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaObjeto;
        }


        public ListaFacturaElectronica GetList_PendingsDocuments_RC(string ruccomp)
        {
            FacturaElectronica Objeto = new FacturaElectronica();
            ListaFacturaElectronica oListaObjeto = new ListaFacturaElectronica();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetList_PendingDocuments_RC;
                cmd.Connection = cnn;

                //cmd.Parameters.AddWithValue("@Fecha", fecha.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ruccomp", ruccomp);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexFecha = objReader.GetOrdinal("FechaEmision");
                    int indexEstado = objReader.GetOrdinal("Estado");
                    int indexCantidad = objReader.GetOrdinal("Cantidad");

                    while (objReader.Read())
                    {
                        Objeto = new FacturaElectronica();
                        Objeto.Estado = new Estado();

                        Objeto.FechaEmision = DataUtil.DbValueToDefault<DateTime>(objReader[indexFecha]);
                        Objeto.FechaEmision2 = (DataUtil.DbValueToDefault<DateTime>(objReader[indexFecha])).ToString("yyyy/MM/dd");

                        Objeto.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexEstado]);
                        Objeto.Cantidad = DataUtil.DbValueToDefault<int>(objReader[indexCantidad]);
                        oListaObjeto.Add(Objeto);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetList_PendingsDocuments_RC ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaObjeto;
        }

        public ListaFacturaElectronica GetList_PendingsDocuments_ErrorSend(string ruccomp)
        {
            FacturaElectronica Objeto = new FacturaElectronica();
            ListaFacturaElectronica oListaObjeto = new ListaFacturaElectronica();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetExchangeRate_TD;
                cmd.Connection = cnn;

                //cmd.Parameters.AddWithValue("@Fecha", fecha.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ruccomp", ruccomp);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexFecha = objReader.GetOrdinal("FechaEmision");
                    int indexEstado = objReader.GetOrdinal("Estado");
                    int indexCantidad = objReader.GetOrdinal("Cantidad");

                    while (objReader.Read())
                    {
                        Objeto = new FacturaElectronica();
                        Objeto.Estado = new Estado();

                        Objeto.FechaEmision = DataUtil.DbValueToDefault<DateTime>(objReader[indexFecha]);
                        Objeto.FechaEmision2 = (DataUtil.DbValueToDefault<DateTime>(objReader[indexFecha])).ToString("yyyy/MM/dd");
                        Objeto.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexEstado]);
                        Objeto.Cantidad = DataUtil.DbValueToDefault<int>(objReader[indexCantidad]);
                        oListaObjeto.Add(Objeto);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetList_PendingsDocuments_ErrorSend ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaObjeto;
        }

        #endregion
    }
}

