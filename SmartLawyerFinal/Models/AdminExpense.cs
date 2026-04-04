using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class AdminExpense
{
    public int Id { get; set; }

    public int CaseId { get; set; }

    public string Description { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateOnly ExpenseDate { get; set; }

    public int PaidBy { get; set; }

    public string? ReceiptPath { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Case Case { get; set; } = null!;

    public virtual User PaidByNavigation { get; set; } = null!;
}
