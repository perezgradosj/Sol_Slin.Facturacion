using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.BusinessEntities.Helper
{
    public class ExcelRead
    {
        public string CodigoDistrito { get; set; }
        public string CodigoProvincia { get; set; }
        public string CodigoDepartamento { get; set; }
        public string CodigoPais { get; set; }

        public string IdDistrito { get; set; }
        public string IdProvincia { get; set; }
        public string IdDepartamento { get; set; }
        public string IdPaid { get; set; }

        public string DescripcionDistrito { get; set; }
        public string DescripcionProvincia { get; set; }
        public string DescripcionDepartamento { get; set; }
        public string DescripcionPais { get; set; }
    }

    public class ListaExcelRead : List<ExcelRead>
    {

    }


    
}
