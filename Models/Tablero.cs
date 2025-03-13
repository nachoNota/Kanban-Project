using System;
using System.Collections.Generic;

namespace tl2_proyecto_2024_nachoNota.Models;

public partial class Tablero
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Color { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}
