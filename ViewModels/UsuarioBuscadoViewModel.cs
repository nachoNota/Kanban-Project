namespace tl2_proyecto_2024_nachoNota.ViewModels
{
    public class UsuarioBuscadoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public UsuarioBuscadoViewModel() { }
        public UsuarioBuscadoViewModel(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }
    }
}
