using AutoMapper;
using FluentValidation;
using LikeTours.Contracts.Services;
using LikeTours.Data.Common.AppConstants;
using LikeTours.Data.DTO.Place;
using LikeTours.Data.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LikeTours.Data.DTO.Question;

namespace LikeTours.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateQuestionDto> _createQuestionDtoValidator;

        public QuestionsController(IQuestionService questionService, IMapper mapper, IValidator<CreateQuestionDto> createQuestionDtoValidator)
        {
            _questionService = questionService;
            _mapper = mapper;
            _createQuestionDtoValidator = createQuestionDtoValidator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] QuestionQueryParam param)
        {
            var places = await _questionService.GetAllAsync(param);
            return Ok(places);
        }

        [HttpGet("GetData")]
        [AllowAnonymous]

        public async Task<IActionResult> GetData(string? Question)
        {
            var Questions = await _questionService.GetDataAsync(Question);
            return Ok(Questions);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]

        public async Task<IActionResult> GetById(int id, string lang = AppConstants.DefaultLang)
        {
            var question = await _questionService.GetByIdAsync(id, lang);

            ApiResponse<QuestionDto> questionResponse = new ApiResponse<QuestionDto>(question);

            return Ok(questionResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateQuestionDto CreateQuestionDto)
        {
            var validationResult = await _createQuestionDtoValidator.ValidateAsync(CreateQuestionDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var createdId = await _questionService.AddAsync(CreateQuestionDto);

            CreateQuestionDto.Id = createdId;
            return CreatedAtAction(nameof(GetById), new { id = createdId }, CreateQuestionDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateQuestionDto questioneDto)
        {
            questioneDto.Id = id;
            var validationResult = await _createQuestionDtoValidator.ValidateAsync(questioneDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await _questionService.UpdateAsync(questioneDto);
            ApiResponse<CreateQuestionDto> questionResponse = new ApiResponse<CreateQuestionDto>(questioneDto);
            return Ok(questionResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            await _questionService.DeleteAsync(id);
            return NoContent();
        }
    }
}

