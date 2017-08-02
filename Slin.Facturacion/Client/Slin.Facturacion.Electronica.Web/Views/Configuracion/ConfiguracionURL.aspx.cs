using Slin.Facturacion.Common;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using Slin.Facturacion.Electronica.Web.Views.Util;
using Slin.Facturacion.Proxies.ServicioConfiguracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;

namespace Slin.Facturacion.Electronica.Web.Views.Configuracion
{
    public partial class ConfiguracionURL : System.Web.UI.Page
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

        private ListaURLAmbiente olistURLAmbiente;
        public ListaURLAmbiente oListURLAmbiente
        {
            get { return olistURLAmbiente; }
            set { olistURLAmbiente = value; }
        }

        private AmbienteSunat oambienteSunat;
        public AmbienteSunat oAmbienteSunat
        {
            get { return oambienteSunat; }
            set { oambienteSunat = value; }
        }

        private ListaDocumentoAmbiente olistaDocumentoAmbiente;
        public ListaDocumentoAmbiente oListaDocumentoAmbiente
        {
            get { return olistaDocumentoAmbiente; }
            set { olistaDocumentoAmbiente = value; }
        }


        private URLAmbiente objurlamb;
        public URLAmbiente objUrlAmb
        {
            get { return objurlamb; }
            set { objurlamb = value; }
        }

        private ListaURLAmbiente objlisturlamb;
        public ListaURLAmbiente objListURLAmb
        {
            get { return objlisturlamb; }
            set { objlisturlamb = value; }
        }

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        #endregion

        #region METHOD

