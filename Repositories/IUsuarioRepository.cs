using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public interface IUsuarioRepository
    {
        IEnumerable<Usuario> GetAll();
        Usuario GetById(int id);
        Usuario GetUser(string nombreUsuario, string contrasenia);
        IEnumerable<Usuario> SearchByName(string nombreUsuario);
        void Create(Usuario usuario);
        void Update(Usuario usuario);
        void Delete(int id);
        void ChangeRol(int idUsuario, int idRol);
        void ChangePassword(int id, string pass);
    }

}
