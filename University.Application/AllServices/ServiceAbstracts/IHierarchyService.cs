using University.Domain.CustomResponses;
using University.Domain.Models;

namespace University.Application.AllServices.ServiceAbstracts
{
    public interface IHierarchyService
    {
        Task<ApiResponse<GetDtoWithCount<List<HierarchyDto>>>> Get(HierarchyDto filter, CancellationToken cancellationToken);
        Task<ApiResponse<HierarchyDto>> Create(HierarchyDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> Update(HierarchyDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> Delete(int hierarchyObjectId, CancellationToken cancellationToken);
    }
}
