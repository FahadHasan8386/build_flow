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
}
