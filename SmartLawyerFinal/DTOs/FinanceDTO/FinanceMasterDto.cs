using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLawyerFinal.DTOs.FinanceDTO
{
    internal class FinanceMasterDto
    {
        public int FeeId { get; set; }
        public string CaseNumber { get; set; }
        public string ClientName { get; set; }
        public string CaseTitle { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal Remaining => TotalAmount - TotalPaid;
        public bool IsFullyPaid => Remaining <= 0;

        // ÈíÇäÇÊ ÇáÃŞÓÇØ (ááİáÊÑÉ: ÃŞÓÇØ áã ÊÏİÚ / ãÊÃÎÑÉ)
        public int TotalInstallmentsCount { get; set; }
        public int PaidInstallmentsCount { get; set; }
        public int PendingInstallmentsCount => TotalInstallmentsCount - PaidInstallmentsCount;

        // ÈíÇäÇÊ ÇáãæÇÚíÏ (ááİáÊÑÉ: ÇáãÊÃÎÑíä)
        public DateOnly NextDueDate { get; set; }
        public bool HasOverdueInstallments { get; set; } // åá íæÌÏ ŞÓØ æÇÍÏ Úáì ÇáÃŞá ÍÇáÊå 'ãÊÃÎÑ'¿

        // ÍÇáÉ ÇáÚŞÏ ÇáãÇáíÉ ÇáÚÇãÉ
        public string OverallStatus { get; set; }
    }
}
