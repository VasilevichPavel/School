using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Core.Exceptions;
using School.Entity.Models.People;
using School.Infrastructure.Contexts;
using School.Web.Dto;

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

        private static async Task<List<GetStudentDto>> GetStudents(
            [FromServices] ISqlDbContextQuery context,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var students = await context.Students
                .Include(x => x.Address)
                .ToListAsync(cancellationToken);

            return mapper.Map<List<GetStudentDto>>(students);
        }

        private static async Task<GetStudentDto?> GetStudent(
            int id,
            [FromServices] ISqlDbContextQuery context,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var student = await context.Students
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                ?? throw new NotFoundEntityException(typeof(Student).Name, id);

            return mapper.Map<GetStudentDto>(student);
        }

        private static async Task DeleteStudent(
            int id,
            [FromServices] ISqlDbContext context,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var student = await context.Students
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                ?? throw new NotFoundEntityException(typeof(Student).Name, id);

            context.Students.Remove(student);
            await context.SaveChangesAsync(cancellationToken);
        }

        private static async Task UpdateStudent(
            [FromBody] UpdateStudentDto studentDto,
            [FromServices] ISqlDbContext context,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var student = await context.Students
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == studentDto.Id, cancellationToken)
                ?? throw new NotFoundEntityException(typeof(Student).Name, studentDto.Id);

            student.FirstName = student.FirstName;
            student.LastName = student.LastName;
            student.DayOfBirth = student.DayOfBirth;

            if (student.Address != null)
            {
                if (student.Address == null)
                {
                    student.Address = student.Address;
                }
                else
                {
                    student.Address.Street = student.Address.Street;
                    student.Address.City = student.Address.City;
                    student.Address.PostalCode = student.Address.PostalCode;
                }
            }

            await context.SaveChangesAsync(cancellationToken);
        }

        private static async Task CreateStudent(
            [FromBody] CreateStudentDto studentDto,
            [FromServices] ISqlDbContext context,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var student = mapper.Map<Student>(studentDto);
            await context.Students.AddAsync(student, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
