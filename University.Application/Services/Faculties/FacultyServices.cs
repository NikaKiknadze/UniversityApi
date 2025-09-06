using Microsoft.EntityFrameworkCore;
using University.Application.Services.Faculties.Helpers;
using University.Data.Data.Entities;
using University.Data.Repositories.Interfaces;
using University.Domain.CustomExceptions;
using University.Domain.CustomResponses;
using University.Domain.Models;
using University.Domain.Models.FacultyModels;

namespace University.Application.Services.Faculties;

public class FacultyServices(IFacultyRepository facultyRepository) : IFacultyServices
{
    public async Task<ApiResponse<GetDtoWithCount<ICollection<FacultyGetDto>>>> Get(FacultyGetFilter filter,
        CancellationToken cancellationToken)
    {
        var faculties = facultyRepository.AllAsNoTracking.FilterData(filter);

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

        faculty = faculty.FillData(input);

        await facultyRepository.AddAsync(faculty, cancellationToken);

        return faculty.Id;
    }

    public async Task<int> Update(FacultyPutDto input, CancellationToken cancellationToken)
    {
        var faculty = await facultyRepository.All
            .Include(f => f.FacultyCourses)
            .FirstOrDefaultAsync(faculty => faculty.Id == input.Id,
                cancellationToken);

        if(faculty is null)
            throw new NotFoundException("Faculty not found");
            
        faculty =  faculty.FillData(input);
            
        await facultyRepository.UpdateAsync(faculty, cancellationToken);
            
        return faculty.Id;
    }

    public async Task<int> Delete(int facultyId, CancellationToken cancellationToken)
    {
        var faculty = await facultyRepository.All
            .Include(f => f.Users)
            .Include(f => f.FacultyCourses)
            .FirstOrDefaultAsync(f => f.Id == facultyId, cancellationToken);
            
        if (faculty == null)
            throw new NotFoundException("Faculty not found");

        faculty.FacultyCourses.Clear();
        faculty.IsActive = false;
        await facultyRepository.UpdateAsync(faculty, cancellationToken);
            
        return facultyId;
    }
}