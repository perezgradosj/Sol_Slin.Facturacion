using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using Slin.Facturacion.Common;
using Slin.Facturacion.Proxies.ServicioFacturacion;

using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Electronica.Web.Views.Util;
using System.Globalization;
using Slin.Facturacion.Common.Helper;

namespace Slin.Facturacion.Electronica.Web.Views.Facturacion
{
    public partial class AnularDocumento : System.Web.UI.Page
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

        private ListaTipoDocumento listaTipoDocumento;
        public ListaTipoDocumento ListaTipoDocumento
        {
            get { return listaTipoDocumento; }
            set { listaTipoDocumento = value; }
        }

        private FacturaElectronica odocumentoAnulado;
        public FacturaElectronica oDocumentoAnulado
        {
            get { return odocumentoAnulado; }
            set { odocumentoAnulado = value; }
        }


        private ListaDocumento olistadocAnulado;
        public ListaDocumento oListaDocAnulado
        {
            get { return olistadocAnulado; }
            set { olistadocAnulado = value; }
        }

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        #endregion

        #region METHOD
        void Cargar()
        {
            //string Url = HttpContext.Current.Request.Url.AbsoluteUri;
            int result = objController.Process_UrlPage(HttpContext.Current.Request.Url.AbsoluteUri);
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
            catch (Exception ex) { }
        }

        WCFSeguridad.ListaRol oListaRolesUserLog = new WCFSeguridad.ListaRol();
        void CapturarUserLogRoles()
        {
            try
            {
                oListaRolesUserLog = objController.GetList_RolUserLog();
                btnRegistrar.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnRegistrar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
            }
            catch (Exception ex) { }
        }

        void CargarLista()
        {
            try
            {
                ListaTipoDocumento = ServiceFacturacionController.Instance.ListarTipoDocumento();
                ListaTipoDocumento.RemoveAt(0);
                var lista = new ListaTipoDocumento();
                lista.Insert(0, new TipoDocumento() { IdTipoDocumento = 0, Descripcion = Constantes.ValorSeleccione });


                foreach (var tpodoc in ListaTipoDocumento)
                {
                    switch (tpodoc.CodigoDocumento)
                    {
                        case Constantes.Factura:
                        case Constantes.Boleta:
                        case Constantes.NotaCredito:
                        case Constantes.NotaDebito:
                        case Constantes.Retencion:
                            {
                                lista.Add(tpodoc);
                                break;
                            }
                    }
                }
                //cbotipodocumento.DataSource = ListaTipoDocumento;
                cbotipodocumento.DataSource = lista;
                cbotipodocumento.DataValueField = "IdTipoDocumento";
                cbotipodocumento.DataTextField = "Descripcion";
                cbotipodocumento.DataBind();
            }
            catch (Exception ex) { }
        }
        
        public string respuesta = string.Empty;
        
