namespace tl2_proyecto_2024_nachoNota.Models
{
    public enum EstadoTarea
    {
        Ideas = 1,
        ToDo,
        Doing,
        Review,
        Done
    }

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
    }
}
