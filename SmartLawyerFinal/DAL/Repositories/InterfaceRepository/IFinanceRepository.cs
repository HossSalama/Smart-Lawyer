using SmartLawyerFinal.DTOs.FinanceDTO;
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
    }
}
