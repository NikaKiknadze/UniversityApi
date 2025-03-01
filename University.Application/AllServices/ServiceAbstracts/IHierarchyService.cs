using University.Data.Data.Entities;
using University.Domain.CustomResponses;
using University.Domain.Models;

namespace University.Application.AllServices.ServiceAbstracts
{
    public interface IHierarchyService
    {
        Task<ApiResponse<GetDtoWithCount<List<HierarchyDto>>>> GetHierarchyObjectsAsync(HierarchyDto filter, CancellationToken cancellationToken);
        Task<ApiResponse<HierarchyDto>> CreateHierarchyObjectAsync(HierarchyDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> UpdateHierarchyObjectAsync(HierarchyDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> DeleteHierarchyObjectAsync(int hierarchyObjectId, CancellationToken cancellationToken);
        List<Hierarchy> FilterData(HierarchyDto filter, IQueryable<Hierarchy> hierarchies);
    }
}
