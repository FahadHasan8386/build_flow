using BuildFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BuildFlow.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Guid> CreateAsync(User user,IDbConnection connection,IDbTransaction transaction);

    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByIdAsync(Guid id);

    Task<bool> ExistsByEmailAsync(string email);
}
