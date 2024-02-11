using Customer.BusinessLogic.DTOs;
using FluentValidation;

namespace Customer.BusinessLogic.Validators
{
    public class UpsertCustomerValidator : AbstractValidator<UpsertCustomerDto>
    {
        public UpsertCustomerValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(c => c.LastName).NotEmpty().MaximumLength(100);
            RuleFor(c => c.EmailAddress).NotEmpty().MaximumLength(50).EmailAddress();
            RuleFor(c => c.Age).NotEmpty();
        }
    }
}
