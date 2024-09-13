using System.ComponentModel.DataAnnotations.Schema;

namespace LikeTours.Data.DTO.AboutUs
{
    public class AboutUsDto
    {
        public int Id { get; set; }
        public string MainImage { get; set; }
        public string WhoAreImage { get; set; }
        public string WhoAreText { get; set; }
        public string Lang { get; set; }
        public bool Main { get; set; } = false;
        public int? AboutId { get; set; }
    }
}
