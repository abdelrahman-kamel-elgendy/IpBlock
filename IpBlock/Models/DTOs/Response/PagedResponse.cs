namespace IpBlock.Models.DTOs.Response
{
    public class PagedResponse<T>
    {
        public int Page { get; init; }
        public int PageSize { get; init; }
        public long Total { get; init; }
        public IEnumerable<T> Items { get; init; } = Enumerable.Empty<T>();
    }
}
