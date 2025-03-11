using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public interface IPasswordResetRepository
    {
        void Create(PasswordReset passwordReset);
    }
}
