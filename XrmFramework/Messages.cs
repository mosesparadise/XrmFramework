using System;

namespace Mosesparadise.XrmFramework
{
    public class Messages : IEquatable<Messages>
    {
        protected string MessageName { get; }

        protected Messages(string messageName)
        {
            MessageName = messageName;
        }

        public bool Equals(Messages other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return MessageName == other.MessageName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Messages) obj);
        }

        public override int GetHashCode()
        {
            return MessageName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(MessageName) : 0;
        }

        public override string ToString() => MessageName;

        public static Messages GetMessage(string messageName)
        {
            if (string.IsNullOrEmpty(messageName))
            {
                return Default;
            }

            return new Messages(messageName);
        }

        public static Messages CreateMessage(string messageName) => new(messageName);
        public static Messages Default { get; } = CreateMessage("Default");
        public static Messages Create { get; } = CreateMessage("Create");
        public static Messages Update { get; } = CreateMessage("Update");
    }
}