using BuildFlow.Application.Features.TaskComments.AddComments;
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
}
