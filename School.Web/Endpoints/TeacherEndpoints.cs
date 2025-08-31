using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Core.Exceptions;
using School.Entity.Models.People;
using School.Infrastructure.Contexts;

namespace School.Web.Endpoints
{
    public static class TeacherEndpoints
    {
        public static void MapTeacherEndpoints(this IEndpointRouteBuilder routes)
        {
            var endpoints = routes.MapGroup("/api/teachers");

            endpoints.MapGet("/", GetTeachers);
            endpoints.MapGet("/{id:int}", GetTeacher);
            endpoints.MapPut("/", UpdateTeacher);
        }

        private static async Task<List<Teacher>> GetTeachers(
            [FromServices] ISqlDbContextQuery context,
            CancellationToken cancellationToken)
        {
            return await context.Teachers
                .ToListAsync(cancellationToken);
        }

        private static async Task<Teacher?> GetTeacher(
            int id,
            [FromServices] ISqlDbContextQuery context,
            CancellationToken cancellationToken)
        {
            return await context.Teachers
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                ?? throw new NotFoundEntityException(typeof(Teacher).Name, id);
        }

        private static async Task<bool> UpdateTeacher(
            [FromBody] Teacher teacher,
            [FromServices] ISqlDbContext context,
            CancellationToken cancellationToken)
        {
            var oldTeacher = await context.Teachers
                .FirstOrDefaultAsync(x => x.Id == teacher.Id, cancellationToken)
                ?? throw new NotFoundEntityException(typeof(Teacher).Name, teacher.Id);

            oldTeacher.FirstName = teacher.FirstName;
            oldTeacher.LastName = teacher.LastName;

            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}
