using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAll();
        Task<Usuario?> GetById(int id);
        Task<Usuario?> GetByName(string nombreUsuario);
        Task<bool> ExistsByEmail(string email);
        Task<Usuario?> GetByEmail(string email);
        Task<IEnumerable<Usuario>> SearchByName(string nombreUsuario);
        Task<bool> Exists(int id);
        Task<string?> GetNameById(int? id);
        Task<string?> GetPasswordById(int id);
        Task Create(Usuario usuario);
        Task Update(Usuario usuario);
        Task Delete(int id);
        Task ChangeRol(int idUsuario, RolUsuario rol);
        Task ChangePassword(int id, string pass);
    }

}
