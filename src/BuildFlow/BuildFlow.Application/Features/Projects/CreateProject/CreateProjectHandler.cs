using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using BuildFlow.Domain.Entities;
using MediatR;

namespace BuildFlow.Application.Features.Projects.CreateProject;
//request handler
public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, CreateProjectResponse>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    // Constructor
    public CreateProjectHandler(IProjectRepository projectRepository, ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    // ADD Handle() 
    public async Task<CreateProjectResponse> Handle( CreateProjectCommand request,CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return new CreateProjectResponse
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }

        var userId = _currentUserService.UserId;
        var tenantId = _currentUserService.TenantId;

        if (userId == Guid.Empty || tenantId == Guid.Empty)
        {
            return new CreateProjectResponse
            {
                Success = false,
                Message = "Invalid user or tenant information."
            };
        }

        var project = new Project
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,

            Name = request.Request.Name,
            Description = request.Request.Description,
            StartDate = request.Request.StartDate,
            EndDate = request.Request.EndDate,

            IsArchived = false,
            CreatedByUserId = userId,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        var projectId = await _projectRepository.CreateAsync(project);

        return new CreateProjectResponse
        {
            Success = true,
            Message = "Project created successfully.",
            ProjectId = projectId
        };
    }
}