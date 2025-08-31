using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Core.Exceptions;
using School.Entity.Models;
using School.Entity.Models.People;
using School.Infrastructure.Contexts;
using School.Web.Dto;

namespace School.Web.Endpoints
{
    public static class ClassEndpoints
    {
        public static void MapClassEndpoints(this IEndpointRouteBuilder routes)
        {
            var endpoints = routes.MapGroup("/api/classes");

            endpoints.MapGet("/", GetClasses);
            endpoints.MapGet("/{id:int}", GetClass);
            endpoints.MapPost("/", CreateClass);
            endpoints.MapPost("/{classId:int}/students", AssignStudentsToClass);
            endpoints.MapPut("/{classId:int}/teacher/{teacherId:int}", UpdateClassTeacher);
            endpoints.MapDelete("/{classId:int}/students", UnassignStudentsFromClass);
            endpoints.MapDelete("/{classId:int}/students/{studentId}", UnassignStudentFromClass);
            endpoints.MapDelete("/{classId:int}", DeleteClass);
        }

        private static async Task<List<GetClassDto>> GetClasses(
            [FromServices] ISqlDbContext context,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var classes = await context.Classes
                .Include(x => x.Teacher)
                .Include(x => x.Students)
                .ToListAsync(cancellationToken);

            return mapper.Map<List<GetClassDto>>(classes);
        }

        private static async Task<List<GetClassDto>> GetClass(
            int id,
            [FromServices] ISqlDbContext context,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var classes = await context.Classes
                .Include(x => x.Teacher)
                .Include(x => x.Students)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                ?? throw new NotFoundEntityException(typeof(Class).Name, id);

            return mapper.Map<List<GetClassDto>>(classes);
        }

        private static async Task CreateClass(
            [FromBody] CreateClassDto classDto,
            [FromServices] ISqlDbContext context,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            if (classDto.StudentIds != null && classDto.StudentIds.Count > 20)
            {
                throw new ValidationException("Cannot assign more than 20 students to a class.");
            }

            var teacherExists = await context.Teachers
                .AnyAsync(t => t.Id == classDto.TeacherId, cancellationToken);

            if (!teacherExists)
            {
                throw new NotFoundEntityException(nameof(Teacher), classDto.TeacherId);
            }

            var @class = mapper.Map<Class>(classDto);

            if (classDto.StudentIds != null! && classDto.StudentIds.Count != 0)
            {
                var students = await context.Students
                    .Where(s => classDto.StudentIds.Contains(s.Id))
                    .ToListAsync(cancellationToken);

                if (classDto.StudentIds.Count != students.Count)
                {
                    var wrongIds = classDto.StudentIds
                        .Except(students.Select(s => s.Id));

                    throw new NotFoundEntityException($"{typeof(Student).Name}s", wrongIds);
                }

                @class.Students = students;
            }

            await context.Classes.AddAsync(@class, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        private static async Task AssignStudentsToClass(
            int classId,
            [FromBody]List<int> studentIds,
            [FromServices] ISqlDbContext context,
            CancellationToken cancellationToken)
        {
            if (studentIds.Count == 0)
            {
                throw new ValidationException("The list of students is empty");
            }

            var classEntity = await context.Classes
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == classId, cancellationToken)
                ?? throw new NotFoundEntityException(nameof(Class), classId);

            int availableSlots = 20 - classEntity.Students.Count;
            if (availableSlots <= 0)
            {
                throw new ValidationException("This class already has 20 students.");
            }

            var students = await context.Students
                .Where(s => studentIds.Contains(s.Id))
                .ToListAsync(cancellationToken);

            if (studentIds.Count != students.Count)
            {
                var wrongIds = studentIds
                    .Except(students.Select(s => s.Id));

                throw new NotFoundEntityException($"{typeof(Student).Name}s", wrongIds);
            }

            classEntity.Students.AddRange(students.Take(availableSlots));

            await context.SaveChangesAsync(cancellationToken);
        }

        private static async Task UnassignStudentsFromClass(
            int classId,
            [FromServices] ISqlDbContext context,
            CancellationToken cancellationToken)
        {
            var classEntity = await context.Classes
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == classId, cancellationToken)
                ?? throw new NotFoundEntityException(nameof(Class), classId);

            classEntity.Students.Clear();

            await context.SaveChangesAsync(cancellationToken);
        }

        private static async Task UnassignStudentFromClass(
            int classId,
            int studentId,
            [FromServices] ISqlDbContext context,
            CancellationToken cancellationToken)
        {
            var classEntity = await context.Classes
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == classId, cancellationToken)
                ?? throw new NotFoundEntityException(nameof(Class), classId);

            var student = classEntity.Students.FirstOrDefault(s => s.Id == studentId)
                ?? throw new NotFoundEntityException(nameof(Student), classId);

            classEntity.Students.Remove(student);
            await context.SaveChangesAsync(cancellationToken);
        }

        private static async Task DeleteClass(
            int classId,
            [FromServices] ISqlDbContext context,
            CancellationToken cancellationToken)
        {
            var classEntity = await context.Classes
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == classId, cancellationToken)
                ?? throw new NotFoundEntityException(nameof(Class), classId);

            classEntity.Students.Clear();
            context.Classes.Remove(classEntity);
            await context.SaveChangesAsync(cancellationToken);
        }

        private static async Task UpdateClassTeacher(
            int classId,
            int teacherId,
            [FromServices] ISqlDbContext context,
            CancellationToken cancellationToken)
        {
            var classEntity = await context.Classes
                .FirstOrDefaultAsync(c => c.Id == classId, cancellationToken)
                ?? throw new NotFoundEntityException(nameof(Class), classId);

            var teacherExists = await context.Teachers
                .AnyAsync(t => t.Id == teacherId, cancellationToken);

            if (!teacherExists)
            {
                throw new NotFoundEntityException(nameof(Teacher), teacherId);
            }

            classEntity.TeacherId = teacherId;

            await context.SaveChangesAsync(cancellationToken);
        }

    }
}
