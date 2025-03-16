using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public interface ITareaRepository
    {
        Task<IEnumerable<Tarea>> GetAll();
        Task<Tarea?> GetById(int id);
        Task<IEnumerable<Tarea>> GetByTablero(int idTablero);
        Task CambiarEstado(int idTarea, EstadoTarea estado);
        Task AsignarUsuarioATarea(int idUsuario, int idTarea);
        Task Create(Tarea tarea);
        Task Update(Tarea tarea);
        Task Delete(int id);
    }
}
