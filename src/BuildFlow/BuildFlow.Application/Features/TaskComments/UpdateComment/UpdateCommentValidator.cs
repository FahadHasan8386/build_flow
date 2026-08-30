using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.UpdateComment;

public class UpdateCommentValidator
: AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentValidator()
    {
        RuleFor(x => x.CommentId)
            .NotEmpty()
            .WithMessage("Comment ID is required.");

        RuleFor(x => x.Request.Comment)
            .NotEmpty()
            .WithMessage("Comment is required.")
            .MaximumLength(5000)
            .WithMessage("Comment cannot exceed 5000 characters.");
    }
}
