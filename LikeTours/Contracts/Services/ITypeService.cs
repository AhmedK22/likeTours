
using LikeTours.Data.DTO.Place;
using LikeTours.Data.DTO;
using LikeTours.Data.DTO.Type;
using LikeTours.Data.Enums;

namespace LikeTours.Contracts.Services
{
    public interface ITypeService
    {
        Task<ApiResponse<PaginationDto<TypeDto>>> GetAllAsync(TypeQueryParam param, OrderType orderType = OrderType.Descending);
        Task<ApiResponse<IEnumerable<TypeDto>>> GetDataAsync(string? Name = null, OrderType orderType = OrderType.Descending);
        Task<TypeDto> GetByIdAsync(int id, string Lang);
        Task<int> AddAsync(CreateTypeDto CreateTypeDto);
        Task UpdateAsync(CreateTypeDto CreateTypeDto);
        Task DeleteAsync(int id);
    }
}
