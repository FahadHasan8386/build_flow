using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.DeleteComment;

public class DeleteCommentValidator : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentValidator()
    {
        RuleFor(x => x.CommentId)
            .NotEmpty()
            .WithMessage("Comment ID is required.");
    }
}
