using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Test_Console_https
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Program().PrintECompany();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message: " + ex.Message + ", " + ex.InnerException);
            }
            Console.ReadLine();
            Console.Read();
        }



        private void PrintECompany()
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                wcf_mantenimiento.ServicioMantenimientoClient client = new wcf_mantenimiento.ServicioMantenimientoClient();

                var list = client.GetListaEmpresa();


                foreach (var obj in list)
                {
                    Console.WriteLine("Compañia: " + obj.IdEmpresa + ", " + obj.RazonSocial);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Message: " + ex.Message + ", " + ex.InnerException);
            }

            Console.ReadLine();
            Console.Read();
        }
    }
}
