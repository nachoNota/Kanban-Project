namespace tl2_proyecto_2024_nachoNota.Models
{
    public class Tablero
    {
        private int id;
        private int idUsuario;
        private string titulo;
        private string? descripcion;

        public int Id { get; }
        public int IdUsuario { get; }
        public string Titulo { get; set; }
        public string? Descripcion { get; set; }

        public void AsignarId(int id)
        {
            this.id = id;
        }

        public void AsignarUsuario(int idUsuario)
        {
            this.idUsuario = idUsuario;
        }
    }
}
