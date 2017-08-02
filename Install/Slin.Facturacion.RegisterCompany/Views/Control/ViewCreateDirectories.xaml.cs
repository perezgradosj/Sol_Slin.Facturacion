using Slin.Facturacion.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Microsoft.VisualBasic.Devices;
using System.Threading;
using System.IO;
using System.Configuration;
using Slin.Facturacion.InstallCommon;

namespace Slin.Facturacion.RegisterCompany.Views.Control
{
    /// <summary>
    /// Interaction logic for ViewCreateDirectories.xaml
    /// </summary>
    public partial class ViewCreateDirectories : Window
    {
        private ConectionClass obj_DB = new ConectionClass();
        private Company oCompany = new Company();

        private string Val_Port = string.Empty;

        private ListSevices list_APP_service = new ListSevices();
        private ListSevices list_WS_service = new ListSevices();

        int Cant_Apps_WS = Constantes.ValorCero;
        int Cant_Apps_WS_Installed = Constantes.ValorCero;

        public ViewCreateDirectories(ListSevices list_WS, ListSevices list_APP, ConectionClass objBD, Company oentity)
        {
            InitializeComponent();

            Receibed_ListService(list_WS, list_APP);

            list_APP_service = list_APP;
            list_WS_service = list_WS;

            obj_DB = objBD;
            oCompany = oentity;

            Cant_Apps_WS = list_WS.Count + list_APP.Count;
            lbl_installed.Content = Constantes.ValorCero + "/" + Cant_Apps_WS;
        }


        #region Entity

        Computer mypc = new Computer();

        #endregion



        #region method
        public List<string> list_files_ws = new List<string>();
        public List<string> list_files_app = new List<string>();

        private void Receibed_ListService(ListSevices list_ws, ListSevices list_app)
        {
            list_files_ws = new List<string>();
            list_files_app = new List<string>();
            try
            {
                string pathFile_WS = string.Empty;
                pathFile_WS = System.Reflection.Assembly.GetExecutingAssembly().Location;
                if (list_ws.Count > 0)
                {
                    foreach (var obj in list_ws)
                    {
                        string ret_ws = @"..\..\..\Files\WS\";
                        ret_ws += obj;
                        pathFile_WS = System.IO.Path.GetFullPath(System.IO.Path.Combine(pathFile_WS, ret_ws));
                        list_files_ws.Add(pathFile_WS);
                    }
                }

                string pathFile_APP = string.Empty;
                pathFile_APP = System.Reflection.Assembly.GetExecutingAssembly().Location;
                if (list_app.Count > 0)
                {
                    foreach (var obj in list_app)
                    {
                        string ret_app = @"..\..\..\Files\ADE\";
                        ret_app += obj;
                        pathFile_APP = System.IO.Path.GetFullPath(System.IO.Path.Combine(pathFile_APP, ret_app));
                        list_files_app.Add(pathFile_APP);
                    }
                }
            }
            catch (Exception ex) { }
        }

        #endregion


