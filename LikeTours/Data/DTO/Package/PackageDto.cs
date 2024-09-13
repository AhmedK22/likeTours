using LikeTours.Data.DTO.Place;
using LikeTours.Data.DTO.Type;

namespace LikeTours.Data.DTO.Package
{
    public class PackageDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Lang { get; set; }
        public int PlaceId { get; set; }
        public int TourTypeId { get; set; }
        public bool HasSale { get; set; }
        public string? SaleType { get; set; }
        public int SaleAmount { get; set; }
        public int Amount { get; set; }
        public int AdultAgeFrom { get; set; }
        public decimal PriceAdult { get; set; }
        public decimal AdultFinalPrice { get; set; }
        public int ChildAgeFrom { get; set; }
        public decimal ChildFinalTo { get; set; }
        public decimal PriceChild { get; set; }
        public string DaysOfWeek { get; set; }
        public int EstimateDuration { get; set; }
        public PlaceDto Place { get; set; } 
        public TypeDto TourType { get; set; } 
        public List<SectionDto> Sections { get; set; } = new List<SectionDto>();
        public List<string> Images { get; set; } = new List<string>();
        public List<PackageDto> RefferencePackages{ get; set; }

    }
}