        private void Cargar()
        {
            string Url = HttpContext.Current.Request.Url.AbsoluteUri;
            int result = objController.Process_UrlPage(Url);
            if (result == Constantes.ValorCero)
            { Response.Redirect("~/Views/Home/Inicio.aspx"); }

            CargarLista();
            CapturarUserLogRoles();
            logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";
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
        private void CapturarUserLogRoles()
        {
            try
            {
                oListaRolesUserLog = objController.GetList_RolUserLog();

                btnGuardar.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnGuardar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

                btnEditTest.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnEdtiQA.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnEditProduc.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

                btnEditTest.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnEdtiQA.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnEditProduc.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
            }
            catch (Exception ex) { }
        }


        private void Mensaje()
        {
            try
            {
                string respuesta = (string)Session["MensajeUpdDocAmd"];
                if (respuesta == Constantes.msjActualizado)
                {
                    Session.Remove("MensajeUpdDocAmd");
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                
            }
            catch (Exception ex) { } 
        }

        private void CargarLista()
        {
            try
            {
                ObtenerUsuarioLogeado();
                oListURLAmbiente = ServiceConfiguracionController.Instance.GetListURLAmbienteSunat();
                Session["ListaURLAmbiente"] = oListURLAmbiente;

                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();

                #region AMBIENTE PRODUCTION
                {
                    idproducc.InnerText = string.Empty + oListURLAmbiente[0].AmbienteSunat.IdAmbiente;

                    txtPRODCE.Value = oListURLAmbiente[0].URL;
                    txtPRODGUIA.Value = oListURLAmbiente[1].URL;
                    txtPRODCPECRE.Value = oListURLAmbiente[2].URL;

                    IdUrlProdCE.InnerText = string.Empty + oListURLAmbiente[0].IdUrl;
                    IdUrlProdGUIA.InnerText = string.Empty + oListURLAmbiente[1].IdUrl;
                    IdUrlProdCPECRE.InnerText = string.Empty + oListURLAmbiente[2].IdUrl;
                }
                #endregion

                #region AMBIENTE TEST OR DEV
                {
                    idTest.InnerText = string.Empty + oListURLAmbiente[4].AmbienteSunat.IdAmbiente;

                    txtTestCE.Value = oListURLAmbiente[3].URL;
                    txtTestGUIA.Value = oListURLAmbiente[4].URL;
                    txtTestCPECRE.Value = oListURLAmbiente[5].URL;

                    IdUrlTestCE.InnerText = string.Empty + oListURLAmbiente[3].IdUrl;
                    IdUrlTestGUIA.InnerText = string.Empty + oListURLAmbiente[4].IdUrl;
                    IdUrlTestCPECRE.InnerText = string.Empty + oListURLAmbiente[5].IdUrl;
                }
                #endregion

                #region AMBIENTE HOMOLOGACION
                {
                    idhomolog.InnerText = string.Empty + oListURLAmbiente[6].AmbienteSunat.IdAmbiente;

                    txtQACEGUIACPECRE.Value = oListURLAmbiente[6].URL;

                    IdUrlQACEGUIACPECRE.InnerText = string.Empty + oListURLAmbiente[6].IdUrl;
                }
                #endregion

                #region CONSULTA DOC CON NRO TICKER OR ATENCION
                {
                    txtCONSALLDOC.Value = oListURLAmbiente[7].URL;
                    IdUrlConsCEGUIACPECRE.InnerText = string.Empty + oListURLAmbiente[7].IdUrl;
                }
                #endregion
            }
            catch (Exception ex) { }
        }


        private void GenerarListURLAmb()
        {
            try
            {
                objListURLAmb = new ListaURLAmbiente();


                // TEST - DEV
                objUrlAmb = new URLAmbiente();
                objUrlAmb.IdUrl = int.Parse(IdUrlTestCE.InnerText);
                objUrlAmb.URL = txtTestCE.Value;
                objListURLAmb.Add(objUrlAmb);

                objUrlAmb = new URLAmbiente();
                objUrlAmb.IdUrl = int.Parse(IdUrlTestGUIA.InnerText);
                objUrlAmb.URL = txtTestGUIA.Value;
                objListURLAmb.Add(objUrlAmb);

                objUrlAmb = new URLAmbiente();
                objUrlAmb.IdUrl = int.Parse(IdUrlTestCPECRE.InnerText);
                objUrlAmb.URL = txtTestCPECRE.Value;
                objListURLAmb.Add(objUrlAmb);


                // HOMOLOGACION QA

                objUrlAmb = new URLAmbiente();
                objUrlAmb.IdUrl = int.Parse(IdUrlQACEGUIACPECRE.InnerText);
                objUrlAmb.URL = txtQACEGUIACPECRE.Value;
                objListURLAmb.Add(objUrlAmb);


                // PRODUCTION

                objUrlAmb = new URLAmbiente();
                objUrlAmb.IdUrl = int.Parse(IdUrlProdCE.InnerText);
                objUrlAmb.URL = txtPRODCE.Value;
                objListURLAmb.Add(objUrlAmb);

                objUrlAmb = new URLAmbiente();
                objUrlAmb.IdUrl = int.Parse(IdUrlProdGUIA.InnerText);
                objUrlAmb.URL = txtPRODGUIA.Value;
                objListURLAmb.Add(objUrlAmb);

                objUrlAmb = new URLAmbiente();
                objUrlAmb.IdUrl = int.Parse(IdUrlProdCPECRE.InnerText);
                objUrlAmb.URL = txtPRODCPECRE.Value;
                objListURLAmb.Add(objUrlAmb);

                // CONSULTA CON NRO TICKET, OR NRO ATENCION
                objUrlAmb = new URLAmbiente();
                objUrlAmb.IdUrl = int.Parse(IdUrlConsCEGUIACPECRE.InnerText);
                objUrlAmb.URL = txtCONSALLDOC.Value;
                objListURLAmb.Add(objUrlAmb);
            }
            catch (Exception ex) { } 
        }

        public string  ActualizarListURL()
        {
            string msje = string.Empty;
            try
            {
                GenerarListURLAmb();
                msje = ServiceConfiguracionController.Instance.UpdateURLAmbiente(objListURLAmb);
            }
            catch (Exception ex) { }
            return msje;
        }

        #endregion

        protected void btnEditTest_Click(object sender, EventArgs e)
        {
            Session.Remove("ListaDocumentoAmbiente");
            Session.Remove("IdAmbiente");
            try
            {
                ObtenerUsuarioLogeado();
                int idamb = Int32.Parse(idTest.InnerText);
                oListaDocumentoAmbiente = ServiceConfiguracionController.Instance.GetListaDocAmbiente(idamb, oUsuarioLogeado.Empresa.RUC);
                Session["ListaDocumentoAmbiente"] = oListaDocumentoAmbiente;
                Session["IdAmbiente"] = idamb;
                Session["NombreAmbienteSeleccionado"] = Constantes.AmbienteTEST.ToUpper();
                Response.Redirect("DetalleAmbiente");
            }
            catch (Exception ex) { }
        }

        protected void btnEdtiQA_Click(object sender, EventArgs e)
        {
            try
            {
                ObtenerUsuarioLogeado();
                int idamb = Int32.Parse(idhomolog.InnerText);
                oListaDocumentoAmbiente = ServiceConfiguracionController.Instance.GetListaDocAmbiente(idamb, oUsuarioLogeado.Empresa.RUC);
                Session["ListaDocumentoAmbiente"] = oListaDocumentoAmbiente;
                Session["IdAmbiente"] = idamb;
                Session["NombreAmbienteSeleccionado"] = Constantes.AmbienteQA.ToUpper();
                Response.Redirect("DetalleAmbiente");
            }
            catch (Exception ex) { }
        }

        protected void btnEditProduc_Click(object sender, EventArgs e)
        {
            try
            {
                ObtenerUsuarioLogeado();
                int idamb = Int32.Parse(idproducc.InnerText);
                oListaDocumentoAmbiente = ServiceConfiguracionController.Instance.GetListaDocAmbiente(idamb, oUsuarioLogeado.Empresa.RUC);
                Session["ListaDocumentoAmbiente"] = oListaDocumentoAmbiente;
                Session["IdAmbiente"] = idamb;
                Session["NombreAmbienteSeleccionado"] = Constantes.AmbientePROD.ToUpper();
                Response.Redirect("DetalleAmbiente");
            }
            catch (Exception ex) { }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string result = ActualizarListURL();
                Response.Write("<script language=javascript>alert('" + result + "');</script>");
            }
            catch (Exception ex) { }
        }
    }
}