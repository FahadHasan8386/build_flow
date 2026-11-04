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

        // Fix 1: Controller-এর [Route("api/projects")] এর সাথে মিল রেখে BaseUrl আপডেট করা হলো
        private const string BaseUrl = "api/projects";

        public ProjectService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task<List<ProjectDto>> GetProjectsAsync()
        {
            try
            {
                Console.WriteLine("ProjectService: Starting GetProjectsAsync");

                await AttachBearerTokenAsync();
                Console.WriteLine("ProjectService: Bearer token attached");

                var fullUrl = $"{_httpClient.BaseAddress}{BaseUrl}".TrimEnd('/');
                Console.WriteLine($"ProjectService: Calling API endpoint: {fullUrl}");

                var response = await _httpClient.GetFromJsonAsync<ProjectListResponse>(BaseUrl);

                if (response == null)
                {
                    Console.WriteLine("ProjectService: API returned null response");
                    return new List<ProjectDto>();
                }

                Console.WriteLine($"ProjectService: API returned {response.Projects?.Count ?? 0} projects");
                return response.Projects ?? new List<ProjectDto>();
            }
            catch (Exception ex)
            {
                // Fix 2: C#-এর Multiline string Syntax Error ঠিক করা হলো (\n ব্যবহার করে)
                Console.WriteLine($"ProjectService Error: {ex.GetType().Name}\n{ex.Message}");
                Console.WriteLine($"ProjectService Stack trace: {ex.StackTrace}");
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

                // Fix 3: MediatR wrapper matching. GetProjectsAsync এর মত এটিও Wrapper Response রিসিভ করবে
                var response = await _httpClient.GetFromJsonAsync<ProjectSingleResponse>($"{BaseUrl}/{id}");
                return response?.Project;
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
                // Fix 4: Create, Update, Delete মেথডগুলোতে Token পাঠানো হচ্ছিল না, যা 401 Unauthorized এরর দিতো
                await AttachBearerTokenAsync();
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
                await AttachBearerTokenAsync(); // Token Added
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
                await AttachBearerTokenAsync(); // Token Added
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