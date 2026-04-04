using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class CaseLawyer
{
    public int Id { get; set; }

    public int CaseId { get; set; }

    public int UserId { get; set; }

    public string Role { get; set; } = null!;

    public DateTime AssignedAt { get; set; }

    public DateTime? RemovedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Case Case { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
