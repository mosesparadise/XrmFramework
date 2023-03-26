using System;
using Mosesparadise.XrmFramework.Plugin;

namespace SamplePlugin
{
    public class SamplePlugin : PluginBase
    {
        public SamplePlugin(string unsecureConfiguration, string secureConfiguration)
            : base(unsecureConfiguration, secureConfiguration)
        {
        }

        protected override void ExecuteCrmPlugin(ILocalPluginContext localPluginContext)
        {
            if (localPluginContext == null)
            {
                throw new ArgumentNullException(nameof(localPluginContext));
            }

            localPluginContext.Trace("Hello World!");
        }
    }
}