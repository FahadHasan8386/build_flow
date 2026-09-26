namespace BuildFlow.Application.Interfaces.Security;

/// <summary>
/// Interface for resource-level authorization checks.
/// This service provides the foundation for scaling from role-based to claim-based authorization.
/// 
/// PHASE 1 (Current): Admin-only operations
/// PHASE 2 (Future): Project owner/member roles, granular permissions
/// PHASE 3 (Future): Custom policies, claim-based resource access
/// </summary>
public interface IResourceAuthorizationService
{
    /// <summary>
    /// Checks if the current user can perform an action on a specific resource.
    /// Supports tenant isolation and role-based or claim-based authorization.
    /// </summary>
    /// <param name="userId">The current user's ID</param>
    /// <param name="tenantId">The tenant ID (multi-tenant isolation)</param>
    /// <param name="resourceType">Type of resource (Project, Task, ProjectMember, etc.)</param>
    /// <param name="resourceId">ID of the specific resource</param>
    /// <param name="action">Action being performed (Create, Read, Update, Delete)</param>
    /// <returns>True if authorized, false otherwise</returns>
    Task<bool> IsAuthorizedAsync(
        Guid userId,
        Guid tenantId,
        string resourceType,
        Guid resourceId,
        string action);

    /// <summary>
    /// Checks if the current user is a project owner or admin.
    /// PHASE 2: Will extend to check project roles from claims.
    /// </summary>
    Task<bool> IsProjectOwnerOrAdminAsync(
        Guid userId,
        Guid tenantId,
        Guid projectId);

    /// <summary>
    /// Gets all projects the user has access to (with pagination).
    /// PHASE 2: Will filter based on project ownership claims/roles.
    /// </summary>
    Task<IEnumerable<Guid>> GetUserAccessibleProjectsAsync(
        Guid userId,
        Guid tenantId);

    /// <summary>
    /// Validates that a resource belongs to the specified tenant (tenant isolation).
    /// PHASE 2: Will be enhanced with resource-level policies.
    /// </summary>
    Task<bool> IsResourceInTenantAsync(
        Guid resourceId,
        Guid tenantId,
        string resourceType);
}
