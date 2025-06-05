using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;

namespace PartnerControlAPI.Logging
{
    public class CustomFileSink : ILogEventSink
    {
        private readonly string _logDirectory;
        private readonly string _prefix;
        private readonly ITextFormatter _formatter;
        private readonly object _lock = new object();

        public CustomFileSink(string logDirectory, string prefix, string outputTemplate)
        {
            _logDirectory = logDirectory;
            _prefix = prefix;
            _formatter = new MessageTemplateTextFormatter(outputTemplate);
        }

        public void Emit(LogEvent logEvent)
        {
            var fileName = Path.Combine(_logDirectory, $"{_prefix}{logEvent.Timestamp:yyyyMMdd}.log");

            lock (_lock)
            {
                Directory.CreateDirectory(_logDirectory);
                using var writer = File.AppendText(fileName);
                _formatter.Format(logEvent, writer);
                writer.WriteLine();
            }
        }
    }

    public static class CustomFileSinkExtensions
    {
        public static LoggerConfiguration CustomFile(
            this LoggerSinkConfiguration loggerConfiguration,
            string logDirectory,
            string prefix,
            string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
        {
            return loggerConfiguration.Sink(new CustomFileSink(logDirectory, prefix, outputTemplate));
        }
    }
} 