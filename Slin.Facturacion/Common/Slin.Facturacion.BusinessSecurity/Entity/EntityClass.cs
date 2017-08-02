using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.BusinessSecurity.Entity
{
    public class EntityClass
    {
        public string EntityId_3 = "+j4hnjICO0lWKUW5T4zeVQ==";//SLIN
        //public string EntityId_2 = "e+xtM8QKrNB1sZLqkpO5LQ==";//PWC
        //public string EntityId_3 = "1q7ew7gL0RtiuizQVmG8jw==";//GAVEGLIO
        //public string EntityId_4 = "t6U5yI/3RJUlfJIc1RVpSg==";//ANCRO
        //public string EntityId_5 = "uQ09eyP86TPgqemom5M9Kg==";//DEPOSEGURO
        //public string EntityId = "GKut1XliNKJ1uHZpF8LchA==";//TECNI SERVICES
        //public string EntityId_7 = "qpm6va9nIP0R6RZ/S4veAA=="; //GAMMA
        public string EntityId = "D9GQxN7ifX0noiQ3guZnaw==";//AIRFRANCE
        public string EntityId_2 = "QFLs2zNVt8ZTDcZLFgtfvQ==";//KLM

        //List<string> listentity = new List<string>();

        public List<string> getsListEntity()
        {
            List<string> listentity = new List<string>();
            listentity.Add(new Helper.Encrypt().DecryptKey(EntityId));
            listentity.Add(new Helper.Encrypt().DecryptKey(EntityId_2));
            listentity.Add(new Helper.Encrypt().DecryptKey(EntityId_3));
            //listentity.Add(new Helper.Encrypt().DecryptKey(EntityId_4));
            //listentity.Add(new Helper.Encrypt().DecryptKey(EntityId_5));
            //listentity.Add(new Helper.Encrypt().DecryptKey(EntityId_6));
            //listentity.Add(new Helper.Encrypt().DecryptKey(EntityId_7));
            //listentity.Add(new Helper.Encrypt().DecryptKey(EntityId_8));
            //listentity.Add(new Helper.Encrypt().DecryptKey(EntityId_9));
            return listentity;
        }
    }
}
