using Microsoft.EntityFrameworkCore;
using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly KanbanContext _context;

        public PasswordResetRepository(KanbanContext context)
        {
            _context = context;
        }

        public async Task Create(Passwordreset passwordReset)
        {
            _context.Passwordresets.Add(passwordReset);
            await _context.SaveChangesAsync();
        }

        public async Task<Passwordreset?> GetByToken(string token) =>
            await _context.Passwordresets
            .Where(x => x.Token == token)
            .FirstOrDefaultAsync();

        public async Task Delete(int id)
        {
            var passwordReset = await _context.Passwordresets.FindAsync(id);

            if (passwordReset is null) throw new KeyNotFoundException("Objeto no encontrado.");

            _context.Passwordresets.Remove(passwordReset);
            await _context.SaveChangesAsync();
        }
    }
}
