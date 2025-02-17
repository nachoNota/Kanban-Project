using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public interface ITareaRepository
    {
        IEnumerable<Tarea> GetAll();
        Tarea GetById(int id);
        IEnumerable<Tarea> GetByUser(int idUsuario);
        IEnumerable<Tarea> GetByTablero(int idTablero);
        void CambiarEstado(int idTarea, EstadoTarea estado);
        void AsignarUsuarioATarea(int idUsuario, int idTarea);
        void Create(Tarea tarea);
        void Update(Tarea tarea);
        void Delete(int id);
        IEnumerable<Tarea> GetByTablaYTablero(int idTabla, int idTablero);
    }
}
