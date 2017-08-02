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
using System.Windows.Shapes;

//using Slin.Facturacion.BusinessEntities;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using System.IO;
using System.Diagnostics;
using System.Timers;
using System.Threading;
using Slin.Facturacion.Common;
using LinqToExcel;
using Slin.Facturacion.InstallCommon;

namespace Slin.Facturacion.Install.Views.Control
{
    /// <summary>
    /// Interaction logic for DBSettings.xaml
    /// </summary>
    public partial class ViewDBSettings : Window
    {
        public ViewDBSettings(Company oemp, Correo mailSend, Correo mailReceived, CredentialCertificate oCertificate)
        {
            InitializeComponent();
            StartCharger(oemp, mailSend, mailReceived, oCertificate);
        }


        #region ENTITY
        public CredentialCertificate oCert;

        public Company oEmpresa;
        public User oUser;
        public UserRol oUserRol;
        public ListUserRol objListUserRol;
        public MenuPerfil oMenuPerfil;
        public ListMenuPerfil objListMenuPerfil;
        public Correo oMailSend;
        public Correo oMailReceived;

        private string ParamHost = string.Empty;
        private string ParamDBName = string.Empty;
        private string ParamUser = string.Empty;
        private string ParamPwd = string.Empty;

        public int Param_IdCompanyRegistr = Constantes.ValorCero;

        #endregion


        #region METHOD

        private void StartCharger(Company oemp, Correo mailsend, Correo mailreceived, CredentialCertificate ocertificate)
        {
            ReceivedObjectEnterprise(oemp, mailsend, mailreceived, ocertificate);
            //txtbdname.Text = "BD_Fact_Install";
            //txtbdname.IsEnabled = false;

            cboSqlVersion.ItemsSource = new InstallCommon.Util.Util().GetListVersionSQLInstalled();
            cboSqlVersion.SelectedIndex = 1;

            cboSqlVersion_port.ItemsSource = new InstallCommon.Util.Util().GetListVersionSQLInstalled();
            cboSqlVersion_port.SelectedIndex = 1;

            ClearTextBox();

            //txthost.Text = "192.168.90.8";
            //txtusuariobd.Text = "sa";
            //txtpassword.Password = "C0rporaci0n";

            //txthost_port.Text = "192.168.90.8";
            //txtusuariobd_port.Text = "sa";
            //txtpassword_port.Password = "C0rporaci0n";
        }

        private void ClearTextBox()
        {
            txthost.Clear();
            txtusuariobd.Clear();
            txtpassword.Clear();
            txthost.Clear();
            txtusuariobd_port.Clear();
            txtpassword_port.Clear();
            txtbdnameportal.Clear();
        }

        private void ReceivedObjectEnterprise(Company oemp, Correo mailsend, Correo mailreceived, CredentialCertificate ocertificate)
        {
            oEmpresa = new Company();
            oMailSend = new Correo();
            oMailReceived = new Correo();
            oCert = new CredentialCertificate();
            //chekear aqui
            oEmpresa = oemp;

            oMailSend = mailsend;
            oMailReceived = mailreceived;
            oCert = ocertificate;


            //oEmpresa.CodCompany = oemp.CodCompany;
            //oEmpresa.Ubi = oemp.Ubi;
            //oEmpresa.Ruc = oemp.Ruc;
            //oEmpresa.RazonSocial = oemp.RazonSocial;
            //oEmpresa.RazonComercial = oemp.RazonComercial;
            //oEmpresa.Telefono = oemp.Telefono;
            //oEmpresa.Fax = oemp.Fax;
            //oEmpresa.Direccion = oemp.Direccion;
            //oEmpresa.DomicilioFiscal = oemp.DomicilioFiscal;
            //oEmpresa.Urbanizacion = oemp.Urbanizacion;
            //oEmpresa.FechaRegistro = oemp.FechaRegistro;
            //oEmpresa.PaginaWeb = oemp.PaginaWeb;
            //oEmpresa.Email = oemp.Email;
            //oEmpresa.IdEstado = oemp.IdEstado;
            //oEmpresa.Id_TDI = oemp.Id_TDI;
            //oEmpresa.TpoLogin = oemp.TpoLogin;
            //oEmpresa.UserAdmin = oemp.UserAdmin;

            //oMailSend.Email = mailsend.Email;
            //oMailSend.Password = mailsend.Password;
            //oMailSend.Domain = mailsend.Domain;
            //oMailSend.IP = mailsend.IP;
            //oMailSend.Port = mailsend.Port;
            //oMailSend.CodeSSL = mailsend.CodeSSL;
            //oMailSend.TypeMail = mailsend.TypeMail;
            //oMailSend.IdEstado = mailsend.IdEstado;

            //oMailReceived.Email = mailreceived.Email;
            //oMailReceived.Password = mailreceived.Password;
            //oMailReceived.Domain = mailreceived.Domain;
            //oMailReceived.IP = mailreceived.IP;
            //oMailReceived.Port = mailreceived.Port;
            //oMailReceived.CodeSSL = mailreceived.CodeSSL;
            //oMailReceived.TypeMail = mailreceived.TypeMail;
            //oMailReceived.IdEstado = mailreceived.IdEstado;
        }

