using System;

namespace Mosesparadise.XrmFramework
{
    public class OutputParameters : IEquatable<OutputParameters>
    {
        protected string ParameterName { get; }

        protected OutputParameters(string outputParameterName)
        {
            ParameterName = outputParameterName;
        }

        public bool Equals(OutputParameters other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ParameterName == other.ParameterName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OutputParameters) obj);
        }

        public override int GetHashCode()
        {
            return ParameterName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ParameterName) : 0;
        }

        public override string ToString() => ParameterName;

        public static OutputParameters Create(string outputParameterName) => new(outputParameterName);
    }
}