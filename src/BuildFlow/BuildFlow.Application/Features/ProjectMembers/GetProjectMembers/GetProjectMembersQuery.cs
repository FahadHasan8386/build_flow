using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.GetProjectMembers;

public class GetProjectMemberQuery(Guid ProjectId,Guid UserId) : IRequest<GetProjectMemberResponse>;
