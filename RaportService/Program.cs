using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace RaportService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                var parametr = string.Concat(args);
                switch (parametr)//jeśli do klasy Main przekażemy parametr (z konsoli cmd)
                {
                    case "--install": //--install to usługa zostanie zainstalowana
                        ManagedInstallerClass.InstallHelper(new[]
                        {Assembly.GetExecutingAssembly().Location});
                        break;
                    case "--uinstall"://a jesli --uinstall to usługa zostanie odinstalowana
                        ManagedInstallerClass.InstallHelper(new[]
                        {"/u", Assembly.GetExecutingAssembly().Location});
                        break;
                    default:
                        break;
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new ReportService()
                };
                ServiceBase.Run(ServicesToRun);
            }

            
        }
    }
}
