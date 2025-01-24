namespace LibraryLab.Models
{
    public class Utilizador
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }

        public bool IsActivated { get; set; }
        public bool IsBlocked { get; set; } 
        public string? BlockReason { get; set; } 
    }
}
