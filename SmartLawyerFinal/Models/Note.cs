using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class Note
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public string NoteType { get; set; } = null!;

    public string RelatedTable { get; set; } = null!;

    public int RelatedId { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
