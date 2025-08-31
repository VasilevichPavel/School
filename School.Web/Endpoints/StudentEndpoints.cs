using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Entity.Models.People;
using School.Infrastructure.Contexts;

namespace School.Web.Endpoints
{
    public static class StudentEndpoints
    {
        public static void MapStudentEndpoints(this IEndpointRouteBuilder routes)
        {
            var endpoints = routes.MapGroup("/api/students");

            endpoints.MapGet("/", GetStudents);
            endpoints.MapGet("/{id:int}", GetStudent);
            endpoints.MapDelete("/{id:int}", DeleteStudent);
            endpoints.MapPut("/", UpdateStudent);
            endpoints.MapPost("/", CreateStudent);
        }

        private static async Task<List<Student>> GetStudents(
            [FromServices] ISqlDbContextQuery context,
            CancellationToken cancellationToken)
        {
            return await context.Students.ToListAsync(cancellationToken);
        }

        private static async Task<Student> GetStudent(
            int id,
            [FromServices] ISqlDbContextQuery context,
            CancellationToken cancellationToken)
        {
            return await context.Students.FirstAsync(x => x.Id == id, cancellationToken);
        }

        private static async Task<bool> DeleteStudent(
            int id,
            [FromServices] ISqlDbContext context,
            CancellationToken cancellationToken)
        {
            var student = await context.Students.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (student is not null)
            {
                context.Students.Remove(student);
                var result = await context.SaveChangesAsync(cancellationToken);
                return result > 0;
            }

            return false;
        }

        private static async Task<bool> UpdateStudent(
            [FromBody] Student student,
            [FromServices] ISqlDbContext context,
            CancellationToken cancellationToken)
        {
            var oldStudent = await context.Students.FirstOrDefaultAsync(x => x.Id == student.Id, cancellationToken);

            if (oldStudent is not null)
            {
                oldStudent.FirstName = student.FirstName;
                oldStudent.LastName = student.LastName;
                oldStudent.DayOfBirth = student.DayOfBirth;
                oldStudent.Address = student.Address;
                oldStudent.StudentId = student.StudentId;

                var result = await context.SaveChangesAsync(cancellationToken);
                return result > 0;
            }

            return false;
        }

        private static async Task<bool> CreateStudent(
            [FromBody] Student student,
            [FromServices] ISqlDbContext context,
            CancellationToken cancellationToken)
        {
            await context.Students.AddAsync(student, cancellationToken);
            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}
