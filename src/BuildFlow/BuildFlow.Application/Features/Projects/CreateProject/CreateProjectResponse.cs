using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Projects.CreateProject;

public class CreateProjectResponse : ApiResponse
{
    public Guid ProjectId { get; set; }
}
