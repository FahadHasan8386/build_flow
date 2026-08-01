using System;
using System.Collections.Generic;
using System.Text;
using BuildFlow.Domain.Entities;

namespace BuildFlow.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task CreateAsync(RefreshToken refreshToken);

    Task<RefreshToken?> GetByTokenAsync(string token);

    Task RevokeAsync(string token);
}
