using BuildFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BuildFlow.Application.Interfaces.Repositories;

public interface IRoleRepository
{
    Task<Guid> CreateAsync(Role role,IDbConnection connection,IDbTransaction transaction);

    Task<Role?> GetByNameAsync(Guid tenantId, string roleName);

    Task<bool> ExistsAsync(Guid tenantId, string roleName);
}
