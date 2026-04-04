using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLawyerFinal.DAL.Helpers
{
    public static class AppSettings
    {
        public static string ConnectionString =
        "Server=.\\SQLEXPRESS;Database=LegalCaseManagementDB;" +
        "Trusted_Connection=True;" +
        "TrustServerCertificate=True;";
    }
}
