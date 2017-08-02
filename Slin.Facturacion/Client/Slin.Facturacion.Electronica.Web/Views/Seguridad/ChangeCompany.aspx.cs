using Slin.Facturacion.Common;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using Slin.Facturacion.Electronica.Web.Views.Util;
using Slin.Facturacion.Proxies.ServicioMantenimiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;

namespace Slin.Facturacion.Electronica.Web.Views.Seguridad
{
    public partial class ChangeCompany : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
                //Message();
            }
        }

        #region entity

        ServiceWebFactController objController = new ServiceWebFactController();

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        private WCFSeguridad.Usuario userupdate;
        public WCFSeguridad.Usuario UserUpdate
        {
            get { return userupdate; }
            set { userupdate = value; }
        }

        private ListaEmpresa olistcompany;
        public ListaEmpresa oListCompany
        {
            get { return olistcompany; }
            set
            {
                olistcompany = value;
            }
        }

        #endregion

        #region method

        private void Message()
        {
            try
            {
                string respuesta = (string)Session["MsjeSessionChangeCompany"];
                if (respuesta.Length > Constantes.ValorCero)
                {
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                Session.Remove("MsjeSessionChangeCompany");
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
                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";
                ChargerList();
            }
            catch (Exception ex)
            {

            }
        }

        private void ChargerList()
        {
            oListCompany = new ListaEmpresa();
            oListCompany = ServiceMantenimientoController.Instance.GetListaEmpresa();
            oListCompany = ServiceMantenimientoController.Instance.Get_ListCompanyGroup(oUsuarioLogeado.Empresa.IdGrp);

            cboCompany.DataSource = oListCompany;
            cboCompany.DataTextField = "RazonSocial";
            cboCompany.DataValueField = "IdEmpresa";
            cboCompany.DataBind();

            //if (oListCompany.Count > Constantes.ValorCero)
            //{
            //    ChargerListProfileCompany(oListCompany[0].IdEmpresa);
            //}
        }

        private void ObtenerUsuarioLogeado()
        {
            try
            {
                oUsuarioLogeado = objController.GetUserLogueado_Fact();
            }
            catch (Exception ex) { }
        }

        private void CreateObjectUpdate()
        {
            ObtenerUsuarioLogeado();

            UserUpdate = new WCFSeguridad.Usuario();
            UserUpdate.Empresa = new WCFSeguridad.Empresa();
            UserUpdate.Perfil = new WCFSeguridad.Perfil();

            UserUpdate.IdUsuario = oUsuarioLogeado.IdUsuario;
            UserUpdate.Empresa.IdEmpresa = int.Parse(cboCompany.Value);
            UserUpdate.DNI = oUsuarioLogeado.Empleado.DNI;


            //string [] array = cboperfil.Value.Split('-');
            //UserUpdate.Perfil.IdPerfil = int.Parse(array[0]);
            UserUpdate.Perfil.IdPerfil = oUsuarioLogeado.Perfil.IdPerfil;

        }

        #endregion

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                CreateObjectUpdate();
                var msjeResult = ServiceSeguridadController.Instance.Update_UserCompany(UserUpdate);


                //var user = new WCFSeguridad.Usuario();
                //user.Username = oUsuarioLogeado.Username;
                //user.Password = oUsuarioLogeado.Password;

                var newUserSession = ServiceSeguridadController.Instance.GetUsuarioLogeado(oUsuarioLogeado);
                Session.Remove("UsuarioLogueadoFact");
                Session["UsuarioLogueadoFact"] = newUserSession;

                //Session["MsjeSessionChangeCompany"] = msjeResult;
                //Response.Redirect("ChangeCompany.aspx");

                //ObtenerUsuarioLogeado();

                lblmsje.InnerText = "¡" + msjeResult + "!";
                lblempresa.InnerText = newUserSession.Empresa.RazonSocial.ToUpper();
                logoEmpresa.Src = "~/Img/home/" + newUserSession.Empresa.RUC + ".png";
                
            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert( Message: '" + ex.Message + "');</script>");
            }
        }


        protected void cboCompany_ServerChange(object sender, EventArgs e)
        {
            try
            {
                //int IdComp = int.Parse(cboCompany.Value);
                //ChargerListProfileCompany(IdComp);
                //string script = @"<script type='text/javascript'>";
                //script += @"</script>";
                //Response.Write(script);
            }
            catch (Exception ex)
            {

            }
        }

        private void ChargerListProfileCompany(int IdComp)
        {
            ObtenerUsuarioLogeado();

            var list_Profile = ServiceSeguridadController.Instance.GetList_ProfileCompany(IdComp);

            var list = new WCFSeguridad.ListaPerfil();

            for (int i = 0; i<= list_Profile.Count - 1; i++)
            {
                string[] array = list_Profile[i].Codigo.Split('-');
                if (array[1] == oUsuarioLogeado.Perfil.Codigo)
                {
                    list.Add(list_Profile[i]);
                    break;
                }
            }
            cboperfil.DataSource = string.Empty;
            cboperfil.DataSource = list;
            cboperfil.DataTextField = "NombrePerfil";
            cboperfil.DataValueField = "Codigo";
            cboperfil.DataBind();
        }
    }
}