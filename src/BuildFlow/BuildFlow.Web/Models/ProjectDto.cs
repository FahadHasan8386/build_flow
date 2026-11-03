namespace BuildFlow.Web.Models
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsArchived { get; set; }
        public Guid CreatedByUserId { get; set; }
    }

    public class ProjectFormModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; } = DateTime.Today;
        public DateTime? EndDate { get; set; } = DateTime.Today.AddDays(30);
        public bool IsArchived { get; set; }
    }

    public class ProjectListResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<ProjectDto> Projects { get; set; } = new();
    }

    // Response Wrapper for Get By Id
    public class ProjectSingleResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ProjectDto? Project { get; set; }
    }
}
