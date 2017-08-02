using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

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
    public class UsuarioDataAccess
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

        #region SEGURIDAD



        public String RegistrarUsuario(Usuario oUsuario)
        {
            

            string msj = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = Procedimientos.Usp_RegistrarUsuario;//no hacer nada
                    cmd.Connection = cnn;

                    cmd.Parameters.AddWithValue("@IdEmpleado", oUsuario.Empleado.IdEmpleado);
                    cmd.Parameters.AddWithValue("@IdEstado", oUsuario.Estado.IdEstado);
                    cmd.Parameters.AddWithValue("@IdEmpresa", oUsuario.Empresa.IdEmpresa);
                    cmd.Parameters.AddWithValue("@DNI", oUsuario.Empleado.DNI);
                    cmd.Parameters.AddWithValue("@RUC", oUsuario.Empresa.RUC);
                    cmd.Parameters.AddWithValue("@Username", oUsuario.Username);
                    cmd.Parameters.AddWithValue("@Password", oUsuario.Password);
                    cmd.Parameters.AddWithValue("@FechaRegistro", oUsuario.FechaRegistro);
                    cmd.Parameters.AddWithValue("@IdPerfil", oUsuario.Perfil.IdPerfil);

                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Insertar" : "Registrado Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: RegistrarUsuario ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }

            return msj;
        }

        public String InsertarUsuario(Usuario oUsuario)
        {
            string msj = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = Procedimientos.Usp_InsertarUsuario;
                    cmd.Connection = cnn;

                    cmd.Parameters.AddWithValue("@IdEstado", oUsuario.Estado.IdEstado);
                    cmd.Parameters.AddWithValue("@IdEmpresa", oUsuario.Empresa.IdEmpresa);
                    cmd.Parameters.AddWithValue("@IdPerfil", oUsuario.Perfil.IdPerfil);
                    cmd.Parameters.AddWithValue("@Nombres", oUsuario.Empleado.Nombres);
                    cmd.Parameters.AddWithValue("@ApePaterno", oUsuario.Empleado.ApePaterno);
                    cmd.Parameters.AddWithValue("@ApeMaterno", oUsuario.Empleado.ApeMaterno);
                    cmd.Parameters.AddWithValue("@DNI_RUC", oUsuario.Empleado.DNI);
                    cmd.Parameters.AddWithValue("@Direccion", oUsuario.Empleado.Direccion);
                    cmd.Parameters.AddWithValue("@Telefono", oUsuario.Empleado.Telefono);
                    cmd.Parameters.AddWithValue("@Email", oUsuario.Empleado.Email);
                    cmd.Parameters.AddWithValue("@Username", oUsuario.Username);
                    cmd.Parameters.AddWithValue("@Password", oUsuario.Password);
                    cmd.Parameters.AddWithValue("@FechaExpiracion", oUsuario.FechaExpiracion);
                    cmd.Parameters.AddWithValue("@FechaRegistro", oUsuario.FechaRegistro);

                    cmd.Parameters.AddWithValue("@NameSede", oUsuario.Sede.Name);

                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Insertar" : "Registrado Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: InsertarUsuario ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }

        public String ActualizarUsuario(Usuario oUsuario)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = Procedimientos.Usp_ActualizarUsuario;
                    cmd.Connection = cnn;

                    cmd.Parameters.AddWithValue("@IdUsuario", oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("@IdEstado", oUsuario.Estado.IdEstado);
                    cmd.Parameters.AddWithValue("@IdEmpresa", oUsuario.Empresa.IdEmpresa);
                    cmd.Parameters.AddWithValue("@IdPerfil", oUsuario.Perfil.IdPerfil);
                    cmd.Parameters.AddWithValue("@Nombres", oUsuario.Empleado.Nombres);
                    cmd.Parameters.AddWithValue("@ApePaterno", oUsuario.Empleado.ApePaterno);
                    cmd.Parameters.AddWithValue("@ApeMaterno", oUsuario.Empleado.ApeMaterno);
                    cmd.Parameters.AddWithValue("@DNI_RUC", oUsuario.Empleado.DNI);
                    cmd.Parameters.AddWithValue("@Direccion", oUsuario.Empleado.Direccion);
                    cmd.Parameters.AddWithValue("@Telefono", oUsuario.Empleado.Telefono);
                    cmd.Parameters.AddWithValue("@Email", oUsuario.Empleado.Email);
                    cmd.Parameters.AddWithValue("@Username", oUsuario.Username);
                    cmd.Parameters.AddWithValue("@Password", oUsuario.NuevoPassword);
                    cmd.Parameters.AddWithValue("@FechaExpiracion", oUsuario.FechaExpiracion);

                    cmd.Parameters.AddWithValue("@NameSede", oUsuario.Sede.Name);

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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: ActualizarUsuario ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }

        public ListaUsuario ValidarDniUsuario(String Dni_Ruc)
        {
            Usuario oUsuario = new Usuario();
            ListaUsuario oListaUsuario = new ListaUsuario();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ValidarDniUsuario;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Dni_Ruc", Dni_Ruc);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    int indexIdUsuario = reader.GetOrdinal("IdUsuario");
                    int indexDni = reader.GetOrdinal("DNI");

                    while (reader.Read())
                    {
                        oUsuario = new Usuario();
                        oUsuario.Empleado = new Empleado();

                        oUsuario.IdUsuario = DataUtil.DbValueToDefault<int>(reader[indexIdUsuario]);
                        oUsuario.Empleado.DNI = DataUtil.DbValueToDefault<string>(reader[indexDni]);
                        oListaUsuario.Add(oUsuario);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: ValidarDniUsuario ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaUsuario;
        }

        public ListaUsuario InsertarUsuario_ForExcel(ListaUsuario olistausuario)
        {
            Usuario objUsuario = new Usuario();
            ListaUsuario oListaUser = new ListaUsuario();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = Procedimientos.Usp_InsertarUsuario_Excel;
                    cmd.Connection = cnn;

                    for (int i = 0; i <= olistausuario.Count - 1; i++)
                    {
                        int result = 0;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@IdEstado", olistausuario[i].Estado.IdEstado);
                        cmd.Parameters.AddWithValue("@IdEmpresa", olistausuario[i].Empresa.IdEmpresa);
                        cmd.Parameters.AddWithValue("@IdPerfil", olistausuario[i].Perfil.IdPerfil);
                        cmd.Parameters.AddWithValue("@Nombres", olistausuario[i].Empleado.Nombres);
                        cmd.Parameters.AddWithValue("@ApePaterno", olistausuario[i].Empleado.ApePaterno);
                        cmd.Parameters.AddWithValue("@ApeMaterno", olistausuario[i].Empleado.ApeMaterno);
                        cmd.Parameters.AddWithValue("@DNI_RUC", olistausuario[i].Empleado.DNI);
                        cmd.Parameters.AddWithValue("@Direccion", olistausuario[i].Empleado.Direccion);
                        cmd.Parameters.AddWithValue("@Telefono", olistausuario[i].Empleado.Telefono);
                        cmd.Parameters.AddWithValue("@Email", olistausuario[i].Empleado.Email);
                        cmd.Parameters.AddWithValue("@Username", olistausuario[i].Username);
                        cmd.Parameters.AddWithValue("@Password", olistausuario[i].Password);
                        cmd.Parameters.AddWithValue("@FechaExpiracion", olistausuario[i].FechaExpiracion);
                        cmd.Parameters.AddWithValue("@FechaRegistro", olistausuario[i].FechaRegistro);


                        result = cmd.ExecuteNonQuery();

                        if (result <= 0)
                        {
                            objUsuario = new Usuario();
                            objUsuario.Perfil = new Perfil();
                            objUsuario.Estado = new Estado();
                            objUsuario.Empresa = new Empresa();
                            objUsuario.Empleado = new Empleado();

                            objUsuario.Estado.IdEstado = olistausuario[i].Estado.IdEstado;
                            objUsuario.Perfil.IdPerfil = olistausuario[i].Perfil.IdPerfil;
                            objUsuario.Empleado.Nombres = olistausuario[i].Empleado.Nombres;
                            objUsuario.Empleado.ApePaterno = olistausuario[i].Empleado.ApePaterno;
                            objUsuario.Empleado.ApeMaterno = olistausuario[i].Empleado.ApeMaterno;
                            objUsuario.Empleado.DNI = olistausuario[i].Empleado.DNI;
                            objUsuario.Empleado.Direccion = olistausuario[i].Empleado.Direccion;
                            objUsuario.Empleado.Telefono = olistausuario[i].Empleado.Telefono;
                            objUsuario.Empleado.Email = olistausuario[i].Empleado.Email;
                            objUsuario.Username = olistausuario[i].Username;
                            objUsuario.Password = olistausuario[i].Password;
                            objUsuario.FechaExpiracion = olistausuario[i].FechaExpiracion;
                            objUsuario.FechaRegistro = olistausuario[i].FechaRegistro;
                            objUsuario.Empresa.RUC = olistausuario[i].Empresa.RUC;

                            oListaUser.Add(objUsuario);
                        }
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: InsertarUsuario_ForExcel ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaUser;
        }

        public ListaUsuario GetListaUsuario(Usuario oUsuario)
        {
            ListaUsuario oListaUsuario = new ListaUsuario();
            Usuario objUsuario = new Usuario();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaUsuario;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Empresa", oUsuario.Empresa.IdEmpresa);
                cmd.Parameters.AddWithValue("@DNI", oUsuario.Empleado.CodEmpleado);
                cmd.Parameters.AddWithValue("@Username", oUsuario.Username);
                cmd.Parameters.AddWithValue("@Estado", oUsuario.Estado.IdEstado);
                cmd.Parameters.AddWithValue("@RucEmpresa", oUsuario.Empresa.RUC);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexNro = objReader.GetOrdinal("Nro");
                    int indexIdUsuario = objReader.GetOrdinal("IdUsuario");
                    int indexDNI = objReader.GetOrdinal("DNI");
                    int indexEmpleado = objReader.GetOrdinal("Empleado");
                    int indexNombApe = objReader.GetOrdinal("NombresApellidos");
                    int indexNombres = objReader.GetOrdinal("Nombres");
                    int indexApePaterno = objReader.GetOrdinal("ApePaterno");
                    int indexApeMaterno = objReader.GetOrdinal("ApeMaterno");
                    int indexUsername = objReader.GetOrdinal("Username");
                    int indexPassword = objReader.GetOrdinal("Password");
                    int indexDireccion = objReader.GetOrdinal("Direccion");
                    int indexTelefono = objReader.GetOrdinal("Telefono");
                    int indexEmail = objReader.GetOrdinal("Email");
                    int indexIdEstado = objReader.GetOrdinal("IdEstado");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");
                    int indexIdEmpresa = objReader.GetOrdinal("IdEmpresa");
                    int indexRazonSocial = objReader.GetOrdinal("RazonSocial");
                    int indexRuc = objReader.GetOrdinal("Ruc");
                    int indexIdPerfil = objReader.GetOrdinal("IdPerfil");
                    int indexNombrePerfil = objReader.GetOrdinal("NombrePerfil");
                    int indexFechaExpiracion = objReader.GetOrdinal("FechaExpiracion");

                    int indexSede = objReader.GetOrdinal("Sede");

                    while (objReader.Read())
                    {
                        objUsuario = new Usuario();
                        objUsuario.Empleado = new Empleado();
                        objUsuario.Estado = new Estado();
                        objUsuario.Empresa = new Empresa();
                        objUsuario.Perfil = new Perfil();
                        objUsuario.Sede = new Sede();

                        objUsuario.Nro = DataUtil.DbValueToDefault<int>(objReader[indexNro]);
                        objUsuario.IdUsuario = DataUtil.DbValueToDefault<int>(objReader[indexIdUsuario]);
                        objUsuario.Empleado.DNI = DataUtil.DbValueToDefault<string>(objReader[indexDNI]);

                        objUsuario.DNI = DataUtil.DbValueToDefault<string>(objReader[indexDNI]);

                        objUsuario.Empleado.NombresApellidos = DataUtil.DbValueToDefault<string>(objReader[indexEmpleado]);

                        objUsuario.NombresApellidos = DataUtil.DbValueToDefault<string>(objReader[indexNombApe]);

                        objUsuario.Empleado.Nombres = DataUtil.DbValueToDefault<string>(objReader[indexNombres]);
                        objUsuario.Empleado.ApePaterno = DataUtil.DbValueToDefault<string>(objReader[indexApePaterno]);
                        objUsuario.Empleado.ApeMaterno = DataUtil.DbValueToDefault<string>(objReader[indexApeMaterno]);


                        objUsuario.Username = DataUtil.DbValueToDefault<string>(objReader[indexUsername]);
                        objUsuario.Password = DataUtil.DbValueToDefault<string>(objReader[indexPassword]);

                        objUsuario.Empleado.Direccion = DataUtil.DbValueToDefault<string>(objReader[indexDireccion]);
                        objUsuario.Empleado.Telefono = DataUtil.DbValueToDefault<string>(objReader[indexTelefono]);
                        objUsuario.Empleado.Email = DataUtil.DbValueToDefault<string>(objReader[indexEmail]);

                        objUsuario.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        objUsuario.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);

                        objUsuario.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);

                        objUsuario.Empresa.IdEmpresa = DataUtil.DbValueToDefault<int>(objReader[indexIdEmpresa]);
                        objUsuario.Empresa.RazonSocial = DataUtil.DbValueToDefault<string>(objReader[indexRazonSocial]);
                        objUsuario.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRuc]);

                        objUsuario.Perfil.IdPerfil = DataUtil.DbValueToDefault<int>(objReader[indexIdPerfil]);
                        objUsuario.Perfil.NombrePerfil = DataUtil.DbValueToDefault<string>(objReader[indexNombrePerfil]);

                        objUsuario.NombrePerfil = DataUtil.DbValueToDefault<string>(objReader[indexNombrePerfil]);

                        objUsuario.FechaExpiracion = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaExpiracion]);

                        
                        objUsuario.Sede.Name = DataUtil.DbValueToDefault<string>(objReader[indexSede]);

                        oListaUsuario.Add(objUsuario);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListaUsuario ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaUsuario;
        }

        public ListaUsuario ValidarUsername(String Username)
        {
            ListaUsuario oListaUsuario = new ListaUsuario();
            Usuario objUsuario = new Usuario();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ValidarUsername;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Username", Username);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexUsername = objReader.GetOrdinal("Username");
                    int indexDNI = objReader.GetOrdinal("DNI");

                    while (objReader.Read())
                    {
                        objUsuario = new Usuario();
                        objUsuario.Empleado = new Empleado();
                        objUsuario.Username = DataUtil.DbValueToDefault<string>(objReader[indexUsername]);
                        objUsuario.Empleado.DNI = DataUtil.DbValueToDefault<string>(objReader[indexDNI]);
                        oListaUsuario.Add(objUsuario);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: ValidarUsername ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaUsuario;
        }


        public ListaEmpleados ValidarExisteEmpleado(String DNI)
        {
            Empleado oEmpleado = new Empleado();
            ListaEmpleados oListaEmpleado = new ListaEmpleados();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ObtenerDatosxDNI;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Dni", DNI);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdEmpleado = objReader.GetOrdinal("IdEmpleado");
                    int indexNombres = objReader.GetOrdinal("Nombres");
                    int indexApePaterno = objReader.GetOrdinal("ApePaterno");
                    int indexApeMaterno = objReader.GetOrdinal("ApeMaterno");
                    int indexDni = objReader.GetOrdinal("DNI");
                    int indexIdEmpresa = objReader.GetOrdinal("IdEmpresa");
                    int indexRuc = objReader.GetOrdinal("Ruc");
                    int indexRazonSocial = objReader.GetOrdinal("RazonSocial");

                    while (objReader.Read())
                    {
                        oEmpleado = new Empleado();
                        oEmpleado.Empresa = new Empresa();

                        oEmpleado.IdEmpleado = DataUtil.DbValueToDefault<int>(objReader[indexIdEmpleado]);
                        oEmpleado.Nombres = DataUtil.DbValueToDefault<string>(objReader[indexNombres]);
                        oEmpleado.ApePaterno = DataUtil.DbValueToDefault<string>(objReader[indexApePaterno]);
                        oEmpleado.ApeMaterno = DataUtil.DbValueToDefault<string>(objReader[indexApeMaterno]);
                        oEmpleado.DNI = DataUtil.DbValueToDefault<string>(objReader[indexDni]);
                        oEmpleado.Empresa.IdEmpresa = DataUtil.DbValueToDefault<int>(objReader[indexIdEmpresa]);
                        oEmpleado.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRuc]);
                        oEmpleado.Empresa.RazonSocial = DataUtil.DbValueToDefault<string>(objReader[indexRazonSocial]);
                        oListaEmpleado.Add(oEmpleado);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: ValidarExisteEmpleado ] " + ex.Message);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListaEmpleado;
        }

        #endregion
    }
}
