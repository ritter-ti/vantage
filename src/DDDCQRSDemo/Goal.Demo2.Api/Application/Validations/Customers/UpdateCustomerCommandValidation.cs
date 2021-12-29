using System;
using FluentValidation;
using Goal.Demo2.Api.Application.Commands.Customers;

namespace Goal.Demo2.Api.Application.Validations.Customers
{
    public class UpdateCustomerCommandValidation : CustomerValidation<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidation()
        {
            ValidateId();
            ValidateName();
            ValidateBirthDate();
            ValidateEmail();
        }

        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .NotEqual(Guid.Empty);
        }
    }
}