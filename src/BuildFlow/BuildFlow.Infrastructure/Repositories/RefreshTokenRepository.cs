using BuildFlow.Application.Interfaces.Persistence;
using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Domain.Entities;
using BuildFlow.Infrastructure.Persistence;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BuildFlow.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public RefreshTokenRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task CreateAsync(RefreshToken refreshToken, IDbConnection connection,
    IDbTransaction transaction)
    {

        const string sql = @"
                            INSERT INTO RefreshTokens
                            (Id, UserId,Token, ExpiresAt, RevokedAt,CreatedBy,CreatedAt,ModifiedBy,ModifiedAt,InActive)
                            VALUES
                            (@Id,@UserId,@Token,@ExpiresAt,@RevokedAt,@CreatedBy,@CreatedAt,@ModifiedBy,@ModifiedAt,@IsDeleted);";

        await connection.ExecuteAsync(sql, refreshToken , transaction);
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
