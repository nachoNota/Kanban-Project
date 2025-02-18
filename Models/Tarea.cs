namespace tl2_proyecto_2024_nachoNota.Models
{

    public class Tarea
    {
        private int id;
        private int idUsuario;
        private int idTablero;
        private EstadoTarea estado;
        private string titulo;
        private string? descripcion;
        private string color;
        private DateTime fechaModificacion;
        
        public int Id { get => id; set => id = value; }
        public int IdUsuario { get; set; }
        public int IdTablero { get; set; }
        public string? Titulo { get => titulo; set => titulo = value; }
        public string? Descripcion { get => descripcion; set => descripcion = value; }
        public string Color { get => color; set => color = value; }
        public DateTime FechaModificacion { get => fechaModificacion; set => fechaModificacion = value; }
        public EstadoTarea Estado { get => estado; set => estado = value; }

        public Tarea() { }

        public Tarea(int id, string titulo, string? descripcion, string color)
        {
            Id = id;
            Titulo = titulo;
            Descripcion = descripcion;
            Color = color;
        }

        public Tarea(int idUsuario, int idTablero, string titulo, string descripcion, string color) 
        {
            IdUsuario = idUsuario;
            IdTablero = idTablero;
            Titulo = titulo;
            Descripcion = descripcion;
            Color = color;
            Estado = EstadoTarea.Ideas;
        }

    }
}
