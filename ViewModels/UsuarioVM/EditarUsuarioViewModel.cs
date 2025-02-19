using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.ViewModels.UsuarioVM
{
    public class EditarUsuarioViewModel
    {
        public int Id { get; set; }
        public string Nombre {  get; set; }
        public string Email {  get; set; }
        public RolUsuario Rol {  get; set; }

        public EditarUsuarioViewModel() { }
        public EditarUsuarioViewModel(int id, string nombre, string email, RolUsuario rol)
        {
            Id = id;
            Nombre = nombre;
            Email = email;
            Rol = rol;
        }
    }
}
