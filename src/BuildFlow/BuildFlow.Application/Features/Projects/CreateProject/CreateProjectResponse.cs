using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Projects.CreateProject;

public class CreateProjectResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid ProjectId { get; set; }
}
