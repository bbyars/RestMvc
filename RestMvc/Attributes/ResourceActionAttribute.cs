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

        public string[] ResourceUris { get; private set; }

        public string ResourceUri
        {
            get { return ResourceUris[0]; }
        }

        public string HttpMethod
        {
            get { return GetType().Name.Replace("Attribute", "").ToUpper(); }
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
            return string.Format("{0} {1}", HttpMethod, ResourceUri);
        }
    }
}
