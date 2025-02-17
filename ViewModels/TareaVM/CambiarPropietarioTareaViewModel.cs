namespace tl2_proyecto_2024_nachoNota.ViewModels.TareaVM
{
    public class CambiarPropietarioTareaViewModel
    {
        public int IdTarea { get; set; }
        public List<UsuarioBuscadoViewModel> Usuarios { get; set; }

        public CambiarPropietarioTareaViewModel() { }
        public CambiarPropietarioTareaViewModel(int idTarea, List<UsuarioBuscadoViewModel> usuarios)
        {
            IdTarea = idTarea;
            Usuarios = usuarios;
        }
    }
}
