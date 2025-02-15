using System.ComponentModel.DataAnnotations;
using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.ViewModels.TableroVM
{
    public class ModificarTableroViewModel
    {

        private int id;
        private string titulo;
        private string color;
        private string? descripcion;

        public int Id { get; set; }
        [Required(ErrorMessage = "Debes ingresar un titulo para el tablero.")]
        public string Titulo { get; set; }
        public string Color { get; set; }
        public string? Descripcion { get; set; }

        public ModificarTableroViewModel()
        {

        }

        public ModificarTableroViewModel(ListarTablerosPropiosViewModel tableroVM)
        {
            Id = tableroVM.IdTablero;
            Titulo = tableroVM.Titulo;
            Color = tableroVM.Color;
            Descripcion = tableroVM.Descripcion;
        }
    }
}
