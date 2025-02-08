using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.ViewModels
{
    public class TablaConTareasViewModel
    {
        public Tablero Tablero {  get; set; }
        public List<Tarea> Tareas { get; set; }

        public TablaConTareasViewModel() 
        {
            Tareas = new List<Tarea>();
        }

        public TablaConTareasViewModel(Tablero tablero, List<Tarea> tareas)
        {
            Tablero = tablero;
            Tareas = tareas;
        }
    }
}
