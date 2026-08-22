using BuildFlow.Application.Features.ProjectMembers.AddProjectMember;
using BuildFlow.Application.Features.ProjectMembers.AddProjectMemberRole;
using BuildFlow.Application.Features.ProjectMembers.GetProjectMember;
using BuildFlow.Application.Features.ProjectMembers.GetProjectMembers;
using BuildFlow.Application.Features.ProjectMembers.RemoveProjectMemberRole;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildFlow.api.Controllers;

[ApiController]
[Route("api/project-members")]
[Authorize]
public class ProjectMemberController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectMemberController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddMember(
        [FromBody] AddProjectMemberRequest request)
    {
        var command = new AddProjectMemberCommand(request);

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetProjectMembers(
        Guid projectId)
    {
        var query = new GetProjectMembersQuery(projectId);

        var result = await _mediator.Send(query);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("project/{projectId}/user/{userId}")]
    public async Task<IActionResult> GetProjectMember(Guid projectId,Guid userId)
    {
        var query = new GetProjectMemberQuery(projectId, userId);

        var result = await _mediator.Send(query);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost("project/{projectId}/user/{userId}/roles")]
    public async Task<IActionResult> AddRole(Guid projectId,Guid userId,
    [FromBody] AddProjectMemberRoleRequest request)
    {
        var command = new AddProjectMemberRoleCommand(
            projectId,
            userId,
            request);

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpDelete("project/{projectId}/user/{userId}/roles")]
    public async Task<IActionResult> RemoveRole(Guid projectId,Guid userId,
    [FromBody] RemoveProjectMemberRoleRequest request)
    {
        var command = new RemoveProjectMemberRoleCommand(
            projectId,
            userId,
            request);

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

}
