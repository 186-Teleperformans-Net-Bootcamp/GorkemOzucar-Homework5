namespace Homework5.Data
{
    public class PagingQueryParameter
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public string Keyword { get; set; } = String.Empty;
    }
}
