using System.ComponentModel.DataAnnotations;

namespace tl2_proyecto_2024_nachoNota.ViewModels.TableroVM
{
    public class ModificarTableroViewModel
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "Debes ingresar un titulo para el tablero.")]
        [StringLength(20, ErrorMessage = "El titulo debe ser menor a los 20 caracteres.")]
        public string Titulo { get; set; }
        public string Color { get; set; }
        [StringLength(100, ErrorMessage = "La descripción debe ser menor a los 100 caracteres.")]
        public string? Descripcion { get; set; }

        public ModificarTableroViewModel()
        {

        }
        public ModificarTableroViewModel(int id, int idUsuario, string titulo, string color, string desc)
        {
            Id = id;
            IdUsuario = idUsuario;
            Titulo = titulo;
            Color = color;
            Descripcion = desc;
        }

        public ModificarTableroViewModel(ListarTablerosViewModel tableroVM)
        {
            Id = tableroVM.IdTablero;
            IdUsuario = tableroVM.IdUsuario;
            Titulo = tableroVM.Titulo;
            Color = tableroVM.Color;
            Descripcion = tableroVM.Descripcion;
        }
    }
}
