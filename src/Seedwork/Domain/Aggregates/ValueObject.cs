using System;
using System.Linq;
using System.Reflection;

namespace Goal.Seedwork.Domain.Aggregates
{
    public class ValueObject : IEquatable<ValueObject>
    {
        protected ValueObject() { }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj is ValueObject valueObject)
            {
                return Equals(valueObject);
            }

            return false;
        }

        public virtual bool Equals(ValueObject obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            PropertyInfo[] properties = GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            if (properties.Any())
            {
                return properties.All(p =>
                {
                    object left = p.GetValue(this, null);
                    object right = p.GetValue(obj, null);

                    return Equals(left, right);
                });
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = 31;
            bool changeMultiplier = false;
            int index = 1;

            PropertyInfo[] properties = GetType().GetProperties();

            if (properties.Any())
            {
                foreach (PropertyInfo item in properties)
                {
                    object value = item.GetValue(this, null);

                    if (value is null)
                    {
                        hashCode ^= (index * 13);
                    }
                    else
                    {
                        hashCode = (hashCode * (changeMultiplier ? 59 : 114)) + value.GetHashCode();
                        changeMultiplier = !changeMultiplier;
                    }
                }
            }

            return Math.Abs(hashCode);
        }

        public static bool operator ==(ValueObject left, ValueObject right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        public static bool operator !=(ValueObject left, ValueObject right) => !(left == right);
    }
}