        void ValidarAnularDocumento()
        {
            try
            {
                oListaDocAnulado = new ListaDocumento();
                LlenarObjeto();


                if (txtmotivoanul.Value.Trim().Length < Constantes.ValorCinco)
                {
                    lblmensaje.InnerText = Constantes.msjValidaMotivoAnul;
                    DinamicPanel2.Visible = true;
                }
                else if (txtCodigoDocumento.Value.Length < Constantes.ValorUno && txtfecha.Value.Length < Constantes.ValorUno && txtmontototal.Value.Length < Constantes.ValorUno)
                {
                    //Response.Write("<script language=javascript>alert('" + Constantes.msjNoExisteDocIncor + "');</script>");
                    lblmensaje.InnerText = Constantes.msjNoExisteDocIncor;
                    DinamicPanel2.Visible = true;
                }
                else
                {
                    oListaDocAnulado = ServiceFacturacionController.Instance.ValidarExisteDocAnulado(oDocumentoAnulado);
                    if (oListaDocAnulado.Count > 0)
                    {
                        LimpiarCampos();
                        respuesta = Constantes.msjRegistroExistente;
                        lblmensaje.InnerText = respuesta;
                        DinamicPanel2.Visible = true;
                    }
                    else
                    {
                        respuesta = ServiceFacturacionController.Instance.InsertarDocumentoAnulado(oDocumentoAnulado);
                        LimpiarCampos();
                        lblmensaje.InnerText = respuesta;
                        DinamicPanel2.Visible = true;
                    }
                }
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

        void LlenarObjeto()
        {
            try
            {
                ObtenerUsuarioLogeado();
                oDocumentoAnulado = new FacturaElectronica();
                oDocumentoAnulado.TipoDocumento = new TipoDocumento();
                oDocumentoAnulado.Serie = new Serie();
                oDocumentoAnulado.Estado = new Estado();
                oDocumentoAnulado.Empresa = new Empresa();

                oDocumentoAnulado.TipoDocumento.IdTipoDocumento = Convert.ToInt32(cbotipodocumento.Value) == Constantes.ValorCero ? Constantes.ValorCero : Convert.ToInt32(cbotipodocumento.Value);
                oDocumentoAnulado.TipoDocumento.CodigoDocumento = txtCodigoDocumento.Value;
                oDocumentoAnulado.Serie.NumeroSerie = txtserie.Value.ToUpper();
                oDocumentoAnulado.NumeroDocumento = string.Format("{0:00000000}", int.Parse(txtnrodocumento.Value));
                //oDocumentoAnulado.FechaAnulado = odocfecha.FechaEmision;
                oDocumentoAnulado.FechaAnulado = Convert.ToDateTime(txtfecha.Value);
                oDocumentoAnulado.MotivoAnulado = txtmotivoanul.Value;
                oDocumentoAnulado.Estado.IdEstado = Constantes.ValorCero;
                oDocumentoAnulado.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
                oDocumentoAnulado.Usuario = oUsuarioLogeado.Username;
            }
            catch (Exception ex) { }   
        }

        void LimpiarCampos()
        {
            cbotipodocumento.Value = Constantes.ValorCero.ToString();
            txtserie.Value = string.Empty;
            txtnrodocumento.Value = string.Empty;
            txtmotivoanul.Value = string.Empty;
            txtfecha.Value = string.Empty;
            txtmontototal.Value = string.Empty;
            DinamicPanel.Visible = false;
            DinamicPanel2.Visible = false;

            btnRegistrar.Enabled = false;
        }
        #endregion

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            ValidarAnularDocumento();
        }

        FacturaElectronica odocfecha = new FacturaElectronica();
        void ConsultarDocumento()
        {
            try
            {
                ObtenerUsuarioLogeado();

                odocfecha = new FacturaElectronica();
                odocfecha = ServiceFacturacionController.Instance.GetFechaDocumento(Convert.ToInt32(cbotipodocumento.Value), txtserie.Value, string.Format("{0:00000000}", int.Parse(txtnrodocumento.Value)), oUsuarioLogeado.Empresa.RUC);

                if (odocfecha.NumeroSerie == null)
                {
                    DinamicPanel.Visible = false;
                    DinamicPanel2.Visible = true;
                    lblmensaje.InnerText = "El Documento no Existe o aún no ha sido aceptado por sunat";
                    btnRegistrar.Enabled = false;
                    txtfecha.Value = string.Empty;
                    txtmontototal.Value = string.Empty;
                    txtCodigoDocumento.Value = string.Empty;
                }
                else
                {
                    txtfecha.Value = odocfecha.FechaEmision.ToShortDateString();
                    var monto = Convert.ToDecimal(odocfecha.MontoTotalCad, CultureInfo.CreateSpecificCulture("es-PE"));
                    txtmontototal.Value = monto.ToString("N2");
                    txtCodigoDocumento.Value = odocfecha.TipoDocumento.CodigoDocumento;
                    lblmonto_total.InnerText = "Monto Total (" + odocfecha.Moneda.Descripcion + "):";
                    DinamicPanel2.Visible = false;
                    DinamicPanel.Visible = true;
                    lblmensaje.InnerText = string.Empty;
                    btnRegistrar.Enabled = true;
                }
            }
            catch (Exception ex) { }
        }

        protected void btnConsultar_ServerClick(object sender, EventArgs e)
        {
            ConsultarDocumento();
        }

    }

}