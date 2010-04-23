namespace RestMvc.Attributes
{
    public class PutAttribute : ResourceActionAttribute
    {
        public PutAttribute(string resourceUri) : base(resourceUri)
        {
        }
    }
}