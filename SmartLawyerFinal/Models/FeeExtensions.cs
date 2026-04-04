using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLawyerFinal.Models
{
    public partial class Fee
    {
        public string CaseNumber { get; set; }
        public string CaseTitle { get; set; }
        public string ClientName { get; set; }
        public decimal TotalPaid { get; set; }

        public decimal Remaining => TotalAmount - TotalPaid;
        public int PaymentPercent => TotalAmount > 0
            ? (int)(TotalPaid / TotalAmount * 100) : 0;
        public bool IsFullyPaid => TotalPaid >= TotalAmount;
        public bool IsOverdue => DueDate.ToDateTime(TimeOnly.MinValue) < DateTime.Now
                                         && TotalPaid < TotalAmount;
        public string StatusText => IsFullyPaid ? "مدفوعة بالكامل" :
                                         IsOverdue ? "متأخرة" : "دفع جزئي";
    }

    public partial class ActualPayment
    {
        public string ReceivedByName { get; set; }
    }
}
