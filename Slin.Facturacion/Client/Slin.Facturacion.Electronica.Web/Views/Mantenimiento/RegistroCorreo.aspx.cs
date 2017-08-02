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
using System.IO.Ports;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Mantenimiento
{
    public partial class RegistroCorreo : System.Web.UI.Page
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

        private void ObtenerUsuarioLogeado()
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
                ObtenerUsuarioLogeado();
                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();

                CargarLista();
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
                ObtenerUsuarioLogeado();

                oListaEmpresa = ServiceMantenimientoController.Instance.GetListaEmpresa();

                oListaEmpresa.Insert(0, new Empresa() { IdEmpresa = Constantes.ValorCero, RazonSocial = Constantes.ValorSeleccione });
                cboempresa.DataSource = oListaEmpresa;
                cboempresa.DataValueField = "IdEmpresa";
                cboempresa.DataTextField = "RazonSocial";
                cboempresa.DataBind();

                cboempresa.Value = oUsuarioLogeado.Empresa.IdEmpresa + string.Empty;


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

                btnGuardar.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnGuardar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

            }
            catch (Exception ex)
            {

            }
            
        }


        void LlenarObjeto()
        {
            try
            {
                ObtenerUsuarioLogeado();
                oCorreo = new WCFSeguridad.Correo();
                oCorreo.Estado = new WCFSeguridad.Estado();
                oCorreo.Empresa = new WCFSeguridad.Empresa();
                oCorreo.SSL = new WCFSeguridad.SSL();

                oCorreo.Empresa.IdEmpresa = Convert.ToInt32(cboempresa.Value);
                oCorreo.Email = txtemail.Value;
                oCorreo.Password = new Helper.Encrypt().EncryptKey(txtpassword.Value);
                //oCorreo.Password = new Helper.Encrypt().HashPassword(txtpassword.Value);//PWC

                oCorreo.Domain = txtDomain.Value.Length == Constantes.ValorCero ? "-" : txtDomain.Value;
                oCorreo.IP = txtIP.Value.Length == Constantes.ValorCero ? "-" : txtIP.Value;
                oCorreo.Port = int.Parse(txtPort.Value);

                oCorreo.Estado.IdEstado = int.Parse(cboestado.Value);
                oCorreo.Empresa.RUC = oUsuarioLogeado.Empresa.RUC.Length > 0 ? oUsuarioLogeado.Empresa.RUC : string.Empty;

                oCorreo.TypeMail = cboTypeMail.Value;
                oCorreo.SSL.Id = int.Parse(cboSSL.Value);
                //oCorreo.Password = txtpassword.Value;
            }
            catch (Exception ex) { }
        }


        public string msj = string.Empty;
        void GuardarRegistro()
        {
            try
            {
                if (cboempresa.SelectedIndex == Constantes.ValorCero)
                {
                    Response.Write("<script language=javascript>alert('Seleccione una Empresa');</script>");
                    cboempresa.Focus();
                }
                else
                {
                    LlenarObjeto();
                    var respuesta = ValidarExistsCorreoEmpresa(oCorreo);
                    if (respuesta != Constantes.msjRegistroExistente)
                    {
                        msj = ServiceSeguridadController.Instance.InsertarCorreo(oCorreo);
                        Session["MensajeCorreoRegistrado"] = Constantes.msjRegistrado;//msj;
                        Limpiar();
                        Response.Redirect("ListadoCorreo");
                    }
                    else
                    {                        
                        Response.Write("<script language=javascript>alert('" + Constantes.msjRegistroExistente +"');</script>");
                        txtemail.Focus();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        string ValidarExistsCorreoEmpresa(WCFSeguridad.Correo obj)
        {
            string result = string.Empty;
            LlenarObjeto();
            var listaresult = ServiceSeguridadController.Instance.ValidarExistsCorreoEmpresa(obj);

            if (listaresult.Count > 0)
            {
                result = Constantes.msjRegistroExistente;
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }

        ListaEstado listaestado = new ListaEstado();
        Estado estado = new Estado();

        void Limpiar()
        {
            txtemail.Value = string.Empty;
            txtpassword.Value = string.Empty;
            cboempresa.SelectedIndex = Constantes.ValorCero;
        }

        #endregion

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarRegistro();
        }
    }
}