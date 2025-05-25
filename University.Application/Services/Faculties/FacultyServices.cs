using Microsoft.EntityFrameworkCore;
using University.Application.Services.Faculties.Helpers;
using University.Data.ContextMethodsDirectory;
using University.Data.Data.Entities;
using University.Domain.CustomExceptions;
using University.Domain.CustomResponses;
using University.Domain.Models;
using University.Domain.Models.FacultyModels;

namespace University.Application.Services.Faculties;

public class FacultyServices(IUniversityContext universityContext) : IFacultyServices
{
    public async Task<ApiResponse<GetDtoWithCount<ICollection<FacultyGetDto>>>> Get(FacultyGetFilter filter,
        CancellationToken cancellationToken)
    {
        var faculties = universityContext.Faculties.AllAsNoTracking.FilterData(filter);

        var result = await faculties
            .MapToFacultyGetDto()
            .AsSplitQuery()
            .OrderByDescending(f => f.Id)
            .Skip(filter.Offset ?? 0)
            .Take(filter.Limit ?? 10)
            .ToListAsync(cancellationToken);

        return ApiResponse<GetDtoWithCount<ICollection<FacultyGetDto>>>.SuccessResult(
            new GetDtoWithCount<ICollection<FacultyGetDto>>
            {
                Data = result,
                Count = await faculties.CountAsync(cancellationToken)
            });
    }

    public async Task<int> Create(FacultyPostDto input, CancellationToken cancellationToken)
    {
        var faculty = new Faculty
        {
            FacultyName = input.FacultyName
        };

        faculty = await faculty.FillData(input, universityContext, cancellationToken);

        universityContext.Faculties.Add(faculty);
        
        await universityContext.CompleteAsync(cancellationToken);

        return faculty.Id;
    }

    public async Task<int> Update(FacultyPutDto input, CancellationToken cancellationToken)
    {
        var faculty = await universityContext.Faculties.All
            .Include(f => f.Users)
            .Include(f => f.Courses)
            .FirstOrDefaultAsync(faculty => faculty.Id == input.Id,
                cancellationToken);

        if(faculty is null)
            throw new NotFoundException("Faculty not found");
            
        faculty = await faculty.FillData(input, universityContext, cancellationToken);
            
        await universityContext.CompleteAsync(cancellationToken);
            
        return faculty.Id;
    }

    public async Task<int> Delete(int facultyId, CancellationToken cancellationToken)
    {
        var faculty = await universityContext.Faculties.All
            .Include(f => f.Users)
            .Include(f => f.Courses)
            .FirstOrDefaultAsync(f => f.Id == facultyId, cancellationToken);
            
        if (faculty == null)
            throw new NotFoundException("Faculty not found");

        faculty.Users.Clear();
        faculty.Courses.Clear();
        faculty.IsActive = false;
        await universityContext.CompleteAsync(cancellationToken);
            
        return facultyId;
    }
}