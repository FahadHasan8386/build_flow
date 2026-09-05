
using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.GetUserById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetUserByIdHandler( IUserRepository userRepository, ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return new GetUserByIdResponse
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }

        if (!_currentUserService.IsInRole("Admin"))
        {
            return new GetUserByIdResponse
            {
                Success = false,
                Message = "Only tenant administrators can view users."
            };
        }

        var tenantId = _currentUserService.TenantId;

        if (tenantId == Guid.Empty)
        {
            return new GetUserByIdResponse
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
            return new GetUserByIdResponse
            {
                Success = false,
                Message = "User not found."
            };
        }

        return new GetUserByIdResponse
        {
            Success = true,
            Message = "User retrieved successfully.",

            User = new UserDetailsDto
            {
                Id = user.Id,
                TenantId = user.TenantId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            }
        };
    }
}