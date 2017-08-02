using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.Common
{
    public class Equivalentes
    {
        private static readonly Equivalentes instance = new Equivalentes();
        static Equivalentes() { }
        private Equivalentes() { }
        public static Equivalentes Instance { get { return instance; } }

        public string Get_Equivalente(string val)
        {

            string response = string.Empty;
            #region case

            switch (val)
            {
                case Constantes.Val_NIU:
                    { response = Constantes.Val_UNI; break; }
                case Constantes.Val_KGM:
                    { response = Constantes.Val_KG; break; }
                case Constantes.Val_MTR:
                    { response = Constantes.Val_METRO; break; }
                case Constantes.Val_BX:
                    { response = Constantes.Val_CAJA; break; }
                case Constantes.Val_PK:
                    { response = Constantes.Val_PACK; break; }
                case Constantes.Val_LTR:
                    { response = Constantes.Val_LT; break; }
                case Constantes.Val_GLL:
                    { response = Constantes.Val_GLN; break; }
                case Constantes.Val_CMT:
                    { response = Constantes.Val_CM; break; }
                case Constantes.Val_P4:
                    { response = Constantes.Val_4PACK; break; }
                case Constantes.Val_KT:
                    { response = Constantes.Val_KIT; break; }
                case Constantes.Val_CNP:
                    { response = Constantes.Val_PQ100; break; }
                case Constantes.Val_MTQ:
                    { response = Constantes.Val_M3; break; }
                case Constantes.Val_RO:
                    { response = Constantes.Val_ROLL; break; }
                case Constantes.Val_BG:
                    { response = Constantes.Val_BOLSA; break; }
                case Constantes.Val_MTK:
                    { response = Constantes.Val_M2; break; }
                case Constantes.Val_HUR:
                    { response = Constantes.Val_HORA; break; }
            }

            #endregion

            return response;
        }
    }
}
