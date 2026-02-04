using Microsoft.EntityFrameworkCore;
using ChatApp.Models;


namespace ChatApp.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Message> Messages { get; set; } = null!;
    }
}
