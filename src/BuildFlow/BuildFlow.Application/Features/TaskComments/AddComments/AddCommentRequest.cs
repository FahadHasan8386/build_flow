using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.AddComments;

public class AddCommentRequest
{
    public Guid TaskId { get; set; }

    public string Comment { get; set; } = string.Empty;
}
