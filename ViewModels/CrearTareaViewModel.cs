using System.ComponentModel.DataAnnotations;
using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.ViewModels
{
    public class CrearTareaViewModel
    {
        public int IdUsuario { get; set; }
        public int IdTablero { get; set; }
        [Required(ErrorMessage = "Debes ingreasar un titulo para tu tarea.")]
        public string? Titulo { get ; set ; }
        public string? Descripcion { get ; set ; }
        public string Color { get ; set ; }
        public EstadoTarea Estado { get ; set ; }
    
        public CrearTareaViewModel() { }

        public CrearTareaViewModel(int idTablero)
        {
            IdTablero = idTablero;
        }
    }
}
