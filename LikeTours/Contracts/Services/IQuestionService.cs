using LikeTours.Data.DTO.Contact;
using LikeTours.Data.DTO;
using LikeTours.Data.Enums;
using LikeTours.Data.DTO.Question;

namespace LikeTours.Contracts.Services
{
    public interface IQuestionService
    {
        Task<ApiResponse<PaginationDto<QuestionDto>>> GetAllAsync(QuestionQueryParam param, OrderType orderType = OrderType.Descending);
        Task<ApiResponse<IEnumerable<QuestionDto>>> GetDataAsync(string? Question = null, OrderType orderType = OrderType.Descending);
        Task<QuestionDto> GetByIdAsync(int id,string Lang);
        Task<int> AddAsync(CreateQuestionDto CreateQuestionDto);
        Task UpdateAsync(CreateQuestionDto CreateQuestionDto);
        Task DeleteAsync(int id);
    }
}
