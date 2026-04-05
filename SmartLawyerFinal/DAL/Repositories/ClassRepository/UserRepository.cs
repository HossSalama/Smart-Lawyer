

global using Dapper;
global using Microsoft.VisualBasic.ApplicationServices;
global using SmartLawyerFinal.DAL.Helpers;

namespace SmartLawyerFinal.DAL.Repositories.ClassRepository
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
