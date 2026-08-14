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
            INSERT INTO UserRoles (Id, UserId, RoleId, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt, InActive)
            VALUES (@Id, @UserId, @RoleId, @CreatedBy, @CreatedAt, @ModifiedBy, @ModifiedAt, @IsDeleted);
        ";

        await connection.ExecuteAsync(sql, userRole, transaction);
        return userRole.Id;
    }
}
