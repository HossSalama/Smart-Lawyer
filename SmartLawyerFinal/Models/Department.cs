using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class Department
{
    public int Id { get; set; }

    public string DeptName { get; set; } = null!;

    public int CourtId { get; set; }

    public string? JudgeName { get; set; }

    public virtual ICollection<Case> Cases { get; set; } = new List<Case>();

    public virtual Court Court { get; set; } = null!;

    public virtual ICollection<Hearing> Hearings { get; set; } = new List<Hearing>();
}
