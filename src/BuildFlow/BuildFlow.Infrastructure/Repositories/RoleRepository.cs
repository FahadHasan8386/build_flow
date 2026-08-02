using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Domain.Entities;
using BuildFlow.Infrastructure.Persistence;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BuildFlow.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public RoleRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> CreateAsync(Role role , IDbConnection connection,IDbTransaction transaction)
    {

        const string sql = @"INSERT INTO Roles
                            (Id,TenantId,Name,Description,IsSystemRole,CreatedBy,CreatedAt, ModifiedBy,ModifiedAt,InActive)
                            VALUES
                            ( @Id, @TenantId,@Name,@Description,@IsSystemRole,@CreatedBy,@CreatedAt, @ModifiedBy,@ModifiedAt,@InActive);";

        await connection.ExecuteAsync(sql, role , transaction);

        return role.Id;
    }

    public async Task<Role?> GetByNameAsync(Guid tenantId, string roleName)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Role>(
                    @"SELECT * FROM Roles
                      WHERE TenantId=@TenantId AND Name=@RoleName",
            new { TenantId = tenantId, RoleName = roleName });
    }

    public async Task<bool> ExistsAsync(Guid tenantId, string roleName)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
                    @"SELECT COUNT(1)
                      FROM Roles
                      WHERE TenantId=@TenantId
                      AND Name=@RoleName",
            new { TenantId = tenantId, RoleName = roleName }) > 0;
    }
}
