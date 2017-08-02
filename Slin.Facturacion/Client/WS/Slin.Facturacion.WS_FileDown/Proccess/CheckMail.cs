using Slin.Facturacion.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenPop.Pop3;

using Message = OpenPop.Mime.Message;
using OpenPop.Common.Logging;
using OpenPop.Mime;
using Slin.Facturacion.ServiceImplementation;
using Slin.Facturacion.Common;
using System.IO;
using System.Diagnostics;

namespace Slin.Facturacion.WS_FileDown.Proccess
{
    public class CheckMail
    {
        public static string PathLogSLINADE = ConfigurationManager.AppSettings["PathLogSLINADE"].ToString();

        #region ENTITY

        private Empresa objmailentity;
        public Empresa objMailEntity
        {
            get { return objmailentity; }
            set
            {
                objmailentity = value;
            }
        }

        List<string> List_AllProcessFiles = new List<string>();
        string pathlog_Company = string.Empty;
        string pathfiles_Company = string.Empty;


        //VARIABLES MAIL DOWN
        //private readonly Pop3Client pop3Client;
        private Pop3Client pop3Client;
        private readonly Dictionary<int, Message> messages = new Dictionary<int, Message>();
        //private Dictionary<int, Message> messages = new Dictionary<int, Message>();
        //private CForms.TreeView listMessages;
        //private CForms.TreeView ListAttachments;

        #endregion


        #region METHOD
        string path_service = string.Empty;
        public void getCredentialMail()
        {
            path_service = string.Empty;
            path_service = PathLogSLINADE + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smd\";


            

            CreateDirecoty(path_service);

            //entityIdRucDesencrypt = new Helper.Encrypt().DecryptKey(EntityId);



            var listCompany = new BusinessSecurity.Entity.EntityClass().getsListEntity();


            foreach (var obj in listCompany)
            {
                //var obj_com = new Helper.Encrypt().EncryptKey(obj);
                //obj = new Helper.Encrypt().EncryptKey(obj);
                //var ruc = new Helper.Encrypt().DecryptKey(obj);
                objMailEntity = new ServicioSeguridadSOA().GetCredentialEntity_Received(obj);
                //objMailEntity = new ServicioSeguridadSOA().GetCredentialEntity_Received(obj_com);

                if (objMailEntity.Email != null && objMailEntity.Email.Length > Constantes.ValorCero && objMailEntity.Password != null && objMailEntity.Password.Length > Constantes.ValorCero)
                {
                    List_AllProcessFiles = new List<string>();
                    ConnectPop(objMailEntity);

                    if (List_AllProcessFiles.Count > Constantes.ValorCero)
                    {

                        foreach (var cadfull in List_AllProcessFiles)
                        {
                            string[] array = cadfull.Split('|');
                            if (array.Length == Constantes.ValorDos)
                            {
                                new RegistrProcess().Execute_ProcessRegistr(array[0], array[1]);
                            }
                        }
                    }
                }
                else
                {
                    //ConnectPop(objMailEntity);
                    using (StreamWriter sw = new StreamWriter(path_service + @"\WS_Download.log", true, Encoding.UTF8))
                    {
                        sw.WriteLine("[" + DateTime.Now + "] -------------------------------------------INICIO-------------------------------------------");
                        sw.WriteLine("[" + DateTime.Now + "] El Servicio no está activo para la empresa con ruc: " + obj);
                        sw.WriteLine("[" + DateTime.Now + "] --------------------------------------------FIN----------------------------------------------");
                    }
                }
            }




            
        }

        //int cantMessages = Constantes.ValorCero;(ReadXml, file.FullName, obj.RUC);



