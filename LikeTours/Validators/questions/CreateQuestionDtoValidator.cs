using FluentValidation;
using LikeTours.Data.Common.AppConstants;
using LikeTours.Data.DTO.Question;

namespace LikeTours.Validators.questions
{
    public class CreateQuestionDtoValidator:AbstractValidator<CreateQuestionDto>
    {
        public CreateQuestionDtoValidator()
        {


            RuleFor(x => x.Question)
                .NotEmpty().WithMessage("Question is required.");

               

            RuleFor(x => x.Lang)
                 .NotEmpty().WithMessage("Lang is required.")
                .Must(BeAValidLang).WithMessage("Lang must be a valid value from AppLangs.");

            RuleFor(x => x.QuestionId)
               .NotEmpty()
               .When(x => !x.Main)
               .WithMessage("QuestionId is required when Main is false.");


        }

        private bool BeAValidLang(string lang)
        {
            return AppConstants.AppLangs.Contains(lang);
        }

    }
}
