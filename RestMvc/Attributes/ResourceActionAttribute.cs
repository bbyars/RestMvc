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

        public virtual string[] ResourceUris { get; private set; }

        public virtual string ResourceUri
        {
            get { return ResourceUris[0]; }
        }

        public virtual string HttpMethod
        {
            get { return GetType().Name.Replace("Attribute", "").ToUpper(); }
        }

        public virtual bool SupportsUri(string resourceUri)
        {
            return ResourceUris.Any(uri => string.Equals(uri, resourceUri, StringComparison.InvariantCultureIgnoreCase));
        }

        public virtual bool Contains(ResourceActionAttribute other)
        {
            return HttpMethod.Equals(other.HttpMethod)
                && ResourceUris.Intersect(other.ResourceUris, StringComparer.InvariantCultureIgnoreCase).Count() > 0;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ResourceActionAttribute;
            return other != null && Equals(ToString(), other.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            var lines = ResourceUris.Select(uri => string.Format("{0} {1}", HttpMethod, uri));
            return string.Join(Environment.NewLine, lines.ToArray());
        }
    }
}
