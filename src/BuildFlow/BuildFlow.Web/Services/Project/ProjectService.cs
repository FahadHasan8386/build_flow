using BuildFlow.Web.Models;
using BuildFlow.Web.Services.Interfaces;
using System.Net.Http.Json;

namespace BuildFlow.Web.Services.Project
{
    public class ProjectService : IProjectService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/project";

        public ProjectService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProjectDto>> GetProjectsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<ProjectDto>>(BaseUrl);
                return response ?? new List<ProjectDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching projects: {ex.Message}");
                return new List<ProjectDto>();
            }
        }

        public async Task<ProjectDto?> GetProjectByIdAsync(Guid id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ProjectDto>($"{BaseUrl}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching project {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateProjectAsync(ProjectFormModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(BaseUrl, model);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating project: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateProjectAsync(Guid id, ProjectFormModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", model);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating project {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteProjectAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting project {id}: {ex.Message}");
                return false;
            }
        }

    }
}
