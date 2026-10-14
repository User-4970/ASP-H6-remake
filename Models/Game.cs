namespace ASP_Core.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public double Rating { get; set; }
        public bool IsCompleted { get; set; }
    }
}
