using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    
    [DataContract]
    public class Producto
    {
        [DataMember]
        public Int32 IdProducto { get; set; }

        [DataMember]
        public String CodigoProducto { get; set; }

        [DataMember]
        public String Descripcion { get; set; }

        [DataMember]
        public String UnidadMedida { get; set; }

        [DataMember]
        public Precio Precio { get; set; }

        [DataMember]
        public Decimal Cantidad { get; set; }

        [DataMember]
        public decimal ValorVenta { get; set; }

        [DataMember]
        public decimal PrecioVenta { get; set; }
    }
}
