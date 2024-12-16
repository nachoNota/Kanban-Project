namespace tl2_proyecto_2024_nachoNota.Models
{
    public class Rol
    {
        private int id;
        private string nombreRol;

        public int Id { get; }
        public string NombreRol { get; set; }
    
        public void AsignarId(int id)
        {
            this.id = id;
        }
    }
}
