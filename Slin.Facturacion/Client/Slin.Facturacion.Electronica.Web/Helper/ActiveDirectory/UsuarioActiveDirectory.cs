using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.DirectoryServices;
using System.Text;
using System.Configuration;

namespace Slin.Facturacion.Electronica.Web.Helper.ActiveDirectory
{
    public class UsuarioActiveDirectory
    {


        //void Lista()
        //{
        //    string Server = "Server2003";
        //    string ruta = "LDAP://" + Server + "/DC=test,DC=com";
        //    DirectoryEntry raiz = new DirectoryEntry();
        //    raiz.Path = ruta;
        //    raiz.AuthenticationType = AuthenticationTypes.Secure;
        //    raiz.Username = TextBox1.Text;
        //    raiz.Password = TextBox2.Text;

        //    string filtro = "sAMAccountName";
        //    string strSearch = filtro + "=" + TextBox1.Text;
        //    DirectorySearcher dsSystem = new DirectorySearcher(raiz, strSearch);
        //    dsSystem.SearchScope = SearchScope.Subtree;
        //    try
        //    {
        //        SearchResult srSystem = dsSystem.FindOne();
        //        Response.Write("Autenticacion Correcta");
        //    }
        //    catch (Exception error)
        //    {
        //        Response.Write(error.Message);
        //    }
        //}





        #region ACTIVE DIRECTORY METHOD

        private String _path;
        private String _filterAttribute;

        //public UsuarioActiveDirectory(String path)
        //{
        //    _path = path;
        //}

        public bool IsAuthenticated(String domain, String username, String pwd)
        {

            String domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);

            try
            {	//Bind to the native AdsObject to force authentication.			
                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (null == result)
                {
                    return false;
                }

                //Update the new path to the user in the directory.
                _path = result.Path;
                _filterAttribute = (String)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user. " + ex.Message);
            }
            return true;
        }

        public String GetGroups()
        {
            DirectorySearcher search = new DirectorySearcher(_path);
            search.Filter = "(cn=" + _filterAttribute + ")";
            search.PropertiesToLoad.Add("memberOf");
            StringBuilder groupNames = new StringBuilder();

            try
            {
                SearchResult result = search.FindOne();

                int propertyCount = result.Properties["memberOf"].Count;

                String dn;
                int equalsIndex, commaIndex;

                for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                {
                    dn = (String)result.Properties["memberOf"][propertyCounter];

                    equalsIndex = dn.IndexOf("=", 1);
                    commaIndex = dn.IndexOf(",", 1);
                    if (-1 == equalsIndex)
                    {
                        return null;
                    }

                    groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                    groupNames.Append("|");

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error obtaining group names. " + ex.Message);
            }
            return groupNames.ToString();
        }

        #endregion




        #region OTHER METHOD 

        private void LoginLDAP(string domain, string user, string pwd)
        {
            string error = string.Empty;

            try
            {
                foreach (string key in ConfigurationSettings.AppSettings.Keys)
                {
                    domain = key.Contains("DirectoryDomain") ? ConfigurationSettings.AppSettings[key] : domain;
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

    }
}