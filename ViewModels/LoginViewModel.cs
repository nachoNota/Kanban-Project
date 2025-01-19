namespace tl2_proyecto_2024_nachoNota.ViewModels
{
    public class LoginViewModel
    {
        public string nombreUsuario { get; set; }
        public string contrasenia { get; set; }
        public bool IsAuthenticated {  get; set; }
        public string ErrorMessage { get; set; }
    }
}
