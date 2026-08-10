using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Projects.UpdateProject;

public class UpdateProjectValidator : AbstractValidator<UpdateProjectCommand>
{
     public UpdateProjectValidator()
     {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("Project ID is required.");

        RuleFor(x => x.Request.Name)
            .NotEmpty()
            .WithMessage("Project name is required.")
            .MaximumLength(200)
            .WithMessage("Project name cannot exceed 200 characters.");

        RuleFor(x => x.Request.Description)
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.Request)
            .Must(x =>
                !x.StartDate.HasValue ||
                !x.EndDate.HasValue ||
                x.StartDate <= x.EndDate)
            .WithMessage("Start date must be before or equal to end date.");
     }
}

