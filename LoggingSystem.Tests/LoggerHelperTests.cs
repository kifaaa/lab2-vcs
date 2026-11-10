using System.Collections.Generic;
using Xunit;

namespace LoggingSystem.Tests
{
    public class LoggerHelperTests
    {
        private class MemoryLogger : ILogger
        {
            public List<string> Entries { get; } = new List<string>();

            public void Log(string level, string message)
            {
                Entries.Add($"{level}:{message}");
            }
        }

        [Fact]
        public void Format_ContainsLevelAndMessage()
        {
            string result = LoggerHelper.Format("info", "hello");
            Assert.Contains("[INFO]", result);
            Assert.Contains("hello", result);
        }

        [Fact]
        public void WriteInfo_CallsLoggerWithInfoLevel()
        {
            var logger = new MemoryLogger();
            LoggerHelper.WriteInfo(logger, "started");
            Assert.Single(logger.Entries);
            Assert.Equal("INFO:started", logger.Entries[0]);
        }

        [Fact]
        public void Application_LogEvent_NotifiesAllLoggers()
        {
            var first = new MemoryLogger();
            var second = new MemoryLogger();
            var app = new Application(new List<ILogger> { first, second });

            app.LogEvent("ERROR", "fail");

            Assert.Equal("ERROR:fail", first.Entries[0]);
            Assert.Equal("ERROR:fail", second.Entries[0]);
        }
    }
}
