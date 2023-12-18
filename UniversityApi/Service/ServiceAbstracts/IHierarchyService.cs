using UniversityApi.CustomResponses;
using UniversityApi.Dtos;
using UniversityApi.Entities;

namespace UniversityApi.Service.ServiceAbstracts
{
    public interface IHierarchyService
    {
        Task<ApiResponse<GetDtosWithCount<List<HierarchyDto>>>> GetHierarchyObjectsAsync(HierarchyDto filter, CancellationToken cancellationToken);
        Task<ApiResponse<HierarchyDto>> CreateHierarchyObjectAsync(HierarchyDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> UpdateHierarchyObjectAsync(HierarchyDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> DeleteHierarchyObjectAsync(int hierarchyObjectId, CancellationToken cancellationToken);
        List<Hierarchy> FilterData(HierarchyDto filter, IQueryable<Hierarchy> hierarchies);
    }
}
