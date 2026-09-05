using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateUserHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateUserResponse> Handle( UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return new UpdateUserResponse
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }

        if (!_currentUserService.IsInRole("Admin"))
        {
            return new UpdateUserResponse
            {
                Success = false,
                Message = "Only tenant administrators can update users."
            };
        }

        var tenantId = _currentUserService.TenantId;
        var currentUserId = _currentUserService.UserId;

        if (tenantId == Guid.Empty)
        {
            return new UpdateUserResponse
            {
                Success = false,
                Message = "Invalid tenant."
            };
        }

        var user = await _userRepository.GetByIdAsync(
            request.UserId,
            tenantId);

        if (user is null)
        {
            return new UpdateUserResponse
            {
                Success = false,
                Message = "User not found."
            };
        }

        var email = request.Request.Email
            .Trim()
            .ToLowerInvariant();

        // Check if email belongs to another user
        var existingUser =
            await _userRepository.GetByEmailAsync(
                email,
                tenantId);

        if (existingUser is not null &&
            existingUser.Id != user.Id)
        {
            return new UpdateUserResponse
            {
                Success = false,
                Message = "Another user already uses this email."
            };
        }

        user.FirstName = request.Request.FirstName.Trim();
        user.LastName = request.Request.LastName.Trim();
        user.Email = email;
        user.ModifiedAt = DateTime.UtcNow;
        user.ModifiedBy = currentUserId.ToString();

        await _userRepository.UpdateAsync(user);

        return new UpdateUserResponse
        {
            Success = true,
            Message = "User updated successfully.",
            UserId = user.Id
        };
    }
}
