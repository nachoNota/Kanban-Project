using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public interface ITareaRepository
    {
        IEnumerable<Tarea> GetAll();
        Tarea GetById(int id);
        IEnumerable<Tarea> GetByUser(int idUsuario);
        IEnumerable<Tarea> GetByTablero(int idTablero);

        void AsignarUsuarioATarea(int idUsuario, int idTarea);
        void Create(int idTablero, Tarea tablero);
        void Update(int id, Tarea tablero);
        void Delete(int id);
        IEnumerable<Tarea> GetByTablaYTablero(int idTabla, int idTablero);
    }
}
