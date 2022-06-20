namespace Homework5.Data
{
    public class GroupPagingQueryParameter : PagingQueryParameter
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

    }
}
