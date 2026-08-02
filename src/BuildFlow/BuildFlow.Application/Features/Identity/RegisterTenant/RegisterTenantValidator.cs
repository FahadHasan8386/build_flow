using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace BuildFlow.Application.Features.Identity.RegisterTenant
{
    public class RegisterTenantValidator : AbstractValidator<RegisterTenantRequest>
    {
        public RegisterTenantValidator()
        {
            RuleFor(x => x.CompanyName)
            .NotEmpty()
            .MaximumLength(200);

            RuleFor(x => x.CompanySlug)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);
        }
    }
}
