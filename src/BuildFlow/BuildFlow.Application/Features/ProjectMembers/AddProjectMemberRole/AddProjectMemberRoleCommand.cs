using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.AddProjectMemberRole;

public record AddProjectMemberRoleCommand(
    Guid ProjectId,
    Guid UserId, AddProjectMemberRoleRequest Request) : IRequest<AddProjectMemberRoleResponse>;