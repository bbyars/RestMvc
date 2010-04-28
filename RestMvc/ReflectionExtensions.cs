using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RestMvc.Attributes;

namespace RestMvc
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Returns the name of the controller with the suffix stripped out
        /// </summary>
        public static string GetControllerName(this Type type)
        {
            return type.Name.Replace("Controller", "");
        }

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

        /// <summary>
        /// The distinct set of URI templates supported by ResourceActionAttributes on the type
        /// </summary>
        public static string[] GetResourceUris(this Type type)
        {
            return type.GetResourceActions()
                .SelectMany(action => action.GetResourceActionAttribute().ResourceUris)
                .Distinct(StringComparer.InvariantCultureIgnoreCase).ToArray();
        }

        /// <summary>
        /// The set of HTTP methods supported at resourceUri as specified in ResourceActionAttributes
        /// </summary>
        public static string[] GetSupportedMethods(this Type type, string resourceUri)
        {
            return type.GetResourceActions()
                .Select(action => action.GetResourceActionAttribute())
                .Where(attribute => attribute.SupportsUri(resourceUri))
                .Select(attribute => attribute.HttpMethod).ToArray();
        }

        /// <summary>
        /// The set of HTTP methods _not_ supported at resourceUri as specified in ResourceActionAttributes
        /// </summary>
        public static string[] GetUnsupportedMethods(this Type type, string resourceUri)
        {
            var supportedMethods = type.GetSupportedMethods(resourceUri);
            return new[] { "GET", "POST", "PUT", "DELETE" }
                .Where(method => !supportedMethods.Contains(method)).ToArray();
        }

        /// <summary>
        /// The MethodInfo associated with the given HTTP method and resource URI,
        /// as annotated by a ResourceActionAttribute, or null if no match is found.
        /// </summary>
        public static MethodInfo GetAction(this Type type, string httpMethod, string resourceUri)
        {
            var attribute = ResourceActionAttribute.Create(httpMethod, resourceUri);
            return type.GetResourceActions()
                .FirstOrDefault(action => action.GetResourceActionAttribute().Contains(attribute));
        }
    }
}
