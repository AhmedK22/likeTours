using LikeTours.Data.Common.Model;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LikeTours.Data.Models
{
    public class Package : IDBEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Lang { get; set; }
        public decimal Amount { get; set; }
        public int? AdultAgeFrom { get; set; }
        public decimal? PriceAdult { get; set; }
        public decimal? AdultFinalTo { get; set; }
        public int? ChildAgeFrom { get; set; }
        public decimal? ChildFinalTo { get; set; }
        public decimal? PriceChild { get; set; }
        public string DaysOfWeek { get; set; }
        public int EstimateDuration { get; set; }
        public int PlaceId { get; set; }
        public int TourTypeId { get; set; }
        public bool Main { get; set; } = false;
        public int? PackageId { get; set; }
        public bool HasSale { get; set; } = false;
        public string? SaleType { get; set; }
        public int? SaleAmount { get; set; }
        public Place Place { get; set; }
        public TourType TourType { get; set; }
        public Package MainPackage { get; set; }
        [JsonIgnore]
        public ICollection<Package> RefferencePackages { get; set; }
        [JsonIgnore]
        public ICollection<Image> Images { get; set; }
        [JsonIgnore]
        public ICollection<Section> Sections { get; set; }
    }
}
