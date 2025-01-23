using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.ViewModels
{
    public class TableroModificar
    {

        private int id;
        private int idUsuario;
        private string titulo;
        private string color;
        private string? descripcion;

        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Titulo { get; set; }
        public string Color { get; set; }
        public string? Descripcion { get; set; }

        public TableroModificar()
        {

        }

        public TableroModificar(Tablero tablero)
        {
            Id = tablero.Id;
            IdUsuario = tablero.IdUsuario;
            Titulo = tablero.Titulo;
            Color = tablero.Color;
            Descripcion = tablero.Descripcion;
        }
    }
}
