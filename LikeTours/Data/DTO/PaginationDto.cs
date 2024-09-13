namespace LikeTours.Data.DTO
{
    public class PaginationDto<T> where T : class
    {
        public int PageSize {  get; set; } 
        public int Page {  get; set; }
        public int Total  {  get; set; }
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    }
}
