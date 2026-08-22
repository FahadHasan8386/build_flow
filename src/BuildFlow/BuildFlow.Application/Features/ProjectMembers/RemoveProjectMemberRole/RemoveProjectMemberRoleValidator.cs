using BuildFlow.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.RemoveProjectMemberRole;

public class RemoveProjectMemberRoleValidator
    : AbstractValidator<RemoveProjectMemberRoleRequest>
{
    public RemoveProjectMemberRoleValidator()
    {
        RuleFor(x => x.Role)
            .Must(role => Enum.IsDefined(
                typeof(ProjectMemberRoleType),
                role))
            .WithMessage("Invalid project member role.");
    }
}
