using AutoMapper;
using LikeTours.Contracts.Services;
using LikeTours.Data.DTO.Place;
using LikeTours.Data.DTO;
using LikeTours.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LikeTours.Exceptions.Types;
using LikeTours.Data.DTO.Type;
using LikeTours.Data.Common.AppConstants;
using LikeTours.Exceptions.Places;
using LikeTours.Services;
using LikeTours.Validators.Places;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace LikeTours.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TypesController : ControllerBase
    {
        private readonly ITypeService _typeService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTypeDto> _typeCreateDtoValidator;

        public TypesController(ITypeService typeService, IMapper mapper, IValidator<CreateTypeDto> typeCreateDtoValidator)
        {
            _typeService = typeService;
            _mapper = mapper;
            _typeCreateDtoValidator = typeCreateDtoValidator;
        }

        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> GetAll([FromQuery] TypeQueryParam param)
        {
            try
            {
                var places = await _typeService.GetAllAsync(param);
                return Ok(places);

            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        [HttpGet("GetData")]
        [AllowAnonymous]

        public async Task<IActionResult> GetData(string? Name)
        {
            var places = await _typeService.GetDataAsync(Name);
            return Ok(places);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]

        public async Task<IActionResult> GetById(int id, string lang = AppConstants.DefaultLang)
        {
            var typeTour = await _typeService.GetByIdAsync(id, lang);

            ApiResponse<TypeDto> placeResponse = new ApiResponse<TypeDto>(typeTour);

            return Ok(placeResponse);
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTypeDto CreateTypeDto)
        {
            var validationResult = await _typeCreateDtoValidator.ValidateAsync(CreateTypeDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var createdId = await _typeService.AddAsync(CreateTypeDto);
            CreateTypeDto.Id = createdId;

            return CreatedAtAction(nameof(GetById), new { id = createdId }, CreateTypeDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateTypeDto CreateTypeDto)
        {
            CreateTypeDto.Id = id;
            var validationResult = await _typeCreateDtoValidator.ValidateAsync(CreateTypeDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await _typeService.UpdateAsync(CreateTypeDto);
            ApiResponse<CreateTypeDto> placeResponse = new ApiResponse<CreateTypeDto>(CreateTypeDto);
            return Ok(placeResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            await _typeService.DeleteAsync(id);
            return NoContent();
        }
    }
}
