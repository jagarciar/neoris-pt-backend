using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Http;
using Serilog;
using NeorisBackend.App_Start;

namespace NeorisBackend
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ConfigureLogging();
            
            // Configurar inyección de dependencias
            UnityConfig.RegisterComponents();
            
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_End()
        {
            Log.CloseAndFlush();
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            if (exception != null)
            {
                Log.Error(exception, "Unhandled exception captured by Application_Error");
            }
        }

        private static void ConfigureLogging()
        {
            var logFilePathSetting = ConfigurationManager.AppSettings["LogFilePath"];
            var logLevelSetting = ConfigurationManager.AppSettings["LogLevelMin"];
            var logFileSizeLimitSetting = ConfigurationManager.AppSettings["LogFileSizeLimitBytes"];

            var logPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                logFilePathSetting ?? "logs\\neoris-backend-.log");

            var logDir = Path.GetDirectoryName(logPath);
            if (!string.IsNullOrWhiteSpace(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            var level = Serilog.Events.LogEventLevel.Information;
            if (!string.IsNullOrWhiteSpace(logLevelSetting) && Enum.TryParse(logLevelSetting, true, out Serilog.Events.LogEventLevel parsed))
            {
                level = parsed;
            }

            var fileSizeLimitBytes = 10 * 1024 * 1024;
            if (!string.IsNullOrWhiteSpace(logFileSizeLimitSetting) && int.TryParse(logFileSizeLimitSetting, out var parsedSize))
            {
                fileSizeLimitBytes = parsedSize;
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(level)
                .Enrich.FromLogContext()
                .WriteTo.File(
                    logPath,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 14,
                    fileSizeLimitBytes: fileSizeLimitBytes,
                    rollOnFileSizeLimit: true,
                    shared: true)
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
