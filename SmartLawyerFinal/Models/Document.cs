using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class Document
{
    public int Id { get; set; }

    public int CaseId { get; set; }

    public string Title { get; set; } = null!;

    public string DocType { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public string MimeType { get; set; } = null!;

    public int UploadedBy { get; set; }

    public DateTime UploadedAt { get; set; }

    public bool IsArchived { get; set; }

    public DateTime? ArchivedAt { get; set; }

    public int? ArchivedBy { get; set; }

    public string? Notes { get; set; }

    public virtual User? ArchivedByNavigation { get; set; }

    public virtual Case Case { get; set; } = null!;

    public virtual User UploadedByNavigation { get; set; } = null!;
}
