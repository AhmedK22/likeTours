using LikeTours.Data.DTO;
using LikeTours.Data.DTO.Place;
using LikeTours.Data.Enums;
using LikeTours.Data.Models;

namespace LikeTours.Contracts.Services
{
    public interface IPlaceService
    {
        Task<ApiResponse<PaginationDto<PlaceDto>>> GetAllAsync(PlaceQueryParams param, OrderType orderType = OrderType.Descending);
        Task<ApiResponse<IEnumerable<PlaceDto>>> GetDataAsync(string? Name = null,OrderType orderType = OrderType.Descending);
        Task<PlaceDto> GetByIdAsync(int id,string lang);
        Task<int> AddAsync(PlaceCreateDto placeCreateDto); 
        Task UpdateAsync(PlaceCreateDto placeDto);
        Task DeleteAsync(int id);
    }
}
