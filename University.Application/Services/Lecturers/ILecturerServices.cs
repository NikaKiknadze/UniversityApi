using University.Domain.CustomResponses;
using University.Domain.Models;
using University.Domain.Models.LecturerModels;

namespace University.Application.Services.Lecturers
{
    public interface ILecturerServices
    {
        Task<ApiResponse<GetDtoWithCount<ICollection<LecturerGetDto>>>> Get(LecturerGetFilter filter, CancellationToken cancellationToken);
        Task<int> Create(LecturerPostDto input, CancellationToken cancellationToken);
        Task<int> Update(LecturerPutDto input, CancellationToken cancellationToken);
        Task<int> Delete(int lecturerId, CancellationToken cancellationToken);
    }
}
