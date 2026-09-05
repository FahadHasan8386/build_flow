using BuildFlow.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BuildFlow.Infrastructure.Security;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public Guid UserId =>
        GetGuidClaim(ClaimTypes.NameIdentifier);

    public Guid TenantId =>
        GetGuidClaim("TenantId");

    public bool IsInRole(string role)
    {
        return _httpContextAccessor.HttpContext?
            .User?
            .IsInRole(role) ?? false;
    }

    private Guid GetGuidClaim(string claimType)
    {
        var value = _httpContextAccessor.HttpContext?
            .User?
            .FindFirst(claimType)?
            .Value;

        return Guid.TryParse(value, out var id)
            ? id
            : Guid.Empty;
    }
}
