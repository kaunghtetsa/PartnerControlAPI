using Serilog.Core;
using Serilog.Events;

namespace PartnerControlAPI.Logging
{
    public class RequestResponseEnricher : ILogEventEnricher
    {
        private readonly string _propertyName;
        private readonly object _value;

        public RequestResponseEnricher(string propertyName, object value)
        {
            _propertyName = propertyName;
            _value = value;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var property = propertyFactory.CreateProperty(_propertyName, _value);
            logEvent.AddPropertyIfAbsent(property);
        }
    }
} 