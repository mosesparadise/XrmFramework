using System;
using Microsoft.Xrm.Sdk;

namespace Mosesparadise.XrmFramework.Plugin.Context
{
    public partial class LocalContext
    {
        public bool TryGetInputParameter<T>(InputParameters parameterName, out T entity)
        {
            if (HasInputParameter(parameterName))
            {
                entity = GetInputParameter<T>(parameterName);
                return true;
            }

            entity = default;
            return false;
        }

        public virtual T GetInputParameter<T>(InputParameters parameterName)
        {
            VerifyInputParameter(parameterName);

            if (typeof(T).IsEnum)
            {
                var value = (OptionSetValue) ExecutionContext.InputParameters[parameterName.ToString()];
                return (T) Enum.ToObject(typeof(T), value.Value);
            }

            return (T) ExecutionContext.InputParameters[parameterName.ToString()];
        }

        public void SetInputParameter<T>(InputParameters parameterName, T parameterValue)
        {
            ExecutionContext.InputParameters[parameterName.ToString()] = parameterValue;
        }

        public virtual T GetOutputParameter<T>(OutputParameters parameterName)
        {
            return (T) ExecutionContext.OutputParameters[parameterName.ToString()];
        }

        public void SetOutputParameter<T>(OutputParameters parameterName, T parameterValue)
        {
            ExecutionContext.OutputParameters[parameterName.ToString()] = parameterValue;
        }

        public void VerifyInputParameter(InputParameters parameterName)
        {
            if (!ExecutionContext.InputParameters.Contains(parameterName.ToString())
                || ExecutionContext.InputParameters[parameterName.ToString()] == null)
            {
                throw new ArgumentNullException(nameof(parameterName), $"InputParameter {parameterName} does not exist in this context");
            }
        }

        public bool HasInputParameter(InputParameters parameterName)
        {
            if (!ExecutionContext.InputParameters.Contains(parameterName.ToString())
                || ExecutionContext.InputParameters[parameterName.ToString()] == null)
                return false;
            return true;
        }

        public bool HasOutputParameter(OutputParameters parameterName)
        {
            if (!ExecutionContext.OutputParameters.Contains(parameterName.ToString())
                || ExecutionContext.OutputParameters[parameterName.ToString()] == null)
                return false;
            return true;
        }

        public bool HasSharedVariable(string variableName)
        {
            return ExecutionContext.SharedVariables.ContainsKey(variableName);
        }

        public void SetSharedVariable<T>(string variableName, T value)
        {
            if (typeof(T).IsEnum)
            {
                ExecutionContext.SharedVariables[variableName] = Enum.GetName(typeof(T), value);
            }
            else
            {
                ExecutionContext.SharedVariables[variableName] = value;
            }
        }

        public virtual T GetSharedVariable<T>(string variableName)
        {
            T value = default;

            if (ExecutionContext.SharedVariables.ContainsKey(variableName))
            {
                if (typeof(T).IsEnum)
                {
                    var valueTemp = (string) ExecutionContext.SharedVariables[variableName];

                    value = (T) Enum.Parse(typeof(T), valueTemp);
                }
                else
                {
                    value = (T) ExecutionContext.SharedVariables[variableName];
                }
            }

            return value;
        }
    
        public virtual void DumpSharedVariables()
        {
            Logger.LogCollection(ExecutionContext.SharedVariables);
        }


        public virtual void DumpInputParameters()
        {
            Logger.LogCollection(ExecutionContext.InputParameters, false, "ExtensionData", "Parameters", "RequestId", "RequestName");
        }
    }
}