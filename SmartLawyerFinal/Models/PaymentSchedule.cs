using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class PaymentSchedule
{
    public int Id { get; set; }

    public int FeeId { get; set; }

    public int InstallmentNumber { get; set; }

    public decimal PlannedAmount { get; set; }

    public DateOnly DueDate { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public virtual ICollection<ActualPayment> ActualPayments { get; set; } = new List<ActualPayment>();

    public virtual Fee Fee { get; set; } = null!;
}
