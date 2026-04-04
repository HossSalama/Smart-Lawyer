using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class Fee
{
    public int Id { get; set; }

    public int CaseId { get; set; }

    public int ClientId { get; set; }

    public string FeeType { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public DateOnly DueDate { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public virtual ICollection<ActualPayment> ActualPayments { get; set; } = new List<ActualPayment>();

    public virtual Case Case { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<PaymentSchedule> PaymentSchedules { get; set; } = new List<PaymentSchedule>();
}
