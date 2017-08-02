using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Slin.Facturacion.WS_PrintDoc
{
    partial class WService_PrintDoc : ServiceBase
    {
        public static string PathLogSLINADE = ConfigurationManager.AppSettings["PathLogSLINADE"].ToString() + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smp\";//log para los servicios
        string PathInput = ConfigurationManager.AppSettings["PathInput"].ToString();

        public WService_PrintDoc()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.

            try
            {
                CreateDirectory(PathLogSLINADE);
                using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADEPrintService.log", true, Encoding.UTF8))
                {
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                    sw.WriteLine("[" + DateTime.Now + "] El Servicio de Impresión se a iniciado Correctamente.");
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                }

                FileInfoList();

                this.Monitorear();
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADEPrintService.log", true, Encoding.UTF8))
                {
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                    sw.WriteLine("[" + DateTime.Now + "] Message: " + ex.Message);
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                }
            }
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.

            try
            {
                using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADEPrintService.log", true, Encoding.UTF8))
                {
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                    sw.WriteLine("[" + DateTime.Now + "] El Servicio de Impresión se a detenido Correctamente.");
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADEPrintService.log", true, Encoding.UTF8))
                {
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                    sw.WriteLine("[" + DateTime.Now + "] Message: " + ex.Message);
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                }
            }
        }


        #region method

        public void Monitorear()
        {
            //Directorio a monitorear
            FileSystemWatcher fsw = new FileSystemWatcher(PathInput);
            fsw.IncludeSubdirectories = true;
            fsw.Filter = "*.xml";
            fsw.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.DirectoryName;

            //Eventos a monitorear
            fsw.Created += new FileSystemEventHandler(OnChanged);
            //Habilitamos al componente a producir eventos
            fsw.EnableRaisingEvents = true;
        }

        static void OnChanged(object sender, FileSystemEventArgs e)
        {
            CreateDirectory(PathLogSLINADE);

            var listalog = new List<string>();

            try
            {
                //listalog.Add(" ");
                //listalog.Add("[" + DateTime.Now + "] ------------------------------INICIO---------------------------------");
                //listalog.Add("[" + DateTime.Now + "] Archivo Detectado: " + e.FullPath);
                //listalog.Add("[" + DateTime.Now + "] Esperando que termine de ser copiado para ser procesado");

                using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADEPrintService.log", true, Encoding.UTF8))
                {
                    sw.WriteLine(" ");
                    sw.WriteLine("[" + DateTime.Now + "] ------------------------------INICIO---------------------------------");
                    sw.WriteLine("[" + DateTime.Now + "] Archivo Detectado: " + e.FullPath);
                    sw.WriteLine("[" + DateTime.Now + "] Esperando que termine de ser copiado para ser procesado");
                }


                Console.WriteLine(" ");
                Console.WriteLine("[" + DateTime.Now + "] ------------------------------INICIO---------------------------------");
                Console.WriteLine("[" + DateTime.Now + "] Archivo Detectado: " + e.FullPath);
                Console.WriteLine("[" + DateTime.Now + "] Esperando que termine de ser copiado para ser procesado");


                string PathRutaXmlState = e.FullPath;
                if (GetExclusiveFileLock(e.FullPath))
                {
                    //se enviada para ser procesado
                    //PathExe_ProcessSend

                    //listalog.Add("[" + DateTime.Now + "] El archivo se a copiado con exito.");

                    using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADEPrintService.log", true, Encoding.UTF8))
                    {
                        sw.WriteLine("[" + DateTime.Now + "] El archivo se a copiado con exito.");
                    }


                    Console.WriteLine("[" + DateTime.Now + "] El archivo se a copiado con exito.");


                    //debo leer el documento que se envio para imprimir
                    var fileXml = new XmlDocument();
                    fileXml.Load(e.FullPath);

                    var DocPrint = Return_DocumentPrint(fileXml.InnerXml);


                    if (DocPrint.Length > 30)
                    {
                        string[] array = e.FullPath.Split('\\');
                        int index = array.Length;
                        string num_ce = array[index - 1].Replace(".xml", "");
                        string path_exe = Return_PathExe(num_ce);

                        if (System.IO.File.Exists(path_exe))
                        {
                            using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADEPrintService.log", true, Encoding.UTF8))
                            {
                                sw.WriteLine("[" + DateTime.Now + "] Execute     : " + DocPrint);
                            }


                            //new Process.PrintDocumentClass().Execute_PrintDoc(DocPrint);




                            System.Diagnostics.Process appy = new System.Diagnostics.Process();
                            appy.StartInfo.FileName = path_exe;
                            //appy.StartInfo.Arguments = " " + num_ce;
                            appy.StartInfo.Arguments = " " + DocPrint.Replace(" ", "smp");
                            appy.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            //p.StartInfo.UseShellExecute = false;
                            //p.StartInfo.RedirectStandardOutput = true;
                            if (System.IO.File.Exists(e.FullPath))
                            {
                                System.IO.File.Delete(e.FullPath);
                            }
                            appy.Start();
                            appy.WaitForExit();

                            if (System.IO.File.Exists(e.FullPath))
                            {
                                System.IO.File.Delete(e.FullPath);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                //listalog.Add("[" + DateTime.Now + "] Error: " + ex.Message);
                using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADEPrintService.log", true, Encoding.UTF8))
                {
                    sw.WriteLine("[" + DateTime.Now + "] Error: " + ex.Message);
                }
                Console.WriteLine("[" + DateTime.Now + "] Error: " + ex.Message);
            }

            //listalog.Add("[" + DateTime.Now + "] --------------------------------FIN----------------------------------");
            using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADEPrintService.log", true, Encoding.UTF8))
            {
                sw.WriteLine("[" + DateTime.Now + "] --------------------------------FIN----------------------------------");
            }

            Console.WriteLine("[" + DateTime.Now + "] --------------------------------FIN----------------------------------");

            //foreach (var obj in listalog)
            //{
            //    using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADEPrintService.log", true, Encoding.UTF8))
            //    {
            //        sw.WriteLine(obj);
            //    }
            //}
        }

        private static bool GetExclusiveFileLock(string path)
        {
            var fileReady = false;
            const int MaximumAttemptsAllowed = 15;
            var attemptsMade = 0;

            while (!fileReady && attemptsMade <= MaximumAttemptsAllowed)
            {
                try
                {
                    using (File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        return true;
                    }
                }
                catch (IOException ioex)
                {
                    attemptsMade++;
                    Thread.Sleep(1000);
                    //using (StreamWriter sw = new StreamWriter(pathlog + "WS_Service.log", true, Encoding.UTF8))
                    //{
                    //    sw.WriteLine("[" + DateTime.Now + "] Error: " + ioex.Message);
                    //}
                    //Console.WriteLine("[" + DateTime.Now + "] Error: " + ioex.Message);
                }
            }
            return fileReady;
        }

        private void FileInfoList()
        {
            try
            {
                CreateDirectory(PathLogSLINADE);

                List<string> listaDetected = new List<string>();
                List<string> listDoclog = new List<string>();
                //List<string> listDoc_ToSend = new List<string>();

                List<string> list_xml_line = new List<string>();

                DirectoryInfo di = new DirectoryInfo(PathInput);

                //listDoclog.Add(" ");
                //listDoclog.Add("[" + DateTime.Now + "] ------------------------------INICIO---------------------------------");

                

                foreach (var fi in di.GetFiles())
                {
                    using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADEPrintService.log", true, Encoding.UTF8))
                    {
                        sw.WriteLine(" ");
                        sw.WriteLine("[" + DateTime.Now + "] ------------------------------INICIO---------------------------------");
                    }

                    //listDoclog.Add("[" + DateTime.Now + "] Archivo Encontrado: " + fi.FullName);

                    using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADEPrintService.log", true, Encoding.UTF8))
                    {
                        sw.WriteLine("[" + DateTime.Now + "] Archivo Encontrado: " + fi.FullName);
                    }

                    listaDetected.Add(PathInput + fi.FullName);
                    string[] array = fi.FullName.Split('\\');
                    int index = array.Length;

                    if (array[index - 1].Contains(".xml"))
                    {
                        //listDoc_ToSend.Add(array[index - 1].Replace(".xml", ""));
                        //list_xml_line
                        var xml_file = new XmlDocument();
                        xml_file.Load(PathInput + array[index - 1]);
                        list_xml_line.Add(xml_file.InnerXml);
                    }


                    using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADEPrintService.log", true, Encoding.UTF8))
                    {
                        sw.WriteLine("[" + DateTime.Now + "] --------------------------------FIN----------------------------------");
                    }
                }
                //listDoclog.Add("[" + DateTime.Now + "] --------------------------------FIN----------------------------------");
                

                list_xml_line = Return_ListDocumentPrint(list_xml_line);



                foreach (var obj in list_xml_line)
                {
                    //send document

                    string path_exe = Return_PathExe(obj);

                    //if (System.IO.File.Exists(PathExe_ProcessSend))
                    if (System.IO.File.Exists(path_exe))
                    {
                        System.Diagnostics.Process appy = new System.Diagnostics.Process();
                        //appy.StartInfo.FileName = PathExe_ProcessSend; //path y name del ejecutable de auxiliares
                        appy.StartInfo.FileName = path_exe; //path y name del ejecutable de auxiliares
                        appy.StartInfo.Arguments = " " + obj.Replace(" ", "smp");
                        appy.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        //p.StartInfo.UseShellExecute = false;
                        //p.StartInfo.RedirectStandardOutput = true;
                        appy.Start();
                        appy.WaitForExit();

                        using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADEPrintService.log", true, Encoding.UTF8))
                        {
                            sw.WriteLine("[" + DateTime.Now + "] Execute     : " + obj);
                        }

                        //new Process.PrintDocumentClass().Execute_PrintDoc(obj);
                    }
                }

                foreach (var obj in listaDetected)
                {
                    if (System.IO.File.Exists(obj))
                    {
                        System.IO.File.Delete(obj);
                    }
                }
            }
            catch (Exception ex) { }
        }


        private static string Return_PathExe(string num_ce)
        {
            string[] array = num_ce.Split('-');
            string result = array[0];
            result = ConfigurationManager.AppSettings[result].ToString();
            return result;
        }

        private static void CreateDirectory(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        #endregion

        #region method get list num_ce

        private static string Return_DocumentPrint(string xml_line)
        {
            string objxml = string.Empty;


            var xmlread = new DocumentState();
            XmlSerializer xmlSerial = new XmlSerializer(typeof(DocumentState));
            using (TextReader reader = new StringReader(xml_line))
            {
                xmlread = (DocumentState)xmlSerial.Deserialize(reader);
            }
            objxml = xmlread.ID_CE + "*" + xmlread.Tipo_CE + "*" + xmlread.PrintName + "*" + xmlread.Copies + "*" + xmlread.TypeFormat;
            return objxml;
        }





        private List<string> Return_ListDocumentPrint(List<string> listxml_line)
        {
            List<string> list = new List<string>();


            foreach (var objxml in listxml_line)
            {
                var xmlread = new DocumentState();
                XmlSerializer xmlSerial = new XmlSerializer(typeof(DocumentState));
                using (TextReader reader = new StringReader(objxml))
                {
                    xmlread = (DocumentState)xmlSerial.Deserialize(reader);
                }

                string add = xmlread.ID_CE + "*" + xmlread.Tipo_CE + "*" + xmlread.PrintName + "*" + xmlread.Copies + "*" + xmlread.TypeFormat;
                list.Add(add);
            }            

            return list;
        }

        #endregion
    }
}
