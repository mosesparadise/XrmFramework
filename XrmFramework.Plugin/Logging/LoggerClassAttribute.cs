using System;

// ReSharper disable once CheckNamespace
namespace Mosesparadise.XrmFramework.Logging
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class LoggerClassAttribute : Attribute
    {
        public Type LoggerClassType { get; }

        public LoggerClassAttribute(Type loggerClassType)
        {
            LoggerClassType = loggerClassType;
        }
    }
}