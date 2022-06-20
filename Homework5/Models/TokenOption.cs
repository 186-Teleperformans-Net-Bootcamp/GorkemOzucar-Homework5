namespace Homework5.Models
{
    public class TokenOption
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Expiration { get; set; }
        public string Key { get; set; }
    }
}
