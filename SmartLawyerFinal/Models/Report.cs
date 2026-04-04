using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class Report
{
    public int Id { get; set; }

    public string ReportType { get; set; } = null!;

    public string Title { get; set; } = null!;

    public int GeneratedBy { get; set; }

    public DateTime GeneratedAt { get; set; }

    public string? Parameters { get; set; }

    public string? FilePath { get; set; }

    public virtual User GeneratedByNavigation { get; set; } = null!;
}
