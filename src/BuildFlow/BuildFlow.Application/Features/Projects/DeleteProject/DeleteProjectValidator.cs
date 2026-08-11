using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Projects.DeleteProject;

public class DeleteProjectValidator
: AbstractValidator<DeleteProjectCommand>
{
    public DeleteProjectValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("Project ID is required.");
    }
}
