using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.Common.Helper
{
    public class ListClass
    {

        public ListaEmail GetListTypeMail_Notify()
        {
            Email oMail = new Email();
            ListaEmail oListMail = new ListaEmail();

            try
            {
                oListMail.Insert(0, new Email() { TypeMail = "F", Description = "Facturación" });
                oListMail.Insert(0, new Email() { TypeMail = "S", Description = "Soporte" });
                //oListaMail.Insert(0, new Email() { TypeMail = " ", = "- Seleccione -" });
            }
            catch (Exception ex) { }
            return oListMail;
        }

        public static string GetValue_TypeDocClie(string Code)
        {
            string Value = string.Empty;

            switch (Code)
            {
                case "0": { Value = Constantes.Code_0; break; }
                case "1": { Value = Constantes.Code_1; break; }
                case "4": { Value = Constantes.Code_4; break; }
                case "6": { Value = Constantes.Code_6; break; }
                case "7": { Value = Constantes.Code_7; break; }
                case "A": { Value = Constantes.Code_A; break; }
            }
            return Value;
        }







    }


    public class Email
    {
        public string Para { get; set; }
        public string CC { get; set; }
        public string CCO { get; set; }
        public string TypeMail { get; set; }
        public string Description { get; set; }
    }

    public class ListaEmail : List<Email>
    {

    }


    public class TypeDocIndentification
    {
        public string Code { get; set; }
        public string Value { get; set; }
    }

    public class ListTypeDocIndentificaction : List<TypeDocIndentification>
    {

    }




    public class TypeNC
    {
        public string Code { get; set; }
        public string Desc { get; set; }
    }

    public class ListTypeNC : List<TypeNC>
    {
        public ListTypeNC Return_ListTypeNC()
        {
            var list = new ListTypeNC();
            return list;
        }
    }



    public class TypeND
    {
        public string Code { get; set; }
        public string Desc { get; set; }
    }

    public class ListTypeND : List<TypeNC>
    {

    }


}
