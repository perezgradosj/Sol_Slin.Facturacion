using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Configuration;

using CForms = System.Windows.Forms;

using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.ServiceImplementation;
using Slin.Facturacion.Common;


using OpenPop.Pop3;
//using OpenPop.Mime;

using Message = OpenPop.Mime.Message;
using Slin.Facturacion.DownMail.Helper;
using OpenPop.Common.Logging;
using OpenPop.Mime;


namespace Slin.Facturacion.DownMail
{
    class Program
    {
        #region GLOBAL VARIBLES

        public string PathDocumentReceived_ExtXML = ConfigurationManager.AppSettings["PathDocumentReceived_ExtXML"].ToString();
        public string PathDocumentReceived_ExtPDF = ConfigurationManager.AppSettings["PathDocumentReceived_ExtPDF"].ToString();
        public string PathDocumentReceived_OtherExt = ConfigurationManager.AppSettings["PathDocumentReceived_OtherExt"].ToString();

        public string Path_Log = ConfigurationManager.AppSettings["PathListErrorLog"].ToString();


        //public string Path_List_DocNotInserted = ConfigurationManager.AppSettings["PathListDocNotInserted"].ToString();
        //public string Path_List_DocInserted = ConfigurationManager.AppSettings["PathListDocInserted"].ToString();
        //public string Path_List_ErrorLog = ConfigurationManager.AppSettings["PathListErrorLog"].ToString();

        //public string Path_MoveDocInserted_OK = ConfigurationManager.AppSettings["PathMoveInsertedOK"].ToString();
        //public string Path_MoveDoc_NotInserted = ConfigurationManager.AppSettings["PathMoveDocNotInserted"].ToString();



        #endregion END GLOBAL VARIABLES

        static void Main(string[] args)
        {
            //new Program().getCredentialMail();


            new Slin.Facturacion.WS_FileDown.Proccess.CheckMail().getCredentialMail();
        }



        #region ENTITY


        //private string entityIdRucEncrypt = "t6U5yI/3RJUlfJIc1RVpSg==";//ancro
        //string entityIdRucEncrypt = "+j4hnjICO0lWKUW5T4zeVQ==";//slin
        string entityIdRucEncrypt = "GKut1XliNKJ1uHZpF8LchA==";//tecni services
        //private string entityIdRucEncrypt = "e+xtM8QKrNB1sZLqkpO5LQ==";//PRICE PWC
        //private string entityIdRucEncrypt = "1q7ew7gL0RtiuizQVmG8jw==";//GAVEGLIO


        string entityIdRucDesencrypt = string.Empty;// get RucEntity desencrypt


        private Empresa objmailentity;
        public Empresa objMailEntity
        {
            get { return objmailentity; }
            set
            {
                objmailentity = value;
            }
        }




        //VARIABLES MAIL DOWN
        //private readonly Pop3Client pop3Client;
        private Pop3Client pop3Client;
        private readonly Dictionary<int, Message> messages = new Dictionary<int, Message>();
        //private Dictionary<int, Message> messages = new Dictionary<int, Message>();
        //private CForms.TreeView listMessages;
        //private CForms.TreeView ListAttachments;

        #endregion


        #region METHOD

        private void getCredentialMail()
        {
            entityIdRucDesencrypt = new Helper.Encrypt().DecryptKey(entityIdRucEncrypt);

            objMailEntity = new ServicioSeguridadSOA().GetCredentialEntity_Received(entityIdRucDesencrypt);

            if (objMailEntity.Email != null && objMailEntity.Email.Length > Constantes.ValorCero && objMailEntity.Password != null && objMailEntity.Password.Length > Constantes.ValorCero)
            {
                ConnectPop(objMailEntity);
            }
            else
            {
                ConnectPop(objMailEntity);
            }
        }


