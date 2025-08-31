using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using School.Entity.Models;
using School.Entity.Models.People;

namespace School.Infrastructure.Contexts
{
    internal class SqlDbContext
        : DbContext,
        ISqlDbContext,
        ISqlDbContextQuery
    {
        public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options)
        {
            Database = base.Database;
        }

        public DbSet<Student> Students { get; set; } = null!;

        public DbSet<Teacher> Teachers { get; set; } = null!;

        public DbSet<Class> Classes { get; set; } = null!;

        public DbSet<Address> Addresses { get; set; } = null!;

        public override DatabaseFacade Database { get; }

        IQueryable<Student> ISqlDbContextQuery.Students => Students.AsNoTracking();

        IQueryable<Teacher> ISqlDbContextQuery.Teachers => Teachers.AsNoTracking();

        IQueryable<Class> ISqlDbContextQuery.Classes => Classes.AsNoTracking();

        IQueryable<Address> ISqlDbContextQuery.Addresses => Addresses.AsNoTracking();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Human>()
                .UseTptMappingStrategy();

            modelBuilder.Entity<Human>(entity =>
            {
                entity.HasKey(h => h.Id);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasOne(e => e.Address)
                    .WithMany()
                    .HasForeignKey(e => e.AddressId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Teacher>();

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(a => a.Street)
                    .IsRequired();

                entity.Property(a => a.City)
                    .IsRequired();

                entity.Property(a => a.PostalCode)
                    .IsRequired();
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasMany(x => x.Students)
                    .WithMany(x => x.Classes)
                    .UsingEntity<Dictionary<string, object>>(
                        "ClassStudent",
                        j => j
                            .HasOne<Student>()
                            .WithMany()
                            .HasForeignKey("StudentId")
                            .OnDelete(DeleteBehavior.Cascade),
                        j => j
                            .HasOne<Class>()
                            .WithMany()
                            .HasForeignKey("ClassId")
                            .OnDelete(DeleteBehavior.Cascade),
                        j =>
                        {
                            j.HasKey("ClassId", "StudentId");
                        });
            });
        }
    }
}
