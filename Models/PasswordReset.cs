using System;
using System.Collections.Generic;

namespace tl2_proyecto_2024_nachoNota.Models;

public partial class Passwordreset
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Token { get; set; } = null!;

    public DateTime Expiration { get; set; }

    public virtual Usuario EmailNavigation { get; set; } = null!;
}
