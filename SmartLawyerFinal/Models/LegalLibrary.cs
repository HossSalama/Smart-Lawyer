using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class LegalLibrary
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string MimeType { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public string? Description { get; set; }

    public int AddedBy { get; set; }

    public virtual User AddedByNavigation { get; set; } = null!;
}
