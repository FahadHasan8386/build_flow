using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Domain.Entities;
using BuildFlow.Infrastructure.Persistence;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public RefreshTokenRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task CreateAsync(RefreshToken refreshToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
                            INSERT INTO RefreshTokens
                            (Id, UserId,Token, ExpiresAt, RevokedAt,CreatedBy,CreatedAt,ModifiedBy,ModifiedAt,InActive)
                            VALUES
                            (@Id,@UserId,@Token,@ExpiresAt,@RevokedAt,@CreatedBy,@CreatedAt,@ModifiedBy,@ModifiedAt,@InActive);";

        await connection.ExecuteAsync(sql, refreshToken);
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<RefreshToken>(
            "SELECT * FROM RefreshTokens WHERE Token=@Token",
            new { Token = token });
    }

    public async Task RevokeAsync(string token)
    {
        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(
            @"UPDATE RefreshTokens
              SET RevokedAt = GETUTCDATE()
              WHERE Token=@Token",
            new { Token = token });
    }
}
