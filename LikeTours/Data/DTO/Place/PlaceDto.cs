using System.ComponentModel.DataAnnotations;

namespace LikeTours.Data.DTO.Place
{
    public class PlaceDto
    {
      
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lang { get; set; }
        public string? Image { get; set; }
        public bool Main { get; set; }
        public List<PlaceDto> RefferencePlaces { get; set; }
    }
}
