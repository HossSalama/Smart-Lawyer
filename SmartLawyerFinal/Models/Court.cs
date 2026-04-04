using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class Court
{
    public int Id { get; set; }

    public string CourtName { get; set; } = null!;

    public string? Location { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public virtual ICollection<Appeal> Appeals { get; set; } = new List<Appeal>();

    public virtual ICollection<Case> Cases { get; set; } = new List<Case>();

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Hearing> Hearings { get; set; } = new List<Hearing>();
}
