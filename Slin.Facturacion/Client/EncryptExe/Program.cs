using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptExe
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().GeneraScript();
        }


        public void GeneraScript()
        {
            Console.WriteLine("Ingrese Accion a Realizar");
            Console.WriteLine("1.- Encrypt");
            Console.WriteLine("2.- Descrypt");
            Console.WriteLine("3.- Salir");

            string opt = Console.ReadLine();

            Console.WriteLine(Genera(opt));
            Console.ReadLine();
            new Program().GeneraScript();
        }

        public string Genera(string opt)
        {
            string text = string.Empty;
            switch (opt)
            {
                case "1":
                    {
                        Console.WriteLine("Ingrese Texto a Encriptar: ");
                        text = Console.ReadLine();
                        text = EncryptString(text);
                        break;
                    }

                case "2":
                    {
                        Console.WriteLine("Ingrese Texto a Desencriptar: ");
                        text = Console.ReadLine();

                        text = DesencryptString(text);
                        break;
                    }
                case "3":
                    {
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalido");
                        break;
                    }
            }

            return text;
        }

        private string EncryptString(string textEncrypt)
        {
            string textResult = string.Empty;
            //textResult = new Encrypt().EncryptKey(textEncrypt);
            textResult = new Encrypt().HashPassword(textEncrypt);
            return textResult;
        }

        private string DesencryptString(string textDesencrypt)
        {
            string textResult = string.Empty;
            textResult = new Encrypt().DecryptKey(textDesencrypt);
            return textResult;
        }




    }
}
