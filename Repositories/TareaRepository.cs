using Microsoft.EntityFrameworkCore;
using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public class TareaRepository : ITareaRepository
    {
        private readonly KanbanContext _context;

        public TareaRepository(KanbanContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tarea>> GetAll() =>
            await _context.Tareas.ToListAsync();

        public async Task<Tarea?> GetById(int id) =>
            await _context.Tareas.FindAsync(id);

        public async Task<IEnumerable<Tarea>> GetByTablero(int idTablero) =>
            await _context.Tareas
                .Where(t => t.IdTablero == idTablero)
                .ToListAsync();

        public async Task AsignarUsuarioATarea(int idUsuario, int idTarea)
        {
            var tarea = await _context.Tareas.FindAsync(idTarea);
            if (tarea is null) throw new KeyNotFoundException("Tarea no encontrada.");

            tarea.IdUsuario = idUsuario;

            await _context.SaveChangesAsync();
        }

        public async Task Create(Tarea tarea)
        {
            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();
        }
        public async Task Update(Tarea tarea)
        {
            var tareaEncontrada = _context.Tareas.AsNoTracking().FirstOrDefault(t => t.Id == tarea.Id);
            
            if (tareaEncontrada is null) throw new KeyNotFoundException("Tarea no encontrada.");

            _context.Tareas.Attach(tarea); //indica que la entidad ya existe, pero no marca todos los campos como modificados

            _context.Entry(tarea).Property(t => t.Titulo).IsModified = true;
            _context.Entry(tarea).Property(t => t.Descripcion).IsModified = true;
            _context.Entry(tarea).Property(t => t.Color).IsModified = true;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);

            if (tarea is null) throw new KeyNotFoundException("Tarea no encontrada.");

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();
        }


        public async Task CambiarEstado(int idTarea, EstadoTarea estado)
        {
            var tarea = await _context.Tareas.FindAsync(idTarea);

            if (tarea is null) throw new KeyNotFoundException("Tarea no encontrada.");

            tarea.Estado = estado;

            await _context.SaveChangesAsync();
        }
    }
}
