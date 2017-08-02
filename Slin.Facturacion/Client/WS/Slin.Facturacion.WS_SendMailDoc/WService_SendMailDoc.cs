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

namespace Slin.Facturacion.WS_SendMailDoc
{
    partial class WService_SendMailDoc : ServiceBase
    {
        public static string PathLogSLINADE = ConfigurationManager.AppSettings["PathLogSLINADE"].ToString() + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smc\";//log para los servicios
        string PathInput = ConfigurationManager.AppSettings["PathInput"].ToString();


        public WService_SendMailDoc()
        {
            InitializeComponent();
            //FileInfoList();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.

            try
            {
                CreateDirectory(PathLogSLINADE);
                using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADESendService.log", true, Encoding.UTF8))
                {
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                    sw.WriteLine("[" + DateTime.Now + "] El Servicio de Envio de Correos se a iniciado Correctamente.");
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                }

                FileInfoList();

                this.Monitorear();
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADESendService.log", true, Encoding.UTF8))
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
                CreateDirectory(PathLogSLINADE);
                using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADESendService.log", true, Encoding.UTF8))
                {
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                    sw.WriteLine("[" + DateTime.Now + "] El Servicio de Envio de Correos se a detenido Correctamente.");
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADESendService.log", true, Encoding.UTF8))
                {
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                    sw.WriteLine("[" + DateTime.Now + "] Message: " + ex.Message);
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                }
            }
        }


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


        //void OnChanged(object sender, FileSystemEventArgs e)
        static void OnChanged(object sender, FileSystemEventArgs e)
        {
            CreateDirectory(PathLogSLINADE);

            var listalog = new List<string>();

            try
            {
                listalog.Add(" ");
                listalog.Add("[" + DateTime.Now + "] ------------------------------INICIO---------------------------------");
                listalog.Add("[" + DateTime.Now + "] Archivo Detectado: " + e.FullPath);
                listalog.Add("[" + DateTime.Now + "] Esperando que termine de ser copiado para ser procesado");

                Console.WriteLine(" ");
                Console.WriteLine("[" + DateTime.Now + "] ------------------------------INICIO---------------------------------");
                Console.WriteLine("[" + DateTime.Now + "] Archivo Detectado: " + e.FullPath);
                Console.WriteLine("[" + DateTime.Now + "] Esperando que termine de ser copiado para ser procesado");


                string PathRutaXmlState = e.FullPath;
                if (GetExclusiveFileLock(e.FullPath))
                {
                    //se enviada para ser procesado
                    //PathExe_ProcessSend

                    listalog.Add("[" + DateTime.Now + "] El archivo se a copiado con exito.");
                    Console.WriteLine("[" + DateTime.Now + "] El archivo se a copiado con exito.");

                    string[] array = e.FullPath.Split('\\');
                    int index = array.Length;

                    string num_ce = array[index - 1].Replace(".xml", "");


                    //string[] array_id = num_ce.Split('-');
                    //int index_id = array_id.Length;
                    //string CUR = array_id[0].ToString();

                    string path_exe = Return_PathExe(num_ce);


                    //if (System.IO.File.Exists(PathExe_ProcessSend))
                    if (System.IO.File.Exists(path_exe))
                    {
                        System.Diagnostics.Process appy = new System.Diagnostics.Process();
                        //appy.StartInfo.FileName = PathExe_ProcessSend; //path y name del ejecutable de auxiliares
                        appy.StartInfo.FileName = path_exe; //path y name del ejecutable de auxiliares
                        //appy.StartInfo.Arguments = " " + array[index - 1].Replace(".xml", "");
                        appy.StartInfo.Arguments = " " + num_ce;
                        //appy.StartInfo.Arguments = "/c " + array[index - 1];
                        appy.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        //appy.StartInfo.UseShellExecute = false;
                        //p.StartInfo.RedirectStandardOutput = true;

                        if (System.IO.File.Exists(e.FullPath))
                        {
                            System.IO.File.Delete(e.FullPath);
                        }

                        appy.Start();
                        appy.WaitForExit();
                    }

                    //if (System.IO.File.Exists(e.FullPath))
                    //{
                    //    System.IO.File.Delete(e.FullPath);
                    //}
                }
            }
            catch (Exception ex)
            {
                listalog.Add("[" + DateTime.Now + "] Error: " + ex.Message);
                Console.WriteLine("[" + DateTime.Now + "] Error: " + ex.Message);
            }

            listalog.Add("[" + DateTime.Now + "] --------------------------------FIN----------------------------------");
            Console.WriteLine("[" + DateTime.Now + "] --------------------------------FIN----------------------------------");

            foreach (var obj in listalog)
            {
                using (StreamWriter sw = new StreamWriter(PathLogSLINADE + "ADESendService.log", true, Encoding.UTF8))
                {
                    sw.WriteLine(obj);
                }
            }
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
                    //using (StreamWriter sw = new StreamWriter(pathlog + "ADESendService.log", true, Encoding.UTF8))
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
                List<string> listDoc_ToSend = new List<string>();

                DirectoryInfo di = new DirectoryInfo(PathInput);

                listDoclog.Add(" ");
                listDoclog.Add("[" + DateTime.Now + "] ------------------------------INICIO---------------------------------");
                foreach (var fi in di.GetFiles())
                {
                    listDoclog.Add("[" + DateTime.Now + "] Archivo Encontrado: " + fi.FullName);


                    listaDetected.Add(fi.FullName);
                    string[] array = fi.FullName.Split('\\');
                    int index = array.Length;

                    if (array[index - 1].Contains(".xml"))
                    {
                        listDoc_ToSend.Add(array[index - 1].Replace(".xml", ""));
                    }
                }
                listDoclog.Add("[" + DateTime.Now + "] --------------------------------FIN----------------------------------");


                foreach (var obj in listDoc_ToSend)
                {
                    //send document

                    string path_exe = Return_PathExe(obj);

                    //if (System.IO.File.Exists(PathExe_ProcessSend))
                    if (System.IO.File.Exists(path_exe))
                    {
                        System.Diagnostics.Process appy = new System.Diagnostics.Process();
                        //appy.StartInfo.FileName = PathExe_ProcessSend; //path y name del ejecutable de auxiliares
                        appy.StartInfo.FileName = path_exe; //path y name del ejecutable de auxiliares
                        appy.StartInfo.Arguments = " " + obj;
                        appy.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        appy.StartInfo.UseShellExecute = false;
                        //p.StartInfo.RedirectStandardOutput = true;
                        appy.Start();
                        appy.WaitForExit();
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
    }
}
