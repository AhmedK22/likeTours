using FluentValidation;
using LikeTours.Contracts.Repositories;
using LikeTours.Data.Common.AppConstants;
using LikeTours.Data.DTO.Place;
using LikeTours.Data.DTO.Type;
using LikeTours.Data.Enums;
using LikeTours.Data.Models;

namespace LikeTours.Validators.Types
{
    public class CreateTypeValidator:AbstractValidator<CreateTypeDto>
    {
        public CreateTypeValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(3, 100).WithMessage("Name must be between 3 and 100 characters.");


            RuleFor(x => x.Lang)
                  .Must(BeAValidLang).WithMessage("Lang must be a valid value from AppLangs.");

            RuleFor(x => x.TourTypeId)
               .NotEmpty()
               .When(x => !x.Main)
               .WithMessage("TourTypeId is required when Main is false.");
        }

        private bool BeAValidLang(string lang)
        {
            return AppConstants.AppLangs.Contains(lang);
        }


    }
}
