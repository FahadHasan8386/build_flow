using BuildFlow.Application.Interfaces.Persistence;
using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using BuildFlow.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.CreateUser;

public class CreateUserHandler
    : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDbConnectionFactory _connectionFactory;

    public CreateUserHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        IPasswordHasher passwordHasher,
        IDbConnectionFactory connectionFactory)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _passwordHasher = passwordHasher;
        _connectionFactory = connectionFactory;
    }

    public async Task<CreateUserResponse> Handle(CreateUserCommand request,CancellationToken cancellationToken)
    {
        // Authentication check
        if (!_currentUserService.IsAuthenticated)
        {
            return new CreateUserResponse
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }

        // Get current tenant from JWT
        var tenantId = _currentUserService.TenantId;

        if (tenantId == Guid.Empty)
        {
            return new CreateUserResponse
            {
                Success = false,
                Message = "Invalid tenant."
            };
        }

        var currentUserId = _currentUserService.UserId;

        // Clean input
        var email = request.Request.Email
            .Trim()
            .ToLowerInvariant();

        var firstName = request.Request.FirstName.Trim();
        var lastName = request.Request.LastName.Trim();

        // heck duplicate email inside current tenant
        if (await _userRepository.ExistsByEmailAsync(
            email,
            tenantId))
        {
            return new CreateUserResponse
            {
                Success = false,
                Message = "A user with this email already exists."
            };
        }

        // Create User
        var user = new User
        {
            Id = Guid.NewGuid(),

            TenantId = tenantId,

            FirstName = firstName,

            LastName = lastName,

            Email = email,

            PasswordHash = _passwordHasher.HashPassword(
                request.Request.Password),

            IsActive = true,

            CreatedAt = DateTime.UtcNow,

            CreatedBy = currentUserId.ToString(),

            IsDeleted = false
        };

        //  Create transaction
        using var connection = _connectionFactory.CreateConnection();

        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            // Save User
            var userId = await _userRepository.CreateAsync( user, connection, transaction);

            // Commit
            transaction.Commit();

            return new CreateUserResponse
            {
                Success = true,
                Message = "User created successfully.",
                UserId = userId
            };
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}