using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.ExchangeRate
{
    public class ExchangeRateClass
    {
        public string Url_ExchangeRate_Snt = ConfigurationManager.AppSettings["Url_ExchangeRate_Snt"].ToString();

        public List<string> GetList_ChangeType(string param_mes, string param_anio)
        {


            List<string> listChangeType = new System.Collections.Generic.List<string>();

            listChangeType = new System.Collections.Generic.List<string>();

            try
            {
                // Dim sUrl As String = "http://www.sunat.gob.pe/cl-at-ittipcam/tcS01Alias"
                // Dim sUrl As String = "http://www.sunat.gob.pe/cl-at-ittipcam/tcS01Alias?mes=02&anho=2016"
                string MESS = param_mes;

                //string sUrl = "http://www.sunat.gob.pe/cl-at-ittipcam/tcS01Alias";
                string sUrl = Url_ExchangeRate_Snt;
                string mes = "?mes=";
                string anio = "&anho=";
                string SUrl2 = sUrl + mes + MESS + anio + param_anio;

                Encoding objEncoding = Encoding.GetEncoding("ISO-8859-1");
                CookieCollection objCookies = new CookieCollection();
                HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(SUrl2);
                getRequest.Method = "GET";
                getRequest.CookieContainer = new CookieContainer();
                getRequest.CookieContainer.Add(objCookies);
                string sGetResponse = string.Empty;
                using (HttpWebResponse getResponse = (HttpWebResponse)getRequest.GetResponse())
                {
                    objCookies = getResponse.Cookies;
                    using (StreamReader srGetResponse = new StreamReader(getResponse.GetResponseStream(), objEncoding))
                    {
                        sGetResponse = srGetResponse.ReadToEnd();
                    }
                }

                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(sGetResponse);
                HtmlNodeCollection NodesTr = document.DocumentNode.SelectNodes("//table[@class='class=\"form-table\"']//tr");
                if (NodesTr != null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Dia", typeof(String));
                    dt.Columns.Add("Compra", typeof(String));
                    dt.Columns.Add("Venta", typeof(String));
                    //Cbaño.text.add ("dia, typeof(String));
                    int iNumFila = 0;
                    foreach (HtmlNode Node in NodesTr)
                    {
                        if (iNumFila > 0)
                        {
                            int iNumColumna = 0;
                            DataRow dr = dt.NewRow();
                            foreach (HtmlNode subNode in Node.Elements("td"))
                            {
                                if (iNumColumna == 0)
                                {
                                    dr = dt.NewRow();
                                }
                                string sValue = subNode.InnerHtml.ToString().Trim();
                                sValue = System.Text.RegularExpressions.Regex.Replace(sValue, "<.*?>", " ");
                                //dr(iNumColumna) = sValue;
                                iNumColumna += 1;
                                if (iNumColumna == 3)
                                {
                                    dt.Rows.Add(dr);
                                    iNumColumna = 0;
                                }

                                listChangeType.Add(sValue);

                            }
                        }
                        iNumFila += 1;
                    }
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine("Message: " + ex.Message);
            }

            return listChangeType;





        }






        

    }

}
