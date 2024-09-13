using AutoMapper;
using FluentValidation;
using LikeTours.Contracts.Services;
using LikeTours.Data.Common.AppConstants;
using LikeTours.Data.DTO.Place;
using LikeTours.Data.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LikeTours.Validators.Contacts;
using LikeTours.Data.DTO.Contact;
using LikeTours.Data.DTO.AboutUs;

namespace LikeTours.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateContactDto> _addContactValidator;

        public ContactsController(IContactService contactService, IMapper mapper, IValidator<CreateContactDto> addContactValidator)
        {
            _contactService = contactService;
            _mapper = mapper;
            _addContactValidator = addContactValidator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] ContactQueryParam param)
        {
            var contacts = await _contactService.GetAllAsync(param);
            return Ok(contacts);
        }

        [HttpGet("GetData")]
        [AllowAnonymous]
        public async Task<IActionResult> GetData(string? Name)
        {
            var contacts = await _contactService.GetDataAsync(Name);
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var contact = await _contactService.GetByIdAsync(id);

            ApiResponse<ContactDto> contactResponse = new ApiResponse<ContactDto>(contact);

            return Ok(contactResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContactDto CreateContactDto)
        {
            var validationResult = await _addContactValidator.ValidateAsync(CreateContactDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var createdId = await _contactService.AddAsync(CreateContactDto);
            CreateContactDto.Id = createdId;

            return CreatedAtAction(nameof(GetById), new { id = createdId }, CreateContactDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateContactDto CreateContactDto)
        {
            CreateContactDto.Id = id;
            var validationResult = await _addContactValidator.ValidateAsync(CreateContactDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await _contactService.UpdateAsync(CreateContactDto);
            ApiResponse<CreateContactDto> placeResponse = new ApiResponse<CreateContactDto>(CreateContactDto);
            return Ok(placeResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            await _contactService.DeleteAsync(id);
            return NoContent();
        }
    }
}
