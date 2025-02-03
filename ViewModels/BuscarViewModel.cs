using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.ViewModels
{
    public class BuscarViewModel
    {
        public int IdTablero { get; set; }
        public string Usuario { get; set; }
        public List<Usuario> Usuarios { get; set; } 

        public BuscarViewModel() { }

        public BuscarViewModel(int idTablero) 
        {
            IdTablero = idTablero;
            Usuarios = new List<Usuario>();
        }

    }
}
