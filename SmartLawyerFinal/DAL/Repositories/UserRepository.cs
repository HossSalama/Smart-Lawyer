using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SmartLawyerFinal.DAL.Helpers;
using SmartLawyerFinal.Models;


namespace SmartLawyerFinal.DAL.Repositories
{
    public class UserRepository
    {
        public User? GetByEmail(string email)
        {
            using var conn = DbHelper.GetConnection();
            string sql = @"
                SELECT 
                    u.Id, u.FullName, u.Email,
                    u.PasswordHash, u.RoleId,
                    u.IsActive, u.CreatedAt,
                    r.RoleName
                FROM Core.Users u
                INNER JOIN Core.Roles r ON u.RoleId = r.Id
                WHERE u.Email = @Email 
                AND u.IsActive = 1";

            return conn.QueryFirstOrDefault<User>(
                sql, new { Email = email });
        }
    }
}
