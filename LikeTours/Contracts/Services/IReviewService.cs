using LikeTours.Data.DTO.Question;
using LikeTours.Data.DTO;
using LikeTours.Data.Enums;
using LikeTours.Data.DTO.Review;

namespace LikeTours.Contracts.Services
{
    public interface IReviewService
    {
        Task<ApiResponse<PaginationDto<ReviewDto>>> GetAllAsync(ReviewQueryParam param, OrderType orderType = OrderType.Descending);
        Task<ApiResponse<IEnumerable<ReviewDto>>> GetDataAsync(int? packageId = null, OrderType orderType = OrderType.Descending);
        Task<ReviewDto> GetByIdAsync(int id, string Lang);
        Task<int> AddAsync(CreateReviewDto CreateReviewDto);
        Task UpdateAsync(CreateReviewDto CreateReviewDto);
        Task DeleteAsync(int id);
    }
}
