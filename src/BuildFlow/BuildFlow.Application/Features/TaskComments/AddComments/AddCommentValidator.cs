using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.AddComments;

public class AddCommentValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentValidator()
    {
        RuleFor(x => x.Request.TaskId)
            .NotEmpty()
            .WithMessage("Task ID is required.");

        RuleFor(x => x.Request.Comment)
            .NotEmpty()
            .WithMessage("Comment is required.")
            .MaximumLength(5000)
            .WithMessage("Comment cannot exceed 5000 characters.");
    }
}
