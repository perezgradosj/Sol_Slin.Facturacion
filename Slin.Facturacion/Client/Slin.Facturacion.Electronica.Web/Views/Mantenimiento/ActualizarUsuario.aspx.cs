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
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Mantenimiento
{
    public partial class ActualizarUsuario : System.Web.UI.Page
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

        private WCFSeguridad.Usuario ousuario;
        public WCFSeguridad.Usuario oUsuario
        {
            get { return ousuario; }
            set { ousuario = value; }
        }

        private ListaEstado olistaEstado;
        public ListaEstado oListaEstado
        {
            get { return olistaEstado; }
            set { olistaEstado = value; }
        }

        private ListaEmpresa olistaEmpresa;
        public ListaEmpresa oListaEmpresa
        {
            get { return olistaEmpresa; }
            set { olistaEmpresa = value; }
        }

        private WCFSeguridad.ListaPerfil olistaPerfil;
        public WCFSeguridad.ListaPerfil oListaPerfil
        {
            get { return olistaPerfil; }
            set { olistaPerfil = value; }
        }

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }


        private WCFSeguridad.ListaRol objlistarol;
        public WCFSeguridad.ListaRol objListaRol
        {
            get { return objlistarol; }
            set { objlistarol = value; }
        }

        private WCFSeguridad.ListaSede olistasede;
        public WCFSeguridad.ListaSede oListaSede
        {
            get { return olistasede; }
            set { olistasede = value; }
        }
        #endregion

        #region METHOD

        void ObtenerUsuarioLogueado()
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

                btnActualizar.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnActualizar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
               
            }
            catch (Exception ex)
            {

            }
        }

        void RecibirObjeto()
        {
            try
            {
                oUsuario = new WCFSeguridad.Usuario();
                oUsuario = (WCFSeguridad.Usuario)Session["EditUsuarioSeleccionado"];


                txtnombres.Value = oUsuario.Empleado.Nombres;
                txtapepaterno.Value = oUsuario.Empleado.ApePaterno;
                txtapematerno.Value = oUsuario.Empleado.ApeMaterno;
                txtdni.Value = oUsuario.Empleado.DNI;
                txtdireccion.Value = oUsuario.Empleado.Direccion;
                txttelefono.Value = oUsuario.Empleado.Telefono;
                txtemail.Value = oUsuario.Empleado.Email;
                txtusername.Value = oUsuario.Username;
                txtpassword.Value = oUsuario.Password;
                txtfechaexpiracion.Value = oUsuario.FechaExpiracion.ToShortDateString();

                cboempresa.Value = oUsuario.Empresa.IdEmpresa.ToString();
                cboestado.Value = oUsuario.Estado.IdEstado.ToString();
                cboperfil.Value = oUsuario.Perfil.IdPerfil.ToString();
                cbosede.Value = oUsuario.Sede.Name;


                #region This method is only to tecniservices
                if (cboperfil.Value == Constantes.ValorSiete + string.Empty)
                {
                    cbosede.Disabled = true;
                }
                else if (cboperfil.Value == Constantes.ValorDiez + string.Empty)
                {
                    cbosede.Disabled = true;
                }
                else
                {
                    cbosede.Disabled = false;
                }
                #endregion

                //ObtenerPermisosUsuarioEdit();
            }
            catch (Exception ex)
            {

            }
            
        }


        WCFSeguridad.ListaRol oListaRolUsuarioEdit = new WCFSeguridad.ListaRol();
        public WCFSeguridad.ListaRol AllListaRol = new WCFSeguridad.ListaRol();

        void Cargar()
        {
            string Url = HttpContext.Current.Request.Url.AbsoluteUri;
            int result = objController.Process_UrlPage(Url);
            if (result == Constantes.ValorCero)
            { Response.Redirect("~/Views/Home/Inicio.aspx"); }

            try
            {
                ObtenerUsuarioLogueado();
                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();
                CargarListas();
                RecibirObjeto();
                CapturarUserLogRoles();

                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";

                //PARA EL TREEVIEW
                objListaRol = ServiceSeguridadController.Instance.GetListadoRol();
                foreach (var obj in objListaRol)
                {
                    if (obj.CodigoRol == Constantes.RolNuevo)
                    {
                        objListaRol.Remove(obj);
                        break;
                    }
                }
                CreateeTreeView(objListaRol, 0, null);
                HabilitarCheckContent(oUsuario);
            }
            catch (Exception ex)
            {

            }
            
        }

        void CargarListas()
        {
            try
            {
                ObtenerUsuarioLogueado();
                oListaEmpresa = ServiceMantenimientoController.Instance.GetListaEmpresa();

                cboempresa.DataSource = oListaEmpresa;
                cboempresa.DataValueField = "IdEmpresa";
                cboempresa.DataTextField = "RazonSocial";
                cboempresa.DataBind();

                oListaEstado = ServiceMantenimientoController.Instance.GetListaEstado();

                cboestado.DataSource = oListaEstado;
                cboestado.DataValueField = "IdEstado";
                cboestado.DataTextField = "Descripcion";
                cboestado.DataBind();

                oListaPerfil = ServiceSeguridadController.Instance.GetListaPerfiles(oUsuarioLogeado.Empresa.RUC);
                cboperfil.DataSource = oListaPerfil;
                cboperfil.DataValueField = "IdPerfil";
                cboperfil.DataTextField = "NombrePerfil";
                cboperfil.DataBind();


                oListaSede = new WCFSeguridad.ListaSede();

                oListaSede = ServiceSeguridadController.Instance.GetListSede(oUsuarioLogeado.Empresa.RUC);

                oListaSede.Insert(0, new WCFSeguridad.Sede() { IdSede = Constantes.ValorCero, Name = Constantes.ValorSeleccione });

                cbosede.DataSource = oListaSede;
                cbosede.DataValueField = "Name";
                cbosede.DataTextField = "Name";
                cbosede.DataBind();

                AllListaRol = ServiceSeguridadController.Instance.GetListadoRol();
                Session["oFullListaRol"] = AllListaRol;
            }
            catch (Exception ex)
            {

            }
            
        }


        WCFSeguridad.Usuario usuario = new WCFSeguridad.Usuario();
        void LlenarObjeto()
        {
            try
            {
                oUsuario = (WCFSeguridad.Usuario)Session["EditUsuarioSeleccionado"];

                usuario = new WCFSeguridad.Usuario();
                usuario.Empleado = new WCFSeguridad.Empleado();
                usuario.Estado = new WCFSeguridad.Estado();
                usuario.Perfil = new WCFSeguridad.Perfil();
                usuario.Empresa = new WCFSeguridad.Empresa();
                usuario.Sede = new WCFSeguridad.Sede();

                usuario.IdUsuario = oUsuario.IdUsuario;
                usuario.Estado.IdEstado = Convert.ToInt32(cboestado.Value);
                usuario.Empresa.IdEmpresa = Convert.ToInt32(cboempresa.Value);
                usuario.Perfil.IdPerfil = Convert.ToInt32(cboperfil.Value);
                usuario.Empleado.Nombres = txtnombres.Value.Length == 0 ? string.Empty : txtnombres.Value;
                usuario.Empleado.ApePaterno = txtapepaterno.Value.Length == 0 ? string.Empty : txtapepaterno.Value;
                usuario.Empleado.ApeMaterno = txtapematerno.Value.Length == 0 ? string.Empty : txtapematerno.Value;
                usuario.Empleado.DNI = txtdni.Value.Length == 0 ? string.Empty : txtdni.Value;
                usuario.Empleado.Direccion = txtdireccion.Value.Length == 0 ? string.Empty : txtdireccion.Value;
                usuario.Empleado.Telefono = txttelefono.Value.Length == 0 ? string.Empty : txttelefono.Value;
                usuario.Empleado.Email = txtemail.Value.Length == 0 ? string.Empty : txtemail.Value;
                usuario.Username = txtusername.Value.Length == 0 ? string.Empty : txtusername.Value;

                usuario.NuevoPassword = txtpassword.Value.Length == Constantes.ValorCero ? oUsuario.Password : new Helper.Encrypt().HashPassword(txtpassword.Value);

                usuario.FechaExpiracion = txtfechaexpiracion.Value.Length == 0 ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(txtfechaexpiracion.Value);

                //usuario.Sede.Name = cbosede.Value;
                //usuario.Sede.Name = cbosede.Value == Constantes.ValorSeleccione ? Constantes.ValorAdmin : cbosede.Value;

                if (cbosede.Value == Constantes.ValorSeleccione)
                {
                    usuario.Sede.Name = string.Empty;
                }
                else { usuario.Sede.Name = cbosede.Value; }
            }
            catch (Exception ex) { }
        }

        #endregion


        #region VALIDAR

        public WCFSeguridad.ListaUsuario listarespuesta = new WCFSeguridad.ListaUsuario();
        public string respuesta = string.Empty;

        void ValidarUsername()
        {
            try
            {
                listarespuesta = ServiceSeguridadController.Instance.ValidarUsername(txtusername.Value);

                if (listarespuesta.Count > 1)
                {
                    Response.Write("<script language=javascript>alert('Usuario ya Existe');</script>");
                }
                else
                {
                    LlenarObjeto();
                    respuesta = ServiceSeguridadController.Instance.ActualizarUsuario(usuario);
                    ActualizarRolesDelUsuarioSeleccionado();
                    Session["MensajeUsuario"] = respuesta;
                    Response.Redirect("ListaUsuarios");
                }
            }
            catch (Exception ex) { }
        }

        public string msjRol = string.Empty;
        void CapturarDatosSession()
        {
            try
            {
                AllListaRol = (WCFSeguridad.ListaRol)Session["oFullListaRol"];
                oUsuario = (WCFSeguridad.Usuario)Session["EditUsuarioSeleccionado"];
            }
            catch (Exception ex) { }
        }

        #endregion

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            ValidarUsername();
        }

        #region TREEVIEW

        WCFSeguridad.ListaUsuario listaRolUserAddorUpdate = new WCFSeguridad.ListaUsuario();
        WCFSeguridad.ListaUsuario listaRolUserDelete = new WCFSeguridad.ListaUsuario();
        WCFSeguridad.Usuario ouser = new WCFSeguridad.Usuario();

        private void LlenarListaRolUpdate()
        {
            try
            {
                listaRolUserAddorUpdate = new WCFSeguridad.ListaUsuario();
                listaRolUserDelete = new WCFSeguridad.ListaUsuario();

                for (int i = 0; i <= TreeView1.Nodes.Count - 1; i++)
                {
                    ouser = new WCFSeguridad.Usuario();
                    ouser.Empleado = new WCFSeguridad.Empleado();
                    ouser.Rol = new WCFSeguridad.Rol();

                    ouser.Empleado.DNI = txtdni.Value; //cambiar por el objetoUsuarioSessionSeleccionado;
                    ouser.Rol.IdRol = int.Parse(TreeView1.Nodes[i].Value);

                    if (TreeView1.Nodes[i].Checked == true)
                    {
                        listaRolUserAddorUpdate.Add(ouser);
                    }
                    else
                    {
                        listaRolUserDelete.Add(ouser);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ActualizarRolesDelUsuarioSeleccionado()
        {
            LlenarListaRolUpdate();

            try
            {
                if (listaRolUserAddorUpdate.Count > 0)
                {
                    foreach (var objInserUpdate in listaRolUserAddorUpdate)
                    {
                        var result = ServiceSeguridadController.Instance.RegistrarUsuarioRol(objInserUpdate);
                    }
                }

                if (listaRolUserDelete.Count > 0)
                {
                    foreach (var objDel in listaRolUserDelete)
                    {
                        var result = ServiceSeguridadController.Instance.DeleteUsuarioRol(objDel);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('" + msjRol + "');</script>");
            }
        }


        void HabilitarCheckContent(WCFSeguridad.Usuario ouser)
        {
            //oUsuario = (WCFSeguridad.Usuario)Session["EditUsuarioSeleccionado"]; //SE CONSIDERA QUE TODO ESTA CORRIENDO EN EL MISMO MOMENTO
            WCFSeguridad.ListaRol oListaRolUsuarioEdit = new WCFSeguridad.ListaRol();
            oListaRolUsuarioEdit = ServiceSeguridadController.Instance.GetListaRolesUsuario(ouser);
            try
            {
                for (int i = 0; i <= TreeView1.Nodes.Count - 1; i++)
                {
                    for (int mp = 0; mp <= oListaRolUsuarioEdit.Count - 1; mp++)
                    {
                        if (TreeView1.Nodes[i].Value == oListaRolUsuarioEdit[mp].IdRol.ToString())
                        {
                            TreeView1.Nodes[i].Checked = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void CreateeTreeView(WCFSeguridad.ListaRol source, int PadreRol, TreeNode parentNode)
        {
            try
            {
                var nuevalista = source.Where(a => a.Padre.Equals(PadreRol)).ToList();
                foreach (var i in nuevalista)
                {
                    TreeNode newNode = new TreeNode(i.NombreRol, i.IdRol.ToString());
                    if (parentNode == null)
                    {
                        TreeView1.Nodes.Add(newNode);
                    }
                    else
                    {
                        parentNode.ChildNodes.Add(newNode);
                    }
                    CreateeTreeView(source, i.IdRol, newNode);
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        protected void cboperfil_ServerChange(object sender, EventArgs e)
        {
            try
            {

                #region This method is only to tecniservices

                //if (cboperfil.Value == Constantes.ValorSiete + string.Empty) //FOR TECNI SERVICES (REMOVE OTHER ENTITY) //
                //{
                //    cbosede.SelectedIndex = Constantes.ValorCero;
                //    cbosede.Disabled = true;
                //}
                //else if (cboperfil.Value == Constantes.ValorDiez + string.Empty)
                //{
                //    cbosede.SelectedIndex = Constantes.ValorCero;
                //    cbosede.Disabled = true;
                //}
                //else
                //{
                //    cbosede.SelectedIndex = Constantes.ValorCero;
                //    cbosede.Disabled = false;
                //}
                #endregion

                string script = @"<script type='text/javascript'>";
                script += @"</script>";
                Response.Write(script);
            }
            catch (Exception ex)
            {

            }
        }
    }
}