using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.GetUsers;

public class GetUsersHandler : IRequestHandler<GetUsersQuery, GetUsersResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetUsersHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetUsersResponse> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return new GetUsersResponse
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }

        if (!_currentUserService.IsInRole("Admin"))
        {
            return new GetUsersResponse
            {
                Success = false,
                Message = "Only tenant administrators can view users."
            };
        }

        var tenantId = _currentUserService.TenantId;

        if (tenantId == Guid.Empty)
        {
            return new GetUsersResponse
            {
                Success = false,
                Message = "Invalid tenant."
            };
        }

        var users = await _userRepository.GetAllAsync(tenantId);

        var result = users.Select(user => new UserListItemDto
        {
            Id = user.Id,
            TenantId = user.TenantId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        }).ToList();

        return new GetUsersResponse
        {
            Success = true,
            Message = "Users retrieved successfully.",
            Users = result
        };
    }
}
