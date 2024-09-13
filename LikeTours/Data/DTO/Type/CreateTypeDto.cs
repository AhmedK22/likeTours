namespace LikeTours.Data.DTO.Type
{
    public class CreateTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lang { get; set; }
        public bool Main { get; set; }
        public int? TourTypeId { get; set; }
    }
}
