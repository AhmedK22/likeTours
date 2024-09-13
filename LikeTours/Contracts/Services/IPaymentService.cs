using LikeTours.Data.DTO.Question;
using LikeTours.Data.DTO;
using LikeTours.Data.Enums;
using LikeTours.Data.DTO.Payment;

namespace LikeTours.Contracts.Services
{
    public interface IPaymentService
    {
        Task<ApiResponse<PaginationDto<PaymentDto>>> GetAllAsync(PaymentQueryParams param, OrderType orderType = OrderType.Descending);
        Task<ApiResponse<IEnumerable<PaymentDto>>> GetDataAsync(string? title = null, OrderType orderType = OrderType.Descending);
        Task<PaymentDto> GetByIdAsync(int id, string Lang);
        Task<int> AddAsync(CreatePaymentDto CreatePaymentDto);
        Task UpdateAsync(CreatePaymentDto CreatePaymentDto);
        Task DeleteAsync(int id);
    }
}
