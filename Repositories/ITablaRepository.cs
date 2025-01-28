using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public interface ITablaRepository
    {
        IEnumerable<Tabla> GetByTablero(int idTablero);
    }
}
