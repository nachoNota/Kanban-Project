using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.ViewModels
{
    public class BuscarViewModel
    {
        private int idTablero;
        private IEnumerable<BuscarUsuarioViewModel> usuarios;

        public int IdTablero { get; set; }
        public IEnumerable<BuscarUsuarioViewModel> Usuarios { get; }

        public BuscarViewModel(int idTablero, IEnumerable<BuscarUsuarioViewModel> usuarios)
        {
            this.idTablero = idTablero;
            this.usuarios = usuarios;
        }
        public BuscarViewModel()
        {
        }

    }
}
