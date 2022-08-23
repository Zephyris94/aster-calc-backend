using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        public DbSet<MoveType> MoveTypes { get; set; }

        public DbSet<NodeType> NodeTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Node>()
                .HasKey(x => x.Id);
            builder.Entity<Node>()
                .Property(x => x.Id).UseIdentityColumn();
            builder.Entity<Node>()
                .Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<Node>()
                .HasOne(x => x.NodeType);

            builder.Entity<Route>()
                .HasKey(x => x.Id);
            builder.Entity<Route>()
                .Property(x => x.Id).UseIdentityColumn();
            builder.Entity<Route>()
               .Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<Route>()
                .HasOne(x => x.Destination);
            builder.Entity<Route>()
                .HasOne(x => x.Source);
            builder.Entity<Route>()
                .HasOne(x => x.MoveType);

            builder.Entity<Calculation>()
                .HasKey(x => x.Id);
            builder.Entity<Calculation>()
                .HasMany(x => x.Routes)
                .WithMany(x => x.Calculations);

            builder.Entity<MoveType>()
                .HasKey(x => x.Id);
            builder.Entity<MoveType>()
                .Property(et => et.Id)
                .ValueGeneratedNever();

            builder.Entity<NodeType>()
               .HasKey(x => x.Id);
            builder.Entity<NodeType>()
                .Property(et => et.Id)
                .ValueGeneratedNever();
        }

        public void VerifyEnums()
        {
            if(!NodeTypes.Any())
            {
                VerifyEnum<Model.Domain.NodeType, NodeType>(NodeTypes);
            }

            if (!MoveTypes.Any())
            {
                VerifyEnum<Model.Domain.MoveType, MoveType>(MoveTypes);
            }
        }

        private void VerifyEnum<TEnum, TEntity>(DbSet<TEntity> entities) 
            where TEnum : struct, IConvertible
            where TEntity : EnumDbModel
        {
            System.Type enumType = typeof(TEnum);
            System.Array enumValues = System.Enum.GetValues(enumType);

            for (int i = 0; i < enumValues.Length; i++)
            {
                // Retrieve the value of the ith enum item.
                string value = enumValues.GetValue(i).ToString();

                TEntity entity = Activator.CreateInstance<TEntity>();

                entity.Id = i;
                entity.Name = value;

                entities.Add(entity);
                SaveChanges();
            }
        }
    }
}
