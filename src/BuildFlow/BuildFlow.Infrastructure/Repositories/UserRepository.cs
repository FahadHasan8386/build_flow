using BuildFlow.Application.Interfaces.Persistence;
using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Domain.Entities;
using BuildFlow.Infrastructure.Persistence;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;

namespace BuildFlow.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> CreateAsync(User user , IDbConnection connection,IDbTransaction transaction)
    {

        const string sql = @"INSERT INTO Users(Id,TenantId,FirstName,LastName,Email,PasswordHash,
                             IsActive,CreatedBy,CreatedAt,ModifiedBy,ModifiedAt,IsDeleted)
                        VALUES
                        (@Id, @TenantId, @FirstName,@LastName, @Email,@PasswordHash, @IsActive,@CreatedBy,
                            @CreatedAt,@ModifiedBy, @ModifiedAt,@IsDeleted);";

        await connection.ExecuteAsync(sql, user , transaction);

        return user.Id;
    }

    public async Task<User?> GetByEmailAsync(string email , Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"SELECT * FROM Users WHERE Email= @Email
                             AND TenantId = @TenantId
                             AND IsDeleted = 0";

        return await connection.QueryFirstOrDefaultAsync<User>(
            sql,
            new
            {
                Email = email,
                TenantId = tenantId
            });
    }

    public async Task<User?> GetByIdAsync(Guid id , Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"SELECT * FROM Users WHERE Id = @Id
                             AND TenantId = @TenantId
                             AND IsDeleted = 0";

        return await connection.QueryFirstOrDefaultAsync<User>(
            sql,
            new { Id = id, TenantId = tenantId });
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        const string sql = @" SELECT COUNT(1)
                        FROM Users
                        WHERE Email = @Email
                          AND IsDeleted = 0";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new { Email = email }) > 0;
    }


    public async Task<bool> ExistsByEmailAsync(string email, Guid tenantId)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        const string sql = @"
        SELECT COUNT(1)
        FROM Users
        WHERE Email = @Email
          AND TenantId = @TenantId
          AND IsDeleted = 0";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                Email = email,
                TenantId = tenantId
            }) > 0;
    }

    public async Task<IEnumerable<User>> GetAllAsync(Guid tenantId)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        const string sql = @"SELECT Id, TenantId, FirstName, LastName, Email,
                        IsActive,CreatedAt, CreatedBy,ModifiedAt,ModifiedBy,IsDeleted
                    FROM Users
                    WHERE TenantId = @TenantId
                      AND IsDeleted = 0
                    ORDER BY CreatedAt DESC";

        return await connection.QueryAsync<User>(
            sql,
            new
            {
                TenantId = tenantId
            });
    }

    public async Task UpdateAsync(User user)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"UPDATE Users
                        SET FirstName = @FirstName,
                            LastName = @LastName,
                            Email = @Email,
                            ModifiedAt = @ModifiedAt,
                            ModifiedBy = @ModifiedBy
                        WHERE Id = @Id
                          AND TenantId = @TenantId
                          AND IsDeleted = 0";

        await connection.ExecuteAsync(sql, user);
    }

}
