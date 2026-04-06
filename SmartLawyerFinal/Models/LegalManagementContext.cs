using Microsoft.EntityFrameworkCore;
using SmartLawyerFinal.Enums;
using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class LegalManagementContext : DbContext
{
    public LegalManagementContext()
    {
    }

    public LegalManagementContext(DbContextOptions<LegalManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActualPayment> ActualPayments { get; set; }

    public virtual DbSet<AdminExpense> AdminExpenses { get; set; }

    public virtual DbSet<Appeal> Appeals { get; set; }

    public virtual DbSet<Case> Cases { get; set; }

    public virtual DbSet<CaseLawyer> CaseLawyers { get; set; }

    public virtual DbSet<CaseOpponent> CaseOpponents { get; set; }

    public virtual DbSet<CaseStatus> CaseStatuses { get; set; }

    public virtual DbSet<CaseType> CaseTypes { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Court> Courts { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<DocumentTemplate> DocumentTemplates { get; set; }

    public virtual DbSet<Fee> Fees { get; set; }

    public virtual DbSet<Hearing> Hearings { get; set; }

    public virtual DbSet<LegalLibrary> LegalLibraries { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Opponent> Opponents { get; set; }

    public virtual DbSet<PaymentSchedule> PaymentSchedules { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=LegalCaseManagementDB;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Arabic_CI_AI");

        modelBuilder.Entity<ActualPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ActualPa__3214EC079243D2AE");

            entity.ToTable("ActualPayments", "Finance");

            entity.HasIndex(e => e.ReceiptNumber, "UQ__ActualPa__C08AFDAB7C3F98C9").IsUnique();

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Method)
                .HasMaxLength(50)
                .HasDefaultValue("ﬂ«‘");
            entity.Property(e => e.PaymentDate).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.ReceiptNumber).HasMaxLength(100);

            entity.HasOne(d => d.Fee).WithMany(p => p.ActualPayments)
                .HasForeignKey(d => d.FeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActPay_Fee");

            entity.HasOne(d => d.Installment).WithMany(p => p.ActualPayments)
                .HasForeignKey(d => d.InstallmentId)
                .HasConstraintName("FK_ActPay_Installment");

            entity.HasOne(d => d.ReceivedByNavigation).WithMany(p => p.ActualPayments)
                .HasForeignKey(d => d.ReceivedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActPay_RecBy");
        });

        modelBuilder.Entity<AdminExpense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AdminExp__3214EC0712846B0C");

            entity.ToTable("AdminExpenses", "Finance");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ExpenseDate).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.ReceiptPath).HasMaxLength(1000);

            entity.HasOne(d => d.Case).WithMany(p => p.AdminExpenses)
                .HasForeignKey(d => d.CaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdminExp_Case");

            entity.HasOne(d => d.PaidByNavigation).WithMany(p => p.AdminExpenses)
                .HasForeignKey(d => d.PaidBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdminExp_PaidBy");
        });

        modelBuilder.Entity<Appeal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Appeals__3214EC0717FB0C75");

            entity.ToTable("Appeals", "Legal");

            entity.HasIndex(e => e.AppealNumber, "UQ__Appeals__9802DB2581E9FC9B").IsUnique();

            entity.Property(e => e.AppealNumber)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AppealType).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.AssignedLawyer).WithMany(p => p.Appeals)
                .HasForeignKey(d => d.AssignedLawyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appeals_Lawyer");

            entity.HasOne(d => d.Case).WithMany(p => p.Appeals)
                .HasForeignKey(d => d.CaseId)
                .HasConstraintName("FK_Appeals_Case");

            entity.HasOne(d => d.Court).WithMany(p => p.Appeals)
                .HasForeignKey(d => d.CourtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appeals_Court");

            entity.HasOne(d => d.Status).WithMany(p => p.Appeals)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appeals_Status");
        });

        modelBuilder.Entity<Case>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cases__3214EC077FEDE386");

            entity.ToTable("Cases", "Legal");

            entity.Property(e => e.ArchiveNote).HasMaxLength(500);
            entity.Property(e => e.ArchivedAt).HasColumnType("datetime");
            entity.Property(e => e.CaseNumber).HasMaxLength(100);
            entity.Property(e => e.OpenDate).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.Stage).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(500);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ArchivedByNavigation).WithMany(p => p.CaseArchivedByNavigations)
                .HasForeignKey(d => d.ArchivedBy)
                .HasConstraintName("FK_Cases_ArchivedBy");

            entity.HasOne(d => d.AssignedLawyer).WithMany(p => p.CaseAssignedLawyers)
                .HasForeignKey(d => d.AssignedLawyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cases_Lawyer");

            entity.HasOne(d => d.CaseType).WithMany(p => p.Cases)
                .HasForeignKey(d => d.CaseTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cases_CaseType");

            entity.HasOne(d => d.Client).WithMany(p => p.Cases)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cases_Client");

            entity.HasOne(d => d.Court).WithMany(p => p.Cases)
                .HasForeignKey(d => d.CourtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cases_Court");

            entity.HasOne(d => d.Dept).WithMany(p => p.Cases)
                .HasForeignKey(d => d.DeptId)
                .HasConstraintName("FK_Cases_Dept");

            entity.HasOne(d => d.Status).WithMany(p => p.Cases)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cases_Status");
        });

        modelBuilder.Entity<CaseLawyer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CaseLawy__3214EC0729B90EEA");

            entity.ToTable("CaseLawyers", "Legal");

            entity.HasIndex(e => new { e.CaseId, e.UserId }, "UQ_CaseLawyers").IsUnique();

            entity.Property(e => e.AssignedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RemovedAt).HasColumnType("datetime");
            entity.Property(e => e.Role)
                .HasMaxLength(100)
                .HasDefaultValue("„Õ«„Ì „”«⁄œ");

            entity.HasOne(d => d.Case).WithMany(p => p.CaseLawyers)
                .HasForeignKey(d => d.CaseId)
                .HasConstraintName("FK_CaseLawyers_Case");

            entity.HasOne(d => d.User).WithMany(p => p.CaseLawyers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CaseLawyers_User");
        });

        modelBuilder.Entity<CaseOpponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CaseOppo__3214EC0713244ACE");

            entity.ToTable("CaseOpponents", "Legal");

            entity.HasIndex(e => new { e.CaseId, e.OpponentId }, "UQ_CaseOpponents").IsUnique();

            entity.Property(e => e.AddedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Case).WithMany(p => p.CaseOpponents)
                .HasForeignKey(d => d.CaseId)
                .HasConstraintName("FK_CaseOpponents_Case");

            entity.HasOne(d => d.Opponent).WithMany(p => p.CaseOpponents)
                .HasForeignKey(d => d.OpponentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CaseOpponents_Opponent");
        });

        modelBuilder.Entity<CaseStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CaseStat__3214EC07B5F2F383");

            entity.ToTable("CaseStatuses", "Lookup");

            entity.HasIndex(e => e.StatusName, "UQ__CaseStat__05E7698A51AACFCC").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Color)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasDefaultValue("#1D9E75");
            entity.Property(e => e.StatusName)
                .HasMaxLength(15)
                .HasDefaultValue("„› ÊÕ…");
        });

        modelBuilder.Entity<CaseType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CaseType__3214EC075B6A89A7");

            entity.ToTable("CaseTypes", "Lookup");

            entity.HasIndex(e => e.TypeName, "UQ__CaseType__D4E7DFA851ADBB57").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.TypeName).HasMaxLength(15);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clients__3214EC072E76F1EE");

            entity.ToTable("Clients", "Legal");

            entity.HasIndex(e => e.CommercialReg, "UQ__Clients__5553E82743175A53").IsUnique();

            entity.HasIndex(e => e.NationalId, "UQ__Clients__E9AA32FAC4ED8D46").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.ClientType)
                .HasMaxLength(10)
                .HasDefaultValue("›—œ");
            entity.Property(e => e.CommercialReg)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.JobTitle).HasMaxLength(50);
            entity.Property(e => e.NationalId)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SecondaryPhone)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Court>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Courts__3214EC0774F0030F");

            entity.ToTable("Courts", "Lookup");

            entity.HasIndex(e => e.CourtName, "UQ__Courts__5750888E36409F76").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.CourtName).HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC07056CED17");

            entity.ToTable("Departments", "Lookup");

            entity.HasIndex(e => new { e.DeptName, e.CourtId }, "UQ_Departments").IsUnique();

            entity.Property(e => e.DeptName).HasMaxLength(100);
            entity.Property(e => e.JudgeName).HasMaxLength(100);

            entity.HasOne(d => d.Court).WithMany(p => p.Departments)
                .HasForeignKey(d => d.CourtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Departments_Courts");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC07983567AD");

            entity.ToTable("Documents", "Docs");

            entity.Property(e => e.DocType).HasMaxLength(50);
            entity.Property(e => e.FilePath)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.MimeType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(500);
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ArchivedByNavigation).WithMany(p => p.DocumentArchivedByNavigations)
                .HasForeignKey(d => d.ArchivedBy)
                .HasConstraintName("FK_Documents_ArchivedBy");

            entity.HasOne(d => d.Case).WithMany(p => p.Documents)
                .HasForeignKey(d => d.CaseId)
                .HasConstraintName("FK_Documents_Case");

            entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.DocumentUploadedByNavigations)
                .HasForeignKey(d => d.UploadedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Documents_UploadedBy");
        });

        modelBuilder.Entity<DocumentTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC0780986D55");

            entity.ToTable("DocumentTemplates", "Docs");

            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.MimeType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(500);

            entity.HasOne(d => d.AddedByNavigation).WithMany(p => p.DocumentTemplates)
                .HasForeignKey(d => d.AddedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentTemplates_AddedBy");
        });

        modelBuilder.Entity<Fee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Fees__3214EC0761BC1105");

            entity.ToTable("Fees", "Finance");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Case).WithMany(p => p.Fees)
                .HasForeignKey(d => d.CaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fees_Case");

            entity.HasOne(d => d.Client).WithMany(p => p.Fees)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fees_Client");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Fees)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fees_CreatedBy");
        });

        modelBuilder.Entity<Hearing>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Hearings__3214EC075601EDF7");

            entity.ToTable("Hearings", "Legal");

            entity.Property(e => e.AttendanceStatus)
                .HasMaxLength(50)
                .HasDefaultValue("ﬁ«œ„");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.HearingDateTime).HasColumnType("datetime");
            entity.Property(e => e.HearingType)
                .HasMaxLength(50)
                .HasDefaultValue("Ã·”…");
            entity.Property(e => e.JudgeName).HasMaxLength(100);
            entity.Property(e => e.NextHearingPeriod).HasMaxLength(20);
            entity.Property(e => e.Period)
                .HasMaxLength(5)
                .HasComputedColumnSql("(case when datepart(hour,[HearingDateTime])<(12) then N'’»«ÕÌ' else N'„”«∆Ì' end)", false);

            entity.HasOne(d => d.Case).WithMany(p => p.Hearings)
                .HasForeignKey(d => d.CaseId)
                .HasConstraintName("FK_Hearings_Case");

            entity.HasOne(d => d.Court).WithMany(p => p.Hearings)
                .HasForeignKey(d => d.CourtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Hearings_Court");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Hearings)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Hearings_CreatedBy");

            entity.HasOne(d => d.Dept).WithMany(p => p.Hearings)
                .HasForeignKey(d => d.DeptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Hearings_Dept");
        });

        modelBuilder.Entity<LegalLibrary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LegalLib__3214EC072F97D00B");

            entity.ToTable("LegalLibrary", "Docs");

            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.FilePath)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.MimeType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(500);

            entity.HasOne(d => d.AddedByNavigation).WithMany(p => p.LegalLibraries)
                .HasForeignKey(d => d.AddedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LegalLibrary_AddedBy");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notes__3214EC07523701A4");

            entity.ToTable("Notes", "Legal");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NoteType)
                .HasMaxLength(50)
                .HasDefaultValue("⁄«„…");
            entity.Property(e => e.RelatedTable).HasMaxLength(100);

            entity.HasOne(d => d.User).WithMany(p => p.Notes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notes_User");
        });

        modelBuilder.Entity<Opponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Opponent__3214EC075B5771E3");

            entity.ToTable("Opponents", "Legal");

            entity.HasIndex(e => e.NationalId, "UQ__Opponent__E9AA32FA65B02F64").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.NationalId)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.OpponentLawyerName).HasMaxLength(200);
            entity.Property(e => e.OpponentLawyerPhone)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<PaymentSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentS__3214EC070A799146");

            entity.ToTable("PaymentSchedule", "Finance");

            entity.HasIndex(e => new { e.FeeId, e.InstallmentNumber }, "UQ_PaySched_Inst").IsUnique();

            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.PlannedAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
           .HasConversion<int>() 
           .HasDefaultValue(PaymentStatus.Pending);

            entity.HasOne(d => d.Fee).WithMany(p => p.PaymentSchedules)
                .HasForeignKey(d => d.FeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PaySched_Fee");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reports__3214EC0721CC3C25");

            entity.ToTable("Reports", "Core");

            entity.Property(e => e.FilePath).HasMaxLength(1000);
            entity.Property(e => e.GeneratedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReportType).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(500);

            entity.HasOne(d => d.GeneratedByNavigation).WithMany(p => p.Reports)
                .HasForeignKey(d => d.GeneratedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reports_GeneratedBy");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07B922EC91");

            entity.ToTable("Roles", "Core");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160D0D90D89").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RoleName)
                .HasMaxLength(10)
                .HasDefaultValue("„Õ«„Ì");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0763A419AA");

            entity.ToTable("Users", "Core");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Users__85FB4E38776812BE").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053418FB8C99").IsUnique();

            entity.HasIndex(e => e.NationalId, "UQ__Users__E9AA32FA09CF1B85").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastLoginAt).HasColumnType("datetime");
            entity.Property(e => e.NationalId)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SecondNumber)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
