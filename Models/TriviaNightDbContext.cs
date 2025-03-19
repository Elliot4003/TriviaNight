using Microsoft.EntityFrameworkCore;

namespace TriviaNight.Models
{
    public class TriviaNightDbContext : DbContext
    {
        public DbSet<QuestionModel> Questions { get; set; }

        public TriviaNightDbContext(DbContextOptions<TriviaNightDbContext> options) 
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuestionModel>().HasKey(x => x.Id);
            base.OnModelCreating(modelBuilder);
        }

    }
}
