using System;
using Microsoft.Xrm.Sdk;
using Mosesparadise.XrmFramework.Plugin.Context;

namespace Mosesparadise.XrmFramework.Plugin
{
    /// <summary>
    /// Plug-in context object. 
    /// </summary>
    // public class LocalPluginContext : ILocalPluginContext
    // {
    //     [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "LocalPluginContext")]
    //     internal IServiceProvider ServiceProvider { get; private set; }
    //
    //     /// <summary>
    //     /// The PowerApps CDS organization service for current user account.
    //     /// </summary>
    //     [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "LocalPluginContext")]
    //     public IOrganizationService CurrentUserService { get; private set; }
    //
    //     /// <summary>
    //     /// The PowerApps CDS organization service for system user account.
    //     /// </summary>
    //     [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "LocalPluginContext")]
    //     public IOrganizationService SystemUserService { get; private set; }
    //
    //     /// <summary>
    //     /// IPluginExecutionContext contains information that describes the run-time environment in which the plug-in executes, information related to the execution pipeline, and entity business information.
    //     /// </summary>
    //     public IPluginExecutionContext PluginExecutionContext { get; private set; }
    //
    //     /// <summary>
    //     /// Synchronous registered plug-ins can post the execution context to the Microsoft Azure Service Bus. <br/> 
    //     /// It is through this notification service that synchronous plug-ins can send brokered messages to the Microsoft Azure Service Bus.
    //     /// </summary>
    //     public IServiceEndpointNotificationService NotificationService { get; private set; }
    //
    //     /// <summary>
    //     /// Provides logging run-time trace information for plug-ins. 
    //     /// </summary>
    //     public ITracingService TracingService { get; private set; }
    //
    //     /// <summary>
    //     /// Helper object that stores the services available in this plug-in.
    //     /// </summary>
    //     /// <param name="serviceProvider"></param>
    //     public LocalPluginContext(IServiceProvider serviceProvider)
    //     {
    //         ServiceProvider = serviceProvider ?? throw new InvalidPluginExecutionException("serviceProvider");
    //
    //         // Obtain the execution context service from the service provider.
    //         PluginExecutionContext = (IPluginExecutionContext) serviceProvider.GetService(typeof(IPluginExecutionContext));
    //
    //         // Obtain the tracing service from the service provider.
    //         TracingService = new LocalTracingService(serviceProvider);
    //
    //         // Get the notification service from the service provider.
    //         NotificationService = (IServiceEndpointNotificationService) serviceProvider.GetService(typeof(IServiceEndpointNotificationService));
    //
    //         // Obtain the organization factory service from the service provider.
    //         IOrganizationServiceFactory factory = (IOrganizationServiceFactory) serviceProvider.GetService(typeof(IOrganizationServiceFactory));
    //
    //         // Use the factory to generate the organization service.
    //         CurrentUserService = factory.CreateOrganizationService(PluginExecutionContext.UserId);
    //
    //         // Use the factory to generate the organization service.
    //         SystemUserService = factory.CreateOrganizationService(null);
    //     }
    //
    //     /// <summary>
    //     /// Writes a trace message to the CRM trace log.
    //     /// </summary>
    //     /// <param name="message">Message name to trace.</param>
    //     public void Trace(string message)
    //     {
    //         if (string.IsNullOrWhiteSpace(message) || TracingService == null)
    //         {
    //             return;
    //         }
    //
    //         if (PluginExecutionContext == null)
    //         {
    //             TracingService.Trace(message);
    //         }
    //         else
    //         {
    //             TracingService.Trace($"{message}, Correlation Id: {PluginExecutionContext.CorrelationId}, Initiating User: {PluginExecutionContext.InitiatingUserId}");
    //         }
    //     }
    // }

    public class LocalPluginContext : LocalContext, ILocalPluginContext
    {
        public LocalPluginContext(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public IPluginExecutionContext PluginExecutionContext => (IPluginExecutionContext) ExecutionContext;

        public bool IsPreOperation()
        {
            return IsStage(Stages.PreOperation);
        }

        public bool IsPreValidation()
        {
            return IsStage(Stages.PreValidation);
        }

        public bool IsPostOperation()
        {
            return IsStage(Stages.PostOperation);
        }

        public bool IsStage(Stages stage)
        {
            return PluginExecutionContext.Stage == (int) stage;
        }

        public string PrimaryEntityName => PluginExecutionContext.PrimaryEntityName;

        public Guid PrimaryEntityId => PluginExecutionContext.PrimaryEntityId;

        public int Stage => PluginExecutionContext.Stage;

        public Guid OrganizationId => PluginExecutionContext.OrganizationId;

        public int Depth => PluginExecutionContext.Depth;

        public void LogStart()
        {
            Log($"Entity: {PrimaryEntityName}, Message: {MessageName}, Stage: {Enum.ToObject(typeof(Stages), Stage)}, Mode: {Mode}");

            Log($"\r\nUserId\t\t\t{UserId}\r\nInitiatingUserId\t{InitiatingUserId}\r\nCorrelationId\t\t{CorrelationId}");
            Log($"\r\nStart : {DateTime.Now:dd/MM/yyyy HH:mm:ss.fff}");
        }

        public void LogExit(string message = null)
        {
            Log($"End : {DateTime.Now:dd/MM/yyyy HH:mm:ss.fff}\r\n");
            if (string.IsNullOrWhiteSpace(message) is false)
                Log(message);
        }

        public void LogContextEntry()
        {
            Log("\r\n------------------ Input Variables (before) ------------------");
            DumpInputParameters();
            Log("\r\n------------------ Shared Variables (before) ------------------");
            DumpSharedVariables();
            Log("\r\n---------------------------------------------------------------");
        }

        public void LogContextExit()
        {
            if (IsStage(Stages.PreValidation) || IsStage(Stages.PreOperation))
            {
                Log("\r\n\r\n------------------ Input Variables (after) ------------------");
                DumpInputParameters();
                Log("\r\n------------------ Shared Variables (after) ------------------");
                DumpSharedVariables();
                Log("\r\n---------------------------------------------------------------");
            }
        }
    }
}