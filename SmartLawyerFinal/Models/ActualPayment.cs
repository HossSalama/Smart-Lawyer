using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class ActualPayment
{
    public int Id { get; set; }

    public int FeeId { get; set; }

    public decimal Amount { get; set; }

    public DateOnly PaymentDate { get; set; }

    public string Method { get; set; } = null!;

    public string? ReceiptNumber { get; set; }

    public int ReceivedBy { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? InstallmentId { get; set; }

    public virtual Fee Fee { get; set; } = null!;

    public virtual PaymentSchedule? Installment { get; set; }

    public virtual User ReceivedByNavigation { get; set; } = null!;
}
