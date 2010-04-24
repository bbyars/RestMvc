namespace RestMvc.Attributes
{
    /// <summary>
    /// Route the DELETE method to the provided URI
    /// </summary>
    public class DeleteAttribute : ResourceActionAttribute
    {
        public DeleteAttribute(string resourceUri) : base(resourceUri)
        {
        }
    }
}