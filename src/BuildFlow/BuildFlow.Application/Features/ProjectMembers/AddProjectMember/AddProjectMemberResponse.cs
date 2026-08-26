using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.AddProjectMember;

public class AddProjectMemberResponse : ApiResponse
{

    public Guid ProjectMemberId { get; set; }
}