        private void TestConnectionWithDB()
        {
            SqlConnection cnn = new SqlConnection("Server=" + txthost.Text.Trim() + ";" + "database=" + txtbdname.Text.Trim() + ";user=" + txtusuariobd.Text.Trim() + ";pwd=" + txtpassword.Password.Trim() + ";");
            try
            {
                cnn.Open();
                if (cnn.State == ConnectionState.Open)
                {
                    MessageBox.Show("Connection is Successful!", "Info");
                    cnn.Close();
                }
                else
                {
                    MessageBox.Show("No Connection!", "Info");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Message: " + ex.Message, "Info");
            }
        }


        //bool ResultCreateDB = false;
        private bool CreateDataBaseConnection(string host, string user, string pwd, string bdname, string type_bd, string msjeFacturacion, string msjeportal)
        {
            bool result = false;
            try
            {
                var return_cnn = Return_ConnectionString(host, user, pwd);
                var response = CreateDatabase(return_cnn, bdname, ("Base de Datos " + bdname.ToUpper() + " Creada Correctamente!"), "Creando Base de Datos " + bdname);

                if (response == true)
                {
                    progressBar1.Value = 0;
                    lblProgress.Content = string.Empty;
                    Value = Constantes.ValorCero;

                    result = Execute_ScriptCommand(type_bd, host, user, pwd, bdname, msjeFacturacion, msjeportal);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        private bool Execute_Modifyque_ProfileUser()
        {
            string conex = Return_ConnectionString(txthost.Text, txtusuariobd.Text, txtpassword.Password);

            bool response = false;
            SqlConnection cnn = new SqlConnection(conex);

            string val = Constantes.ScriptUpdate_ProfileUser.Replace("DATABASEREPLACE", txtbdname.Text);
            val = val.Replace("RUCREPLACE", oEmpresa.Ruc);
            val = val.Replace("USERREPLACE", oUser.Username);


            SqlCommand cmd = new SqlCommand(val, cnn);

            try
            {
                cnn.Open();
                var result = cmd.ExecuteNonQuery();
                if (result != 0)
                { }
                response = true;
                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Message: " + ex.Message, "Info");
                response = false;
                cnn.Close();
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
            return response;
        }

        private bool CreateDatabase(string connectionString, string bdname, string msjeShow, string msjeCreating)
        {
            bool response = false;
            SqlConnection cnn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("CREATE DATABASE " + bdname, cnn);

            try
            {
                cnn.Open();
                var result = cmd.ExecuteNonQuery();
                if (result != 0)
                {
                    //StartProgress(200, msjeShow, msjeCreating);
                }
                response = true;
                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Message: " + ex.Message, "Info");
                response = false;
                cnn.Close();
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
            return response;
        }

        private string Return_ConnectionString(string host, string user, string pwd)
        {
            return "Server=" + host + "; " + "database=master; user=" + user + "; pwd=" + pwd + ";";
        }

        private bool Execute_ScriptCommand(string tipo_bd, string host, string user, string pwd, string bdname, string msjeFacturacion, string msjeportal)
        {
            string PathScript_BD = Return_PathofScriptSQL(tipo_bd); //RETORNA DONDE ESTA UBICADO EL SCRIPT DE TABLAS DE LA BD
            string Sql_Command = Return_CommandToExe(host, user, pwd, PathScript_BD);
            var result = ExecuteScriptToBD_Fact(PathScript_BD, Sql_Command, bdname, msjeFacturacion, msjeportal);
            return result;
        }

        private string Return_PathofScriptSQL(string tipo_bd)
        {

            string pathScriptSQL_BD = string.Empty;

            pathScriptSQL_BD = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string ret = @"..\..\..\Files\Script\";

            pathScriptSQL_BD = System.IO.Path.GetFullPath(System.IO.Path.Combine(pathScriptSQL_BD, ret));

            switch (cboSqlVersion.SelectedIndex)
            {
                case 0://SQL SERVER 2008 R2
                    {
                        pathScriptSQL_BD += "Sql2008_R2_";
                        break;
                    }
                case 1://SQL SERVER 2012
                    {
                        pathScriptSQL_BD += "Sql2012_";
                        break;
                    }
            }

            switch (tipo_bd)
            {
                case "fact":
                    {
                        pathScriptSQL_BD += "fact.sql";
                        break;
                    }
                case "port":
                    {
                        pathScriptSQL_BD += "port.sql";
                        break;
                    }
            }
            return pathScriptSQL_BD;
        }

        private string Return_CommandToExe(string host, string user, string pwd, string pathscript)
        {
            return "sqlcmd -S " + host + " -U " + user + " -P " + pwd + " -i " + pathscript;
        }

        private bool ExecuteScriptToBD_Fact(string pathScript, string Command, string bdname, string msjeFacturacion, string msjeportal)
        {
            bool result = false;
            try
            {
                string script_bdfact = System.IO.File.ReadAllText(pathScript);



                //string[] script = System.IO.File.ReadAllLines(pathScript);

                if (System.IO.File.Exists(pathScript))
                    System.IO.File.Delete(pathScript);

                //script_bdfact = script_bdfact.Replace("[BD_FACTURACION]", bdname);
                //script_bdfact = script_bdfact.Replace("[BD_FacturacionPortal]", bdname);

                var scriptOriginal = script_bdfact;
                script_bdfact = "USE " + bdname + "\nGO\n" + script_bdfact;

                script_bdfact = script_bdfact.Replace("RUCREPLACE", oEmpresa.Ruc);
                script_bdfact = script_bdfact.Replace("RAZONSOCIALREPLACE", oEmpresa.RazonSocial.ToUpper());
                script_bdfact = script_bdfact.Replace("EMAILREPLACE", oMailSend.Email);
                script_bdfact = script_bdfact.Replace("PASSWORDREPLACE", oMailSend.Password);

                script_bdfact = script_bdfact.Replace("DIRECCTIONREPLACE", oEmpresa.Direccion);
                script_bdfact = script_bdfact.Replace("PHONEREPLACE", oEmpresa.Telefono);
                script_bdfact = script_bdfact.Replace("MAILCOMPANYREPLACE", oMailSend.Email);
                script_bdfact = script_bdfact.Replace("PASSWORDCOMAPANYREPLACE", oMailSend.Password);
                script_bdfact = script_bdfact.Replace("DOMAINREPLACE", oMailSend.Domain);
                script_bdfact = script_bdfact.Replace("IPREPLACE", oMailSend.IP);
                script_bdfact = script_bdfact.Replace("PORTREPLACE", oMailSend.Port + string.Empty);
                script_bdfact = script_bdfact.Replace("USESSLREPLACE", oMailSend.CodeSSL);

                using (StreamWriter sw = new StreamWriter(pathScript, true, Encoding.UTF8))
                {
                    sw.WriteLine(script_bdfact);
                }

                //string BatExecute = Return_CommandToExe(host, user, pwd, pathScript);

                if (System.IO.File.Exists(pathScript))
                {
                    //System.Diagnostics.Process.Start(@"C:\WINDOWS\system32\cmd.exe", "/c " + BatExecute);
                    System.Diagnostics.Process appy = new System.Diagnostics.Process();
                    appy.StartInfo.FileName = "cmd.exe";
                    appy.StartInfo.Arguments = "/c " + Command;
                    appy.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    //appy.StartInfo.UseShellExecute = false;
                    //p.StartInfo.RedirectStandardOutput = true;
                    appy.Start();

                    progressBar1.Value = 0;
                    lblProgress.Content = string.Empty;
                    Value = Constantes.ValorCero;
                    StartProgress(200, "Componentes de BD creados correctamente!", "Creando Tablas, Procedimientos y Funciones de la Base de Datos", msjeFacturacion, msjeportal);
                    appy.WaitForExit();
                }

                if (System.IO.File.Exists(pathScript))
                {
                    System.IO.File.Delete(pathScript);

                    using (StreamWriter sw = new StreamWriter(pathScript, true, Encoding.UTF8))
                    {
                        sw.WriteLine(scriptOriginal);
                    }
                }
                result = true;
            }
            catch (Exception ex) { result = false; }

            return result;
        }


        #endregion


        #region


        string msjeProgressOk = string.Empty;
        void StartProgress(int speed, string msjeShow, string msjeCreating, string msjeFacturacion, string msjeportal)
        {
            Value = 0;
            CountingProgressBar(100, speed, msjeShow, msjeCreating, msjeFacturacion, msjeportal);
            //LayoutRoot.Children.Add(cpb);
            //ProgressCompletedEvent += () => { MessageBox.Show("Count Complete"); };
            Start();
        }

        int Maximum, Value;
        Thread timerThread;
        public void CountingProgressBar(int Time, int speed, string msjeShow, string msjeCreating, string msjeFacturacion, string msjeportal)
        {
            Maximum = Time;
            SetTimer(Time, speed, msjeShow, msjeCreating, msjeFacturacion, msjeportal);
        }

        void SetTimer(int time, int speed, string msjeShow, string msjeCreating, string msjeFacturacion, string msjeportal)
        {
            if (timerThread != null && timerThread.IsAlive)
                timerThread.Abort();

            timerThread = new Thread(() =>
            {
                int cont = 0;
                for (int i = 0; i <= time; ++i)
                {
                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
                        {
                            //lblProgress.ContentStringFormat = "Loading... " + Value;
                            lblProgress.Content = Convert.ToString(msjeCreating + "... " + Value + "%");
                            Value += 1;
                            progressBar1.Value = progressBar1.Value + 1;

                            if (progressBar1.Value == 100)
                            {
                                cont++;
                                if (cont == 2)
                                {
                                    MessageBox.Show(msjeShow, "Info");

                                    lblCreateDBFact.Content = msjeFacturacion;
                                    lblCreateDBPortal.Content = msjeportal;
                                }
                            }

                        }));
                    Thread.Sleep(speed);
                }
                ProgressComplete();
            });
            timerThread.IsBackground = true;
        }

        public void Start()
        {
            if (timerThread != null)
                timerThread.Start();
        }
        public void SetTime(int Time, int speed, string msjeShow, string msjeCreating, string msjeFacturacion, string msjeportal)
        {
            Maximum = Time;
            SetTimer(Time, speed, msjeShow, msjeCreating, msjeFacturacion, msjeportal);
        }
        public void Reset(int speed, string msjeShow, string msjeCreating, string msjeFacturacion, string msjeportal)
        {
            Value = 0;
            SetTimer(Convert.ToInt32(Maximum), speed, msjeShow, msjeCreating, msjeFacturacion, msjeportal);
        }

        public delegate void ProgressCompleted();
        public event ProgressCompleted ProgressCompletedEvent;
        private void ProgressComplete()
        {
            if (ProgressCompletedEvent != null)
                ProgressCompletedEvent();

        }

        #endregion



        private void btnTestConnection_Click(object sender, RoutedEventArgs e)
        {
            TestConnectionWithDB();
        }







        string msje_content_fact = string.Empty;
        private void btnCreateBD_Click(object sender, RoutedEventArgs e)
        {
            Create_DB_Fact();

            //if (chk_DataBaseCreated.IsChecked == true)
            //{
            //    lblCreateDBFact.Content = "";
            //}
            //else
            //{
            //    lblCreateDBFact.Content = string.Empty;
            //}

        }
        private void Create_DB_Fact()
        {
            bool ResultCreateBD = false;
            var fail = Validate_ContentCampo_BD_FACT();
            if (fail == Constantes.ValorCero)
            {
                param_msje_fact = "Base de Datos " + txtbdname.Text + " creada correctamente!";
                ResultCreateBD = CreateDataBaseConnection(txthost.Text.Trim(), txtusuariobd.Text.Trim(), txtpassword.Password.Trim(), txtbdname.Text.ToUpper(), "fact", param_msje_fact, param_msje_port);
                if (ResultCreateBD == true)
                {
                    btnSaveDataFact.IsEnabled = true;
                    //lblCreateDBFact.Content = "Database " + txtbdname.Text.ToUpper() + " Created Successfully";
                }
                else
                {
                    btnSaveDataFact.IsEnabled = false;
                    lblCreateDBFact.Content = string.Empty;
                }
            }
        }
        private int Validate_ContentCampo_BD_FACT()
        {
            int cant_fail = Constantes.ValorCero;
            if (txthost.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese Host del Servidor de Base de Datos, Campo necesario", "Info");
                cant_fail++;
                txthost.Focus();
            }

            if (txtbdname.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese un Nombre para la Base de Datos de Facturación Electrónica, Campo necesario", "Info");
                cant_fail++;
                txtbdname.Focus();
            }

            if (txtusuariobd.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese usuario de acceso a la Base de Datos, Campo necesario", "Info");
                cant_fail++;
                txtusuariobd.Focus();
            }

            if (txtpassword.Password.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese contraseña de acceso a la Base de Datos, Campo necesario", "Info");
                cant_fail++;
                txtpassword.Focus();
            }
            return cant_fail;
        }



        string param_msje_fact = string.Empty;
        string param_msje_port = string.Empty;

        private void btnCreateBD_Port_Click(object sender, RoutedEventArgs e)
        {
            Create_DB_Port();
        }
        private void Create_DB_Port()
        {
            bool ResultCreateBD = false;
            var fail = Validate_ContentCampo_BD_PORT();
            if (fail == Constantes.ValorCero) 
            {
                param_msje_port = "Base de Datos " + txtbdnameportal.Text + " creada correctamente!";
                ResultCreateBD = CreateDataBaseConnection(txthost_port.Text.Trim(), txtusuariobd_port.Text.Trim(), txtpassword_port.Password.Trim(), txtbdnameportal.Text.Trim().ToUpper(), "port", param_msje_fact, param_msje_port);
                if (ResultCreateBD == true)
                {
                    btnSaveDataPort.IsEnabled = true;
                    //lblCreateDBPortal.Content = "Database " + txtbdnameportal.Text.ToUpper() + " Created Successfully";
                }
                else
                { btnSaveDataPort.IsEnabled = false; }
            }
        }
        private int Validate_ContentCampo_BD_PORT()
        {
            int cant_fail = Constantes.ValorCero;

            if (txthost_port.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese Host del Servidor de Base de Datos del Portal, Campo necesario", "Info");
                cant_fail++;
                txthost_port.Focus();
            }

            if (txtbdnameportal.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese un Nombre para la Base de Datos del Portal de Facturación Electrónica, Campo necesario", "Info");
                cant_fail++;
                txtbdnameportal.Focus();
            }

            if (txtusuariobd_port.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese usuario de acceso a la Base de Datos, Campo necesario", "Info");
                cant_fail++;
                txtusuariobd_port.Focus();
            }

            if (txtpassword_port.Password.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese contraseña de acceso a la Base de Datos, Campo necesario", "Info");
                cant_fail++;
                txtpassword_port.Focus();
            }

            return cant_fail;
        }


        private void btnSaveDataFact_Click(object sender, RoutedEventArgs e)
        {
            var result = Registr_CompanyData_UserDataRoot();

            if (result == true) { btnNext.IsEnabled = true; MessageBox.Show("Empresa registrada con exito.", "Info"); }
            else { btnNext.IsEnabled = false; }
        }


        private void btnSaveDataPort_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //ViewValidateServicesAdq frmvalWS = new ViewValidateServicesAdq(obj_DB, oEmpresa);
            //frmvalWS.ShowDialog();
            this.Close();
        }

        private void btnTestCE_Click(object sender, RoutedEventArgs e)
        {
            Views.Test.ViewTestControlCE frmtestCE = new Test.ViewTestControlCE();
            //this.Hide();
            frmtestCE.ShowDialog();
        }

        #region methods to objects


        private bool Registr_CompanyData_UserDataRoot()
        {
            bool registr_Ok = false;
            try
            {
                GetData_toRegistrUserRoot();
                Param_IdCompanyRegistr = Registr_Company();//devuelve el id de la compañia registrada

                CreateUserObject();
                CreateUserRoleObject();
                CreateMenuPerfilObject();
                

                Registr_UserData_UserRol();
                Registr_MenuPerfil();
                Registr_MailCompany();
                Register_CertificateDigital();

                CreateConfigMainObject();

                Registr_ConfigMain();


                Execute_Modifyque_ProfileUser();


                //registr_Ok = Param_IdCompanyRegistr;

                var list_Comp = objImplement.GetListCompany(Param_IdCompanyRegistr);

                if (list_Comp.Count == Constantes.ValorUno)
                {
                    registr_Ok = true;
                }else { registr_Ok = false; }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                registr_Ok = false;
            }
            return registr_Ok;
        }
        private int Registr_Company()
        {
            var result = objImplement.InsertCompany(oEmpresa);
            Param_IdCompanyRegistr = result;
            return result;
        }
        private void Registr_UserData_UserRol()
        {
            //se regisra el usuario
            var msjeResult = objImplement.InsertUserRoot(oUser);



            //registramos los roles para el usuario root
            string coma = string.Empty;
            string msjeRolUser = string.Empty;
            if (objListUserRol.Count > 0)
            {
                foreach (var obj in objListUserRol)
                {
                    msjeRolUser += coma + objImplement.Insert_RolUserRoot(obj);
                    coma = ",";
                }
            }
            //
        }
        private void Registr_MenuPerfil()
        {
            var msjeResult = objImplement.Insert_MenuPerfil(objListMenuPerfil);

            string[] arrayResult = msjeResult.Split(',');

            if (arrayResult.Length == objListMenuPerfil.Count)
            {
                //escribimos la cantidad de registros que se ingresaron
                //return true;
            }
            else
            {
                //escribimos la cantidad de registros que se ingresaron
                //return false;
            }
        }

        private void Registr_ConfigMain()
        {
            if (ObjListConfigMain.Count > 0)
            {
                if (ObjListConfigMain.Count == 180) { ObjListConfigMain.RemoveAt(179); }

                foreach (var obj in ObjListConfigMain)
                {
                    var msje = objImplement.Insert_ConfigMain(obj);
                }
            }
        }


        private void Registr_MailCompany()
        {
            oMailSend.IdCompany = Param_IdCompanyRegistr;
            oMailReceived.IdCompany = Param_IdCompanyRegistr;

            oMailSend.RucCompany = oEmpresa.Ruc;
            oMailReceived.RucCompany = oEmpresa.Ruc;

            if (oMailSend != null)
            {
                var result = objImplement.Insert_MailCompany(oMailSend);
            }

            if (oMailReceived != null)
            {
                var result = objImplement.Insert_MailCompany(oMailReceived);
            }
        }

        private void Register_CertificateDigital()
        {
            try
            {
                var result = objImplement.Register_CertificateDigital(oCert);
            }
            catch (Exception ex) { }
        }


        public ImplementationInstall objImplement;

        public ListPerfil oListPerfil = new ListPerfil();
        public ListRol oListRol = new ListRol();
        public ListMenu oListMenu = new ListMenu();


        public int Param_IdPerfilAdmin = Constantes.ValorCero;
        private void GetData_toRegistrUserRoot()
        {
            oListRol = new ListRol();
            oListPerfil = new ListPerfil();
            oListMenu = new ListMenu();

            objImplement = new ImplementationInstall(txthost.Text.Trim(), txtbdname.Text.ToUpper(), txtusuariobd.Text.Trim(), txtpassword.Password.Trim());


            oListPerfil = objImplement.GetList_Perfil(oEmpresa.Ruc);
            oListRol = objImplement.GetList_Roles();
            oListMenu = objImplement.GetListMenu();
        }

        private void CreateUserObject()
        {
            oUser = new User();
            oUser.IdEstado = Constantes.ValorUno;
            oUser.IdCompany = Param_IdCompanyRegistr;
            oUser.IdPerfil = Constantes.ValorUno;

            if (oListPerfil.Count > 0)
            {
                foreach (var obj in oListPerfil)
                {
                    if (obj.NombrePerfil.Contains(Constantes.Admin)) { oUser.IdPerfil = obj.IdPerfil; Param_IdPerfilAdmin = obj.IdPerfil; break; }
                }
            }

            oUser.Nombres = Constantes.root;
            oUser.ApePaterno = Constantes.root;
            oUser.ApeMaterno = Constantes.root;
            oUser.DNI = oEmpresa.Ruc;
            oUser.Direccion = Constantes.root;
            oUser.Telefono = Constantes.rootfone;
            oUser.Email = Constantes.rootemail;

            if (oEmpresa.TpoLogin == Constantes.LoginWithLDAP)
            {
                oUser.Username = oEmpresa.UserAdmin;
                oUser.Password = new InstallCommon.Util.Encrypt().HashPassword(oEmpresa.UserAdmin);
            }
            else
            {
                oUser.Username = oEmpresa.Ruc;
                oUser.Password = new InstallCommon.Util.Encrypt().HashPassword(oEmpresa.Ruc);
            }
            
            oUser.FechaExpiracion = Convert.ToDateTime(Constantes.DateNull);
            oUser.FechaRegistro = DateTime.Now;
            oUser.Sede = string.Empty;
        }

        //lista_usuario_rol
        private void CreateUserRoleObject()
        {
            objListUserRol = new ListUserRol();

            if (oListRol.Count > 0)
            {
                foreach (var obj in oListRol)
                {
                    oUserRol = new UserRol();
                    oUserRol.IdRol = obj.IdRol;
                    //oUserRol.Dni_Ruc = oEmpresa.Ruc;
                    oUserRol.Dni_Ruc = oUser.DNI;

                    //if (oEmpresa.TpoLogin == Constantes.LoginWithLDAP)
                    //{
                    //    oUserRol.Dni_Ruc = oEmpresa.UserAdmin;
                    //}
                    //else
                    //{
                    //    oUserRol.Dni_Ruc = oEmpresa.Ruc;
                    //}
                    
                    objListUserRol.Add(oUserRol);
                }
            }
        }

        //lista_menu_perfil
        private void CreateMenuPerfilObject()
        {
            objListMenuPerfil = new ListMenuPerfil();

            if (Param_IdPerfilAdmin != Constantes.ValorCero && Param_IdPerfilAdmin == Constantes.ValorUno)
            {
                if (oListMenu.Count > 0)
                {
                    foreach (var obj in oListMenu)
                    {
                        oMenuPerfil = new MenuPerfil();
                        oMenuPerfil.IdMenu = obj.IdMenu;
                        oMenuPerfil.IdPerfil = Param_IdPerfilAdmin;
                        objListMenuPerfil.Add(oMenuPerfil);
                    }
                }
            }
        }


        public void WriteLog_Service_Comp(string path, List<ConfigMain> log)
        {

            //using (StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8))
            //{
            //    sw.WriteLine(log);
            //}

            using (StreamWriter sw = new StreamWriter(path + ".txt", true, Encoding.UTF8))
            {
                foreach (var line in log)
                {
                    sw.WriteLine(line.TAB + "#" + line.NOM + "#" + line.POS + "#" + line.VAL + "#" + line.MND + "#" + line.DOC + "#" + line.MSG);
                }



                //string [] array = path.Split('#');
            }
        }
                    

        public List<ConfigMain> ObjListConfigMain = new List<ConfigMain>();
        private void CreateConfigMainObject()
        {
            string pathFileExcel = string.Empty;
            pathFileExcel = System.Reflection.Assembly.GetExecutingAssembly().Location;

            //string ret = @"..\..\..\Files\Excel\Config_Main.xlsx";
            string ret = @"..\..\..\Files\Excel\Config_Main.txt";
            pathFileExcel = System.IO.Path.GetFullPath(System.IO.Path.Combine(pathFileExcel, ret));
            ObjListConfigMain = ToEntidadHojaExcelList(pathFileExcel);



            //WriteLog_Service_Comp(@"J:\Config\ConfigMain", ObjListConfigMain);

        }
        public List<ConfigMain> ToEntidadHojaExcelList(string pathDelFicheroExcel)
        {
            var list_res = new List<ConfigMain>();
            ConfigMain obj = new ConfigMain();
            #region
            //try
            //{
            //    var book = new ExcelQueryFactory(pathDelFicheroExcel);
            //    res = (from row in book.Worksheet("Config")
            //                     let item = new ConfigMain
            //                     {
            //                         TAB = row["TAB"].Cast<string>() == null ? string.Empty : row["TAB"].Cast<string>(),
            //                         NOM = row["NOM"].Cast<string>() == null ? string.Empty : row["NOM"].Cast<string>(),
            //                         POS = row["POS"].Cast<string>() == null ? string.Empty : row["POS"].Cast<string>(),
            //                         VAL = row["VAL"].Cast<string>() == null ? string.Empty : row["VAL"].Cast<string>(),
            //                         MND = row["MND"].Cast<string>() == null ? string.Empty : row["MND"].Cast<string>(),
            //                         DOC = row["DOC"].Cast<string>() == null ? string.Empty : row["DOC"].Cast<string>(),
            //                         MSG = row["MSG"].Cast<string>() == null ? string.Empty : row["MSG"].Cast<string>(),
            //                         ECV = row["ECV"].Cast<string>() == null ? string.Empty : row["ECV"].Cast<string>(),
            //                         ECN = row["ECN"].Cast<string>() == null ? string.Empty : row["ECN"].Cast<string>()
            //                     }
            //                     select item).ToList();

            //    book.Dispose();

            //    //return resultado;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Message: " + ex.Message, "Info");
            //}
            //return res;
            #endregion
            try
            {
                

                foreach (string line in File.ReadLines(pathDelFicheroExcel))
                {
                    obj = new ConfigMain();

                    string[] array = line.Split('#');
                    
                    obj.TAB = array[0];
                    obj.NOM = array[1];
                    obj.POS = array[2];
                    obj.VAL = array[3];
                    obj.MND = array[4];
                    obj.DOC = array[5];
                    obj.MSG = array[6];
                    obj.ECN = string.Empty;
                    obj.ECV = string.Empty;

                    list_res.Add(obj);
                }
            }
            catch (Exception ex) { }
            return list_res;
        }

        #endregion


















        private ConectionClass obj_DB = new ConectionClass();

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Validate_ToNext();
        }

        private void Validate_ToNext()
        {
            if (lblCreateDBPortal.Content.ToString().Length > Constantes.ValorCero && lblCreateDBFact.Content.ToString().Length > Constantes.ValorCero)
            {
                obj_DB = new ConectionClass();

                obj_DB.Host = txthost.Text.Trim();
                obj_DB.DBName = txtbdname.Text.ToUpper();
                obj_DB.UserDB = new InstallCommon.Util.Encrypt().EncryptKey(txtusuariobd.Text.Trim());
                obj_DB.PwdDB = new InstallCommon.Util.Encrypt().EncryptKey(txtpassword.Password.Trim());

                ViewValidateServicesAdq frmvalWS = new ViewValidateServicesAdq(obj_DB, oEmpresa);
                frmvalWS.ShowDialog();
            }
            else
            {
                if (lblCreateDBPortal.Content.ToString().Length > Constantes.ValorCero)
                {
                    MessageBox.Show("Asegurese de Crear la base de datos de Facturación Electrónica.", "Info");
                }
                else if (lblCreateDBFact.Content.ToString().Length > Constantes.ValorCero)
                {
                    MessageBox.Show("Debe Crear una Base de Datos para el Portal de Facturación Electrónica.", "Info");
                }
            }
        }

















        #region load instances sql server

        List<string> listInstance = new List<string>();
        private void LoadInstancesInstalled()
        {
            Microsoft.Win32.RegistryKey rk;
            rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server", false);
            listInstance = (List<string>)rk.GetValue("InstalledInstances");
        }

        List<string> m_instances = new List<string>();

        private void chk_DataBaseCreated_Checked(object sender, RoutedEventArgs e)
        {
            lblCreateDBFact.Content = "Base de Datos Existe";
            btnCreateBD.IsEnabled = false;
            btnSaveDataFact.IsEnabled = true;
        }

        private void chk_DataBaseCreated_Unchecked(object sender, RoutedEventArgs e)
        {
            lblCreateDBFact.Content = string.Empty;
            btnCreateBD.IsEnabled = true;
            btnSaveDataFact.IsEnabled = false;
        }

        private void chk_DataBaseCreated_Port_Checked(object sender, RoutedEventArgs e)
        {
            lblCreateDBPortal.Content = "Base de Datos Existe";
            btnCreateBD_Port.IsEnabled = false;
        }

        private void chk_DataBaseCreated_Port_Unchecked(object sender, RoutedEventArgs e)
        {
            lblCreateDBPortal.Content = string.Empty;
            btnCreateBD_Port.IsEnabled = true;
        }

        private void GetInstances()
        {
            m_instances = new List<string>();
            SqlDataSourceEnumerator servidores;
            System.Data.DataTable tablaServidores;
            string servidor;

            servidores = SqlDataSourceEnumerator.Instance;

            tablaServidores = new DataTable();

            tablaServidores = servidores.GetDataSources();
            foreach (DataRow rowServidor in tablaServidores.Rows)
            {

                // La instancia de SQL Server puede tener nombre de instancia 
                //o únicamente el nombre del servidor, comprobamos si hay 
                //nombre de instancia para mostrarlo
                if (String.IsNullOrEmpty(rowServidor["InstanceName"].ToString()))
                    m_instances.Add(rowServidor["ServerName"].ToString());
                else
                    m_instances.Add(rowServidor["ServerName"] + "\\" + rowServidor["InstanceName"]);
            }
        }

        #endregion






    }
}
