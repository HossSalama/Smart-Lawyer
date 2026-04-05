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
        public FinanceService()
        {
            var context = new LegalManagementContext();
            _financeRepository = new FinanceRepository(context);
        }
        public FinanceService(IFinanceRepository financeRepository)
        {
            _financeRepository = financeRepository;
        }
        public async Task<FinanceCaedsDTO> GetDashboardSummaryAsync()
        {
        
            return await _financeRepository.GetDashboardSummaryAsync();
        }
    }
}
