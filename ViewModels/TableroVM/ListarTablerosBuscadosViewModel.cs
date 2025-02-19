namespace tl2_proyecto_2024_nachoNota.ViewModels.TableroVM
{
    public class ListarTablerosBuscadosViewModel
    {
        public string NombreUsuario {  get; set; }
        public List<ListarTablerosViewModel> Tableros {  get; set; }

        public ListarTablerosBuscadosViewModel() { }

        public ListarTablerosBuscadosViewModel(string nombreUsuario, List<ListarTablerosViewModel> tableros)
        {
            Tableros = tableros;
            NombreUsuario = nombreUsuario;
        }
    }
}
