using Microsoft.EntityFrameworkCore;
using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public class TableroRepository : ITableroRepository
    {
        private readonly KanbanContext _context;

        public TableroRepository(KanbanContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tablero>> GetAll() =>
            await _context.Tableros.ToListAsync();

        public async Task<IEnumerable<Tablero>> GetAllByUser(int idUsuario) =>
            await _context.Tableros
                .Where(t => t.IdUsuario == idUsuario)
                .ToListAsync();
        
        public async Task<IEnumerable<Tablero>> GetTablerosConTareasAsignadas(int idUsuario) =>
            await _context.Tableros
                .Where(tab => tab.Tareas.Any(tar => tar.IdUsuario == idUsuario) && tab.IdUsuario != idUsuario) //WHERE tar.id_usuario = @idUsuario AND tab.id_usuario <> @idUsuario"
                .Include(t => t.IdUsuarioNavigation)    
                .Distinct()
                .ToListAsync();
 
        public async Task<Tablero?> GetById(int id) =>
            await _context.Tableros.FindAsync(id);

        public async Task Create(Tablero tablero)
        {
            _context.Tableros.Add(tablero);
            await _context.SaveChangesAsync();
        }
        
        public async Task Delete(int id)
        {
            var tablero = await _context.Tableros.FindAsync(id);

            if (tablero is null) throw new KeyNotFoundException("Tablero no encontrado");
            
            _context.Tableros.Remove(tablero);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Tablero tablero)
        {
            var tableroEncontrado = await _context.Tableros
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(t => t.Id == tablero.Id);

            if (tableroEncontrado is null) throw new KeyNotFoundException("Tablero no encontrado");

            _context.Tableros.Update(tablero);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetPropietario(int idTablero) =>
            await _context.Tableros
                .Where(t => t.Id == idTablero)
                .Select(t => t.IdUsuario)
                .FirstOrDefaultAsync();
    }
}
