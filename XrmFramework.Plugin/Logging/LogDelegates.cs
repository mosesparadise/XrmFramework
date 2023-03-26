// ReSharper disable once CheckNamespace
namespace Mosesparadise.XrmFramework.Logging
{
    public delegate void LogServiceMethod(string methodName, string message, params object[] args);

    public delegate void LogMethod(string message, params object[] args);
}