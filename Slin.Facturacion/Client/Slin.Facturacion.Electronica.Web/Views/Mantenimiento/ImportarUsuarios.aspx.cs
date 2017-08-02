using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.OleDb;
//using Microsoft.ApplicationBlocks.Data;

using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Proxies.ServicioMantenimiento;
using Slin.Facturacion.Common;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;

using System.Text.RegularExpressions;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Configuration;
using Slin.Facturacion.Electronica.Web.Views.Util;
using Microsoft.VisualBasic.FileIO;
using Slin.Facturacion.Common.Method;

namespace Slin.Facturacion.Electronica.Web.Views.Mantenimiento
{
    public partial class ImportarUsuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
                Message();
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

        private WCFSeguridad.ListaUsuario olistausuario;
        public WCFSeguridad.ListaUsuario oListaUsuario
        {
            get { return olistausuario; }
            set { olistausuario = value; }
        }


        private WCFSeguridad.Rol orol;
        public WCFSeguridad.Rol oRol
        {
            get { return orol; }
            set { orol = value; }
        }

        private WCFSeguridad.ListaRol olistarol;
        public WCFSeguridad.ListaRol oListaRol
        {
            get { return olistarol; }
            set { olistarol = value; }
        }

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }
        #endregion

        #region METHOD

        private void Message()
        {
            var listOk = (WCFSeguridad.ListaUsuario)Session["ListOk"];
            var listNotOk = (WCFSeguridad.ListaUsuario)Session["ListNotOk"];

            if (listOk != null)
            {
                
                if (listNotOk.Count == Constantes.ValorCero)
                {
                    string cadena = string.Empty;
                    string del = string.Empty;
                    foreach (var u in listOk)
                    {
                        cadena += del + u.Username;
                        del = ",";
                    }
                    Session.Remove("ListOk");
                    Session.Remove("ListNotOk");
                    //Response.Write("<script language=javascript>alert('" + cadena + " Usuario registrado correctamente');</script>");
                    Response.Write("<script language=javascript>alert('" + listOk.Count + " Registros añadidos correctamente.');</script>");
                }
                else
                {
                    string cadena = string.Empty;
                    string del = string.Empty;
                    foreach (var u in listNotOk)
                    {
                        cadena = del + u.Username;
                        del = ",";
                    }
                    Session.Remove("ListOk");
                    Session.Remove("ListNotOk");
                    Response.Write("<script language=javascript>alert('" + cadena + " no registrado');</script>");
                }
            }
        }

        private void ObtenerUsuarioLogueado()
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
                ObtenerUsuarioLogueado();

                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";
                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();
            }
            catch (Exception ex) { }
        }

        //void CargarDatosExcel(GridView dgv, string nombreHoja)
        //{
        //    string ruta = "";
        //    try
        //    {
        //        OpenFileDialog openfile1 = new OpenFileDialog();
        //        openfile1.Filter = "Excel Files |*.xlsx|Excel Files |*.xls";
        //        //openfile1.Filter = "Excel Files |*.*";
        //        openfile1.Title = "Seleccione el archivo de Excel";
        //        if (openfile1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            if (openfile1.FileName.Equals("") == false)
        //            {
        //                ruta = openfile1.FileName;
        //            }
        //        }
        //        conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;data source=" + ruta + ";Extended Properties='Excel 12.0 Xml;HDR=Yes'");
        //        MyDataAdapter = new OleDbDataAdapter("Select * from [" + nombreHoja + "$]", conn);
        //        dt = new DataTable();
        //        MyDataAdapter.Fill(dt);
        //        dgv.DataSource = dt;
        //        panel1.Visible = true;
        //    }
        //    catch (Exception ex) { } 
        //}

        void CargarArchivoExcel(GridView dgv, string nombreHoja)
        {
            try
            {
                //string CadenaConexion = OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;data source=" + fileName + ";Extended Properties='Excel 12.0 Xml;HDR=Yes'");
                OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;data source=D:\FormatoUsuario.xlsx" + @";Extended Properties='Excel 12.0 Xml;HDR=Yes'");
                string command = "Select * from [" + nombreHoja + "$]";
                OleDbDataAdapter da = new OleDbDataAdapter(command, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgv.DataSource = dt;
                dgv.DataBind();
            }
            catch (Exception ex)
            {

            }
            
        }


        //carga EXCEL
        //OleDbConnection conn;
        //OleDbDataAdapter MyDataAdapter;
        DataTable dt;
        public void ValidarRegistros(GridView dgv)
        {
            string nombreArchivo = string.Empty;

            try
            {
                nombreArchivo = FileUpload1.PostedFile.FileName.ToString();

                if (nombreArchivo.Length == Constantes.ValorCero)
                {
                    Response.Write("<script language=javascript>alert('Seleccione el archivo de .CSV');</script>");
                }
                else
                {
                    Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));

                    var listuserfail = new WCFSeguridad.ListaUsuario();
                    var listuserinsert = new WCFSeguridad.ListaUsuario();

                    if (FileUpload1.HasFile)
                    {
                        string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                        string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                        //string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

                        string FilePath = Server.MapPath("../../DocumentoXML/" + FileName);

                        FileUpload1.SaveAs(FilePath);

                        List<string> list = new List<string>(); list = new List<string>();
                        //string[] values = File.ReadAllText(Server.MapPath("../../DocumentoXML/file_csv.csv"), System.Text.Encoding.GetEncoding(1252)).Split(',');
                        string[] values = File.ReadAllText(Server.MapPath("../../DocumentoXML/" + FileName), System.Text.Encoding.GetEncoding(1252)).Split(',');
                        string cadena = values[0];
                        cadena = cadena.Replace("\r\n", ";");
                        values = cadena.Split(';');
                        for (int i = 0; i <= values.Length - 2; i++)
                        {
                            list.Add(values[i] + "-" + values[i + 1] + "-" + values[i + 2] + "-" + values[i + 3] + "-" + values[i + 4] + "-" + values[i + 5] + "-" + values[i + 6] + "-" + values[i + 7] + "-" + values[i + 8] + "-" + values[i + 9] + "-" + values[i + 10] + "-" + values[i + 11] + "-" + values[i + 12] + "-" + values[i + 13] + "-" + values[i + 14] + "-" + values[i + 15] + "-" + values[i + 16]);
                            i += 16;
                        }

                        list.RemoveAt(0);

                        
                        var listresult = new WCFSeguridad.ListaUsuario();

                        foreach (string item in list)
                        {
                            string[] arrayUser = item.Split('-');
                            var user = new WCFSeguridad.Usuario();
                            user.Perfil = new WCFSeguridad.Perfil();
                            user.Estado = new WCFSeguridad.Estado();
                            user.Empresa = new WCFSeguridad.Empresa();
                            user.Empleado = new WCFSeguridad.Empleado();
                            
                            user.Estado.IdEstado = arrayUser[0].Length > Constantes.ValorCero ? int.Parse(arrayUser[0]) : 2;
                            user.Perfil.IdPerfil = int.Parse(arrayUser[1]);

                            user.Empleado.Nombres = arrayUser[2].Length > Constantes.ValorCero ? arrayUser[2] : string.Empty;
                            user.Empleado.ApePaterno = arrayUser[3].Length > Constantes.ValorCero ? arrayUser[3] : string.Empty;
                            user.Empleado.ApeMaterno = arrayUser[4].Length > Constantes.ValorCero ? arrayUser[4] : string.Empty;
                            user.Empleado.DNI = arrayUser[5];

                            user.Empleado.Direccion = arrayUser[6].Length > Constantes.ValorCero ? arrayUser[6] : string.Empty;
                            user.Empleado.Telefono = arrayUser[7].Length > Constantes.ValorCero ? arrayUser[7] : string.Empty;
                            user.Empleado.Email = arrayUser[8].Length > Constantes.ValorCero ? arrayUser[8] : string.Empty;

                            user.Username = arrayUser[9];
                            user.Password = arrayUser[10].Length > Constantes.ValorCero ? arrayUser[10] : "123456";

                            try
                            {
                                user.FechaExpiracion = Convert.ToDateTime(arrayUser[11]);
                            }
                            catch (Exception ex)
                            {
                                user.FechaExpiracion = Convert.ToDateTime("01/01/1990");
                            }

                            
                            

                            

                            listresult = new WCFSeguridad.ListaUsuario();
                            listresult = ServiceSeguridadController.Instance.ValidarDniUsuario(user.Empleado.DNI);

                            if (listresult.Count > 0)
                            {
                                listuserfail.Add(user);
                            }
                            else
                            {
                                listuserinsert.Add(user);
                            }
                        }
                        if (listuserfail.Count == Constantes.ValorCero)
                        {
                            try
                            {
                                //using (TextFieldParser csvReader = new TextFieldParser(Server.MapPath("../../DocumentoXML/" + FileName), System.Text.Encoding.GetEncoding(1252)))
                                using (TextFieldParser csvReader = new TextFieldParser(FilePath, System.Text.Encoding.GetEncoding(1252)))
                                {
                                    
                                    csvReader.SetDelimiters(new string[] { "," });
                                    csvReader.HasFieldsEnclosedInQuotes = true;
                                    string[] colFields = csvReader.ReadFields();

                                    colFields[0] = colFields[0].Replace("\r\n",";");

                                    string[] array_col = colFields[0].Split(';');

                                    //foreach (string column in colFields)
                                    //{
                                    //    DataColumn serialno = new DataColumn(column);
                                    //    serialno.AllowDBNull = true;
                                    //    dt.Columns.Add(serialno);
                                    //}

                                    dt = new DataTable();

                                    foreach (var col in array_col)
                                    {
                                        DataColumn serialno = new DataColumn(col);
                                        serialno.AllowDBNull = true;
                                        dt.Columns.Add(serialno);
                                    }

                                    while (!csvReader.EndOfData)
                                    {
                                        string[] fieldData = csvReader.ReadFields(); 

                                        for (int i = 0; i < fieldData.Length; i++)
                                        {
                                            if (fieldData[i] == null)
                                                fieldData[i] = string.Empty;
                                            //dr[i] = fieldData[i];

                                            string[] array_val = fieldData[i].Split(';');

                                            dt.Rows.Add(array_val[0], array_val[1], array_val[2], array_val[3], array_val[4], array_val[5], array_val[6], array_val[7], array_val[8], array_val[9], array_val[10], array_val[11], array_val[12], array_val[13], array_val[14], array_val[15], array_val[16]);
                                        }
                                        //dt.Rows.Add(dr);
                                    }
                                }

                                dgv.DataSource = dt;
                                dgv.DataBind();

                                Session["DataTableRol"] = dt;
                            }
                            catch (Exception ex) { }
                        }
                        //dgv.Caption = Path.GetFileName(FilePath);
                    }

                    var listaErrorDniUsuario = new WCFSeguridad.ListaUsuario();//lista con dni existentes
                    listaErrorDniUsuario = listuserfail;


                    lblmensaje2.InnerText = "Error en la columna DNI, Nro de DNI ya Registrado";

                    GVListaUsuariosExcel.DataSource = listaErrorDniUsuario;
                    GVListaUsuariosExcel.DataBind();

                    ObtenerUsuarioLogueado();

                    if (listaErrorDniUsuario.Count == Constantes.ValorCero)
                    {
                        panel1.Visible = false;
                        panel2.Visible = true;
                        btnGuardar.Enabled = true;

                        ObtenerUsuarioLogueado();

                        oListaUsuario = new WCFSeguridad.ListaUsuario();//para llenar usuarios
                        //oListaRol = new WCFSeguridad.ListaRol();//lista para llenar roles

                        foreach (var obj in listuserinsert)
                        {
                            oUsuario = new WCFSeguridad.Usuario();

                            oUsuario = new WCFSeguridad.Usuario();
                            oUsuario.Perfil = new WCFSeguridad.Perfil();
                            oUsuario.Estado = new WCFSeguridad.Estado();
                            oUsuario.Empresa = new WCFSeguridad.Empresa();
                            oUsuario.Empleado = new WCFSeguridad.Empleado();

                            oUsuario.Estado.IdEstado = obj.Estado.IdEstado;
                            oUsuario.Perfil.IdPerfil = obj.Perfil.IdPerfil;
                            oUsuario.Empleado.Nombres = obj.Empleado.Nombres;
                            oUsuario.Empleado.ApePaterno = obj.Empleado.ApePaterno;
                            oUsuario.Empleado.ApeMaterno = obj.Empleado.ApeMaterno;
                            oUsuario.Empleado.DNI = obj.Empleado.DNI;
                            oUsuario.Empleado.Direccion = obj.Empleado.Direccion;
                            oUsuario.Empleado.Telefono = obj.Empleado.Telefono;
                            oUsuario.Empleado.Email = obj.Empleado.Email;

                            oUsuario.Username = obj.Username;
                            string pass = new Helper.Encrypt().HashPassword(obj.Password);
                            oUsuario.Password = pass;

                            oUsuario.Empresa.IdEmpresa = oUsuarioLogeado.Empresa.IdEmpresa;
                            oUsuario.FechaExpiracion = Convert.ToDateTime(obj.FechaExpiracion);
                            oUsuario.FechaRegistro = DateTime.Now;
                            oListaUsuario.Add(oUsuario);
                        }
                        Session["ListaUsuarioExcel"] = oListaUsuario;
                    }
                    else
                    {
                        panel1.Visible = true;
                        panel2.Visible = false;
                        btnGuardar.Enabled = false;
                    }
                    var file = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + nombreArchivo));
                    if (System.IO.File.Exists(file))
                        System.IO.File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                var file = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + nombreArchivo));
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
                Response.Write("<script language=javascript>alert('Error al Validar Datos');</script>");
            }
        }



        void RegistrarListaUsuariosExcel()
        {
            try
            {
                oListaUsuario = (WCFSeguridad.ListaUsuario)Session["ListaUsuarioExcel"];
                var lista = ServiceSeguridadController.Instance.InsertarUsuario_ForExcel(oListaUsuario);

                RegistrarRolesUsuarioExcel();

                Session["ListOk"] = oListaUsuario;
                Session["ListNotOk"] = lista;

                Response.Redirect("ImportarUsuarios.aspx");
            }
            catch (Exception ex) { }
        }

        void RegistrarRolesUsuarioExcel()
        {
            try
            {
                oListaRol = new WCFSeguridad.ListaRol();
                oListaRol = ServiceSeguridadController.Instance.GetListadoRol();

                int IdGuardar, IdModificar, IdBuscar, IdExportar, IdEnviar;
                var dt2 = new DataTable();
                var grid = new GridView();

                dt2 = (DataTable)Session["DataTableRol"];

                //grid.AutoGenerateColumns = false;
                grid.DataSource = dt2;
                grid.DataBind();

                for (int i = 0; i <= grid.Rows.Count - 1; i++)
                {
                    IdGuardar = Convert.ToInt32(grid.Rows[i].Cells[12].Text);
                    IdModificar = Convert.ToInt32(grid.Rows[i].Cells[13].Text);
                    IdBuscar = Convert.ToInt32(grid.Rows[i].Cells[14].Text);
                    IdExportar = Convert.ToInt32(grid.Rows[i].Cells[15].Text);
                    IdEnviar = Convert.ToInt32(grid.Rows[i].Cells[16].Text);

                    if (IdGuardar > 0)
                    {
                        var usuario = new WCFSeguridad.Usuario();
                        usuario.Rol = new WCFSeguridad.Rol();
                        usuario.Empleado = new WCFSeguridad.Empleado();

                        usuario.Empleado.DNI = grid.Rows[i].Cells[5].Text;
                        usuario.Rol.IdRol = IdGuardar;
                        string msg = ServiceSeguridadController.Instance.RegistrarUsuarioRol(usuario);
                    }

                    if (IdModificar > 0)
                    {
                        var usuario = new WCFSeguridad.Usuario();
                        usuario.Rol = new WCFSeguridad.Rol();
                        usuario.Empleado = new WCFSeguridad.Empleado();

                        usuario.Empleado.DNI = grid.Rows[i].Cells[5].Text;
                        usuario.Rol.IdRol = IdModificar;
                        string msg = ServiceSeguridadController.Instance.RegistrarUsuarioRol(usuario);
                    }

                    if (IdBuscar > 0)
                    {
                        var usuario = new WCFSeguridad.Usuario();
                        usuario.Rol = new WCFSeguridad.Rol();
                        usuario.Empleado = new WCFSeguridad.Empleado();

                        usuario.Empleado.DNI = grid.Rows[i].Cells[5].Text;
                        usuario.Rol.IdRol = IdBuscar;
                        string msg = ServiceSeguridadController.Instance.RegistrarUsuarioRol(usuario);
                    }

                    if (IdExportar > 0)
                    {
                        var usuario = new WCFSeguridad.Usuario();
                        usuario.Rol = new WCFSeguridad.Rol();
                        usuario.Empleado = new WCFSeguridad.Empleado();

                        usuario.Empleado.DNI = grid.Rows[i].Cells[5].Text;
                        usuario.Rol.IdRol = IdExportar;
                        string msg = ServiceSeguridadController.Instance.RegistrarUsuarioRol(usuario);
                    }

                    if (IdEnviar > 0)
                    {
                        var usuario = new WCFSeguridad.Usuario();
                        usuario.Rol = new WCFSeguridad.Rol();
                        usuario.Empleado = new WCFSeguridad.Empleado();

                        usuario.Empleado.DNI = grid.Rows[i].Cells[5].Text;
                        usuario.Rol.IdRol = IdEnviar;
                        string msg = ServiceSeguridadController.Instance.RegistrarUsuarioRol(usuario);
                    }
                }
            }
            catch (Exception ex) { }
        }

        #endregion


        GridView gridview = new GridView();
        protected void btnValidar_Click(object sender, EventArgs e)
        {
            try
            {
                gridview = new GridView();
                ValidarRegistros(gridview);
            }
            catch (Exception ex) { }   
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            RegistrarListaUsuariosExcel();
        }
    }
}