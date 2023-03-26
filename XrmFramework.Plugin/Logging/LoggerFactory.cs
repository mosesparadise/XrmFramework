using Mosesparadise.XrmFramework.Plugin.Context;

// ReSharper disable once CheckNamespace
namespace Mosesparadise.XrmFramework.Logging
{
    public static class LoggerFactory
    {
        public static ILogger GetLogger(IServiceContext context, LogMethod logMethod)
        {
            return new XrmLogger(context.SystemUserService, context, logMethod);
            // var loggerAttribute = typeof(LoggerFactory).Assembly.GetCustomAttribute<LoggerClassAttribute>();
            //
            // var loggerType = typeof(XrmLogger);
            //
            // if (loggerAttribute != null)
            // {
            //     loggerType = loggerAttribute.LoggerClassType;
            // }
            //
            // var logger = (ILogger) Activator.CreateInstance(loggerType, context.SystemUserService, context, logMethod);
            //
            // return logger;
        }
    }
}