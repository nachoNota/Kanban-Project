using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public interface IPasswordResetRepository
    {
        Task Create(Passwordreset passwordReset);
        Task<Passwordreset> GetByToken(string token);
        Task Delete(int id);
    }
}
