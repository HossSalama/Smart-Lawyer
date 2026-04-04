using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class Case
{
    public int Id { get; set; }

    public string CaseNumber { get; set; } = null!;

    public string Title { get; set; } = null!;

    public DateOnly OpenDate { get; set; }

    public DateOnly? CloseDate { get; set; }

    public string? Stage { get; set; }

    public bool IsArchived { get; set; }

    public DateTime? ArchivedAt { get; set; }

    public int? ArchivedBy { get; set; }

    public string? ArchiveNote { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int ClientId { get; set; }

    public int CaseTypeId { get; set; }

    public int CourtId { get; set; }

    public int? DeptId { get; set; }

    public int StatusId { get; set; }

    public int AssignedLawyerId { get; set; }

    public virtual ICollection<AdminExpense> AdminExpenses { get; set; } = new List<AdminExpense>();

    public virtual ICollection<Appeal> Appeals { get; set; } = new List<Appeal>();

    public virtual User? ArchivedByNavigation { get; set; }

    public virtual User AssignedLawyer { get; set; } = null!;

    public virtual ICollection<CaseLawyer> CaseLawyers { get; set; } = new List<CaseLawyer>();

    public virtual ICollection<CaseOpponent> CaseOpponents { get; set; } = new List<CaseOpponent>();

    public virtual CaseType CaseType { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;

    public virtual Court Court { get; set; } = null!;

    public virtual Department? Dept { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<Fee> Fees { get; set; } = new List<Fee>();

    public virtual ICollection<Hearing> Hearings { get; set; } = new List<Hearing>();

    public virtual CaseStatus Status { get; set; } = null!;
}
