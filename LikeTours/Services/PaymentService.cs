using AutoMapper;
using LikeTours.Contracts.Repositories;
using LikeTours.Contracts.Services;
using LikeTours.Data.DTO;
using LikeTours.Data.DTO.Payment;
using LikeTours.Data.DTO.Place;
using LikeTours.Data.Enums;
using LikeTours.Data.Models;
using LikeTours.Exceptions.Payment;
using System.Linq.Expressions;

namespace LikeTours.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IGenericRepository<Payment> _paymentRepo;
        private readonly IMapper _mapper;
        public PaymentService(IMapper mapper, IGenericRepository<Payment> paymentRepo)
        {

            _mapper = mapper;
            _paymentRepo = paymentRepo;
        }



        public async Task<ApiResponse<PaginationDto<PaymentDto>>> GetAllAsync(PaymentQueryParams param, OrderType orderType = OrderType.Descending)
        {
            Expression<Func<Payment, bool>> filter = p =>
                (string.IsNullOrEmpty(param.Lang) || p.Lang == param.Lang) &&
                 (string.IsNullOrEmpty(param.Title) || p.Title.Contains(param.Title)) &&
                  (!param.Main.HasValue || p.Main == param.Main)  ;

            var payments = await _paymentRepo.GetAllAsync(param, filter, null, orderType);
            var paymentsDtos = _mapper.Map<IEnumerable<PaymentDto>>(payments);

            int totalCount = await _paymentRepo.CountAsync(filter); // Use the same filter for counting
            PaginationDto<PaymentDto> paginationDto = new PaginationDto<PaymentDto>()
            {
                Page = param.Page,
                PageSize = param.PageSize,
                Total = totalCount,
                Items = paymentsDtos
            };

            return new ApiResponse<PaginationDto<PaymentDto>>(paginationDto);
        }


        public async Task<ApiResponse<IEnumerable<PaymentDto>>> GetDataAsync(string? title = null, OrderType orderType = OrderType.Descending)
        {
            var param = new PlaceQueryParams()
            {
                HasPagination = false,
            };

            Expression<Func<Payment, bool>> filter = null;

            if (title != null)
            {
                filter = p =>

               (string.IsNullOrEmpty(title) || p.Title.Contains(title));
            }

            var payments = await _paymentRepo.GetAllAsync(param, filter, null, orderType);
            var paymentsDto = _mapper.Map<IEnumerable<PaymentDto>>(payments);
            var result = new ApiResponse<IEnumerable<PaymentDto>>(paymentsDto);

            return result;
        }


        public async Task<PaymentDto> GetByIdAsync(int id, string Lang)
        {

            var payment = await _paymentRepo.GetByIdAsync(id, includes: p => p.RefferencePayments);
            if (payment == null)
            {
                throw new PaymentNotFoundException($"payment with id {id} not exist");
            }
            Expression<Func<Payment, bool>> filter = null;
            Payment paymentLang;

            if (payment.Lang != Lang)
            {
                if (payment.Main)
                {
                    filter = p =>
                      (string.IsNullOrEmpty(Lang) || p.Lang == Lang)
                      &&
                      (p.PaymentId == payment.Id);
                }
                else
                {
                    filter = p =>
                    (string.IsNullOrEmpty(Lang) || p.Lang == Lang)
                     && (p.PaymentId == payment.PaymentId);
                }
                var payments = await _paymentRepo.GetByColumnsAsync(filter, includes: p => p.RefferencePayments);
                paymentLang = payments.FirstOrDefault();
                if (paymentLang == null)
                {
                    throw new PaymentNotFoundException($"no payments with lang {Lang} for id {id} exist");
                }
            }
            else
            {
                paymentLang = payment;
            }


            var PaymentDto = _mapper.Map<PaymentDto>(paymentLang);

            return PaymentDto;
        }

        public async Task<int> AddAsync(CreatePaymentDto CreatePaymentDto)
        {

            if (CreatePaymentDto.Main)
            {
                CreatePaymentDto.PaymentId = null;
            }

            var place = _mapper.Map<Payment>(CreatePaymentDto);
            return await _paymentRepo.AddAsync(place);
        }

        public async Task UpdateAsync(CreatePaymentDto CreatePaymentDto)
        {
            var existingpayment = await _paymentRepo.GetByIdAsync(CreatePaymentDto.Id);

            if (existingpayment == null)
            {
                throw new PaymentNotFoundException($"payment with id {CreatePaymentDto.Id} not found.");
            }



            if (existingpayment.Main && !CreatePaymentDto.Main)
            {
                Expression<Func<Payment, bool>> filter = p => (p.PaymentId == existingpayment.Id);
                var checkMainpaymentAssign = await _paymentRepo.GetByColumnsAsync(filter);
                if (checkMainpaymentAssign.Count() > 0)
                {
                    throw new AssignedMainPaymentException($"this is main payment that assigned to another places so remove assighnes payments first");
                }
            }

            var paymentToUpdate = _mapper.Map(CreatePaymentDto, existingpayment);

            await _paymentRepo.UpdateAsync(paymentToUpdate);

        }

        public async Task DeleteAsync(int id)
        {
            var existingpayment = await _paymentRepo.GetByIdAsync(id);

            if (existingpayment == null)
            {
                throw new PaymentNotFoundException($"payment with id {id} not found.");
            }

            Expression<Func<Payment, bool>> filter = p => (p.PaymentId == existingpayment.Id);
            var payments = await _paymentRepo.GetByColumnsAsync(filter);

            if (payments.Count() > 0)
            {
                throw new ConstraintPaymentErrorException($"this payment is refference to another types so delete other first");
            }

            await _paymentRepo.DeleteAsync(existingpayment);
        }


    }
}

