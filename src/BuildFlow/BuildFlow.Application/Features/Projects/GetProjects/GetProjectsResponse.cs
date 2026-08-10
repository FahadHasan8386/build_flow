using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Projects.GetProjects;

public class GetProjectsResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public IEnumerable<ProjectData> Projects { get; set; }= new List<ProjectData>();
}
