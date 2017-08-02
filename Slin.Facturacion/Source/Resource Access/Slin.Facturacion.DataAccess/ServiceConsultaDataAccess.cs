using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.DataAccess.Helper;


using System.IO;
using System.Xml.Serialization;
using Slin.Facturacion.Common;
using Slin.Facturacion.Common.Method;

namespace Slin.Facturacion.DataAccess
{
    public class ServiceConsultaDataAccess
    {

        static string PathLogSLINADE = Conexion.Cadena;
        static string PathLog_NotLicense = Conexion.Cadena_Nl;
        static string cadena = "Server=" + Conexion.Host + ";Database=" + Conexion.BD + ";User=" + Conexion.USER + ";pwd=" + Conexion.PWD;
        SqlConnection cnn = new SqlConnection(cadena);

        #region FROM DATABASE

        private void WriteLog_Service(string msje)
        {
            using (StreamWriter sw = new StreamWriter(Path_TologConsult, true, Encoding.UTF8))
            {
                sw.WriteLine(msje);
            }
        }

        private void WriteLog_Process(string msje)
        {
            Singleton.Instance.CreateDirectory(PathLogSLINADE);
            using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "Fact_ConsultDocumentDA.log", true, Encoding.UTF8))
            {
                sw.WriteLine(msje);
            }
        }

        private void WriteLog_NotLicense(string msje)
        {
            Singleton.Instance.CreateDirectory(PathLog_NotLicense);
            using (StreamWriter sw = new StreamWriter(PathLog_NotLicense + "ws_consult_nl.log", true, Encoding.UTF8))
            {
                sw.WriteLine(msje);
            }
        }


        string PathCompany = string.Empty;

        string Path_TologConsult = string.Empty;

        public string GetDocumentoXML(string NUM_CPE)
        {
            Singleton.Instance.CreateDirectory(PathLogSLINADE);

            string response = string.Empty;
            string forbase64 = string.Empty;

            byte[] strData = { };

            try
            {
                var EntityIdReceived = NUM_CPE.Substring(0, 11);
                var listEntity = new Slin.Facturacion.BusinessSecurity.Entity.EntityClass().getsListEntity();

                if (listEntity.Contains(EntityIdReceived))
                {

                    string[] array = NUM_CPE.Split('-');

                    PathCompany = ConfigurationManager.AppSettings[array[0]].ToString();

                    //var PathXml_Company = PathCompany + @"ProcesoCE\XML\";

                    Path_TologConsult = PathCompany + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\ser_consult\";
                    Singleton.Instance.CreateDirectory(Path_TologConsult);
                    Path_TologConsult = PathCompany + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\ser_consult\ws_consult.log";

                    WriteLog_Service(" ");
                    WriteLog_Service(Constantes.MsjStart);
                    WriteLog_Service(Constantes.Doc_inConsult + NUM_CPE);
                    WriteLog_Service(Constantes.Msje_FileConsult_xml);

                    #region HAVE
                    try
                    {
                        SqlCommand oDbCommand = new SqlCommand();

                        cnn.Open();
                        oDbCommand.CommandType = CommandType.StoredProcedure;
                        oDbCommand.CommandText = Procedimientos.Usp_GetDocumentoXML;
                        oDbCommand.Connection = cnn;

                        oDbCommand.Parameters.AddWithValue("@NUM_CPE", NUM_CPE);

                        using (IDataReader objReader = oDbCommand.ExecuteReader())
                        {
                            int indexXML = objReader.GetOrdinal("XML");

                            while (objReader.Read())
                            {
                                byte[] byteData = (byte[])objReader[indexXML];
                                forbase64 = Encoding.GetEncoding("iso-8859-1").GetString(byteData);

                                strData = (byte[])objReader[indexXML];
                            }
                        }
                        cnn.Close();

                        if (forbase64.Length < 50)
                        {
                            string sAscii = "<Respuesta>No Contiene un xml</Respuesta>";
                            strData = Encoding.GetEncoding("iso-8859-1").GetBytes(sAscii);
                            response = Convert.ToBase64String(strData);

                            WriteLog_Service(Constantes.Msj_XmlNoExists);
                        }
                        else
                        {
                            WriteLog_Service(Constantes.Msj_TpoDoc + array[1]);

                            response = Convert.ToBase64String(strData);
                            WriteLog_Service(Constantes.Msj_Response_xmlBase64);
                        }
                    }
                    catch (Exception ex)
                    {
                        cnn.Close();

                        Singleton.Instance.CreateDirectory(PathLogSLINADE);
                        WriteLog_Service("[" + DateTime.Now + "] [ FACTURACIÓN(WS_GetDocument) - MethodName: GetDocumentoXML ] " + ex.Message);

                        string sAscii = "<Respuesta> " + ex.Message + ", " + ex.InnerException + "</Respuesta>";
                        strData = Encoding.GetEncoding("iso-8859-1").GetBytes(sAscii);
                        response = Convert.ToBase64String(strData);
                    }
                    #endregion

                    WriteLog_Service(Constantes.MsjEndProcessDoc + NUM_CPE);
                    WriteLog_Service(Constantes.MsjEnd);
                }
                else
                {
                    string sAscii = "<Respuesta>El Ruc Enviado no Corresponde.</Respuesta>";
                    strData = Encoding.GetEncoding("iso-8859-1").GetBytes(sAscii);
                    response = Convert.ToBase64String(strData);

                    WriteLog_NotLicense("[" + DateTime.Now + "] El Ruc Enviado no Corresponde: " + NUM_CPE);
                    WriteLog_NotLicense(Constantes.MsjEndProcessDoc + NUM_CPE);
                    WriteLog_NotLicense(Constantes.MsjEnd);
                }
            }
            catch (Exception ex)
            {
                string sAscii = "<Respuesta> " + ex.Message + "</Respuesta>";
                strData = Encoding.GetEncoding("iso-8859-1").GetBytes(sAscii);
                response = Convert.ToBase64String(strData);
                WriteLog_Process(" ");
                WriteLog_Process(Constantes.MsjStart);
                WriteLog_Process(Constantes.Doc_inConsult + NUM_CPE);
                WriteLog_Process(Constantes.Value_DateToLog + "Consulta de XML");
                WriteLog_Process("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetStatusDocument ] " + sAscii  + ", Doc. : " + NUM_CPE);
                WriteLog_Process(Constantes.MsjEndProcessDoc + NUM_CPE);
                WriteLog_Process(Constantes.MsjEnd);
            }
            return response;
        }


        

        //private void Singleton.Instance.CreateDirectory(string path)
        //{
        //    if (!System.IO.Directory.Exists(path))
        //    {
        //        System.IO.Directory.Singleton.Instance.CreateDirectory(path);
        //    }
        //}


        public string GetDocumentoPDF(string NUM_CPE)
        {
            Singleton.Instance.CreateDirectory(PathLogSLINADE);
            string response = string.Empty;
            string forbase64 = string.Empty;

            string xmline = string.Empty;
            string tpodoc = string.Empty;
            string typeformat = string.Empty;


            byte[] strData = { };

            try
            {
                var EntityIdReceived = NUM_CPE.Substring(0, 11);
                var listEntity = new Slin.Facturacion.BusinessSecurity.Entity.EntityClass().getsListEntity();

                if (listEntity.Contains(EntityIdReceived))
                {

                    string[] array = NUM_CPE.Split('-');

                    PathCompany = ConfigurationManager.AppSettings[array[0]].ToString();

                    //var PathXml_Company = PathCompany + @"ProcesoCE\XML\";
                    //var PathPdf_Company = PathCompany + @"ProcesoCE\PDF\";

                    Path_TologConsult = PathCompany + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\ser_consult\";
                    Singleton.Instance.CreateDirectory(Path_TologConsult);
                    Path_TologConsult = PathCompany + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\ser_consult\ws_consult.log";


                    WriteLog_Service(" ");
                    WriteLog_Service(Constantes.MsjStart);
                    WriteLog_Service(Constantes.Doc_inConsult + NUM_CPE);
                    WriteLog_Service(Constantes.Msje_FileConsult_pdf);

                    #region HAVE

                    try
                    {
                        SqlCommand oDbCommand = new SqlCommand();

                        cnn.Open();
                        oDbCommand.CommandType = CommandType.StoredProcedure;
                        oDbCommand.CommandText = Procedimientos.Usp_GetDocumentoXML;
                        oDbCommand.Connection = cnn;

                        oDbCommand.Parameters.AddWithValue("@NUM_CPE", NUM_CPE);

                        using (IDataReader objReader = oDbCommand.ExecuteReader())
                        {
                            int indexTpoDoc = objReader.GetOrdinal("TPO_CPE");
                            int indexXML = objReader.GetOrdinal("XML");
                            int indexTypeFormat = objReader.GetOrdinal("TypeFormat");

                            while (objReader.Read())
                            {
                                tpodoc = DataUtil.DbValueToDefault<string>(objReader[indexTpoDoc]);
                                byte[] byteData = (byte[])objReader[indexXML];
                                xmline = Encoding.GetEncoding("iso-8859-1").GetString(byteData);

                                strData = (byte[])objReader[indexXML];

                                typeformat = DataUtil.DbValueToDefault<string>(objReader[indexTypeFormat]);
                            }
                        }
                        cnn.Close();

                        if (xmline.Length < 50)
                        {
                            //string sAscii = "<No Contiene un XML/>";
                            string sAscii = "<Respuesta>No Contiene un xml</Respuesta>";
                            strData = Encoding.GetEncoding("iso-8859-1").GetBytes(sAscii);

                            response = Convert.ToBase64String(strData);

                            WriteLog_Service(Constantes.Msj_XmlNoExists);
                        }
                        else
                        {
                            WriteLog_Service(Constantes.Msj_TpoDoc + array[1]);
                            strData = new ServiceTransform().GetPDFDocFromXmlLine(xmline, tpodoc, NUM_CPE, typeformat);
                            response = Convert.ToBase64String(strData);
                        }
                    }
                    catch (Exception ex)
                    {
                        cnn.Close();
                        WriteLog_Service("[" + DateTime.Now + "] [ FACTURACIÓN(WS_GetDocument) - MethodName: GetDocumentoXML ] " + ex.Message);

                        string sAscii = "<Respuesta> " + ex.Message + ", " + ex.InnerException + "</Respuesta>";
                        strData = Encoding.GetEncoding("iso-8859-1").GetBytes(sAscii);
                        response = Convert.ToBase64String(strData);
                    }
                    #endregion END HAVE


                    WriteLog_Service(Constantes.MsjEndProcessDoc + NUM_CPE);
                    WriteLog_Service(Constantes.MsjEnd);
                }
                else
                {
                    string sAscii = "<Respuesta>El Ruc Enviado no Corresponde.</Respuesta>";
                    strData = Encoding.GetEncoding("iso-8859-1").GetBytes(sAscii);
                    response = Convert.ToBase64String(strData);


                    WriteLog_NotLicense("[" + DateTime.Now + "] El Ruc Enviado no Corresponde: " + NUM_CPE);
                    WriteLog_NotLicense(Constantes.MsjEndProcessDoc + NUM_CPE);
                    WriteLog_NotLicense(Constantes.MsjEnd);
                }
            }
            catch (Exception ex)
            {
                string sAscii = "<Respuesta> " + ex.Message + "</Respuesta>";
                strData = Encoding.GetEncoding("iso-8859-1").GetBytes(sAscii);
                response = Convert.ToBase64String(strData);
                WriteLog_Process(" ");
                WriteLog_Process(Constantes.MsjStart);
                WriteLog_Process(Constantes.Doc_inConsult + NUM_CPE);
                WriteLog_Process(Constantes.Value_DateToLog + "Consulta de PDF");
                WriteLog_Process("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetStatusDocument ] " + sAscii + ", Doc. : " + NUM_CPE);
                WriteLog_Process(Constantes.MsjEndProcessDoc + NUM_CPE);
                WriteLog_Process(Constantes.MsjEnd);
            }
            return response;
        }

        #endregion






        #region CONSULTA ESTADO DOCUMENTO


        public string GetStatusDocument(string NUM_CE)
        {
            string response = string.Empty;
            string palote = string.Empty;
            Singleton.Instance.CreateDirectory(PathLogSLINADE);

            try
            {
                var EntityIdReceived = NUM_CE.Substring(0, 11);
                var listEntity = new Slin.Facturacion.BusinessSecurity.Entity.EntityClass().getsListEntity();

                if (listEntity.Contains(EntityIdReceived))
                {
                    string[] array = NUM_CE.Split('-');

                    PathCompany = ConfigurationManager.AppSettings[array[0]].ToString();

                    Path_TologConsult = PathCompany + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\ser_consult\";
                    Singleton.Instance.CreateDirectory(Path_TologConsult);
                    Path_TologConsult = PathCompany + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\ser_consult\ws_consult.log";


                    WriteLog_Service(" ");
                    WriteLog_Service(Constantes.MsjStart);
                    WriteLog_Service(Constantes.Doc_inConsult + NUM_CE);
                    WriteLog_Service(Constantes.Msj_TpoDoc + array[1]);
                    WriteLog_Service(Constantes.Msje_StatusConsult);

                    var listresponse = GetStatusDocumentDA(NUM_CE);

                    string Param_status = string.Empty;
                    if (listresponse.Count > 0)
                    {
                        int cont = Constantes.ValorCero;
                        foreach (var et in listresponse)
                        {
                            cont++;
                            response += palote + et;
                            palote = "|";
                            
                            if (cont == Constantes.ValorTres)
                            {
                                Param_status = et;
                            }
                        }
                        //response += string.Join(",", listresponse);
                        
                        WriteLog_Service(Constantes.Value_DateToLog + "El estado del Doc. es   : " + Param_status);
                    }
                    else
                    {
                        response = "El Documento|" + NUM_CE + "|No Existe";

                        WriteLog_Service(Constantes.Value_DateToLog + "El Doc. consultado no existe.");
                    }

                    WriteLog_Service(Constantes.MsjEndProcessDoc + NUM_CE);
                    WriteLog_Service(Constantes.MsjEnd);
                }
                else
                {

                    response = "El Ruc Enviado|" + NUM_CE + "|No Corresponde.";
                    WriteLog_NotLicense(" ");
                    WriteLog_NotLicense(Constantes.MsjStart);
                    WriteLog_NotLicense(Constantes.Value_DateToLog + "El Ruc Enviado no Corresponde: ");
                    WriteLog_NotLicense(Constantes.MsjEndProcessDoc + NUM_CE);
                    WriteLog_NotLicense(Constantes.MsjEnd);
                }
            }
            catch (Exception ex)
            {
                Singleton.Instance.CreateDirectory(PathLogSLINADE);
                WriteLog_Process(" ");
                WriteLog_Process(Constantes.MsjStart);
                WriteLog_Process(Constantes.Doc_inConsult + NUM_CE);
                WriteLog_Process(Constantes.Value_DateToLog + "Consulta de Estado");
                WriteLog_Process("[" + DateTime.Now + "] [ FACTURACIÓN(WS_GetDocument) - MethodName: GetDocumentoXML ] " + ex.Message + ", Doc. : " + NUM_CE);
                WriteLog_Process(Constantes.MsjEndProcessDoc + NUM_CE);
                WriteLog_Process(Constantes.MsjEnd);
                response = "Error|Message: " + ex.Message;
            }
            
            return response;
        }


        public List<string> GetStatusDocumentDA(string NUM_CE)
        {
            List<string> listresponse = new List<string>();

            try
            {

                SqlCommand oDbCommand = new SqlCommand();

                cnn.Open();
                oDbCommand.CommandType = CommandType.StoredProcedure;
                oDbCommand.CommandText = Procedimientos.Usp_GetStatusDocument;
                oDbCommand.Connection = cnn;

                oDbCommand.Parameters.AddWithValue("@NUM_CE", NUM_CE);

                using (IDataReader objReader = oDbCommand.ExecuteReader())
                {
                    int indexNum_CE = objReader.GetOrdinal("NUM_CE");
                    int indexCodEst = objReader.GetOrdinal("Cod");
                    int indexEstado = objReader.GetOrdinal("Estado");

                    while (objReader.Read())
                    {
                        listresponse.Add(string.Empty + DataUtil.DbValueToDefault<string>(objReader[indexNum_CE]));
                        listresponse.Add(string.Empty + DataUtil.DbValueToDefault<string>(objReader[indexCodEst]));
                        listresponse.Add(string.Empty + DataUtil.DbValueToDefault<string>(objReader[indexEstado]));
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();

                Singleton.Instance.CreateDirectory(PathLogSLINADE);
                WriteLog_Process(" ");
                WriteLog_Process(Constantes.MsjStart);
                WriteLog_Process(Constantes.Doc_inConsult + NUM_CE);
                WriteLog_Process(Constantes.Value_DateToLog + "Consulta de Estado");
                WriteLog_Process("[" + DateTime.Now + "] [ FACTURACIÓN - MethodName: GetStatusDocument ] " + ex.Message);
                WriteLog_Process(Constantes.MsjEndProcessDoc + NUM_CE);
                WriteLog_Process(Constantes.MsjEnd);
            }
            return listresponse;
        }


        #endregion

        #region FROM DIRECTORY
        List<string> lista = new List<string>();
        List<string> listaPDF = new List<string>();


        //string NombreArchivo = string.Empty;    

        public string GetDocumentoXMLDir(string num_cpe)
        {
            string response = string.Empty;
            string xmlreceipt = string.Empty;

            try
            {
                var docxml = new Helper.ServiceTransformDir().GetListDocumentConsult(num_cpe);

                if (docxml.IdEstado == 7.ToString())
                {
                    xmlreceipt = new ServiceTransformDir().GetXmlFromDir(docxml.Num_CPE);
                }

                if (xmlreceipt.Length < 50)
                {
                    string sAscii = "<Respuesta>No Contiene un xml</Respuesta>";
                    byte[] strData = Encoding.GetEncoding("iso-8859-1").GetBytes(sAscii);
                    response = Convert.ToBase64String(strData);
                }
                else
                {
                    byte[] strData = Encoding.GetEncoding("iso-8859-1").GetBytes(xmlreceipt);
                    response = Convert.ToBase64String(strData);
                }
            }
            catch (Exception ex)
            {
                //cnn.Close();
                ////logError.AppendLine(ex.Message);

                //#region
                //listError = new List<string>();
                //listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN(WS_GetDocument) - MethodName: GetDocumentoXMLDir ] " + ex.Message + " " + ex.InnerException);

                //string PathDirectoryError = PathLogSLINADE + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                //if (!Directory.Exists(PathDirectoryError))
                //{
                //    CrearNuevaCarpeta(PathDirectoryError);
                //}

                //if (listError.Count > 0)
                //{
                //    foreach (var line in listError)
                //    {
                //        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Fact_ConsultDocumentDA.log", true, Encoding.UTF8))
                //        {
                //            sw.WriteLine(line);
                //        }
                //    }
                //}
                //#endregion
            }
            return response;


            //foreach (var docxml in ListDocXml.ListDocumento)
            //{
            //    xmlreceipt = new ServiceTransformDir().GetXmlFromDir(docxml.Num_CPE);
            //}

            //DirectoryInfo di = new DirectoryInfo("");
            //foreach (var fi in di.GetFiles("*", SearchOption.TopDirectoryOnly))
            //foreach (var fi in di.GetFiles("*.xml*"))
            //{
            //    listaPDF.Add(fi.Name);
            //}
        }


        public string GetDocumentoPDFDir(string NUM_CPE)
        {
            string response = string.Empty;
            string xmlreceipt = string.Empty;

            //byte[] strData = { };
            try
            {
                var docxml = new Helper.ServiceTransformDir().GetListDocumentConsult(NUM_CPE);

                if (docxml.IdEstado == 7.ToString())
                {
                    xmlreceipt = new ServiceTransformDir().GetXmlFromDir(docxml.Num_CPE);
                }

                if (xmlreceipt.Length < 50)
                {
                    string sAscii = "<Respuesta>No Contiene un xml</Respuesta>";
                    byte[] strData = Encoding.GetEncoding("iso-8859-1").GetBytes(sAscii);
                    response = Convert.ToBase64String(strData);
                }
                else
                {
                    byte[] strData = new ServiceTransform().GetPDFDocFromXmlLine(xmlreceipt, docxml.Tpo_CPE, NUM_CPE, "");
                    response = Convert.ToBase64String(strData);
                }

            }
            catch (Exception ex)
            {
                //cnn.Close();
                //logError.AppendLine(ex.Message);

                //#region
                //listError = new List<string>();
                //listError.Add("[" + DateTime.Now + "] [ FACTURACIÓN(WS_GetDocument) - MethodName: GetDocumentoPDFDir ] " + ex.Message + " " + ex.InnerException);

                //string PathDirectoryError = PathLogSLINADE + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

                //if (!Directory.Exists(PathDirectoryError))
                //{
                //    CrearNuevaCarpeta(PathDirectoryError);
                //}

                //if (listError.Count > 0)
                //{
                //    foreach (var line in listError)
                //    {
                //        using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Fact_ConsultDocumentDA.log", true, Encoding.UTF8))
                //        {
                //            sw.WriteLine(line);
                //        }
                //    }
                //}
                //#endregion
            }
            return response;
        }

        #endregion
    }
}
