using LikeTours.Data.Common.AppConstants;

namespace LikeTours.Data.DTO.Package
{
    public class PackageQueryParam:QueryParams
    {
        public string? Lang { get; set; } = AppConstants.DefaultLang;

        public string? Title { get; set; }
        public int? PlaceId { get; set; }
        public int? TourTypeId { get; set; }
        public bool? HasSale { get; set; }
        public string? SaleType { get; set; }
        public int? SaleAmount { get; set; }

    }
}
