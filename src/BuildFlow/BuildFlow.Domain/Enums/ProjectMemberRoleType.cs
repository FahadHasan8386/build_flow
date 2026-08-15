using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Domain.Enums;

public enum ProjectMemberRoleType
{
    ProjectManager = 1,
    Engineer = 2, 
    SiteSupervisor = 3,
    Accountant = 4,
    QualityEngineer = 5,
    Viewer = 6
}
