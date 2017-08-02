using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Common;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Configuracion
{
    public partial class ConfiguracionAmbienteTrabj : System.Web.UI.Page
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

        private WCFSeguridad.ListaAmbienteSunat objlistamb;
        public WCFSeguridad.ListaAmbienteSunat ObjListAmb
        {
            get { return objlistamb; }
            set { objlistamb = value; }
        }

        private WCFSeguridad.AmbienteTrabjActual objambtrabj;
        public WCFSeguridad.AmbienteTrabjActual ObjAmbTrabj
        {
            get { return objambtrabj; }
            set { objambtrabj = value; }
        }

        private WCFSeguridad.AmbienteTrabjActual oambtrbjActual;
        public WCFSeguridad.AmbienteTrabjActual oAmbTrbjActual
        {
            get { return oambtrbjActual; }
            set { oambtrbjActual = value; }
        }

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        #endregion


        #region METHOD

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

        void ObtenerUsuarioLogeado()
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

            CargarLista();
            CapturarUserLogRoles();
            logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";
        }

        void CargarLista()
        {
            try
            {
                ObtenerUsuarioLogeado();
                oAmbTrbjActual = new WCFSeguridad.AmbienteTrabjActual();
                ObjListAmb = new WCFSeguridad.ListaAmbienteSunat();

                ObjListAmb = ServiceSeguridadController.Instance.GetListAmbTrabj();

                ObjListAmb.Insert(0, new WCFSeguridad.AmbienteSunat() { IdAmbiente = Constantes.ValorCero, Descripcion = Constantes.ValorSeleccione });

                cboambiente.DataSource = ObjListAmb;
                cboambiente.DataValueField = "IdAmbiente";
                cboambiente.DataTextField = "Descripcion";
                cboambiente.DataBind();

                oAmbTrbjActual = (WCFSeguridad.AmbienteTrabjActual)Session["AmbTrabjActual"];

                if (oAmbTrbjActual.ID == Constantes.ValorCero)
                {
                    lblambienteDesc.InnerText = "Sin Ambiente, Seleccione un Ambiente para Registrar";
                }
                else
                {
                    lblambienteDesc.InnerText = oAmbTrbjActual.DESCRIPCION.ToUpper();
                }
                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();
            }
            catch (Exception ex) { }
        }


        void LlenarObjetoUpdate()
        {
            ObtenerUsuarioLogeado();
            ObjAmbTrabj = new WCFSeguridad.AmbienteTrabjActual();
            ObjAmbTrabj.Empresa = new WCFSeguridad.Empresa();
            ObjAmbTrabj.ID = int.Parse(cboambiente.Value);
            ObjAmbTrabj.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
        }


        void UpdateAmbTrabjAct()
        {
            try
            {
                LlenarObjetoUpdate();
                string msje = ServiceSeguridadController.Instance.UpdateAmbTrabjActual(ObjAmbTrabj);

                Response.Write("<script language=javascript>alert('" + msje + "');</script>");
            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('" + ex.Message + "');</script>");
            }
        }

        #endregion

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cboambiente.SelectedIndex == Constantes.ValorCero)
            {
                Response.Write("<script language=javascript>alert('Seleccione un Ambiente a Establecer como Ambiente Actual');</script>");
            }
            else
            {
                UpdateAmbTrabjAct();
            }
        }
    }
}