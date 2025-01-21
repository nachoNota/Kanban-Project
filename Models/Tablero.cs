namespace tl2_proyecto_2024_nachoNota.Models
{
    public class Tablero
    {
        private int id;
        private int idUsuario;
        private string titulo;

        public int Id { get; }
        public int IdUsuario { get; }
        public string Titulo { get; set; }

        public Tablero(int id, int idUsuario, string titulo)
        {
            Id = id;
            IdUsuario = idUsuario;
            Titulo = titulo;
        }
    }
}
