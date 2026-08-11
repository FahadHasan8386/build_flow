using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Projects.DeleteProject;

public record DeleteProjectCommand (Guid ProjectId) : IRequest<DeleteProjectResponse>;
