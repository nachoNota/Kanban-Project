using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.ViewModels.TareaVM
{
    public class DetalleTareaViewModel
    {
        public int Id { get; set; }
        public int? IdUsuario { get; set; }
        public int IdTablero { get; set; }
        public int IdPropietarioTablero { get; set; }
        public string NombreUsuario { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public string Color { get; set; }
        public DateTime FechaModificacion { get; set; }
        public EstadoTarea Estado { get; set; }

        public DetalleTareaViewModel() { }

        public DetalleTareaViewModel(Tarea tarea, string nombreUsuario, int idPropietarioTablero)
        {
            Id = tarea.Id;
            IdUsuario = tarea.IdUsuario;
            IdTablero = tarea.IdTablero;
            Titulo = tarea.Titulo;
            Descripcion = tarea.Descripcion;
            Color = tarea.Color;
            FechaModificacion = tarea.FechaModificacion;
            Estado = tarea.Estado;
            NombreUsuario = nombreUsuario;
            IdPropietarioTablero = idPropietarioTablero;
        }

    }
}
