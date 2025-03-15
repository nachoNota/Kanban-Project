using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.ViewModels.TareaVM
{
    public class ListarTareasViewModel
    {
        public int IdTarea { get; set; }
        public int? IdUsuario { get; set; }
        public string? Titulo { get; set; }
        public string Color { get; set; }
        public EstadoTarea Estado { get; set; }

        public ListarTareasViewModel() { }

        public ListarTareasViewModel(Tarea tarea)
        {
            IdTarea = tarea.Id;
            IdUsuario = tarea.IdUsuario;
            Titulo = tarea.Titulo;
            Color = tarea.Color;
            Estado = tarea.Estado;
        }
    }
}
