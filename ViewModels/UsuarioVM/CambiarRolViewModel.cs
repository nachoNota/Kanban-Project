using System.ComponentModel.DataAnnotations;
using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.ViewModels.UsuarioVM
{
    public class CambiarRolViewModel
    {
        [Required(ErrorMessage = "Debe asignarle un rol al usuario.")]
        public int IdRol { get; set; } = 0;
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "Debes ingresar tu contraseña para continuar.")]
        public string ContraseniaIngresada { get; set; }
        public string ContraseniaActual {  get; set; }
        public string ErrorMessage { get; set; }   
        public List<Rol> Roles { get; set; }

        public CambiarRolViewModel() { }
        public CambiarRolViewModel(IEnumerable<Rol> roles, string nombre, int id)
        {
            Roles = roles.ToList();
            NombreUsuario = nombre;
            Id = id;
        }
    }
}

