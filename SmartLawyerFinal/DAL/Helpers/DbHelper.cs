using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SmartLawyerFinal.DAL.Helpers
{
    public static class DbHelper
    {
        public static IDbConnection GetConnection()
        {
            return new SqlConnection(AppSettings.ConnectionString);
        }
    }
}
