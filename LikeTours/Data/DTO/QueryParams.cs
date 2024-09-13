using Microsoft.AspNetCore.Mvc;

namespace LikeTours.Data.DTO
{
    public class QueryParams
    {

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public bool HasPagination { get; set; } = true;
    }
}
