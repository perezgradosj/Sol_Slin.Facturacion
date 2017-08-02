using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Common;
using Slin.Facturacion.Proxies.ServicioFacturacion;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;

using System.Web.UI.DataVisualization.Charting;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Home
{
    public partial class Inicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Mensaje();
                Cargar();
                DeleteRegistroLogueox3Month();
            }
        }

        #region ENTITY

        ServiceWebFactController objController = new ServiceWebFactController();

        private ListaSistemaEstado olistaSistemaEstado_ok;
        public ListaSistemaEstado oListaSistemaEstado_Ok
        {
            get { return olistaSistemaEstado_ok; }
            set { olistaSistemaEstado_ok = value; }
        }

        private ListaSistemaEstado olistaSistemaEstado_error;
        public ListaSistemaEstado oListaSistemaEstado_Error
        {
            get { return olistaSistemaEstado_error; }
            set { olistaSistemaEstado_error = value; }
        }

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
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

        private void Mensaje()
        {
            try
            {
                string respuesta = (string)Session["MensajeCambiarClave"];
                if (respuesta == Constantes.msjClaveCambiada)
                {
                    //Session.Remove("MensajeCambiarClave");
                    Session.Remove("MensajeCambiarClave");
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                
            }
            catch (Exception ex)
            { }
        }

        private void Cargar()
        {
            try
            {
                CargarListas();

                ObtenerUsuarioLogeado();

                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";
                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();
            }
            catch (Exception ex)
            { }
        }


        private void CargarListas()
        {
            ObtenerUsuarioLogeado();

            oListaSistemaEstado_Ok = ServiceFacturacionController.Instance.GetListaEstadoSistema_Ok(oUsuarioLogeado.Empresa.RUC);
            GVEstadoSistema_Ok.DataSource = oListaSistemaEstado_Ok;
            GVEstadoSistema_Ok.DataBind();

            oListaSistemaEstado_Error = ServiceFacturacionController.Instance.GetListaEstadoSistema_Error(oUsuarioLogeado.Empresa.RUC);
            

            

            int index = oListaSistemaEstado_Error.Count;

            var list = new ListaSistemaEstado();

            var objTot = oListaSistemaEstado_Error[index - 1];
            list.Add(objTot);

            GVTotDocumentosADE.DataSource = list;
            GVTotDocumentosADE.DataBind();



            oListaSistemaEstado_Error.RemoveAt(index - 1);
            GVEstadoSistema_Error.DataSource = oListaSistemaEstado_Error;
            GVEstadoSistema_Error.DataBind();

            




            //grafico1.DataSource = oListaSistemaEstado_Ok;
            //grafico1.Width = 250;
            //grafico1.Height = 250;
            //grafico1.Titles.Add("Estado");
            //Series fseries = new Series();
            //fseries.ChartType = SeriesChartType.Bar;
            //fseries.XValueType = ChartValueType.Int32;
            //fseries.XValueMember = "UltimaSemana";
            //fseries.YValueType = ChartValueType.Int32;
            //fseries.YValueMembers = "Criterio";
            //grafico1.Series.Add(fseries);
            //grafico1.DataBind();


            grafico1.DataSource = oListaSistemaEstado_Ok;
            grafico1.Titles.Add("Aceptados");
            grafico1.DataBind();


            grafico2.DataSource = oListaSistemaEstado_Error;
            grafico2.Titles.Add("Rechazados");
            grafico2.DataBind();

        }


        private void AsignarDatosGrafico()
        {
            
        }

        #endregion

        protected void GVEstadoSistema_Ok_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GVEstadoSistema_Ok_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void GVEstadoSistema_Error_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GVEstadoSistema_Error_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void chkenable3D_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkenable3D.Checked == true)
                {
                    Cargar();
                    grafico1.ChartAreas[0].Area3DStyle.Enable3D = true;
                }
                else
                {
                    Cargar();
                    grafico1.ChartAreas[0].Area3DStyle.Enable3D = false;
                }
            }
            catch (Exception ex)
            { }
        }

        #region DELETE REGISTRO LOGUEO X 3 MONTH

        string fecha3month = string.Empty;

        private void DeleteRegistroLogueox3Month()
        {
            try
            {
                ObtenerUsuarioLogeado();
                var fecha = DateTime.Now.ToShortDateString();
                var fecha3 = DateTime.Now.AddDays(-90);

                fecha3month = fecha3.ToShortDateString();

                var result = ServiceSeguridadController.Instance.DeleteRegistroLogueox3M(fecha3month, oUsuarioLogeado.Empresa.RUC);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        protected void GVTotDocumentosADE_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GVTotDocumentosADE_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void GVEstadoSistema_Error_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (int.Parse(e.Row.Cells[1].Text) + int.Parse(e.Row.Cells[2].Text) + int.Parse(e.Row.Cells[3].Text) + int.Parse(e.Row.Cells[4].Text) + int.Parse(e.Row.Cells[5].Text) + int.Parse(e.Row.Cells[6].Text) > 0)
                    {
                        e.Row.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            { }
        }
    }
}