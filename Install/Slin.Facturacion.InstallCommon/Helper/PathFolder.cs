using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.InstallCommon
{
    public class PathFolder
    {
        //public const string slinade = @"\SLIN-ADE\";

        //EntradaCE
        public const string entradaCE = @"\EntradaCE\";
        public const string ent_InterfERRO = @"\EntradaCE\InterfERRO\";
        public const string ent_InterfPROC = @"\EntradaCE\InterfPROC\";
        public const string ent_InterfSUC = @"\EntradaCE\InterfSUC\";
        public const string ent_InterfTXT = @"\EntradaCE\InterfTXT\";
        public const string ent_InterfXML = @"\EntradaCE\InterfXML\";

        //Librerias
        public const string libreriascrt = @"\Librerias\crt\"; //COPY FILE


        //ProcesoCE
        public const string procesoCE = @"\ProcesoCE\";
        public const string proc_cdr = @"\ProcesoCE\CDR\";
        public const string proc_env = @"\ProcesoCE\ENVIO\";
        public const string proc_ord = @"\ProcesoCE\ORD\";
        public const string proc_pdf = @"\ProcesoCE\PDF\";
        public const string proc_pdf417 = @"\ProcesoCE\PDF417\";
        public const string proc_xml = @"\ProcesoCE\XML\";


        //Procesos
        public const string procesos = @"\Procesos\";
        public const string procs_bin = @"\Procesos\bin\"; //COPY FILE
        public const string procs_env = @"\Procesos\env\"; //COPY FILE

        //proceso int smc
        public const string procs_smc = @"\Procesos\smc\";
            public const string procs_smc_bin = @"\Procesos\smc\bin\"; //COPY FILE
            public const string procs_smc_cds = @"\Procesos\smc\cds\"; //COPY FILE
            public const string procs_smc_html = @"\Procesos\smc\html\"; //COPY FILE
            public const string procs_smc_report = @"\Procesos\smc\Report\Reporte\"; //COPY FILE


        //proceso int smd
        public const string procs_smd = @"\Procesos\smd\";
            public const string procs_smd_bin = @"\Procesos\smd\bin\"; //COPY FILE


        //proceso int smi
        public const string procs_smi = @"\Procesos\smi\";
            public const string procs_smi_bin = @"\Procesos\smi\bin\"; //COPY FILE


        //proceso int smp
        public const string procs_smp = @"\Procesos\smp\";
            public const string procs_smp_bin = @"\Procesos\smp\bin\"; //COPY FILE
            public const string procs_smp_prt = @"\Procesos\smp\prt\";
            public const string procs_smp_report = @"\Procesos\smp\Report\Reporte\"; //COPY FILE

        //proceso int exchange rate
        public const string procs_ser = @"Procesos\ser\";

        //proceso
        public const string procs_svc = @"\Procesos\svc\"; 


        //receivedCE
        public const string receivedCE = @"\RecievedCE\";
        public const string rec_input_other = @"\RecievedCE\input_other\";
        public const string rec_input_pdf = @"\RecievedCE\input_pdf\";
        public const string rec_input_xml = @"\RecievedCE\input_xml\";
        public const string rec_ins = @"\RecievedCE\ins\";
        public const string rec_nins = @"\RecievedCE\nins\";


        //configuration
        public const string configuration = @"\Configuracion\"; //COPY FILE

        //_ADE
        public const string _ade = @"\_ADE\";
        public const string _ade_env = @"\_ADE\env\"; //COPY FILE
        public const string _ade_in = @"\_ADE\in\";
        public const string _ade_log = @"\_ADE\log\";
        public const string _ade_res = @"\_ADE\res\"; //COPY FILE
        public const string _ade_svc = @"\_ADE\svc\"; //COPY FILE


        


        //logs
        public const string logs = @"\Logs\";



        //path slin ade

        //path service ade

        //path portal ade

        //path service portal 

        //path web service interface

        //path web service consult

        //public const string logsmc = @"\Logs" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smc\";
        //public const string logsmd = @"\Logs" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smd\";
        //public const string logsmi = @"\Logs" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smi\";
        //public const string logsmp = @"\Logs" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smp\";
        //public const string logsmp = @"\Logs" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\logade\";





        public const string smc = "smc";
        public const string smd = "smd";
        public const string smi = "smi";
        public const string smp = "smp";
        public const string env = "env";
        public const string svc = "svc";
        public const string res = "res";
        public const string ser = "ser";

        public const string slinade = "slinade";
        public const string serviceade = "serviceade";
        public const string serviceinterface = "serviceinterface";
        public const string serviceconsult = "serviceconsult";
        public const string serviceresumen = "serviceresumen";
        
    }
}
