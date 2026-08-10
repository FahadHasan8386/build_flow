using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Projects.UpdateProject;

public record UpdateProjectCommand(Guid ProjectId,UpdateProjectRequest Request) : IRequest<UpdateProjectResponse>;
