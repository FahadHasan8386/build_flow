using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Projects.GetProjects;

public class GetProjectsResponse : ApiResponse
{
    public IEnumerable<ProjectData> Projects { get; set; }= new List<ProjectData>();
}
