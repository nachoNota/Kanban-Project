namespace tl2_proyecto_2024_nachoNota.ViewModels
{
    public class BuscarUsuarioViewModel
    {
        private int idUsuario;
        private string nombre;

        public int IdUsuario {  get; }
        public string Nombre { get; }

        public BuscarUsuarioViewModel(int idUsuario, string nombre)
        {
            this.idUsuario = idUsuario;
            this.nombre = nombre;
        }
    }
}
