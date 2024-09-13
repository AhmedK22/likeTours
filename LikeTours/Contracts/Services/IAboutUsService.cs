using LikeTours.Data.DTO.Place;
using LikeTours.Data.DTO;
using LikeTours.Data.Enums;
using LikeTours.Data.DTO.AboutUs;

namespace LikeTours.Contracts.Services
{
    public interface IAboutUsService
    {
        Task<ApiResponse<PaginationDto<AboutUsDto>>> GetAllAsync(AboutUsQueryParams param,OrderType orderType = OrderType.Descending);
        Task<AboutUsDto> GetByIdAsync(int id, string lang);
        Task<int> AddAsync(CreateAboutUsDto CreateAboutUsDto);
        Task UpdateAsync(UpdateAboutUsDto PlaceCreateDto);
        Task DeleteAsync(int id);
    }
}
