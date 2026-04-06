using SmartLawyerFinal.DTOs.FinanceDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLawyerFinal.BLL.Services.InterfaceService
{
    public interface IFinanceService
    {
        Task<FinanceCaedsDTO> GetDashboardSummaryAsync();
        Task<List<FinanceTransactionDTO>> GetAllTransactionsAsync();
        Task<List<FinanceTransactionDTO>> GetActualPaymentsAsync();
        Task<List<FinanceTransactionDTO>> GetUpcomingInstallmentsAsync();
        Task<List<FinanceTransactionDTO>> GetOverduePaymentsAsync();
        Task<List<FinanceTransactionDTO>> GetAdminExpencesAsync() ;
        Task<List<FinanceTransactionDTO>> SearchAsync(string term);
    }
}
