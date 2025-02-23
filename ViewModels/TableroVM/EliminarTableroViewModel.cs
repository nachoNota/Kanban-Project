namespace tl2_proyecto_2024_nachoNota.ViewModels.TableroVM
{
    public class EliminarTableroViewModel
    {
        public int IdUsuario { get; set; }
        public int IdTablero {  get; set; }
        public string ReturnUrl { get; set; }

        public EliminarTableroViewModel() { }
        public EliminarTableroViewModel(int idTablero, int idUsuario, string returnUrl)
        {
            IdUsuario = idUsuario;
            IdTablero = idTablero;
            ReturnUrl = returnUrl;
        }
    }
}
