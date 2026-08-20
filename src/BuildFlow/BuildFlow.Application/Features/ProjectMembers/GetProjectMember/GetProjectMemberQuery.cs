using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.GetProjectMember;

public class GetProjectMemberQuery(Guid ProjectId,Guid UserId) : IRequest<GetProjectMemberResponse>;
