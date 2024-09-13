using FluentValidation;
using LikeTours.Data.DTO.Contact;

namespace LikeTours.Validators.Contacts
{
    public class AddContactValidator : AbstractValidator<CreateContactDto>
    {
        public AddContactValidator()
        {
            RuleFor(x => x.Name)
              .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.ContactWay)
              .NotEmpty().WithMessage("Contact is required.");

        }
    }
}
