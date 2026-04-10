namespace studyapp.Models
{
    public class ResponseVM
    {
        public int status { get; set; }
        public string? Message { get; set; }
        public dynamic? data { get; set; }
    }
}
