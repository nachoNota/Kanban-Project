using System.ComponentModel.DataAnnotations;

namespace tl2_proyecto_2024_nachoNota.ViewModels.UsuarioVM
{
    public class EliminarUsuarioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Se requiere una contraseña")]
        public string ContraseniaIngresada { get; set; }
        public string NombreUsuario {  get; set; }
        public string ErrorMessage { get; set; }

        public EliminarUsuarioViewModel() { }

        public EliminarUsuarioViewModel(int id, string nombre)
        {
            Id = id;
            NombreUsuario = nombre;
        }
        public EliminarUsuarioViewModel(int id)
        {
            Id = id;
        }
    }
}
