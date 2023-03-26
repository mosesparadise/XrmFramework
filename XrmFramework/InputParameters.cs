using System;

namespace Mosesparadise.XrmFramework
{
    public class InputParameters : IEquatable<InputParameters>
    {
        public string ParameterName { get; }

        protected InputParameters(string inputParameterName)
        {
            ParameterName = inputParameterName;
        }

        public bool Equals(InputParameters other)
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
            return Equals((InputParameters) obj);
        }

        public override int GetHashCode()
        {
            return ParameterName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ParameterName) : 0;
        }

        public override string ToString() => ParameterName;

        public static InputParameters Create(string inputParameterName) => new(inputParameterName);

        public static InputParameters State { get; } = Create("State");
        public static InputParameters Status { get; } = Create("Status");
        public static InputParameters SystemUserId { get; } = Create("SystemUserId");
    }
}