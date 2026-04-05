
global using SmartLawyerFinal.DAL.Repositories;
global using SmartLawyerFinal.Models;
global using User = SmartLawyerFinal.Models.User;
using SmartLawyerFinal.DAL.Repositories.ClassRepository;

namespace SmartLawyerFinal.BLL.Services.ClassSevice
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
                message = "ãä ÝÖáß ÇÏÎá ÇáÈÑíÏ ÇáÅáßÊÑæäí";
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                message = "ãä ÝÖáß ÇÏÎá ßáãÉ ÇáãÑæÑ";
                return false;
            }

            var user = _repo.GetByEmail(email);

            if (user == null)
            {
                message = "ÇáÈÑíÏ ÇáÅáßÊÑæäí ÛíÑ ãæÌæÏ";
                return false;
            }

            bool valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!valid)
            {
                message = "ßáãÉ ÇáãÑæÑ ÛíÑ ÕÍíÍÉ";
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