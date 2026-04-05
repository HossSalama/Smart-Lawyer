
using Microsoft.EntityFrameworkCore;
using SmartLawyerFinal.DAL.Repositories.InterfaceRepository;
using SmartLawyerFinal.DTOs.FinanceDTO;

namespace SmartLawyerFinal.DAL.Repositories.ClassRepository
{
    public class FinanceRepository : IFinanceRepository
    {
        private readonly LegalManagementContext _context;
        public FinanceRepository(LegalManagementContext context)
        {
            _context = context;
        }
        public async Task<FinanceCaedsDTO> GetDashboardSummaryAsync()
        {

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            var summary = new FinanceCaedsDTO
            {
   
                TotalAmount = await _context.Fees.AsNoTracking().SumAsync(c => (decimal?)c.TotalAmount)?? 0,

                PaidAmount = await _context.ActualPayments.AsNoTracking().SumAsync(p => (decimal?)p.Amount)?? 0,

                overDueAmount = await _context.PaymentSchedules.AsNoTracking()
                    .Where(i => i.DueDate < today && i.Status == "Pending")
                    .SumAsync(i => (decimal?)i.PlannedAmount)?? 0,

                UpcomingAmount = await _context.PaymentSchedules.AsNoTracking()
                    .Where(i => i.DueDate >= today && i.Status == "Pending")
                    .SumAsync(i => (decimal?)i.PlannedAmount)?? 0
            };

            return summary;
        }
    }   
}
