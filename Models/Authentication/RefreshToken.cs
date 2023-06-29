namespace asp.net_core_api_template.Models.Authentication
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string TokenValue { get; set; }
        public string ExpiryDate { get; set; }
        public int UserId { get; set; }

        public bool IsExpired()
        {
            return new DateTimeOffset(Convert.ToDateTime(ExpiryDate)).ToUnixTimeMilliseconds() < new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
        }
    }
}
