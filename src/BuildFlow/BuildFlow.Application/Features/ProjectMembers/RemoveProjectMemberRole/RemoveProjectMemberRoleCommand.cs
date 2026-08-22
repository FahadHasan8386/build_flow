using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.RemoveProjectMemberRole;

public record RemoveProjectMemberRoleCommand(Guid ProjectId,Guid UserId,
RemoveProjectMemberRoleRequest Request) : IRequest<RemoveProjectMemberRoleResponse>;
