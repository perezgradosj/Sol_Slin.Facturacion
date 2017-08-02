using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Configuration;

using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.DataAccess.Helper;
using System.IO;

using Slin.Facturacion.BusinessEntities.Configuracion;
using Slin.Facturacion.Common.Method;

namespace Slin.Facturacion.DataAccess
{
    public class SeguridadDataAccess
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


        public Int32 ValidarAcceso(Usuario oUsuario)
        {
            Int32 Validar = 0;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_IniciarSesion;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Username", oUsuario.Username);
                cmd.Parameters.AddWithValue("@Password", oUsuario.Password);
                cmd.Parameters.AddWithValue("@IdEmpresa", oUsuario.Empresa.IdEmpresa);
                cmd.Parameters.AddWithValue("@TpoLogin", oUsuario.Empresa.TipoLogin);

                Validar = Convert.ToInt32(cmd.ExecuteScalar());

                //Validar = cmd.ExecuteNonQuery();

                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);

                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: ValidarAcceso ] " + ex.Message);

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
            return Validar;
        }

        public String ActualizarContrasenia(Usuario oUsuario)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ActualizarContrasenia;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdUsuario", oUsuario.IdUsuario);
                //cmd.Parameters.AddWithValue("@Username", oUsuario.Username);
                cmd.Parameters.AddWithValue("@Password", oUsuario.Password);
                cmd.Parameters.AddWithValue("@NuevoPassword", oUsuario.NuevoPassword);

                {
                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Actualizar" : "Contraseña Actualizada Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: ActualizarContrasenia ] " + ex.Message);

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

        public Usuario GetUsuarioLogeado(Usuario oUsuario)
        {
            Usuario objUsuario = new Usuario();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ObtenerUsuarioLogeado;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Username", oUsuario.Username);
                cmd.Parameters.AddWithValue("@Password", oUsuario.Password);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdUsuario = objReader.GetOrdinal("IdUsuario");
                    int indexIdEstadoUsuario = objReader.GetOrdinal("IdEstadoUsuario");
                    int indexEstadoUsuario = objReader.GetOrdinal("EstadoUsuario");
                    int indexUsername = objReader.GetOrdinal("Username");
                    int indexNombre = objReader.GetOrdinal("Nombres");
                    int indexApellidos = objReader.GetOrdinal("Apellidos");
                    int indexDni_Ruc = objReader.GetOrdinal("DNI");
                    int indexDireccion = objReader.GetOrdinal("Direccion");
                    int indexTelefono = objReader.GetOrdinal("Telefono");
                    int indexEmail = objReader.GetOrdinal("Email");

                    int indexClaveUsuario = objReader.GetOrdinal("ClaveUsuario");

                    int indexIdEmpresa = objReader.GetOrdinal("IdEmpresa");
                    int indexRuc = objReader.GetOrdinal("Ruc");
                    int indexRazonSocial = objReader.GetOrdinal("RazonSocial");
                    int indexPagWeb = objReader.GetOrdinal("PaginaWeb");

                    //int indexCorreo = objReader.GetOrdinal("Correo"); //ver para que estoy usando estos dos campos
                    //int indexPassword = objReader.GetOrdinal("Password");

                    int indexIdPerfil = objReader.GetOrdinal("IdPerfil");

                    int indexSede = objReader.GetOrdinal("Sede");

                    int indexNameProfile = objReader.GetOrdinal("NombrePerfil");
                    int indexCodProfile = objReader.GetOrdinal("Codigo");

                    int indexIdGrp = objReader.GetOrdinal("IdGrp");
                    int indexGroup = objReader.GetOrdinal("Group");

                    while (objReader.Read())
                    {
                        objUsuario = new Usuario();
                        objUsuario.Estado = new Estado();
                        objUsuario.Empleado = new Empleado();
                        objUsuario.Empresa = new Empresa();
                        objUsuario.Perfil = new Perfil();
                        objUsuario.Sede = new Sede();

                        objUsuario.IdUsuario = DataUtil.DbValueToDefault<int>(objReader[indexIdUsuario]);
                        objUsuario.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstadoUsuario]);
                        objUsuario.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexEstadoUsuario]);
                        objUsuario.Username = DataUtil.DbValueToDefault<string>(objReader[indexUsername]);
                        objUsuario.Empleado.Nombres = DataUtil.DbValueToDefault<string>(objReader[indexNombre]);
                        objUsuario.Empleado.ApePaterno = DataUtil.DbValueToDefault<string>(objReader[indexApellidos]);
                        objUsuario.Empleado.DNI = DataUtil.DbValueToDefault<string>(objReader[indexDni_Ruc]);
                        objUsuario.Empleado.Direccion = DataUtil.DbValueToDefault<string>(objReader[indexDireccion]);
                        objUsuario.Empleado.Telefono = DataUtil.DbValueToDefault<string>(objReader[indexTelefono]);
                        objUsuario.Empleado.Email = DataUtil.DbValueToDefault<string>(objReader[indexEmail]);
                        objUsuario.Password = DataUtil.DbValueToDefault<string>(objReader[indexClaveUsuario]);

                        objUsuario.Empresa.IdEmpresa = DataUtil.DbValueToDefault<int>(objReader[indexIdEmpresa]);
                        objUsuario.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRuc]);
                        objUsuario.Empresa.RazonSocial = DataUtil.DbValueToDefault<string>(objReader[indexRazonSocial]);
                        objUsuario.Empresa.PaginaWeb = DataUtil.DbValueToDefault<string>(objReader[indexPagWeb]);
                        //objUsuario.Empresa.Email = DataUtil.DbValueToDefault<string>(objReader[indexCorreo]);
                        //objUsuario.Empresa.Password = DataUtil.DbValueToDefault<string>(objReader[indexPassword]);

                        objUsuario.Perfil.IdPerfil = DataUtil.DbValueToDefault<int>(objReader[indexIdPerfil]);

                        objUsuario.Sede.Name = DataUtil.DbValueToDefault<string>(objReader[indexSede]);
                        objUsuario.Perfil.NombrePerfil = DataUtil.DbValueToDefault<string>(objReader[indexNameProfile]);
                        objUsuario.Perfil.Codigo = DataUtil.DbValueToDefault<string>(objReader[indexCodProfile]);

                        objUsuario.Empresa.CodEmpresa = DataUtil.DbValueToDefault<string>(objReader[indexCodProfile]);
                        objUsuario.Empresa.IdGrp = DataUtil.DbValueToDefault<int>(objReader[indexIdGrp]);
                        objUsuario.Empresa.Group = DataUtil.DbValueToDefault<string>(objReader[indexGroup]);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetUsuarioLogeado ] " + ex.Message);

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
            return objUsuario;
        }



        public ListaMenu GetListaMenu()
        {
            Menu oMenu = new Menu();
            ListaMenu oListaMenu = new ListaMenu();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListarMenu;
                cmd.Connection = cnn;

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdMenu = objReader.GetOrdinal("IdMenu");
                    int indexNombreMenu = objReader.GetOrdinal("NombreMenu");
                    int indexCodigoMenu = objReader.GetOrdinal("CodigoMenu");
                    int indexPadreMenu = objReader.GetOrdinal("PadreMenu");
                    int indexNivelMenu = objReader.GetOrdinal("NivelMenu");
                    int indexIdEstado = objReader.GetOrdinal("IdEstado");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");

                    while (objReader.Read())
                    {
                        oMenu = new Menu();
                        oMenu.Estado = new Estado();

                        oMenu.IdMenu = DataUtil.DbValueToDefault<int>(objReader[indexIdMenu]);
                        oMenu.NombreMenu = DataUtil.DbValueToDefault<string>(objReader[indexNombreMenu]).Remove(0, 5);
                        oMenu.CodigoMenu = DataUtil.DbValueToDefault<string>(objReader[indexCodigoMenu]);
                        oMenu.PadreMenu = DataUtil.DbValueToDefault<int>(objReader[indexPadreMenu]);
                        oMenu.NivelMenu = DataUtil.DbValueToDefault<int>(objReader[indexNivelMenu]);
                        oMenu.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        oMenu.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);
                        oListaMenu.Add(oMenu);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListaMenu ] " + ex.Message);

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
            return oListaMenu;
        }


        public ListaPerfil GetListaPerfiles(string RucEntity)
        {
            Perfil oPerfil = new Perfil();
            ListaPerfil oListaPerfil = new ListaPerfil();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListadoPerfiles;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@RucEntity", RucEntity);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdPerfil = objReader.GetOrdinal("IdPerfil");
                    int indexNombreMenu = objReader.GetOrdinal("NombrePerfil");
                    int indexCodigoMenu = objReader.GetOrdinal("Codigo");
                    int indexRucEntity = objReader.GetOrdinal("RucEntity");

                    while (objReader.Read())
                    {
                        oPerfil = new Perfil();
                        oPerfil.Empresa = new Empresa();

                        oPerfil.IdPerfil = DataUtil.DbValueToDefault<int>(objReader[indexIdPerfil]);
                        oPerfil.NombrePerfil = DataUtil.DbValueToDefault<string>(objReader[indexNombreMenu]);
                        oPerfil.Codigo = DataUtil.DbValueToDefault<string>(objReader[indexCodigoMenu]);
                        oPerfil.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRucEntity]);

                        oListaPerfil.Add(oPerfil);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListaPerfiles ] " + ex.Message);

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
            return oListaPerfil;
        }


        public ListaMenu GetListarMenuPerfil(Perfil oPerfil)
        {
            Menu oMenu = new Menu();
            ListaMenu oListaMenu = new ListaMenu();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListarMenuPerfil;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdPerfil", oPerfil.IdPerfil);
                cmd.Parameters.AddWithValue("@RucEntity", oPerfil.Empresa.RUC);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdPerfil = objReader.GetOrdinal("IdPerfil");
                    int indexIdMenu = objReader.GetOrdinal("IdMenu");
                    int indexNombreMenu = objReader.GetOrdinal("NombreMenu");
                    int indexCodigoMenu = objReader.GetOrdinal("CodigoMenu");
                    int indexPadreMenu = objReader.GetOrdinal("PadreMenu");
                    int indexNivelMenu = objReader.GetOrdinal("NivelMenu");
                    int indexIdEstado = objReader.GetOrdinal("IdEstado");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");

                    int indexRucEntity = objReader.GetOrdinal("RucEntity");

                    while (objReader.Read())
                    {
                        oMenu = new Menu();
                        oMenu.Estado = new Estado();
                        oMenu.Perfil = new Perfil();
                        oMenu.Empresa = new Empresa();

                        oMenu.Perfil.IdPerfil = DataUtil.DbValueToDefault<int>(objReader[indexIdPerfil]);
                        oMenu.IdMenu = DataUtil.DbValueToDefault<int>(objReader[indexIdMenu]);
                        oMenu.NombreMenu = DataUtil.DbValueToDefault<string>(objReader[indexNombreMenu]);
                        oMenu.CodigoMenu = DataUtil.DbValueToDefault<string>(objReader[indexCodigoMenu]);
                        oMenu.PadreMenu = DataUtil.DbValueToDefault<int>(objReader[indexPadreMenu]);
                        oMenu.NivelMenu = DataUtil.DbValueToDefault<int>(objReader[indexNivelMenu]);
                        oMenu.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        oMenu.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);

                        oMenu.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRucEntity]);

                        oListaMenu.Add(oMenu);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListarMenuPerfil ] " + ex.Message);

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
            return oListaMenu;
        }



        public ListaMenu GetListarMenu(Usuario oUsuario)
        {
            Menu oMenu = new Menu();
            ListaMenu oListaMenu = new ListaMenu();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandText = Procedimientos.Usp_ListarPerfilUsuario;
                cmd.CommandText = Procedimientos.Usp_ListarMenuPerfilUsuario;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdUsuario", oUsuario.IdUsuario);
                cmd.Parameters.AddWithValue("@RucEntity", oUsuario.Empresa.RUC);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdPerfil = objReader.GetOrdinal("IdPerfil");
                    int indexIdMenu = objReader.GetOrdinal("IdMenu");
                    int indexNombreMenu = objReader.GetOrdinal("NombreMenu");
                    int indexCodigoMenu = objReader.GetOrdinal("CodigoMenu");
                    int indexPadreMenu = objReader.GetOrdinal("PadreMenu");
                    int indexNivelMenu = objReader.GetOrdinal("NivelMenu");
                    int indexIdEstado = objReader.GetOrdinal("IdEstado");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");

                    int indexRucEntity = objReader.GetOrdinal("RucEntity");

                    while (objReader.Read())
                    {
                        oMenu = new Menu();
                        oMenu.Estado = new Estado();
                        oMenu.Perfil = new Perfil();
                        oMenu.Empresa = new Empresa();

                        oMenu.Perfil.IdPerfil = DataUtil.DbValueToDefault<int>(objReader[indexIdPerfil]);
                        oMenu.IdMenu = DataUtil.DbValueToDefault<int>(objReader[indexIdMenu]);
                        oMenu.NombreMenu = DataUtil.DbValueToDefault<string>(objReader[indexNombreMenu]).Remove(0, 5);
                        oMenu.CodigoMenu = DataUtil.DbValueToDefault<string>(objReader[indexCodigoMenu]);
                        oMenu.PadreMenu = DataUtil.DbValueToDefault<int>(objReader[indexPadreMenu]);
                        oMenu.NivelMenu = DataUtil.DbValueToDefault<int>(objReader[indexNivelMenu]);
                        oMenu.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIdEstado]);
                        oMenu.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);

                        oMenu.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRucEntity]);

                        oListaMenu.Add(oMenu);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListarMenu ] " + ex.Message);

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
            return oListaMenu;
        }



        public String RegistrarUsuarioPerfil(Usuario oUsuario)
        {
            string msj = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_RegistrarUsuarioPerfil;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdUsuario", oUsuario.IdUsuario);
                cmd.Parameters.AddWithValue("@IdPerfil", oUsuario.Perfil.IdPerfil);

                {
                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Registrar" : "Añadido Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: RegistrarUsuarioPerfil ] " + ex.Message);

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


        public String DeleteUsuarioPerfil(Usuario oUsuario)
        {
            string msj = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_DeleteUsuarioPerfil;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdUsuario", oUsuario.IdUsuario);
                cmd.Parameters.AddWithValue("@IdPerfil", oUsuario.Perfil.IdPerfil);

                {
                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Quitar" : "Quitado Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: DeleteUsuarioPerfil ] " + ex.Message);

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

        public ListaRol GetListadoRol()
        {
            Rol oRol = new Rol();
            ListaRol oListaRol = new ListaRol();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListadoRoles;
                cmd.Connection = cnn;

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdRol = objReader.GetOrdinal("IdRol");
                    int indexNombreRol = objReader.GetOrdinal("NombreRol");
                    int indexCodigoRol = objReader.GetOrdinal("CodigoRol");

                    while (objReader.Read())
                    {
                        oRol = new Rol();
                        oRol.IdRol = DataUtil.DbValueToDefault<int>(objReader[indexIdRol]);
                        oRol.NombreRol = DataUtil.DbValueToDefault<string>(objReader[indexNombreRol]);
                        oRol.CodigoRol = DataUtil.DbValueToDefault<string>(objReader[indexCodigoRol]);
                        oRol.Padre = 0;
                        oListaRol.Add(oRol);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListadoRol ] " + ex.Message);

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
            return oListaRol;
        }

        public ListaRol GetListaRolesUsuario(Usuario oUsuario)
        {
            Rol oRol = new Rol();
            ListaRol oListaRol = new ListaRol();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListarRolUsuario;
                cmd.Connection = cnn;

                //cmd.Parameters.AddWithValue("@IdUsuario", oUsuario.IdUsuario);
                cmd.Parameters.AddWithValue("@Dni", oUsuario.Empleado.DNI);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdRol = objReader.GetOrdinal("IdRol");
                    int indexNombreRol = objReader.GetOrdinal("NombreRol");
                    int indexCodigoRol = objReader.GetOrdinal("CodigoRol");

                    while (objReader.Read())
                    {
                        oRol = new Rol();
                        oRol.IdRol = DataUtil.DbValueToDefault<int>(objReader[indexIdRol]);
                        oRol.NombreRol = DataUtil.DbValueToDefault<string>(objReader[indexNombreRol]);
                        oRol.CodigoRol = DataUtil.DbValueToDefault<string>(objReader[indexCodigoRol]);
                        oListaRol.Add(oRol);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListaRolesUsuario ] " + ex.Message);

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
            return oListaRol;
        }

        public String RegistrarUsuarioRol(Usuario usuarioRol)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_RegistrarUsuarioRol;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdRol", usuarioRol.Rol.IdRol);
                cmd.Parameters.AddWithValue("@Dni", usuarioRol.Empleado.DNI.TrimEnd());
                {
                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Registrar" : "Registrado Correctamente";
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                //msj = "Error al Registrar";
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: RegistrarUsuarioRol ] " + ex.Message);

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

        public String DeleteUsuarioRol(Usuario oUsuario)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_DeleteUsuarioRol;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Dni", oUsuario.Empleado.DNI.TrimEnd());
                cmd.Parameters.AddWithValue("@IdRol", oUsuario.Rol.IdRol);
                {
                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Quitar" : "Quitado Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: DeleteUsuarioRol ] " + ex.Message);

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


        public Int32 ObtenerNuevoIdUsuario()
        {
            int IdUsuario = 0;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ObtenerUltimoIdUsuario;
                cmd.Connection = cnn;
                using (IDataReader oReader = cmd.ExecuteReader())
                {
                    int index = oReader.GetOrdinal("IdUsuario");

                    while (oReader.Read())
                    {
                        IdUsuario = DataUtil.DbValueToDefault<int>(oReader[index]);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: ObtenerNuevoIdUsuario ] " + ex.Message);

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
            return IdUsuario;
        }



        public String RegistrarNuevoPerfil(Perfil oPerfil)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertarNuevoPerfil;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@NombrePerfil", oPerfil.NombrePerfil);
                cmd.Parameters.AddWithValue("@Codigo", oPerfil.Codigo);
                cmd.Parameters.AddWithValue("@RucEntity", oPerfil.Empresa.RUC);

                {
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: RegistrarNuevoPerfil ] " + ex.Message);

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



        public String Delete_ProfileComp(Perfil profile)
        {
            string msj = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_DeleteProfile;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdProf", profile.IdPerfil);
                cmd.Parameters.AddWithValue("@Cod", profile.Codigo);
                cmd.Parameters.AddWithValue("@RucComp", profile.Empresa.RUC);

                {
                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Quitar" : "Quitado Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: Delete_ProfileComp ] " + ex.Message);

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



        public ListaPerfil GetListaPerfil(Perfil oPerfil)
        {
            Perfil operfil = new Perfil();
            ListaPerfil oListaPerfil = new ListaPerfil();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaPerfiles;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@NombrePerfil", oPerfil.NombrePerfil);
                cmd.Parameters.AddWithValue("@RucEntity", oPerfil.Empresa.RUC);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdPerfil = objReader.GetOrdinal("IdPerfil");
                    int indexNombrePerfil = objReader.GetOrdinal("NombrePerfil");
                    int indexCodigo = objReader.GetOrdinal("Codigo");
                    int indexRucEntity = objReader.GetOrdinal("RucEntity");

                    while (objReader.Read())
                    {
                        operfil = new Perfil();
                        operfil.Empresa = new Empresa();

                        operfil.IdPerfil = DataUtil.DbValueToDefault<int>(objReader[indexIdPerfil]);
                        operfil.NombrePerfil = DataUtil.DbValueToDefault<string>(objReader[indexNombrePerfil]);
                        operfil.Codigo = DataUtil.DbValueToDefault<string>(objReader[indexCodigo]);
                        operfil.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRucEntity]);
                        oListaPerfil.Add(operfil);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListaPerfil ] " + ex.Message);

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
            return oListaPerfil;
        }


        public String InsertarMenuPerfil(Perfil oPerfil)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertarMenuPerfil;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdMenu", oPerfil.IdMenu);
                cmd.Parameters.AddWithValue("@IdPerfil", oPerfil.IdPerfil);
                {
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: InsertarMenuPerfil ] " + ex.Message);

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


        public String DeleteMenuPerfil(Perfil oPerfil)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_DeleteMenuPerfil;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdMenu", oPerfil.IdMenu);
                cmd.Parameters.AddWithValue("@IdPerfil", oPerfil.IdPerfil);
                {
                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Quitar" : "Quitado Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: DeleteMenuPerfil ] " + ex.Message);

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


        public String BloquearUsuario(Usuario oUsuario)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_BloquearUsuario;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Username", oUsuario.Username);
                //cmd.Parameters.AddWithValue("@Password", oUsuario.Password);
                {
                    int ressult = cmd.ExecuteNonQuery();
                    msj = ressult == 0 ? "Error al Bloquear" : "Usuario Bloqueado";
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: BloquearUsuario ] " + ex.Message);

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


        public String InsertarCorreo(Correo oCorreo)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertarCorreo;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdEmpresa", oCorreo.Empresa.IdEmpresa);
                cmd.Parameters.AddWithValue("@Email", oCorreo.Email);
                cmd.Parameters.AddWithValue("@Password", oCorreo.Password);
                cmd.Parameters.AddWithValue("@DOMAIN", oCorreo.Domain);
                cmd.Parameters.AddWithValue("@IP", oCorreo.IP);
                cmd.Parameters.AddWithValue("@PORT", oCorreo.Port);
                cmd.Parameters.AddWithValue("@RucEmpresa", oCorreo.Empresa.RUC);
                cmd.Parameters.AddWithValue("@IDESTADO", oCorreo.Estado.IdEstado);

                cmd.Parameters.AddWithValue("@TypeMail", oCorreo.TypeMail);
                cmd.Parameters.AddWithValue("@UseSSL", oCorreo.SSL.Id); // if 1 = true; 0 = false;

                //cmd.Parameters.AddWithValue("@UseAuthenticate", oCorreo.UseAuthenticate); //if 1 = true; 0 = false;

                {
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: InsertarCorreo ] " + ex.Message);

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

        public String ActualizarCorreo(Correo oCorreo)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ActualizarCorreo;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdEmail", oCorreo.IdEmail);
                cmd.Parameters.AddWithValue("@IdEmpresa", oCorreo.Empresa.IdEmpresa);
                cmd.Parameters.AddWithValue("@Email", oCorreo.Email);
                cmd.Parameters.AddWithValue("@Password", oCorreo.Password);
                cmd.Parameters.AddWithValue("@DOMAIN", oCorreo.Domain);
                cmd.Parameters.AddWithValue("@IP", oCorreo.IP);
                cmd.Parameters.AddWithValue("@PORT", oCorreo.Port);
                cmd.Parameters.AddWithValue("@RucEmpresa", oCorreo.Empresa.RUC);
                cmd.Parameters.AddWithValue("@IDESTADO", oCorreo.Estado.IdEstado);

                cmd.Parameters.AddWithValue("@TypeMail", oCorreo.TypeMail);
                cmd.Parameters.AddWithValue("@UseSSL", oCorreo.SSL.Id); // if 1 = true; 0 = false;

                //cmd.Parameters.AddWithValue("@UseAuthenticate", oCorreo.UseAuthenticate);// if 1 = true; 0 = false;

                {
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: ActualizarCorreo ] " + ex.Message);

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


        public String DeleteCorreo(Correo oCorreo)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_DeleteCorreo;
                cmd.Connection = cnn;

                //cmd.Parameters.AddWithValue("@IdEmail", oCorreo.IdCorreo);
                cmd.Parameters.AddWithValue("@IdEmpresa", oCorreo.Empresa.IdEmpresa);
                cmd.Parameters.AddWithValue("@Email", oCorreo.Email);
                {
                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Quitar" : "Quitado Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: DeleteCorreo ] " + ex.Message);

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

        public ListaCorreo GetListaCorreo(Empresa oEmpresa)
        {
            Correo oCorreo = new Correo();
            ListaCorreo oListaCorreo = new ListaCorreo();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListaCorreo;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdEmpresa", oEmpresa.IdEmpresa);
                cmd.Parameters.AddWithValue("@RazonSocial", oEmpresa.RazonSocial);
                cmd.Parameters.AddWithValue("@Email", oEmpresa.Email);
                cmd.Parameters.AddWithValue("@IdEstado", oEmpresa.Estado.IdEstado);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdCorreo = objReader.GetOrdinal("IdEmail");
                    int indexIdEmpresa = objReader.GetOrdinal("IdEmpresa");
                    int indexRazonSocial = objReader.GetOrdinal("RazonSocial");
                    int indexEmail = objReader.GetOrdinal("Email");
                    int indexContrasena = objReader.GetOrdinal("Password");

                    int indexDomain = objReader.GetOrdinal("Domain");
                    int indexIP = objReader.GetOrdinal("IP");
                    int indexPort = objReader.GetOrdinal("Port");
                    int indexEstado = objReader.GetOrdinal("IdEstado");
                    int indexDescEstado = objReader.GetOrdinal("Descripcion");
                    int indexRucEmpresa = objReader.GetOrdinal("RUC");

                    int indexTypeMail = objReader.GetOrdinal("TypeMail");
                    int indexSSL = objReader.GetOrdinal("UseSSL");

                    //int indexUseAuthenticate = objReader.GetOrdinal("UseAuthenticate");

                    while (objReader.Read())
                    {
                        oCorreo = new Correo();
                        oCorreo.Estado = new Estado();
                        oCorreo.Empresa = new Empresa();
                        oCorreo.SSL = new SSL();

                        oCorreo.IdEmail = DataUtil.DbValueToDefault<int>(objReader[indexIdCorreo]);
                        oCorreo.Empresa.IdEmpresa = DataUtil.DbValueToDefault<int>(objReader[indexIdEmpresa]);
                        oCorreo.Empresa.RazonSocial = DataUtil.DbValueToDefault<string>(objReader[indexRazonSocial]);
                        oCorreo.Email = DataUtil.DbValueToDefault<string>(objReader[indexEmail]);
                        oCorreo.Password = DataUtil.DbValueToDefault<string>(objReader[indexContrasena]);

                        oCorreo.Domain = DataUtil.DbValueToDefault<string>(objReader[indexDomain]);
                        oCorreo.IP = DataUtil.DbValueToDefault<string>(objReader[indexIP]);
                        oCorreo.Port = DataUtil.DbValueToDefault<int>(objReader[indexPort]);
                        oCorreo.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexEstado]);
                        oCorreo.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescEstado]);
                        oCorreo.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRucEmpresa]);

                        oCorreo.TypeMail = DataUtil.DbValueToDefault<string>(objReader[indexTypeMail]);
                        oCorreo.SSL.Id = DataUtil.DbValueToDefault<int>(objReader[indexSSL]);

                        //oCorreo.UseAuthenticate = DataUtil.DbValueToDefault<int>(objReader[indexUseAuthenticate]);

                        oListaCorreo.Add(oCorreo);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListaCorreo ] " + ex.Message);

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
            return oListaCorreo;
        }

        public ListaCorreo ValidarExistsCorreoEmpresa(Correo oCorreo)
        {
            Correo objCorreo = new Correo();
            ListaCorreo objListaCorreo = new ListaCorreo();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ValidarExistsCorreoEmpresa;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdEmpresa", oCorreo.Empresa.IdEmpresa);
                cmd.Parameters.AddWithValue("@Email", oCorreo.Email);
                cmd.Parameters.AddWithValue("@RucEmpresa", oCorreo.Empresa.RUC);
                cmd.Parameters.AddWithValue("@TypeMail", oCorreo.TypeMail);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdCorreo = objReader.GetOrdinal("IdEmail");
                    int indexIdEmpresa = objReader.GetOrdinal("IdEmpresa");
                    int indexRazonSocial = objReader.GetOrdinal("RazonSocial");
                    int indexEmail = objReader.GetOrdinal("Email");

                    while (objReader.Read())
                    {
                        objCorreo = new Correo();
                        objCorreo.Empresa = new Empresa();

                        objCorreo.IdEmail = DataUtil.DbValueToDefault<int>(objReader[indexIdCorreo]);
                        objCorreo.Empresa.IdEmpresa = DataUtil.DbValueToDefault<int>(objReader[indexIdEmpresa]);
                        objCorreo.Empresa.RazonSocial = DataUtil.DbValueToDefault<string>(objReader[indexRazonSocial]);
                        objCorreo.Email = DataUtil.DbValueToDefault<string>(objReader[indexEmail]);
                        objListaCorreo.Add(objCorreo);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: ValidarExistsCorreoEmpresa ] " + ex.Message);

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
            return objListaCorreo;
        }

        #endregion





        #region Ky

        public ListaSum41 GetKySum41ToDeep(string value1, string value2, string value3, string value4, string value5)
        {
            Sum41 obj = new Sum41();
            ListaSum41 listaobj = new ListaSum41();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_KySum41;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Parametro1", value1);
                cmd.Parameters.AddWithValue("@Parametro2", value2);
                cmd.Parameters.AddWithValue("@Parametro3", value3);
                cmd.Parameters.AddWithValue("@Parametro4", value4);
                cmd.Parameters.AddWithValue("@Parametro5", value5);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    //int indexKy = objReader.GetOrdinal("Ky");
                    int indexId = objReader.GetOrdinal("Id");
                    //int indexKySum41 = objReader.GetOrdinal("KySum41");

                    while (objReader.Read())
                    {
                        obj = new Sum41();
                        obj.Id = DataUtil.DbValueToDefault<int>(objReader[indexId]);
                        listaobj.Add(obj);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetKySum41ToDeep ] " + ex.Message);

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
            return listaobj;
        }

        public ListaEmpresa GetobjEntity(string value)
        {
            ListaEmpresa objlista = new ListaEmpresa();
            Empresa obj = new Empresa();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetIdEntityEmp;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Parametro", value);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdEntity = objReader.GetOrdinal("EntityId");

                    while (objReader.Read())
                    {
                        obj = new Empresa();
                        obj.RUC = DataUtil.DbValueToDefault<string>(objReader[indexIdEntity]);
                        objlista.Add(obj);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetobjEntity ] " + ex.Message);

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
            return objlista;
        }



        public ListaEmpresa GetEntityEmpresa(string entityId)
        {
            ListaEmpresa objlista = new ListaEmpresa();
            Empresa obj = new Empresa();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetEntitySetup;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@EntityId", entityId);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdEntity = objReader.GetOrdinal("EntityId");
                    int indexRucEntity = objReader.GetOrdinal("RucEntity");
                    int indexRazonSocialEntity = objReader.GetOrdinal("RazonSocial");

                    while (objReader.Read())
                    {
                        obj = new Empresa();
                        obj.IdEmpresa = DataUtil.DbValueToDefault<int>(objReader[indexIdEntity]);
                        obj.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRucEntity]);
                        obj.RazonSocial = DataUtil.DbValueToDefault<string>(objReader[indexRazonSocialEntity]);
                        objlista.Add(obj);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetEntityEmpresa ] " + ex.Message);

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
            return objlista;
        }


        public Empresa GetobjEntitySingle(string value)
        {
            Empresa obj = new Empresa();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetIdEntityEmp;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Parametro", value);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdEntity = objReader.GetOrdinal("EntityId");

                    while (objReader.Read())
                    {
                        obj = new Empresa();
                        obj.RUC = DataUtil.DbValueToDefault<string>(objReader[indexIdEntity]);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetobjEntitySingle ] " + ex.Message);

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
            return obj;
        }


        #endregion


        #region Get Info Entity

        public Empresa GetCredentialEntity(string RucEmpresa)
        {
            Empresa obj = new Empresa();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetCredentialEntitySend;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@RucEmpresa", RucEmpresa);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexRucEmpresa = objReader.GetOrdinal("Ruc");
                    int indexEmail = objReader.GetOrdinal("Email");
                    int indexPassword = objReader.GetOrdinal("Password");
                    int indexDomain = objReader.GetOrdinal("Domain");
                    int indexIP = objReader.GetOrdinal("IP");
                    int indexPort = objReader.GetOrdinal("Port");
                    int indexUseSsl = objReader.GetOrdinal("UseSSL");

                    int indexUrl_CompanyLogo = objReader.GetOrdinal("Url_CompanyLogo");
                    int indexUrl_CompanyConsult = objReader.GetOrdinal("Url_CompanyConsult");

                    while (objReader.Read())
                    {
                        obj = new Empresa();
                        obj.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRucEmpresa]);
                        obj.Email = DataUtil.DbValueToDefault<string>(objReader[indexEmail]);
                        obj.Password = DataUtil.DbValueToDefault<string>(objReader[indexPassword]);
                        obj.Dominio = DataUtil.DbValueToDefault<string>(objReader[indexDomain]);
                        obj.IP = DataUtil.DbValueToDefault<string>(objReader[indexIP]);
                        obj.Port = DataUtil.DbValueToDefault<int>(objReader[indexPort]);
                        obj.UseSSL = DataUtil.DbValueToDefault<int>(objReader[indexUseSsl]);

                        obj.Url_CompanyLogo = DataUtil.DbValueToDefault<string>(objReader[indexUrl_CompanyLogo]);
                        obj.Url_CompanyConsult = DataUtil.DbValueToDefault<string>(objReader[indexUrl_CompanyConsult]);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetCredentialEntity ] " + ex.Message);

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
            return obj;
        }



        public Empresa GetCredentialEntity_Received(string RucEmpresa)
        {
            Empresa obj = new Empresa();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetCredentialEntity_Received;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@RucEmpresa", RucEmpresa);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexRucEmpresa = objReader.GetOrdinal("Ruc");
                    int indexEmail = objReader.GetOrdinal("Email");
                    int indexPassword = objReader.GetOrdinal("Password");
                    int indexDomain = objReader.GetOrdinal("Domain");
                    int indexIP = objReader.GetOrdinal("IP");
                    int indexPort = objReader.GetOrdinal("Port");
                    int indexUseSSL = objReader.GetOrdinal("UseSSL");

                    while (objReader.Read())
                    {
                        obj = new Empresa();
                        obj.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRucEmpresa]);
                        obj.Email = DataUtil.DbValueToDefault<string>(objReader[indexEmail]);
                        obj.Password = DataUtil.DbValueToDefault<string>(objReader[indexPassword]);
                        obj.Dominio = DataUtil.DbValueToDefault<string>(objReader[indexDomain]);
                        obj.IP = DataUtil.DbValueToDefault<string>(objReader[indexIP]);
                        obj.Port = DataUtil.DbValueToDefault<int>(objReader[indexPort]);
                        obj.UseSSL = DataUtil.DbValueToDefault<int>(objReader[indexUseSSL]);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetCredentialEntity_Received ] " + ex.Message);

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
            return obj;
        }


        public ListaEmail GetListTypeMailEntity()
        {
            logError = new StringBuilder();
            Email oMail = new Email();
            ListaEmail oListMail = new ListaEmail();

            try
            {
                oListMail.Insert(0, new Email() { TypeMail = "Send", Description = "Envio" });
                oListMail.Insert(1, new Email() { TypeMail = "Reception", Description = "Recepción" });
                oListMail.Insert(2, new Email() { TypeMail = "Support", Description = "Soporte" });
                //oListaMail.Insert(0, new Email() { TypeMail = " ", = "- Seleccione -" });
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListTypeMailEntity ] " + ex.Message);

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
            return oListMail;
        }

        public ListSSL GetListUseProt_SSL()
        {
            logError = new StringBuilder();
            SSL ossl = new SSL();
            ListSSL oListSSL = new ListSSL();

            try
            {
                oListSSL.Insert(0, new SSL() { Id = 1, Description = "SI" });
                oListSSL.Insert(0, new SSL() { Id = 0, Description = "NO" });
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListUseProt_SSL ] " + ex.Message);

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
            return oListSSL;
        }


        public AmbienteTrabjActual GetAmbienteTrabjActual(string RucEntity)
        {
            AmbienteTrabjActual obj = new AmbienteTrabjActual();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetAmbienteTrbjActual;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@RUCENTITY", RucEntity);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexId = objReader.GetOrdinal("ID");
                    int indexCod = objReader.GetOrdinal("COD");
                    int indexDesc = objReader.GetOrdinal("DESCRIPCION");
                    int indexIdAmb = objReader.GetOrdinal("IDAMBIENTE");

                    while (objReader.Read())
                    {
                        obj = new AmbienteTrabjActual();
                        obj.ID = DataUtil.DbValueToDefault<int>(objReader[indexId]);
                        obj.COD = DataUtil.DbValueToDefault<string>(objReader[indexCod]);
                        obj.DESCRIPCION = DataUtil.DbValueToDefault<string>(objReader[indexDesc]);
                        obj.IDAmbiente = DataUtil.DbValueToDefault<int>(objReader[indexIdAmb]);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetAmbienteTrabjActual ] " + ex.Message);

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
            return obj;
        }



        public String UpdateAmbTrabjActual(AmbienteTrabjActual objAmb)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_UpdateAmbTrabjActual;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdAmbTrabj", objAmb.ID);
                cmd.Parameters.AddWithValue("@RUCENTITY", objAmb.Empresa.RUC);
                {
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: UpdateAmbTrabjActual ] " + ex.Message);

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


        public ListaAmbienteSunat GetListAmbTrabj()
        {
            ListaAmbienteSunat objlista = new ListaAmbienteSunat();
            AmbienteSunat obj = new AmbienteSunat();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetListAmbiente;
                cmd.Connection = cnn;

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIDAMBIENTE = objReader.GetOrdinal("IDAMBIENTE");
                    int indexCOD = objReader.GetOrdinal("COD");
                    int indexDESCRIPCION = objReader.GetOrdinal("DESCRIPCION");
                    int indexIDESTADO = objReader.GetOrdinal("IDESTADO");
                    int indexEstadoDescripcion = objReader.GetOrdinal("EstadoDescripcion");

                    while (objReader.Read())
                    {
                        obj = new AmbienteSunat();
                        obj.Estado = new Estado();
                        obj.IdAmbiente = DataUtil.DbValueToDefault<int>(objReader[indexIDAMBIENTE]);
                        obj.Codigo = DataUtil.DbValueToDefault<string>(objReader[indexCOD]);
                        obj.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDESCRIPCION]);
                        obj.Estado.IdEstado = DataUtil.DbValueToDefault<int>(objReader[indexIDESTADO]);
                        obj.Estado.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexEstadoDescripcion]);
                        objlista.Add(obj);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListAmbTrabj ] " + ex.Message);

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
            return objlista;
        }


        #endregion



        #region AUDITORIA

        public Usuario GetDataFromUserLogueo(string Username)
        {
            Usuario obj = new Usuario();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetDataFromUserLogueo;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Username", Username);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexID = objReader.GetOrdinal("IdUsuario");
                    int indexDni = objReader.GetOrdinal("DNI");
                    int indexIdPerfil = objReader.GetOrdinal("IdPerfil");

                    while (objReader.Read())
                    {
                        obj = new Usuario();
                        obj.Perfil = new Perfil();
                        obj.Empleado = new Empleado();

                        obj.IdUsuario = DataUtil.DbValueToDefault<int>(objReader[indexID]);
                        obj.Empleado.DNI = DataUtil.DbValueToDefault<string>(objReader[indexDni]);
                        obj.Perfil.IdPerfil = DataUtil.DbValueToDefault<int>(objReader[indexIdPerfil]);

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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetDataFromUserLogueo ] " + ex.Message);

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
            return obj;
        }

        public Int32 InsertRegistroLogueo(LogueoClass ObjLogeo)
        {
            int IdRet = 0;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertRegistroLogueo;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@FECHA", Convert.ToDateTime(ObjLogeo.Fecha.ToShortDateString()));
                cmd.Parameters.AddWithValue("@DNI_USUARIO", ObjLogeo.Empleado.DNI);
                cmd.Parameters.AddWithValue("@USERNAME", ObjLogeo.Username);
                cmd.Parameters.AddWithValue("@IDPERFIL", ObjLogeo.Perfil.IdPerfil);
                cmd.Parameters.AddWithValue("@FECHAINGRESO", ObjLogeo.FechaIngreso);
                cmd.Parameters.AddWithValue("@HOSTNAME", ObjLogeo.HostName);
                cmd.Parameters.AddWithValue("@SIP", ObjLogeo.sIP);
                cmd.Parameters.AddWithValue("@IDTPOLOG", ObjLogeo.TipoLog.ID);
                cmd.Parameters.AddWithValue("@IdEmpresa", ObjLogeo.Empresa.IdEmpresa);

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
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: InsertRegistroLogueo ] " + ex.Message);

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
            return IdRet;
        }

        public String UpdateRegistroLogueo(LogueoClass ObjLogeo)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_UpdateRegistroLogueo;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@ID", ObjLogeo.ID);
                cmd.Parameters.AddWithValue("@DNI_USUARIO", ObjLogeo.Empleado.DNI);
                cmd.Parameters.AddWithValue("@USERNAME", ObjLogeo.Username);
                cmd.Parameters.AddWithValue("@FECHASALIDA", ObjLogeo.FechaSalida);
                {
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: UpdateRegistroLogueo ] " + ex.Message);

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


        public String DeleteRegistroLogueox3M(string FechaDesde, string RucEntity)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_DeleteRegistroLogueo;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@FechaDesde", Convert.ToDateTime(FechaDesde));
                cmd.Parameters.AddWithValue("@RucEntity", RucEntity);
                {
                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Quitar" : "Quitado Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: DeleteRegistroLogueox3M ] " + ex.Message);

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

        public ListaLogueoClass GetListLogueoClass(LogueoClass objLogueo)
        {
            ListaLogueoClass objlista = new ListaLogueoClass();
            LogueoClass obj = new LogueoClass();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetListRegistroLogueo;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@FechaDesde", objLogueo.FechaDesde);
                cmd.Parameters.AddWithValue("@FechaHasta", objLogueo.FechaHasta);
                cmd.Parameters.AddWithValue("@IdTipoLog", objLogueo.TipoLog.ID);
                cmd.Parameters.AddWithValue("@RucEntity", objLogueo.Empresa.RUC);
                cmd.Parameters.AddWithValue("@IdPerfil", objLogueo.Perfil.IdPerfil);
                cmd.Parameters.AddWithValue("@Username", objLogueo.Username);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexID = objReader.GetOrdinal("ID");
                    int indexDNI = objReader.GetOrdinal("DNI");
                    int indexIdPerfil = objReader.GetOrdinal("IdPerfil");
                    int indexNombPerfil = objReader.GetOrdinal("NombrePerfil");
                    int indexUsername = objReader.GetOrdinal("Username");
                    int indexNombreApellido = objReader.GetOrdinal("NombresApellidos");
                    int indexFechaIngreso = objReader.GetOrdinal("FechaIngreso");
                    int indexFechaSalida = objReader.GetOrdinal("FechaSalida");
                    int indexHostName = objReader.GetOrdinal("HostName");
                    int indexsIP = objReader.GetOrdinal("sIP");
                    int indexIdTipoLog = objReader.GetOrdinal("IdTipoLog");
                    int indexDescrTipoLog = objReader.GetOrdinal("Descripcion");

                    while (objReader.Read())
                    {
                        obj = new LogueoClass();
                        obj.Perfil = new Perfil();
                        obj.Empleado = new Empleado();
                        obj.TipoLog = new TipoLog();

                        obj.ID = DataUtil.DbValueToDefault<int>(objReader[indexID]);
                        obj.Empleado.DNI = DataUtil.DbValueToDefault<string>(objReader[indexDNI]);
                        obj.DNI = DataUtil.DbValueToDefault<string>(objReader[indexDNI]);
                        obj.Perfil.IdPerfil = DataUtil.DbValueToDefault<int>(objReader[indexIdPerfil]);
                        obj.IdPerfil = DataUtil.DbValueToDefault<int>(objReader[indexIdPerfil]);
                        obj.Perfil.NombrePerfil = DataUtil.DbValueToDefault<string>(objReader[indexNombPerfil]);
                        obj.NombrePerfil = DataUtil.DbValueToDefault<string>(objReader[indexNombPerfil]);
                        obj.Username = DataUtil.DbValueToDefault<string>(objReader[indexUsername]);
                        obj.Empleado.NombresApellidos = DataUtil.DbValueToDefault<string>(objReader[indexNombreApellido]);
                        obj.NombresApellidos = DataUtil.DbValueToDefault<string>(objReader[indexNombreApellido]);
                        obj.FechaIngreso = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaIngreso]);
                        obj.FechaSalida = DataUtil.DbValueToDefault<DateTime>(objReader[indexFechaSalida]);
                        obj.HostName = DataUtil.DbValueToDefault<string>(objReader[indexHostName]);
                        obj.sIP = DataUtil.DbValueToDefault<string>(objReader[indexsIP]);
                        obj.TipoLog.ID = DataUtil.DbValueToDefault<int>(objReader[indexIdTipoLog]);
                        obj.TipoLog.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescrTipoLog]);
                        obj.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescrTipoLog]);
                        objlista.Add(obj);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListLogueoClass ] " + ex.Message);

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
            return objlista;
        }


        public Int32 InsertLogLogueo(LogueoClass ObjLogeo)
        {
            int IdRet = 0;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertLogLogueo;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@FECHA", ObjLogeo.Fecha);
                //cmd.Parameters.AddWithValue("@USERNAME", ObjLogeo.Username);
                cmd.Parameters.AddWithValue("@USUARIO_LOG", ObjLogeo.Username_log);
                cmd.Parameters.AddWithValue("@IDTIPOLOG", ObjLogeo.TipoLog.ID);

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
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: InsertLogLogueo ] " + ex.Message);

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
            return IdRet;
        }


        public ListaLogueoClass GetListLogLogueo(LogueoClass objlog)
        {
            ListaLogueoClass objlista = new ListaLogueoClass();
            LogueoClass obj = new LogueoClass();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetListLogLogueo;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@FechaDesde", objlog.FechaDesde);
                cmd.Parameters.AddWithValue("@FechaHasta", objlog.FechaHasta);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexID = objReader.GetOrdinal("ID");
                    int indexFecha = objReader.GetOrdinal("Fecha");
                    int indexUsername = objReader.GetOrdinal("Username");
                    int indexUsername_Log = objReader.GetOrdinal("Username_Log");
                    int indexIdTipoLog = objReader.GetOrdinal("IdTipoLog");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");

                    while (objReader.Read())
                    {
                        obj = new LogueoClass();
                        obj.TipoLog = new TipoLog();

                        obj.ID = DataUtil.DbValueToDefault<int>(objReader[indexID]);
                        obj.Fecha = DataUtil.DbValueToDefault<DateTime>(objReader[indexFecha]);
                        obj.Username = DataUtil.DbValueToDefault<string>(objReader[indexUsername]);
                        obj.Username_log = DataUtil.DbValueToDefault<string>(objReader[indexUsername_Log]);
                        obj.TipoLog.ID = DataUtil.DbValueToDefault<int>(objReader[indexIdTipoLog]);
                        obj.TipoLog.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);
                        obj.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);
                        objlista.Add(obj);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListLogLogueo ] " + ex.Message);

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
            return objlista;
        }

        #endregion


        #region LOG ADE

        public Int32 InsertLogADE(LogAde objlog)
        {
            int IdRet = 0;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_InsertLogAde;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdMenu", objlog.Menu.IdMenu);
                cmd.Parameters.AddWithValue("@MenuAmbiente", objlog.MenuAmbiente);
                cmd.Parameters.AddWithValue("@MessageLog", objlog.MessageLog);
                cmd.Parameters.AddWithValue("@InnerException", objlog.InnerException);
                cmd.Parameters.AddWithValue("@Fecha", objlog.Fecha);
                cmd.Parameters.AddWithValue("@Correction", objlog.Correction);

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
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: InsertLogADE ] " + ex.Message);

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
            return IdRet;
        }

        public ListaLogAde GetListLogADE(LogAde objlog)
        {
            ListaLogAde objlista = new ListaLogAde();
            LogAde obj = new LogAde();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetListLogAde;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdMenu", objlog.Menu.IdMenu);
                cmd.Parameters.AddWithValue("@FechaDesde", objlog.FechaDesde);
                cmd.Parameters.AddWithValue("@FechaHasta", objlog.FechaHasta);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexID = objReader.GetOrdinal("ID");
                    int indexIdMenu = objReader.GetOrdinal("IdMenu");
                    int indexNombreMenu = objReader.GetOrdinal("NombreMenu");
                    int indexMenuAmb = objReader.GetOrdinal("MenuAmbiente");
                    int indexMessageLog = objReader.GetOrdinal("MessageLog");
                    int indexInnerException = objReader.GetOrdinal("InnerException");
                    int indexFecha = objReader.GetOrdinal("Fecha");
                    int indexCorrection = objReader.GetOrdinal("Correction");

                    while (objReader.Read())
                    {
                        obj = new LogAde();
                        obj.Menu = new Menu();

                        obj.ID = DataUtil.DbValueToDefault<int>(objReader[indexID]);
                        obj.IdMenu = DataUtil.DbValueToDefault<int>(objReader[indexIdMenu]);
                        obj.Menu.IdMenu = DataUtil.DbValueToDefault<int>(objReader[indexIdMenu]);
                        obj.Menu.NombreMenu = DataUtil.DbValueToDefault<string>(objReader[indexNombreMenu]);
                        obj.NombreMenu = DataUtil.DbValueToDefault<string>(objReader[indexNombreMenu]);
                        obj.MenuAmbiente = DataUtil.DbValueToDefault<string>(objReader[indexMenuAmb]);

                        obj.MessageLog = DataUtil.DbValueToDefault<string>(objReader[indexMessageLog]);
                        obj.InnerException = DataUtil.DbValueToDefault<string>(objReader[indexInnerException]);
                        obj.Fecha = DataUtil.DbValueToDefault<DateTime>(objReader[indexFecha]);
                        obj.Correction = DataUtil.DbValueToDefault<string>(objReader[indexCorrection]);
                        objlista.Add(obj);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListLogADE ] " + ex.Message);

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
            return objlista;
        }

        #endregion END LOG ADE


        #region UTIL LOG

        public ListaTipoLog GetListTipoLog()
        {
            ListaTipoLog objlista = new ListaTipoLog();
            TipoLog obj = new TipoLog();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetListTipoLog;
                cmd.Connection = cnn;

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexID = objReader.GetOrdinal("ID");
                    int indexCodigo = objReader.GetOrdinal("Codigo");
                    int indexDescripcion = objReader.GetOrdinal("Descripcion");

                    while (objReader.Read())
                    {
                        obj = new TipoLog();

                        obj.ID = DataUtil.DbValueToDefault<int>(objReader[indexID]);
                        obj.Codigo = DataUtil.DbValueToDefault<string>(objReader[indexCodigo]);
                        obj.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexDescripcion]);
                        objlista.Add(obj);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListTipoLog ] " + ex.Message);

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
            return objlista;
        }

        #endregion


        #region SEDE
        public ListaSede GetListSede(string RucEntity)
        {
            Sede obj = new Sede();
            ListaSede objList = new ListaSede();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_ListSede;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@RucEntity", RucEntity);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexId = objReader.GetOrdinal("Id");
                    int indexCod = objReader.GetOrdinal("Cod");
                    int indexNombre = objReader.GetOrdinal("Name");
                    int indexRuc = objReader.GetOrdinal("RucEntity");

                    while (objReader.Read())
                    {
                        obj = new Sede();
                        obj.Empresa = new Empresa();

                        obj.IdSede = DataUtil.DbValueToDefault<int>(objReader[indexId]);
                        obj.Cod = DataUtil.DbValueToDefault<string>(objReader[indexCod]);
                        obj.Name = DataUtil.DbValueToDefault<string>(objReader[indexNombre]);
                        obj.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRuc]);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListSede ] " + ex.Message);

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
            return objList;
        }
        #endregion

        #region

        public ListService GetStatus_WService(string entityId, string NameService)
        {
            Services objService = new Services();
            ListService objList = new ListService();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetStatus_WService;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Ruc", entityId);
                cmd.Parameters.AddWithValue("@NameService", NameService);

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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetStatus_WService ] " + ex.Message);

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

            return objList;
        }

        #endregion


        #region user change company
        public String Update_UserCompany(Usuario ouser)
        {
            string msj = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_Update_UserCompany;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdComp", ouser.Empresa.IdEmpresa);
                //cmd.Parameters.AddWithValue("@RucComp", ouser.Empresa.RUC);
                cmd.Parameters.AddWithValue("@IdUser", ouser.IdUsuario);
                cmd.Parameters.AddWithValue("@Ind", ouser.DNI);
                cmd.Parameters.AddWithValue("@IdProf", ouser.Perfil.IdPerfil);

                {
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: Update_UserCompany ] " + ex.Message);

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



        public ListaPerfil GetList_ProfileCompany(int IdComp)
        {
            Perfil oPerfil = new Perfil();
            ListaPerfil oListaPerfil = new ListaPerfil();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetList_ProfileCompany;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdComp", IdComp);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdPerfil = objReader.GetOrdinal("IdPerfil");
                    int indexNombreMenu = objReader.GetOrdinal("NombrePerfil");
                    int indexCodigoMenu = objReader.GetOrdinal("Codigo");
                    int indexRucEntity = objReader.GetOrdinal("RucEntity");

                    while (objReader.Read())
                    {
                        oPerfil = new Perfil();
                        oPerfil.Empresa = new Empresa();

                        oPerfil.IdPerfil = DataUtil.DbValueToDefault<int>(objReader[indexIdPerfil]);
                        oPerfil.NombrePerfil = DataUtil.DbValueToDefault<string>(objReader[indexNombreMenu]);
                        oPerfil.Codigo = DataUtil.DbValueToDefault<int>(objReader[indexIdPerfil]) + "-" + DataUtil.DbValueToDefault<string>(objReader[indexCodigoMenu]);
                        oPerfil.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRucEntity]);

                        oListaPerfil.Add(oPerfil);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetList_ProfileCompany ] " + ex.Message);

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
            return oListaPerfil;
        }


        public ListAuthenticate Get_ListAuthenticate()
        {
            logError = new StringBuilder();
            ListAuthenticate olistAuthent = new ListAuthenticate();

            try
            {
                olistAuthent.Insert(0, new Authenticate() { IdUseAuthenticate = 0, Desc = "SI" });
                olistAuthent.Insert(1, new Authenticate() { IdUseAuthenticate = 1, Desc = "NO" });
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);
                Singleton.Instance.CreateDirectory(PathLogSLINADE);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: Get_ListAuthenticate ] " + ex.Message);

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
            return olistAuthent;
        }
        #endregion




        #region mail to alert

        public ListaCorreo GetList_NotificationsMail(string ruccomp)
        {
            Correo obj = new Correo();
            ListaCorreo oListObj = new ListaCorreo();

            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetList_NotificationsMail;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@ruccomp", ruccomp);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexRucEntity = objReader.GetOrdinal("RucEntity");
                    int indexEmail = objReader.GetOrdinal("Email");
                    int indexContactName = objReader.GetOrdinal("ContactName");
                    int indexTypeMail = objReader.GetOrdinal("TypeMail");

                    while (objReader.Read())
                    {
                        obj = new Correo();
                        obj.Empresa = new Empresa();

                        obj.Empresa.RUC = DataUtil.DbValueToDefault<string>(objReader[indexRucEntity]);
                        obj.Email = DataUtil.DbValueToDefault<string>(objReader[indexEmail]);
                        obj.ContactName = DataUtil.DbValueToDefault<string>(objReader[indexContactName]);
                        obj.TypeMail = DataUtil.DbValueToDefault<string>(objReader[indexTypeMail]);

                        oListObj.Add(obj);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetList_NotificationsMail ] " + ex.Message);

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
            return oListObj;
        }

        public String Insert_MailToAlert(Correo oCorreo)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_Insert_MailAlert;
                cmd.Connection = cnn;

                //cmd.Parameters.AddWithValue("@IdEmpresa", oCorreo.Empresa.IdEmpresa);
                cmd.Parameters.AddWithValue("@RucComp", oCorreo.Empresa.RUC); 
                cmd.Parameters.AddWithValue("@Email", oCorreo.Email);
                cmd.Parameters.AddWithValue("@ContactName", oCorreo.ContactName);
                cmd.Parameters.AddWithValue("@Email_Old", oCorreo.EmailSalida);
                cmd.Parameters.AddWithValue("@TypeMail", oCorreo.TypeMail);

                {
                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Registrar, es posible que el correo ya exista" : "Registrado Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: Insert_MailToAlert ] " + ex.Message);

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


        public String Update_MailToAlert(Correo oCorreo)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_Update_MailAlert;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@RucComp", oCorreo.Empresa.RUC);
                cmd.Parameters.AddWithValue("@Email", oCorreo.Email);
                cmd.Parameters.AddWithValue("@ContactName", oCorreo.ContactName);

                {
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: Update_MailToAlert ] " + ex.Message);

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


        public String Delete_MailToAlert(Correo oCorreo)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_Delete_MailAlert;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@RucComp", oCorreo.Empresa.RUC);
                cmd.Parameters.AddWithValue("@Email", oCorreo.Email);
                cmd.Parameters.AddWithValue("@TypeMail", oCorreo.TypeMail);

                {
                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Eliminar" : "Eliminado Correctamente";
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: Delete_MailToAlert ] " + ex.Message);

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

        #endregion


        #region get id
        public Empresa GetId(string ruccomp)
        {
            Empresa obj = new Empresa();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.Usp_GetId;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@ruccomp", ruccomp);
                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexId = objReader.GetOrdinal("Id");
                    while (objReader.Read())
                    {
                        obj = new Empresa();
                        obj.IdEmpresa = DataUtil.DbValueToDefault<int>(objReader[indexId]);
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
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetId ] " + ex.Message);
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
            return obj;            
        }
        #endregion
    }
}