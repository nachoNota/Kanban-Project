namespace tl2_proyecto_2024_nachoNota.Models
{
    public class Tabla
    {
        private int id;
        private int idUsuario;
        private int idTablero;
        private string titulo;

        public int Id { get; }
        public int IdUsuario { get; }
        public int IdTablero { get; }
        public string Titulo { get; set; }
    }
}
