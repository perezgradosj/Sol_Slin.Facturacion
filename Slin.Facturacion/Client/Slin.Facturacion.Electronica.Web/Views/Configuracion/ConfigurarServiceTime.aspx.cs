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
using Slin.Facturacion.Proxies.ServicioMantenimiento;
using Slin.Facturacion.Electronica.Web.Helper.Config;

using SW = System.ServiceProcess;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Configuracion
{
    public partial class ConfigurarServiceTime : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
                Mensaje();
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

        private WCFConfiguracion.Services objupdate;
        public WCFConfiguracion.Services ObjUpdate
        {
            get { return objupdate; }
            set { objupdate = value; }
        }

        private WCFConfiguracion.Services objedited;
        public WCFConfiguracion.Services ObjEdited
        {
            get { return objedited; }
            set { objedited = value; }
        }

        private ListaEstado olistaEstado;
        public ListaEstado oListaEstado
        {
            get { return olistaEstado; }
            set { olistaEstado = value; }
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
                    Session.Remove("MensajeService");
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                
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

                btnDetener.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnDetener.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

                //btnActualizar.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                //btnActualizar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

                //btnIniciar.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                //btnIniciar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
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
                CargarListas();
                RecibirObjeto();
                CapturarUserLogRoles();
                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";

            }
            catch (Exception ex) { }
        }


        void RecibirObjeto()
        {
            try
            {
                ObjEdited = new WCFConfiguracion.Services();
                ObjEdited = (WCFConfiguracion.Services)Session["EditServiceSeleccionado"];

                txtidService.Value = string.Empty + ObjEdited.IdService;
                txtcodeService.Value = ObjEdited.CodeService;
                txtnameservice.Value = ObjEdited.NombreService;
                txthoraejecucion.Value = ObjEdited.ValueTime;
                txtintervalo.Value = ObjEdited.IntervaleValue;
                txtmaxintentosEnvio.Value = ObjEdited.MaxNumAttempts + string.Empty;

                cboestado.Value = ObjEdited.Estado.IdEstado.ToString();
                VerificarStatusService(ObjEdited.CodeService);
            }
            catch (Exception ex) { }
        }

        void CargarListas()
        {
            try
            {
                oListaEstado = ServiceMantenimientoController.Instance.GetListaEstado();

                cboestado.DataSource = oListaEstado;
                cboestado.DataValueField = "IdEstado";
                cboestado.DataTextField = "Descripcion";
                cboestado.DataBind();
            }
            catch (Exception ex) { }
        }


        private void VerificarStatusService(string nameService)
        {
            SW.ServiceController service = new SW.ServiceController(nameService);
            lblstatusService.InnerText = "El Estado del Servicio es: " + service.Status.ToString();

            switch (service.Status)
            {
                case SW.ServiceControllerStatus.Stopped:
                case SW.ServiceControllerStatus.StopPending:
                    {
                        btnIniciar.Enabled = true;
                        btnDetener.Enabled = false;
                        btnActualizar.Enabled = true;
                        break;
                    }
                case SW.ServiceControllerStatus.Running:
                case SW.ServiceControllerStatus.StartPending:
                    {
                        btnDetener.Enabled = true;
                        btnIniciar.Enabled = false;
                        btnActualizar.Enabled = false;
                        break;
                    }
            }
        }



        private void VerificarStatusService_btn(string nameService)
        {
            SW.ServiceController service = new SW.ServiceController(nameService);

            switch (service.Status)
            {
                case SW.ServiceControllerStatus.Stopped:
                case SW.ServiceControllerStatus.StopPending:
                    {
                        btnIniciar.Enabled = true;
                        btnDetener.Enabled = false;
                        break;
                    }
                case SW.ServiceControllerStatus.Running:
                case SW.ServiceControllerStatus.StartPending:
                    {
                        btnDetener.Enabled = true;
                        btnIniciar.Enabled = false;
                        break;
                    }
            }

        }

        #endregion

        void LlenarServiceEdit()
        {
            try
            {
                ObtenerUsuarioLogeado();
                ObjEdited = (WCFConfiguracion.Services)Session["EditServiceSeleccionado"];

                ObjUpdate = new WCFConfiguracion.Services();
                ObjUpdate.Estado = new WCFConfiguracion.Estado();
                ObjUpdate.Empresa = new WCFConfiguracion.Empresa();

                ObjUpdate.IdService = ObjEdited.IdService;
                ObjUpdate.CodeService = ObjEdited.CodeService;
                ObjUpdate.NombreService = ObjEdited.NombreService;
                ObjUpdate.ValueTime = txthoraejecucion.Value.Length == Constantes.ValorCero ? string.Empty : txthoraejecucion.Value;
                ObjUpdate.IntervaleValue = txtintervalo.Value.Length == Constantes.ValorCero ? string.Empty : txtintervalo.Value;
                ObjUpdate.MaxNumAttempts = txtmaxintentosEnvio.Value.Length == Constantes.ValorCero ? Constantes.ValorCero : int.Parse(txtmaxintentosEnvio.Value);
                ObjUpdate.Estado.IdEstado = Convert.ToInt32(cboestado.Value);

                ObjUpdate.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;

                ObjUpdate.CodeService = ObjEdited.CodeService;
            }
            catch (Exception ex) { }
        }

        void ActualizarRegistro()
        {
            try
            {
                LlenarServiceEdit();

                string msje = string.Empty;

                msje = ServiceConfiguracionController.Instance.UpdateTimeService(ObjUpdate);

                Session["MensajeService"] = Constantes.msjActualizado;

                CapturarObjUpdate();
                Response.Redirect("ConfigurarServiceTime");
            }
            catch (Exception ex) { }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            ActualizarRegistro();
        }


        private void CapturarObjUpdate()
        {
            Session.Remove("EditServiceSeleccionado");

            ObjUpdate = new WCFConfiguracion.Services();
            ObjUpdate.Estado = new WCFConfiguracion.Estado();
            ObjUpdate.Empresa = new WCFConfiguracion.Empresa();

            ObjUpdate.IdService = int.Parse(txtidService.Value);
            ObjUpdate.CodeService = txtcodeService.Value.TrimEnd();
            ObjUpdate.NombreService = txtnameservice.Value;

            ObjUpdate.ValueTime = txthoraejecucion.Value.Length == Constantes.ValorCero ? string.Empty : txthoraejecucion.Value;
            ObjUpdate.IntervaleValue = txtintervalo.Value.Length == Constantes.ValorCero ? string.Empty : txtintervalo.Value;
            ObjUpdate.MaxNumAttempts = txtmaxintentosEnvio.Value.Length == Constantes.ValorCero ? Constantes.ValorCero : int.Parse(txtmaxintentosEnvio.Value);
            ObjUpdate.Estado.IdEstado = Convert.ToInt32(cboestado.Value);

            ObjUpdate.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
            ObjUpdate.CodeService = ObjEdited.CodeService;

            Session["EditServiceSeleccionado"] = ObjUpdate;
        }

        protected void btnIniciar_Click(object sender, EventArgs e)
        {
            lblstatusService.InnerText = new ConfigService().Iniciar_Service(txtcodeService.Value.TrimEnd());
            VerificarStatusService_btn(txtcodeService.Value);
            btnActualizar.Enabled = false;
        }

        protected void btnDetener_Click(object sender, EventArgs e)
        {
            lblstatusService.InnerText = new ConfigService().Detener_Service(txtcodeService.Value.TrimEnd());
            VerificarStatusService_btn(txtcodeService.Value);
            btnActualizar.Enabled = true;
        }



        private void ProcessingStarting(string nameService)
        {            
            SW.ServiceController service = new SW.ServiceController(nameService);

            if (service != null && service.Status != SW.ServiceControllerStatus.Running)
            {
                ProcessingStarting(nameService);
            }
            else
            {
                Response.Redirect("ConfigurarServiceTime");
            }
        }


        private void ProcessingStopping(string nameService)
        {
            SW.ServiceController service = new SW.ServiceController(nameService);

            if (service != null && service.Status != SW.ServiceControllerStatus.Stopped)
            {
                ProcessingStopping(nameService);
            }
            else
            {
                Response.Redirect("ConfigurarServiceTime");
            }
        }
    }
}