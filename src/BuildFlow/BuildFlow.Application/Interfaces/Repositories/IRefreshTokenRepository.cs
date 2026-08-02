using BuildFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BuildFlow.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task CreateAsync(RefreshToken refreshToken,IDbConnection connection,IDbTransaction transaction);

    Task<RefreshToken?> GetByTokenAsync(string token);

    Task RevokeAsync(string token);
}
