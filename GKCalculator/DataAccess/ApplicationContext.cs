using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Node> Nodes { get; set; }

        public DbSet<Route> Routes { get; set; }

        public DbSet<Calculation> Calculations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Node>()
                .HasKey(x => x.Id);

            builder.Entity<Route>()
                .HasKey(x => x.Id);
            builder.Entity<Route>()
                .HasOne(x => x.Destination);
            builder.Entity<Route>()
                .HasOne(x => x.Source);

            builder.Entity<Calculation>()
                .HasKey(x => x.Id);
            builder.Entity<Calculation>()
                .HasMany(x => x.Routes)
                .WithMany(x => x.Calculations);
        }
    }
}
