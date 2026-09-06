using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.DeleteUser;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteUserHandler(IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<DeleteUserResponse> Handle(DeleteUserCommand request,CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return new DeleteUserResponse
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }

        if (!_currentUserService.IsInRole("Admin"))
        {
            return new DeleteUserResponse
            {
                Success = false,
                Message = "Only tenant administrators can delete users."
            };
        }

        var tenantId = _currentUserService.TenantId;

        if (tenantId == Guid.Empty)
        {
            return new DeleteUserResponse
            {
                Success = false,
                Message = "Invalid tenant."
            };
        }

        // Prevent admin from deleting himself
        if (request.UserId == _currentUserService.UserId)
        {
            return new DeleteUserResponse
            {
                Success = false,
                Message = "You cannot delete your own account."
            };
        }

        var user = await _userRepository.GetByIdAsync(
            request.UserId,
            tenantId);

        if (user is null)
        {
            return new DeleteUserResponse
            {
                Success = false,
                Message = "User not found."
            };
        }

        user.ModifiedAt = DateTime.UtcNow;
        user.ModifiedBy = _currentUserService.UserId.ToString();

        await _userRepository.SoftDeleteAsync(user);

        return new DeleteUserResponse
        {
            Success = true,
            Message = "User deleted successfully.",
            UserId = user.Id
        };
    }
}