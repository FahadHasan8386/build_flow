using BuildFlow.Application.Interfaces.Persistence;
using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using BuildFlow.Domain.Entities;
using BuildFlow.Domain.Enums;
using MediatR;

namespace BuildFlow.Application.Features.Identity.RegisterTenant;

public class RegisterTenantHandler : IRequestHandler<RegisterTenantCommand, RegisterTenantResponse>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IDbConnectionFactory _connectionFactory;

    public RegisterTenantHandler(ITenantRepository tenantRepository,IUserRepository userRepository,IRoleRepository roleRepository,IRefreshTokenRepository refreshTokenRepository,
                                IJwtTokenService jwtTokenService,IPasswordHasher passwordHasher, IUserRoleRepository userRoleRepository,IDbConnectionFactory connectionFactory)
    {
        _tenantRepository = tenantRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
        _userRoleRepository = userRoleRepository;
        _connectionFactory = connectionFactory;
    }

    public async Task<RegisterTenantResponse> Handle(RegisterTenantCommand request, CancellationToken cancellationToken)
    {
        if (await _tenantRepository.ExistsBySlugAsync(request.Request.CompanySlug))
        {
            return new RegisterTenantResponse { Success = false, Message = "Tenant slug already exists." };
        }

        if (await _userRepository.ExistsByEmailAsync(request.Request.Email))
        {
            return new RegisterTenantResponse { Success = false, Message = "Email already exists." };
        }

        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            var tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = request.Request.CompanyName,
                Slug = request.Request.CompanySlug,
                Status = TenantStatus.Active,
                CreatedBy = request.Request.Email,
                CreatedAt = DateTime.UtcNow,
                ModifiedBy = request.Request.Email,
                ModifiedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var tenantId = await _tenantRepository.CreateAsync(tenant, connection, transaction);

            var user = new User
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                FirstName = request.Request.FirstName,
                LastName = request.Request.LastName,
                Email = request.Request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Request.Password),
                IsActive = true,
                CreatedBy = request.Request.Email,
                CreatedAt = DateTime.UtcNow,
                ModifiedBy = request.Request.Email,
                ModifiedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var userId = await _userRepository.CreateAsync(user, connection, transaction);

            var adminRole = new Role
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                Name = "Admin",
                Description = "Tenant administrator",
                IsSystemRole = true,
                CreatedBy = request.Request.Email,
                CreatedAt = DateTime.UtcNow,
                ModifiedBy = request.Request.Email,
                ModifiedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var roleId = await _roleRepository.CreateAsync(adminRole, connection, transaction);

            await _userRoleRepository.CreateAsync(new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RoleId = roleId,
                CreatedBy = request.Request.Email,
                CreatedAt = DateTime.UtcNow,
                ModifiedBy = request.Request.Email,
                ModifiedAt = DateTime.UtcNow,
                IsDeleted = false
            }, connection, transaction);

            var refreshToken = _jwtTokenService.GenerateRefreshToken(userId);
            await _refreshTokenRepository.CreateAsync(refreshToken, connection, transaction);

            transaction.Commit();

            return new RegisterTenantResponse
            {
                Success = true,
                Message = "Tenant registered successfully.",
                TenantId = tenantId,
                UserId = userId,
                AccessToken = _jwtTokenService.GenerateAccessToken(user, "Admin"),
                RefreshToken = refreshToken.Token
            };
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
