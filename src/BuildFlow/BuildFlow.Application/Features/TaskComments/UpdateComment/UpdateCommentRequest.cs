using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.UpdateComment;

public class UpdateCommentRequest
{
    public string Comment { get; set; } = string.Empty;
}
