namespace RestMvc.Attributes
{
    /// <summary>
    /// Route the POST method to the provided URI.
    /// </summary>
    public class PostAttribute : ResourceActionAttribute
    {
        public PostAttribute(string resourceUri) : base(resourceUri)
        {
        }
    }
}