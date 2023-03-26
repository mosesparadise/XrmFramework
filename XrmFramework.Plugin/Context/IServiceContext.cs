using System;
using Microsoft.Xrm.Sdk;
using Mosesparadise.XrmFramework.Logging;

namespace Mosesparadise.XrmFramework.Plugin.Context
{
    public interface IServiceContext : ILoggerContext
    {
        IOrganizationServiceFactory Factory { get; }

        // The PowerApps CDS organization service for system user account
        IOrganizationService SystemUserService { get; }

        // The PowerApps CDS organization service for current user account
        IOrganizationService CurrentUserService { get; }

        // Provides logging run time trace information for plug-ins. 
        ITracingService TracingService { get; }

        IOrganizationService GetService(Guid userId);

        LogServiceMethod LogServiceMethod { get; }
    }
}