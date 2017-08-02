using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Proxies.ServicioFacturacion;
using Slin.Facturacion.Common;

using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Facturacion
{
    public partial class DetResumenRC : System.Web.UI.Page
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

        private FacturaElectronica odetalleRC;
        public FacturaElectronica oDetalleRC
        {
            get { return odetalleRC; }
            set { odetalleRC = value; }
        }

        private ListaDetalleFacturaElectronica olistaDetalleRC;
        public ListaDetalleFacturaElectronica oListaDetalleRC
        {
            get { return olistaDetalleRC; }
            set { olistaDetalleRC = value; }
        }


        private ListaFacturaElectronica olistaDocRC;
        public ListaFacturaElectronica oListaDocRC
        {
            get { return olistaDocRC; }
            set { olistaDocRC = value; }
        }

        private FacturaElectronica odocrc;
        public FacturaElectronica oDocRC
        {
            get { return odocrc; }
            set { odocrc = value; }
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
                oDetalleRC = (FacturaElectronica)Session["objDetalleRC"];
                oListaDetalleRC = (ListaDetalleFacturaElectronica)Session["objListaDetalleRC"];
            }
            catch (Exception ex) { } 
        }

        void CargarDatos()
        {
            try
            {
                ObtenerUsuarioLogeado();
                NombreEmpresaDetDoc.InnerText = oUsuarioLogeado.Empresa.RazonSocial;

                RecibirObjeto();
                LlenarCampos();
                GVListadoDetalleRC.DataSource = oListaDetalleRC;
                GVListadoDetalleRC.DataBind();
                ValidarVisibleCamp();
            }
            catch (Exception ex) { }
        }

        void LlenarCampos()
        {
            try
            {
                RecibirObjeto();
                cabecera.InnerText = "Detalle Reporte de Boletas del " + oDetalleRC.FechaInicio;
                txtnroticket.Value = oDetalleRC.NumeroAtencion;
                txtsecuencia.Value = oDetalleRC.Secuencia;
                txtfechaenvio.Value = oDetalleRC.FechaEnvio;

                txtmensaje.Value = oDetalleRC.MensajeEnvioDetalle;
                txtestado.Value = oDetalleRC.Estado.IdEstado.ToString();
                txtestadodescripcion.Value = oDetalleRC.Estado.Descripcion;
                var list = ServiceFacturacionController.Instance.Get_ExchangeRate_Today(Convert.ToDateTime(oDetalleRC.FechaInicio));

                if (list.Count == Constantes.ValorUno) { txtexchangerate.Value = list[0].Value + string.Empty; } else { txtexchangerate.Value = 0.00m + string.Empty; }
            }
            catch (Exception ex)
            {

            }
            
        }

        private void ValidarVisibleCamp()
        {
            try
            {
                foreach (var item in oListaDetalleRC)
                {
                    double inafecto = Constantes.ValorCero;
                    double exonerado = Constantes.ValorCero;
                    double otros = Constantes.ValorCero;
                    double isc = Constantes.ValorCero;
                    double otTributos = Constantes.ValorCero;

                    for (int i = 0; i <= oListaDetalleRC.Count - 1; i++)
                    {
                        //inafecto = inafecto + Convert.ToDouble(e.Row.Cells[6].Text);
                        //exonerado = exonerado + Convert.ToDouble(e.Row.Cells[7].Text);
                        //otros = otros + Convert.ToDouble(e.Row.Cells[8].Text);
                        //isc = isc + Convert.ToDouble(e.Row.Cells[9].Text);
                        //otTributos = otTributos + Convert.ToDouble(e.Row.Cells[11].Text);


                        inafecto = inafecto + Convert.ToDouble(GVListadoDetalleRC.Rows[i].Cells[6].Text);

                        exonerado = exonerado + Convert.ToDouble(GVListadoDetalleRC.Rows[i].Cells[7].Text);
                        otros = otros + Convert.ToDouble(GVListadoDetalleRC.Rows[i].Cells[8].Text);
                        isc = isc + Convert.ToDouble(GVListadoDetalleRC.Rows[i].Cells[9].Text);
                        otTributos = otTributos + Convert.ToDouble(GVListadoDetalleRC.Rows[i].Cells[11].Text);
                    }
                    if (inafecto < 1) GVListadoDetalleRC.Columns[6].Visible = false;
                    if (exonerado < 1) GVListadoDetalleRC.Columns[7].Visible = false;
                    if (otros < 1) GVListadoDetalleRC.Columns[8].Visible = false;
                    if (isc < 1) GVListadoDetalleRC.Columns[9].Visible = false;
                    if (otTributos < 1) GVListadoDetalleRC.Columns[11].Visible = false;
                }
            }
            catch (Exception ex) { }
        }

        #endregion


        protected void GVListadoDetalleRC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType != DataControlRowType.DataRow)
            //    return;

            //if (Convert.ToDecimal(e.Row.Cells[6].Text) < 1)
            //    GVListadoDetalleRC.Columns[6].Visible = false;

            //if (Convert.ToDecimal(e.Row.Cells[7].Text) < 1)
            //    GVListadoDetalleRC.Columns[7].Visible = false;

            //if (Convert.ToDecimal(e.Row.Cells[8].Text) < 1)
            //    GVListadoDetalleRC.Columns[8].Visible = false;

            //if (Convert.ToDecimal(e.Row.Cells[9].Text) < 1)
            //    GVListadoDetalleRC.Columns[9].Visible = false;

            //if (Convert.ToDecimal(e.Row.Cells[11].Text) < 1)
            //    GVListadoDetalleRC.Columns[11].Visible = false;

            //////double inafecto = Constantes.ValorCero;
            //////double exonerado = Constantes.ValorCero;
            //////double otros = Constantes.ValorCero;
            //////double isc = Constantes.ValorCero;
            //////double otTributos = Constantes.ValorCero;

            //////for (int i = 0; i <= oListaDetalleRC.Count - 1; i++)
            //////{
            //////    inafecto = inafecto + Convert.ToDouble(e.Row.Cells[6].Text);
            //////    exonerado = exonerado + Convert.ToDouble(e.Row.Cells[7].Text);
            //////    otros = otros + Convert.ToDouble(e.Row.Cells[8].Text);
            //////    isc = isc + Convert.ToDouble(e.Row.Cells[9].Text);
            //////    otTributos = otTributos + Convert.ToDouble(e.Row.Cells[11].Text);
            //////}
            //////if (inafecto < 1) GVListadoDetalleRC.Columns[6].Visible = false;
            //////if (exonerado < 1) GVListadoDetalleRC.Columns[7].Visible = false;
            //////if (otros < 1) GVListadoDetalleRC.Columns[8].Visible = false;
            //////if (isc < 1) GVListadoDetalleRC.Columns[9].Visible = false;
            //////if (otTributos < 1) GVListadoDetalleRC.Columns[11].Visible = false;

            //GVListadoDetalleRC.Columns[6].Visible = inafecto > 0 ? true : false;
        }
    }
}