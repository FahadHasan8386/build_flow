using BuildFlow.Domain.Entities;
using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.GetComments;

public class GetCommentsResponse : ApiResponse
{
    public IEnumerable<TaskComment> Comments { get; set; } = new List<TaskComment>();
}
