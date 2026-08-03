using BuildFlow.Domain.Entities;

namespace BuildFlow.Application.Interfaces.Repositories;

public interface IUserRoleRepository
{
    Task<Guid> CreateAsync(UserRole userRole);
}
