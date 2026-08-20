using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.AddProjectMember;

public record AddProjectMemberCommand(AddProjectMemberRequest Request) : IRequest<AddProjectMemberResponse>;
