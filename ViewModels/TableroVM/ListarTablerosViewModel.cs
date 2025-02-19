namespace tl2_proyecto_2024_nachoNota.ViewModels.TableroVM
{
    public class ListarTablerosViewModel
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Titulo { get; set; }
        public string Color { get; set; }
        public string? Descripcion { get; set; }

        public ListarTablerosViewModel() { }
        public ListarTablerosViewModel(int idTablero, string titulo, string color, string descripcion)
        {
            Id = idTablero;
            Titulo = titulo;
            Color = color;
            Descripcion = descripcion;
        }
    }
}
