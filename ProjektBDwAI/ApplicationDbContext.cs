using Microsoft.EntityFrameworkCore;
using ProjektBDwAI.Models;

namespace ProjektBDwAI
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Survey> Surveys { get; set;}
        public DbSet<Question> Questions { get; set; }
        public DbSet<ProjektBDwAI.Models.Result> Results { get; set; }
        public DbSet<UserResult> UserResults { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => {
                entity.HasKey(k => k.Id);
                });

            modelBuilder.Entity<User>()
                .HasMany(u => u.Surveys)
                .WithOne(s => s.Owner)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Survey>()
                .HasMany(s => s.Questions)
                .WithOne(q => q.Survey)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Survey>()
                .HasMany(s => s.Results)
                .WithOne(r => r.Survey)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Result>()
                .HasMany(r => r.UserResults)
                .WithOne(ur => ur.Result)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.UserResults)
                .WithOne(ur => ur.Question)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);
        }
    }
}
