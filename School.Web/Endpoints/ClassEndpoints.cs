using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Core.Exceptions;
using School.Entity.Models;
using School.Entity.Models.People;
using School.Infrastructure.Contexts;

namespace School.Web.Endpoints
{
    public class ClassEndpoints
    {
        public static void MapClassEndpoints(this IEndpointRouteBuilder routes)
        {
            var endpoints = routes.MapGroup("/api/classes");

            endpoints.MapGet("/", CreateClass);
        }

        private static async Task<bool> CreateClass(
            int studentId,
            int classId,
            [FromServices] ISqlDbContext context,
            CancellationToken cancellationToken)
        {
            var classEntity = await context.
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == classId, cancellationToken)
                ?? throw new NotFoundEntityException(nameof(Class), classId);

            if (classEntity.Students.Count >= 20)
                throw new InvalidOperationException("This class already has 20 students assigned.");

            var student = await context.Students
                .FirstOrDefaultAsync(s => s.Id == studentId, cancellationToken)
                ?? throw new NotFoundEntityException(nameof(Student), studentId);

            student.ClassId = classId;

            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        private static async Task<bool> AssignStudentToClass(
            int studentId,
            int classId,
            [FromServices] ISqlDbContext context,
            CancellationToken cancellationToken)
        {
            var classEntity = await context.Classes
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == classId, cancellationToken)
                ?? throw new NotFoundEntityException(nameof(Class), classId);

            if (classEntity.Students.Count >= 20)
                throw new InvalidOperationException("This class already has 20 students assigned.");

            var student = await context.Students
                .FirstOrDefaultAsync(s => s.Id == studentId, cancellationToken)
                ?? throw new NotFoundEntityException(nameof(Student), studentId);

            student.ClassId = classId;

            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}
