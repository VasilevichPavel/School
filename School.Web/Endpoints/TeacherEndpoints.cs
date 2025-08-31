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
    public static class TeacherEndpoints
    {
        public static void MapTeacherEndpoints(this IEndpointRouteBuilder routes)
        {
            var endpoints = routes.MapGroup("/api/teachers");

            endpoints.MapGet("/", GetTeachers);
            endpoints.MapGet("/{id:int}", GetTeacher);
            endpoints.MapPut("/", UpdateTeacher);
            endpoints.MapPost("/", CreateTeacher);
        }

        private static async Task<List<GetTeacherDto>> GetTeachers(
            [FromServices] ISqlDbContextQuery context,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            return await context.Teachers
                .ProjectTo<GetTeacherDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        private static async Task<GetTeacherDto?> GetTeacher(
            int id,
            [FromServices] ISqlDbContextQuery context,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            return await context.Teachers
                .ProjectTo<GetTeacherDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                ?? throw new NotFoundEntityException(typeof(Teacher).Name, id);
        }

        private static async Task UpdateTeacher(
            [FromBody] UpdateTeacherDto teacherDto,
            [FromServices] ISqlDbContext context,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var teacher = await context.Teachers
                .FirstOrDefaultAsync(x => x.Id == teacherDto.Id, cancellationToken)
                ?? throw new NotFoundEntityException(typeof(Teacher).Name, teacherDto.Id);

            teacher.FirstName = teacherDto.FirstName;
            teacher.LastName = teacherDto.LastName;

            await context.SaveChangesAsync(cancellationToken);
        }

        private static async Task CreateTeacher(
            [FromBody] CreateTeacherDto teacherDto,
            [FromServices] ISqlDbContext context,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var teacher = mapper.Map<Teacher>(teacherDto);
            await context.Teachers.AddAsync(teacher, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
