using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Projects.GetProjects;

public record GetProjectsQuery : IRequest<GetProjectsResponse>;
