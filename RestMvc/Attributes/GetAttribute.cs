namespace RestMvc.Attributes
{
    /// <summary>
    /// Route the GET method to the provided URI.
    /// </summary>
    public class GetAttribute : ResourceActionAttribute
    {
        public GetAttribute(string resourceUri) : base(resourceUri)
        {
        }
    }
}