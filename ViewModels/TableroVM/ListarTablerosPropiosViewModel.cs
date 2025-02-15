namespace tl2_proyecto_2024_nachoNota.ViewModels.TableroVM
{
    public class ListarTablerosPropiosViewModel
    {
        public int IdTablero { get; set; }
        public string Titulo { get; set; }
        public string Color { get; set; }
        public string? Descripcion { get; set; }

        public ListarTablerosPropiosViewModel() { }
        public ListarTablerosPropiosViewModel(int idTablero, string titulo, string color, string descripcion)
        {
            IdTablero = idTablero;
            Titulo = titulo;
            Color = color;
            Descripcion = descripcion;
        }
    }
}
