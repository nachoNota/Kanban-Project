using System.ComponentModel.DataAnnotations;
using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.ViewModels.TableroVM
{
    public class ModificarTableroViewModel
    {

        private int id;
        private int idUsuario;
        private string titulo;
        private string color;
        private string? descripcion;

        public int Id { get; set; }
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "Debes ingresar un titulo para el tablero.")]
        public string Titulo { get; set; }
        public string Color { get; set; }
        public string? Descripcion { get; set; }

        public ModificarTableroViewModel()
        {

        }

        public ModificarTableroViewModel(Tablero tablero)
        {
            Id = tablero.Id;
            IdUsuario = tablero.IdUsuario;
            Titulo = tablero.Titulo;
            Color = tablero.Color;
            Descripcion = tablero.Descripcion;
        }
    }
}
