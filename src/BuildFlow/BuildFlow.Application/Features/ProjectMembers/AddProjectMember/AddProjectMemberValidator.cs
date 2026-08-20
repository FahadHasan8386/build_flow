using BuildFlow.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.AddProjectMember;

public class AddProjectMemberValidator : AbstractValidator<AddProjectMemberRequest>
{
    public AddProjectMemberValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("ProjectId is required.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(x => x.Roles)
            .NotEmpty()
            .WithMessage("At least one role is required.");

        RuleForEach(x => x.Roles)
            .Must(role => Enum.IsDefined(
                typeof(ProjectMemberRoleType),
                role))
            .WithMessage("Invalid project member role.");

        RuleFor(x => x.Roles)
            .Must(roles => roles.Distinct().Count() == roles.Count)
            .WithMessage("Duplicate roles are not allowed.");
    }
}
