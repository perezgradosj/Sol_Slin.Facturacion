using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Common;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using Slin.Facturacion.Proxies.ServicioMantenimiento;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Mantenimiento
{
    public partial class ActualizarCorreo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
            }
        }


        #region ENTITY

        ServiceWebFactController objController = new ServiceWebFactController();

        private ListaEmpresa olistaEmpresa;
        public ListaEmpresa oListaEmpresa
        {
            get { return olistaEmpresa; }
            set { olistaEmpresa = value; }
        }


        private WCFSeguridad.Correo ocorreo;
        public WCFSeguridad.Correo oCorreo
        {
            get { return ocorreo; }
            set { ocorreo = value; }
        }

        private WCFSeguridad.Empresa oempresa;
        public WCFSeguridad.Empresa oEmpresa
        {
            get { return oempresa; }
            set { oempresa = value; }
        }

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        private ListaEstado olistaEstado;
        public ListaEstado oListaEstado
        {
            get { return olistaEstado; }
            set { olistaEstado = value; }
        }

        private WCFSeguridad.ListaEmail objlisttypemail;
        public WCFSeguridad.ListaEmail objListTypeMail
        {
            get { return objlisttypemail; }
            set
            {
                objlisttypemail = value;
            }
        }

        private WCFSeguridad.ListSSL objlistssl;
        public WCFSeguridad.ListSSL objListSSL
        {
            get { return objlistssl; }
            set
            {
                objlistssl = value;
            }
        }

        #endregion


        #region METHOD

        void ObtenerUsuarioLogueado()
        {
            try
            {
                oUsuarioLogeado = objController.GetUserLogueado_Fact();
            }
            catch (Exception ex) { }
        }

        void Cargar()
        {
            string Url = HttpContext.Current.Request.Url.AbsoluteUri;
            int result = objController.Process_UrlPage(Url);
            if (result == Constantes.ValorCero)
            { Response.Redirect("~/Views/Home/Inicio.aspx"); }

            try
            {
                ObtenerUsuarioLogueado();


                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();
                CargarLista();
                RecibirCorreoSeleccionado();
                CapturarUserLogRoles();

                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";
            }
            catch (Exception ex)
            {

            }
            
        }

        void CargarLista()
        {
            try
            {
                oListaEmpresa = ServiceMantenimientoController.Instance.GetListaEmpresa();

                oListaEmpresa.Insert(0, new Empresa() { IdEmpresa = Constantes.ValorCero, RazonSocial = Constantes.ValorSeleccione });
                cboempresa.DataSource = oListaEmpresa;
                cboempresa.DataValueField = "IdEmpresa";
                cboempresa.DataTextField = "RazonSocial";
                cboempresa.DataBind();

                oListaEstado = new ListaEstado();
                oListaEstado = ServiceMantenimientoController.Instance.GetListaEstado();
                cboestado.DataSource = oListaEstado;
                cboestado.DataValueField = "IdEstado";
                cboestado.DataTextField = "Descripcion";
                cboestado.DataBind();

                objListTypeMail = new WCFSeguridad.ListaEmail();
                objListTypeMail = ServiceSeguridadController.Instance.GetListTypeMailEntity();

                cboTypeMail.DataSource = objListTypeMail;
                cboTypeMail.DataValueField = "TypeMail";
                cboTypeMail.DataTextField = "Description";
                cboTypeMail.DataBind();


                objListSSL = new WCFSeguridad.ListSSL();
                objListSSL = ServiceSeguridadController.Instance.GetListUseProt_SSL();

                cboSSL.DataSource = objListSSL;
                cboSSL.DataValueField = "Id";
                cboSSL.DataTextField = "Description";
                cboSSL.DataBind();
            }
            catch (Exception ex)
            {

            }
            
        }

        WCFSeguridad.ListaRol oListaRolesUserLog = new WCFSeguridad.ListaRol();
        void CapturarUserLogRoles()
        {
            try
            {
                oListaRolesUserLog = objController.GetList_RolUserLog();

                btnActualizar.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnActualizar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

            }
            catch (Exception ex)
            {

            }
            
        }


        void RecibirCorreoSeleccionado()
        {
            try
            {
                oCorreo = new WCFSeguridad.Correo();
                oCorreo = (WCFSeguridad.Correo)Session["EditCorreoSeleccionado"];

                txtemail.Value = oCorreo.Email;
                cboempresa.Value = oCorreo.Empresa.IdEmpresa.ToString();
                cboestado.Value = oCorreo.Estado.IdEstado + string.Empty;
                txtDomain.Value = oCorreo.Domain;
                txtIP.Value = oCorreo.IP;
                txtPort.Value = oCorreo.Port + string.Empty;

                cboSSL.Value = oCorreo.SSL.Id + string.Empty;
                cboTypeMail.Value = oCorreo.TypeMail;
            }
            catch (Exception ex)
            {

            }
        }

        public WCFSeguridad.Correo Editcorreo = new WCFSeguridad.Correo();
        void LlenarObjeto()
        {
            try
            {
                oCorreo = (WCFSeguridad.Correo)Session["EditCorreoSeleccionado"];

                Editcorreo = new WCFSeguridad.Correo();
                Editcorreo.Estado = new WCFSeguridad.Estado();
                Editcorreo.Empresa = new WCFSeguridad.Empresa();
                Editcorreo.SSL = new WCFSeguridad.SSL();

                Editcorreo.IdEmail = oCorreo.IdEmail;
                Editcorreo.Empresa.IdEmpresa = Convert.ToInt32(cboempresa.Value);
                Editcorreo.Email = txtemail.Value;  
                Editcorreo.Password = txtpassword.Value.Length == Constantes.ValorCero ? oCorreo.Password : new Helper.Encrypt().EncryptKey(txtpassword.Value);

                Editcorreo.Domain = txtDomain.Value.Length == Constantes.ValorCero ? "-" : txtDomain.Value;
                Editcorreo.IP = txtIP.Value.Length == Constantes.ValorCero ? "-" : txtIP.Value;
                Editcorreo.Port = int.Parse(txtPort.Value);
                Editcorreo.Estado.IdEstado = int.Parse(cboestado.Value);

                Editcorreo.Empresa.RUC = oCorreo.Empresa.RUC; // EL MISMO RUC QUE YA EXISTIA

                Editcorreo.TypeMail = cboTypeMail.Value;
                Editcorreo.SSL.Id = int.Parse(cboSSL.Value);
            }
            catch (Exception ex)
            {

            }
            


        }

        void ValidarCampos()
        {
            try
            {
                if (cboempresa.SelectedIndex == Constantes.ValorCero)
                {
                    Response.Write("<script language=javascript>alert('Seleccione Empresa');</script>");
                }
                else
                {
                    LlenarObjeto();

                    var respuesta = ValidarExistsCorreoEmpresa(oCorreo);
                    if (respuesta != Constantes.msjRegistroExistente)
                    {
                        string result = string.Empty;
                        result = ServiceSeguridadController.Instance.ActualizarCorreo(Editcorreo);
                        Session["MensajeCorreo"] = Constantes.msjActualizado;
                        Response.Redirect("ListadoCorreo");
                    }
                    else
                    {
                        Response.Write("<script language=javascript>alert('" + Constantes.msjRegistroExistente + "');</script>");
                        txtemail.Focus();
                    }

                    
                }
            }
            catch (Exception ex) { }
        }



        string ValidarExistsCorreoEmpresa(WCFSeguridad.Correo obj)
        {
            string result = string.Empty;
            var listaresult = ServiceSeguridadController.Instance.ValidarExistsCorreoEmpresa(obj);

            if (listaresult.Count > 1)
            {
                result = Constantes.msjRegistroExistente;
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }

        #endregion

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            ValidarCampos();
        }
    }
}