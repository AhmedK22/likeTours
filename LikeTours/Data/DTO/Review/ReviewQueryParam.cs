namespace LikeTours.Data.DTO.Review
{
    public class ReviewQueryParam : QueryParams 
    {
        public int? PackageId { get; set; }
        public string? Lang { get; set; }
        public int? Type { get; set; }
    }
}
