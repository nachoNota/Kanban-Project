using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public interface IRolRepository
    {
        IEnumerable<Rol> GetAll();
        Rol GetById(int idRol);
        void Create(string nombreRol);
        void Update(int id, string nombreRol);
        void Delete(int id);
    }
}
