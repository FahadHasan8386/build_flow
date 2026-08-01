using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Domain.Entities;
using BuildFlow.Infrastructure.Persistence;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public UserRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> CreateAsync(User user)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"INSERT INTO Users(Id,TenantId,FirstName,LastName,Email,PasswordHash,
                             IsActive,CreatedBy,CreatedAt,ModifiedBy,ModifiedAt,InActive)
                        VALUES
                        (@Id, @TenantId, @FirstName,@LastName, @Email,@PasswordHash, @IsActive,@CreatedBy,
                            @CreatedAt,@ModifiedBy, @ModifiedAt,@InActive);";

        await connection.ExecuteAsync(sql, user);

        return user.Id;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE Email=@Email",
            new { Email = email });
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE Id=@Id",
            new { Id = id });
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM Users WHERE Email=@Email",
            new { Email = email }) > 0;
    }
}
