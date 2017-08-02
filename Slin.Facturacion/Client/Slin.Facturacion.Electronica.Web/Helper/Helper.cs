using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using Microsoft.Reporting.WebForms;
//using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Common;
//using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.Proxies.ServicioFacturacion;

using iTextSharp.text.pdf;
using System.Drawing;
using System.IO;
using System.Data;
using System.ComponentModel;
using System.Globalization;
//using System.Globalization;

namespace Slin.Facturacion.Electronica.Web.Helper
{
    public class Helper
    {
        //private void ExportGridToExcel()
        //{

        //}


        //public static Nullable<T> DbValueToNullable<T>(object dbValue) where T : struct
        //{
        //    Nullable<T> returnValue = null;

        //    if ((dbValue != null) && (dbValue != DBNull.Value))
        //    {
        //        returnValue = (T)dbValue;
        //    }

        //    return returnValue;
        //}

        //public static T DbValueToDefault<T>(object obj)
        //{
        //    if (obj == null || obj == DBNull.Value) return default(T);
        //    else { return (T)obj; }
        //}

        
        public int SemanaAnio()
        {
            DateTime date = DateTime.Now;
            System.Globalization.CultureInfo norwCulture =
            System.Globalization.CultureInfo.CreateSpecificCulture("es");
            System.Globalization.Calendar cal = norwCulture.Calendar;
            int weekNo = cal.GetWeekOfYear(date,
            norwCulture.DateTimeFormat.CalendarWeekRule,
            norwCulture.DateTimeFormat.FirstDayOfWeek);
            // Show the result
            //string texto = weekNo.ToString();


            return weekNo - Constantes.ValorDos;
        }


        public void FechaSemana()
        {
            var time = DateTime.Now.DayOfWeek;
        }


        public DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }



        public DateTime FirstDateOfWeekISO8601_2(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            //return result.AddDays(-3);
            result.AddDays(-3);

            var resultado = result;

            return resultado.AddDays(3);
        }



        public DataTable TransformarToDataTable(IEnumerable<IGrouping<string, DataRow>> datos)
        {
            //
            // Se define la estructura del DataTable
            //
            DataTable dt = new DataTable();
            dt.Columns.Add("Documento");
            dt.Columns.Add("CantRegistros");
            dt.Columns.Add("Total");

            //
            // Se recorre cada valor agruparo por linq y se vuelca el resultado 
            // en un nuevo registro del datatable
            //
            foreach (IGrouping<string, DataRow> item in datos)
            {
                DataRow row2 = dt.NewRow();
                row2["Documento"] = item.Key;
                row2["CantRegistros"] = item.Count();
                row2["Total"] = item.Sum<DataRow>(x => Convert.ToInt32(x["Total"]));

                dt.Rows.Add(row2);
            }

            return dt;
        }
    }
}