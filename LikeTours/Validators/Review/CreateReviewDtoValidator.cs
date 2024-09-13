using FluentValidation;
using LikeTours.Data.Common.AppConstants;
using LikeTours.Data.DTO.Question;
using LikeTours.Data.DTO.Review;

namespace LikeTours.Validators.Review
{
    public class CreateReviewDtoValidator : AbstractValidator<CreateReviewDto>
    {
        public CreateReviewDtoValidator()
        {


            RuleFor(x => x.Header)
                .NotEmpty().WithMessage("Header is required.");
            RuleFor(x => x.Body)
              .NotEmpty().WithMessage("Body is required.");

            RuleFor(x => x.Lang)
                 .NotEmpty().WithMessage("Lang is required.")
                .Must(BeAValidLang).WithMessage("Lang must be a valid value from AppLangs.");

            RuleFor(x => x.ReviewId)
               .NotEmpty()
               .When(x => !x.Main)
               .WithMessage("ReviewId is required when Main is false.");


        }

        private bool BeAValidLang(string lang)
        {
            return AppConstants.AppLangs.Contains(lang);
        }

    }
}
