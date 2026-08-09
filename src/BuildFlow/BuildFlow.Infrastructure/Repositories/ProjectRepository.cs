using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Domain.Entities;
using BuildFlow.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;

namespace BuildFlow.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public ProjectRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> CreateAsync(Project project)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"INSERT INTO Projects(
                Id,TenantId,Name,Description,StartDate, EndDate,IsArchived,CreatedByUserId, CreatedAt,CreatedBy,IsDeleted
                )VALUES(@Id,@TenantId,@Name,@Description,@StartDate,@EndDate,@IsArchived, @CreatedByUserId,
                @CreatedAt,@CreatedBy,@IsDeleted);";

        await connection.ExecuteAsync( sql,project);

        return project.Id;
    }
    public async Task<Project?> GetByIdAsync(Guid id, Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"SELECT * FROM Projects
                            WHERE Id = @Id
                              AND TenantId = @TenantId
                              AND IsDeleted = 0";

        return await connection.QueryFirstOrDefaultAsync<Project>(sql,
            new { Id = id , TenanatId = tenantId});
    }

    public async Task<IEnumerable<Project>> GetByTenantAsync(Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @" SELECT * FROM Projects
                            WHERE TenantId = @TenantId
                              AND IsDeleted = 0
                            ORDER BY CreatedAt DESC";

        return await connection.QueryAsync<Project>(sql,new { TenantId = tenantId });
    }

    public async Task UpdateAsync(Project project, Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"UPDATE Projects SET
                            Name = @Name,
                            Description = @Description,
                            StartDate = @StartDate,
                            EndDate = @EndDate,
                            IsArchived = @IsArchived,
                            ModifiedAt = @ModifiedAt,
                            ModifiedBy = @ModifiedBy
                        WHERE Id = @Id
                          AND TenantId = @TenantId
                          AND IsDeleted = 0
                        ";

        await connection.ExecuteAsync(sql,
                        new
                        {
                            project.Id,
                            project.TenantId,
                            project.Name,
                            project.Description,
                            project.StartDate,
                            project.EndDate,
                            project.IsArchived,
                            project.ModifiedAt,
                            project.ModifiedBy
                        });
    }

    public async Task DeleteAsync(Guid id, Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"UPDATE Projects
                            SET IsDeleted = 1
                            WHERE Id = @Id
                            AND TenantId = @TenantId
                            AND IsDeleted = 0";

        await connection.ExecuteAsync(sql,new { Id = id , TenantId = tenantId });
    }
}
