using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.UpdateUserStatus;

public class UpdateUserStatusHandler : IRequestHandler<UpdateUserStatusCommand, UpdateUserStatusResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateUserStatusHandler(IUserRepository userRepository,ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateUserStatusResponse> Handle(
        UpdateUserStatusCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return new UpdateUserStatusResponse
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }

        if (!_currentUserService.IsInRole("Admin"))
        {
            return new UpdateUserStatusResponse
            {
                Success = false,
                Message = "Only tenant administrators can change user status."
            };
        }

        var tenantId = _currentUserService.TenantId;
        var currentUserId = _currentUserService.UserId;

        if (tenantId == Guid.Empty)
        {
            return new UpdateUserStatusResponse
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
            return new UpdateUserStatusResponse
            {
                Success = false,
                Message = "User not found."
            };
        }

        // Prevent admin from deactivating himself
        if (user.Id == currentUserId &&
            !request.Request.IsActive)
        {
            return new UpdateUserStatusResponse
            {
                Success = false,
                Message = "You cannot deactivate your own account."
            };
        }

        user.IsActive = request.Request.IsActive;
        user.ModifiedAt = DateTime.UtcNow;
        user.ModifiedBy = currentUserId.ToString();

        await _userRepository.UpdateStatusAsync(user);

        return new UpdateUserStatusResponse
        {
            Success = true,
            Message = request.Request.IsActive
                ? "User activated successfully."
                : "User deactivated successfully.",
            UserId = user.Id,
            IsActive = user.IsActive
        };
    }
}
