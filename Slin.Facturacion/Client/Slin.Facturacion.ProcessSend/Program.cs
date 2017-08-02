using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

using LinqToExcel;

using Slin.Facturacion.BusinessEntities.Helper;
using Slin.Facturacion.DataAccess;
using Slin.Facturacion.BusinessEntities;


namespace Slin.Facturacion.ProcessSend
{
    class Program
    {
        public static string PathExcelUbigeo = ConfigurationManager.ConnectionStrings["PathExcel"].ToString();

        static void Main(string[] args)
        {
            new Program().GeneraProceso();
        }


        private ExcelRead objubi;
        public ExcelRead ObjUbi
        {
            get { return objubi; }
            set { objubi = value; }
        }

        private ListaExcelRead olistaexcelread;
        public ListaExcelRead oListaExcelRead
        {
            get { return olistaexcelread; }
            set { olistaexcelread = value; }
        }

        public List<ExcelRead> ToEntidadHojaExcelList(string pathDelFicheroExcel)
        {
            var book = new ExcelQueryFactory(pathDelFicheroExcel);
            var resultado = (from row in book.Worksheet("Hoja 1")
                             let item = new ExcelRead
                             {
                                 CodigoDepartamento = row["CodigoDepartamento"].Cast<string>(),
                                 CodigoProvincia = row["CodigoProvincia"].Cast<string>(),
                                 CodigoDistrito = row["CodigoDistrito"].Cast<string>(),
                                 DescripcionDepartamento = row["DescDepartamento"].Cast<string>(),
                                 DescripcionProvincia = row["DescProvincia"].Cast<string>(),
                                 DescripcionDistrito = row["DescDistrito"].Cast<string>()
                             }
                             select item).ToList();

            book.Dispose();
            return resultado;
        }

        public void GeneraProceso()
        {
            var lista = ToEntidadHojaExcelList(PathExcelUbigeo);
            GeneraLista(lista);
        }

        public void GeneraLista(List<ExcelRead> lista)
        {
            oListaExcelRead = new ListaExcelRead();

            List<string> list = new List<string>();

            foreach (var ubi in lista)
            {
                ObjUbi = new ExcelRead();

                ObjUbi.CodigoDepartamento = ubi.CodigoDepartamento;
                ObjUbi.DescripcionDepartamento = ubi.DescripcionDepartamento;
                ObjUbi.CodigoProvincia = ubi.CodigoProvincia;
                ObjUbi.DescripcionProvincia = ubi.DescripcionProvincia;
                ObjUbi.CodigoDistrito = ubi.CodigoDistrito;
                ObjUbi.DescripcionDistrito = ubi.DescripcionDistrito;
                if (ubi.CodigoDepartamento != null)
                {
                    oListaExcelRead.Add(ObjUbi);
                }
            }
            InsertarRegistros();
        }

        private void InsertarRegistros()
        {
            int cont = 0;
            Console.WriteLine("============DEPARTAMENTO===========");
            foreach (var obj in oListaExcelRead)
            {
                cont++;
                var result = new ConfiguracionDataAccess().InsertDepartament(obj);

                if (result == "Registrado Correctamente")
                {
                    Console.WriteLine("Nro: " + cont + " EL Registro " + obj.DescripcionDepartamento + " se a " + result);
                }
            }
            Console.WriteLine("==========END DEPARTAMENTO==========");
            Console.WriteLine("====================================");


            cont = 0;
            Console.WriteLine("==========DISTRITOS==========");
            foreach (var obj in oListaExcelRead)
            {
                cont++;
                var result = new ConfiguracionDataAccess().InsertarDistritoxProvincia(obj);

                if (result == "Registrado Correctamente")
                {
                    Console.WriteLine("Nro: " + cont + ": EL Registro " + obj.DescripcionDistrito + " se a " + result);
                }
            }
            Console.WriteLine("==========END DISTRITOS==========");
            Console.WriteLine("=================================");
            Console.WriteLine("============PROVINCIAS===========");
            cont = 0;
            foreach (var obj in oListaExcelRead)
            {
                cont++;

                var result = new ConfiguracionDataAccess().InsertarProvinciaxDepartament(obj);

                if (result == "Registrado Correctamente")
                {
                    Console.WriteLine("Nro: " + cont + " EL Registro " + obj.DescripcionProvincia + " se a " + result);
                }
            }
            Console.WriteLine("==========END PROVINCIAS==========");
            Console.WriteLine("==================================");
            Console.WriteLine("=======ACTUALIZA DISTRITO=========");
            cont = 0;
            foreach (var obj in oListaExcelRead)
            {
                cont++;
                var result = new ConfiguracionDataAccess().UpdateDistritoxProvincia(obj);

                if (result == "Actualizado Correctamente")
                {
                    Console.WriteLine("Nro: " + cont + ": EL Registro " + obj.DescripcionDistrito + " se a " + result);
                }
            }
            Console.WriteLine("======END ACTUALIZA DISTRITO=======");
            Console.ReadLine();
        }
    }
}
