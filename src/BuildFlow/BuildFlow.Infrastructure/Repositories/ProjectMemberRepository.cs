using BuildFlow.Application.Interfaces.Persistence;
using BuildFlow.Domain.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Infrastructure.Repositories;

public class ProjectMemberRepository : IProjectMemberRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ProjectMemberRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> AddMemberAsync(ProjectMember member)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"INSERT INTO ProjectMembers(Id,ProjectId,UserId,TenantId,CreatedAt,CreatedBy,IsDeleted)
                VALUES(@Id,@ProjectId,@UserId, @TenantId, @CreatedAt,@CreatedBy,@IsDeleted);";

        await connection.ExecuteAsync(sql, member);

        return member.Id;
    }
    public async Task<ProjectMember?> GetMemberAsync(Guid projectId,Guid userId,Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"SELECT *FROM ProjectMembers
            WHERE ProjectId = @ProjectId
              AND UserId = @UserId
              AND TenantId = @TenantId
              AND IsDeleted = 0;";

        return await connection.QueryFirstOrDefaultAsync<ProjectMember>(sql,new
            {
                ProjectId = projectId,
                UserId = userId,
                TenantId = tenantId
            });
    }

    public async Task<IEnumerable<ProjectMember>> GetMembersByProjectAsync(Guid projectId,
        Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"SELECT * FROM ProjectMembers
            WHERE ProjectId = @ProjectId
              AND TenantId = @TenantId
              AND IsDeleted = 0
            ORDER BY CreatedAt DESC;";

        return await connection.QueryAsync<ProjectMember>(sql,new
            {
                ProjectId = projectId,
                TenantId = tenantId
            });
    }

    public async Task RemoveMemberAsync(Guid projectId,Guid userId,Guid tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"UPDATE ProjectMembers
                        SET IsDeleted = 1,
                            ModifiedAt = @ModifiedAt
                        WHERE ProjectId = @ProjectId
                          AND UserId = @UserId
                          AND TenantId = @TenantId
                          AND IsDeleted = 0;";

        await connection.ExecuteAsync(sql, new
            {
                ProjectId = projectId,
                UserId = userId,
                TenantId = tenantId,
                ModifiedAt = DateTime.UtcNow
            });
    }

    public async Task AddRoleAsync(ProjectMemberRole role)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"INSERT INTO ProjectMemberRoles(
                Id,ProjectMemberId,Role,CreatedAt,CreatedBy,IsDeleted)
                VALUES
                (@Id,@ProjectMemberId,@Role,@CreatedAt,@CreatedBy,@IsDeleted);";

        await connection.ExecuteAsync(sql, role);
    }

    public async Task RemoveRoleAsync(Guid projectMemberId,int role)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"UPDATE ProjectMemberRoles
                    SET IsDeleted = 1,
                        ModifiedAt = @ModifiedAt
                    WHERE ProjectMemberId = @ProjectMemberId
                      AND Role = @Role
                      AND IsDeleted = 0;";

        await connection.ExecuteAsync(sql,new
            {
                ProjectMemberId = projectMemberId,
                Role = role,
                ModifiedAt = DateTime.UtcNow
            });
    }

    public async Task<IEnumerable<ProjectMemberRole>> GetRolesAsync(Guid projectMemberId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"SELECT * FROM ProjectMemberRoles
            WHERE ProjectMemberId = @ProjectMemberId
              AND IsDeleted = 0;";

        return await connection.QueryAsync<ProjectMemberRole>(sql,new
            {
                ProjectMemberId = projectMemberId
            });
    }
}
