using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLawyerFinal.DTOs.FinanceDTO
{
    public class FinanceCaedsDTO
    {
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal overDueAmount { get; set; }
        public decimal UpcomingAmount { get; set; }   
    }
}
