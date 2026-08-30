using BuildFlow.Domain.Entities;
using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.UpdateComment;

public class UpdateCommentResponse : ApiResponse
{
    public TaskComment? Comment { get; set; }
}
