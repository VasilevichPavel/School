using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using School.Entity.Models;
using School.Entity.Models.People;

namespace School.Infrastructure.Contexts
{
    public interface ISqlDbContext
    {
        DbSet<Student> Students { get; }

        DbSet<Teacher> Teachers { get; }

        DbSet<Entity.Models.Class> Classes { get; }

        DbSet<Address> Addresses { get; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
