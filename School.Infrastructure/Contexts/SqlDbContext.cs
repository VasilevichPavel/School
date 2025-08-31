using Microsoft.EntityFrameworkCore;
using School.Entity.Models;
using School.Entity.Models.People;

namespace School.Infrastructure.Contexts
{
    internal class SqlDbContext(DbContextOptions<SqlDbContext> options)
        : DbContext(options),
        ISqlDbContext,
        ISqlDbContextQuery
    {
        public DbSet<Student> Students { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Class> Classes { get; set; }

        public DbSet<Address> Addresses { get; set; }

        IQueryable<Student> ISqlDbContextQuery.Students => Students.AsNoTracking();

        IQueryable<Teacher> ISqlDbContextQuery.Teachers => Teachers.AsNoTracking();

        IQueryable<Class> ISqlDbContextQuery.Classes => Classes.AsNoTracking();

        IQueryable<Address> ISqlDbContextQuery.Addresses => Addresses.AsNoTracking();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Human>()
                .UseTptMappingStrategy();

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(e => e.StudentId)
                    .IsUnique();

                entity.HasOne(e => e.Address)
                    .WithMany()
                    .HasForeignKey(e => e.AddressId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

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
        }
    }
}
