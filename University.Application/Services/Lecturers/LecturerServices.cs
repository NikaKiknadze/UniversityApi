using Microsoft.EntityFrameworkCore;
using University.Application.Services.Lecturers.Helpers;
using University.Data.ContextMethodsDirectory;
using University.Data.Data.Entities;
using University.Domain.CustomExceptions;
using University.Domain.CustomResponses;
using University.Domain.Models;
using University.Domain.Models.LecturerModels;

namespace University.Application.Services.Lecturers;

public class LecturerServices(IUniversityContext universityContext) : ILecturerServices
{
    public async Task<ApiResponse<GetDtoWithCount<ICollection<LecturerGetDto>>>> Get(LecturerGetFilter filter,
        CancellationToken cancellationToken)
    {
        var lecturers = universityContext.Lecturers.AllAsNoTracking.FilterData(filter);

        var result = await lecturers
            .MapDataToLecturerGetDto()
            .AsSplitQuery()
            .OrderByDescending(l => l.Id)
            .Skip(filter.Offset ?? 0)
            .Take(filter.Limit ?? 10)
            .ToListAsync(cancellationToken);

        return ApiResponse<GetDtoWithCount<ICollection<LecturerGetDto>>>.SuccessResult(
            new GetDtoWithCount<ICollection<LecturerGetDto>>
            {
                Data = result,
                Count = await lecturers.CountAsync(cancellationToken)
            });
    }

    public async Task<int> Create(LecturerPostDto input, CancellationToken cancellationToken)
    {
        var lecturer = new Lecturer
        {
            Name = input.Name,
            SurName = input.Surname,
            Age = input.Age
        };

        lecturer = lecturer.FillData(input);

        universityContext.Lecturers.Add(lecturer);
        
        await universityContext.CompleteAsync(cancellationToken);

        return lecturer.Id;
    }

    public async Task<int> Update(LecturerPutDto input, CancellationToken cancellationToken)
    {
        var lecturer = await universityContext.Lecturers.All
            .Include(l => l.UsersLecturers)
            .Include(l => l.CoursesLecturers)
            .FirstOrDefaultAsync(lecturer => lecturer.Id == input.Id, cancellationToken);

        if (lecturer is null)
            throw new NotFoundException("Lecturer not found");

        lecturer = lecturer.FillData(input);
            
        await universityContext.CompleteAsync(cancellationToken);
        return lecturer.Id;
    }

    public async Task<int> Delete(int lecturerId, CancellationToken cancellationToken)
    {
        var lecturer = await universityContext.Lecturers.All
            .Include(ul => ul.UsersLecturers)
            .ThenInclude(u => u.User)
            .Include(cl => cl.CoursesLecturers)
            .ThenInclude(c => c.Course)
            .FirstOrDefaultAsync(lecturer => lecturer.Id == lecturerId, cancellationToken);

        if (lecturer is null)
            throw new NotFoundException("Lecturer not found");

        lecturer.CoursesLecturers.Clear();
        lecturer.UsersLecturers.Clear();
        lecturer.IsActive = false;
            
        await universityContext.CompleteAsync(cancellationToken);
        return lecturer.Id;
    }
}