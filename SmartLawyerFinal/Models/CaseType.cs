using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class CaseType
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Case> Cases { get; set; } = new List<Case>();
}
