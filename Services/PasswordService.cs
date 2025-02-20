using Microsoft.AspNetCore.Identity;

namespace tl2_proyecto_2024_nachoNota.Services
{

    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string inputPassword);
    }

    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<object> _passwordHasher = new();
        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string hashedPassword, string inputPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, inputPassword);

            return result == PasswordVerificationResult.Success;
        }
    }
}
