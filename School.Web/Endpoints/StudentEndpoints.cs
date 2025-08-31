using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Core.Exceptions;
using School.Entity.Models;
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
            endpoints.MapGet("/student-id{studentId}", GetStudentByStudentId);
            endpoints.MapDelete("/{id:int}", DeleteStudent);
            endpoints.MapPut("/", UpdateStudent);
            endpoints.MapPost("/", CreateStudent);
        }

        private static async Task<List<Student>> GetStudents(
            [FromServices] ISqlDbContextQuery context,
            CancellationToken cancellationToken)
        {
            return await context.Students
                .Include(x => x.Address)
                .ToListAsync(cancellationToken);
        }

        private static async Task<Student?> GetStudent(
            int id,
            [FromServices] ISqlDbContextQuery context,
            CancellationToken cancellationToken)
        {
            return await context.Students
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                ?? throw new NotFoundEntityException(typeof(Student).Name, id);
        }

        private static async Task<Student?> GetStudentByStudentId(
            [FromQuery]string studentId,
            [FromServices] ISqlDbContextQuery context,
            CancellationToken cancellationToken)
        {
            return await context.Students.FirstOrDefaultAsync(x => x.StudentId == studentId, cancellationToken)
                ?? throw new NotFoundEntityException(typeof(Student).Name, studentId);
        }

        private static async Task<bool> DeleteStudent(
            int id,
            [FromServices] ISqlDbContext context,
            CancellationToken cancellationToken)
        {
            var student = await context.Students.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                ?? throw new NotFoundEntityException(typeof(Student).Name, id);

            context.Students.Remove(student);
            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        private static async Task<bool> UpdateStudent(
            [FromBody] Student student,
            [FromServices] ISqlDbContext context,
            CancellationToken cancellationToken)
        {
            var oldStudent = await context.Students
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == student.Id, cancellationToken)
                ?? throw new NotFoundEntityException(typeof(Student).Name, student.Id);

            oldStudent.FirstName = student.FirstName;
            oldStudent.LastName = student.LastName;
            oldStudent.DayOfBirth = student.DayOfBirth;
            oldStudent.StudentId = student.StudentId;

            if (student.Address != null)
            {
                if (oldStudent.Address == null)
                {
                    oldStudent.Address = student.Address;
                }
                else
                {
                    oldStudent.Address.Street = student.Address.Street;
                    oldStudent.Address.City = student.Address.City;
                    oldStudent.Address.PostalCode = student.Address.PostalCode;
                }
            }

            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
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
