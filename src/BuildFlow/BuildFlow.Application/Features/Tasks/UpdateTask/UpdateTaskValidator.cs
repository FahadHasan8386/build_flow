using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Tasks.UpdateTask;

public class UpdateTaskValidator: AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty()
            .WithMessage("Task ID is required.");

        RuleFor(x => x.Request.Title)
            .NotEmpty()
            .WithMessage("Task title is required.")
            .MaximumLength(200)
            .WithMessage("Task title cannot exceed 200 characters.");

        RuleFor(x => x.Request.Description)
            .MaximumLength(2000)
            .WithMessage("Task description cannot exceed 2000 characters.");

        RuleFor(x => x.Request.DueDate)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.Request.DueDate.HasValue)
            .WithMessage("Due date must be in the future.");
    }
}
