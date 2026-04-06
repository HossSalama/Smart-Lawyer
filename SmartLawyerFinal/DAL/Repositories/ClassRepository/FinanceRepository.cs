
using Microsoft.EntityFrameworkCore;
using SmartLawyerFinal.DAL.Repositories.InterfaceRepository;
using SmartLawyerFinal.DTOs.FinanceDTO;
using SmartLawyerFinal.Enums;
using System.Data;

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
                    .Where(i => i.DueDate < today && i.Status == PaymentStatus.Pending)
                    .SumAsync(i => (decimal?)i.PlannedAmount)?? 0,

                UpcomingAmount = await _context.PaymentSchedules.AsNoTracking()
                    .Where(i => i.DueDate >= today && i.Status == PaymentStatus.Pending)
                    .SumAsync(i => (decimal?)i.PlannedAmount)?? 0
            };

            return summary;
        }
        public async Task<List<FinanceTransactionDTO>> GetAllFinanceTransactionsAsync() => await GetFilteredFinanceAsync(null);

        public async Task<List<FinanceTransactionDTO>> SearchFinanceTransactionsAsync(string searchTerm) => await GetFilteredFinanceAsync(searchTerm); 
        public async Task<List<FinanceTransactionDTO>> GetPaidTransactionsAsync()
        {
            var all = await GetAllFinanceTransactionsAsync();
            return all.Where(x => x.Status == PaymentStatus.Paid ||
                          x.PaymentType == TransactionType.ActualPayment)
              .OrderByDescending(x => x.PaymentDate)
              .ToList();
        }
        public async Task<List<FinanceTransactionDTO>> GetInstallmentsTransactionsAsync()
        {
            var all = await GetAllFinanceTransactionsAsync();
            return all
                .Where(x => x.PaymentType == TransactionType.Installment)
                .OrderByDescending(x => x.PaymentDate)
                .ToList();
        }

        public async Task<List<FinanceTransactionDTO>> GetAdminExpensesTransactionsAsync()
        {
            var all = await GetAllFinanceTransactionsAsync();
            return all
                .Where(x => x.PaymentType == TransactionType.AdminExpense)
                .OrderByDescending(x => x.PaymentDate) 
                .ToList();
        }
        public async Task<List<FinanceTransactionDTO>> GetOverdueTransactionsAsync()
        {
            var allTransactions = await GetAllFinanceTransactionsAsync();
            return allTransactions
                .Where(x => x.Status == PaymentStatus.Overdue)
                .OrderByDescending(x => x.PaymentDate)
                .ToList();
        }
        private async Task<List<FinanceTransactionDTO>> GetFilteredFinanceAsync(string term)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            bool hasFilter = !string.IsNullOrWhiteSpace(term);
            string filter = term?.ToLower().Trim();

            var actualQuery = _context.ActualPayments.AsNoTracking().AsQueryable();
            if (hasFilter)
                actualQuery = actualQuery.Where(p => p.Fee.Case.CaseNumber.Contains(filter) || p.Fee.Client.FullName.Contains(filter));

            var installmentQuery = _context.PaymentSchedules.AsNoTracking().AsQueryable();
            if (hasFilter)
                installmentQuery = installmentQuery.Where(i => i.Fee.Case.CaseNumber.Contains(filter) || i.Fee.Case.Client.FullName.Contains(filter));

            var expenseQuery = _context.AdminExpenses.AsNoTracking().AsQueryable();
            if (hasFilter)
                expenseQuery = expenseQuery.Where(e => e.Case.CaseNumber.Contains(filter) || e.Case.Client.FullName.Contains(filter));
            var actualResults = await actualQuery.Select(p => new FinanceTransactionDTO
            {
                Id = p.Id,
                ClientName = p.Fee.Client.FullName,
                CaseNumber = p.Fee.Case.CaseNumber,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentType = TransactionType.ActualPayment,
                Status = PaymentStatus.Paid
            }).ToListAsync();

            var installmentResults = await installmentQuery.Select(i => new FinanceTransactionDTO
            {
                Id = i.Id,
                ClientName = i.Fee.Case.Client.FullName,
                CaseNumber = i.Fee.Case.CaseNumber,
                Amount = i.PlannedAmount,
                PaymentDate = i.DueDate,
                PaymentType = TransactionType.Installment,
                Status = (i.Status == PaymentStatus.Paid) ? PaymentStatus.Paid :
                 (i.DueDate < today ? PaymentStatus.Overdue : PaymentStatus.Pending)
            }).ToListAsync();

            var expenseResults = await expenseQuery.Select(e => new FinanceTransactionDTO
            {
                Id = e.Id,
                ClientName = e.Case.Client.FullName,
                CaseNumber = e.Case.CaseNumber,
                Amount = e.Amount,
                PaymentDate = e.ExpenseDate,
                PaymentType = TransactionType.AdminExpense,
                Status = PaymentStatus.Paid
            }).ToListAsync();

            return actualResults.Concat(installmentResults).Concat(expenseResults)
                                .OrderByDescending(x => x.PaymentDate).ToList();
        }
    }   
}
