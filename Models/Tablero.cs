namespace tl2_proyecto_2024_nachoNota.Models
{
    public class Tablero
    {
        private int id;
        private int idUsuario;
        private string titulo;
        private string color;
        private string? descripcion;

        public int Id { get; }
        public int IdUsuario { get; }
        public string Titulo { get; set; }
        public string Color { get; set; }
        public string? Descripcion { get; set ; }

        public Tablero()
        {

        }

        public Tablero(int id, int idUsuario, string titulo, string color, string desc)
        {
            Id = id;
            IdUsuario = idUsuario;
            Titulo = titulo;
            Color = color;
            Descripcion = desc;
        }
    }
}
