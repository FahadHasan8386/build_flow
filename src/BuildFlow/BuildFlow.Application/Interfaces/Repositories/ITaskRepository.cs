using BuildFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(Guid taskId,Guid tenantId);

        Task<IEnumerable<TaskItem>> GetByProjectAsync(Guid projectId,Guid tenantId);

        Task AddAsync(TaskItem task);

        Task UpdateAsync(TaskItem task);

        Task DeleteAsync(Guid taskId,Guid tenantId);
    }
}
