using System.Collections.Generic;
using System.Linq;

namespace RestMvc.Conneg
{
    /// <summary>
    /// Represents the mapping between a media type and the internal format key used by the controller actions.
    /// A simple dictionary doesn't work because there's an ordering - we prioritize if multiple
    /// media types are accepted.
    /// </summary>
    public class MediaTypeFormatMap
    {
        private readonly List<KeyValuePair<MediaType, string>> map = new List<KeyValuePair<MediaType, string>>();

        /// <summary>
        /// Returns the default format (first added)
        /// </summary>
        public virtual string DefaultFormat
        {
            get { return map.Count == 0 ? null : map[0].Value; }
        }

        /// <summary>
        /// Maps the provided media type to the given format.
        /// </summary>
        public virtual void Add(string mediaType, string format)
        {
            map.Add(new KeyValuePair<MediaType, string>(new MediaType(mediaType), format));
        }

        /// <summary>
        /// Returns true if this map contains an entry for mediaType, including
        /// a wildcard match (e.g. text/*, */*)
        /// </summary>
        public virtual bool SupportsMediaType(string mediaType)
        {
            return FormatFor(mediaType) != null;
        }

        /// <summary>
        /// Returns the format for the first entry in this map that matches any of the 
        /// media types in the given array.  Wildcard matches (e.g. text/*, */*) are acceptable.
        /// Since entries in this map take precedence over entries in the given array, this provides
        /// a way for the server to set priority over the client, if desired.
        /// </summary>
        public virtual string FormatFor(params string[] mediaTypes)
        {
            return map.Where(pair => mediaTypes.Any(mediaType => pair.Key.Matches(mediaType)))
                .Select(pair => pair.Value)
                .FirstOrDefault();
        }
    }
}
