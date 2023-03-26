using Microsoft.Xrm.Sdk;

namespace Mosesparadise.XrmFramework.Plugin.Context
{
    public interface ILocalContext : IServiceContext
    {
        IExecutionContext ExecutionContext { get; }
        EntityReference UserRef { get; }
        EntityReference BusinessUnitRef { get; }
        Messages MessageName { get; }
        Modes Mode { get; }

        void LogFields(Entity entity, params string[] fieldNames);
        bool TryGetInputParameter<T>(InputParameters parameterName, out T entity);
        T GetInputParameter<T>(InputParameters parameterName);
        void SetInputParameter<T>(InputParameters parameterName, T parameterValue);
        T GetOutputParameter<T>(OutputParameters parameterName);
        void SetOutputParameter<T>(OutputParameters parameterName, T parameterValue);
        void VerifyInputParameter(InputParameters parameterName);
        bool HasInputParameter(InputParameters parameterName);
        bool HasOutputParameter(OutputParameters parameterName);
        bool HasSharedVariable(string variableName);
        void SetSharedVariable<T>(string variableName, T value);
        T GetSharedVariable<T>(string variableName);
        bool HasPreImage(string imageName);
        Entity GetPreImage(string imageName);
        Entity GetPreImageOrDefault(string imageName);
        bool HasPostImage(string imageName);
        Entity GetPostImage(string imageName);
        bool IsCreate();
        bool IsUpdate();
        bool IsMessage(Messages message);
        bool IsSynchronous();
        bool IsAsynchronous();
        bool TryGetPreImage(string imageName, out Entity image);
        bool TryGetPostImage(string imageName, out Entity image);
        void DumpSharedVariables();
        void DumpInputParameters();
    }
}