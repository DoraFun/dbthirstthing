using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Web;
using System.Web.Mvc;

namespace dbthirstthing.Controllers
{
    public class HealthCheckController : Controller
    {
        // GET: HealthCheck
        public ActionResult Index()
        {
            // Get the name of the server where the application is running
            var serverName = Environment.MachineName;

            // Get the processor name and speed
            var processorName = string.Empty;
            var processorSpeed = string.Empty;
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            foreach (var obj in searcher.Get())
            {
                processorName = obj["Name"].ToString();
                processorSpeed = obj["MaxClockSpeed"].ToString() + " MHz";
                break;
            }

            // Get the total RAM installed on the system
            var totalRam = string.Empty;
            searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            foreach (var obj in searcher.Get())
            {
                var ram = Convert.ToDouble(obj["TotalPhysicalMemory"]);
                totalRam = Math.Round(ram / (1024 * 1024 * 1024), 2).ToString() + " GB";
                break;
            }

            // Get the free space on the main disk
            var drive = new DriveInfo("C");
            var freeSpace = Math.Round((double)drive.AvailableFreeSpace / (1024 * 1024 * 1024), 2).ToString() + " GB";

            // Return the healthcheck results
            var result = new
            {
                ServerName = serverName,
                ProcessorName = processorName,
                ProcessorSpeed = processorSpeed,
                TotalRAM = totalRam,
                FreeSpace = freeSpace
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}