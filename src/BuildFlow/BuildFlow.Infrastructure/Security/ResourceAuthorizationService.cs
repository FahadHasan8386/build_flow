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
> IsAuthorizedAsync(
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


        return false;
    }

    public async Task<bool> IsProjectOwnerOrAdminAsync(
        Guid userId,
        Guid tenantId,
        Guid projectId)
    {
        if (_currentUserService.IsInRole("Admin"))
        {
            return true;
        }


        return false;
    }

    public async Task<IEnumerable<Guid>> GetUserAccessibleProjectsAsync(
        Guid userId,
        Guid tenantId)
    {
        if (_currentUserService.IsInRole("Admin"))
        {
            return Enumerable.Empty<Guid>();
        }


        return Enumerable.Empty<Guid>();
    }

    public async Task<bool> IsResourceInTenantAsync(
        Guid resourceId,
        Guid tenantId,
        string resourceType)
    {
        switch (resourceType)
        {
            case "Project": var project = await _projectRepository.GetByIdAsync(resourceId, tenantId);
                return project != null;


            default:
                return false;
        }
    }
}
