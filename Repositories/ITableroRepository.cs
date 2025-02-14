using tl2_proyecto_2024_nachoNota.Models;
using ZstdSharp.Unsafe;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public interface ITableroRepository
    {
        IEnumerable<Tablero> GetAll();
        IEnumerable<Tablero> GetAllByUser(int idUsuario);
        IEnumerable<Tablero> GetTablerosConTareasAsignadas(int idUsuario);
        Tablero GetById(int id);
        IEnumerable<Tablero> GetByUser(int idUsuario);
        void Create(Tablero tablero);
        void Update(Tablero tablero);
        void Delete(int id);
    }
}
