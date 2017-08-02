using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Proxies.ServicioFacturacion;
using Slin.Facturacion.Common;
using System.IO;

namespace Slin.Facturacion.Electronica.Web.Views.Facturacion
{
    public partial class DetalleDoc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                RecibirFactura();
            }
        }


        #region ENTITY
        FacturaElectronica oDocumentoCab = new FacturaElectronica();
        ListaDetalleFacturaElectronica oListaDetalle = new ListaDetalleFacturaElectronica();
        ListaFacturaElectronica listamonto = new ListaFacturaElectronica();

        public string TextoNexto = string.Empty;
        #endregion

        #region METHOD
        void RecibirFactura()
        {
            try
            {
                oDocumentoCab = (FacturaElectronica)Session["DocumentoCab"];
                oListaDetalle = (ListaDetalleFacturaElectronica)Session["DocumentoDet"];
                //oListaMontos = (ListaFacturaElectronica)Session["ListaMontos"];
                PasarDatosVista();
            }
            catch (Exception ex) { }
        }

        void PasarDatosVista()
        {
            try
            {
                txttipodocumentocod.Value = oDocumentoCab.TipoDocumento.CodigoDocumento;
                txttipodocDescripcion.Value = oDocumentoCab.TipoDocumento.Descripcion;
                txtserie.Value = oDocumentoCab.Serie.NumeroSerie;
                txtcorrelativo.Value = oDocumentoCab.NumeroDocumento;

                txtfechaemision.Value = oDocumentoCab.FechaEmision.ToShortDateString();
                txtrucempresa.Value = oDocumentoCab.Empresa.RUC;
                txtrazonsocialempresa.Value = oDocumentoCab.Empresa.RazonSocial;
                txtdireccionempresa.Value = oDocumentoCab.Empresa.Direccion;
                txtubigeo.Value = oDocumentoCab.Empresa.Ubigeo.CodigoUbigeo;
                txtciudad.Value = oDocumentoCab.Empresa.Ubigeo.Descripcion;

                txtclientenrodocumento.Value = oDocumentoCab.Cliente.NumeroDocumentoIdentidad.Length == 0 ? String.Empty : oDocumentoCab.Cliente.NumeroDocumentoIdentidad;
                txtclienterazonsocial.Value = oDocumentoCab.Cliente.Nombres.Length == 0 ? String.Empty : oDocumentoCab.Cliente.Nombres;
                txtmoneda.Value = oDocumentoCab.TipoMoneda;
                txttasaigv.Value = Constantes.ValorIGV.ToString();
                NombreEmpresaDetDoc.InnerText = oDocumentoCab.Empresa.RazonSocial;
                txtEstadoDocumentoBD.Value = oDocumentoCab.Estado.Descripcion;

                {
                    if (oDocumentoCab.CodeMessage != null && oDocumentoCab.DocMessage != null)
                    {
                        if (oDocumentoCab.CodeMessage == "0")
                        {
                            txtNotaRespuesta.Value = oDocumentoCab.DocMessage;
                        }
                        else
                        {
                            txtNotaRespuesta.Value = oDocumentoCab.CodeMessage + " - " + oDocumentoCab.DocMessage;
                        }
                    }
                }

                txtobservacion.Value = oDocumentoCab.CodeResponse + " - " + oDocumentoCab.NoteResponse;

                txtmontoneto.Value = oDocumentoCab.TotalGravado;
                txtmontototal.Value = oDocumentoCab.MontoTotalCad;
                txtmontoigv.Value = oDocumentoCab.MontoIgvCad;
                txtmontogratis.Value = oDocumentoCab.TotalDescuento;
                txtmontoinafecto.Value = oDocumentoCab.TotalnoGravado == "0" ? "0.00" : oDocumentoCab.TotalnoGravado;
                txtmontoexonerado.Value = oDocumentoCab.TotalExonerado;


                GridView1.DataSource = oListaDetalle;
                GridView1.DataBind();
            }
            catch (Exception ex) { }  
        }
        #endregion
    }
}