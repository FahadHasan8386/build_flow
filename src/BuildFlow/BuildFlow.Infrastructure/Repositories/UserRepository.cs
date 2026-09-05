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
        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM Users WHERE Email=@Email",
            new { Email = email }) > 0;
    }
}
