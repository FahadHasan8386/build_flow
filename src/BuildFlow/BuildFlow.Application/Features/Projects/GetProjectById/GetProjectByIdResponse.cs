using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Projects.GetProjectById;

public class GetProjectByIdResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;

    public ProjectData? Project {  get; set; }

}
