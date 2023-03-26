using System;
using Mosesparadise.XrmFramework.Logging;

namespace Mosesparadise.XrmFramework.Plugin.Context
{
    public interface ILoggerContext
    {
        Guid UserId { get; }

        Guid InitiatingUserId { get; }

        Guid CorrelationId { get; }

        string OrganizationName { get; }

        ILogger Logger { get; }

        void Log(string message, params object[] formatArgs);

        void LogError(Exception e, string message = null, params object[] args);

        /// <summary>
        /// Writes a trace message to the CRM trace log.
        /// </summary>
        /// <param name="message">Message name to trace.</param>
        void Trace(string message);
    }
}