        int cantMessages = Constantes.ValorCero;
        private void ConnectPop(Empresa obj)
        {
            string path = Path_Log + DateTime.Now.Year + "_" + DateTime.Now.Month;
            CreateDirecoty(path);

            try
            {
                pop3Client = new Pop3Client();

                //if (pop3Client.Connected)
                //    pop3Client.Disconnect();
                //pop3Client.Connect(obj.Dominio, obj.Port, obj.UseSSL == Constantes.ValorUno ? true : false);
                //pop3Client.Authenticate(obj.Email, new Helper.Encrypt().DecryptKey(obj.Password));
                ////pop3Client.Connect(obj.Dominio, obj.Port, true);
                

                //if (pop3Client.Connected)
                //    pop3Client.Disconnect();
                //pop3Client.Connect("190.107.180.68", 110, false);
                //pop3Client.Authenticate("emisionfacturas@ancro.com.pe", new Helper.Encrypt().DecryptKey("F+s08niaKwY="));


                if (pop3Client.Connected)
                    pop3Client.Disconnect();
                pop3Client.Connect("pop.gmail.com", 995, true);
                pop3Client.Authenticate("riddarejosue@gmail.com", ".123borussia");

                int count = pop3Client.GetMessageCount();


                cantMessages = count;
                //messages.Clear();

                //messages = new Dictionary<int, Message>();

                //listMessages.Nodes.Clear();
                //ListAttachments.Nodes.Clear();

                int success = Constantes.ValorCero;
                int fail = Constantes.ValorCero;



                List<int> listaMailDownloadForDelete = new List<int>();

                for (int i = count; i >= Constantes.ValorUno; i -= 1)
                {

                    try
                    {
                        Message message = pop3Client.GetMessage(i);

                        //messages.Add(i, message);
                        //System.Windows.Forms.TreeNode node = new TreeNodeBuilder().VisitMessage(message);
                        //node.Tag = i;
                        //listMessages.Nodes.Add(node);
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
                                        PathDocumentReceived = PathDocumentReceived_ExtXML;
                                    }
                                    else if (attachment[j].FileName.Contains(Constantes.Ext_PDF))
                                    {
                                        PathDocumentReceived = PathDocumentReceived_ExtPDF;
                                    }
                                    else
                                    {
                                        PathDocumentReceived = PathDocumentReceived_OtherExt;
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
                                    }
                                    else if (System.IO.File.Exists(file.FullName))
                                    {
                                        cantAttachmentforDelete++;
                                        using (StreamWriter sw = new StreamWriter(path + @"\WS_Download.log", true, Encoding.UTF8))
                                        {
                                            sw.WriteLine("[" + DateTime.Now + "] -------------------------------------------INICIO-------------------------------------------");
                                            sw.WriteLine("[" + DateTime.Now + "] El archivo: " + PathDocumentReceived + nameAttachment + ", ya existe!.");
                                            sw.WriteLine("[" + DateTime.Now + "] --------------------------------------------FIN----------------------------------------------");
                                        }
                                    }
                                    else
                                    {
                                        //cantAttachment++;
                                        //using (StreamWriter sw = new StreamWriter(path + @"\WS_Download.log", true, Encoding.UTF8))
                                        //{
                                        //    sw.WriteLine("[" + DateTime.Now + "] -------------------------------------------INICIO-------------------------------------------");
                                        //    sw.WriteLine("[" + DateTime.Now + "] El archivo: " + PathDocumentReceived + nameAttachment + ", ya existe!.");
                                        //    sw.WriteLine("[" + DateTime.Now + "] --------------------------------------------FIN----------------------------------------------");
                                        //}
                                    }

                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    using (StreamWriter sw = new StreamWriter(path + @"\WS_Download.log", true, Encoding.UTF8))
                                    {
                                        sw.WriteLine("[" + DateTime.Now + "] Eeror " + ex.Message + ", " + ex.InnerException);
                                    }
                                }
                            }

                            //ELIMINAR EL CORREO SOLO SI LA CANTIDAD DE ARCHIVOS DESCARGADOS ES IGUAL A LA CANT DE ARCHV. ADJTS. QUE CONTENIA EL CORREO
                            if (cantAttachmentforDelete == cantAttachment)
                            {
                                //pop3Client.DeleteMessage(i);
                                //pop3Client.Disconnect();
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
                        
                        using (StreamWriter sw = new StreamWriter(path + @"\WS_Download.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine("[" + DateTime.Now + "] Eeror " + e.Message + ", " + e.InnerException);
                        }
                    }

                    //AQUI CARGABA EN PROGRESS BAR

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

            }
        }

        #endregion


        private void CreateDirecoty(string pathDirectory)
        {
            if (!Directory.Exists(pathDirectory))
            {
                Directory.CreateDirectory(pathDirectory);
            }

        }

    }
}
