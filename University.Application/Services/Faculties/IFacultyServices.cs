using University.Domain.CustomResponses;
using University.Domain.Models;
using University.Domain.Models.FacultyModels;

namespace University.Application.Services.Faculties
{
    public interface IFacultyServices
    {
        Task<ApiResponse<GetDtoWithCount<ICollection<FacultyGetDto>>>> Get(FacultyGetFilter filter, CancellationToken cancellationToken);
        Task<int> Create(FacultyPostDto input, CancellationToken cancellationToken);
        Task<int> Update(FacultyPutDto input, CancellationToken cancellationToken);
        Task<int> Delete(int facultyId, CancellationToken cancellationToken);
    }
}
