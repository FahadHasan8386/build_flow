using BuildFlow.Domain.Entities;
using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Tasks.AssignTask;

public class AssignTaskResponse : ApiResponse   
{
    public TaskItem? Task { get; set; }
}
