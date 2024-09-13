using AutoMapper;
using FluentValidation;
using LikeTours.Contracts.Services;
using LikeTours.Data.Common.AppConstants;
using LikeTours.Data.DTO;
using LikeTours.Data.DTO.Package;
using LikeTours.Data.DTO.Place;
using LikeTours.Data.Models;
using LikeTours.Services;
using LikeTours.Validators.packages;
using LikeTours.Validators.Places;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System;

namespace LikeTours.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageService _packageService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreatePackageDto> _packageCreateDtoValidator;
        private readonly IValidator<AddSaleDto> _addSaleValidator;

        public PackagesController(IPackageService packageService, IMapper mapper, IValidator<CreatePackageDto> packageCreateDtoValidator, IValidator<AddSaleDto> addSaleValidator)
        {
            _packageService = packageService;
            _mapper = mapper;
            _packageCreateDtoValidator = packageCreateDtoValidator;
            _addSaleValidator = addSaleValidator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPackages([FromQuery]PackageQueryParam param)
        {
            var packages = await _packageService.GetAllPackagesAsync(param);
           
            return Ok(packages);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPackage(int id, string Lang)
        {
            var package = await _packageService.GetPackageByIdAsync(id, Lang = AppConstants.DefaultLang);
           
            return Ok(package);
        }

        [HttpPost]

        public async Task<IActionResult> CreatePackage([FromForm] CreatePackageDto createPackageDto)
        {
            var validationResult = await _packageCreateDtoValidator.ValidateAsync(createPackageDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var package = _mapper.Map<Package>(createPackageDto);

            var packageId = await _packageService.AddPackageAsync(package, createPackageDto.Images, createPackageDto.Sections);
            createPackageDto.Id = packageId;
            return CreatedAtAction(nameof(GetPackage), new { id = packageId }, createPackageDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage(int id, [FromForm] CreatePackageDto package)
        {
            package.Id = id;
            var validationResult = await _packageCreateDtoValidator.ValidateAsync(package);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
           

            await _packageService.UpdatePackageAsync(package,package.Images, package.Sections);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            await _packageService.DeletePackageAsync(id);
            return NoContent();
        }
        [HttpPut("AddSale/{Id}")]
        public async Task<IActionResult> AddSale(int Id,AddSaleDto saleDto)
        {
            var validationResult = await _addSaleValidator.ValidateAsync(saleDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await _packageService.AddSalePackageAsync(Id, saleDto.SaleAmount, saleDto.SaleType);
            return NoContent(); 

        }

        
    }


}
