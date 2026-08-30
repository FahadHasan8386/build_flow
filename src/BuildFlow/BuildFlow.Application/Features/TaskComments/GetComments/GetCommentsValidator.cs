using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.GetComments;

public class GetCommentsValidator
: AbstractValidator<GetCommentsQuery>
{
    public GetCommentsValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty()
            .WithMessage("Task ID is required.");
    }
}