using AutoMapper;
using FluentValidation;
using LikeTours.Contracts.Services;
using LikeTours.Data.Common.AppConstants;
using LikeTours.Data.DTO;
using LikeTours.Data.DTO.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LikeTours.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreatePaymentDto> _paymentCreateDtoValidator;

        public PaymentsController(IMapper mapper, IPaymentService paymentService, IValidator<CreatePaymentDto> paymentCreateDtoValidator)
        {

            _mapper = mapper;
            _paymentService = paymentService;
            _paymentCreateDtoValidator = paymentCreateDtoValidator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] PaymentQueryParams param)
        {
            var payments = await _paymentService.GetAllAsync(param);
            return Ok(payments);
        }

        [HttpGet("GetData")]
        [AllowAnonymous]

        public async Task<IActionResult> GetData(string? title)
        {
            var payments = await _paymentService.GetDataAsync(title);
            return Ok(payments);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]

        public async Task<IActionResult> GetById(int id, string lang = AppConstants.DefaultLang)
        {
            var payment = await _paymentService.GetByIdAsync(id, lang);

            ApiResponse<PaymentDto> paymentResponse = new ApiResponse<PaymentDto>(payment);

            return Ok(paymentResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreatePaymentDto CreatePaymentDto)
        {
            var validationResult = await _paymentCreateDtoValidator.ValidateAsync(CreatePaymentDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var createdId = await _paymentService.AddAsync(CreatePaymentDto);

            CreatePaymentDto.Id = createdId;
            return CreatedAtAction(nameof(GetById), new { id = createdId }, CreatePaymentDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreatePaymentDto paymentDto)
        {
            paymentDto.Id = id;
            var validationResult = await _paymentCreateDtoValidator.ValidateAsync(paymentDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _paymentService.UpdateAsync(paymentDto);
            ApiResponse<CreatePaymentDto> paymentResponse = new ApiResponse<CreatePaymentDto>(paymentDto);
            return Ok(paymentResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            await _paymentService.DeleteAsync(id);
            return NoContent();
        }
    }
}

