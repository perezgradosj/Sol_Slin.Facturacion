using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Proxies.ServicioFacturacion;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using WCFMantenimiento = Slin.Facturacion.Proxies.ServicioMantenimiento;
using WCFConfiguracion = Slin.Facturacion.Proxies.ServicioConfiguracion;
using Slin.Facturacion.Common;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using System.Data;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Configuracion
{
    public partial class ConfiguracionEnvio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
                Mensaje();
            }
        }

        #region ENTITY

        ServiceWebFactController objController = new ServiceWebFactController();

        private ListaEstado olistaestado;
        public ListaEstado oListaEstado
        {
            get { return olistaestado; }
            set { olistaestado = value; }
        }

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        private ListaTipoDocumento olistatipoDocumento;
        public ListaTipoDocumento oListaTipoDocumento
        {
            get { return olistatipoDocumento; }
            set { olistatipoDocumento = value; }
        }

        private ListaEstadoEnvio olistaestadoEnvio;
        public ListaEstadoEnvio oListaEstadoEnvio
        {
            get { return olistaestadoEnvio; }
            set { olistaestadoEnvio = value; }
        }

        private ListaEstadoEnvio olistinsertSend;
        public ListaEstadoEnvio oListInsertSend
        {
            get { return olistinsertSend; }
            set { olistinsertSend = value; }
        }

        private ListaEstadoEnvio olistdeleteSend;
        public ListaEstadoEnvio oListDeleteSend
        {
            get { return olistdeleteSend; }
            set { olistdeleteSend = value; }
        }

        private EstadoEnvio objsend;
        public EstadoEnvio ObjSend
        {
            get { return objsend; }
            set { objsend = value; }
        }

        #endregion

        #region METHOD

        void Mensaje()
        {
            try
            {
                string respuesta = (string)Session["MensajeUpdChangeTipoDocSend"];
                //if (respuesta == Constantes.msjActualizado)
                if (respuesta.Length > 0)
                {
                    Session.Remove("MensajeUpdChangeTipoDocSend");
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                
            }
            catch (Exception ex)
            {

            }
        }


        void ObtenerUsuarioLogeado()
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

                btnGuardar.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnGuardar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

            }
            catch (Exception ex) { }
        }

        void CargarLista()
        {
            try
            {
                oListaEstado = ServiceFacturacionController.Instance.ListarEstadoDocumento();
                oListaEstado.RemoveAt(0);

                cboestado.DataSource = oListaEstado;
                cboestado.DataValueField = "IdEstado";
                cboestado.DataTextField = "Descripcion";
                cboestado.DataBind();
            }
            catch (Exception ex)
            {

            }
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
                CargarLista();
                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";

                oListaTipoDocumento = new ListaTipoDocumento();
                oListaTipoDocumento = ServiceFacturacionController.Instance.ListarTipoDocumento();

                oListaTipoDocumento.RemoveAt(0);
                CreateeTreeView(oListaTipoDocumento, 0, null);

                ListarTipoDocumentoSend();

                oListaEstado = new ListaEstado();
                oListaEstado = ServiceFacturacionController.Instance.ListarEstadoDocumento();
                oListaEstado.RemoveAt(0);
                oListaEstado.Insert(0, new Estado() { IdEstado = Constantes.ValorCero, Descripcion = Constantes.ValorSeleccione });

                Session["ListStatusDocumentSend"] = oListaEstado;
                CargaListTypeDocumentSend();
                Session["oListTypeDocumentSend"] = oListTypeDocument;
            }
            catch (Exception ex)
            {

            }
        }

        private void CreateeTreeView(ListaTipoDocumento source, int PadreTipoDoc, TreeNode parentNode)
        {
            try
            {
                var nuevalista = source.Where(a => a.Padre.Equals(PadreTipoDoc)).ToList();
                foreach (var i in nuevalista)
                {
                    //TreeNode newNode = new TreeNode(i.Descripcion, i.IdTipoDocumento.ToString());
                    TreeNode newNode = new TreeNode(i.Descripcion, i.CodigoDocumento);
                    if (parentNode == null)
                    {
                        TreeView1.Nodes.Add(newNode);
                    }
                    else
                    {
                        parentNode.ChildNodes.Add(newNode);
                    }
                    CreateeTreeView(source, i.IdTipoDocumento, newNode);
                }
            }
            catch (Exception ex)
            {

            }
        }

        void ListarTipoDocumentoSend()
        {
            ObtenerUsuarioLogeado();
            oListaEstadoEnvio = new ListaEstadoEnvio();

            oListaEstadoEnvio = ServiceFacturacionController.Instance.GetListEstadoEnvio(int.Parse(cboestado.Value), oUsuarioLogeado.Empresa.RUC);

            HabilitarCheckContent(oListaEstadoEnvio);
        }

        void HabilitarCheckContent(ListaEstadoEnvio objlista)
        {
            try
            {
                for (int j = 0; j <= TreeView1.Nodes.Count - 1; j++)
                {
                    TreeView1.Nodes[j].Checked = false;
                }

                for (int i = 0; i <= TreeView1.Nodes.Count - 1; i++)
                {
                    for (int mp = 0; mp <= objlista.Count - 1; mp++)
                    {
                        //if (TreeView1.Nodes[i].Value == objlista[mp].TipoDocumento.IdTipoDocumento.ToString())
                        if (TreeView1.Nodes[i].Value == objlista[mp].TipoDocumento.CodigoDocumento)
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

        protected void cboestado_ServerChange(object sender, EventArgs e)
        {
            try
            {
                ListarTipoDocumentoSend();
                string script = @"<script type='text/javascript'>";
                script += @"</script>";
                Response.Write(script);
            }
            catch (Exception ex)
            {

            }
        }


        #endregion

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {

        }

        private void LlenarLista()
        {
            try
            {
                ObtenerUsuarioLogeado();
                oListInsertSend = new ListaEstadoEnvio();
                oListDeleteSend = new ListaEstadoEnvio();

                for (int i = 0; i <= TreeView1.Nodes.Count - 1; i++)
                {
                    ObjSend = new EstadoEnvio();
                    ObjSend.Empresa = new Empresa();
                    ObjSend.TipoDocumento = new TipoDocumento();

                    ObjSend.TipoDocumento.CodigoDocumento = TreeView1.Nodes[i].Value;
                    ObjSend.IdEstadoEnvio = int.Parse(cboestado.Value);
                    ObjSend.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;

                    if (TreeView1.Nodes[i].Checked == true)
                    {
                        oListInsertSend.Add(ObjSend);
                    }
                    else
                    {
                        oListDeleteSend.Add(ObjSend);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Error: " + ex.Message + "');</script>");
            }
        }


        private void GuardarCambiosSendDocument()
        {
            try
            {
                LlenarLista();

                if (oListInsertSend.Count > 0)
                {
                    var resultInsert = ServiceFacturacionController.Instance.InsertTipoDocumentSend(oListInsertSend);

                    if (oListInsertSend.Count == TreeView1.Nodes.Count)
                    {
                        Session["MensajeUpdChangeTipoDocSend"] = Constantes.msjActualizado;
                    }
                }

                if (oListDeleteSend.Count > 0)
                {
                    var resulDelete = ServiceFacturacionController.Instance.DeleteTipoDocumentSend(oListDeleteSend);
                    Session["MensajeUpdChangeTipoDocSend"] = Constantes.msjActualizado;
                }
                Response.Redirect("ConfiguracionEnvio");
            }
            catch (Exception ex)
            {
                //Session["MensajeUpdChangeTipoDocSend"] = ex.Message;
                Response.Write("<script language=javascript>alert('Error: " + ex.Message + "');</script>");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarCambiosSendDocument();
        }







        private WCFConfiguracion.ListaTipoDocumento olisttypedocument;
        public WCFConfiguracion.ListaTipoDocumento oListTypeDocument
        {
            get { return olisttypedocument; }
            set { olisttypedocument = value; }
        }

        private void CargaListTypeDocumentSend()
        {
            ObtenerUsuarioLogeado();
            oListTypeDocument = new WCFConfiguracion.ListaTipoDocumento();
            oListTypeDocument = ServiceConfiguracionController.Instance.ListTypeDocument_TypeSend(oUsuarioLogeado.Empresa.RUC);

            GVListaTypeDocument.DataSource = oListTypeDocument;
            GVListaTypeDocument.DataBind();
        }

        protected void GVListaTypeDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            #region
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList combo = (DropDownList)e.Row.FindControl("cboEstadoRow");
                    combo.ClearSelection();
                    if (!object.ReferenceEquals(combo, DBNull.Value))
                    {
                        this.GeneraDropDownList(combo);
                    }
                }
            }
            catch (Exception ex) { }
            #endregion
        }

        int inicia = Constantes.ValorCero;
        private void GeneraDropDownList(DropDownList cboCombo)
        {
            oListaEstado = new ListaEstado();
            oListaEstado = (ListaEstado)Session["ListStatusDocumentSend"];

            cboCombo.DataSource = oListaEstado;
            cboCombo.DataTextField = "Descripcion";
            cboCombo.DataValueField = "IdEstado";
            cboCombo.DataBind();

            cboCombo.SelectedValue = oListTypeDocument[inicia].Estado.IdEstado + string.Empty;
            inicia++;
        }

        protected void GVListaTypeDocument_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            ObtenerUsuarioLogeado();
            try
            {
                //GridViewRow gvrow = GVListaTypeDocument.Rows[e.RowIndex];
                //TypeDocSelected = GVListaTypeDocument.Rows[gvrow.RowIndex].Cells[2].Text;

                WCFConfiguracion.TipoDocumento otip = new WCFConfiguracion.TipoDocumento();
                otip.Estado = new WCFConfiguracion.Estado();
                otip.Empresa = new WCFConfiguracion.Empresa();

                //Update the values.
                GridViewRow row = GVListaTypeDocument.Rows[e.RowIndex];

                //otip.IdTipoDocumento = int.Parse(((TextBox)(row.Cells[1].Controls[0])).Text);
                //otip.CodigoDocumento = ((TextBox)(row.Cells[2].Controls[0])).Text;
                //otip.Descripcion = ((TextBox)(row.Cells[3].Controls[0])).Text;

                //otip.IdTipoDocumento = int.Parse(GVListaTypeDocument.Rows[e.RowIndex].Cells[1].Text);


                otip.CodigoDocumento = GVListaTypeDocument.Rows[e.RowIndex].Cells[2].Text;//revisar esto porque estaba asi
                //otip.CodigoDocumento = GVListaTypeDocument.Rows[e.RowIndex].Cells[2].Text;

                //otip.Descripcion = GVListaTypeDocument.Rows[e.RowIndex].Cells[3].Text;
                otip.Estado.IdEstado = int.Parse(((DropDownList)row.FindControl("cboEstadoRow")).SelectedItem.Value);
                otip.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;

                var result = ServiceConfiguracionController.Instance.InsertTypeDocument_ForSend(otip);
                Session["MensajeUpdChangeTipoDocSend"] = result;
                //if (result == Constantes.msjActualizado)
                //{
                //    Response.Write("<script language=javascript>alert('" + result + "');</script>");
                //}
                //CargaListTypeDocumentSend();

                Response.Redirect("ConfiguracionEnvio.aspx");

                //Reset the edit index.
                GVListaTypeDocument.EditIndex = -1;
                //BindData(-1);
            }
            catch (Exception ex)
            {

            }
        }

        protected void GVListaTypeDocument_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GVListaTypeDocument.EditIndex = e.NewEditIndex;
            int num = e.NewEditIndex;
            BindData(num);
        }

        protected void GVListaTypeDocument_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GVListaTypeDocument.EditIndex = -1;
            BindData(-1);
        }

        private void BindData(int edit)
        {
            oListTypeDocument = new WCFConfiguracion.ListaTipoDocumento();
            oListTypeDocument = (WCFConfiguracion.ListaTipoDocumento)Session["oListTypeDocumentSend"];

            GVListaTypeDocument.DataSource = oListTypeDocument;
            GVListaTypeDocument.DataBind();

            if (edit != Constantes.ValorMenosUno)
            {
                GridViewRow row = GVListaTypeDocument.Rows[edit];
                ((DropDownList)row.FindControl("cboEstadoRow")).Enabled = true;
            }
        }


    }
}