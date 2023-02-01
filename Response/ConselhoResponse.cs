namespace WebApi.Response
{    
    public class ConselhoResponse
    {
        public Slip slip { get; set; }
    }

    public class Slip
    {
        public int id { get; set; }
        public string advice { get; set; }
    }
}
