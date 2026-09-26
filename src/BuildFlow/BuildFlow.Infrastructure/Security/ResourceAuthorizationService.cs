using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;

namespace BuildFlow.Infrastructure.Security;

/// <summary>
/// Implementation of resource authorization service.
/// PHASE 1: Checks if user is admin for all operations.
/// PHASE 2: Will extend to check project ownership and member roles via JWT claims.
/// PHASE 3: Will support custom authorization policies.
/// </summary>
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

    /// <summary>
    /// PHASE 1: Only admins can perform actions. 
    /// FUTURE: Will check JWT claims for project ownership or member roles.
    /// </summary>
    public async Task<bool> IsAuthorizedAsync(
        Guid userId,
        Guid tenantId,
        string resourceType,
        Guid resourceId,
        string action)
    {
        // Phase 1: Admin-only
        if (_currentUserService.IsInRole("Admin"))
        {
            return true;
        }

        // TODO: Phase 2 - Check JWT claims for project ownership
        // Example: if (resourceType == "Project" && _currentUserService.HasClaim("ProjectOwner", resourceId.ToString()))
        // Example: if (resourceType == "Task" && await _currentUserService.HasProjectAccessAsync(projectId))

        // TODO: Phase 3 - Evaluate custom authorization policies

        return false;
    }

    /// <summary>
    /// PHASE 1: Checks if current user is admin.
    /// PHASE 2: Will also check project ownership from JWT claims.
    /// </summary>
    public async Task<bool> IsProjectOwnerOrAdminAsync(
        Guid userId,
        Guid tenantId,
        Guid projectId)
    {
        // Phase 1: Only admin
        if (_currentUserService.IsInRole("Admin"))
        {
            return true;
        }

        // TODO: Phase 2 - Check JWT claims for project ownership
        // var projectOwners = context.User.FindAll("ProjectOwner");
        // return projectOwners.Any(c => c.Value.Contains(projectId.ToString()));

        return false;
    }

    /// <summary>
    /// PHASE 1: Returns empty (admin only).
    /// PHASE 2: Will return projects owned by user or user is member of.
    /// </summary>
    public async Task<IEnumerable<Guid>> GetUserAccessibleProjectsAsync(
        Guid userId,
        Guid tenantId)
    {
        // Phase 1: Admin gets all, non-admin gets none
        if (_currentUserService.IsInRole("Admin"))
        {
            // Return all projects for admin
            // TODO: Implement retrieval of all projects
            return Enumerable.Empty<Guid>();
        }

        // TODO: Phase 2 - Return projects from user roles/claims
        // Example: Get all projects where user is a member or owner

        return Enumerable.Empty<Guid>();
    }

    /// <summary>
    /// Validates tenant isolation - ensures resource belongs to the tenant.
    /// This is critical for multi-tenant SaaS security.
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
