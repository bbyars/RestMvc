namespace RestMvc.Attributes
{
    /// <summary>
    /// Route the DELETE method to the provided URIs.
    /// </summary>
    public class DeleteAttribute : ResourceActionAttribute
    {
        public DeleteAttribute(params string[] resourceUris) : base(resourceUris)
        {
        }
    }
}
