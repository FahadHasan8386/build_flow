using BuildFlow.Web.Models;
using BuildFlow.Web.Services.Interfaces;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace BuildFlow.Web.Services.Project
{
    public class ProjectService : IProjectService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private const string BaseUrl = "api/Project";

        public ProjectService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task<List<ProjectDto>> GetProjectsAsync()
        {
            try
            {
                await AttachBearerTokenAsync();

                var response = await _httpClient.GetFromJsonAsync<ProjectListResponse>(BaseUrl);
                return response?.Projects ?? new List<ProjectDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching projects: {ex.Message}");
                return new List<ProjectDto>();
            }
        }

        private async Task AttachBearerTokenAsync()
        {
            var stored = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "buildflow-auth");
            if (!string.IsNullOrWhiteSpace(stored))
            {
                var payload = JsonSerializer.Deserialize<AuthResponse>(stored);
                if (!string.IsNullOrWhiteSpace(payload?.AccessToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", payload.AccessToken);
                }
            }
        }

        public async Task<ProjectDto?> GetProjectByIdAsync(Guid id)
        {
            try
            {
                await AttachBearerTokenAsync();

                var response = await _httpClient.GetFromJsonAsync<ProjectDto>($"{BaseUrl}/{id}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching project with id {id}: {ex.Message}");
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