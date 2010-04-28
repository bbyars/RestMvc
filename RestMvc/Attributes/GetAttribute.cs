namespace RestMvc.Attributes
{
    /// <summary>
    /// Route the GET method to the provided URIs.
    /// </summary>
    public class GetAttribute : ResourceActionAttribute
    {
        public GetAttribute(params string[] resourceUris) : base(resourceUris)
        {
        }
    }
}