using System;
using System.Linq.Expressions;
using Microsoft.WindowsAzure.Storage.Table;

namespace ChatService.Repository
{
    public static class RepositoryHelper
    {
        public static DateTime? DateTimeValue(this EntityProperty prop)
        {
            var offsetValue = prop.DateTimeOffsetValue;

            if (!offsetValue.HasValue)
                return null;

            return offsetValue.Value.DateTime;
        }

        public static TimeSpan? TimeSpanValue(this EntityProperty prop)
        {
            var stringValue = prop.StringValue;

            if (string.IsNullOrWhiteSpace(stringValue))
                return null;

            return TimeSpan.Parse(stringValue);
        }


        //taken from http://stackoverflow.com/questions/671968/retrieving-property-name-from-lambda-expression
        public static string GetName<TSource, TField>(Expression<Func<TSource, TField>> field)
        {
            if (object.Equals(field, null))
            {
                throw new NullReferenceException("Field is required");
            }

            MemberExpression expr = null;

            if (field.Body is MemberExpression)
            {
                expr = (MemberExpression)field.Body;
            }
            else if (field.Body is UnaryExpression)
            {
                expr = (MemberExpression)((UnaryExpression)field.Body).Operand;
            }
            else
            {
                const string Format = "Expression '{0}' not supported.";
                string message = string.Format(Format, field);

                throw new ArgumentException(message, "field");
            }

            return expr.Member.Name;
        }
    }
}