        public string cadena_slinade = string.Empty;
        public string HARD_DESK = string.Empty;
        private void btnSelectHarkDesk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtacronimo.Text.Length == Constantes.ValorCero)
                {
                    MessageBox.Show("Ingrese un Acronimo para su Empresa", "Info");
                    return;
                }
                else
                {
                    System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
                    if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        HARD_DESK = folderDialog.SelectedPath;
                        cadena_slinade = folderDialog.SelectedPath + @"SLIN-ADE";
                        txtpathRoot.Text = folderDialog.SelectedPath + @"SLIN-ADE\" + txtacronimo.Text.Trim().ToUpper();
                    }
                }
                Enable_Buttons();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void Enable_Buttons()
        {
            if (txtacronimo.Text.Length > Constantes.ValorCero && txtpathRoot.Text.Length > Constantes.ValorTres)
            {
                btnmovefiles.IsEnabled = true;
                //btnSelectHarkDesk_ade.IsEnabled = true;
            }
            else
            {
                btnmovefiles.IsEnabled = false;
                //btnSelectHarkDesk_ade.IsEnabled = false;
            }
        }







        string path_certificado_my_files = string.Empty;
        private void btn_certdigital_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog file = new Microsoft.Win32.OpenFileDialog();
                file.ShowDialog();

                if (file.FileName.Length > Constantes.ValorTres)
                {
                    string[] array = file.FileName.Split('\\');
                    int index = array.Length;
                    string nameFile = array[index - 1];
                    if (System.IO.File.Exists(file.FileName))
                    {
                        string path_Folder_Copy = string.Empty;
                        path_Folder_Copy = System.Reflection.Assembly.GetExecutingAssembly().Location;
                        string ret = @"..\..\..\Files\SLIN-ADE\REPLACE_ASYNC\Librerias\crt\";
                        path_Folder_Copy = System.IO.Path.GetFullPath(System.IO.Path.Combine(path_Folder_Copy, ret));
                        txtpath_certdigital.Text = file.FileName;

                        if (System.IO.File.Exists(path_Folder_Copy + nameFile))
                            System.IO.File.Delete(path_Folder_Copy + nameFile);

                        path_certificado_my_files = path_Folder_Copy + nameFile;
                        mypc.FileSystem.CopyFile(file.FileName, path_Folder_Copy + nameFile);
                    }
                }
            }

            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }





        private void btnmovefiles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (list_WS_service.Count > Constantes.ValorUno)
                {
                    if (System.IO.File.Exists(path_certificado_my_files))
                    {
                        Copy_File_WS();
                        progressBar1.Value = 0;
                        lblProgress.Content = string.Empty;
                        Value = Constantes.ValorCero;
                        StartProgress(100, "Archivos movidos correctamente!", "Moviendo archivos de los Servicios");

                        if (System.IO.Directory.Exists(txtpathRoot.Text))
                        {
                            //btnInstallarServicesWS.IsEnabled = true;
                            btnSelectHarkDesk_ade.IsEnabled = true;
                        }
                        else { /**btnInstallarServicesWS.IsEnabled = false;**/ }
                    }
                    else
                    {
                        MessageBox.Show("Seleccione la ruta de ubicación del certificado digital!", "Info");
                    }
                }
                else { /**btnInstallarServicesWS.IsEnabled = false;**/ btnSelectHarkDesk_ade.IsEnabled = true; }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }





        #region ws
        List<string> list_bat_install = new List<string>();

        private void Copy_File_WS()
        {
            try
            {
                string path_ADE = string.Empty;
                path_ADE = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string ret = @"..\..\..\Files\SLIN-ADE\";
                path_ADE = System.IO.Path.GetFullPath(System.IO.Path.Combine(path_ADE, ret));
                //HARD_DESK += @"SLIN-ADE";

                mypc.FileSystem.CopyDirectory(path_ADE, HARD_DESK + "SLIN-ADE");
                mypc.FileSystem.RenameDirectory(HARD_DESK + "SLIN-ADE" + @"\REPLACE_ASYNC", txtacronimo.Text.ToUpper());

                Read_Config_WS();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }

        }

        private void Read_Config_WS()
        {
            list_bat_install = new List<string>();

            try
            {
                if (list_WS_service.Count > 0)
                {

                    foreach (var obj in list_WS_service)
                    {
                        string cad = @obj.Path_Exe;
                        string[] array = obj.Path_Exe.Split('\\');
                        int index = array.Length;

                        string delimit = "\\";
                        string path_bat = string.Empty;
                        for (int i = 0; i <= array.Length - 2; i++)
                        {
                            path_bat += array[i] + delimit;
                        }

                        ReWrite_Config_WS(HARD_DESK.Substring(0, 2) + obj.Path_Exe + ".config", obj.Name, HARD_DESK.Substring(0, 2) + path_bat, obj.Description_Res, obj.Path_Exe);

                        //reescribe el config del ejecutable
                    }
                }
                ReWrite_Otherfiles();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private List<string> Create_ListFilesToRead()
        {
            List<string> list_OtherFiles = new List<string>();
            list_OtherFiles.Add(txtpathRoot.Text + @"\Configuracion\MainConfig.xml");

            list_OtherFiles.Add(txtpathRoot.Text + @"\Procesos\smc\bin\Slin.Facturacion.ProcessSend.exe.config");
            list_OtherFiles.Add(txtpathRoot.Text + @"\Procesos\smp\bin\Slin.Facturacion.ProcessPrint.exe.config");

            return list_OtherFiles;
        }

        private void ReWrite_Otherfiles()
        {
            try
            {
                var list = Create_ListFilesToRead();

                string ReadText_File = string.Empty;
                if (list.Count > 0)
                {
                    for (int i = 0; i <= list.Count - 1; i++)
                    {
                        ReadText_File = string.Empty;

                        if (System.IO.File.Exists(list[i]))
                        {
                            ReadText_File = System.IO.File.ReadAllText(list[i]);

                            ReadText_File = ReadText_File.Replace("HOST_ARS", obj_DB.Host);
                            ReadText_File = ReadText_File.Replace("DB_ARS", obj_DB.DBName);
                            ReadText_File = ReadText_File.Replace("USER_ARS", obj_DB.UserDB);
                            ReadText_File = ReadText_File.Replace("PWD_ARS", obj_DB.PwdDB);
                            ReadText_File = ReadText_File.Replace("US_CERT", oCompany.Us_Cert);
                            ReadText_File = ReadText_File.Replace("PWD_CERT", oCompany.Pwd_Cert);
                            ReadText_File = ReadText_File.Replace("HARDDESK", HARD_DESK.Substring(0, 2));
                            ReadText_File = ReadText_File.Replace("ACRONIMO", txtacronimo.Text.ToUpper());

                            if (System.IO.File.Exists(list[i]))
                                System.IO.File.Delete(list[i]);

                            using (StreamWriter sw = new StreamWriter(list[i], true, Encoding.UTF8))
                            { sw.WriteLine(ReadText_File); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ReWrite_Config_WS(string path_config, string name_ws, string path_bat, string description, string path_exe)
        {
            try
            {
                path_config = path_config.Replace("NEWACRON", txtacronimo.Text.ToUpper());
                //path_bat = path_bat.Replace("ACRONIMO", txtacronimo.Text.ToUpper());

                if (System.IO.File.Exists(path_config))
                {
                    string read_Config_ws = System.IO.File.ReadAllText(path_config);

                    read_Config_ws = read_Config_ws.Replace("HARDDESK", HARD_DESK.Substring(0, 2));
                    read_Config_ws = read_Config_ws.Replace("NEWACRON", txtacronimo.Text.ToUpper());
                    //read_Config_ws = read_Config_ws.Replace("HOST_ARS", obj_DB.Host);
                    //read_Config_ws = read_Config_ws.Replace("DB_ARS", obj_DB.DBName);
                    //read_Config_ws = read_Config_ws.Replace("USER_ARS", obj_DB.UserDB);
                    //read_Config_ws = read_Config_ws.Replace("PWD_ARS", obj_DB.PwdDB);
                    read_Config_ws = read_Config_ws.Replace("NEWRUC", oCompany.Ruc);

                    if (System.IO.File.Exists(path_config))
                        System.IO.File.Delete(path_config);

                    using (StreamWriter sw = new StreamWriter(path_config, true, Encoding.UTF8))
                    { sw.WriteLine(read_Config_ws); }
                }

                //list_bat_install.Add(path_bat + @"Installer_" + name_ws + ".bat");
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        #endregion




        

        
        

        #region app

        //aqui segun la lista de apps a instalar debo reescribir en el config de las web services de consulta e interface
        ListWeb_App list_web_app = new ListWeb_App();

        private void CreatePath_SLINADE_APP()
        {
            CreateObject_Path_APP(txtpathRoot_app.Text, list_APP_service);
        }
        private void CreateObject_Path_APP(string path_slinade_async_app, ListSevices list_app_rec)
        {
            try
            {
                list_web_app = new ListWeb_App();
                if (list_app_rec.Count > 0)
                {
                    for (int i = 0; i <= list_app_rec.Count - 1; i++)
                    {
                        Web_App objweb = new Web_App();

                        objweb.Name = list_app_rec[i].Name;
                        //objweb.Port = ConfigurationManager.AppSettings[list_app_rec[i].Name].ToString();

                        //objweb.Protocolo = ConfigurationManager.AppSettings[list_app_rec[i].Name + "_protocolo"].ToString();
                        objweb.Path = txtpathRoot_app.Text + @"\" + list_app_rec[i].Name;

                        list_web_app.Add(objweb);
                    }
                }
                lstView_service.ItemsSource = list_web_app;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            
        }




        private void Copy_Files_ADE()
        {
            //if (list_web_app.Count > 0)
            //{
            //    for (int i = 0; i <= list_web_app.Count - 1; i++)
            //    {
            //        string path_Folder_Copy = string.Empty;
            //        path_Folder_Copy = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //        string ret = @"..\..\..\Files\ADE\" + list_APP_service[i].Name;
            //        path_Folder_Copy = System.IO.Path.GetFullPath(System.IO.Path.Combine(path_Folder_Copy, ret));
            //        mypc.FileSystem.CopyDirectory(path_Folder_Copy, list_web_app[i].Path);
            //    }
            //}

            Read_Config_APP_ADE();

            //list_app_site = new List<string>();
            //list_app_site = Return_List_CommandToExe();
        }


        private void Read_Config_APP_ADE()
        {
            if (list_web_app.Count > 0)
            {
                for (int i = 0; i <= list_web_app.Count - 1; i++)
                {
                    ReWrite_Config_APP_ADE(list_web_app[i].Path + @"\" + "Web.config", list_web_app[i].Name);
                }
            }
        }


        private void ReWrite_Config_APP_ADE(string path, string NameApp)
        {
            try
            {
                if (System.IO.File.Exists(path))
                {
                    string ReadText_Config = System.IO.File.ReadAllText(path);

                    if (HARD_DESK.Length == 0)
                    {
                        MessageBox.Show("Seleccione Ruta para la Carpeta SLIN-ADE", "Info");
                        return;
                    }
                    else
                    {

                    }


                    ReadText_Config = ReadText_Config.Replace("HARDDESK", HARD_DESK.Substring(0, 2));
                    ReadText_Config = ReadText_Config.Replace("NEWACRON", txtacronimo.Text.ToUpper());
                    //ReadText_Config = ReadText_Config.Replace("HOST_ARS", obj_DB.Host);
                    //ReadText_Config = ReadText_Config.Replace("DB_ARS", obj_DB.DBName);
                    //ReadText_Config = ReadText_Config.Replace("USER_ARS", obj_DB.UserDB);
                    //ReadText_Config = ReadText_Config.Replace("PWD_ARS", obj_DB.PwdDB);
                    ReadText_Config = ReadText_Config.Replace("NEWRUC", oCompany.Ruc);

                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    using (StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8))
                    { sw.WriteLine(ReadText_Config); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        

        #endregion


        private void btn_movefiles_ade_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Copy_Files_ADE();
                progressBar1.Value = 0;
                lblProgress.Content = string.Empty;
                Value = Constantes.ValorCero;
                StartProgress(100, "Sitio movidos correctamente!", "Moviendo archivos de los sitios");

                if (System.IO.Directory.Exists(txtpathRoot_app.Text + @"\slinade"))
                {
                    //btn_InstalarWEBAPPS.IsEnabled = true;
                }
                else { /**btn_InstalarWEBAPPS.IsEnabled = false;**/ }

                btn_End.IsEnabled = true;
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void btnSelectHarkDesk_ade_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
                if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtpathRoot_app.Text = folderDialog.SelectedPath;
                }
                CreatePath_SLINADE_APP();

                if (lstView_service.Items.Count > Constantes.ValorCero)
                {
                    btn_movefiles_ade.IsEnabled = true;
                }
                else { btn_movefiles_ade.IsEnabled = false; }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void btn_End_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Gracias por instalar el Sistema de Facturación Electrónica SLIN-ADE");
            App.Current.Shutdown();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {

        }


        #region progressbar


        string msjeProgressOk = string.Empty;
        void StartProgress(int speed, string msjeShow, string msjeCreating)
        {
            Value = 0;
            CountingProgressBar(100, speed, msjeShow, msjeCreating);
            //LayoutRoot.Children.Add(cpb);
            //ProgressCompletedEvent += () => { MessageBox.Show("Count Complete"); };
            Start();
        }

        int Maximum, Value;
        Thread timerThread;
        public void CountingProgressBar(int Time, int speed, string msjeShow, string msjeCreating)
        {
            Maximum = Time;
            SetTimer(Time, speed, msjeShow, msjeCreating);
        }

        void SetTimer(int time, int speed, string msjeShow, string msjeCreating)
        {
            if (timerThread != null && timerThread.IsAlive)
                timerThread.Abort();

            timerThread = new Thread(() =>
            {
                int cont = 0;
                for (int i = 0; i <= time; ++i)
                {
                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
                    {
                        //lblProgress.ContentStringFormat = "Loading... " + Value;
                        lblProgress.Content = Convert.ToString(msjeCreating + "... " + Value + "%");
                        Value += 1;
                        progressBar1.Value = progressBar1.Value + 1;


                        if (progressBar1.Value == 100)
                        {
                            cont++;
                            if (cont == 2)
                            {
                                MessageBox.Show(msjeShow, "Info");
                            }
                        }

                    }));
                    Thread.Sleep(speed);
                }
                ProgressComplete();
            });
            timerThread.IsBackground = true;
        }

        public void Start()
        {
            if (timerThread != null)
                timerThread.Start();
        }
        public void SetTime(int Time, int speed, string msjeShow, string msjeCreating)
        {
            Maximum = Time;
            SetTimer(Time, speed, msjeShow, msjeCreating);
        }
        public void Reset(int speed, string msjeShow, string msjeCreating)
        {
            Value = 0;
            SetTimer(Convert.ToInt32(Maximum), speed, msjeShow, msjeCreating);
        }

        public delegate void ProgressCompleted();
        public event ProgressCompleted ProgressCompletedEvent;

        

        private void ProgressComplete()
        {
            if (ProgressCompletedEvent != null)
                ProgressCompletedEvent();

        }

        #endregion
    }
}
