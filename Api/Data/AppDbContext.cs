using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<ToDont> ToDont { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Image> Image { get; set; }
    }
}
