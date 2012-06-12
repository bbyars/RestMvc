namespace RestMvc.Attributes
{
    /// <summary>
    /// Route the POST method to the provided URIs.
    /// </summary>
    public class PostAttribute : ResourceActionAttribute
    {
        public PostAttribute(params string[] resourceUris) : base(resourceUris)
        {
        }
    }
}
