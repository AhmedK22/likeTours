using FluentValidation;
using LikeTours.Data.Common.AppConstants;
using LikeTours.Data.DTO.AboutUs;

namespace LikeTours.Validators.aboutUs
{
    public class UpdateAboutUsDtoValidator : AbstractValidator<UpdateAboutUsDto>
    {


        public UpdateAboutUsDtoValidator()
        {


            RuleFor(x => x.WhoAreText)
                .NotEmpty().WithMessage("WhoAreText is required.")
                .Length(3, 100).WithMessage("WhoAreText must be between 3 and 100 characters.");

            RuleFor(x => x.Lang)
                .Must(BeAValidLang).WithMessage("Lang must be a valid value from AppLangs.");

            RuleFor(x => x.AboutId)
               .NotEmpty()
               .When(x => !x.Main)
               .WithMessage("AboutId is required when Main is false.");


        }

        private bool BeAValidLang(string lang)
        {
            return AppConstants.AppLangs.Contains(lang);
        }
    }
}
