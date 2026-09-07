using BuildFlow.Application.Interfaces.Persistence;
using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Domain.Entities;
using BuildFlow.Infrastructure.Persistence;
using Dapper;
using System.Data;

namespace BuildFlow.Infrastructure.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserRoleRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> CreateAsync(UserRole userRole, IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"
            INSERT INTO UserRoles (Id, UserId, RoleId, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt, IsDeleted)
            VALUES (@Id, @UserId, @RoleId, @CreatedBy, @CreatedAt, @ModifiedBy, @ModifiedAt, @IsDeleted);
        ";

        await connection.ExecuteAsync(sql, userRole, transaction);
        return userRole.Id;
    }

    public async Task<Role?> GetUserRoleAsync(
        Guid userId,
        Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @" SELECT r.*
                    FROM UserRoles ur
                    INNER JOIN Roles r
                        ON ur.RoleId = r.Id
                    WHERE ur.UserId = @UserId
                      AND r.TenantId = @TenantId
                      AND ur.IsDeleted = 0
                      AND r.IsDeleted = 0";

        return await connection.QueryFirstOrDefaultAsync<Role>(
            sql,
            new
            {
                UserId = userId,
                TenantId = tenantId
            });
    }
}
