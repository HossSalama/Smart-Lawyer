using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class CaseStatus
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public string Color { get; set; } = null!;

    public virtual ICollection<Appeal> Appeals { get; set; } = new List<Appeal>();

    public virtual ICollection<Case> Cases { get; set; } = new List<Case>();
}
