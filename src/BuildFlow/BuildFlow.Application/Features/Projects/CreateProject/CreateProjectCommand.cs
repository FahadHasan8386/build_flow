using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Projects.CreateProject;

public record CreateProjectCommand( CreateProjectRequest Request) : IRequest<CreateProjectResponse>;
