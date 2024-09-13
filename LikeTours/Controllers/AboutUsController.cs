using AutoMapper;
using FluentValidation;
using LikeTours.Contracts.Services;
using LikeTours.Data.Common.AppConstants;
using LikeTours.Data.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LikeTours.Data.DTO.AboutUs;

namespace LikeTours.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AboutUsController : ControllerBase
    {
        private readonly IAboutUsService _aboutService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAboutUsDto> _aboutCreateDtoValidator;
        private readonly IValidator<UpdateAboutUsDto> _aboutUpdateDtoValidator;

        public AboutUsController(IMapper mapper, IAboutUsService aboutService, IValidator<CreateAboutUsDto> aboutCreateDtoValidator, IValidator<UpdateAboutUsDto> aboutUpdateDtoValidator)
        {

            _mapper = mapper;
            _aboutService = aboutService;
            _aboutCreateDtoValidator = aboutCreateDtoValidator;
            _aboutUpdateDtoValidator = aboutUpdateDtoValidator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] AboutUsQueryParams param)
        {
            var abouts = await _aboutService.GetAllAsync(param);
            return Ok(abouts);
        }

   

        [HttpGet("{id}")]
        [AllowAnonymous]

        public async Task<IActionResult> GetById(int id, string lang = AppConstants.DefaultLang)
        {
            var about = await _aboutService.GetByIdAsync(id, lang);

            ApiResponse<AboutUsDto> aboutResponse = new ApiResponse<AboutUsDto>(about);

            return Ok(aboutResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateAboutUsDto aboutCreateDto)
        {
            var validationResult = await _aboutCreateDtoValidator.ValidateAsync(aboutCreateDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var createdId = await _aboutService.AddAsync(aboutCreateDto);

            aboutCreateDto.Id = createdId;
            return CreatedAtAction(nameof(GetById), new { id = createdId }, aboutCreateDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromQuery] UpdateAboutUsDto AboutUsDto)
        {
            AboutUsDto.Id = id;
            var validationResult = await _aboutUpdateDtoValidator.ValidateAsync(AboutUsDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _aboutService.UpdateAsync(AboutUsDto);
            ApiResponse<UpdateAboutUsDto> aboutResponse = new ApiResponse<UpdateAboutUsDto>(AboutUsDto);
            return Ok(aboutResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            await _aboutService.DeleteAsync(id);
            return NoContent();
        }
    }
}

