using AutoMapper;
using FluentValidation;
using LikeTours.Contracts.Services;
using LikeTours.Data.Common.AppConstants;
using LikeTours.Data.DTO.Type;
using LikeTours.Data.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LikeTours.Data.DTO.Review;
using LikeTours.Validators.Review;

namespace LikeTours.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService reviewService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateReviewDto> _CreateReviewDtoValidator;

        public ReviewsController(IMapper mapper, IReviewService reviewService, IValidator<CreateReviewDto> typeCreateDtoValidator)
        {

            _mapper = mapper;

            this.reviewService = reviewService;
            _CreateReviewDtoValidator = typeCreateDtoValidator;
        }

        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> GetAll([FromQuery] ReviewQueryParam param)
        {
            try
            {
                var places = await reviewService.GetAllAsync(param);
                return Ok(places);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        [HttpGet("GetData")]
        [AllowAnonymous]

        public async Task<IActionResult> GetData(int? packageid)
        {
            var places = await reviewService.GetDataAsync(packageid);
            return Ok(places);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]

        public async Task<IActionResult> GetById(int id, string lang = AppConstants.DefaultLang)
        {
            var typeTour = await reviewService.GetByIdAsync(id, lang);

            ApiResponse<ReviewDto> placeResponse = new ApiResponse<ReviewDto>(typeTour);

            return Ok(placeResponse);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateReviewDto CreateReviewDto)
        {
            var validationResult = await _CreateReviewDtoValidator.ValidateAsync(CreateReviewDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var createdId = await reviewService.AddAsync(CreateReviewDto);
            CreateReviewDto.Id = createdId;

            return CreatedAtAction(nameof(GetById), new { id = createdId }, CreateReviewDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateReviewDto CreateReviewDto)
        {
            CreateReviewDto.Id = id;
            var validationResult = await _CreateReviewDtoValidator.ValidateAsync(CreateReviewDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await reviewService.UpdateAsync(CreateReviewDto);
            ApiResponse<CreateReviewDto> placeResponse = new ApiResponse<CreateReviewDto>(CreateReviewDto);
            return Ok(placeResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            await reviewService.DeleteAsync(id);
            return NoContent();
        }
    
    }
}
