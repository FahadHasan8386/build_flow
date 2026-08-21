using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.GetProjectMembers;

public record GetProjectMembersQuery(
    Guid ProjectId
) : IRequest<GetProjectMembersResponse>;
