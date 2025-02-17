using System.ComponentModel.DataAnnotations;

namespace tl2_proyecto_2024_nachoNota.ViewModels.TareaVM
{
    public class ModificarTareaViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe asignar un titulo a su tarea.")]
        public string Titulo { get; set; }
        public string? Descripcion { get; set; }
        public string Color { get; set; }

        public ModificarTareaViewModel() { }
        public ModificarTareaViewModel(int id, string titulo, string descripcion, string color)
        {
            Id = id;
            Titulo = titulo;
            Descripcion = descripcion;
            Color = color;
        }
    }
}
