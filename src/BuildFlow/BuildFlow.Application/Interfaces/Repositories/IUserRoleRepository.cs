using BuildFlow.Domain.Entities;
using System.Data;

namespace BuildFlow.Application.Interfaces.Repositories;

public interface IUserRoleRepository
{
    Task<Guid> CreateAsync(UserRole userRole, IDbConnection connection, IDbTransaction transaction);
}
