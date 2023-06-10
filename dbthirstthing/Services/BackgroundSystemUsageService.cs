using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using System.Web;

namespace dbthirstthing.Services
{
    public class BackgroundSystemUsageService : ServiceBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private const string RegistryKey = @"HKEY_CURRENT_USER\Software\SystemLoadMonitor";
        private const int Threshold = 80;

        private readonly PerformanceCounter _cpuLoad;
        private readonly Timer _timer;

        public BackgroundSystemUsageService()
        {
   

            _cpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _timer = new Timer(interval: 5000);
            _timer.Elapsed += Timer_Elapsed;
         
        }

        protected override void OnStart(string[] args)
        {
            _logger.Info("System load monitor service started");
            _timer.Start();
        }

        protected override void OnStop()
        {
            _logger.Info("System load monitor service stopped");
            _timer.Stop();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var cpuLoad = Convert.ToInt32(_cpuLoad.NextValue());

            if (cpuLoad >= Threshold)
            {
                _logger.Warn($"CPU load is too high: {cpuLoad}%");

                var message = $"System load is too high: {cpuLoad}%";

                Registry.SetValue(keyName: RegistryKey, valueName: "Message", value: message);
            }
        }

    }
}