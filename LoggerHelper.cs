using System;

namespace LoggingSystem
{
    /// <summary>
    /// Вспомогательный модуль логирования (добавлен в ЛР №2).
    /// Форматирует сообщения единым образом для всех логгеров.
    /// </summary>
    public static class LoggerHelper
    {
        public static string Format(string level, string message)
        {
            return $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level.ToUpperInvariant()}] {message}";
        }

        public static void WriteInfo(ILogger logger, string message)
        {
            logger.Log("INFO", message);
        }

        public static void WriteWarning(ILogger logger, string message)
        {
            logger.Log("WARNING", message);
        }

        public static void WriteError(ILogger logger, string message)
        {
            logger.Log("ERROR", message);
        }
    }
}
