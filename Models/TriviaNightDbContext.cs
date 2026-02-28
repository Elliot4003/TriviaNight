using Microsoft.EntityFrameworkCore;

namespace TriviaNight.Models;

public class TriviaNightDbContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }

    public TriviaNightDbContext(DbContextOptions<TriviaNightDbContext> options) 
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserModel>().HasKey(x => x.Id);
        base.OnModelCreating(modelBuilder);
    }

}
