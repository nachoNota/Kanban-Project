using System;
using System.Collections.Generic;

namespace tl2_proyecto_2024_nachoNota.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public RolUsuario Rol { get; set; }

    public virtual ICollection<Passwordreset> Passwordresets { get; set; } = new List<Passwordreset>();

    public virtual ICollection<Tablero> Tableros { get; set; } = new List<Tablero>();
}
