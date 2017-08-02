using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using WCFConfiguracion = Slin.Facturacion.Proxies.ServicioConfiguracion;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using Slin.Facturacion.Common;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Configuracion
{
    public partial class ConfiguracionTimeService : System.Web.UI.Page
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

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        private WCFConfiguracion.ListService listservice;
        public WCFConfiguracion.ListService oListaService
        {
            get { return listservice; }
            set { listservice = value; }
        }

        private WCFConfiguracion.Services objselect;
        public WCFConfiguracion.Services ObjSelect
        {
            get { return objselect; }
            set { objselect = value; }
        }

        #endregion

        #region METHOD

        void Mensaje()
        {
            try
            {
                string respuesta = (string)Session["MensajeService"];
                if (respuesta == Constantes.msjActualizado)
                {
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                Session.Remove("MensajeService");
            }
            catch (Exception ex)
            {

            }
        }

        void ObtenerUsuarioLogeado()
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
                //btnGuardar.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                //btnGuardar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
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
                Session.Remove("ListServiceSession");
                oListaService = new WCFConfiguracion.ListService();

                oListaService = ServiceConfiguracionController.Instance.GetListTimeService(oUsuarioLogeado.Empresa.RUC);

                GVListService.DataSource = oListaService;
                GVListService.DataBind();

                Session["ListServiceSession"] = oListaService;
            }
            catch (Exception ex)
            {

            }
        }


        #endregion

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }

        protected void cboservice_ServerChange(object sender, EventArgs e)
        {
            
        }



        void PasarDatosServiceSelected()
        {
            try
            {
                ObjSelect = new WCFConfiguracion.Services();
                ObjSelect.Estado = new WCFConfiguracion.Estado();
                ObjSelect.Empresa = new WCFConfiguracion.Empresa();

                for (int i = 0; i <= oListaService.Count - 1; i++)
                {
                    if (IdserviceSelect == oListaService[i].IdService)
                    {
                        ObjSelect.IdService = oListaService[i].IdService;
                        ObjSelect.CodeService = oListaService[i].CodeService;
                        ObjSelect.NombreService = oListaService[i].NombreService;
                        ObjSelect.ValueTime = oListaService[i].ValueTime;
                        ObjSelect.IntervaleValue = oListaService[i].IntervaleValue;
                        ObjSelect.MaxNumAttempts = oListaService[i].MaxNumAttempts;
                        ObjSelect.Empresa.RUC = oListaService[i].Empresa.RUC;
                        ObjSelect.Estado.IdEstado = oListaService[i].Estado.IdEstado;

                        Session["EditServiceSeleccionado"] = ObjSelect;
                        break;
                    }
                }
                Response.Redirect("ConfigurarServiceTime");
            }
            catch (Exception ex)
            {

            }

        }

        public int IdserviceSelect = Constantes.ValorCero;
        protected void btnImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                oListaService = (WCFConfiguracion.ListService)Session["ListServiceSession"];

                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;
                //index = gvrow.RowIndex;
                IdserviceSelect = int.Parse(GVListService.Rows[gvrow.RowIndex].Cells[1].Text);
                PasarDatosServiceSelected();
            }
            catch (Exception ex)
            {

            }
        }
    }
}