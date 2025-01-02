using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Storage
{
    public class InMemoryDbContext : DbContext
    {
        public DbSet<TaskItem> TaskItems { get; set; } = null!;

        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options) { }
    }
}
