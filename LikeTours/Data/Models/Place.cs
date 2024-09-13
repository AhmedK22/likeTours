using LikeTours.Data.Common.Model;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LikeTours.Data.Models
{
    public class Place: IDBEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public string Lang { get; set; }
        public bool Main { get; set; } = false;
        public int? PlaceId { get; set; }
        public Place MainPlace { get; set; }
        [JsonIgnore]
        public ICollection<Place> RefferencePlaces { get; set; }
        [JsonIgnore]
        public ICollection<Package> Packages { get; set; }
    }

}
