using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using ToDo.API.Data.Seed;
using ToDo.API.Models;

namespace ToDo.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Todo>().Property(t => t.Status).HasConversion<int>();
            builder.Entity<Todo>().HasIndex(t => t.Title).IsUnique();
            builder.Entity<Todo>().HasOne(t => t.User).WithMany(u => u.Todos).HasForeignKey(t => t.UserId);

            TodoSeeds.Seed(builder);
        }
    }
}
