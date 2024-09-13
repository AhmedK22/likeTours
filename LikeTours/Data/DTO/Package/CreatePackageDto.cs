using LikeTours.Data.Models;
using System.Text.Json.Serialization;

namespace LikeTours.Data.DTO.Package
{
    public class CreatePackageDto
    {    
        public int Id { get; set; }
        public string Title { get; set; }
        public string Lang { get; set; }
        public bool Main { get; set; }
        public int PlaceId { get; set; }
        public int? PackageId { get; set; }
        public int TourTypeId { get; set; }
        public bool HasSale { get; set; } 
        public string? SaleType { get; set; }
        public int SaleAmount { get; set; }
        public int Amount { get; set; }
        public int? AdultAgeFrom { get; set; }
        public decimal? PriceAdult { get; set; }
        public decimal? AdultFinalTo { get; set; }
        public int? ChildAgeFrom { get; set; }
        public decimal? ChildFinalTo { get; set; }
        public decimal? PriceChild { get; set; }
        public string DaysOfWeek { get; set; }
        public int EstimateDuration { get; set; }
        public string Sections { get; set; }
        public  List<IFormFile> Images { get; set; }
    }
}
