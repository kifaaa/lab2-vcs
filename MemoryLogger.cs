using System.Collections.Generic;

namespace LoggingSystem
{
    /// <summary>
    /// Новый модуль: логгер, сохраняющий сообщения в оперативной памяти.
    /// Добавлен в ветке feature/new-module без изменения Application.
    /// </summary>
    public class MemoryLogger : ILogger
    {
        private readonly List<string> _entries = new List<string>();

        public IReadOnlyList<string> Entries => _entries.AsReadOnly();

        public void Log(string level, string message)
        {
            string entry = LoggerHelper.Format(level, message);
            _entries.Add(entry);
            System.Console.WriteLine($"[MEMORY] {entry}");
        }
    }
}
