using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public interface ITableroRepository
    {
        Task<IEnumerable<Tablero>> GetAll();
        Task<IEnumerable<Tablero>> GetAllByUser(int idUsuario);
        Task<IEnumerable<Tablero>> GetTablerosConTareasAsignadas(int idUsuario);
        Task<Tablero?> GetById(int id);
        Task<int> GetPropietario(int idTablero);
        Task Create(Tablero tablero);
        Task Update(Tablero tablero);
        Task Delete(int id);
    }
}
