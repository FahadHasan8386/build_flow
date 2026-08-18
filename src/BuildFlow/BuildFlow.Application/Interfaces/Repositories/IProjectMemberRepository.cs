using BuildFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Interfaces.Repositories
{
    public interface IProjectMemberRepository
    {
        Task<Guid> AddMemberAsync(ProjectMember member);

        Task<ProjectMember?> GetMemberAsync(
            Guid projectId,
            Guid userId,
            Guid tenantId);

        Task<IEnumerable<ProjectMember>> GetMembersByProjectAsync(
            Guid projectId,
            Guid tenantId);

        Task RemoveMemberAsync(
            Guid projectId,
            Guid userId,
            Guid tenantId);

        Task AddRoleAsync(ProjectMemberRole role);

        Task RemoveRoleAsync(
            Guid projectMemberId,
            int role);

        Task<IEnumerable<ProjectMemberRole>> GetRolesAsync(
            Guid projectMemberId);
    }
}
