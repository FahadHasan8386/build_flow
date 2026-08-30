using BuildFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Interfaces.Repositories;

public interface ITaskCommentRepository
{
    Task AddAsync(TaskComment comment);

    Task<IEnumerable<TaskComment>> GetByTaskAsync(
        Guid taskId,
        Guid tenantId);

    Task<TaskComment?> GetByIdAsync(
        Guid commentId,
        Guid tenantId);

    Task UpdateAsync(TaskComment comment);

    Task DeleteAsync(
        Guid commentId,
        Guid tenantId);
}
