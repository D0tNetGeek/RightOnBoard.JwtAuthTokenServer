using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RightOnBoard.JwtAuthTokenServer.Service.Models;
using RightOnBoard.JwtAuthTokenServer.Service.Models.Entities;

namespace RightOnBoard.JwtAuthTokenServer.Service.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=DESKTOP-5E96MVL;Initial Catalog=RightOnBoard;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=true");
            optionsBuilder.UseSqlServer(@"Data Source=insyphersql1;Initial Catalog=RightOnBoard;Persist Security Info=True;User ID=rightonboard;Password=rightonboard22;MultipleActiveResultSets=true");//p_$@,83L6$z~23mW
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // it should be placed here, otherwise it will rewrite the following settings!
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            builder.Entity<UserRole>().ToTable("AspNetUserRoles");
            builder.Entity<Role>().ToTable("AspNetRoles");

            builder.Entity<UserRole>()
                .HasOne(p => p.User)
                .WithMany(b => b.UserRoles)
                .HasForeignKey(p => p.UserId);

            builder.Entity<UserRole>()
                .HasOne(x => x.Role)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(p => p.RoleId);

            // Custom application mappings
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.UserName).HasMaxLength(450).IsRequired();
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.SerialNumber).HasMaxLength(450);
            });

            builder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(450).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
            });

            builder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.RoleId);
                entity.Property(e => e.UserId);
                entity.Property(e => e.RoleId);
                entity.HasOne(d => d.Role).WithMany(p => p.UserRoles).HasForeignKey(d => d.RoleId);
                entity.HasOne(d => d.User).WithMany(p => p.UserRoles).HasForeignKey(d => d.UserId);
            });

            builder.Entity<UserToken>(entity =>
            {
                entity.HasOne(ut => ut.User)
                    .WithMany(u => u.UserTokens)
                    .HasForeignKey(ut => ut.UserId);

                entity.Property(ut => ut.RefreshTokenIdHash).HasMaxLength(450).IsRequired();
                entity.Property(ut => ut.RefreshTokenIdHashSource).HasMaxLength(450);
            });
        }
    }
}
