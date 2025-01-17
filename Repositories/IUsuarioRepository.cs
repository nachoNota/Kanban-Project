using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public interface IUsuarioRepository
    {
        IEnumerable<Usuario> GetAll();
        Usuario GetById(int id);
        Usuario GetUser(string nombreUsuario, string contrasenia);
        void Create(Usuario usuario);
        void Update(int id, Usuario usuario);
        void Delete(int id);
        void ChangePassword(int id, string pass);
    }
}
