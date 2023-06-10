using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using NLog;
using System.Diagnostics;

namespace dbthirstthing.Jobs
{
    public class LoadMonitor : IJob
    {
        private readonly ILogger _logger;

        public LoadMonitor()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                var ramCounter = new PerformanceCounter("Memory", "Available MBytes");

                var cpuValue = cpuCounter.NextValue();
                var ramValue = ramCounter.NextValue();

                // Log CPU & RAM values
                _logger.Info($"CPU Usage: {cpuValue:0.00}%, RAM Usage: {ramValue:0.00}MB");

                // Check if CPU usage is too high (>= 90%)
                if (cpuValue >= 90)
                {
                    _logger.Warn("CPU usage is too high!");
                }

                // Check if available RAM is too low (<= 100MB)
                if (ramValue <= 100)
                {
                    _logger.Warn("Available RAM is too low!");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception occurred while monitoring server load.");
            }
            await Task.CompletedTask;
        }
    }
    }
