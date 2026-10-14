namespace ASP_Core.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        // Store hashed password (simple SHA256 hash used in example)
        public string PasswordHash { get; set; }
    }
}
