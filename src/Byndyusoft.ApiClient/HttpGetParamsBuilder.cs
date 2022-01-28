namespace Byndyusoft.ApiClient
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;

    public static class HttpGetParamsBuilder
    {
        public static string Build(object obj)
        {
            var args = new HashSet<string>();
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                var values = GetParams(obj, descriptor);
                args.UnionWith(values);
            }

            return string.Join("&", args);
        }

        private static IEnumerable<string> GetParams(object obj, PropertyDescriptor descriptor)
        {
            var value = descriptor.GetValue(obj);
            if (value == null)
                return new List<string>();

            var customFormat = GetCustomFormat(descriptor);
            if (customFormat != null)
            {
                var formattedValue = ((IFormattable) value).ToString(customFormat, CultureInfo.InvariantCulture);
                return new[] {$"{descriptor.Name}={formattedValue}"};
            }

            if (descriptor.PropertyType == typeof(DateTime) || descriptor.PropertyType == typeof(DateTime?))
                return new[] {$"{descriptor.Name}={((DateTime) value):O}"};

            if (descriptor.PropertyType.IsArray)
                return (from object element
                        in (IEnumerable) value
                    select $"{descriptor.Name}={element}").ToArray();

            return new[] {$"{descriptor.Name}={value}"};
        }

        private static string? GetCustomFormat(MemberDescriptor descriptor)
        {
            var attributes = descriptor.Attributes
                .OfType<DisplayFormatAttribute>()
                .Where(x => string.IsNullOrWhiteSpace(x.DataFormatString) == false)
                .ToArray();
            if (attributes.Length > 1)
                throw new InvalidOperationException($"Only one format can be defined for member {descriptor.Name}");

            return attributes.Length == 0 ? null : attributes[0].DataFormatString;
        }
    }
}