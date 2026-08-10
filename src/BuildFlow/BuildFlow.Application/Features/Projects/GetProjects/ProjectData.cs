using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Projects.GetProjects;

public class ProjectData
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsArchived { get; set; }
}
