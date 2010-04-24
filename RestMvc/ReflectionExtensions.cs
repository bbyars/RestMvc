using System;
using System.Collections.Generic;
using System.Reflection;
using RestMvc.Attributes;

namespace RestMvc
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Get all methods associated with a ResourceActionAttribute.
        /// </summary>
        public static IEnumerable<MethodInfo> GetResourceActions(this Type type)
        {
            var methods = type.GetMethods();
            foreach (var method in methods)
            {
                if (method.GetResourceActionAttribute() != null)
                    yield return method;
            }
        }

        /// <summary>
        /// Gets the first ResourceActionAttribute associated with the given method.
        /// </summary>
        public static ResourceActionAttribute GetResourceActionAttribute(this MethodInfo method)
        {
            var attributes = method.GetCustomAttributes(typeof(ResourceActionAttribute), false);
            if (attributes.Length > 0)
                return attributes[0] as ResourceActionAttribute;
            return null;
        }
    }
}
