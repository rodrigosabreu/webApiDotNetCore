namespace WebApi.Response
{    
    public class ConselhoResponse
    {
        public ConselhoResponse()
        {
            slip = new Slip();
        }

        public Slip slip { get; set; }
    }

    public class Slip
    {
        public Slip()
        {
            advice = null;
        }

        public int id { get; set; }
        public string advice { get; set; }
    }
}
