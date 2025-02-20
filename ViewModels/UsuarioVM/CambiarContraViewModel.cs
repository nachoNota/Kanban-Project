using System.ComponentModel.DataAnnotations;

namespace tl2_proyecto_2024_nachoNota.ViewModels.UsuarioVM
{
    public class CambiarContraViewModel
    {
        [Required(ErrorMessage = "Debes ingresar tu contraseña actual")]
        public string PasswordActual { get; set; }

        [Required(ErrorMessage = "Debes ingresar una nueva contraseña")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "La contraseña debe estar entre los 5 y 15 caracteres")]
        public string PasswordNueva { get; set; }

        public int IdUsuario { get; set; }

        public string ErrorMessage { get; set; }

        public CambiarContraViewModel() { }

        public CambiarContraViewModel(string passwordActual, string passwordNueva, int id)
        {
            PasswordActual = passwordActual;
            PasswordNueva = passwordNueva;
            IdUsuario = id;
        }
    }
}
