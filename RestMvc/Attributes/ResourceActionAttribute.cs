using System;
using System.Linq;

namespace RestMvc.Attributes
{
    /// <summary>
    /// Superclass for all routing attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ResourceActionAttribute : Attribute
    {
        /// <summary>
        /// A factory method for dynamically creating the appropriate subclass
        /// </summary>
        public static ResourceActionAttribute Create(string httpMethod, string resourceUri)
        {
            switch(httpMethod.ToUpper())
            {
                case "GET":
                    return new GetAttribute(resourceUri);
                case "POST":
                    return new PostAttribute(resourceUri);
                case "PUT":
                    return new PutAttribute(resourceUri);
                case "DELETE":
                    return new DeleteAttribute(resourceUri);
                default:
                    return null;
            }
        }

        protected ResourceActionAttribute(params string[] resourceUris)
        {
            ResourceUris = resourceUris.Select(uri => uri.TrimStart('~').TrimStart('/')).ToArray();
        }

        /// <summary>
        /// The set of URI templates that map to the method annotated with this attribute
        /// </summary>
        public virtual string[] ResourceUris { get; private set; }

        /// <summary>
        /// The HTTP method required to route to the method annotated with this attribute
        /// </summary>
        public virtual string HttpMethod
        {
            get { return GetType().Name.Replace("Attribute", "").ToUpper(); }
        }

        /// <summary>
        /// Returns true if this attribute's ResourceUris contains the provided uri; false otherwise
        /// </summary>
        public virtual bool SupportsUri(string resourceUri)
        {
            return ResourceUris.Any(uri => string.Equals(uri, resourceUri, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Returns true if the HttpMethod of other matches this HttpMethod, and if this attribute's
        /// ResourceUris is a superset of other's ResourceUris
        /// </summary>
        public virtual bool Contains(ResourceActionAttribute other)
        {
            var commonUris = ResourceUris.Intersect(other.ResourceUris, StringComparer.InvariantCultureIgnoreCase).Count();
            return HttpMethod.Equals(other.HttpMethod) && commonUris == other.ResourceUris.Length;
        }

        public override string ToString()
        {
            var lines = ResourceUris.Select(uri => string.Format("{0} {1}", HttpMethod, uri));
            return string.Join(Environment.NewLine, lines.ToArray());
        }
    }
}
