using LikeTours.Data.Common.AppConstants;
using Microsoft.AspNetCore.Mvc;

namespace LikeTours.Data.DTO.Place
{
    public class PlaceQueryParams : QueryParams
    {

        public string? Lang { get; set; } 

        public string? Name { get; set; }
    }
}
