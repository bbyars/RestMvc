using System;

namespace RestMvc.Attributes
{
    /// <summary>
    /// Superclass for all routing attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ResourceActionAttribute : Attribute
    {
        protected ResourceActionAttribute(string resourceUri)
        {
            ResourceUri = resourceUri;
        }

        public string ResourceUri { get; private set; }

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
