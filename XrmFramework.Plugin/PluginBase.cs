using System;
using System.Diagnostics;
using System.Reflection;
using System.ServiceModel;
using System.Text.Json;
using Microsoft.Xrm.Sdk;

namespace Mosesparadise.XrmFramework.Plugin
{
    /// <summary>
    /// Base class for all plug-in classes.
    /// Plugin development guide: https://docs.microsoft.com/powerapps/developer/common-data-service/plug-ins
    /// Best practices and guidance: https://docs.microsoft.com/powerapps/developer/common-data-service/best-practices/business-logic/
    /// </summary>    
    public abstract class PluginBase : IPlugin
    {
        protected string SecuredConfig { get; }

        protected string UnSecuredConfig { get; }

        /// <summary>
        ///     Gets or sets the name of the child class.
        /// </summary>
        /// <value>The name of the child class.</value>
        protected string PluginClassName => GetType().Name;

        protected PluginBase()
        {
        }

        protected PluginBase(string unsecuredConfig)
        {
            UnSecuredConfig = unsecuredConfig;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PluginBase" /> class.
        /// </summary>
        protected PluginBase(string unsecuredConfig, string securedConfig)
        {
            SecuredConfig = securedConfig;
            UnSecuredConfig = unsecuredConfig;
        }

        /// <summary>
        /// Main entry point for he business logic that the plug-in is to execute.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <remarks>
        /// </remarks>
        public void Execute(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new InvalidPluginExecutionException("serviceProvider");
            }

            // Construct the local plug-in context.
            var localContext = new LocalPluginContext(serviceProvider);

            localContext.Log($"\r\nEntered {PluginClassName}.Execute()");
            localContext.LogStart();
            localContext.Log("The context is genuine");
            localContext.LogContextEntry();

            // localPluginContext.Trace($"Entered {PluginClassName}.Execute() " +
            //                          $"Correlation Id: {localPluginContext.PluginExecutionContext.CorrelationId}, " +
            //                          $"Initiating User: {localPluginContext.PluginExecutionContext.InitiatingUserId}");
            //
            var stopwatch = Stopwatch.StartNew();
            try
            {
                // Invoke the custom implementation 
                ExecuteCrmPlugin(localContext);

                // now exit - if the derived plug-in has incorrectly registered overlapping event registrations,
                // guard against multiple executions.
                return;
            }
            catch (FaultException<OrganizationServiceFault> orgServiceFault)
            {
                // localPluginContext.Trace($"Exception: {orgServiceFault.ToString()}");
                localContext.LogError(orgServiceFault, $"{nameof(FaultException<OrganizationServiceFault>)} exception thrown in {PluginClassName}.Execute()");

                // Handle the exception.
                throw new InvalidPluginExecutionException($"OrganizationServiceFault: {orgServiceFault.Message}", orgServiceFault);
            }
            catch (Exception exception)
            {
                localContext.LogError(exception, $"{exception.GetType()} exception thrown in {PluginClassName}.Execute()");
                PluginExceptionHandler(localContext, exception);
            }
            finally
            {
                localContext.LogContextExit();
                localContext.Log($"Exiting {PluginClassName}.Execute()");
                localContext.LogExit();
                // localPluginContext.Trace($"Exiting {PluginClassName}.Execute(). duration : {stopwatch.Elapsed}");
            }
        }

        /// <summary>
        /// Placeholder for a custom plug-in implementation. 
        /// </summary>
        /// <param name="localPluginContext">Context for the current plug-in.</param>
        protected abstract void ExecuteCrmPlugin(ILocalPluginContext localPluginContext);

        private void PluginExceptionHandler(LocalPluginContext localContext, Exception exception)
        {
            switch (exception)
            {
                case TargetInvocationException e:
                    // localContext.Log($"Exception : {e.InnerException}");
                    localContext.LogError(e.InnerException, $"{nameof(TargetInvocationException)} exception thrown in {PluginClassName}.Execute()");
                    if (e.InnerException != null)
                    {
                        if (e.InnerException is InvalidPluginExecutionException invalidPluginExecutionException)
                        {
                            throw invalidPluginExecutionException;
                        }

                        throw new InvalidPluginExecutionException(e.InnerException.Message);
                    }

                    throw e;

                case JsonException e:
                {
                    throw new InvalidPluginExecutionException(e.ToString());
                }
                default: throw exception;
            }
        }
    }
}