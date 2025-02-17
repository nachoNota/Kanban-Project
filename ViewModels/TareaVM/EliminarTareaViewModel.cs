namespace tl2_proyecto_2024_nachoNota.ViewModels.TareaVM
{
    public class EliminarTareaViewModel
    {
        public int IdTarea { get; set; }
        public int IdTablero { get; set; }

        public EliminarTareaViewModel() { }
        public EliminarTareaViewModel(int idTarea, int idTablero)
        {
            IdTarea = idTarea;
            IdTablero = idTablero;
        }
    }
}
