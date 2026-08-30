using BuildFlow.Domain.Entities;
using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.AddComments
{
    public class AddCommentResponse : ApiResponse
    {
        public TaskComment? Comment { get; set; }
    }
}
