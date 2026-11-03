using BuildFlow.Web.Models;

namespace BuildFlow.Web.Services.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetProjectsAsync();
        Task<ProjectDto?> GetProjectByIdAsync(Guid id);
        Task<bool> CreateProjectAsync(ProjectFormModel model);
        Task<bool> UpdateProjectAsync(Guid id, ProjectFormModel model);
        Task<bool> DeleteProjectAsync(Guid id);
    }
}
