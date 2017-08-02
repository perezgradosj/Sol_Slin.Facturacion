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
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.DataAccess
{
    public class InterfaceDataAccess
    {
        static string PathLogSLINADE = Conexion.Cadena;

        static string cadena = "Server=" + Conexion.Host + ";Database=" + Conexion.BD + ";User=" + Conexion.USER + ";pwd=" + Conexion.PWD;


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


        public List<ERegex> getRegex()
        {
            List<ERegex> Validaciones = new List<ERegex>();
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    using (SqlCommand cmd = new SqlCommand("[Fact].[Usp_ObtieneRegex]", con))
                    {
                        SqlDataReader dr;
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            ERegex Val = new ERegex();
                            Val.KEY = dr[0].ToString().Trim();
                            Val.NOM = dr[1].ToString().Trim();
                            Val.VAL = dr[2].ToString().Trim();
                            Val.MND = dr[3].ToString().Trim();
                            Val.DOC = dr[4].ToString().Trim();
                            Val.TAB = dr[5].ToString().Trim();
                            Val.MSG = dr[6].ToString().Trim();
                            Validaciones.Add(Val);
                            //var reponse = Validaciones.Find(r => r.ID_NOMBRE_TABLA == "CABECERA-PRINCIPAL");
                        }
                        con.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(" " + ex.Message);

                #region
                listError = new List<string>();
                listError.Add("[" + DateTime.Now + "] [ SEGURIDAD - MethodName: getRegex ] " + ex.Message + " " + ex.InnerException);

                string PathDirectoryError = PathLogSLINADE + DateTime.Now.Year + "-" + string.Format("{0:00}",DateTime.Now.Month) + @"\";

                //if (!Directory.Exists(PathDirectoryError))
                //{
                //    CrearNuevaCarpeta(PathDirectoryError);
                //}

                Singleton.Instance.CreateDirectory(PathDirectoryError);

                if (listError.Count > 0)
                {
                    foreach (var line in listError)
                    {
                        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Interf_log_WS.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                #endregion
            }
            return Validaciones;
        }
    }
}
