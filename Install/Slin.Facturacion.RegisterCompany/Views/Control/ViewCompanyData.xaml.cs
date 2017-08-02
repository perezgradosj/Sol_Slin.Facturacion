using Slin.Facturacion.Common;
using Slin.Facturacion.InstallCommon;
using Slin.Facturacion.InstallCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ViewCompanyData.xaml
    /// </summary>
    public partial class ViewCompanyData : Window
    {
        public ViewCompanyData()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            InitializeComponent();

            lblTitle.Width = lblTitle.Width * 3;
            StartCharger();
        }

        #region ENTITY

        private static Company oempresa;
        public static Company oEmpresa
        {
            get { return oempresa; }
            set
            {
                oempresa = value;
            }
        }



        private CredentialCertificate ocert;
        public CredentialCertificate oCertificate
        {
            get { return ocert; }
            set
            {
                ocert = value;
            }
        }



        #endregion

        #region method

        private void StartCharger()
        {
            Charger();
            ClearTextbox();
        }

        private void Charger()
        {
            cboactivedirectory.ItemsSource = new InstallCommon.Util.Util().GetListAD();
            cboactivedirectory.SelectedIndex = 0;
        }

        void ClearTextbox()
        {
            txtdireccion.Clear();
            //txtdomiciliofiscal.Clear();
            txtemail.Clear();
            txtpaginaweb.Clear();
            txtphone.Clear();
            txtrazoncomercial.Clear();
            txtrazonsocial.Clear();
            txtruc.Clear();
            txtubigeo.Clear();
            txturbanizacion.Clear();
            txtruc.MaxLength = 11;
            txtubigeo.MaxLength = 6;

            txtUseractiveDirectory.Clear();
            txtUserCertificate.Clear();
            txtPwdCertificate.Clear();
            txtactive_directoryName.Clear();

            txturl_CompanyLogo.Clear();
            txturl_CompanyConsult.Clear();
            


            //txtdireccion.Text = "avenida";
            //txtdomiciliofiscal.Text = "avenida";
            //txtemail.Text = "email@gmail.com";
            //txtpaginaweb.Text = "www.pagweb.com";
            //txtphone.Text = "123456789";
            //txtrazoncomercial.Text = "Company SAC";
            //txtrazonsocial.Text = "Company";
            //txtruc.Text = "12345678911";
            //txtubigeo.Text = "123456";
            //txturbanizacion.Text = "123456";

            //txtPwdCertificate.Text = "123456789A.";
            //txtUserCertificate.Text = "gaveglio.pfx";

            txtactive_directoryName.IsEnabled = false;
            txtUseractiveDirectory.IsEnabled = false;
        }

        private void LlenarObjeto()
        {
            oEmpresa = new Company();

            oCertificate = new CredentialCertificate();

            oEmpresa.CodCompany = txtruc.Text.Trim();
            oEmpresa.Ubi = txtubigeo.Text.Trim();
            oEmpresa.Ruc = txtruc.Text.Trim();
            oEmpresa.RazonSocial = txtrazonsocial.Text.Trim();
            oEmpresa.RazonComercial = txtrazoncomercial.Text.TrimEnd();
            oEmpresa.Telefono = txtphone.Text.TrimEnd();
            oEmpresa.Fax = txtphone.Text.TrimEnd();
            oEmpresa.Direccion = txtdireccion.Text.TrimEnd();
            oEmpresa.DomicilioFiscal = string.Empty;
            oEmpresa.Urbanizacion = txturbanizacion.Text.TrimEnd();
            oEmpresa.FechaRegistro = DateTime.Now;
            oEmpresa.PaginaWeb = txtpaginaweb.Text.Trim();
            oEmpresa.Email = txtemail.Text.Trim();
            oEmpresa.IdEstado = Constantes.ValorUno;//DURACELL
            oEmpresa.Id_TDI = Constantes.ValorUno;//DURACELL (1=6=RUC)

            oEmpresa.Us_Cert = txtUserCertificate.Text.Trim();
            oEmpresa.Pwd_Cert = new InstallCommon.Util.Encrypt().EncryptKey(txtPwdCertificate.Text.Trim());
            oEmpresa.Name_ServerAD = txtactive_directoryName.Text;

            oEmpresa.UserAdmin = txtUseractiveDirectory.Text;

            if (cboactivedirectory.SelectedIndex == Constantes.ValorUno) { oEmpresa.TpoLogin = Constantes.LoginWithLDAP; }
            else { oEmpresa.TpoLogin = string.Empty; }

            oEmpresa.Url_CompanyLogo = txturl_CompanyLogo.Text.Length == Constantes.ValorCero ? string.Empty : txturl_CompanyLogo.Text;
            oEmpresa.Url_CompanyConsult = txturl_CompanyConsult.Text.Length == Constantes.ValorCero ? string.Empty : txturl_CompanyConsult.Text;

            //oEmpresa.ExpirationDate_Cert = txtExpirationDate.SelectedDate.Value.ToString("dd-MM-yyyy");

            //oEmpresa.Protocolo = cboprotocolo.Text;

            oCertificate.ExpirationDate = txtExpirationDate.SelectedDate.Value.ToString("dd-MM-yyyy");
            oCertificate.NameCertificate = txtUserCertificate.Text;
            oCertificate.Password = new InstallCommon.Util.Encrypt().EncryptKey(txtPwdCertificate.Text);
            oCertificate.RucCompany = txtruc.Text;
        }

        #endregion


        private void cboactivedirectory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboactivedirectory.SelectedIndex == Constantes.ValorUno)
            {
                txtUseractiveDirectory.IsEnabled = true;
                txtactive_directoryName.IsEnabled = true;
            }
            else
            {
                txtUseractiveDirectory.IsEnabled = false;
                txtactive_directoryName.IsEnabled = false;

                txtUseractiveDirectory.Clear();
                txtactive_directoryName.Clear();
            }
        }


        //public MainControl oMainControl;
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {

            var missed = Validate_ContentCampo();

            if (missed == Constantes.ValorCero)
            {
                LlenarObjeto();
                Views.Control.ViewMailSettings frmmail = new ViewMailSettings(oEmpresa, oCertificate);
                //this.Visibility = System.Windows.Visibility.Hidden;
                frmmail.ShowDialog();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private int Validate_ContentCampo()
        {
            int cant_missed = Constantes.ValorCero;
            if (txtrazonsocial.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese la Razón Social, Campo necesario", "Info");
                cant_missed++;
                txtrazonsocial.Focus();
                return cant_missed;
            }

            if (txtrazoncomercial.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese la Razón Comercial, Campo necesario", "Info");
                cant_missed++;
                txtrazoncomercial.Focus();
                return cant_missed;
            }

            if (txtruc.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese la RUC de la Empresa, Campo necesario", "Info");
                cant_missed++;
                txtruc.Focus();
                return cant_missed;
            }

            if (txtruc.Text.Length > Constantes.ValorCero)
            {
                bool isNumber = Regex.IsMatch(txtruc.Text, @"^\d{11}$", RegexOptions.IgnoreCase);
                if (isNumber == false)
                {
                    MessageBox.Show("Ingrese un valor Númerico de 11 dígitos", "Info");
                    cant_missed++;
                    txtruc.Focus();
                    return cant_missed;
                }
            }



            if (txtubigeo.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese la Ubigeo, Campo necesario", "Info");
                cant_missed++;
                txtubigeo.Focus();
                return cant_missed;
            }

            if (txtubigeo.Text.Length > Constantes.ValorCero)
            {
                bool isNumber = Regex.IsMatch(txtubigeo.Text, @"^\d{6}$", RegexOptions.IgnoreCase);
                if (isNumber == false)
                {
                    MessageBox.Show("Ingrese un valor Númerico de 6 dígitos", "Info");
                    cant_missed++;
                    txtubigeo.Focus();
                    return cant_missed;
                }
            }


            if (txtdireccion.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese Dirección, Campo necesario", "Info");
                cant_missed++;
                txtdireccion.Focus();
                return cant_missed;
            }

            //if (txtdomiciliofiscal.Text.Length == Constantes.ValorCero)
            //{
            //    MessageBox.Show("Ingrese Domicilio fiscal, Campo necesario", "Info");
            //    cant_missed++;
            //    txtdomiciliofiscal.Focus();
            //    return cant_missed;
            //}


            if (txtpaginaweb.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese su Página web, Campo necesario", "Info");
                cant_missed++;
                txtpaginaweb.Focus();
                return cant_missed;
            }

            if (txtemail.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese Email, Campo necesario", "Info");
                cant_missed++;
                txtemail.Focus();
                return cant_missed;
            }


            if (txtemail.Text.Length > Constantes.ValorTres)
            {
                bool isEmail = Regex.IsMatch(txtemail.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                if (isEmail == false)
                {
                    MessageBox.Show("Ingrese un Email valido, Campo necesario", "Info");
                    cant_missed++;
                    txtemail.Focus();
                    return cant_missed;
                }
            }

            if (txtphone.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese Teléfono, Campo necesario", "Info");
                cant_missed++;
                txtphone.Focus();
                return cant_missed;
            }

            if (cboactivedirectory.Text.Contains("YES"))
            {
                if (txtactive_directoryName.Text.Length == Constantes.ValorCero)
                {
                    MessageBox.Show("Ingrese Nombre del Servidor de Dominio, Campo necesario", "Info");
                    cant_missed++;
                    cboactivedirectory.Focus();
                    return cant_missed;
                }

                if (txtUseractiveDirectory.Text.Length == Constantes.ValorCero)
                {
                    MessageBox.Show("Ingrese Usuario de Active Directory, Campo necesario", "Info");
                    cant_missed++;
                    txtUseractiveDirectory.Focus();
                    return cant_missed;
                }
            }

            if (txtUserCertificate.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese Usuario del Certificado Digital, Campo necesario", "Info");
                cant_missed++;
                txtUserCertificate.Focus();
                return cant_missed;
            }

            if (txtPwdCertificate.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese Contraseña del Certificado Digital, Campo necesario", "Info");
                cant_missed++;
                txtPwdCertificate.Focus();
                return cant_missed;
            }
            
            if (!txtExpirationDate.Text.Contains("/"))
            {
                MessageBox.Show("Seleccione la fecha de expiración del certificado digital!, valor necesario", "Info");
                cant_missed++;
                txtphone.Focus();
                return cant_missed;
            }

            return cant_missed;
        }
    }
}
