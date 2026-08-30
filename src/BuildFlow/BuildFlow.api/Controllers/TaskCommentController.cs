using BuildFlow.Application.Features.TaskComments.AddComments;
using BuildFlow.Application.Features.TaskComments.GetCommentById;
using BuildFlow.Application.Features.TaskComments.GetComments;
using BuildFlow.Application.Features.TaskComments.UpdateComment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildFlow.api.Controllers;

[ApiController]
[Route("api/task-comments")]
[Authorize]
public class TaskCommentController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaskCommentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(
        [FromBody] AddCommentRequest request)
    {
        var command = new AddCommentCommand(request);

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("task/{taskId}")]
    public async Task<IActionResult> GetComments(Guid taskId)
    {
        var query = new GetCommentsQuery(taskId);

        var result = await _mediator.Send(query);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpGet("{commentId}")]
    public async Task<IActionResult> GetCommentById(Guid commentId)
    {
        var query = new GetCommentByIdQuery(commentId);

        var result = await _mediator.Send(query);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPut("{commentId}")]
    public async Task<IActionResult> UpdateComment(
    Guid commentId,
    [FromBody] UpdateCommentRequest request)
    {
        var command = new UpdateCommentCommand(
            commentId,
            request);

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
