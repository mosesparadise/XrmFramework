using System;
using Microsoft.Xrm.Sdk;

namespace Mosesparadise.XrmFramework.Plugin
{
    // Specialized ITracingService implementation that prefixes all traced messages with a time delta for Plugin performance diagnostics
    public class LocalTracingService : ITracingService
    {
        private readonly ITracingService _tracingService;

        private DateTime _previousTraceTime;

        public LocalTracingService(IServiceProvider serviceProvider)
        {
            DateTime utcNow = DateTime.UtcNow;

            var context = (IExecutionContext) serviceProvider.GetService(typeof(IExecutionContext));

            DateTime initialTimestamp = context.OperationCreatedOn;

            if (initialTimestamp > utcNow)
            {
                initialTimestamp = utcNow;
            }

            _tracingService = (ITracingService) serviceProvider.GetService(typeof(ITracingService));

            _previousTraceTime = initialTimestamp;
        }

        public void Trace(string message, params object[] args)
        {
            var utcNow = DateTime.UtcNow;

            // The duration since the last trace.
            var deltaMilliseconds = utcNow.Subtract(_previousTraceTime).TotalMilliseconds;
            var duration = $"[+{deltaMilliseconds:N0}ms)]";
            //var lol= string.Format("{0} - {1}", duration, message);
            // _tracingService.Trace($"[+{deltaMilliseconds:N0}ms)] - {message}", args);
            _tracingService.Trace(duration + " - " + message, args);

            _previousTraceTime = utcNow;
        }
    }
}