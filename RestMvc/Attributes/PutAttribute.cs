namespace RestMvc.Attributes
{
    /// <summary>
    /// Route the PUT method to the provided URIs.
    /// </summary>
    public class PutAttribute : ResourceActionAttribute
    {
        public PutAttribute(params string[] resourceUris) : base(resourceUris)
        {
        }
    }
}