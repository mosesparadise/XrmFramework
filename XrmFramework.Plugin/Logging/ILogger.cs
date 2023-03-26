using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Mosesparadise.XrmFramework.Logging
{
    public interface ILogger
    {
        void LogWithMethodName(string methodName, string message, params object[] formatArgs);

        void Log(string message, params object[] args);

        void LogError(Exception e, string message = null, params object[] args);

        void LogCollection(IEnumerable<KeyValuePair<string, object>> collection, bool verifyIncluded = false, params string[] excludedIncludedKeys);

        void DumpLog();
    }
}