using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.WS_FileDown
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                ///new Service1()
                new ServiceMailDown()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
