using BuildFlow.Application.Interfaces.Security;
using BuildFlow.Infrastructure.Persistence;
using BuildFlow.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<DbConnectionFactory>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        return services;
    }
}
