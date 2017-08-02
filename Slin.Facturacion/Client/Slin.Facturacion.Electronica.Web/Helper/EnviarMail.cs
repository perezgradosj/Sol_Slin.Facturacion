using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Slin.Facturacion.Electronica.Web.Helper
{
    public class EnviarMail
    {
        MailMessage ms = new MailMessage();
        SmtpClient smtp = new SmtpClient();

        public bool EnviarCorreo(string from, string password, string to, string mensaje)
        {
            try
            {
                ms.From = new MailAddress(from);
                ms.To.Add(new MailAddress(to));
                ms.Body = mensaje;
                //smtp.Host = "smtp.gmail.com";

                //smtp.Host = "smtp-mail.outlook.com";
                
                smtp.Credentials = new NetworkCredential(from, password);
                smtp.Port = 587;
                smtp.EnableSsl = false;
                //smtp.UseDefaultCredentials = false;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.Send(ms);

                return true;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }
    }
}