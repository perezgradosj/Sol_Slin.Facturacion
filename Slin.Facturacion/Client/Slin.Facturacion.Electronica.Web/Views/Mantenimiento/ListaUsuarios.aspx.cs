using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Common;
using Slin.Facturacion.Proxies.ServicioMantenimiento;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using System.IO;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Mantenimiento
{
    public partial class ListaUsuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
                Mensaje();
                MensajeRegistrado();
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

        ListaEstado oListaEstado = new ListaEstado();
        ListaEmpresa oListaEmpresa = new ListaEmpresa();

        WCFSeguridad.ListaUsuario oListaUsuario = new WCFSeguridad.ListaUsuario();
        WCFSeguridad.Usuario oUsuario = new WCFSeguridad.Usuario();

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

        void Mensaje()
        {
            try
            {
                string respuesta = (string)Session["MensajeUsuario"];
                if (respuesta == Constantes.msjActualizado)
                {
                    Session.Remove("MensajeUsuario");
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                
            }
            catch (Exception ex) { }         
        }

        void MensajeRegistrado()
        {
            try
            {
                string respuesta = (string)Session["MensajeUsuarioRegistrado"];
                if (respuesta == Constantes.msjRegistrado)
                {
                    Session.Remove("MensajeUsuarioRegistrado");
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                
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
                btnNuevo.Visible = false;

                ObtenerUsuarioLogeado();
                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();
                CargarListas();
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

                btnBuscar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

                btnNuevo.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

                btnExportarExcel.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnExportarExcel.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

                btnPDF.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnPDF.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

            }
            catch (Exception ex) { }
        }

        void CargarListas()
        {
            try
            {
                ObtenerUsuarioLogeado();

                oListaEstado = new ListaEstado();

                oListaEstado = ServiceMantenimientoController.Instance.GetListaEstado();

                oListaEstado.Insert(0, new Estado() { IdEstado = Constantes.ValorCero, Descripcion = Constantes.ValorTodos });

                cbobuscarestado.DataSource = oListaEstado;
                cbobuscarestado.DataValueField = "IdEstado";
                cbobuscarestado.DataTextField = "Descripcion";
                cbobuscarestado.DataBind();

                oListaEmpresa = new ListaEmpresa();

                oListaEmpresa = ServiceMantenimientoController.Instance.GetListaEmpresa();
                oListaEmpresa.Insert(0, new Empresa() { IdEmpresa = Constantes.ValorCero, RazonSocial = Constantes.ValorTodos });
                cbobuscarempresa.DataSource = oListaEmpresa;
                cbobuscarempresa.DataValueField = "IdEmpresa";
                cbobuscarempresa.DataTextField = "RazonSocial";
                cbobuscarempresa.DataBind();
                cbobuscarempresa.Value = oUsuarioLogeado.Empresa.IdEmpresa.ToString();
            }
            catch (Exception ex) { }
        }


        void LlenarObjetoBusqueda()
        {
            try
            {
                ObtenerUsuarioLogeado();

                oUsuario = new WCFSeguridad.Usuario();
                oUsuario.Empresa = new WCFSeguridad.Empresa();
                oUsuario.Empleado = new WCFSeguridad.Empleado();
                oUsuario.Estado = new WCFSeguridad.Estado();

                oUsuario.Empresa.IdEmpresa = Convert.ToInt32(cbobuscarempresa.Value);
                oUsuario.Estado.IdEstado = Convert.ToInt32(cbobuscarestado.Value);
                oUsuario.Empleado.CodEmpleado = txtbuscarcodigo.Value.Length == Constantes.ValorCero ? string.Empty : txtbuscarcodigo.Value;
                oUsuario.Username = txtbuscarusername.Value.Length == Constantes.ValorCero ? string.Empty : txtbuscarusername.Value;

                oUsuario.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
            }
            catch (Exception ex) { }
        }

        void CargarListaUsuarios()
        {
            try
            {
                Session.Remove("ListadoUsuarios");
                LlenarObjetoBusqueda();
                oListaUsuario = ServiceSeguridadController.Instance.GetListaUsuario(oUsuario);
                GVListaUsuarios.DataSource = oListaUsuario;
                GVListaUsuarios.DataBind();

                Session["ListadoUsuarios"] = oListaUsuario;
            }
            catch (Exception ex) { }
        }

        #endregion


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                GVListaUsuarios.PageIndex = 0;
                GVListaUsuarios.DataSourceID = "";
                GVListaUsuarios.DataBind();
                CargarListaUsuarios();
            }
            catch (Exception ex) { }
        }

        void PasarDatosUsuario()
        {
            try
            {
                oUsuario = new WCFSeguridad.Usuario();

                oUsuario.Empleado = new WCFSeguridad.Empleado();
                oUsuario.Estado = new WCFSeguridad.Estado();
                oUsuario.Empresa = new WCFSeguridad.Empresa();
                oUsuario.Perfil = new WCFSeguridad.Perfil();
                oUsuario.Sede = new WCFSeguridad.Sede();

                for (int i = 0; i <= oListaUsuario.Count - 1; i++)
                {
                    if (DNIUsuario == oListaUsuario[i].Empleado.DNI)
                    {
                        oUsuario.IdUsuario = oListaUsuario[i].IdUsuario;
                        oUsuario.Empleado.Nombres = oListaUsuario[i].Empleado.Nombres;
                        oUsuario.Empleado.ApePaterno = oListaUsuario[i].Empleado.ApePaterno;
                        oUsuario.Empleado.ApeMaterno = oListaUsuario[i].Empleado.ApeMaterno;
                        oUsuario.Empleado.DNI = oListaUsuario[i].Empleado.DNI;
                        oUsuario.Empleado.Direccion = oListaUsuario[i].Empleado.Direccion;
                        oUsuario.Empleado.Telefono = oListaUsuario[i].Empleado.Telefono;
                        oUsuario.Empleado.Email = oListaUsuario[i].Empleado.Email;
                        oUsuario.Estado.IdEstado = oListaUsuario[i].Estado.IdEstado;
                        oUsuario.Empresa.IdEmpresa = oListaUsuario[i].Empresa.IdEmpresa;
                        oUsuario.Username = oListaUsuario[i].Username;
                        oUsuario.Password = oListaUsuario[i].Password;
                        oUsuario.Perfil.IdPerfil = oListaUsuario[i].Perfil.IdPerfil;
                        oUsuario.FechaExpiracion = oListaUsuario[i].FechaExpiracion;
                        oUsuario.Sede.Name = oListaUsuario[i].Sede.Name;

                        Session["EditUsuarioSeleccionado"] = oUsuario;
                        break;
                    }
                }
                Response.Redirect("ActualizarUsuario");
            }
            catch (Exception ex) { }
        }

        public string DNIUsuario = string.Empty;
        protected void btnImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                oListaUsuario = (WCFSeguridad.ListaUsuario)Session["ListadoUsuarios"];

                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;
                //index = gvrow.RowIndex;
                DNIUsuario = GVListaUsuarios.Rows[gvrow.RowIndex].Cells[2].Text;
                PasarDatosUsuario();
            }
            catch (Exception ex) { }
        }

        #region EXPORT EXCEL

        private WCFSeguridad.Usuario objusuario;
        public WCFSeguridad.Usuario ObjUsuario
        {
            get { return objusuario; }
            set { objusuario = value; }
        }

        private WCFSeguridad.ListaUsuario objlistusuario;
        public WCFSeguridad.ListaUsuario ObjListaUsuario
        {
            get { return objlistusuario; }
            set { objlistusuario = value; }
        }

        void EnviarToExcel()
        {
            try
            {
                Common.Method.Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));

                var lista = new WCFSeguridad.ListaUsuario();
                lista = (WCFSeguridad.ListaUsuario)Session["ListadoUsuarios"];

                Session["ExcelListUsuario"] = lista;
                Session["TipoExport"] = Constantes.ValorExportExcel;
                Response.Redirect("../Informes/ExportReportListUsuario");
            }
            catch (Exception ex) { }
        }

        void EnviarToPDF()
        {
            try
            {                
                Common.Method.Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));
                var lista = new WCFSeguridad.ListaUsuario();
                //lista = ServiceSeguridadController.GetListaUsuario(oUsuario);
                lista = (WCFSeguridad.ListaUsuario)Session["ListadoUsuarios"];

                Session["ExcelListUsuario"] = lista;
                Session["TipoExport"] = Constantes.ValorExportPDF;
                Response.Redirect("../Informes/ExportReportListUsuario");
            }
            catch (Exception ex) { }
        }

        #endregion END EXPORT EXCEL

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (GVListaUsuarios.Rows.Count == 0)
                {
                    Response.Write("<script language=javascript>alert('No se ha realizado una consulta previa');</script>");
                }
                else
                { EnviarToExcel(); }
            }
            catch (Exception ex) { }
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (GVListaUsuarios.Rows.Count == 0)
                {
                    Response.Write("<script language=javascript>alert('No se ha realizado una consulta previa');</script>");
                }
                else
                { EnviarToPDF(); }
            }
            catch (Exception ex) { }
        }

    }
}