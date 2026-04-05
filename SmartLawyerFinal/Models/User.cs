using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? SecondNumber { get; set; }

    public string? NationalId { get; set; }

    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public virtual ICollection<ActualPayment> ActualPayments { get; set; } = new List<ActualPayment>();

    public virtual ICollection<AdminExpense> AdminExpenses { get; set; } = new List<AdminExpense>();

    public virtual ICollection<Appeal> Appeals { get; set; } = new List<Appeal>();

    public virtual ICollection<Case> CaseArchivedByNavigations { get; set; } = new List<Case>();

    public virtual ICollection<Case> CaseAssignedLawyers { get; set; } = new List<Case>();

    public virtual ICollection<CaseLawyer> CaseLawyers { get; set; } = new List<CaseLawyer>();

    public virtual ICollection<Document> DocumentArchivedByNavigations { get; set; } = new List<Document>();

    public virtual ICollection<DocumentTemplate> DocumentTemplates { get; set; } = new List<DocumentTemplate>();

    public virtual ICollection<Document> DocumentUploadedByNavigations { get; set; } = new List<Document>();

    public virtual ICollection<Fee> Fees { get; set; } = new List<Fee>();

    public virtual ICollection<Hearing> Hearings { get; set; } = new List<Hearing>();

    public virtual ICollection<LegalLibrary> LegalLibraries { get; set; } = new List<LegalLibrary>();

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual Role Role { get; set; } = null!;
}
