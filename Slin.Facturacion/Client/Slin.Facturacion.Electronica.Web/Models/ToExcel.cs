using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Web.UI.WebControls;


namespace Slin.Facturacion.Electronica.Web.Models
{
    public class ToExcel
    {
        OleDbConnection conn;
        OleDbDataAdapter MyDataAdapter;
        DataTable dt;


        void ImportarExcel(GridView gv, String nombreHoja) 
        {
            string ruta = string.Empty;

            try
            {
                OpenFileDialog openFile1 = new OpenFileDialog();
                openFile1.Filter = "Excel Files |*.xlsx";
                openFile1.Title = "Seleccione Archivo de Excel";

                if (openFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (openFile1.FileName.Equals("") == false)
                    {
                        ruta = openFile1.FileName;
                    }
                }

                conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;data source=" + ruta + "; Extended Properties='Excel 12.0 Xml;HDR=Yes'");
                MyDataAdapter = new OleDbDataAdapter("Select * from [" + nombreHoja + "$]", conn);
                dt = new DataTable();
                MyDataAdapter.Fill(dt);
                gv.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}