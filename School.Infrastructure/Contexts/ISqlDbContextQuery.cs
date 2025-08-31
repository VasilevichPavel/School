using School.Entity.Models;
using School.Entity.Models.People;

namespace School.Infrastructure.Contexts
{
    public interface ISqlDbContextQuery
    {
        IQueryable<Student> Students { get; }

        IQueryable<Teacher> Teachers { get; }

        IQueryable<Entity.Models.Class> Classes { get; }

        IQueryable<Address> Addresses { get; }
    }
}
