using Microsoft.EntityFrameworkCore;
using ASP_Core.Models;

namespace ASP_Core.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<AppUser> Users { get; set; }
    }
}
