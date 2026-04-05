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
    }
}
