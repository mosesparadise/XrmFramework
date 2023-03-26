using System;
using Microsoft.Xrm.Sdk;

namespace Mosesparadise.XrmFramework.Plugin.Context
{
    public partial class LocalContext
    {
        public Messages MessageName => Messages.GetMessage(ExecutionContext.MessageName);

        public Modes Mode
        {
            get
            {
                if (!Enum.IsDefined(typeof(Modes), ExecutionContext.Mode))
                {
                    throw new InvalidPluginExecutionException($"Mode {ExecutionContext.Mode} is not part of modes enum");
                }

                return (Modes) ExecutionContext.Mode;
            }
        }

        public virtual bool IsCreate()
        {
            return IsMessage(Messages.Create);
        }

        public virtual bool IsUpdate()
        {
            return IsMessage(Messages.Update);
        }

        public virtual bool IsMessage(Messages message)
        {
            return ExecutionContext.MessageName == message.ToString();
        }

        public virtual bool IsSynchronous()
        {
            return IsMode(Modes.Synchronous);
        }

        public virtual bool IsAsynchronous()
        {
            return IsMode(Modes.Asynchronous);
        }

        private bool IsMode(Modes mode)
        {
            return Mode == mode;
        }
    }
}