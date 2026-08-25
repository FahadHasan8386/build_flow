using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.GetProjectMember;

public class GetProjectMemberResponse : ApiResponse
{
    public ProjectMemberDetails? Member { get; set; }
}
