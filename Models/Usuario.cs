namespace tl2_proyecto_2024_nachoNota.Models
{
    public class Usuario
    {
        private int id;
        private int idRol;
        private string nombreUsuario;
        private string password;

        public int Id { get; }
        public int IdRol { get; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; }

        public Usuario(int id, int idRol, string nombreUsuario, string password)
        {
            Id = id;
            IdRol = idRol;
            NombreUsuario = nombreUsuario;
            Password = password;
        }
    }
}
