using AutoMapper;
using LikeTours.Contracts.Repositories;
using LikeTours.Contracts.Services;
using LikeTours.Data.DTO;
using LikeTours.Data.DTO.Place;
using LikeTours.Data.DTO.Question;
using LikeTours.Data.Enums;
using LikeTours.Data.Models;
using LikeTours.Exceptions.Places;
using LikeTours.Exceptions.Questions;
using System.Linq.Expressions;
using Ubiety.Dns.Core;

namespace LikeTours.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IGenericRepository<Questions> _questionRepo;
        private readonly IMapper _mapper;
        public QuestionService(IMapper mapper, IGenericRepository<Questions> questionRepo)
        {

            _mapper = mapper;
            _questionRepo = questionRepo;
        }


        
        public async Task<ApiResponse<PaginationDto<QuestionDto>>> GetAllAsync(QuestionQueryParam param, OrderType orderType = OrderType.Descending)
        {
            Expression<Func<Questions, bool>> filter = p =>
                (string.IsNullOrEmpty(param.Lang) || p.Lang == param.Lang)&&
                 (string.IsNullOrEmpty(param.Question) || p.Question.Contains(param.Question))&&
                  (string.IsNullOrEmpty(param.Answer) || p.Answer.Contains(param.Answer))&&
                  (!param.Main.HasValue || p.Main == param.Main) 
                ;

            var questions = await _questionRepo.GetAllAsync(param, filter, null, orderType);
            var questionsDtos = _mapper.Map<IEnumerable<QuestionDto>>(questions);

            int totalCount = await _questionRepo.CountAsync(filter); // Use the same filter for counting
            PaginationDto<QuestionDto> paginationDto = new PaginationDto<QuestionDto>()
            {
                Page = param.Page,
                PageSize = param.PageSize,
                Total = totalCount,
                Items = questionsDtos
            };

            return new ApiResponse<PaginationDto<QuestionDto>>(paginationDto);
        }


        public async Task<ApiResponse<IEnumerable<QuestionDto>>> GetDataAsync(string? Question = null, OrderType orderType = OrderType.Descending)
        {
            var param = new PlaceQueryParams()
            {
                HasPagination = false,
            };

            Expression<Func<Questions, bool>> filter = null;

            if (Question != null)
            {
                filter = p =>

               (string.IsNullOrEmpty(Question) || p.Question.Contains(Question));
            }

            var questions = await _questionRepo.GetAllAsync(param, filter, null, orderType);
            var questionsDto = _mapper.Map<IEnumerable<QuestionDto>>(questions);
            var result = new ApiResponse<IEnumerable<QuestionDto>>(questionsDto);

            return result;
        }


        public async Task<QuestionDto> GetByIdAsync(int id, string Lang)
        {

            var question = await _questionRepo.GetByIdAsync(id, includes: p => p.RefferenceQuestion);
            if (question == null)
            {
                throw new QuestionNotFoundException($"question with id {id} not exist");
            }
            Expression<Func<Questions, bool>> filter = null;
            Questions questionLang;

            if (question.Lang != Lang)
            {
                if (question.Main)
                {
                    filter = p =>
                      (string.IsNullOrEmpty(Lang) || p.Lang == Lang)
                      &&
                      (p.QuestionId == question.Id);
                }
                else
                {
                    filter = p =>
                    (string.IsNullOrEmpty(Lang) || p.Lang == Lang)
                     && (p.QuestionId == question.QuestionId);
                }
                var questions = await _questionRepo.GetByColumnsAsync(filter, includes: p => p.RefferenceQuestion);
                questionLang = questions.FirstOrDefault();
                if (questionLang == null)
                {
                    throw new QuestionNotFoundException($"no questions with lang {Lang} for id {id} exist");
                }
            }
            else
            {
               questionLang = question;
            }


            var QuestionDto = _mapper.Map<QuestionDto>(questionLang);

            return QuestionDto;
        }

        public async Task<int> AddAsync(CreateQuestionDto CreateQuestionDto)
        {
          
            if (CreateQuestionDto.Main)
            {
                CreateQuestionDto.QuestionId = null;
            }

            var place = _mapper.Map<Questions>(CreateQuestionDto);
            return await _questionRepo.AddAsync(place);
        }

        public async Task UpdateAsync(CreateQuestionDto CreateQuestionDto)
        {
            var existingQuestion = await _questionRepo.GetByIdAsync(CreateQuestionDto.Id);

            if (existingQuestion == null)
            {
                throw new QuestionNotFoundException($"Question with id {CreateQuestionDto.Id} not found.");
            }

           

            if (existingQuestion.Main && !CreateQuestionDto.Main)
            {
                Expression<Func<Questions, bool>> filter = p => (p.QuestionId == existingQuestion.Id);
                var checkMainQuestionAssign = await _questionRepo.GetByColumnsAsync(filter);
                if (checkMainQuestionAssign.Count() > 0)
                {
                    throw new AssignedMainQuestionException($"this is main Question that assigned to another places so remove assighnes Questions first");
                }
            }

            var questionToUpdate = _mapper.Map(CreateQuestionDto, existingQuestion);

            await _questionRepo.UpdateAsync(questionToUpdate);

        }

        public async Task DeleteAsync(int id)
        {
            var existingQuestion = await _questionRepo.GetByIdAsync(id);

            if (existingQuestion == null)
            {
                throw new QuestionNotFoundException($"Question with id {id} not found.");
            }

            Expression<Func<Questions, bool>> filter = p => (p.QuestionId == existingQuestion.Id);
            var Questions = await _questionRepo.GetByColumnsAsync(filter);

            if (Questions.Count() > 0)
            {
                throw new ConstraintQuestionErrorException($"this question is refference to another types so delete other first");
            }

            await _questionRepo.DeleteAsync(existingQuestion);
        }

      
    }
}
