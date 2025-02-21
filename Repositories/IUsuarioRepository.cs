using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public interface IUsuarioRepository
    {
        IEnumerable<Usuario> GetAll();
        Usuario GetById(int id);
        Usuario GetByName(string nombreUsuario);
        IEnumerable<Usuario> SearchByName(string nombreUsuario);
        string GetNameById(int id);
        string GetPasswordById(int id);
        void Create(Usuario usuario);
        void Update(Usuario usuario);
        void Delete(int id);
        void ChangeRol(int idUsuario, RolUsuario rol);
        void ChangePassword(int id, string pass);
    }

}
