using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.BusinessEntities.Configuracion;
using Slin.Facturacion.BusinessEntities.Helper;
using Slin.Facturacion.Common.Method;
using Slin.Facturacion.DataAccess.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.DataAccess
{
    public class ConfiguracionDataAccess
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

        public ListaURLAmbiente GetListURLAmbienteSunat()
        {
            URLAmbiente objAmb = new URLAmbiente();
            ListaURLAmbiente objListAmb = new ListaURLAmbiente();

            try
            {
                SqlCommand oDbCommand = new SqlCommand();

                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_GetListUrlAmbiente;
                oDbCommand.Connection = cnn;
                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexIdUrl = objReader.GetOrdinal("ID_URL");
                    int indexCodUrl = objReader.GetOrdinal("COD_URL");
                    int indexDescripcion = objReader.GetOrdinal("DESCR_URL");
                    int indexURL = objReader.GetOrdinal("URL");
                    int indexIdEstado = objReader.GetOrdinal("IDESTADO");
                    int indexIdAmbiente = objReader.GetOrdinal("IDAMBIENTE");
                    while (objReader.Read())
                    {
                        objAmb = new URLAmbiente();
                        objAmb.Estado = new Estado();
                        objAmb.AmbienteSunat = new AmbienteSunat();

                        objAmb.IdUrl = DataUtil.DbValueToDefault<int>(objReader[indexIdUrl]);
                        objAmb.Codigo = DataUtil.DbValueToDefault<string>(objReader[indexCodUrl]);
                        objAmb.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);
                        objAmb.URL = DataUtil.DbValueToDefault<string>(objReader[indexURL]);
                        objAmb.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        objAmb.AmbienteSunat.IdAmbiente = DataUtil.DbValueToDefault<int>(objReader[indexIdAmbiente]);
                        objListAmb.Add(objAmb);
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
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: GetListURLAmbienteSunat ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return objListAmb;
        }


        public ListaDocumentoAmbiente GetListaDocAmbiente(int ID, string RucEntity)
        {
            DocumentoAmbiente objDocAmb = new DocumentoAmbiente();
            ListaDocumentoAmbiente objListAmb = new ListaDocumentoAmbiente();

            try
            {
                SqlCommand oDbCommand = new SqlCommand();

                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_GetListDocAmbiente;
                oDbCommand.Connection = cnn;

                oDbCommand.Parameters.AddWithValue("@IDAmbiente", ID);
                oDbCommand.Parameters.AddWithValue("@RucEntity", RucEntity);

                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexID = objReader.GetOrdinal("ID");
                    int indexID_CE = objReader.GetOrdinal("ID_TIPO_CE");
                    int indexTIPOCE = objReader.GetOrdinal("TIPO_CE");
                    int indexDESCR_CE = objReader.GetOrdinal("DESCR_TPO_CE");
                    int indexIdAmbiente = objReader.GetOrdinal("IDAMBIENTE");
                    int indexIdEstado = objReader.GetOrdinal("IDESTADO");
                    int indexDescrEstado = objReader.GetOrdinal("DESCRIPCION");

                    while (objReader.Read())
                    {
                        objDocAmb = new DocumentoAmbiente();
                        objDocAmb.Estado = new Estado();
                        objDocAmb.AmbienteSunat = new AmbienteSunat();

                        objDocAmb.ID = DataUtil.DbValueToDefault<int>(objReader[indexID]);
                        objDocAmb.ID_TPO_CE = DataUtil.DbValueToDefault<int>(objReader[indexID_CE]);
                        objDocAmb.TIPO_CE = DataUtil.DbValueToDefault<string>(objReader[indexTIPOCE]);
                        objDocAmb.DESCRIPCION_TPO_CE = DataUtil.DbValueToDefault<string>(objReader[indexDESCR_CE]);
                        objDocAmb.AmbienteSunat.IdAmbiente = DataUtil.DbValueToDefault<int>(objReader[indexIdAmbiente]);
                        objDocAmb.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        objDocAmb.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescrEstado]);
                        objListAmb.Add(objDocAmb);
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
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: GetListaDocAmbiente ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return objListAmb;
        }

        
        public String InsertDocumentAmbiente(ListaDocumentoAmbiente objListDocAmb)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertDocumentAmbient;
                cmd.Connection = cnn;

                for (int i = 0; i <= objListDocAmb.Count - 1; i++)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IDAMBIENTE", objListDocAmb[i].AmbienteSunat.IdAmbiente);
                    cmd.Parameters.AddWithValue("@TIPO_CE", objListDocAmb[i].TIPO_CE);
                    cmd.Parameters.AddWithValue("@IDESTADO", objListDocAmb[i].Estado.IdEstado);
                    cmd.Parameters.AddWithValue("@RUCENTITY", objListDocAmb[i].Empresa.RUC);

                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Actualizado" : "Actualizado Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: InsertDocumentAmbiente ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }

        public String UpdateDocAmbienteEstado(ListaDocumentoAmbiente objListDocAmb)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_UpdateDocAmb;
                cmd.Connection = cnn;

                for (int i = 0; i <= objListDocAmb.Count - 1; i++)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IDAMBIENTE", objListDocAmb[i].AmbienteSunat.IdAmbiente);
                    cmd.Parameters.AddWithValue("@TIPO_CE", objListDocAmb[i].TIPO_CE);
                    cmd.Parameters.AddWithValue("@IDESTADO", objListDocAmb[i].Estado.IdEstado);
                    cmd.Parameters.AddWithValue("@RUCENTITY", objListDocAmb[i].Empresa.RUC);

                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Actualizar" : "Actualizado Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: UpdateDocAmbienteEstado ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }

        public String UpdateURLAmbiente(ListaURLAmbiente objListUrlAmb)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_UpdateURLAmb;
                cmd.Connection = cnn;

                for (int i = 0; i <= objListUrlAmb.Count - 1; i++)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID_URL", objListUrlAmb[i].IdUrl);
                    cmd.Parameters.AddWithValue("@URL", objListUrlAmb[i].URL);
                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Actualizar" : "Actualizado Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: UpdateURLAmbiente ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }












        public List<string> Delete_DocAmb(ListaDocumentoAmbiente objListDocAmb)
        {
            List<string> listmsj = new List<string>();

            try
            {
                string msj = string.Empty;
                listmsj = new List<string>();
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_UpdateURLAmb;
                cmd.Connection = cnn;

                for (int i = 0; i <= objListDocAmb.Count - 1; i++)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IDAMBIENTE", objListDocAmb[i].AmbienteSunat.IdAmbiente);
                    cmd.Parameters.AddWithValue("@TIPO_CE", objListDocAmb[i].TIPO_CE);
                    cmd.Parameters.AddWithValue("@RUCENTITY", objListDocAmb[i].Empresa.RUC);
                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Eliminar" : "Eliminado Correctamente";

                    listmsj.Add(msj);
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                listmsj.Add(ex.Message);

                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: Delete_DocAmb ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return listmsj;
        }


        public String InsertDepartament(ExcelRead obj)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertarDepartamento;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@CodDepart", obj.CodigoDepartamento);
                cmd.Parameters.AddWithValue("@Descripcion", obj.DescripcionDepartamento);

                int IdRet = cmd.ExecuteNonQuery();

                msj = IdRet > 0 ? "Registrado Correctamente" : "Error al Registrar";
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: InsertDepartament ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }

        public String InsertarProvinciaxDepartament(ExcelRead obj)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertarProvinciaxDepartamento;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@CodProvincia", obj.CodigoProvincia);
                cmd.Parameters.AddWithValue("@Descripcion", obj.DescripcionProvincia);
                cmd.Parameters.AddWithValue("@CodDepartamament", obj.CodigoDepartamento);

                int IdRet = cmd.ExecuteNonQuery();

                msj = IdRet > 0 ? "Registrado Correctamente" : "Error al Registrar";
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: InsertarProvinciaxDepartament ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }

        public String InsertarDistritoxProvincia(ExcelRead obj)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertarDistritoxProvincia;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@CodDistrito", obj.CodigoDistrito);
                cmd.Parameters.AddWithValue("@Descripcion", obj.DescripcionDistrito);
                cmd.Parameters.AddWithValue("@CodProvincia", obj.CodigoProvincia);
                cmd.Parameters.AddWithValue("@CodDepartamento", obj.CodigoDepartamento);

                int IdRet = cmd.ExecuteNonQuery();

                msj = IdRet > 0 ? "Registrado Correctamente" : "Error al Registrar";
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: InsertarDistritoxProvincia ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }

        public String UpdateDistritoxProvincia(ExcelRead obj)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_UpdateDistritoForProvincia;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@CODUBIGEO", obj.CodigoDepartamento + obj.CodigoProvincia + obj.CodigoDistrito);

                int IdRet = cmd.ExecuteNonQuery();

                msj = IdRet > 0 ? "Actualizado Correctamente" : "Error al Actualizar";
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: UpdateDistritoxProvincia ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }


        #region CONFIG TIME SERVICE AND EXCHANGE RATE

        public TipoCambio GetexchangeRateToday(string Today ,string RucEntity)
        {
            TipoCambio objTpoCambio = new TipoCambio();

            try
            {
                SqlCommand oDbCommand = new SqlCommand();

                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_GetExchangeRateToday;
                oDbCommand.Connection = cnn;

                oDbCommand.Parameters.AddWithValue("@Today", Today);
                oDbCommand.Parameters.AddWithValue("@RucEntity", RucEntity);

                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexId = objReader.GetOrdinal("Id");
                    int indexIdCoin = objReader.GetOrdinal("IdCoin");
                    int indexSimbolo = objReader.GetOrdinal("Simbolo");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");
                    int indexPurchaseValue = objReader.GetOrdinal("PurchaseValue");
                    int indexSaleValue = objReader.GetOrdinal("SaleValue");
                    int indexDateValue = objReader.GetOrdinal("DateValue");
                    int indexRucEntity = objReader.GetOrdinal("RucEntity");

                    while (objReader.Read())
                    {
                        objTpoCambio = new TipoCambio();
                        objTpoCambio.Empresa = new Empresa();
                        objTpoCambio.Moneda = new Moneda();

                        objTpoCambio.IdTipoCambio = DataUtil.DbValueToDefault<int>(objReader[indexId]);
                        objTpoCambio.Moneda.IdMoneda = DataUtil.DbValueToDefault<int>(objReader[indexIdCoin]);
                        objTpoCambio.Moneda.Simbolo = DataUtil.DbValueToDefault<string>(objReader[indexSimbolo]);
                        objTpoCambio.Moneda.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);
                        objTpoCambio.PurchaseValue = DataUtil.DbValueToDefault<decimal>(objReader[indexPurchaseValue]);
                        objTpoCambio.SaleValue = DataUtil.DbValueToDefault<decimal>(objReader[indexSaleValue]);
                        objTpoCambio.DateValue = DataUtil.DbValueToDefault<string>(objReader[indexDateValue]);
                        objTpoCambio.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRucEntity]);
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
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: GetexchangeRateToday ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return objTpoCambio;
        }

        public ListaMoneda GetListMoneda()
        {
            Moneda objCoin = new Moneda();
            ListaMoneda objListCoin = new ListaMoneda();
            try
            {
                SqlCommand oDbCommand = new SqlCommand();

                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_GetListCoin;
                oDbCommand.Connection = cnn;

                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexIdCoin = objReader.GetOrdinal("IdMoneda");
                    int indexSimbolo = objReader.GetOrdinal("Simbolo");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");

                    while (objReader.Read())
                    {
                        objCoin = new Moneda();

                        objCoin.IdMoneda = DataUtil.DbValueToDefault<int>(objReader[indexIdCoin]);
                        objCoin.Simbolo = DataUtil.DbValueToDefault<string>(objReader[indexSimbolo]);
                        objCoin.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);
                        objListCoin.Add(objCoin);
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
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: GetListMoneda ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return objListCoin;
        }


        public ListService GetListTimeService(string RucEntity)
        {
            Services objService = new Services();
            ListService objListService = new ListService();

            try
            {
                SqlCommand oDbCommand = new SqlCommand();

                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_ListTimeService;
                oDbCommand.Connection = cnn;

                oDbCommand.Parameters.AddWithValue("@RucEntity", RucEntity);

                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexId = objReader.GetOrdinal("Id");
                    int indexCodeService = objReader.GetOrdinal("CodeService");
                    int indexNameService = objReader.GetOrdinal("NameService");
                    int indexValueTime = objReader.GetOrdinal("ValueTime");
                    int indexIntervalValue = objReader.GetOrdinal("IntervalValue");
                    int indexMaxNumAttempts = objReader.GetOrdinal("MaxNumAttempts");
                    int indexRucEntity = objReader.GetOrdinal("RucEntity");
                    int indexIdEstado = objReader.GetOrdinal("IdEstado");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");
                    int indexServiceStatus = objReader.GetOrdinal("ServiceStatus");

                    while (objReader.Read())
                    {
                        objService = new Services();
                        objService.Empresa = new Empresa();
                        objService.Estado = new Estado();

                        objService.IdService = DataUtil.DbValueToDefault<int>(objReader[indexId]);
                        objService.CodeService = DataUtil.DbValueToDefault<string>(objReader[indexCodeService]);
                        objService.NombreService = DataUtil.DbValueToDefault<string>(objReader[indexNameService]);
                        objService.ValueTime = DataUtil.DbValueToDefault<string>(objReader[indexValueTime]);
                        objService.IntervaleValue = DataUtil.DbValueToDefault<string>(objReader[indexIntervalValue]);
                        objService.MaxNumAttempts = DataUtil.DbValueToDefault<int>(objReader[indexMaxNumAttempts]);
                        objService.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRucEntity]);
                        objService.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        objService.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);


                        ServiceController service = new ServiceController(objService.CodeService);

                        if (service != null)
                        {
                            try
                            {
                                if (service.ServiceName == objService.CodeService)
                                {
                                    //var status = service.Status.ToString()
                                    objService.ServiceStatus = service.Status.ToString();
                                }
                                else
                                {
                                    objService.ServiceStatus = "Not installed";
                                }
                            }
                            catch (Exception ex)
                            {
                                objService.ServiceStatus = "Not installed";
                            }
                        }
                        else
                        {
                            objService.ServiceStatus = "Not installed";
                        }
                        
                        objListService.Add(objService);
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
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: GetListTimeService ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return objListService;
        }

        public String UpdateTimeService(Services oService)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_UpdateTimeService;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Id", oService.IdService);
                cmd.Parameters.AddWithValue("@NameService", oService.NombreService);
                cmd.Parameters.AddWithValue("@ValueTime", oService.ValueTime);
                cmd.Parameters.AddWithValue("@IntervalValue", oService.IntervaleValue);
                cmd.Parameters.AddWithValue("@MaxNumAttempts", oService.MaxNumAttempts);
                cmd.Parameters.AddWithValue("@RucEntity", oService.Empresa.RUC);
                cmd.Parameters.AddWithValue("@IdEstado", oService.Estado.IdEstado);

                int IdRet = cmd.ExecuteNonQuery();

                msj = IdRet > 0 ? "Actualizado Correctamente" : "Error al Actualizar";
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: UpdateTimeService ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }



        public String InsertExchangeRateToday(TipoCambio objRate)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertExchangeRateToday;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Idcoin", objRate.Moneda.IdMoneda);
                cmd.Parameters.AddWithValue("@PurchaseValue", objRate.PurchaseValue);
                cmd.Parameters.AddWithValue("@SaleValue", objRate.SaleValue);
                cmd.Parameters.AddWithValue("@DateValue", objRate.DateValue);
                cmd.Parameters.AddWithValue("@RucEntity", objRate.Empresa.RUC);

                int IdRet = cmd.ExecuteNonQuery();

                msj = IdRet > 0 ? "Registrado Correctamente" : "Error al Registrar";
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: InsertExchangeRateToday ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }


        public String UpdateExchangeRateToday(TipoCambio objRate)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_UpdateExchangeRateToday;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Id", objRate.IdTipoCambio);
                cmd.Parameters.AddWithValue("@IdCoin", objRate.Moneda.IdMoneda);
                cmd.Parameters.AddWithValue("@PurchaseValue", objRate.PurchaseValue);
                cmd.Parameters.AddWithValue("@SaleValue", objRate.SaleValue);
                cmd.Parameters.AddWithValue("@RucEntity", objRate.Empresa.RUC);

                int IdRet = cmd.ExecuteNonQuery();

                msj = IdRet > 0 ? "Actualizado Correctamente" : "Error al Actualizar";
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: UpdateExchangeRateToday ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }

        public DataTable GetListTypeDocumentRowSend(int id)
        {
            DataTable dt = new DataTable();
            string sql = Procedimientos.Usp_GetListTypeDocumentSendEdit;
            using (SqlConnection conn = new SqlConnection(cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
            //if (dt.Rows.Count > 0)
            //    return dt.Rows[0];
            //else
            //    return null;
        }


        #endregion END CONFIG TIME SERVICE AND EXCHANGE RATE


        #region NEW METHOD FOR CONFIG SEND AND PRINT

        public String InsertTypeDocument_ForSend(TipoDocumento otypedoc)
        {
            string msj = string.Empty;
            int IdRet = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertTypeDoc_ForSend;
                cmd.Connection = cnn;

                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@TypeDoc", otypedoc.CodigoDocumento);
                cmd.Parameters.AddWithValue("@IdEstado", otypedoc.Estado.IdEstado);
                cmd.Parameters.AddWithValue("@RucEntity", otypedoc.Empresa.RUC);

                IdRet = cmd.ExecuteNonQuery();
                msj = IdRet > 0 ? "Actualizado Correctamente" : "Error al Registrar";
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
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: GetListTypeDocumentRowSend ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }


        public String InsertTypeDocument_ForPrint(TipoDocumento otypedoc)
        {
            string msj = string.Empty;
            int IdRet = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertTypeDoc_ForPrint;
                cmd.Connection = cnn;

                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@TypeDoc", otypedoc.CodigoDocumento);
                cmd.Parameters.AddWithValue("@IdEstado", otypedoc.Estado.IdEstado);
                cmd.Parameters.AddWithValue("@RucEntity", otypedoc.Empresa.RUC);

                IdRet = cmd.ExecuteNonQuery();
                msj = IdRet > 0 ? "Actualizado Correctamente" : "Error al Registrar";
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
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: InsertTypeDocument_ForPrint ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;

        }

        public ListaTipoDocumento ListTypeDocument_TypeSend(string RucEntity)
        {
            ListaTipoDocumento oListTypeDocument = new ListaTipoDocumento();
            TipoDocumento oTypeDocument = new TipoDocumento();

            try
            {
                SqlCommand oDbCommand = new SqlCommand();
                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_ListTypeDocument_TypeSend;
                oDbCommand.Connection = cnn;

                oDbCommand.Parameters.AddWithValue("@RucEntity", RucEntity);

                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexIdTipoDocumento = objReader.GetOrdinal("IdTipoDocumento");
                    int indexCodigoDocumento = objReader.GetOrdinal("CodigoDocumento");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");
                    int indexPadre = objReader.GetOrdinal("Padre");
                    int indexIdEstado = objReader.GetOrdinal("IdEstado");
                    //int indexDescripcionEstado = objReader.GetOrdinal("DescripcionEstado");
                    while (objReader.Read())
                    {
                        oTypeDocument = new TipoDocumento();
                        oTypeDocument.Estado = new Estado();

                        oTypeDocument.IdTipoDocumento = DataUtil.DbValueToDefault<int>(objReader[indexIdTipoDocumento]);
                        oTypeDocument.CodigoDocumento = DataUtil.DbValueToDefault<string>(objReader[indexCodigoDocumento]);
                        oTypeDocument.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);
                        oTypeDocument.Padre = DataUtil.DbValueToDefault<int>(objReader[indexPadre]);

                        oTypeDocument.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        //oTipoDocumento.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcionEstado]);

                        oListTypeDocument.Add(oTypeDocument);
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
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: ListTypeDocument_TypeSend ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListTypeDocument;
        }




        public ListaTipoDocumento ListTypeDocument_TypePrint(string RucEntity)
        {
            ListaTipoDocumento oListTypeDocument = new ListaTipoDocumento();
            TipoDocumento oTypeDocument = new TipoDocumento();

            try
            {
                SqlCommand oDbCommand = new SqlCommand();
                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_ListTypeDocument_TypePrint;
                oDbCommand.Connection = cnn;

                oDbCommand.Parameters.AddWithValue("@RucEntity", RucEntity);

                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexIdTipoDocumento = objReader.GetOrdinal("IdTipoDocumento");
                    int indexCodigoDocumento = objReader.GetOrdinal("CodigoDocumento");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");
                    int indexPadre = objReader.GetOrdinal("Padre");
                    int indexIdEstado = objReader.GetOrdinal("IdEstado");
                    //int indexDescripcionEstado = objReader.GetOrdinal("DescripcionEstado");
                    while (objReader.Read())
                    {
                        oTypeDocument = new TipoDocumento();
                        oTypeDocument.Estado = new Estado();

                        oTypeDocument.IdTipoDocumento = DataUtil.DbValueToDefault<int>(objReader[indexIdTipoDocumento]);
                        oTypeDocument.CodigoDocumento = DataUtil.DbValueToDefault<string>(objReader[indexCodigoDocumento]);
                        oTypeDocument.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);
                        oTypeDocument.Padre = DataUtil.DbValueToDefault<int>(objReader[indexPadre]);

                        oTypeDocument.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        //oTipoDocumento.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcionEstado]);

                        oListTypeDocument.Add(oTypeDocument);
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
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: ListTypeDocument_TypePrint ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListTypeDocument;
        }

        #endregion END NEW METHOD FOR CONFIG SEND AND PRINT



        #region SERVICE

        public ListService GetTimeServicceForExeProccessMD(string nameService)
        {
            Services objService = new Services();
            ListService objList = new ListService();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetTimeForExeProccessMD;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@CodeService", nameService);
                //cmd.Parameters.AddWithValue("@RucEntity", RucEntity);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexId = objReader.GetOrdinal("Id");
                    int indexCod = objReader.GetOrdinal("CodeService");
                    int indexName = objReader.GetOrdinal("NameService");
                    int indexValueTime = objReader.GetOrdinal("ValueTime");
                    int indexInterval = objReader.GetOrdinal("IntervalValue");
                    int indexMaxattempts = objReader.GetOrdinal("MaxNumAttempts");
                    int indexRucEntity = objReader.GetOrdinal("RucEntity");
                    int indexIdEstado = objReader.GetOrdinal("IdEstado");

                    while (objReader.Read())
                    {
                        objService = new Services();
                        objService.Empresa = new Empresa();
                        objService.Estado = new Estado();

                        objService.IdService = DataUtil.DbValueToDefault<int>(objReader[indexId]);
                        objService.CodeService = DataUtil.DbValueToDefault<string>(objReader[indexCod]);
                        objService.NombreService = DataUtil.DbValueToDefault<string>(objReader[indexName]);
                        objService.ValueTime = DataUtil.DbValueToDefault<string>(objReader[indexValueTime]);
                        objService.IntervaleValue = DataUtil.DbValueToDefault<string>(objReader[indexInterval]);
                        objService.MaxNumAttempts = DataUtil.DbValueToDefault<int>(objReader[indexMaxattempts]);
                        objService.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRucEntity]);
                        objService.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        objList.Add(objService);
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
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: GetTimeServicceForExeProccessMD ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return objList;
        }

        #endregion


        #region SECONDARY USER SUNAT

        public ListSecondaryUser Get_SecondaryUserSunat(int IdAmb, string RucEntity)
        {
            SecondaryUser objSU = new SecondaryUser();
            ListSecondaryUser objListSeconUser = new ListSecondaryUser();

            try
            {
                SqlCommand oDbCommand = new SqlCommand();

                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_Get_us_pwd_amb;
                oDbCommand.Connection = cnn;

                oDbCommand.Parameters.AddWithValue("@IdAmb", IdAmb);
                oDbCommand.Parameters.AddWithValue("@RucEntity", RucEntity);

                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexRuc = objReader.GetOrdinal("RucEntity");
                    int indexUserName = objReader.GetOrdinal("NombreUsuario");
                    int indexPwd = objReader.GetOrdinal("Password");
                    int indexIdAmb = objReader.GetOrdinal("IDAMBIENTE");

                    while (objReader.Read())
                    {
                        objSU = new SecondaryUser();

                        objSU.RucEntity = DataUtil.DbValueToDefault<string>(objReader[indexRuc]);
                        objSU.UserName = DataUtil.DbValueToDefault<string>(objReader[indexUserName]);
                        objSU.Password = DataUtil.DbValueToDefault<string>(objReader[indexPwd]);
                        objSU.IdAmb = DataUtil.DbValueToDefault<int>(objReader[indexIdAmb]);
                        objListSeconUser.Add(objSU);
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
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: Get_SecondaryUserSunat ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return objListSeconUser;
        }

        public string Insert_SecondaryUserSunat_Amb(SecondaryUser objSeconUser)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_Insert_SecondaryUserSunat;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@RucEntity", objSeconUser.RucEntity);
                cmd.Parameters.AddWithValue("@UserName", objSeconUser.UserName);
                cmd.Parameters.AddWithValue("@Password", objSeconUser.Password);
                cmd.Parameters.AddWithValue("@IdAmb", objSeconUser.IdAmb);
                {
                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Actualizar" : "Actualizado Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: Insert_SecondaryUserSunat_Amb ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
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


        #region CD

        
        public ListCertificateDigital Get_CertificateDigitalInformation(string RucEntity)
        {
            CertificateDigital obj = new CertificateDigital();
            ListCertificateDigital objList = new ListCertificateDigital();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetCertificateData;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@RucEntity", RucEntity);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexId = objReader.GetOrdinal("Id");
                    int indexNameCertificate = objReader.GetOrdinal("NameCertificate");
                    int indexPwd = objReader.GetOrdinal("Pwd");
                    int indexExpirationDate = objReader.GetOrdinal("ExpirationDate");
                    int indexRucEntity = objReader.GetOrdinal("RucEntity");

                    while (objReader.Read())
                    {
                        obj = new CertificateDigital();

                        obj.Id = DataUtil.DbValueToDefault<int>(objReader[indexId]);
                        obj.NameCertificate = DataUtil.DbValueToDefault<string>(objReader[indexNameCertificate]);
                        obj.Pwd = DataUtil.DbValueToDefault<string>(objReader[indexPwd]);
                        obj.ExpirationDate = DataUtil.DbValueToDefault<string>(objReader[indexExpirationDate]);
                        obj.RucEntity = DataUtil.DbValueToDefault<string>(objReader[indexRucEntity]);
                        objList.Add(obj);
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
                listError.Add("[" + DateTime.Now + "] [ CONFIGURACIÓN - MethodName: Get_CertificateDigitalInformation ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Conf_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return objList;
        }

        #endregion
    }
}

