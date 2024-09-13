namespace LikeTours.Data.DTO.AboutUs
{
    public class UpdateAboutUsDto
    {
        public int Id { get; set; }
        public IFormFile? MainImage { get; set; }
        public IFormFile? WhoAreImage { get; set; }
        public string WhoAreText { get; set; }
        public string Lang { get; set; }
        public bool Main { get; set; } = false;
        public int? AboutId { get; set; }
    }
}
