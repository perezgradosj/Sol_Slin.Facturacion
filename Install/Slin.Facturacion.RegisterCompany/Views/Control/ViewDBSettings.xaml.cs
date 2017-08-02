using Slin.Facturacion.Common;
using Slin.Facturacion.InstallCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace Slin.Facturacion.RegisterCompany.Views.Control
{
    /// <summary>
    /// Interaction logic for ViewDBSettings.xaml
    /// </summary>
    public partial class ViewDBSettings : Window
    {
        public ViewDBSettings(Company oemp, Correo mailSend, Correo mailReceived, CredentialCertificate oCertificate)
        {
            InitializeComponent();
            StartCharger(oemp, mailSend, mailReceived, oCertificate);
        }

        #region ENTITY

        public Company oEmpresa;
        public User oUser;
        public UserRol oUserRol;
        public ListUserRol objListUserRol;
        public MenuPerfil oMenuPerfil;
        public ListMenuPerfil objListMenuPerfil;
        public Correo oMailSend;
        public Correo oMailReceived;

        public CredentialCertificate oCert;

        private string ParamHost = string.Empty;
        private string ParamDBName = string.Empty;
        private string ParamUser = string.Empty;
        private string ParamPwd = string.Empty;

        public int Param_IdCompanyRegistr = Constantes.ValorCero;


        public ImplementationInstall objImplement;

        public ListPerfil oListPerfil = new ListPerfil();
        public ListRol oListRol = new ListRol();
        public ListMenu oListMenu = new ListMenu();

        public int Param_IdPerfilAdmin = Constantes.ValorCero;
        private ConectionClass obj_DB = new ConectionClass();

        #endregion


        #region METHOD

        private void StartCharger(Company oemp, Correo mailsend, Correo mailreceived, CredentialCertificate ocertificate)
        {
            ReceivedObjectEnterprise(oemp, mailsend, mailreceived, ocertificate);
            

            ClearTextBox();

            //txthost.Text = "192.168.90.8";
            //txtusuariobd.Text = "sa";
            //txtpassword.Password = "C0rporaci0n";
            //txtbdname.Text = "BD_FACTURACION";

            //txthost_port.Text = "192.168.90.8";
            //txtusuariobd_port.Text = "sa";
            //txtpassword_port.Password = "C0rporaci0n";
            //txtbdnameportal.Text = "BD_FacturacionPortal";
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
        #endregion



        private void btnSaveDataFact_Click(object sender, RoutedEventArgs e)
        {
            var result = Registr_CompanyData_UserDataRoot();

            if (result == true) { btnNext.IsEnabled = true; MessageBox.Show("Empresa registrada con exito.", "Info");
                lblCreateDBFact.Content = "Registrado con exito.";
            }
            else { btnNext.IsEnabled = false; }
        }





        private string Return_ConnectionString(string host, string user, string pwd)
        {
            return "Server=" + host + "; " + "database=master; user=" + user + "; pwd=" + pwd + ";";
        }

        private bool Execute_Modifyque_ProfileUser()
        {
            string conex = Return_ConnectionString(txthost.Text, txtusuariobd.Text, txtpassword.Password);

            bool response = false;
            SqlConnection cnn = new SqlConnection(conex);

            string val = Constantes.ScriptUpdate_ProfileUser_Second.Replace("DATABASEREPLACE", txtbdname.Text);
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


        #region other methods
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
                //CreateConfigMainObject();
                CreateObjectListServicesWS();


                Registr_UserData_UserRol();
                Registr_MenuPerfil();
                //Registr_ConfigMain();
                Registr_MailCompany();

                Register_CertificateDigital(); //registro de datos del certificado digital

                var resultMissed = Register_TimeServiceCompany();


                Execute_Modifyque_ProfileUser();




                //registr_Ok = Param_IdCompanyRegistr;
                var list_Comp = objImplement.GetListCompany(Param_IdCompanyRegistr);

                if (list_Comp.Count == Constantes.ValorUno)
                {
                    registr_Ok = true;
                }
                else { registr_Ok = false; }
            }
            catch (Exception ex)
            {
                registr_Ok = false;
                MessageBox.Show("Error: " + ex.Message);
            }
            return registr_Ok;
        }


        private ListPerfil olistPerfil = new ListPerfil();

        private void GetDataListPerfil()
        {
            olistPerfil = new ListPerfil();

            olistPerfil.Insert(0, new Perfil() { NombrePerfil = "Administrador", Codigo = "A", RucCompany = oEmpresa.Ruc });
            olistPerfil.Insert(1, new Perfil() { NombrePerfil = "Facturador", Codigo = "F", RucCompany = oEmpresa.Ruc });
            olistPerfil.Insert(2, new Perfil() { NombrePerfil = "Consultor", Codigo = "CC", RucCompany = oEmpresa.Ruc });
            olistPerfil.Insert(3, new Perfil() { NombrePerfil = "Contabilidad", Codigo = "C", RucCompany = oEmpresa.Ruc });
        }

        private void GetData_toRegistrUserRoot()
        {
            try
            {
                oListRol = new ListRol();
                oListPerfil = new ListPerfil();
                oListMenu = new ListMenu();

                objImplement = new ImplementationInstall(txthost.Text.Trim(), txtbdname.Text.ToUpper(), txtusuariobd.Text.Trim(), txtpassword.Password.Trim());

                GetDataListPerfil();

                foreach (var obj in olistPerfil)
                {
                    var result = objImplement.RegisterPerf_Comp(obj);
                }
                //luego de insertar perfiles los traemos con el id que se le haya asignado a cada uno

                oListPerfil = objImplement.GetList_Perfil(oEmpresa.Ruc);
                oListRol = objImplement.GetList_Roles();
                oListMenu = objImplement.GetListMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            
        }

        private int Registr_Company()
        {
            var result = objImplement.InsertCompany(oEmpresa);
            Param_IdCompanyRegistr = result;
            return result;
        }

        private void CreateUserObject()
        {
            oUser = new User();
            oUser.IdEstado = Constantes.ValorUno;
            oUser.IdCompany = Param_IdCompanyRegistr;
            oUser.IdPerfil = Constantes.ValorCero;

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

        private void CreateUserRoleObject()
        {
            objListUserRol = new ListUserRol();

            if (oListRol.Count > 0)
            {
                foreach (var obj in oListRol)
                {
                    oUserRol = new UserRol();
                    oUserRol.IdRol = obj.IdRol;
                    oUserRol.Dni_Ruc = oUser.DNI;

                    objListUserRol.Add(oUserRol);
                }
            }
        }

        private void CreateMenuPerfilObject()
        {
            objListMenuPerfil = new ListMenuPerfil();

            //if (Param_IdPerfilAdmin != Constantes.ValorCero && Param_IdPerfilAdmin == Constantes.ValorUno)
            if (Param_IdPerfilAdmin != Constantes.ValorCero)
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

        private ListSevices olistservice;
        public ListSevices oListService
        {
            get { return olistservice; }
            set
            {
                olistservice = value;
            }
        }

        private void CreateObjectListServicesWS()
        {
            try
            {
                var list = objImplement.GetList_TimeService();
                oListService = new ListSevices();

                foreach (var obj in list)
                {
                    var objservice = new Service();

                    objservice.CodeService = obj.CodeService;
                    objservice.NameService = obj.NameService;
                    objservice.ValueTime = Constantes.ValueTime_Initial;
                    objservice.IntervalValue = Constantes.IntervalValue_Initial;
                    objservice.MaxNumAttempts = Constantes.MaxNumAttempts_Initial;
                    objservice.RucEntity = oEmpresa.Ruc;
                    objservice.IdEstado = Constantes.ValorDos;
                    objservice.SubType = obj.SubType;
                    oListService.Add(objservice);
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private int Register_TimeServiceCompany()
        {
            int missed = Constantes.ValorCero;
            try
            {
                if (oListService.Count > Constantes.ValorCero)
                {
                    foreach (var obj in oListService)
                    {
                        var result = objImplement.Register_TimeServiceCompany(obj);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); missed++; }
            return missed;
        }

        private void Registr_UserData_UserRol()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            
        }

        private void Registr_MenuPerfil()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            
        }

        private void Registr_MailCompany()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            
        }

        private void Register_CertificateDigital()
        {
            try
            {
                var result = objImplement.Register_CertificateDigital(oCert);
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        #endregion

        private void btnRegister_BDPort_Click(object sender, RoutedEventArgs e)
        {
            

            try
            {
                var result = CreateObjectRegister_Port();

                if (result == true)
                {
                    btnNext.IsEnabled = true; MessageBox.Show("Empresa registrada con exito.", "Info");
                    lblCreateDBPortal.Content = "Registrado con exito.";
                }
                else { btnNext.IsEnabled = false; }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private bool CreateObjectRegister_Port()
        {
            try
            {
                objImplement = new ImplementationInstall(txthost_port.Text.Trim(), txtbdnameportal.Text.ToUpper(), txtusuariobd_port.Text.Trim(), txtpassword_port.Password.Trim());
                //CreateUserObject();
                

                oMailSend.RucCompany = oEmpresa.Ruc;
                oMailReceived.RucCompany = oEmpresa.Ruc;


                var msjeResult = objImplement.RegisterCompany_Portal(oEmpresa, oMailSend);

                var msjResult = objImplement.RegisterUserCompany_Portal(oUser);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Validate_Next();
        }

        private void Validate_Next()
        {
            if (lblCreateDBFact.Content.ToString().Length > Constantes.ValorCero && lblCreateDBPortal.Content.ToString().Length > Constantes.ValorCero)
            {
                obj_DB = new ConectionClass();
                obj_DB.Host = txthost.Text.Trim();
                obj_DB.DBName = txtbdname.Text.ToUpper();
                obj_DB.UserDB = new InstallCommon.Util.Encrypt().EncryptKey(txtusuariobd.Text.Trim());
                obj_DB.PwdDB = new InstallCommon.Util.Encrypt().EncryptKey(txtpassword.Password.Trim());

                ViewValidateServicesAdq frmvalWS = new ViewValidateServicesAdq(obj_DB, oEmpresa);
                frmvalWS.ShowDialog();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //obj_DB = new ConectionClass();
            //obj_DB.Host = txthost.Text.Trim();
            //obj_DB.DBName = txtbdname.Text.ToUpper();
            //obj_DB.UserDB = new InstallCommon.Util.Encrypt().EncryptKey(txtusuariobd.Text.Trim());
            //obj_DB.PwdDB = new InstallCommon.Util.Encrypt().EncryptKey(txtpassword.Password.Trim());

            //ViewValidateServicesAdq frmvalWS = new ViewValidateServicesAdq(obj_DB, oEmpresa);
            //frmvalWS.ShowDialog();
            this.Close();
        }
    }
}
