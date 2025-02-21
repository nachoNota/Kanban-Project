namespace tl2_proyecto_2024_nachoNota.ViewModels.UsuarioVM
{
	public class ConfirmarEliminarUsuarioViewModel
	{
		public int IdUsuario { get; set; }
		public string NombreUsuario { get; set; }

		public ConfirmarEliminarUsuarioViewModel(int idUsuario, string nombreUsuario)
		{
			IdUsuario = idUsuario;
			NombreUsuario = nombreUsuario;
		}
	}

}
