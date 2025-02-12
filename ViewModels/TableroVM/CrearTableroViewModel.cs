using System.ComponentModel.DataAnnotations;
using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.ViewModels.TableroVM
{
    public class CrearTableroViewModel
    {
        private int idUsuario;
        private string titulo;
        private string color;
        private string? descripcion;

        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "Debes ingresar un titulo para el tablero.")]
        public string Titulo { get; set; }
        public string Color { get; set; }
        public string? Descripcion { get; set; }

        public CrearTableroViewModel()
        {

        }

        public CrearTableroViewModel(Tablero tablero)
        {
            IdUsuario = tablero.IdUsuario;
            Titulo = tablero.Titulo;
            Color = tablero.Color;
            Descripcion = tablero.Descripcion;
        }
    }
}
