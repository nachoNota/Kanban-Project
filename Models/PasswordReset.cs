using System;
using System.Collections.Generic;

namespace tl2_proyecto_2024_nachoNota.Models;

public partial class Passwordreset
{
    public int Id { get; set; }

    public string Token { get; set; } = null!;

    public DateTime Expiration { get; set; }

    public int IdUsuario { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
