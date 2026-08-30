using BuildFlow.Domain.Entities;
using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.GetCommentById;

public class GetCommentByIdResponse : ApiResponse
{
    public TaskComment? Comment { get; set; }
}
