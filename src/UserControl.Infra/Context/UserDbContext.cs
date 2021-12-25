using Microsoft.EntityFrameworkCore;
using UserControl.Domain.Models;

namespace UserControl.Infra.Context
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<User>().Ignore(u => u.validationResult);
        //     base.OnModelCreating(modelBuilder);
        // }
    }
}