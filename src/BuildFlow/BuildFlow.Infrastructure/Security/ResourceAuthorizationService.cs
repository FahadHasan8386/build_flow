using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;

namespace BuildFlow.Infrastructure.Security;

public class ResourceAuthorizationService : IResourceAuthorizationService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectRepository _projectRepository;

    public ResourceAuthorizationService(
        ICurrentUserService currentUserService,
        IProjectRepository projectRepository)
    {
        _currentUserService = currentUserService;
        _projectRepository = projectRepository;
    }

    // 1. Fixed: Added 'public async Task<bool>' and removed the '>' character
    public async Task<bool> IsAuthorizedAsync(
        Guid userId,
        Guid tenantId,
        string resourceType,
        Guid resourceId,
        string action)
    {
        if (_currentUserService.IsInRole("Admin"))
        {
            return true;
        }

        // TODO: Add logic for standard users
        return false;
    }

    // 2. Fixed: Removed the floating '>' character above this method
    public async Task<bool> IsProjectOwnerOrAdminAsync(
        Guid userId,
        Guid tenantId,
        Guid projectId)
    {
        if (_currentUserService.IsInRole("Admin"))
        {
            return true;
        }

        // TODO: Check if the specific user is the owner of the project
        return false;
    }

    public async Task<IEnumerable<Guid>> GetUserAccessibleProjectsAsync(
        Guid userId,
        Guid tenantId)
    {
        if (_currentUserService.IsInRole("Admin"))
        {
            // Note: Returning empty means even Admins will see 0 projects if this method is used for list filtering.
            // You may need to fetch all project IDs from _projectRepository here instead.
            return Enumerable.Empty<Guid>();
        }

        // TODO: Fetch user-specific assigned projects
        return Enumerable.Empty<Guid>();
    }

    public async Task<bool> IsResourceInTenantAsync(
        Guid resourceId,
        Guid tenantId,
        string resourceType)
    {
        switch (resourceType)
        {
            case "Project":
                var project = await _projectRepository.GetByIdAsync(resourceId, tenantId);
                return project != null;

            default:
                return false;
        }
    }
}