using BuildFlow.Domain.Enums;
using FluentValidation;

namespace BuildFlow.Application.Features.ProjectMembers.AddProjectMemberRole;

public class AddProjectMemberRoleValidator : AbstractValidator<AddProjectMemberRoleRequest>
{
    public AddProjectMemberRoleValidator()
    {
        RuleFor(x => x.Role)
            .Must(role => Enum.IsDefined(
                typeof(ProjectMemberRoleType),
                role))
            .WithMessage("Invalid project member role.");
    }
}