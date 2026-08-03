using BuildFlow.Application.Features.Identity.Login;
using BuildFlow.Application.Features.Identity.Logout;
using BuildFlow.Application.Features.Identity.Profile;
using BuildFlow.Application.Features.Identity.RefreshToken;
using BuildFlow.Application.Features.Identity.RegisterTenant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildFlow.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register-tenant")]
    public async Task<IActionResult> RegisterTenant([FromBody] RegisterTenantRequest request)
    {
        var response = await _mediator.Send(new RegisterTenantCommand(request));
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _mediator.Send(new LoginCommand(request));
        return response.Success ? Ok(response) : Unauthorized(response);
    }

    [HttpPost("refresh-token")]
    [Authorize]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var response = await _mediator.Send(new RefreshTokenCommand(request));
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        var response = await _mediator.Send(new LogoutCommand(request));
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpGet("profile/{userId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetProfile(Guid userId)
    {
        var response = await _mediator.Send(new GetProfileQuery(userId));
        return Ok(response);
    }
}
