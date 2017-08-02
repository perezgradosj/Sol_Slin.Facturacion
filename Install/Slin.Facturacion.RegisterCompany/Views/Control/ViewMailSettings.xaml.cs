using Slin.Facturacion.Common;
using Slin.Facturacion.InstallCommon;
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
    /// Interaction logic for ViewMailSettings.xaml
    /// </summary>
    public partial class ViewMailSettings : Window
    {
        public ViewMailSettings(Company oemp, CredentialCertificate oCertificate)
        {
            InitializeComponent();
            StartCharger(oemp, oCertificate);
        }



        #region ENTITY

        public Company oEmpresa;

        public Correo oMailSend;
        public Correo oMailReceived;

        public CredentialCertificate oCert;

        #endregion

        #region METHOD

        private void StartCharger(Company oemp, CredentialCertificate ocertificate)
        {
            ReceivedObject(oemp, ocertificate);
            Charger();
            InicialityTextbox();
        }

        private void Charger()
        {
            cboSSL_send.ItemsSource = new InstallCommon.Util.Util().GetListSSL();
            cboSSL_send.SelectedIndex = 0;

            cboSSL_incoming.ItemsSource = new InstallCommon.Util.Util().GetListSSL();
            cboSSL_incoming.SelectedIndex = 0;
        }

        private void InicialityTextbox()
        {
            txtdominioincoming.Clear();
            txtdominiosend.Clear();
            txtipincoming.Clear();
            txtipsend.Clear();
            txtmailincoming.Clear();
            txtmailsend.Clear();
            txtpasswordincoming.Clear();
            txtpasswordsend.Clear();
            txtportincoming.Clear();
            txtportsend.Clear();


            //txtdominioincoming.Text = "pop.tecniservices.net";
            //txtipincoming.Text = "208.84.244.131";
            //txtmailincoming.Text = "facturaelectronica_2@tecniservices.net";
            //txtpasswordincoming.Password = "incoming";
            //txtportincoming.Text = "110";
            cboSSL_incoming.SelectedIndex = Constantes.ValorDos;



            //txtdominiosend.Text = "smtpseguro.tecniservices.net";
            //txtipsend.Text = "208.84.244.49";
            //txtmailsend.Text = "facturaelectronica_2@tecniservices.net";
            //txtpasswordsend.Password = "sendmail";
            //txtportsend.Text = "25";
            cboSSL_send.SelectedIndex = Constantes.ValorDos;
        }

        private void ReceivedObject(Company oemp, CredentialCertificate ocertificate)
        {
            oEmpresa = new Company();
            oCert = new CredentialCertificate();

            oEmpresa = oemp;
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
            //oEmpresa.Id_TDI = oemp.Id_TDI;
            //oEmpresa.IdEstado = oemp.IdEstado;
            //oEmpresa.TpoLogin = oemp.TpoLogin;
        }

        private void FillObjectMail()
        {
            oMailSend = new Correo();
            //oMailSend.SSL = new SSL();
            oMailReceived = new Correo();
            //oMailReceived.SSL = new SSL();

            oMailSend.Email = txtmailsend.Text.Trim();
            oMailSend.Password = new InstallCommon.Util.Encrypt().EncryptKey(txtpasswordsend.Password.Trim());
            oMailSend.Domain = txtdominiosend.Text.Trim();
            oMailSend.IP = txtipsend.Text.Trim();
            oMailSend.Port = txtportsend.Text == string.Empty ? 0 : int.Parse(txtportsend.Text.Trim());
            oMailSend.CodeSSL = cboSSL_send.SelectedIndex == 1 ? "1" : "0";
            oMailSend.TypeMail = "Send";
            oMailSend.IdEstado = Constantes.ValorUno;


            oMailReceived.Email = txtmailincoming.Text.Trim();
            oMailReceived.Password = new InstallCommon.Util.Encrypt().EncryptKey(txtpasswordincoming.Password.Trim());
            oMailReceived.Domain = txtdominioincoming.Text.Trim();
            oMailReceived.IP = txtipincoming.Text.Trim();
            oMailReceived.Port = txtportincoming.Text == string.Empty ? 0 : int.Parse(txtportincoming.Text.Trim());
            oMailReceived.CodeSSL = cboSSL_incoming.SelectedIndex == 1 ? "1" : "0";
            oMailReceived.TypeMail = "Reception";
            oMailReceived.IdEstado = Constantes.ValorUno;
        }


        private void PassValues()
        {
            FillObjectMail();
        }

        #endregion

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            var fail = Validate_ContentCampo();
            if (fail == Constantes.ValorCero)
            {
                //StartCharger(Views.Control.MainControl.oEmpresa);
                //llenar la empresa con mas campos
                PassValues();
                Views.Control.ViewDBSettings frmdb = new ViewDBSettings(oEmpresa, oMailSend, oMailReceived, oCert);
                //this.Close();
                frmdb.ShowDialog();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private int Validate_ContentCampo()
        {
            int cant_fail = Constantes.ValorCero;

            if (txtmailsend.Text.Length > Constantes.ValorTres)
            {
                bool isEmail = Regex.IsMatch(txtmailsend.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                if (isEmail == false)
                {
                    MessageBox.Show("Ingrese un Email valido, Campo necesario", "Info");
                    cant_fail++;
                    txtmailsend.Focus();
                    return cant_fail;
                }
            }

            if (txtmailincoming.Text.Length > Constantes.ValorTres)
            {
                bool isEmail = Regex.IsMatch(txtmailincoming.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (isEmail == false)
                {
                    MessageBox.Show("Ingrese un Email valido, Campo necesario", "Info");
                    cant_fail++;
                    txtmailincoming.Focus();
                    return cant_fail;
                }
            }

            if (txtpasswordsend.Password.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese Contraseña, Campo necesario", "Info");
                cant_fail++;
                txtpasswordsend.Focus();
                return cant_fail;
            }

            if (txtpasswordincoming.Password.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese Contraseña, Campo necesario", "Info");
                cant_fail++;
                txtpasswordincoming.Focus();
                return cant_fail;
            }

            if (txtdominiosend.Text.Length == Constantes.ValorCero && txtipsend.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese Dominio de correo ó Ingrese una IP, Dato Requerido", "Info");
                cant_fail++;
                txtdominiosend.Focus();
                return cant_fail;
            }

            if (txtdominioincoming.Text.Length == Constantes.ValorCero && txtipincoming.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese Dominio de correo ó Ingrese una IP, Dato Requerido", "Info");
                cant_fail++;
                txtdominioincoming.Focus();
                return cant_fail;
            }

            //if (txtipsend.Text.Length == Constantes.ValorCero)
            //{
            //    MessageBox.Show("Ingrese IP, Campo necesario", "Info");
            //    cant_fail++;
            //    txtipsend.Focus();
            //    return cant_fail;
            //}

            //if (txtipincoming.Text.Length == Constantes.ValorCero)
            //{
            //    MessageBox.Show("Ingrese IP, Campo necesario", "Info");
            //    cant_fail++;
            //    txtipincoming.Focus();
            //    return cant_fail;
            //}

            if (txtportsend.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese Puerto Saliente, Campo necesario", "Info");
                cant_fail++;
                txtportsend.Focus();
                return cant_fail;
            }

            if (txtportincoming.Text.Length == Constantes.ValorCero)
            {
                MessageBox.Show("Ingrese Puerto Entrante, Campo necesario", "Info");
                cant_fail++;
                txtportincoming.Focus();
                return cant_fail;
            }

            if (txtportsend.Text.Length > Constantes.ValorCero)
            {
                bool isNumber = Regex.IsMatch(txtportsend.Text, @"^\d{1,5}$", RegexOptions.IgnoreCase);
                if (isNumber == false)
                {
                    MessageBox.Show("Ingrese un valor Númerico de hasta 5 dígitos", "Info");
                    cant_fail++;
                    txtportsend.Focus();
                    return cant_fail;
                }
            }

            if (txtportincoming.Text.Length > Constantes.ValorCero)
            {
                bool isNumber = Regex.IsMatch(txtportincoming.Text, @"^\d{1,5}$", RegexOptions.IgnoreCase);
                if (isNumber == false)
                {
                    MessageBox.Show("Ingrese un valor Númerico de hasta 5 dígitos", "Info");
                    cant_fail++;
                    txtportincoming.Focus();
                    return cant_fail;
                }
            }

            if (txtipsend.Text.Length > Constantes.ValorCero)
            {
                bool isNumber = Regex.IsMatch(txtipsend.Text, @"^[0-9]{1,3}(\.[0-9]{1,3})(\.[0-9]{1,3})\.[0-9]{1,3}?$", RegexOptions.IgnoreCase);
                if (isNumber == false)
                {
                    MessageBox.Show("Ingrese un valor Númerico ejemplo: 192.168.90.1", "Info");
                    cant_fail++;
                    txtipsend.Focus();
                    return cant_fail;
                }
            }

            if (txtipincoming.Text.Length > Constantes.ValorCero)
            {
                bool isNumber = Regex.IsMatch(txtipincoming.Text, @"^[0-9]{1,3}(\.[0-9]{1,3})(\.[0-9]{1,3})\.[0-9]{1,3}?$", RegexOptions.IgnoreCase);
                if (isNumber == false)
                {
                    MessageBox.Show("Ingrese un valor Númerico ejemplo: 192.168.90.1", "Info");
                    cant_fail++;
                    txtipincoming.Focus();
                    return cant_fail;
                }
            }
            return cant_fail;
        }
    }
}
