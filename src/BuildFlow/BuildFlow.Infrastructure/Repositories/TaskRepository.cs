using BuildFlow.Application.Interfaces.Persistence;
using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Domain.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Infrastructure.Repositories; 

public class TaskRepository : ITaskRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TaskRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid taskId, Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"SELECT Id,TenantId,ProjectId,Title,Description,Status,
                    Priority,AssignedToUserId,DueDate,CreatedAt,CreatedBy,
                    ModifiedAt, ModifiedBy,IsDeleted
                FROM Tasks
                WHERE Id = @TaskId
                  AND TenantId = @TenantId
                  AND IsDeleted = 0 ";

        return await connection.QuerySingleOrDefaultAsync<TaskItem>(
            sql,new
            {
                TaskId = taskId,
                TenantId = tenantId
            });
    }

    public async Task<IEnumerable<TaskItem>> GetByProjectAsync(
        Guid projectId,
        Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"SELECTId,TenantId, ProjectId,Title,Description,Status,Priority,AssignedToUserId,DueDate,
                    CreatedAt,CreatedBy,ModifiedAt,ModifiedBy,IsDeleted
                    FROM Tasks
                    WHERE ProjectId = @ProjectId
                      AND TenantId = @TenantId
                      AND IsDeleted = 0
                    ORDER BY CreatedAt DESC
                    ";

        return await connection.QueryAsync<TaskItem>( sql,new
            {
                ProjectId = projectId,
                TenantId = tenantId
            });
    }
    public async Task AddAsync(TaskItem task)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"INSERT INTO Tasks(
                Id,TenantId,ProjectId,Title, Description,Status,Priority,AssignedToUserId,
                DueDate,CreatedAt,CreatedBy, IsDeleted )VALUES
            (
                @Id,@TenantId,@ProjectId, @Title,@Description,@Status, @Priority,@AssignedToUserId,@DueDate,
                @CreatedAt,@CreatedBy,0)";

        await connection.ExecuteAsync(sql, task);
    }

    public async Task UpdateAsync(TaskItem task)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
                        UPDATE Tasks
                        SET
                            Title = @Title,
                            Description = @Description,
                            Status = @Status,
                            Priority = @Priority,
                            AssignedToUserId = @AssignedToUserId,
                            DueDate = @DueDate,
                            ModifiedAt = @ModifiedAt,
                            ModifiedBy = @ModifiedBy
                        WHERE Id = @Id
                          AND TenantId = @TenantId
                          AND IsDeleted = 0";

        await connection.ExecuteAsync(sql, task);
    }

    public async Task DeleteAsync(Guid taskId,Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @" UPDATE Tasks
                        SET
                            IsDeleted = 1,
                            ModifiedAt = @ModifiedAt
                        WHERE Id = @TaskId
                          AND TenantId = @TenantId
                          AND IsDeleted = 0";

        await connection.ExecuteAsync( sql,new
            {
                TaskId = taskId,
                TenantId = tenantId,
                ModifiedAt = DateTime.UtcNow
            });
    }
}
