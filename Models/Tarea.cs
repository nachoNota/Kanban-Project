using System;
using System.Collections.Generic;

namespace tl2_proyecto_2024_nachoNota.Models;

public partial class Tarea
{
    public int Id { get; set; }

    public int? IdUsuario { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Color { get; set; }

    public DateTime FechaModificacion { get; set; }

    public int IdTablero { get; set; }

    public EstadoTarea Estado { get; set; }

    public virtual Tablero IdTableroNavigation { get; set; } = null!;
}
