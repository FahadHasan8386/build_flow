using BuildFlow.Application.Features.Tasks.CreateTask;
using BuildFlow.Application.Features.Tasks.GetTaskById;
using BuildFlow.Application.Features.Tasks.GetTasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildFlow.api.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaskController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
    {
        var command = new CreateTaskCommand(request);

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetTasks(Guid projectId)
    {
        var query = new GetTasksQuery(projectId);

        var result = await _mediator.Send(query);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpGet("{taskId}")]
    public async Task<IActionResult> GetTaskById(Guid taskId)
    {
        var query = new GetTaskByIdQuery(taskId);

        var result = await _mediator.Send(query);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}
