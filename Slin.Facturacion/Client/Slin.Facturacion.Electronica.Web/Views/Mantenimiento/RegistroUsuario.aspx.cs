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
using System.Text;
//using System.Security.Authentication;

using System.Security.Cryptography;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Mantenimiento
{
    public partial class RegistroUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
                MensajeRegistrado();
            }
        }

        #region ENTITY

        ServiceWebFactController objController = new ServiceWebFactController();

        private WCFSeguridad.ListaPerfil olistaPerfil;
        public WCFSeguridad.ListaPerfil oListaPerfil
        {
            get { return olistaPerfil; }
            set { olistaPerfil = value; }
        }

        private ListaEmpresa olistaEmpresa;
        public ListaEmpresa oListaEmpresa
        {
            get { return olistaEmpresa; }
            set { olistaEmpresa = value; }
        }

        private ListaEstado olistaEstado;
        public ListaEstado oListaEstado
        {
            get { return olistaEstado; }
            set { olistaEstado = value; }
        }

        private WCFSeguridad.Usuario ousuario;
        public WCFSeguridad.Usuario oUsuario
        {
            get { return ousuario; }
            set { ousuario = value; }
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

        WCFSeguridad.Usuario usuario = new WCFSeguridad.Usuario();
        WCFSeguridad.ListaRol oListaRol = new WCFSeguridad.ListaRol();
        WCFSeguridad.Usuario userRolRegistr = new WCFSeguridad.Usuario();
        WCFSeguridad.ListaUsuario oListaRolUserRegistr = new WCFSeguridad.ListaUsuario();

        #endregion


        #region METHOD

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
            catch (Exception ex)
            {

            }

        }

        private void ObtenerUsuarioLogeado()
        {
            try
            {
                oUsuarioLogeado = objController.GetUserLogueado_Fact();
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
            }
            catch (Exception ex)
            {

            }

        }

        WCFSeguridad.ListaRol oListaRolesUserLog = new WCFSeguridad.ListaRol();
        void CapturarUserLogRoles()
        {
            try
            {
                oListaRolesUserLog = objController.GetList_RolUserLog();

                btnGuardar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnGuardar.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

            }
            catch (Exception ex)
            {

            }

        }

        void CargarListas()
        {
            try
            {
                ObtenerUsuarioLogeado();
                oListaEmpresa = ServiceMantenimientoController.Instance.GetListaEmpresa();

                oListaEmpresa.Insert(0, new Empresa() { IdEmpresa = Constantes.ValorCero, RazonSocial = Constantes.ValorSeleccione });
                cboempresa.DataSource = oListaEmpresa;
                cboempresa.DataValueField = "IdEmpresa";
                cboempresa.DataTextField = "RazonSocial";
                cboempresa.DataBind();

                cboempresa.Value = oUsuarioLogeado.Empresa.IdEmpresa.ToString();

                oListaPerfil = ServiceSeguridadController.Instance.GetListaPerfiles(oUsuarioLogeado.Empresa.RUC);

                oListaPerfil.Insert(0, new WCFSeguridad.Perfil() { IdPerfil = Constantes.ValorCero, NombrePerfil = Constantes.ValorSeleccione });
                //oListaPerfil.Insert(0, new WCFSeguridad.Perfil() { IdPerfil = Constantes.ValorCero + 1, NombrePerfil = Constantes.ValorContabilidad });
                cboperfil.DataSource = oListaPerfil;
                cboperfil.DataValueField = "IdPerfil";
                cboperfil.DataTextField = "NombrePerfil";
                cboperfil.DataBind();


                oListaEstado = ServiceMantenimientoController.Instance.GetListaEstado();
                //oListaEstado.Insert(0, new Estado() { IdEstado = Constantes.ValorCero, Descripcion = Constantes.ValorSeleccione });
                cboestado.DataSource = oListaEstado;
                cboestado.DataValueField = "IdEstado";
                cboestado.DataTextField = "Descripcion";
                cboestado.DataBind();

                txtfecharegistro.Value = DateTime.Now.ToShortDateString();


                oListaSede = new WCFSeguridad.ListaSede();

                oListaSede = ServiceSeguridadController.Instance.GetListSede(oUsuarioLogeado.Empresa.RUC);

                oListaSede.Insert(0, new WCFSeguridad.Sede() { IdSede = Constantes.ValorCero, Name = Constantes.ValorSeleccione });

                cbosede.DataSource = oListaSede;
                cbosede.DataValueField = "Name";
                cbosede.DataTextField = "Name";
                cbosede.DataBind();
            }
            catch (Exception ex) { }
        }

        void LlenarObjeto()
        {
            try
            {
                oUsuario = new WCFSeguridad.Usuario();
                oUsuario.Empresa = new WCFSeguridad.Empresa();
                oUsuario.Empleado = new WCFSeguridad.Empleado();
                oUsuario.Estado = new WCFSeguridad.Estado();
                oUsuario.Perfil = new WCFSeguridad.Perfil();
                oUsuario.Sede = new WCFSeguridad.Sede();

                oUsuario.Empleado.DNI = txtdni.Value;
                ousuario.Empleado.Nombres = txtnombres.Value.Length == 0 ? string.Empty : txtnombres.Value;
                ousuario.Empleado.ApePaterno = txtapepaterno.Value.Length == 0 ? string.Empty : txtapepaterno.Value;
                ousuario.Empleado.ApeMaterno = txtapematerno.Value.Length == 0 ? string.Empty : txtapematerno.Value;

                ousuario.Empleado.Direccion = txtdireccion.Value.Length == 0 ? string.Empty : txtdireccion.Value;
                ousuario.Empleado.Telefono = txttelefono.Value.Length == 0 ? string.Empty : txttelefono.Value;
                ousuario.Empleado.Email = txtemail.Value.Length == 0 ? string.Empty : txtemail.Value;

                oUsuario.Username = txtusername.Value.Length == 0 ? string.Empty : txtusername.Value;

                //oUsuario.Password = new Helper.Encrypt().EncryptKey(txtpassword.Value);//

                if (txtpassword.Value.Length == Constantes.ValorCero)
                {
                    oUsuario.Password = new Helper.Encrypt().HashPassword("123456");//PWC
                }
                else
                {
                    oUsuario.Password = new Helper.Encrypt().HashPassword(txtpassword.Value);//PWC
                }
                

                oUsuario.FechaRegistro = Convert.ToDateTime(txtfecharegistro.Value);
                ousuario.FechaExpiracion = txtfechaexpiracion.Value.Length == 0 ? Convert.ToDateTime("01/01/1990") : Convert.ToDateTime(txtfechaexpiracion.Value);

                oUsuario.Estado.IdEstado = Convert.ToInt32(cboestado.Value);
                oUsuario.Empresa.IdEmpresa = Convert.ToInt32(cboempresa.Value);
                oUsuario.Perfil.IdPerfil = Convert.ToInt32(cboperfil.Value);
                //oUsuario.Sede.Name = cbosede.Value == Constantes.ValorSeleccione ? Constantes.ValorAdmin : cbosede.Value;

                if (cbosede.Value == Constantes.ValorSeleccione)
                {
                    oUsuario.Sede.Name = string.Empty;
                }
                else { oUsuario.Sede.Name = cbosede.Value; }
            }
            catch (Exception ex) { }
        }

        void ValidarRegistro()
        {
            try
            {
                if (cboperfil.SelectedIndex == Constantes.ValorCero)
                {
                    Response.Write("<script language=javascript>alert('Seleccione Perfil para el Usuario');</script>"); }
                else
                { ValidarUsername(); }
            }
            catch (Exception ex) { } 
        }

        void ValidarUsername()
        {
            try
            {
                var listresult = new WCFSeguridad.ListaUsuario();

                listresult = ServiceSeguridadController.Instance.ValidarUsername(txtusername.Value);

                if (listresult.Count > 0)
                {
                    Response.Write("<script language=javascript>alert('Usuario ya Registrado');</script>");
                    txtusername.Focus();
                }
                else
                {
                    var listaresult = new WCFSeguridad.ListaUsuario();
                    listaresult = ServiceSeguridadController.Instance.ValidarDniUsuario(txtdni.Value);

                    if (listaresult.Count > 0)
                    {
                        Response.Write("<script language=javascript>alert('Código/Dni ya Registrado');</script>");
                        txtdni.Focus();
                    }
                    else
                    {
                        GuardarUsuario();
                        RegistrarRoles();
                        LimpiarCampos();
                    }
                }
            }
            catch (Exception ex) { }
        }

        void RegistrarRoles()
        {
            //VerificarCheckbox();
            LlenarListaRolRegistr();
            
            var listaResult = new List<string>();
            
            foreach (var obj in oListaRolUserRegistr)
            {
                var result = ServiceSeguridadController.Instance.RegistrarUsuarioRol(obj);
                if (result != Constantes.msjRegistrado)
                {
                    listaResult.Add(result);
                }
            }

            string cadena = string.Empty;
            if (listaResult.Count > 0)
            {
                string coma = string.Empty;
                foreach (var res in listaResult)
                {
                    cadena += coma + res;
                    coma = ", ";
                }
            }

            if (listaResult.Count > 0)
            {
                Response.Write("<script language=javascript>alert('" + cadena + "');</script>");
            }
            else
            {
                Session["MensajeUsuarioRegistrado"] = Constantes.msjRegistrado;
                Response.Redirect("RegistroUsuario");
            }            
        }

        private void LlenarListaRolRegistr()
        {
            oListaRolUserRegistr = new WCFSeguridad.ListaUsuario();

            for (int i = 0; i <= TreeView1.Nodes.Count - 1; i++)
            {
                userRolRegistr = new WCFSeguridad.Usuario();
                userRolRegistr.Rol = new WCFSeguridad.Rol();
                userRolRegistr.Empleado = new WCFSeguridad.Empleado();

                //userRolRegistr.Rol.IdRol = oListaRol[i].IdRol;
                //userRolRegistr.Empleado.DNI = txtdni.Value;

                userRolRegistr.Rol.IdRol = int.Parse(TreeView1.Nodes[i].Value);
                userRolRegistr.Empleado.DNI = txtdni.Value;

                if (TreeView1.Nodes[i].Checked == true)
                {
                    oListaRolUserRegistr.Add(userRolRegistr);
                }
            }
        }

        void GuardarUsuario()
        {
            try
            {
                LlenarObjeto();
                string msj = string.Empty;
                msj = ServiceSeguridadController.Instance.InsertarUsuario(oUsuario);
            }
            catch (Exception ex)
            {

            }

        }


        void LimpiarCampos()
        {
            txtdni.Value = string.Empty;
            txtfecharegistro.Value = DateTime.Now.ToShortDateString();
            txtpassword.Value = string.Empty;
            txtusername.Value = string.Empty;
            cboempresa.SelectedIndex = Constantes.ValorCero;
            cboestado.SelectedIndex = Constantes.ValorCero;
            cboperfil.SelectedIndex = Constantes.ValorCero;

            txtnombres.Value = string.Empty;
            txtapepaterno.Value = string.Empty;
            txtapematerno.Value = string.Empty;
            txttelefono.Value = string.Empty;
            txtfechaexpiracion.Value = string.Empty;
            txtdireccion.Value = string.Empty;
            txtemail.Value = string.Empty;
        }

        #endregion

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            ValidarRegistro();
        }

        protected void btndescargar_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "FormatoUsuario" + ".csv");
                Response.WriteFile("../Util/" + "FormatoUsuario" + ".csv");
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {

            }
        }

        #region TREEVIEW

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

                //if (cboperfil.Value == Constantes.ValorSiete + string.Empty) //FOR TECNI SERVICES (REMOVE OTHER ENTITY//
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