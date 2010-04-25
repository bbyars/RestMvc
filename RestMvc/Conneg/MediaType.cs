using System;

namespace RestMvc.Conneg
{
    /// <summary>
    /// Represents a MIME media type
    /// </summary>
    public class MediaType
    {
        public static readonly string Atom = "application/atom+xml";
        public static readonly string Json = "application/json";
        public static readonly string Pdf = "application/pdf";
        public static readonly string Xml = "application/xml";
        public static readonly string Csv = "text/csv";
        public static readonly string Html = "text/html";
        public static readonly string PlainText = "text/plain";

        private readonly string[] parts;

        public MediaType(string text)
        {
            parts = text.Split('/');
        }

        /// <summary>
        /// The grouping for the type (e.g. text in text/html)
        /// </summary>
        public virtual string ContentType
        {
            get { return parts[0]; }
        }

        /// <summary>
        /// The sub type (e.g. html in text/html)
        /// </summary>
        public virtual string SubType
        {
            get { return parts.Length == 1 ? "" : parts[1]; }
        }

        /// <summary>
        /// Returns true if the given mediaType matches this media type.
        /// Wildcard matches are allowed - in other words, if this
        /// media type is text/html, then any of text/html, text/*,
        /// or */* will match.
        /// </summary>
        public virtual bool Matches(string mediaType)
        {
            var other = new MediaType(mediaType);
            return Equals(other) || WildCardMatch(other);
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}", ContentType, SubType);
        }

        public override bool Equals(object obj)
        {
            var other = obj as MediaType;
            if (other == null) return false;
            return string.Equals(ToString(), other.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        private bool WildCardMatch(MediaType mediaType)
        {
            if (mediaType.SubType == "*")
                return mediaType.ContentType == "*"
                   || string.Equals(ContentType, mediaType.ContentType, StringComparison.InvariantCultureIgnoreCase);

            return false;
        }
    }
}
