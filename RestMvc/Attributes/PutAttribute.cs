namespace RestMvc.Attributes
{
    /// <summary>
    /// Route the PUT method to the provided URI.
    /// </summary>
    public class PutAttribute : ResourceActionAttribute
    {
        public PutAttribute(string resourceUri) : base(resourceUri)
        {
        }
    }
}