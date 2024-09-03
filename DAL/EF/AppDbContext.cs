using DAL.Models;
using DAL.Models.Users;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext : IdentityDbContext<User, Role, string,
    IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Movie> Movies { get; set; }
        //public DbSet<Genre> Genres { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<MovieRating> Ratings { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        //public DbSet<TranslatableEntityField> TranslatableEntityFields { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            builder.Entity<UserRole>()
                .HasOne(x => x.User)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            builder.Entity<UserRole>()
                .HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .IsRequired();

            builder.Entity<Movie>()
                .Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(256);

            builder.Entity<Person>()
                .Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(64);

            builder.Entity<Person>()
                .Property(x => x.LastName)
                .IsRequired(false)
                .HasMaxLength(64);

            builder.Entity<Comment>()
                .Property(x => x.Text)
                .IsRequired()
                .HasMaxLength(5000);

            builder.Entity<Subscription>()
            .HasOne(s => s.Plan)
            .WithMany()
            .HasForeignKey(s => s.SubscriptionPlanId)
            .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<TranslatableEntity>()
            //    .ToTable("TranslatableEntities");

            //builder.Entity<Genre>()
            //    .ToTable("Genres");

            foreach (var x in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                x.DeleteBehavior = DeleteBehavior.ClientCascade;
        }
    }
}