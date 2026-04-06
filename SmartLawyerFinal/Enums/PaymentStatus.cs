using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLawyerFinal.Enums
{
    public enum PaymentStatus
    {
        [Display(Name = "قيد الانتظار")]
        Pending = 0,

        [Display(Name = "تم الدفع")]
        Paid = 1,

        [Display(Name = "متأخر ⚠️")]
        Overdue = 2
    }
    public enum TransactionType
    {
        [Display(Name = "تحصيل")]
        ActualPayment = 0,
        [Display(Name = "قسط قضائي")]
        Installment = 1,
        [Display(Name = "مصروف إداري")]
        AdminExpense = 2
    }
}
