using LikeTours.Data.DTO.Place;
using LikeTours.Data.DTO;
using LikeTours.Data.Enums;
using LikeTours.Data.DTO.Contact;

namespace LikeTours.Contracts.Services
{
    public interface IContactService
    {
        Task<ApiResponse<PaginationDto<ContactDto>>> GetAllAsync(ContactQueryParam param, OrderType orderType = OrderType.Descending);
        Task<ApiResponse<IEnumerable<ContactDto>>> GetDataAsync(string? Name = null, OrderType orderType = OrderType.Descending);
        Task<ContactDto> GetByIdAsync(int id);
        Task<int> AddAsync(CreateContactDto CreateContactDto);
        Task UpdateAsync(CreateContactDto CreateContactDto);
        Task DeleteAsync(int id);
    }
}
