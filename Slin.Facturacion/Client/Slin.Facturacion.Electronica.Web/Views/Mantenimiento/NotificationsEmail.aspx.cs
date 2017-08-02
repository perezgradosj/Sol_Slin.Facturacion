using Slin.Facturacion.Common;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using Slin.Facturacion.Electronica.Web.Views.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;

namespace Slin.Facturacion.Electronica.Web.Views.Mantenimiento
{
    public partial class NotificationsEmail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
                Message();
            }
        }

        #region entity

        ServiceWebFactController objController = new ServiceWebFactController();

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        private WCFSeguridad.ListaCorreo list_mail;
        public WCFSeguridad.ListaCorreo OList_Mail
        {
            get { return list_mail; }
            set
            {
                list_mail = value;
            }
        }

        private WCFSeguridad.Correo o_mail;
        public WCFSeguridad.Correo O_Mail
        {
            get { return o_mail; }
            set
            {
                o_mail = value;
            }
        }

        #endregion

        #region method
        private void Message()
        {
            try
            {
                string respuesta = (string)Session["MessageMailNotify"];
                if (respuesta.Length > Constantes.ValorCero)
                {
                    Session.Remove("MessageMailNotify");
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }                
                
            }
            catch (Exception ex)
            {

            }
        }

        private void Cargar()
        {
            string Url = HttpContext.Current.Request.Url.AbsoluteUri;
            int result = objController.Process_UrlPage(Url);
            if (result == Constantes.ValorCero)
            { Response.Redirect("~/Views/Home/Inicio.aspx"); }

            try
            {
                ObtenerUsuarioLogeado();
                CapturarUserLogRoles();
                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();
                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";
                ChargerList();
            }
            catch (Exception ex)
            {

            }
        }

        private void ChargerList()
        {
            ObtenerUsuarioLogeado();
            OList_Mail = new WCFSeguridad.ListaCorreo();

            OList_Mail = ServiceSeguridadController.Instance.GetList_NotificationsMail(oUsuarioLogeado.Empresa.RUC);

            GVListNotificationsMail.DataSource = OList_Mail;
            GVListNotificationsMail.DataBind();

            var list = new Common.Helper.ListClass().GetListTypeMail_Notify();
            cboTypeMail.DataSource = list;
            cboTypeMail.DataValueField = "TypeMail";
            cboTypeMail.DataTextField = "Description";
            cboTypeMail.DataBind();


            Session["ListNotificationsMail"] = OList_Mail;
        }

        private void ObtenerUsuarioLogeado()
        {
            try
            {
                oUsuarioLogeado = objController.GetUserLogueado_Fact();
            }
            catch (Exception ex) { }
        }



        WCFSeguridad.ListaRol oListaRolesUserLog = new WCFSeguridad.ListaRol();
        void CapturarUserLogRoles()
        {
            try
            {
                oListaRolesUserLog = objController.GetList_RolUserLog();
                btnGuardar.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnGuardar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
            }
            catch (Exception ex) { }
        }


        private void CreateObjectMailNotify()
        {
            ObtenerUsuarioLogeado();
            O_Mail = new WCFSeguridad.Correo();
            O_Mail.Empresa = new WCFSeguridad.Empresa();

            O_Mail.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
            O_Mail.Email = txtemailnotify.Value;
            O_Mail.ContactName = txtcontactname.Value;
            O_Mail.TypeMail = cboTypeMail.Value;
            O_Mail.EmailSalida = lbltemp_mail.InnerText.Length < Constantes.ValorUno ? "1" : lbltemp_mail.InnerText;
        }

       

        #endregion

        private int ValidateEmailList()
        {
            var listmail = new List<string>();

            var list = (WCFSeguridad.ListaCorreo)Session["ListNotificationsMail"];

            for (int i = 0; i<= list.Count - 1; i++)
            {
                listmail.Add(list[i].Email);
            }

            if (listmail.Contains(txtemailnotify.Value) && lbltemp_mail.InnerText.Contains(cboTypeMail.Value))
            { return Constantes.ValorDos; }
            else
            { return Constantes.ValorUno; }
        }
         
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            var exists = ValidateEmailList();
            
            if (exists == Constantes.ValorUno)
            {
                if (txtemailnotify.Value.Length > Constantes.ValorCero)
                {
                    CreateObjectMailNotify();
                    var result = ServiceSeguridadController.Instance.Insert_MailToAlert(O_Mail);
                    Session["MessageMailNotify"] = result;
                    ChargerList();
                    Response.Redirect("NotificationsEmail");
                }
                else
                {
                    Response.Write("<script language=javascript>alert('Ingrese una cuenta de correo');</script>");
                }
            }else if (exists == Constantes.ValorDos)
            {
                Response.Write("<script language=javascript>alert('El Correo ya existe!');</script>");
            }

            
        }

        protected void GVListNotificationsMail_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            GVListNotificationsMail.EditIndex = e.NewSelectedIndex;
            GridViewRow row = GVListNotificationsMail.Rows[e.NewSelectedIndex];
            txtemailnotify.Value = GVListNotificationsMail.Rows[row.RowIndex].Cells[1].Text;
            txtcontactname.Value = GVListNotificationsMail.Rows[row.RowIndex].Cells[2].Text;


            string typemail = GVListNotificationsMail.Rows[row.RowIndex].Cells[3].Text;

            if (typemail.Contains("F")) { typemail = "F"; }
            else if (typemail.Contains("G")) { typemail = "S"; }

            cboTypeMail.Value = typemail;

            lbltemp_mail.InnerText = GVListNotificationsMail.Rows[row.RowIndex].Cells[1].Text;

            string script = @"<script type='text/javascript'>";
            script += @"</script>";
            Response.Write(script);
        }

        public string typeMail = string.Empty;
        public string CorreoSelect = string.Empty;
        protected void btnImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;
                //index = gvrow.RowIndex;

                txtemailnotify.Value = GVListNotificationsMail.Rows[gvrow.RowIndex].Cells[1].Text;
                txtcontactname.Value = GVListNotificationsMail.Rows[gvrow.RowIndex].Cells[2].Text;

                lbltemp_mail.InnerText = GVListNotificationsMail.Rows[gvrow.RowIndex].Cells[1].Text;


                string typemail = GVListNotificationsMail.Rows[gvrow.RowIndex].Cells[3].Text;

                if (typemail.Contains("F")) { typemail = "F"; lbltemp_mail.InnerText = "F"; }
                else if (typemail.Contains("S")) { typemail = "S"; lbltemp_mail.InnerText = "S"; }

                cboTypeMail.Value = typemail;
            }
            catch (Exception ex) { }
        }

        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ObtenerUsuarioLogeado();
                O_Mail = new WCFSeguridad.Correo();
                O_Mail.Empresa = new WCFSeguridad.Empresa();

                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;
                
                O_Mail.Email = GVListNotificationsMail.Rows[gvrow.RowIndex].Cells[1].Text;
                O_Mail.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
                

                string typemail = GVListNotificationsMail.Rows[gvrow.RowIndex].Cells[3].Text;

                if (typemail.Contains("F")) { typemail = "F"; lbltemp_mail.InnerText = "F"; }
                else if (typemail.Contains("S")) { typemail = "S"; lbltemp_mail.InnerText = "S"; }

                O_Mail.TypeMail = typemail;

                var result = ServiceSeguridadController.Instance.Delete_MailToAlert(O_Mail);

                Session["MessageMailNotify"] = result;
                ChargerList();
                Response.Redirect("NotificationsEmail");

            }
            catch (Exception ex) { }
        }
    }
}