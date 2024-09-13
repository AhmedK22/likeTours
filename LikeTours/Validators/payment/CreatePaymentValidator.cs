using FluentValidation;
using LikeTours.Data.Common.AppConstants;
using LikeTours.Data.DTO.Payment;
using LikeTours.Data.DTO.Place;

namespace LikeTours.Validators.payment
{
    public class CreatePaymentValidator : AbstractValidator<CreatePaymentDto>
    {


        public CreatePaymentValidator()
        {


            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.");

            RuleFor(x => x.Desc)
              .NotEmpty().WithMessage("Desc is required.");

            RuleFor(x => x.Lang)
                .Must(BeAValidLang).WithMessage("Lang must be a valid value from AppLangs.");

            RuleFor(x => x.PaymentId)
               .NotEmpty()
               .When(x => !x.Main)
               .WithMessage("PlaceId is required when Main is false.");


        }

        private bool BeAValidLang(string lang)
        {
            return AppConstants.AppLangs.Contains(lang);
        }
    }
}
