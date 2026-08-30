using BuildFlow.Application.Interfaces.Persistence;
using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Domain.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Infrastructure.Repositories;

public class TaskCommentRepository : ITaskCommentRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TaskCommentRepository(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task AddAsync(TaskComment comment)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"INSERT INTO TaskComments(
                Id,TenantId,TaskId,UserId,Comment,CreatedAt,CreatedBy,IsDeleted)
                VALUES
                (
                    @Id, @TenantId,@TaskId,@UserId, @Comment,@CreatedAt,@CreatedBy,0)";

        await connection.ExecuteAsync(sql, comment);
    }

    public async Task<IEnumerable<TaskComment>> GetByTaskAsync(
        Guid taskId,
        Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"SELECT
                        Id,TenantId,TaskId,UserId, Comment,CreatedAt,CreatedBy,ModifiedAt,ModifiedBy,IsDeleted
                    FROM TaskComments
                    WHERE TaskId = @TaskId
                      AND TenantId = @TenantId
                      AND IsDeleted = 0
                    ORDER BY CreatedAt ASC";

        return await connection.QueryAsync<TaskComment>(
            sql,
            new
            {
                TaskId = taskId,
                TenantId = tenantId
            });
    }

    public async Task<TaskComment?> GetByIdAsync(Guid commentId,Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"SELECT
                        Id,TenantId,TaskId, UserId,Comment,CreatedAt,CreatedBy,ModifiedAt,ModifiedBy,IsDeleted
                    FROM TaskComments
                    WHERE Id = @CommentId
                      AND TenantId = @TenantId
                      AND IsDeleted = 0";

        return await connection.QuerySingleOrDefaultAsync<TaskComment>(
            sql,
            new
            {
                CommentId = commentId,
                TenantId = tenantId
            });
    }

    public async Task UpdateAsync(TaskComment comment)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            UPDATE TaskComments
            SET
                Comment = @Comment,
                ModifiedAt = @ModifiedAt,
                ModifiedBy = @ModifiedBy
            WHERE Id = @Id
              AND TenantId = @TenantId
              AND IsDeleted = 0";

        await connection.ExecuteAsync(sql, comment);
    }

    public async Task DeleteAsync(Guid commentId,Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @" UPDATE TaskComments SET
                IsDeleted = 1,
                ModifiedAt = @ModifiedAt
            WHERE Id = @CommentId
              AND TenantId = @TenantId
              AND IsDeleted = 0";

        await connection.ExecuteAsync(
            sql,
            new
            {
                CommentId = commentId,
                TenantId = tenantId,
                ModifiedAt = DateTime.UtcNow
            });
    }
} 