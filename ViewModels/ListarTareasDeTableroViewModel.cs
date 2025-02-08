using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.ViewModels
{
    public class ListarTareasDeTableroViewModel
    {
        public Tablero Tablero { get; set; }
        public List<ListarTareasViewModel> Tareas { get; set; }

        public ListarTareasDeTableroViewModel() { }

        public ListarTareasDeTableroViewModel(Tablero tablero, List<ListarTareasViewModel> tareas)
        {
            Tablero = tablero;
            Tareas = tareas;
        }
    }
}
