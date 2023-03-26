using System;
using Microsoft.Xrm.Sdk;
using Mosesparadise.XrmFramework.Logging;

namespace Mosesparadise.XrmFramework.Plugin.Context
{
    public partial class LocalContext : ILocalContext
    {
        private IOrganizationService _adminService;
        private IServiceProvider _serviceProvider;
        private readonly EntityReference _businessUnitRef;

        public Guid UserId => ExecutionContext.UserId;
        public Guid InitiatingUserId => ExecutionContext.InitiatingUserId;
        public Guid CorrelationId => ExecutionContext.CorrelationId;
        public string OrganizationName => ExecutionContext.OrganizationName;
        public ILogger Logger { get; }
        public IOrganizationServiceFactory Factory { get; }
        public IOrganizationService SystemUserService => _adminService ??= Factory.CreateOrganizationService(null);

        public IOrganizationService CurrentUserService { get; }

        // public IPluginExecutionContext PluginExecutionContext { get; }
        public ITracingService TracingService { get; }

        public LocalContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            // Obtain the execution context service from the service provider.
            ExecutionContext = (IPluginExecutionContext) serviceProvider.GetService(typeof(IPluginExecutionContext));

            // // Obtain the execution context service from the service provider.
            // PluginExecutionContext = (IPluginExecutionContext) serviceProvider.GetService(typeof(IPluginExecutionContext));
            //
            // // Obtain the execution context service from the service provider.
            // PluginExecutionContext = (IPluginExecutionContext) serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the tracing service from the service provider.
            TracingService = new LocalTracingService(serviceProvider);

            // Obtain the organization factory service from the service provider.
            Factory = (IOrganizationServiceFactory) serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            // Use the factory to generate the Organization Service.
            CurrentUserService = Factory.CreateOrganizationService(ExecutionContext.UserId);

            _businessUnitRef = new EntityReference("businessunit", ExecutionContext.BusinessUnitId);

            Logger = LoggerFactory.GetLogger(this, TracingService.Trace);
        }

        public IExecutionContext ExecutionContext { get; }

        public EntityReference UserRef => new("systemuser", UserId);

        public EntityReference BusinessUnitRef => _businessUnitRef;

        public void Log(string message, params object[] paramsObject)
        {
            Logger.Log(message, paramsObject);
        }

        public void LogFields(Entity entity, params string[] fieldNames)
        {
            Logger.LogCollection(entity.Attributes, true, fieldNames);
        }

        public void LogError(Exception e, string message = null, params object[] args)
        {
            Logger.LogError(e, message ?? "ERROR", args);
        }

        /// <summary>
        /// Writes a trace message to the CRM trace log.
        /// </summary>
        /// <param name="message">Message name to trace.</param>
        public void Trace(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || TracingService == null)
            {
                return;
            }

            if (ExecutionContext == null)
            {
                TracingService.Trace(message);
            }
            else
            {
                TracingService.Trace($"{message}, Correlation Id: {ExecutionContext.CorrelationId}, Initiating User: {ExecutionContext.InitiatingUserId}");
            }
        }

        public IOrganizationService GetService(Guid userId)
        {
            return Factory.CreateOrganizationService(userId);
        }

        public LogServiceMethod LogServiceMethod => Logger.LogWithMethodName;
    }
}