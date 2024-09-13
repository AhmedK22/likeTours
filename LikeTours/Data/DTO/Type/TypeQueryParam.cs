using LikeTours.Data.Common.AppConstants;

namespace LikeTours.Data.DTO.Type
{
    public class TypeQueryParam : QueryParams
    {
        public string? Lang { get; set; } = AppConstants.DefaultLang;

        public string? Name { get; set; }
    }
}
