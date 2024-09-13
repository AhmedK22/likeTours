using AutoMapper;
using FluentValidation;
using LikeTours.Contracts.Repositories;
using LikeTours.Contracts.Services;
using LikeTours.Data.Common.AppConstants;
using LikeTours.Data.DTO;
using LikeTours.Data.DTO.Place;
using LikeTours.Data.Enums;
using LikeTours.Data.Models;
using LikeTours.Exceptions;
using LikeTours.Exceptions.Places;
using LikeTours.Repositories;
using LikeTours.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LikeTours.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
   
    public class PlacesController : ControllerBase
    {
            private readonly IPlaceService _placeService;
            private readonly IMapper _mapper;
           private readonly IValidator<PlaceCreateDto> _placeCreateDtoValidator;

        public PlacesController(IPlaceService placeService, IMapper mapper, IValidator<PlaceCreateDto> placeCreateDtoValidator)
        {
            _placeService = placeService;
            _mapper = mapper;
            _placeCreateDtoValidator = placeCreateDtoValidator;
        }

           [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] PlaceQueryParams param)
        {
            var places = await _placeService.GetAllAsync(param);
            return Ok(places);
        }

        [HttpGet("GetData")]
        [AllowAnonymous]

        public async Task<IActionResult> GetData(string? Name)
        {
            var places = await _placeService.GetDataAsync(Name);
            return Ok(places);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]

        public async Task<IActionResult> GetById(int id, string lang = AppConstants.DefaultLang)
            {
                var place = await _placeService.GetByIdAsync(id, lang);
              
                ApiResponse<PlaceDto> placeResponse = new ApiResponse<PlaceDto>(place);

                return Ok(placeResponse);
            }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PlaceCreateDto placeCreateDto)
        {
            var validationResult = await _placeCreateDtoValidator.ValidateAsync(placeCreateDto);

            if (!validationResult.IsValid)
            {
               return BadRequest(validationResult.Errors);
            }

           var createdId = await _placeService.AddAsync(placeCreateDto);

            placeCreateDto.Id = createdId;
            return CreatedAtAction(nameof(GetById), new { id = createdId }, placeCreateDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] PlaceCreateDto placeDto)
        {
            var validationResult = await _placeCreateDtoValidator.ValidateAsync(placeDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            placeDto.Id = id;

            await _placeService.UpdateAsync(placeDto);
            ApiResponse<PlaceCreateDto> placeResponse = new ApiResponse<PlaceCreateDto>(placeDto);
            return Ok(placeResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
           
            await _placeService.DeleteAsync(id);
            return NoContent();
        }
    }
}