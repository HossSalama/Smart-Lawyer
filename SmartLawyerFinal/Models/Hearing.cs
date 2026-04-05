using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class Hearing
{
    public int Id { get; set; }

    public int CaseId { get; set; }

    public int CourtId { get; set; }

    public int DeptId { get; set; }

    public string HearingType { get; set; } = null!;

    public DateTime HearingDateTime { get; set; }

    public string JudgeName { get; set; } = null!;

    public string Period { get; set; } = null!;

    public string AttendanceStatus { get; set; } = null!;

    public string Result { get; set; } = null!;

    public DateOnly NextHearingDate { get; set; }

    public string? NextHearingPeriod { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public virtual Case Case { get; set; } = null!;

    public virtual Court Court { get; set; } = null!;

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Department Dept { get; set; } = null!;
}
