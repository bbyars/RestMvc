using RestMvc.Attributes;

namespace RestMvc.Example
{
    public class TunnelledController : RestfulController
    {
        [Put("/tunnelled")]
        public void Put()
        {
            Response.StatusCode = 200;
        }

        [Delete("/tunnelled")]
        public void Delete()
        {
            Response.StatusCode = 200;
        }

        [Post("/tunnelled")]
        public void Post()
        {
            Response.StatusCode = 201;
        }
    }
}
