using BuildFlow.Application.Features.Users.CreateUser;
using BuildFlow.Application.Features.Users.GetUserById;
using BuildFlow.Application.Features.Users.GetUsers;
using BuildFlow.Application.Features.Users.UpdateUser;
using BuildFlow.Application.Features.Users.UpdateUserStatus;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildFlow.api.Controllers
{

    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var result = await _mediator.Send(
                new CreateUserCommand(request));

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _mediator.Send(
                new GetUsersQuery());

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var result = await _mediator.Send(
                new GetUserByIdQuery(userId));

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPut("{userId:guid}")]
        public async Task<IActionResult> UpdateUser(Guid userId,[FromBody] UpdateUserRequest request)
        {
            var result = await _mediator.Send(  new UpdateUserCommand(userId, request));

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPatch("{userId:guid}/status")]
        public async Task<IActionResult> UpdateUserStatus(Guid userId,[FromBody] UpdateUserStatusRequest request)
        {
            var result = await _mediator.Send(
                new UpdateUserStatusCommand(userId, request));

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
