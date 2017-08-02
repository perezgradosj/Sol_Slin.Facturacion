using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.BusinessEntities
{
    public class Precio
    {
        public Int32 IdPrecio { get; set; }
        public Int32 IdProducto { get; set; }
        public String CodigoProducto { get; set; }
        public Moneda Moneda { get; set; }
        public Decimal Valor { get; set; }
    }
}
