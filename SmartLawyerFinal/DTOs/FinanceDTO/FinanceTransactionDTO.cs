using SmartLawyerFinal.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartLawyerFinal.DTOs.FinanceDTO
{
    public class FinanceTransactionDTO
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string CaseNumber { get; set; }
        public decimal Amount { get; set; }
        public DateOnly PaymentDate { get; set; }
        public TransactionType PaymentType { get; set; }
        public PaymentStatus Status { get; set; }
        public string TypeName => GetDisplayName(PaymentType);
        public string StatusName => GetDisplayName(Status);
        private string GetDisplayName(Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .FirstOrDefault()
                            ?.GetCustomAttribute<DisplayAttribute>()
                            ?.Name ?? enumValue.ToString();
        }
    }
               
}
