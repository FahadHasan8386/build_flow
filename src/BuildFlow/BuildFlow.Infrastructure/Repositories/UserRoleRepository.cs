using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Domain.Entities;
using BuildFlow.Infrastructure.Persistence;
using Dapper;
using System.Data;

namespace BuildFlow.Infrastructure.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public UserRoleRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> CreateAsync(UserRole userRole)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            INSERT INTO UserRoles (Id, UserId, RoleId, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt, InActive)
            VALUES (@Id, @UserId, @RoleId, @CreatedBy, @CreatedAt, @ModifiedBy, @ModifiedAt, @IsDeleted);
        ";

        await connection.ExecuteAsync(sql, userRole);
        return userRole.Id;
    }
}
