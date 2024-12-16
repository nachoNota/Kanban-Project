namespace tl2_proyecto_2024_nachoNota.Models
{
    public class Tarea
    {
        private int id;
        private int idUsuario;
        private int idTabla;
        private string titulo;
        private string? descripcion;
        private byte[]? imagen;

        public int Id { get; }
        public int IdUsuario { get; }
        public int IdTabla { get; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public byte[]? Imagen { get; set; }

        public void AsignarId(int id)
        {
            this.id = id;
        }

        public void AsignarUsuario(int idUsuario)
        {
            this.idUsuario = idUsuario;
        }

        public void AsignarTabla(int idTabla)
        {
            this.idTabla = idTabla;
        }
    }
}
