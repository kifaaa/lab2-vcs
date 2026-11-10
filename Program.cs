using System;
using System.Collections.Generic;
using System.IO;

namespace LoggingSystem
{
    public interface ILogger
    {
        void Log(string level, string message);
    }

    public class FileLogger : ILogger
    {
        private readonly string _filePath;

        public FileLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Log(string level, string message)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
            File.AppendAllText(_filePath, logEntry + Environment.NewLine);
            Console.WriteLine($"[FILE] Записано в {_filePath}: {logEntry}");
        }
    }

    public class DatabaseLogger : ILogger
    {
        private readonly string _connectionString;

        public DatabaseLogger(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Log(string level, string message)
        {
            Console.WriteLine($"[DATABASE] (Connection: {_connectionString}) [{level}] {message}");
        }
    }

    public class RemoteLogger : ILogger
    {
        private readonly string _apiUrl;

        public RemoteLogger(string apiUrl)
        {
            _apiUrl = apiUrl;
        }

        public void Log(string level, string message)
        {
            Console.WriteLine($"[REMOTE] (API: {_apiUrl}) [{level}] {message}");
        }
    }

    public class Application
    {
        private readonly List<ILogger> _loggers;

        public Application(List<ILogger> loggers)
        {
            _loggers = loggers;
        }

        public void LogEvent(string level, string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Log(level, message);
            }
        }

        public void Run()
        {
            LogEvent("INFO", "Приложение запущено");
            LogEvent("WARNING", "Это предупреждение");
            LogEvent("ERROR", "Произошла ошибка");
            LogEvent("INFO", "Приложение завершает работу");
        }
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string level, string message)
        {
            ConsoleColor originalColor = Console.ForegroundColor;

            switch (level)
            {
                case "ERROR":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "WARNING":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "INFO":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            Console.WriteLine($"[CONSOLE] [{level}] {message}");
            Console.ForegroundColor = originalColor;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" СИСТЕМА ЛОГИРОВАНИЯ \n");

            var fileLogger = new FileLogger("app.log");
            var dbLogger = new DatabaseLogger("Server=localhost;Database=Logs");
            var remoteLogger = new RemoteLogger("https://api.logging-service.com/logs");

            var loggers = new List<ILogger>
            {
                fileLogger,
                dbLogger,
                remoteLogger
            };

            var app = new Application(loggers);

            Console.WriteLine(" Базовый запуск (3 логгера) \n");
            app.Run();

            Console.WriteLine("\n Расширение системы (добавляем ConsoleLogger) \n");

            var consoleLogger = new ConsoleLogger();
            var extendedLoggers = new List<ILogger>(loggers) { consoleLogger };
            var extendedApp = new Application(extendedLoggers);
            extendedApp.Run();

            Console.WriteLine($"\n Проверка: файл 'app.log' создан в папке проекта   ");
            if (File.Exists("app.log"))
            {
                string content = File.ReadAllText("app.log");
                Console.WriteLine("Содержимое файла:\n" + content);
            }

            if (!Console.IsInputRedirected)
            {
                Console.WriteLine("\nНажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }
    }
}
