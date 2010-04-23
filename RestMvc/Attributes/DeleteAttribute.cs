namespace RestMvc.Attributes
{
    public class DeleteAttribute : ResourceActionAttribute
    {
        public DeleteAttribute(string resourceUri) : base(resourceUri)
        {
        }
    }
}