        private void ConnectPop(Empresa obj)
        {
            List_AllProcessFiles = new List<string>();


            pathlog_Company = string.Empty;
            pathlog_Company = ConfigurationManager.AppSettings[obj.RUC].ToString();
            pathlog_Company = pathlog_Company + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smd\";


            CreateDirecoty(pathlog_Company);


            pathfiles_Company = string.Empty;
            pathfiles_Company = ConfigurationManager.AppSettings[obj.RUC].ToString();
            pathfiles_Company = pathfiles_Company + @"ReceivedCE\";

            //CreateDirecoty(pathfiles_Company);

            try
            {
                pop3Client = new Pop3Client();

                if (pop3Client.Connected)
                    pop3Client.Disconnect();
                pop3Client.Connect(obj.Dominio, obj.Port, obj.UseSSL == Constantes.ValorUno ? true : false);
                pop3Client.Authenticate(obj.Email, new Helper.Encrypt().DecryptKey(obj.Password));

                //pop3Client.Connect(obj.IP, obj.Port, obj.UseSSL == Constantes.ValorUno ? true : false);



                //if (pop3Client.Connected)
                //    pop3Client.Disconnect();
                //pop3Client.Connect("190.107.180.68", 110, false);
                //pop3Client.Authenticate("emisionfacturas@ancro.com.pe", new Helper.Encrypt().DecryptKey("F+s08niaKwY="));

                //if (pop3Client.Connected)
                //    pop3Client.Disconnect();
                //pop3Client.Connect("pop.gmail.com", 995, true);
                //pop3Client.Authenticate("riddarejosue@gmail.com", "asdasda");

                int count = pop3Client.GetMessageCount();

                int success = Constantes.ValorCero;
                int fail = Constantes.ValorCero;

                List<int> listaMailDownloadForDelete = new List<int>();

                bool donwload_IsOK = false;

                for (int i = count; i >= Constantes.ValorUno; i -= 1)
                {
                    try
                    {
                        Message message = pop3Client.GetMessage(i);
                        success++;

                        //Adjuntos
                        string nameAttachment = string.Empty;
                        List<MessagePart> attachment = message.FindAllAttachments();



                        #region METHOD FOR DOWNLOAD ATTACHMENT MESSAGE

                        //FileInfo file = new FileInfo("");
                        int cantAttachmentforDelete = Constantes.ValorCero;
                        int cantAttachment = Constantes.ValorCero;
                        FileInfo file;
                        if (attachment.Count > Constantes.ValorCero)
                        {
                            cantAttachment = attachment.Count;

                            for (int j = 0; j <= attachment.Count - 1; j++)
                            {
                                try
                                {
                                    #region IFF

                                    string PathDocumentReceived = string.Empty;
                                    if (attachment[j].FileName.Contains(Constantes.Ext_XML))
                                    {
                                        //PathDocumentReceived = PathDocumentReceived_ExtXML;
                                        PathDocumentReceived = pathfiles_Company + @"input_xml\";
                                    }
                                    else if (attachment[j].FileName.Contains(Constantes.Ext_PDF))
                                    {
                                        //PathDocumentReceived = PathDocumentReceived_ExtPDF;
                                        PathDocumentReceived = pathfiles_Company + @"input_pdf\";
                                    }
                                    else
                                    {
                                        //PathDocumentReceived = PathDocumentReceived_OtherExt;
                                        PathDocumentReceived = pathfiles_Company + @"input_other\";
                                    }

                                    nameAttachment = attachment[j].FileName;
                                    file = new FileInfo(PathDocumentReceived + nameAttachment);
                                    if (!System.IO.File.Exists(file.FullName))
                                    {
                                        attachment[j].Save(file);
                                        if (System.IO.File.Exists(PathDocumentReceived + nameAttachment))
                                        {
                                            cantAttachmentforDelete++;
                                        }
                                        using (StreamWriter sw = new StreamWriter(pathlog_Company + @"\WS_Download.log", true, Encoding.UTF8))
                                        {
                                            sw.WriteLine("[" + DateTime.Now + "] -------------------------------------------INICIO--------------------------------------------");
                                            sw.WriteLine("[" + DateTime.Now + "] Se ha descargado un archivo: " + PathDocumentReceived + nameAttachment + "!.");
                                            sw.WriteLine("[" + DateTime.Now + "] --------------------------------------------FIN----------------------------------------------");
                                        }

                                        if (System.IO.File.Exists(file.FullName))
                                        {
                                            //string ReadXml = System.IO.File.ReadAllText(file.FullName);
                                            //new RegistrProcess().Execute_ProcessRegistr(ReadXml, file.FullName, obj.RUC);
                                            List_AllProcessFiles.Add(file.FullName + "|" + obj.RUC);
                                        }
                                    }
                                    else if (System.IO.File.Exists(file.FullName))
                                    {
                                        attachment[j].Save(file);
                                        cantAttachmentforDelete++;
                                        using (StreamWriter sw = new StreamWriter(pathlog_Company + @"\WS_Download.log", true, Encoding.UTF8))
                                        {
                                            sw.WriteLine("[" + DateTime.Now + "] -------------------------------------------INICIO--------------------------------------------");
                                            sw.WriteLine("[" + DateTime.Now + "] El archivo: " + PathDocumentReceived + nameAttachment + ", ya existe!, se a reeamplazado el archivo.");
                                            sw.WriteLine("[" + DateTime.Now + "] --------------------------------------------FIN----------------------------------------------");
                                        }
                                    }
                                    else { }

                                    #endregion

                                }
                                catch (Exception ex)
                                {
                                    //Console.WriteLine("[" + DateTime.Now + "] Error: " + ex.Message + ", " + ex.InnerException);
                                    using (StreamWriter sw = new StreamWriter(pathlog_Company + @"\WS_Download.log", true, Encoding.UTF8))
                                    {
                                        sw.WriteLine("[" + DateTime.Now + "] Error: " + ex.Message);
                                    }
                                }
                            }

                            //ELIMINAR EL CORREO SOLO SI LA CANTIDAD DE ARCHIVOS DESCARGADOS ES IGUAL A LA CANT DE ARCHV. ADJTS. QUE CONTENIA EL CORREO
                            if (cantAttachmentforDelete == cantAttachment)
                            {
                                //pop3Client.DeleteMessage(i);
                                listaMailDownloadForDelete.Add(i);
                            }


                        }

                        #endregion END METHOD DOWNLOAD
                    }
                    catch (Exception e)
                    {
                        //DefaultLogger.Log.LogError("TestForm: Message fetching failed: " + e.Message + "\r\n" +
                        //    "Stack trace:\r\n" +
                        //    e.StackTrace);
                        //fail++;

                        using (StreamWriter sw = new StreamWriter(pathlog_Company + @"\WS_Download.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine("[" + DateTime.Now + "] Error: " + e.Message);
                        }
                    }
                    //AQUI CARGABA EN PROGRESSBAR
                    //Console.WriteLine("Mail received!\nSuccesses: " + success + "\nFailed: " + fail + " Message fetching done");
                }

                int cantMessagesDeleted = Constantes.ValorCero;
                try
                {
                    for (int del = 0; del <= listaMailDownloadForDelete.Count; del++)
                    {
                        pop3Client.DeleteMessage(listaMailDownloadForDelete[del]);
                        cantMessagesDeleted++;
                    }
                }
                catch (Exception ex) { }
                if (cantMessagesDeleted == listaMailDownloadForDelete.Count)
                {
                    pop3Client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(pathlog_Company + @"\WS_Download.log", true, Encoding.UTF8))
                {
                    sw.WriteLine("[" + DateTime.Now + "] Error: " + ex.Message);
                }
            }
        }

        #endregion



        #region CREATE DIRECTORY FILES

        private void CreateDirecoty(string pathDirectory)
        {
            if (!Directory.Exists(pathDirectory))
            {
                Directory.CreateDirectory(pathDirectory);
            }

        }

        #endregion

    }
}
