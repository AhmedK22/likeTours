using LikeTours.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace LikeTours.Data.DTO.Place
{
    public class PlaceCreateDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile? Image { get; set; }
        public string Lang { get; set; }
        public bool Main { get; set; }
        public int? PlaceId { get; set; }
    }
}
