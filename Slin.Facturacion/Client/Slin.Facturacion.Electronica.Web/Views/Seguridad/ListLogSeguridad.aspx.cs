using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;

using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using Slin.Facturacion.Common;
using System.IO;
using Slin.Facturacion.Electronica.Web.Views.Util;
using Slin.Facturacion.Common.Method;

namespace Slin.Facturacion.Electronica.Web.Views.Seguridad
{
    public partial class ListLogSeguridad : System.Web.UI.Page
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

        private WCFSeguridad.ListaTipoLog olistatipolog;
        public WCFSeguridad.ListaTipoLog oListaTipoLog
        {
            get { return olistatipolog; }
            set { olistatipolog = value; }
        }

        private WCFSeguridad.ListaLogueoClass olistalogueoClass;
        public WCFSeguridad.ListaLogueoClass oListaLogueoClass
        {
            get { return olistalogueoClass; }
            set { olistalogueoClass = value; }
        }

        private WCFSeguridad.LogueoClass ologueoclass;
        public WCFSeguridad.LogueoClass oLogueoClass
        {
            get { return ologueoclass; }
            set { ologueoclass = value; }
        }

        private WCFSeguridad.ListaPerfil olistaperfil;
        public WCFSeguridad.ListaPerfil oListaPerfil
        {
            get { return olistaperfil; }
            set { olistaperfil = value; }
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

        WCFSeguridad.ListaRol oListaRolesUserLog = new WCFSeguridad.ListaRol();
        void CapturarUserLogRoles()
        {
            try
            {
                oListaRolesUserLog = objController.GetList_RolUserLog();

                btnBuscar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnPDF.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnPDF.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

                btnExportarExcel.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnExportarExcel.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
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
                CapturarUserLogRoles();

                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";

            }
            catch (Exception ex) { }
        }

        void CargarListas()
        {
            try
            {
                ObtenerUsuarioLogeado();
                oListaTipoLog = ServiceSeguridadController.Instance.GetListTipoLog();

                oListaTipoLog.Insert(0, new WCFSeguridad.TipoLog() { ID = Constantes.ValorCero, Descripcion = Constantes.ValorTodos });

                cbotipolog.DataSource = oListaTipoLog;
                cbotipolog.DataValueField = "ID";
                cbotipolog.DataTextField = "Descripcion";
                cbotipolog.DataBind();

                txtfechadesde.Value = DateTime.Now.AddDays(-7).ToShortDateString();
                txtfechahasta.Value = DateTime.Now.ToString("dd/MM/yyyy");


                oListaPerfil = ServiceSeguridadController.Instance.GetListaPerfiles(oUsuarioLogeado.Empresa.RUC);
                oListaPerfil.Insert(0, new WCFSeguridad.Perfil() { IdPerfil = Constantes.ValorCero, NombrePerfil = Constantes.ValorTodos });

                cboperfil.DataSource = oListaPerfil;
                cboperfil.DataValueField = "IdPerfil";
                cboperfil.DataTextField = "NombrePerfil";
                cboperfil.DataBind();
                
            }
            catch (Exception ex) { }
        }


        public Int32 val = Constantes.ValorCero;
        //public DateTime fecha1;
        //public DateTime fecha2;

        void ValidarParametros()
        {
            try
            {
                //if (txtfechadesde.Value.Length > 0)
                //{
                //    fecha1 = Convert.ToDateTime(txtfechadesde.Value);
                //    fecha2 = Convert.ToDateTime(txtfechahasta.Value);
                //    int result = DateTime.Compare(fecha1, fecha2);
                //    val = result < 0 ? 2 : result;
                //}
                //else
                //{
                //    val = Constantes.ValorDos;
                //}
                val = Singleton.Instance.Validate_RangeDate(txtfechadesde.Value, txtfechahasta.Value);
            }
            catch (Exception ex) { }
        }


        void ObtenerDatos()
        {
            try
            {
                ObtenerUsuarioLogeado();

                oLogueoClass = new WCFSeguridad.LogueoClass();
                oLogueoClass.TipoLog = new WCFSeguridad.TipoLog();
                oLogueoClass.Empresa = new WCFSeguridad.Empresa();
                oLogueoClass.Perfil = new WCFSeguridad.Perfil();

                oLogueoClass.FechaDesde = txtfechadesde.Value;
                oLogueoClass.FechaHasta = txtfechahasta.Value;
                oLogueoClass.TipoLog.ID = Convert.ToInt32(cbotipolog.Value);
                oLogueoClass.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;

                oLogueoClass.Perfil.IdPerfil = int.Parse(cboperfil.Value);
                oLogueoClass.Username = txtusername.Value.Length == Constantes.ValorCero ? string.Empty : txtusername.Value;
            }
            catch (Exception ex)
            {

            }

        }

        void CargarData()
        {
            try
            {
                Session.Remove("ListaLogSeguridad");
                ValidarParametros();
                if (val == Constantes.ValorUno)
                {
                    Response.Write(Constantes.MensajeFecha);
                }
                else if (val == Constantes.ValorDos)
                {
                    ObtenerDatos();
                    oListaLogueoClass = ServiceSeguridadController.Instance.GetListLogueoClass(oLogueoClass);
                    GVLogSeguridad.DataSource = oListaLogueoClass;
                    GVLogSeguridad.DataBind();
                }
                else if (val == Constantes.ValorCero)
                {
                    ObtenerDatos();
                    oListaLogueoClass = ServiceSeguridadController.Instance.GetListLogueoClass(oLogueoClass);
                    GVLogSeguridad.DataSource = oListaLogueoClass;
                    GVLogSeguridad.DataBind();
                }
                Session["ListaLogSeguridad"] = oListaLogueoClass;
            }
            catch (Exception ex)
            {

            }

        }

        #endregion

        protected void btnBuscar_ServerClick(object sender, EventArgs e)
        {
            try
            {
                GVLogSeguridad.PageIndex = 0;
                GVLogSeguridad.DataSourceID = "";
                GVLogSeguridad.DataBind();
                CargarData();

                string script = @"<script type='text/javascript'>";
                script += @"</script>";
                Response.Write(script);
            }
            catch (Exception ex)
            {

            }
        }



        #region UTIL
        void CrearNuevaCarpeta()
        {
            try
            {
                string newfolder = Server.MapPath("~/DocumentoXML/");
                if (!Directory.Exists(newfolder))
                {
                    Directory.CreateDirectory(newfolder);
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region EXPORT TO EXCEL

        void EnviarToExcel()
        {
            try
            {
                CrearNuevaCarpeta();
                ObtenerDatos();
                var lista = new WCFSeguridad.ListaLogueoClass();
                lista = ServiceSeguridadController.Instance.GetListLogueoClass(oLogueoClass);
                Session["ExcelListLogueoClass"] = lista;
                Session["TipoExport"] = Constantes.ValorExportExcel;
                Response.Redirect("../Informes/ExportReportExcel");
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (GVLogSeguridad.Rows.Count == 0)
                {
                    Response.Write("<script language=javascript>alert('No se ha realizado una consulta previa');</script>");
                }
                else
                {
                    EnviarToExcel();
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region EXPORT TO PDF
        void EnviarToPDF()
        {
            try
            {
                CrearNuevaCarpeta();
                ObtenerDatos();
                var lista = new WCFSeguridad.ListaLogueoClass();
                lista = ServiceSeguridadController.Instance.GetListLogueoClass(oLogueoClass);
                Session["ExcelListLogueoClass"] = lista;
                Session["TipoExport"] = Constantes.ValorExportPDF;
                Response.Redirect("../Informes/ExportReportExcel");
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (GVLogSeguridad.Rows.Count == 0)
                {
                    Response.Write("<script language=javascript>alert('No se ha realizado una consulta previa');</script>");
                }
                else
                {
                    EnviarToPDF();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}