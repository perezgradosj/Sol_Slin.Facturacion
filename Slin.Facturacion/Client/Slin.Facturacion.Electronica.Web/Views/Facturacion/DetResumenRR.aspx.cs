using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Proxies.ServicioFacturacion;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Facturacion
{
    public partial class DetResumenRR : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                CargarDatos();
            }
        }

        #region ENTITY

        ServiceWebFactController objController = new ServiceWebFactController();

        private FacturaElectronica odetalleRA;
        public FacturaElectronica oDetalleRA
        {
            get { return odetalleRA; }
            set { odetalleRA = value; }
        }

        private ListaDetalleFacturaElectronica olistaDetalleRA;
        public ListaDetalleFacturaElectronica oListaDetalleRA
        {
            get { return olistaDetalleRA; }
            set { olistaDetalleRA = value; }
        }

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        #endregion

        #region METHOD

        void ObtenerUsuarioLogeado()
        {
            try
            {
                oUsuarioLogeado = objController.GetUserLogueado_Fact();
            }
            catch (Exception ex) { }
        }

        void RecibirObjeto()
        {
            try
            {
                oDetalleRA = (FacturaElectronica)Session["objDetalleRR"];
                oListaDetalleRA = (ListaDetalleFacturaElectronica)Session["objListaDetalleRR"];
            }
            catch (Exception ex)
            {

            }

        }


        void CargarDatos()
        {
            try
            {
                ObtenerUsuarioLogeado();
                NombreEmpresaDetDoc.InnerText = oUsuarioLogeado.Empresa.RazonSocial;

                RecibirObjeto();
                GVListadoDetalleRR.DataSource = oListaDetalleRA;
                GVListadoDetalleRR.DataBind();
                LlenarCampos();
            }
            catch (Exception ex)
            {

            }

        }

        void LlenarCampos()
        {
            try
            {
                cabecera.InnerText = "Detalle Reporte de Documentos Anulados del " + oDetalleRA.FechaInicio;

                txtnroticket.Value = oDetalleRA.NumeroAtencion;
                txtsecuencia.Value = oDetalleRA.Secuencia;

                txtfechaenvio.Value = oDetalleRA.FechaEnvio;

                txtestado.Value = oDetalleRA.Estado.IdEstado.ToString();
                txtestadodescripcion.Value = oDetalleRA.Estado.Descripcion;
                txtmensaje.Value = oDetalleRA.MensajeEnvioDetalle;

                //NombreEmpresaDetDoc.InnerText = oDetalleRA.Empresa.RazonSocial;
            }
            catch (Exception ex)
            {

            }

        }

        #endregion
    }
}