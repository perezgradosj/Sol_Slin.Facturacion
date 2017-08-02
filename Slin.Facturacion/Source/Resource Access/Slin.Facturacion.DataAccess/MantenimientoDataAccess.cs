using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

using Slin.Facturacion.DataAccess.Helper;
using Slin.Facturacion.BusinessEntities;
using System.IO;
using Slin.Facturacion.Common.Method;

namespace Slin.Facturacion.DataAccess
{
    public class MantenimientoDataAccess
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

        #region LISTAS

        public ListaSexo GetListaSexo()
        {
            logError = new StringBuilder();
            Sexo oSexo = new Sexo();
            ListaSexo oListaSexo = new ListaSexo();

            try
            {
                oListaSexo.Insert(0, new Sexo() { IdSexo = 1, Descripcion = "Masculino" });
                oListaSexo.Insert(0, new Sexo() { IdSexo = 2, Descripcion = "Femenino" });
                oListaSexo.Insert(0, new Sexo() { IdSexo = 0, Descripcion = "- Seleccione -" });
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: GetListaSexo ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaSexo;
        }

        public ListaEstado GetListaEstado()
        {
            Estado oEstado = new Estado();
            ListaEstado oListaEstado = new ListaEstado();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaEstado;
                cmd.Connection = cnn;

                using (IDataReader objReader = cmd.ExecuteReader())
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: GetListaEstado ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaEstado;
        }

        public ListaEmpresa GetListaEmpresa()
        {
            ListaEmpresa oListaEmpresa = new ListaEmpresa();
            Empresa oEmpresa = new Empresa();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaEmpresa;
                cmd.Connection = cnn;

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdEmpresa = objReader.GetOrdinal("IdEmpresa");
                    int indexCodigo = objReader.GetOrdinal("CodEmpresa");
                    int indexRazonSocial = objReader.GetOrdinal("RazonSocial");
                    int indexTpoLogin = objReader.GetOrdinal("TpoLogin");

                    while (objReader.Read())
                    {
                        oEmpresa = new Empresa();
                        oEmpresa.IdEmpresa = DataUtil.DbValueToDefault<int>(objReader[indexIdEmpresa]);
                        oEmpresa.CodEmpresa = DataUtil.DbValueToDefault<string>(objReader[indexCodigo]);
                        oEmpresa.RazonSocial = DataUtil.DbValueToDefault<string>(objReader[indexRazonSocial]);
                        oEmpresa.TipoLogin = DataUtil.DbValueToDefault<string>(objReader[indexTpoLogin]);
                        oListaEmpresa.Add(oEmpresa);
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: GetListaEmpresa ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaEmpresa;
        }

        public ListaTipoDocumentoIdentidad GetListaTipoDocumentoIdentidad()
        {
            TipoDocumentoIdentidad oTipoDocuemntoIdentidad = new TipoDocumentoIdentidad();
            ListaTipoDocumentoIdentidad oListaTipoDocumentoIdentidad = new ListaTipoDocumentoIdentidad();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaTipoDocumentoIdentidad;
                cmd.Connection = cnn;

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdTipoDoc = objReader.GetOrdinal("IdTipoDocumento");
                    int indexCodigo = objReader.GetOrdinal("Codigo");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");

                    while (objReader.Read())
                    {
                        oTipoDocuemntoIdentidad = new TipoDocumentoIdentidad();
                        oTipoDocuemntoIdentidad.IdTipoDocumentoIdentidad = DataUtil.DbValueToDefault<int>(objReader[indexIdTipoDoc]);
                        oTipoDocuemntoIdentidad.Codigo = DataUtil.DbValueToDefault<string>(objReader[indexCodigo]);
                        oTipoDocuemntoIdentidad.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);
                        oListaTipoDocumentoIdentidad.Add(oTipoDocuemntoIdentidad);
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: GetListaTipoDocumentoIdentidad ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaTipoDocumentoIdentidad;
        }

        public ListaPais GetListaPais()
        {
            Pais oPais = new Pais();
            ListaPais oListaPais = new ListaPais();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaPais;
                cmd.Connection = cnn;

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdPais = objReader.GetOrdinal("Id");
                    int indexCodigo = objReader.GetOrdinal("Codigo");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");

                    while (objReader.Read())
                    {
                        oPais = new Pais();

                        oPais.IdPais = DataUtil.DbValueToDefault<int>(objReader[indexIdPais]);
                        oPais.Codigo = DataUtil.DbValueToDefault<string>(objReader[indexCodigo]);
                        oPais.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);
                        oListaPais.Add(oPais);
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: GetListaPais ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaPais;
        }


        public ListaDepartamento GetListaDepartamento(Int32 IdPais)
        {
            Departamento oDepartamento = new Departamento();
            ListaDepartamento oListaDepartamento = new ListaDepartamento();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaDepartamento;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdPais", IdPais);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdDepartamento = objReader.GetOrdinal("Id");
                    int indexCodigo = objReader.GetOrdinal("Codigo");
                    int indexIdDescripcion = objReader.GetOrdinal("Descripcion");

                    while (objReader.Read())
                    {
                        oDepartamento = new Departamento();
                        oDepartamento.IdDepartamento = DataUtil.DbValueToDefault<int>(objReader[indexIdDepartamento]);
                        oDepartamento.Codigo = DataUtil.DbValueToDefault<string>(objReader[indexCodigo]);
                        oDepartamento.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexIdDescripcion]);
                        oListaDepartamento.Add(oDepartamento);
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: GetListaDepartamento ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaDepartamento;
        }


        public ListaProvincia GetListaProvincia(Int32 IdDepartamento)
        {
            Provincia oProvincia = new Provincia();
            ListaProvincia oListaProvincia = new ListaProvincia();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaProvincia;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdDepartamento", IdDepartamento);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdProvincia = objReader.GetOrdinal("Id");
                    int indexCodigo = objReader.GetOrdinal("Codigo");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");

                    while (objReader.Read())
                    {
                        oProvincia = new Provincia();
                        oProvincia.IdProvincia = DataUtil.DbValueToDefault<int>(objReader[indexIdProvincia]);
                        oProvincia.Codigo = DataUtil.DbValueToDefault<string>(objReader[indexCodigo]);
                        oProvincia.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);
                        oListaProvincia.Add(oProvincia);
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: GetListaProvincia ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaProvincia;
        }

        public ListaDistrito GetListaDistrito(Int32 IdProvincia)
        {
            Distrito oDistrito = new Distrito();
            ListaDistrito oListaDistrito = new ListaDistrito();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaDistrito;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdProvincia", IdProvincia);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdDistrito = objReader.GetOrdinal("Id");
                    int indexCodigo = objReader.GetOrdinal("Codigo");
                    int indexCodigoUbigeo = objReader.GetOrdinal("CodigoUbigeo");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");

                    while (objReader.Read())
                    {
                        oDistrito = new Distrito();
                        oDistrito.IdDistrito = DataUtil.DbValueToDefault<int>(objReader[indexIdDistrito]);
                        oDistrito.Codigo = DataUtil.DbValueToDefault<string>(objReader[indexCodigo]);
                        oDistrito.CodigoUbigeo = DataUtil.DbValueToDefault<string>(objReader[indexCodigoUbigeo]);
                        oDistrito.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);
                        oListaDistrito.Add(oDistrito);
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: GetListaDistrito ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaDistrito;
        }

        #endregion



        #region LISTAS DATA

        public ListaEmpleados GetListaEmpleado(Empleado oEmpleado)
        {
            Empleado objEmpleado = new Empleado();
            ListaEmpleados oListaEmpleado = new ListaEmpleados();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaEmpleado;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Codigo", oEmpleado.CodEmpleado);
                cmd.Parameters.AddWithValue("@NombresApellidos", oEmpleado.NombresApellidos);
                cmd.Parameters.AddWithValue("@DNI", oEmpleado.DNI);
                cmd.Parameters.AddWithValue("@Estado", oEmpleado.Estado.IdEstado);
                cmd.Parameters.AddWithValue("@Empresa", oEmpleado.Empresa.IdEmpresa);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("Nro");
                    int indexIdEmpleado = objReader.GetOrdinal("IdEmpleado");
                    int indexCodEmpleado = objReader.GetOrdinal("Codigo");
                    int indexNombres = objReader.GetOrdinal("Nombres");
                    int indexApePater = objReader.GetOrdinal("ApePaterno");
                    int indexApeMater = objReader.GetOrdinal("ApeMaterno");
                    int indexEmpleado = objReader.GetOrdinal("Empleado");
                    int indexDNI = objReader.GetOrdinal("DNI");
                    int indexTelefono = objReader.GetOrdinal("Telefono");
                    int indexCelular = objReader.GetOrdinal("Celular");
                    int indexDireccion = objReader.GetOrdinal("Direccion");
                    int indexEmail = objReader.GetOrdinal("Email");
                    int indexIdSexo = objReader.GetOrdinal("IdSexo");
                    int indexSexo = objReader.GetOrdinal("Sexo");
                    int indexFechaNac = objReader.GetOrdinal("FechaNac");
                    int indexIdEstado = objReader.GetOrdinal("IdEstado");
                    int indexEstado = objReader.GetOrdinal("Estado");
                    int indexIdTipoDocumento = objReader.GetOrdinal("IdTipoDocumento");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");
                    int indexIdEmpresa = objReader.GetOrdinal("IdEmpresa");
                    int indexRazonSocial = objReader.GetOrdinal("RazonSocial");
                    int indexRUC = objReader.GetOrdinal("RUC");
                    while (objReader.Read())
                    {
                        objEmpleado = new Empleado();
                        objEmpleado.Sexo = new Sexo();
                        objEmpleado.Estado = new Estado();
                        objEmpleado.TipoDocumentoIdentidad = new TipoDocumentoIdentidad();
                        objEmpleado.Empresa = new Empresa();

                        objEmpleado.Nro = DataUtil.DbValueToDefault<int>(objReader[indexNro]);
                        objEmpleado.IdEmpleado = DataUtil.DbValueToDefault<int>(objReader[indexIdEmpleado]);
                        objEmpleado.CodEmpleado = DataUtil.DbValueToDefault<string>(objReader[indexCodEmpleado]);
                        objEmpleado.Nombres = DataUtil.DbValueToDefault<string>(objReader[indexNombres]);
                        objEmpleado.ApePaterno = DataUtil.DbValueToDefault<string>(objReader[indexApePater]);
                        objEmpleado.ApeMaterno = DataUtil.DbValueToDefault<string>(objReader[indexApeMater]);
                        //objEmpleado.NombresApellidos = DataUtil.DbValueToDefault<string>(objReader[indexApePater]) + " " + DataUtil.DbValueToDefault<string>(objReader[indexApeMater]) + ", " + DataUtil.DbValueToDefault<string>(objReader[indexNombres]);
                        objEmpleado.NombresApellidos = DataUtil.DbValueToDefault<string>(objReader[indexEmpleado]);
                        objEmpleado.DNI = DataUtil.DbValueToDefault<string>(objReader[indexDNI]);
                        objEmpleado.Telefono = DataUtil.DbValueToDefault<string>(objReader[indexTelefono]);
                        objEmpleado.Celular = DataUtil.DbValueToDefault<string>(objReader[indexCelular]);
                        objEmpleado.Direccion = DataUtil.DbValueToDefault<string>(objReader[indexDireccion]);
                        objEmpleado.Email = DataUtil.DbValueToDefault<string>(objReader[indexEmail]);
                        objEmpleado.Sexo.IdSexo = DataUtil.DbValueToDefault<int>(objReader[indexIdSexo]);
                        objEmpleado.Sexo.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexSexo]);
                        objEmpleado.FechaNacimiento = Convert.ToDateTime(objReader[indexFechaNac]);
                        objEmpleado.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        objEmpleado.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexEstado]);
                        objEmpleado.TipoDocumentoIdentidad.IdTipoDocumentoIdentidad = DataUtil.DbValueToDefault<int>(objReader[indexIdTipoDocumento]);
                        objEmpleado.TipoDocumentoIdentidad.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);
                        objEmpleado.Empresa.IdEmpresa = DataUtil.DbValueToDefault<int>(objReader[indexIdEmpresa]);
                        objEmpleado.Empresa.RazonSocial = DataUtil.DbValueToDefault<string>(objReader[indexRazonSocial]);
                        objEmpleado.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRUC]);
                        oListaEmpleado.Add(objEmpleado);
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: GetListaEmpleado ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaEmpleado;
        }

        public ListaEmpresa GetListadoEmpresa(Empresa oEmpresa)
        {
            ListaEmpresa oListaEmpresa = new ListaEmpresa();
            Empresa objEmpresa = new Empresa();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListadoEmpresa;
                cmd.Connection = cnn;

                //cmd.Parameters.AddWithValue("@CodEmpresa", oEmpresa.CodEmpresa);
                cmd.Parameters.AddWithValue("@Ubigeo", oEmpresa.Ubigeo.IdUbigeo);
                cmd.Parameters.AddWithValue("@RUC", oEmpresa.RUC);
                cmd.Parameters.AddWithValue("@RazonSocial", oEmpresa.RazonSocial);
                cmd.Parameters.AddWithValue("@RazonComercial", oEmpresa.RazonComercial);
                cmd.Parameters.AddWithValue("@Telefono", oEmpresa.Telefono);
                cmd.Parameters.AddWithValue("@Estado", oEmpresa.Estado.IdEstado);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("Nro");
                    int indexIdEmpresa = objReader.GetOrdinal("IdEmpresa");
                    int indexCodigo = objReader.GetOrdinal("CodEmpresa");
                    int indexUbigeo = objReader.GetOrdinal("Ubigeo");
                    int indexDescripcionUbigeo = objReader.GetOrdinal("DescripcionUbigeo");
                    int indexRUC = objReader.GetOrdinal("Ruc");
                    int indexRazonSocial = objReader.GetOrdinal("RazonSocial");
                    int indexRazonComercial = objReader.GetOrdinal("RazonComercial");
                    int indexDireccion = objReader.GetOrdinal("Direccion");
                    int indexDomicilioFiscal = objReader.GetOrdinal("DomicilioFiscal");
                    int indexUrbanizacion = objReader.GetOrdinal("Urbanizacion");
                    int indexTelefono = objReader.GetOrdinal("Telefono");
                    int indexFax = objReader.GetOrdinal("Fax");
                    int indexPaginaWeb = objReader.GetOrdinal("PaginaWeb");
                    int indexEmail = objReader.GetOrdinal("Email");
                    int indexFecha = objReader.GetOrdinal("FechaRegistro");
                    int indexIdEstado = objReader.GetOrdinal("IdEstado");
                    int indexDescripcionEstado = objReader.GetOrdinal("DescripcionEstado");
                    int indexIdTipoDoc = objReader.GetOrdinal("IdTipoDocumento");
                    int indexDescripcionTipoDoc = objReader.GetOrdinal("DescripcionTipoDocumento");

                    int indexUrl_CompanyLogo = objReader.GetOrdinal("Url_CompanyLogo");
                    int indexUrl_CompanyConsult = objReader.GetOrdinal("Url_CompanyConsult");

                    while (objReader.Read())
                    {
                        objEmpresa = new Empresa();
                        objEmpresa.Estado = new Estado();
                        objEmpresa.Ubigeo = new Ubigeo();
                        objEmpresa.TipoDocumentiIdentidad = new TipoDocumentoIdentidad();

                        objEmpresa.Nro = DataUtil.DbValueToDefault<int>(objReader[indexNro]);
                        objEmpresa.IdEmpresa = DataUtil.DbValueToDefault<int>(objReader[indexIdEmpresa]);
                        objEmpresa.CodEmpresa = DataUtil.DbValueToDefault<string>(objReader[indexCodigo]);
                        objEmpresa.Ubigeo.CodigoUbigeo = DataUtil.DbValueToDefault<string>(objReader[indexUbigeo]);
                        objEmpresa.Ubigeo.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcionUbigeo]);
                        objEmpresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRUC]);
                        objEmpresa.RazonSocial = DataUtil.DbValueToDefault<string>(objReader[indexRazonSocial]);
                        objEmpresa.RazonComercial = DataUtil.DbValueToDefault<string>(objReader[indexRazonComercial]);
                        objEmpresa.Direccion = DataUtil.DbValueToDefault<string>(objReader[indexDireccion]);
                        objEmpresa.DomicilioFiscal = DataUtil.DbValueToDefault<string>(objReader[indexDomicilioFiscal]);
                        objEmpresa.Urbanizacion = DataUtil.DbValueToDefault<string>(objReader[indexUrbanizacion]);
                        objEmpresa.Telefono = DataUtil.DbValueToDefault<string>(objReader[indexTelefono]);
                        objEmpresa.Fax = DataUtil.DbValueToDefault<string>(objReader[indexFax]);
                        objEmpresa.PaginaWeb = DataUtil.DbValueToDefault<string>(objReader[indexPaginaWeb]);
                        objEmpresa.Email = DataUtil.DbValueToDefault<string>(objReader[indexEmail]);
                        objEmpresa.FechaRegistro = Convert.ToDateTime(objReader[indexFecha]);
                        objEmpresa.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        objEmpresa.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcionEstado]);
                        objEmpresa.TipoDocumentiIdentidad.IdTipoDocumentoIdentidad = DataUtil.DbValueToDefault<int>(objReader[indexIdTipoDoc]);
                        objEmpresa.TipoDocumentiIdentidad.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcionTipoDoc]);
                        objEmpresa.Url_CompanyLogo = DataUtil.DbValueToDefault<string>(objReader[indexUrl_CompanyLogo]);
                        objEmpresa.Url_CompanyConsult = DataUtil.DbValueToDefault<string>(objReader[indexUrl_CompanyConsult]);

                        oListaEmpresa.Add(objEmpresa);
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: GetListadoEmpresa ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaEmpresa;
        }

        #endregion


        #region REGISTRO

        public String RegistrarEmpleado(Empleado oEmpleado)
        {
            string msj = string.Empty;
            
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertarEmpleado;
                cmd.Connection = cnn;

                {
                    cmd.Parameters.AddWithValue("@CodEmpleado", oEmpleado.CodEmpleado);
                    cmd.Parameters.AddWithValue("@Nombres", oEmpleado.Nombres);
                    cmd.Parameters.AddWithValue("@ApePaterno", oEmpleado.ApePaterno);
                    cmd.Parameters.AddWithValue("@ApeMaterno", oEmpleado.ApeMaterno);
                    cmd.Parameters.AddWithValue("@Dni_Ruc", oEmpleado.DNI);
                    cmd.Parameters.AddWithValue("@Sexo", oEmpleado.Sexo.Codigo);
                    cmd.Parameters.AddWithValue("@Direccion", oEmpleado.Direccion);
                    cmd.Parameters.AddWithValue("@Telefono", oEmpleado.Telefono);
                    cmd.Parameters.AddWithValue("@Celular", oEmpleado.Celular);
                    cmd.Parameters.AddWithValue("@FechaIngreso", oEmpleado.FechaRegistro);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", oEmpleado.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Email", oEmpleado.Email);
                    cmd.Parameters.AddWithValue("@IdEstado", oEmpleado.Estado.IdEstado);
                    cmd.Parameters.AddWithValue("@IdEmpresa", oEmpleado.Empresa.IdEmpresa);
                    cmd.Parameters.AddWithValue("@Id_TDI", oEmpleado.TipoDocumentoIdentidad.IdTipoDocumentoIdentidad);

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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: RegistrarEmpleado ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }

        public String ActualizarEmpleado(Empleado oEmpleado)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = Procedimientos.Usp_ActualizarEmpleado;
                    cmd.Connection = cnn;
                    cmd.Parameters.AddWithValue("@IdEmpleado", oEmpleado.IdEmpleado);
                    cmd.Parameters.AddWithValue("@Codigo", oEmpleado.CodEmpleado);
                    cmd.Parameters.AddWithValue("@Nombres", oEmpleado.Nombres);
                    cmd.Parameters.AddWithValue("@ApePaterno", oEmpleado.ApePaterno);
                    cmd.Parameters.AddWithValue("@ApeMaterno", oEmpleado.ApeMaterno);
                    cmd.Parameters.AddWithValue("@DNI", oEmpleado.DNI);
                    cmd.Parameters.AddWithValue("@Direccion", oEmpleado.Direccion);
                    cmd.Parameters.AddWithValue("@Telefono", oEmpleado.Telefono);
                    cmd.Parameters.AddWithValue("@Celular", oEmpleado.Celular);
                    cmd.Parameters.AddWithValue("@Email", oEmpleado.Email);
                    cmd.Parameters.AddWithValue("@Estado", oEmpleado.Estado.IdEstado);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", oEmpleado.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@TipoDocumento", oEmpleado.TipoDocumentoIdentidad.IdTipoDocumentoIdentidad);
                    cmd.Parameters.AddWithValue("@Empresa", oEmpleado.Empresa.IdEmpresa);
                    cmd.Parameters.AddWithValue("@Sexo", oEmpleado.Sexo.IdSexo);

                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Actualizar" : "Actualizado Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: ActualizarEmpleado ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }


        public Int32 ValidarDniRuc(String Dni_Ruc)
        {
            int Validar = 0;

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ValidarDniRuc;
                cmd.Connection = cnn;

                {
                    cmd.Parameters.AddWithValue("@Dni_Ruc", Dni_Ruc);
                    Validar = Convert.ToInt32(cmd.ExecuteScalar());
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: ValidarDniRuc ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return Validar;
        }

        public ListaEmpresa ValidarEmpresaRuc(String Ruc_Empresa)
        {
            Empresa oEmpresa = new Empresa();
            ListaEmpresa oListaEmpresa = new ListaEmpresa();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ValidarRucEmpresa;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@RUC", Ruc_Empresa);

                using (IDataReader oReader = cmd.ExecuteReader())
                {
                    int indexIdEmpresa = oReader.GetOrdinal("IdEmpresa");
                    int indexRucEmpresa = oReader.GetOrdinal("RUC");
                    while (oReader.Read())
                    {
                        oEmpresa = new Empresa();
                        oEmpresa.IdEmpresa = DataUtil.DbValueToDefault<int>(oReader[indexIdEmpresa]);
                        oEmpresa.RUC = DataUtil.DbValueToDefault<string>(oReader[indexRucEmpresa]);
                        oListaEmpresa.Add(oEmpresa);
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: ValidarEmpresaRuc ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaEmpresa;
        }


        public String RegistrarEmpresa(Empresa oEmpresa)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertarEmpresa;
                cmd.Connection = cnn;

                {
                    cmd.Parameters.AddWithValue("@CodEmpresa", oEmpresa.CodEmpresa);
                    cmd.Parameters.AddWithValue("@IdUbigeo", oEmpresa.Ubigeo.IdUbigeo);
                    cmd.Parameters.AddWithValue("@Ubigeo", oEmpresa.Ubigeo.CodigoUbigeo);
                    cmd.Parameters.AddWithValue("@RUC", oEmpresa.RUC);
                    cmd.Parameters.AddWithValue("@RazonSocial", oEmpresa.RazonSocial);
                    cmd.Parameters.AddWithValue("@RazonComercial", oEmpresa.RazonComercial);
                    cmd.Parameters.AddWithValue("@Telefono", oEmpresa.Telefono);
                    cmd.Parameters.AddWithValue("@Fax", oEmpresa.Fax);
                    cmd.Parameters.AddWithValue("@Direccion", oEmpresa.Direccion);
                    cmd.Parameters.AddWithValue("@DomicilioFiscal", oEmpresa.DomicilioFiscal);
                    cmd.Parameters.AddWithValue("@Urbanizacion", oEmpresa.Urbanizacion);
                    cmd.Parameters.AddWithValue("@FechaRegistro", oEmpresa.FechaRegistro);
                    cmd.Parameters.AddWithValue("@PaginaWeb", oEmpresa.PaginaWeb);
                    cmd.Parameters.AddWithValue("@Email", oEmpresa.Email);
                    cmd.Parameters.AddWithValue("@IdEstado", oEmpresa.Estado.IdEstado);
                    cmd.Parameters.AddWithValue("@ID_TDI", oEmpresa.TipoDocumentiIdentidad.IdTipoDocumentoIdentidad);

                    cmd.Parameters.AddWithValue("@Url_CompanyLogo", oEmpresa.Url_CompanyLogo);
                    cmd.Parameters.AddWithValue("@Url_CompanyConsult", oEmpresa.Url_CompanyConsult);

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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: RegistrarEmpresa ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }


        public String ActualizarEmpresa(Empresa oEmpresa)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ActualizarEmpresa;
                cmd.Connection = cnn;

                {
                    cmd.Parameters.AddWithValue("@IdEmpresa", oEmpresa.IdEmpresa);
                    cmd.Parameters.AddWithValue("@CodEmpresa", oEmpresa.CodEmpresa);
                    cmd.Parameters.AddWithValue("@Ubigeo", oEmpresa.Ubigeo.IdUbigeo);
                    cmd.Parameters.AddWithValue("@CodigoUbigeo", oEmpresa.Ubigeo.CodigoUbigeo);
                    cmd.Parameters.AddWithValue("@RUC", oEmpresa.RUC);
                    cmd.Parameters.AddWithValue("@RazonSocial", oEmpresa.RazonSocial);
                    cmd.Parameters.AddWithValue("@RazonComercial", oEmpresa.RazonComercial);
                    cmd.Parameters.AddWithValue("@Telefono", oEmpresa.Telefono);
                    cmd.Parameters.AddWithValue("@Fax", oEmpresa.Fax);
                    cmd.Parameters.AddWithValue("@Direccion", oEmpresa.Direccion);
                    cmd.Parameters.AddWithValue("@DomicilioFiscal", oEmpresa.DomicilioFiscal);
                    cmd.Parameters.AddWithValue("@Urbanizacion", oEmpresa.Urbanizacion);
                    cmd.Parameters.AddWithValue("@FechaRegistro", oEmpresa.FechaRegistro);
                    cmd.Parameters.AddWithValue("@PaginaWeb", oEmpresa.PaginaWeb);
                    cmd.Parameters.AddWithValue("@Email", oEmpresa.Email);
                    cmd.Parameters.AddWithValue("@Estado", oEmpresa.Estado.IdEstado);
                    cmd.Parameters.AddWithValue("@ID_TDI", oEmpresa.TipoDocumentiIdentidad.IdTipoDocumentoIdentidad);

                    cmd.Parameters.AddWithValue("@Url_CompanyLogo", oEmpresa.Url_CompanyLogo);
                    cmd.Parameters.AddWithValue("@Url_CompanyConsult", oEmpresa.Url_CompanyConsult);

                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Actualizar" : "Actualizado Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: ActualizarEmpresa ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }

        public Ubigeo GetUbigeo(String CodigoUbigeo)
        {
            Ubigeo oUbigeo = new Ubigeo();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ObtenerUbigeo;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@CodigoUbigeo", CodigoUbigeo);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdPais = objReader.GetOrdinal("IdPais");
                    int indexIdDepartamento = objReader.GetOrdinal("IdDepartamento");
                    int indexIdProvincia = objReader.GetOrdinal("IdProvincia");
                    int indexIdDistrito = objReader.GetOrdinal("IdDistrito");

                    while (objReader.Read())
                    {
                        oUbigeo = new Ubigeo();
                        oUbigeo.Distrito = new Distrito();
                        oUbigeo.Distrito.Provincia = new Provincia();
                        oUbigeo.Distrito.Provincia.Departamento = new Departamento();
                        oUbigeo.Distrito.Provincia.Departamento.Pais = new Pais();
                        oUbigeo.Distrito.Provincia.Departamento.Pais.IdPais = DataUtil.DbValueToDefault<int>(objReader[indexIdPais]);
                        oUbigeo.Distrito.Provincia.Departamento.IdDepartamento = DataUtil.DbValueToDefault<int>(objReader[indexIdDepartamento]);
                        oUbigeo.Distrito.Provincia.IdProvincia = DataUtil.DbValueToDefault<int>(objReader[indexIdProvincia]);
                        oUbigeo.Distrito.IdDistrito = DataUtil.DbValueToDefault<int>(objReader[indexIdDistrito]);
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: GetUbigeo ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oUbigeo;
        }
        

        #endregion


        #region MANTENIMIENTO DE CLIENTE

        public ListaCliente ValidarNroClienteExiste(String NroDocumento)
        {
            Cliente objCliente = new Cliente();
            ListaCliente oListaCliente = new ListaCliente();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ValidarNroDocumentoCliente;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@NroDocumento", NroDocumento);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdCliente = objReader.GetOrdinal("IdCliente");
                    int indexNroDocumento = objReader.GetOrdinal("NroDocumento");
                    int indexEmail = objReader.GetOrdinal("Email");

                    int indexIdEstado = objReader.GetOrdinal("IdEstado");
                    int indexIdEmpresa = objReader.GetOrdinal("IdEmpresa");

                    while (objReader.Read())
                    {
                        objCliente = new Cliente();
                        objCliente.Estado = new Estado();
                        objCliente.Empresa = new Empresa();
                        objCliente.IdCliente = DataUtil.DbValueToDefault<int>(objReader[indexIdCliente]);
                        objCliente.NroDocumento = DataUtil.DbValueToDefault<string>(objReader[indexNroDocumento]);
                        objCliente.Email = DataUtil.DbValueToDefault<string>(objReader[indexEmail]);
                        objCliente.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        objCliente.Empresa.IdEmpresa = DataUtil.DbValueToDefault<int>(objReader[indexIdEmpresa]);
                        oListaCliente.Add(objCliente);
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: ValidarNroClienteExiste ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaCliente;
        }

        public String InsertarCliente(Cliente oCliente)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertarCliente;
                cmd.Connection = cnn;

                {
                    cmd.Parameters.AddWithValue("@RazonSocial", oCliente.RazonSocial);
                    cmd.Parameters.AddWithValue("@NroDocumento", oCliente.NroDocumento);
                    cmd.Parameters.AddWithValue("@Email", oCliente.Email);
                    cmd.Parameters.AddWithValue("@Telefono", oCliente.Telefono);
                    cmd.Parameters.AddWithValue("@Direccion", oCliente.Direccion);
                    cmd.Parameters.AddWithValue("@IdEstado", oCliente.Estado.IdEstado);
                    cmd.Parameters.AddWithValue("@IdEmpresa", oCliente.Empresa.IdEmpresa);

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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: InsertarCliente ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }

        public String ActualizarCliente(Cliente oCliente)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ActualizarCliente;
                cmd.Connection = cnn;

                {
                    cmd.Parameters.AddWithValue("@IdCliente", oCliente.IdCliente);
                    cmd.Parameters.AddWithValue("@RazonSocial", oCliente.RazonSocial);
                    cmd.Parameters.AddWithValue("@NroDocumento", oCliente.NroDocumento);
                    cmd.Parameters.AddWithValue("@Email", oCliente.Email);
                    cmd.Parameters.AddWithValue("@Telefono", oCliente.Telefono);
                    cmd.Parameters.AddWithValue("@Direccion", oCliente.Direccion);
                    cmd.Parameters.AddWithValue("@IdEstado", oCliente.Estado.IdEstado);
                    cmd.Parameters.AddWithValue("@IdEmpresa", oCliente.Empresa.IdEmpresa);

                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Actualizar" : "Actualizado Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: ActualizarCliente ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }


        public ListaCliente GetListaCliente(Cliente oCliente)
        {
            Cliente objCliente = new Cliente();
            ListaCliente oListaCliente = new ListaCliente();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaCliente;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@RazonSocial", oCliente.RazonSocial);
                cmd.Parameters.AddWithValue("@NroDocumento", oCliente.NroDocumento);
                cmd.Parameters.AddWithValue("@IdEstado", oCliente.Estado.IdEstado);
                cmd.Parameters.AddWithValue("@IdEmpresa", oCliente.Empresa.IdEmpresa);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdCliente = objReader.GetOrdinal("IdCliente");
                    int indexRazonSocial = objReader.GetOrdinal("RazonSocial");
                    int indexNroDocumento = objReader.GetOrdinal("NroDocumento");
                    int indexEmail = objReader.GetOrdinal("Email");

                    int indexTelefono = objReader.GetOrdinal("Telefono");
                    int indexDireccion = objReader.GetOrdinal("Direccion");

                    int indexIdEstado = objReader.GetOrdinal("IdEstado");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");
                    int indexIdEmpresa = objReader.GetOrdinal("IdEmpresa");
                    int indexRucEmpresa = objReader.GetOrdinal("RUC");
                    int indexRazonSocialEmpresa = objReader.GetOrdinal("RazonSocial");

                    while (objReader.Read())
                    {
                        objCliente = new Cliente();
                        objCliente.Estado = new Estado();
                        objCliente.Empresa = new Empresa();
                        objCliente.IdCliente = DataUtil.DbValueToDefault<int>(objReader[indexIdCliente]);
                        objCliente.RazonSocial = DataUtil.DbValueToDefault<string>(objReader[indexRazonSocial]);
                        objCliente.NroDocumento = DataUtil.DbValueToDefault<string>(objReader[indexNroDocumento]);
                        objCliente.Email = DataUtil.DbValueToDefault<string>(objReader[indexEmail]);

                        objCliente.Telefono = DataUtil.DbValueToDefault<string>(objReader[indexTelefono]);
                        objCliente.Direccion = DataUtil.DbValueToDefault<string>(objReader[indexDireccion]);

                        objCliente.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        objCliente.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);
                        objCliente.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);

                        objCliente.Empresa.IdEmpresa = DataUtil.DbValueToDefault<int>(objReader[indexIdEmpresa]);
                        objCliente.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRucEmpresa]);
                        objCliente.Empresa.RazonSocial = DataUtil.DbValueToDefault<string>(objReader[indexRazonSocialEmpresa]);
                        oListaCliente.Add(objCliente);
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: GetListaCliente ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaCliente;
        }

        #endregion





        #region MANT BANK

        public string Insert_CtaBank(Bank objbank)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_Insert_CtaBank;
                cmd.Connection = cnn;

                {
                    cmd.Parameters.AddWithValue("@Code", objbank.Code);
                    cmd.Parameters.AddWithValue("@BankName", objbank.BankName);
                    cmd.Parameters.AddWithValue("@CtaSoles", objbank.CtaSoles);
                    cmd.Parameters.AddWithValue("@CtaDolares", objbank.CtaDolares);
                    cmd.Parameters.AddWithValue("@RucEntity", objbank.Empresa.RUC);
                    cmd.Parameters.AddWithValue("@TypeBank", objbank.TypeBank);
                    cmd.Parameters.AddWithValue("@TypeBank", objbank.Estado.IdEstado);

                    int result = cmd.ExecuteNonQuery();
                    msj = result > 0 ? "Registrado Correctamente" : "Error al registrar";
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: Insert_CtaBank ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }




        public string Update_CtaBank(Bank objbank)
        {
            string msj = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_Update_CtaBank;
                cmd.Connection = cnn;
                {
                    cmd.Parameters.AddWithValue("@Id", objbank.Id);
                    cmd.Parameters.AddWithValue("@Code", objbank.Code);
                    cmd.Parameters.AddWithValue("@BankName", objbank.BankName);
                    cmd.Parameters.AddWithValue("@CtaSoles", objbank.CtaSoles);
                    cmd.Parameters.AddWithValue("@CtaDolares", objbank.CtaDolares);
                    cmd.Parameters.AddWithValue("@RucEntity", objbank.Empresa.RUC);
                    cmd.Parameters.AddWithValue("@TypeBank", objbank.TypeBank);
                    cmd.Parameters.AddWithValue("@TypeBank", objbank.Estado.IdEstado);

                    int result = cmd.ExecuteNonQuery();
                    msj = result > 0 ? "Actualizado Correctamente" : "Error al Actualizar";
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: Update_CtaBank ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }

        public ListBank GetListBank(string RucEntity)
        {
            Bank objBank = new Bank();
            ListBank oListBank = new ListBank();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_getListCtaBank;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@RucEntity", RucEntity);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexId = objReader.GetOrdinal("Id");
                    int indexCode = objReader.GetOrdinal("Code");
                    int indexBankName = objReader.GetOrdinal("BankName");
                    int indexCtaSoles = objReader.GetOrdinal("CtaSoles");

                    int indexCtaDolares = objReader.GetOrdinal("CtaDolares");
                    int indexRucEntity = objReader.GetOrdinal("RUC");

                    int indexIdEstado = objReader.GetOrdinal("TypeBank");
                    int indexTypeBank = objReader.GetOrdinal("IdEstado");
                    int indexEstadoDescripcion = objReader.GetOrdinal("Descripcion");

                    while (objReader.Read())
                    {
                        objBank = new Bank();
                        objBank.Estado = new Estado();
                        objBank.Empresa = new Empresa();

                        objBank.Id = DataUtil.DbValueToDefault<int>(objReader[indexId]);
                        objBank.Code = DataUtil.DbValueToDefault<string>(objReader[indexCode]);
                        objBank.BankName = DataUtil.DbValueToDefault<string>(objReader[indexBankName]);
                        objBank.CtaSoles = DataUtil.DbValueToDefault<string>(objReader[indexCtaSoles]);
                        objBank.CtaDolares = DataUtil.DbValueToDefault<string>(objReader[indexCtaDolares]);
                        objBank.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRucEntity]);
                        objBank.TypeBank = DataUtil.DbValueToDefault<int>(objReader[indexTypeBank]);
                        objBank.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        objBank.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexEstadoDescripcion]);

                        oListBank.Add(objBank);
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: GetListBank ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListBank;
        }


        #endregion END MANT BANK


        #region SETTINGS COMPANY

        public ListaEmpresa Get_ListCompanyGroup(int Grp)
        {
            ListaEmpresa oListaEmpresa = new ListaEmpresa();
            Empresa oEmpresa = new Empresa();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListCompGrp;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Grp", Grp);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdEmpresa = objReader.GetOrdinal("IdEmpresa");
                    int indexCodigo = objReader.GetOrdinal("CodEmpresa");
                    int indexRazonSocial = objReader.GetOrdinal("RazonSocial");
                    int indexTpoLogin = objReader.GetOrdinal("TpoLogin");

                    while (objReader.Read())
                    {
                        oEmpresa = new Empresa();
                        oEmpresa.IdEmpresa = DataUtil.DbValueToDefault<int>(objReader[indexIdEmpresa]);
                        oEmpresa.CodEmpresa = DataUtil.DbValueToDefault<string>(objReader[indexCodigo]);
                        oEmpresa.RazonSocial = DataUtil.DbValueToDefault<string>(objReader[indexRazonSocial]);
                        oEmpresa.TipoLogin = DataUtil.DbValueToDefault<string>(objReader[indexTpoLogin]);
                        oListaEmpresa.Add(oEmpresa);
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
                listError.Add("[" + DateTime.Now + "] [ MANTENIMIENTO - MethodName: Get_ListCompanyGroup ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Mant_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaEmpresa;
        }

        #endregion
    }
}


