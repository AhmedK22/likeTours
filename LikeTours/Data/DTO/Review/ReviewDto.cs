using LikeTours.Data.DTO.Type;

namespace LikeTours.Data.DTO.Review
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string? Image { get; set; }
        public string Body { get; set; }
        public int TypeId { get; set; }
        public int PackageId { get; set; }
        public bool IsConfirmed { get; set; }
        public string Lang { get; set; }
        public bool Main { get; set; } = false;
        public int? ReviewId { get; set; }
        public List<ReviewDto> RefferenceReviews { get; set; }
    }
}
