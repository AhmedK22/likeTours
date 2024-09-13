using FluentValidation;
using LikeTours.Contracts.Repositories;
using LikeTours.Data.Common.AppConstants;
using LikeTours.Data.DTO.AboutUs;
using LikeTours.Data.DTO.Place;
using LikeTours.Data.Enums;
using LikeTours.Data.Models;
using System.Linq.Expressions;

namespace LikeTours.Validators.aboutUs
{
    public class CreateAboutDtoValidator : AbstractValidator<CreateAboutUsDto>
    {


        public CreateAboutDtoValidator()
        {


            RuleFor(x => x.WhoAreText)
                .NotEmpty().WithMessage("WhoAreText is required.")
                .Length(3, 100).WithMessage("WhoAreText must be between 3 and 100 characters.");

            RuleFor(x => x.WhoAreImage)
               .NotEmpty().WithMessage("WhoAreImage is required.");

            RuleFor(x => x.WhoAreImage)
               .NotEmpty().WithMessage("WhoAreImage is required.");

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
