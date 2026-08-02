using BuildFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Interfaces.Security;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user , string role);

    RefreshToken GenerateRefreshToken(Guid userId);
}
