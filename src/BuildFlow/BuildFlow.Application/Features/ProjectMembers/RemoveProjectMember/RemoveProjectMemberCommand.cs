using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.RemoveProjectMember;

public record RemoveProjectMemberCommand(Guid ProjectId,Guid UserId) : IRequest<RemoveProjectMemberResponse>;
