using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Projects.GetProjectById;

public class GetProjectByIdResponse : ApiResponse
{
    public ProjectData? Project {  get; set; }

}
