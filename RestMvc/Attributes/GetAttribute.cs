namespace RestMvc.Attributes
{
    public class GetAttribute : ResourceActionAttribute
    {
        public GetAttribute(string resourceUri) : base(resourceUri)
        {
        }
    }
}