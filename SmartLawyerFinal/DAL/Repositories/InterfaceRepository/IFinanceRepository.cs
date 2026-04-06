using Microsoft.EntityFrameworkCore;
using SmartLawyerFinal.DTOs.FinanceDTO;
using SmartLawyerFinal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLawyerFinal.DAL.Repositories.InterfaceRepository
{
    public interface IFinanceRepository
    {
        Task<FinanceCaedsDTO> GetDashboardSummaryAsync();
        Task<List<FinanceTransactionDTO>> GetAllFinanceTransactionsAsync() ;
        Task<List<FinanceTransactionDTO>> SearchFinanceTransactionsAsync(string searchTerm);
        Task<List<FinanceTransactionDTO>> GetPaidTransactionsAsync();
        Task<List<FinanceTransactionDTO>> GetInstallmentsTransactionsAsync();
        Task<List<FinanceTransactionDTO>> GetAdminExpensesTransactionsAsync();
        Task<List<FinanceTransactionDTO>> GetOverdueTransactionsAsync();


    }
}
