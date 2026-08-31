using BuildFlow.Application.Interfaces.Persistence;
using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Domain.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public NotificationRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task AddAsync(Notification notification)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"INSERT INTO Notifications(
                Id,TenantId,UserId, Type,Title,Message,RelatedEntityId,RelatedEntityType,
                IsRead,CreatedAt,CreatedBy,IsDeleted)
            VALUES(
                @Id,@TenantId,@UserId,@Type,@Title, @Message,
                @RelatedEntityId,@RelatedEntityType,0,@CreatedAt,
                @CreatedBy,0)";

        await connection.ExecuteAsync(sql, notification);
    }

    public async Task<IEnumerable<Notification>> GetByUserAsync(
        Guid userId,
        Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"SELECT
                Id,TenantId,UserId,Type,Title,Message,RelatedEntityId,RelatedEntityType,
                IsRead,CreatedAt, CreatedBy,  ModifiedAt,ModifiedBy,IsDeleted
                FROM Notifications
                WHERE UserId = @UserId
                  AND TenantId = @TenantId
                  AND IsDeleted = 0
                ORDER BY CreatedAt DESC";

        return await connection.QueryAsync<Notification>(
            sql,
            new
            {
                UserId = userId,
                TenantId = tenantId
            });
    }

    public async Task<Notification?> GetByIdAsync(
        Guid notificationId,
        Guid userId,
        Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"SELECT
                Id, TenantId,UserId,Type,Title, Message, RelatedEntityId,RelatedEntityType,
                IsRead, CreatedAt,CreatedBy, ModifiedAt, ModifiedBy,IsDeleted
                FROM Notifications
                WHERE Id = @NotificationId
                  AND UserId = @UserId
                  AND TenantId = @TenantId
                  AND IsDeleted = 0";

        return await connection.QuerySingleOrDefaultAsync<Notification>(
            sql,
            new
            {
                NotificationId = notificationId,
                UserId = userId,
                TenantId = tenantId
            });
    }

    public async Task MarkAsReadAsync(
        Guid notificationId,
        Guid userId,
        Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"UPDATE Notifications
                        SET
                            IsRead = 1,
                            ModifiedAt = @ModifiedAt,
                            ModifiedBy = @ModifiedBy
                        WHERE Id = @NotificationId
                          AND UserId = @UserId
                          AND TenantId = @TenantId
                          AND IsDeleted = 0";

        await connection.ExecuteAsync(
            sql,
            new
            {
                NotificationId = notificationId,
                UserId = userId,
                TenantId = tenantId,
                ModifiedAt = DateTime.UtcNow,
                ModifiedBy = userId.ToString()
            });
    }

    public async Task MarkAllAsReadAsync(
        Guid userId,
        Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @" UPDATE Notifications
                    SET
                        IsRead = 1,
                        ModifiedAt = @ModifiedAt,
                        ModifiedBy = @ModifiedBy
                    WHERE UserId = @UserId
                      AND TenantId = @TenantId
                      AND IsRead = 0
                      AND IsDeleted = 0";

        await connection.ExecuteAsync(
            sql,
            new
            {
                UserId = userId,
                TenantId = tenantId,
                ModifiedAt = DateTime.UtcNow,
                ModifiedBy = userId.ToString()
            });
    }

    public async Task DeleteAsync(
        Guid notificationId,
        Guid userId,
        Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @" UPDATE Notifications
                        SET
                            IsDeleted = 1,
                            ModifiedAt = @ModifiedAt,
                            ModifiedBy = @ModifiedBy
                        WHERE Id = @NotificationId
                          AND UserId = @UserId
                          AND TenantId = @TenantId
                          AND IsDeleted = 0";

        await connection.ExecuteAsync(
            sql,
            new
            {
                NotificationId = notificationId,
                UserId = userId,
                TenantId = tenantId,
                ModifiedAt = DateTime.UtcNow,
                ModifiedBy = userId.ToString()
            });
    }
}


