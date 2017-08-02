using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.DirectoryServices;

namespace Authenticate_LDAP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ClearText();
        }






        #region entity

        private string _filterAttribute;

        #endregion





        #region method

        private void ClearText()
        {
            txtactive_directoryName.Clear();
            txtpassword.Clear();
            txtuser.Clear();

            txtactive_directoryName.Text = "";
        }





        

        private void Connect()
        {
            
        }


        public bool ValidarActiveDirectory()
        {
            bool result = false;
            string server = txtactive_directoryName.Text;

            
            using (DirectoryEntry _entry = new DirectoryEntry())
            {
                _entry.Username = txtuser.Text;
                _entry.Password = txtpassword.Password;
                _entry.Path = "LDAP://" + server + "/DC=test,DC=com";
                DirectorySearcher _searcher = new DirectorySearcher(_entry);
                _searcher.Filter = "(objectclass=user)";
                try
                {
                    SearchResult _sr = _searcher.FindOne();
                    string _name = _sr.Properties["displayname"][0].ToString();
                    result = true;

                    MessageBox.Show("Message: Ok te has conectado");
                }
                catch (Exception ex)
                {
                    /* Error handling omitted to keep code short: remember to handle exceptions !*/
                    //string path = ConfigurationManager.AppSettings["PathLogSLINADE"];
                    //using (StreamWriter sw = new StreamWriter(path + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\LDAP.log", true, Encoding.UTF8))
                    //{
                    //    sw.WriteLine("[" + DateTime.Now + "] Message: " + ex.Message + ", InnerException: " + ex.InnerException);
                    //}
                    MessageBox.Show("Message: " + ex.Message + ", innerexception: " + ex.InnerException);

                    if (ex.Message == "El servidor ha devuelto una referencia.\r\n")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    //return false;
                    //Response.Write("<script type=text/javascript>alert('" + ex.Message + "')</script>");
                }
            }
            return result;
        }


        public bool ActiveDirectoryAuthenticate(string username, string password)
        {
            bool result = false;
            using (DirectoryEntry _entry = new DirectoryEntry())
            {
                _entry.Username = username;
                _entry.Password = password;
                DirectorySearcher _searcher = new DirectorySearcher(_entry);
                _searcher.Filter = "(objectclass=user)";
                try
                {
                    SearchResult _sr = _searcher.FindOne();
                    string _name = _sr.Properties["displayname"][0].ToString();
                    result = true;
                }
                catch
                { /* Error handling omitted to keep code short: remember to handle exceptions !*/ }
            }

            return result; //true = user authenticated!


        }




        public bool IsAuthenticated(String domain, String username, String pwd)
        {
            String domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(txtactive_directoryName.Text, domainAndUsername, pwd);

            try
            {//Bind to the native AdsObject to force authentication.
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
                txtactive_directoryName.Text = result.Path;
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
            DirectorySearcher search = new DirectorySearcher(txtactive_directoryName.Text);
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


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //GetGroups();
            //IsAuthenticated(txtactive_directoryName.Text, txtuser.Text, txtpassword.Password);

            ValidarActiveDirectory();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            new Slin.Facturacion.ExchangeRate.ProcessClass().Process_WithCondition();
        }

        private void cboactivedirectory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
