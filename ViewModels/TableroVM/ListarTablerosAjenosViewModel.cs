namespace tl2_proyecto_2024_nachoNota.ViewModels.TableroVM
{
    public class ListarTablerosAjenosViewModel
    {
        public int IdTablero { get; set; }
        public string Titulo { get; set; }
        public string Color {  get; set; }
        public string NombrePropietario { get; set; }
    
        public ListarTablerosAjenosViewModel() { }
        public ListarTablerosAjenosViewModel(int idTablero, string titulo, string color, string nombrePropietario)
        {
            IdTablero = idTablero;
            Titulo = titulo;
            Color = color;
            NombrePropietario = nombrePropietario;
        }
    }
}
