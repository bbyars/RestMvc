namespace RestMvc.Attributes
{
    public class PostAttribute : ResourceActionAttribute
    {
        public PostAttribute(string resourceUri) : base(resourceUri)
        {
        }
    }
}