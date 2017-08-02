using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace Slin.Facturacion.BusinessEntities
{
    [CollectionDataContract]
    public class ListaTipoDocumento : List<TipoDocumento>
    {
    }
}
