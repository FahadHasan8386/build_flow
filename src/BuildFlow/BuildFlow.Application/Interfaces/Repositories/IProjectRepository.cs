using BuildFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BuildFlow.Application.Interfaces.Repositories;

public interface IProjectRepository
{
    Task<Guid> CreateAsync(Project project, IDbConnection connection , IDbTransaction transaction );
    Task<Project?> GetByIdAsync(Guid id , Guid tenantId);
    Task<IEnumerable<Project>> GetByTenantAsync(Guid tenantId);
    Task UpdateAsync(Project project , Guid tenantId);
    Task DeleteAsync(Guid id , Guid tenantId);
}
