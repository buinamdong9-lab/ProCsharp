using Serilog;
using Serilog.Core;
using System;

namespace FrmProject.DataStructures
{
    public static class AppLogger
    {
        private static readonly object _syncRoot = new();
        private static Logger? _logger;

        public static void Initialize()
        {
            EnsureInitialized().Information("Application started.");
        }

        public static void CloseAndFlush()
        {
            lock (_syncRoot)
            {
                if (_logger == null)
                    return;

                _logger.Information("Application shutting down.");
                Log.CloseAndFlush();
                _logger = null;
            }
        }

        public static void Info(string message)
        {
            EnsureInitialized().Information(message);
        }

        public static void Error(string message, Exception? ex = null)
        {
            if (ex != null)
                EnsureInitialized().Error(ex, message);
            else
                EnsureInitialized().Error(message);
        }

        public static void Warn(string message)
        {
            EnsureInitialized().Warning(message);
        }

        public static void Debug(string message)
        {
            EnsureInitialized().Debug(message);
        }

        private static Logger EnsureInitialized()
        {
            if (_logger != null)
                return _logger;

            lock (_syncRoot)
            {
                if (_logger != null)
                    return _logger;

                _logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day,
                                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                    .CreateLogger();

                Log.Logger = _logger;
                return _logger;
            }
        }
    }
}

