using Microsoft.EntityFrameworkCore;
using TaskManager.Shared;

namespace TaskManager.Server.Infrastructure
{
    public class TodoDbContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }

        public TodoDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
