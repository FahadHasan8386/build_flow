using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Projects.GetProjectById;

public record GetProjectByIdQuery(Guid ProjectId) : IRequest<GetProjectByIdResponse>;

