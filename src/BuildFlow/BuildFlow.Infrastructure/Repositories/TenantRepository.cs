using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Domain.Entities;
using BuildFlow.Infrastructure.Persistence;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BuildFlow.Infrastructure.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public TenantRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> CreateAsync(Tenant tenant , IDbConnection connection,IDbTransaction transaction)
    {

        string sql = @"INSERT INTO Tenants(Id,Name,Slug,CreatedBy,CreatedAt,ModifiedBy,ModifiedAt, InActive)
                       VALUES(@Id,@Name,@Slug,@CreatedBy,@CreatedAt,@ModifiedBy,@ModifiedAt,@InActive);";

        await connection.ExecuteAsync(sql, tenant , transaction);

        return tenant.Id;
    }

    public async Task<Tenant?> GetByIdAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = "SELECT * FROM Tenants WHERE Id=@Id";

        return await connection.QueryFirstOrDefaultAsync<Tenant>(
            sql,
            new { Id = id });
    }

    public async Task<Tenant?> GetBySlugAsync(string slug)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = "SELECT * FROM Tenants WHERE Slug=@Slug";

        return await connection.QueryFirstOrDefaultAsync<Tenant>(
            sql,
            new { Slug = slug });
    }

    public async Task<bool> ExistsBySlugAsync(string slug)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = "SELECT COUNT(1) FROM Tenants WHERE Slug=@Slug";

        return await connection.ExecuteScalarAsync<int>(sql,new { Slug = slug }) > 0;
    }

}
