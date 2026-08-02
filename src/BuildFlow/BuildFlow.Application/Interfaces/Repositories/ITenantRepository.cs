using BuildFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BuildFlow.Application.Interfaces.Repositories;

public interface ITenantRepository
{
    Task<Guid> CreateAsync(Tenant tenant,IDbConnection connection, IDbTransaction transaction);

    Task<Tenant?> GetByIdAsync(Guid id);

    Task<Tenant?> GetBySlugAsync(string slug);

    Task<bool> ExistsBySlugAsync(string slug);
}
