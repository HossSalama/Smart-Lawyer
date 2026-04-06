using SmartLawyerFinal.BLL.Services.InterfaceService;
using SmartLawyerFinal.DAL.Repositories.ClassRepository;
using SmartLawyerFinal.DAL.Repositories.InterfaceRepository;
using SmartLawyerFinal.DTOs.FinanceDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLawyerFinal.BLL.Services.ClassSevice
{
    public class FinanceService  : IFinanceService
    {
        private readonly IFinanceRepository _financeRepository;
        private readonly LegalManagementContext _context;
        public FinanceService()
        {
             _context = new LegalManagementContext();
            _financeRepository = new FinanceRepository(_context);
        }
        public FinanceService(IFinanceRepository financeRepository)
        {
            _financeRepository = financeRepository;
        }
        public async Task<FinanceCaedsDTO> GetDashboardSummaryAsync() => await _financeRepository.GetDashboardSummaryAsync();
        public async Task<List<FinanceTransactionDTO>> GetAllTransactionsAsync() => await _financeRepository.GetAllFinanceTransactionsAsync();
        public async Task<List<FinanceTransactionDTO>> GetActualPaymentsAsync() => await _financeRepository.GetPaidTransactionsAsync();
        public async Task<List<FinanceTransactionDTO>> GetUpcomingInstallmentsAsync() => await _financeRepository.GetInstallmentsTransactionsAsync();
        public async Task<List<FinanceTransactionDTO>> GetOverduePaymentsAsync() => await _financeRepository.GetOverdueTransactionsAsync();
        public async Task<List<FinanceTransactionDTO>> GetAdminExpencesAsync() => await _financeRepository.GetAdminExpensesTransactionsAsync();
        public async Task<List<FinanceTransactionDTO>> SearchAsync(string term) => await _financeRepository.SearchFinanceTransactionsAsync(term);
    }
}
