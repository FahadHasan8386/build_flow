using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.GetCommentById;

public class GetCommentByIdValidator : AbstractValidator<GetCommentByIdQuery>
{
    public GetCommentByIdValidator()
    {
        RuleFor(x => x.CommentId)
            .NotEmpty()
            .WithMessage("Comment ID is required.");
    }
}
