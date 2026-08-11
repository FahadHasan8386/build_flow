using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Projects.DeleteProject;

public class DeleteProjectResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;
}
