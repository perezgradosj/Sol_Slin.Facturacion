using Slin.Facturacion.InstallCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.InstallCommon
{
    public class InstallDataAccess
    {

        static string HOST = string.Empty;
        static string BD = string.Empty;
        static string USER = string.Empty;
        static string PWD = string.Empty;
        static string cadena = string.Empty;

        SqlConnection cnn;// = new SqlConnection(cadena);

        public InstallDataAccess(string host, string bd, string user, string pwd)
        {
            HOST = host;
            BD = bd;
            USER = user;
            PWD = pwd;
            cadena = "Server=" + HOST + ";Database=" + BD + ";User=" + USER + ";pwd=" + PWD;

            cnn = new SqlConnection(cadena);
        }

        


        


        StringBuilder logError = new StringBuilder();
        List<string> listError = new List<string>();

        #region OTHERS
        void CrearNuevaCarpeta(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        #endregion


        #region registr

        public int InsertCompany(Company objcompany)
        {
            int result = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_registrCompany;
                cmd.Connection = cnn;

                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@CodEmpresa", objcompany.CodCompany);
                cmd.Parameters.AddWithValue("@IdUbigeo", objcompany.IdUbi);
                cmd.Parameters.AddWithValue("@Ubigeo", objcompany.Ubi);
                cmd.Parameters.AddWithValue("@Ruc", objcompany.Ruc);

                cmd.Parameters.AddWithValue("@RazonSocial", objcompany.RazonSocial);
                cmd.Parameters.AddWithValue("@RazonComercial", objcompany.RazonComercial);
                cmd.Parameters.AddWithValue("@Telefono", objcompany.Telefono);
                cmd.Parameters.AddWithValue("@Fax", objcompany.Fax);
                cmd.Parameters.AddWithValue("@Direccion", objcompany.Direccion);
                cmd.Parameters.AddWithValue("@DomicilioFiscal", objcompany.DomicilioFiscal);
                cmd.Parameters.AddWithValue("@Urbanizacion", objcompany.Urbanizacion);
                cmd.Parameters.AddWithValue("@FechaRegistro", objcompany.FechaRegistro);
                cmd.Parameters.AddWithValue("@PaginaWeb", objcompany.PaginaWeb);
                cmd.Parameters.AddWithValue("@Email", objcompany.Email);
                cmd.Parameters.AddWithValue("@IdEstado", objcompany.IdEstado);
                cmd.Parameters.AddWithValue("@Id_TDI", objcompany.Id_TDI);
                cmd.Parameters.AddWithValue("@TpoLogin", objcompany.TpoLogin);

                cmd.Parameters.AddWithValue("@Url_CompanyLogo", objcompany.Url_CompanyLogo);
                cmd.Parameters.AddWithValue("@Url_CompanyConsult", objcompany.Url_CompanyConsult);

                //{
                //    int result = cmd.ExecuteNonQuery();
                //    msj = result == 0 ? "Error al Registrar" : "Registrado Correctamente";
                //}

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

                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: InsertCompany ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";


                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return result;
        }


        public int InsertUserRoot(User objUser)
        {
            int result = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_registrUserRoot;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdEstado", objUser.IdEstado);
                cmd.Parameters.AddWithValue("@IdEmpresa", objUser.IdCompany);
                cmd.Parameters.AddWithValue("@IdPerfil", objUser.IdPerfil);
                cmd.Parameters.AddWithValue("@Nombres", objUser.Nombres);

                cmd.Parameters.AddWithValue("@ApePaterno", objUser.ApePaterno);
                cmd.Parameters.AddWithValue("@ApeMaterno", objUser.ApeMaterno);
                cmd.Parameters.AddWithValue("@DNI_RUC", objUser.DNI);
                cmd.Parameters.AddWithValue("@Direccion", objUser.Direccion);
                cmd.Parameters.AddWithValue("@Telefono", objUser.Telefono);
                cmd.Parameters.AddWithValue("@Email", objUser.Email);
                cmd.Parameters.AddWithValue("@Username", objUser.Username);
                cmd.Parameters.AddWithValue("@Password", objUser.Password);
                cmd.Parameters.AddWithValue("@FechaExpiracion", objUser.FechaExpiracion);
                cmd.Parameters.AddWithValue("@FechaRegistro", objUser.FechaRegistro);
                cmd.Parameters.AddWithValue("@NameSede", objUser.Sede);

                //{
                //    int result = cmd.ExecuteNonQuery();
                //    msj = result == 0 ? "Error al Registrar" : "Registrado Correctamente";
                //}

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

                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: InsertUserRoot ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return result;
        }





        public string Insert_RolUserRoot(UserRol objroluser)
        {
            string msje = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_registrRolUserRoot;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdRol", objroluser.IdRol);
                cmd.Parameters.AddWithValue("@Dni", objroluser.Dni_Ruc);

                {
                    int result = cmd.ExecuteNonQuery();
                    msje = result == 0 ? "Error al Registrar" : "Registrado Correctamente";
                }

                //SqlDataReader dr;
                //dr = cmd.ExecuteReader();
                //if (dr.Read())
                //{
                //    int Id = int.Parse(dr[0].ToString().Trim());
                //    if (Id > 0)
                //    {
                //        cnn.Close();
                //        return Id;
                //    }
                //    else { cnn.Close(); }
                //}
                //else { cnn.Close(); }

                //cnn.Close();
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: Insert_RolUserRoot ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msje;
        }


        public string Insert_MenuPerfil(ListMenuPerfil objlistmenuperf)
        {
            string msje = string.Empty;
            string coma = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_registrmenuperfil;
                cmd.Connection = cnn;

                for (int i = 0; i <= objlistmenuperf.Count - 1; i++)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdMenu", objlistmenuperf[i].IdMenu);
                    cmd.Parameters.AddWithValue("@IdPerfil", objlistmenuperf[i].IdPerfil);

                    {
                        int result = cmd.ExecuteNonQuery();
                        var msj = result == 0 ? "Error al Registrar" : "Registrado Correctamente";

                        msje += coma + msj;
                        coma = ",";
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: Insert_MenuPerfil ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msje;
        }






        public ListMenu GetListMenu()
        {
            Menu omenu = new Menu();
            ListMenu oListMenu = new ListMenu();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_getlistmenu;
                cmd.Connection = cnn;

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdMenu = objReader.GetOrdinal("IdMenu");
                    int indexNombreMenu = objReader.GetOrdinal("NombreMenu");

                    while (objReader.Read())
                    {
                        omenu = new Menu();
                        omenu.IdMenu = DataUtil.DbValueToDefault<int>(objReader[indexIdMenu]);
                        omenu.NombreMenu = DataUtil.DbValueToDefault<string>(objReader[indexNombreMenu]);
                        oListMenu.Add(omenu);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListMenu ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListMenu;
        }




        //INSERTAMOS EL AMBIENTE ACTUAL
        public string Insert_AmbientTrabj(int idamb, string ruccompany)
        {
            string msje = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_registrambienttrabj;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdAmbTrabj", idamb);
                cmd.Parameters.AddWithValue("@RUCENTITY", ruccompany);

                {
                    int result = cmd.ExecuteNonQuery();
                    msje = result == 0 ? "Error al Registrar" : "Registrado Correctamente";
                }
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: Insert_AmbientTrabj ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msje;
        }


        //se obtiene los amb regitrados
        public ListAmbiente GetListAmbienteTrabaj()
        {
            Ambiente oamb = new Ambiente();
            ListAmbiente oListAmb = new ListAmbiente();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_getlistambient;
                cmd.Connection = cnn;

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdAmb = objReader.GetOrdinal("IDAMBIENTE");
                    int indexCod = objReader.GetOrdinal("COD");
                    int indexNameAmbiente = objReader.GetOrdinal("DESCRIPCION");

                    while (objReader.Read())
                    {
                        oamb = new Ambiente();
                        oamb.IdAmb = DataUtil.DbValueToDefault<int>(objReader[indexIdAmb]);
                        oamb.Cod = DataUtil.DbValueToDefault<string>(objReader[indexCod]);
                        oamb.NameAmbiente = DataUtil.DbValueToDefault<string>(objReader[indexNameAmbiente]);
                        oListAmb.Add(oamb);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListAmbienteTrabaj ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListAmb;
        }



        //se otbiene todos los documentos
        public ListTypeDocument GetListTypeDocument()
        {
            TypeDocument otypedoc = new TypeDocument();
            ListTypeDocument oListTypeDocument = new ListTypeDocument();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_getlisttypedocument;
                cmd.Connection = cnn;

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexId = objReader.GetOrdinal("IdTipoDocumento");
                    int indexCod = objReader.GetOrdinal("CodigoDocumento");
                    int indexName = objReader.GetOrdinal("Descripcion");

                    while (objReader.Read())
                    {
                        otypedoc = new TypeDocument();
                        otypedoc.Id = DataUtil.DbValueToDefault<int>(objReader[indexId]);
                        otypedoc.Codigo = DataUtil.DbValueToDefault<string>(objReader[indexCod]);
                        otypedoc.Descripcion = DataUtil.DbValueToDefault<string>(objReader[indexName]);
                        oListTypeDocument.Add(otypedoc);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetListTypeDocument ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListTypeDocument;
        }


        //INSERTAMOS LKAS CREDENCIALES DEL CERTIFICADO
        public string Insert_CredentialCertificateAmb(CredentialCertificate objcred)
        {
            string msje = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_insertcredentialcertificate;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@RucEntity", objcred.RucCompany);
                cmd.Parameters.AddWithValue("@NombreUsuario", objcred.NombreUsuario);
                cmd.Parameters.AddWithValue("@Password", objcred.Password);
                cmd.Parameters.AddWithValue("@IDAMBIENTE", objcred.IdAmb);

                {
                    int result = cmd.ExecuteNonQuery();
                    msje = result == 0 ? "Error al Registrar" : "Registrado Correctamente";
                }
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: Insert_CredentialCertificateAmb ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msje;
        }


        //se otbiene todos los roles
        public ListRol GetList_Roles()
        {
            Rol orol = new Rol();
            ListRol oListRol = new ListRol();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_getlistroles;
                cmd.Connection = cnn;

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexId = objReader.GetOrdinal("IdRol");
                    int indexName = objReader.GetOrdinal("NombreRol");
                    int indexCod = objReader.GetOrdinal("CodigoRol");

                    while (objReader.Read())
                    {
                        orol = new Rol();
                        orol.IdRol = DataUtil.DbValueToDefault<int>(objReader[indexId]);
                        orol.NombreRol = DataUtil.DbValueToDefault<string>(objReader[indexName]);
                        orol.CodigoRol = DataUtil.DbValueToDefault<string>(objReader[indexCod]);
                        oListRol.Add(orol);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetList_Roles ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListRol;
        }




        //se otbiene todos los perfiles
        public ListPerfil GetList_Perfil(string RucCompany)
        {
            Perfil operfil = new Perfil();
            ListPerfil oListPerfil = new ListPerfil();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_getlistperfil;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("RucEntity", RucCompany);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexId = objReader.GetOrdinal("IdPerfil");
                    int indexName = objReader.GetOrdinal("NombrePerfil");
                    int indexCod = objReader.GetOrdinal("Codigo");
                    int indexRucCompany = objReader.GetOrdinal("RucEntity");

                    while (objReader.Read())
                    {
                        operfil = new Perfil();
                        operfil.IdPerfil = DataUtil.DbValueToDefault<int>(objReader[indexId]);
                        operfil.NombrePerfil = DataUtil.DbValueToDefault<string>(objReader[indexName]);
                        operfil.Codigo = DataUtil.DbValueToDefault<string>(objReader[indexCod]);
                        operfil.RucCompany = DataUtil.DbValueToDefault<string>(objReader[indexRucCompany]);
                        oListPerfil.Add(operfil);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetList_Perfil ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oListPerfil;
        }




        public string Insert_ConfigMain(ConfigMain oConfigmain)
        {
            string msje = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_insert_configmain;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@TAB", oConfigmain.TAB);
                cmd.Parameters.AddWithValue("@NOM", oConfigmain.NOM);
                cmd.Parameters.AddWithValue("@POS", oConfigmain.POS);
                cmd.Parameters.AddWithValue("@VAL", oConfigmain.VAL);
                cmd.Parameters.AddWithValue("@MND", oConfigmain.MND);
                cmd.Parameters.AddWithValue("@DOC", oConfigmain.DOC);
                cmd.Parameters.AddWithValue("@MSG", oConfigmain.MSG);
                cmd.Parameters.AddWithValue("@ECV", oConfigmain.ECV);
                cmd.Parameters.AddWithValue("@ECN", oConfigmain.ECN);

                {
                    int result = cmd.ExecuteNonQuery();
                    msje = result == 0 ? "Error al Registrar" : "Registrado Correctamente";
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: Insert_ConfigMain ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msje;
        }




        public string Insert_MailCompany(Correo oMail)
        {
            string msj = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_insertmail;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdEmpresa", oMail.IdCompany);
                cmd.Parameters.AddWithValue("@Email", oMail.Email);
                cmd.Parameters.AddWithValue("@Password", oMail.Password);
                cmd.Parameters.AddWithValue("@DOMAIN", oMail.Domain);
                cmd.Parameters.AddWithValue("@IP", oMail.IP);
                cmd.Parameters.AddWithValue("@PORT", oMail.Port);
                cmd.Parameters.AddWithValue("@RucEmpresa", oMail.RucCompany);
                cmd.Parameters.AddWithValue("@IDESTADO", oMail.IdEstado);

                cmd.Parameters.AddWithValue("@TypeMail", oMail.TypeMail);
                cmd.Parameters.AddWithValue("@UseSSL", oMail.IdSSL);

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

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: Insert_MailCompany ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
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










        public ListCompany GetListCompany(int id)
        {
            Company obj = new Company();
            ListCompany oList = new ListCompany();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_registrcompany_ok;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@IdComp", id);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexIdCompany = objReader.GetOrdinal("IdEmpresa");
                    int indexRuc = objReader.GetOrdinal("Ruc");

                    while (objReader.Read())
                    {
                        obj = new Company();
                        obj.IdCompany = DataUtil.DbValueToDefault<int>(objReader[indexIdCompany]);
                        obj.Ruc = DataUtil.DbValueToDefault<string>(objReader[indexRuc]);
                        oList.Add(obj);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetListCompany ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oList;
        }


        #region register company

        public string RegisterPerf_Comp(Perfil oPerf)
        {
            string msje = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_registrperfilComp;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@Nomb", oPerf.NombrePerfil);
                cmd.Parameters.AddWithValue("@Cod", oPerf.Codigo);
                cmd.Parameters.AddWithValue("@RucComp", oPerf.RucCompany);

                {
                    int result = cmd.ExecuteNonQuery();
                    msje = result == 0 ? "Error al Registrar" : "Registrado Correctamente";
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: RegisterPerf_Comp ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msje;
        }


        public string RegisterCompany_Portal(Company objcompany, Correo omail)
        {
            string msj = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_registerComp;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@RucComp", objcompany.Ruc);
                cmd.Parameters.AddWithValue("@RazonSoc", objcompany.RazonSocial);
                cmd.Parameters.AddWithValue("@Dir", objcompany.Direccion);
                cmd.Parameters.AddWithValue("@Tel", objcompany.Telefono);

                cmd.Parameters.AddWithValue("@Email", omail.Email);
                cmd.Parameters.AddWithValue("@Pass", omail.Password);
                cmd.Parameters.AddWithValue("@Dom", omail.Domain);
                cmd.Parameters.AddWithValue("@IP", omail.IP);
                cmd.Parameters.AddWithValue("@Port", omail.Port);
                cmd.Parameters.AddWithValue("@Ssl", omail.IdSSL);

                {
                    int result = cmd.ExecuteNonQuery();
                    msj = result == 0 ? "Error al Registrar" : "Registrado Correctamente";
                }

                //SqlDataReader dr;
                //dr = cmd.ExecuteReader();
                //if (dr.Read())
                //{
                //    int Id = int.Parse(dr[0].ToString().Trim());
                //    if (Id > 0)
                //    {
                //        cnn.Close();
                //        return Id;
                //    }
                //    else { cnn.Close(); }
                //}
                //else { cnn.Close(); }

                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: RegisterCompany_Portal ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return msj;
        }




        public string RegisterUserCompany_Portal(User oUser)
        {
            string msj = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_registerUserComp;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@NroDoc", oUser.DNI);
                cmd.Parameters.AddWithValue("@RazonSoc", oUser.Nombres);
                cmd.Parameters.AddWithValue("@Email", oUser.Email);
                cmd.Parameters.AddWithValue("@User", oUser.Username);
                cmd.Parameters.AddWithValue("@Pass", oUser.Password);
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
                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: RegisterUserCompany_Portal ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
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


        #region GET LIST SERVICES WINDOWS

        public ListSevices GetList_TimeService()
        {
            Service obj = new Service();
            ListSevices oList = new ListSevices();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_getlist_timeservice;
                cmd.Connection = cnn;

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    int indexSubType = objReader.GetOrdinal("SubType");
                    int indexCodService = objReader.GetOrdinal("CodeService");
                    int indexNameService = objReader.GetOrdinal("NameService");

                    while (objReader.Read())
                    {
                        obj = new Service();
                        obj.SubType = DataUtil.DbValueToDefault<string>(objReader[indexSubType]);
                        obj.CodeService = DataUtil.DbValueToDefault<string>(objReader[indexCodService]);
                        obj.NameService = DataUtil.DbValueToDefault<string>(objReader[indexNameService]);
                        oList.Add(obj);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                logError.AppendLine(ex.Message);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: GetList_TimeService ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return oList;
        }


        public string Register_TimeServiceCompany(Service obj)
        {
            string msj = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_register_timeservicecompany;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@CodeService", obj.CodeService);
                cmd.Parameters.AddWithValue("@NameService", obj.NameService);
                cmd.Parameters.AddWithValue("@ValueTime", obj.ValueTime);
                cmd.Parameters.AddWithValue("@IntervalValue", obj.IntervalValue);
                cmd.Parameters.AddWithValue("@MaxNumAttempts", obj.MaxNumAttempts);
                cmd.Parameters.AddWithValue("@RucEntity", obj.RucEntity);
                cmd.Parameters.AddWithValue("@IdEstado", obj.IdEstado);
                cmd.Parameters.AddWithValue("@SubType", obj.SubType);

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
                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: Register_TimeServiceCompany ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
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


        #region Certificate Digital

        public string Register_CertificateDigital(CredentialCertificate obj)
        {
            string msj = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedimientos.usp_registercertificateinformation;
                cmd.Connection = cnn;

                cmd.Parameters.AddWithValue("@NameCertificate", obj.NameCertificate);
                cmd.Parameters.AddWithValue("@Pwd", obj.Password);
                cmd.Parameters.AddWithValue("@ExpirationDate", obj.ExpirationDate);
                cmd.Parameters.AddWithValue("@RucEntity", obj.RucCompany);

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
                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: Register_CertificateDigital ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = "PathLogSLINADE" + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                if (!Directory.Exists(PathDirectoryError))
                {
                    CrearNuevaCarpeta(PathDirectoryError);
                }

                if (listError.Count > 100)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADE.log", true, Encoding.UTF8))
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
    }
}
