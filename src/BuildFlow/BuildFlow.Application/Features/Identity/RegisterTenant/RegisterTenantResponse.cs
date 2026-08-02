using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Identity.RegisterTenant;

public class RegisterTenantResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid TenantId { get; set; }

    public Guid UserId { get; set; }

    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;
}
