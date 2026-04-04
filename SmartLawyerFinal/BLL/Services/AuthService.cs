using SmartLawyerFinal.DAL.Repositories;
using SmartLawyerFinal.Models;

namespace SmartLawyerFinal.BLL.Services
{
    public class AuthService
    {
        private readonly UserRepository _repo;
        public static User? CurrentUser { get; private set; }

        public AuthService()
        {
            _repo = new UserRepository();
        }

        public bool Login(string email, string password, out string message)
        {
            message = "";

            if (string.IsNullOrWhiteSpace(email))
            {
                message = "من فضلك ادخل البريد الإلكتروني";
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                message = "من فضلك ادخل كلمة المرور";
                return false;
            }

            var user = _repo.GetByEmail(email);

            if (user == null)
            {
                message = "البريد الإلكتروني غير موجود";
                return false;
            }

            bool valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!valid)
            {
                message = "كلمة المرور غير صحيحة";
                return false;
            }

            CurrentUser = user;
            return true;
        }
        public static string GenerateHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static void Logout()
        {
            CurrentUser = null;
        }

        public static bool IsAdmin() =>
            CurrentUser?.RoleId == 1;

        public static bool IsLawyer() =>
            CurrentUser?.RoleId == 2;
    }
}