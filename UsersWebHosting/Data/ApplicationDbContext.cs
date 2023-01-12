using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using UsersWebHosting.Data.Models;

namespace UsersWebHosting.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AvatarImage> Avatars { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AvatarImage>(e => 
            {
                e.HasKey("Id");
                e.ToTable("AvatarImage");
            });

            builder.Entity<User>().HasOne(s => s.Avatar).WithOne(s => s.User);
        }
    }
}