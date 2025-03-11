namespace tl2_proyecto_2024_nachoNota.Models
{
    public class PasswordReset
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

        public PasswordReset(string email)
        {
            Email = email;
            Token = Guid.NewGuid().ToString();
            Expiration = DateTime.UtcNow.AddMinutes(5);
        }
    }
}
