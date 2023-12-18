using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using UniversityApi.CustomResponses;
using UniversityApi.Data;
using UniversityApi.Dtos;
using UniversityApi.Entities;
using UniversityApi.Repository;
using UniversityApi.Service.ServiceAbstracts;

namespace UniversityApi.Service.AllServices
{
    public class HierarchyServices : IHierarchyService
    {
        private readonly UniversistyContext _context;
        private readonly IRepositories _repositories;

        public HierarchyServices(UniversistyContext context, IRepositories repositories)
        {
            _context = context;
            _repositories = repositories;
        }


        public async Task<ApiResponse<HierarchyDto>> CreateHierarchyObjectAsync(HierarchyDto input, CancellationToken cancellationToken)
        {
            var maxSortIndex = await _context.Hierarchy.MaxAsync(h => (int?)h.SortIndex, cancellationToken) ?? 0;

            var hierarchyObject = new Hierarchy
            {
                ParentId = input.ParentId,
                SortIndex = input.SortIndex.HasValue ? input.SortIndex.Value : maxSortIndex + 1
            };

            await _repositories.HierarchyRepository.CreateObjectAsync(hierarchyObject, cancellationToken);
            await _repositories.HierarchyRepository.SaveChangesAsync(cancellationToken);

            var objectQueryable = await _repositories.HierarchyRepository.GetDataAsync(cancellationToken);
            var fetchedObject = await objectQueryable.FirstOrDefaultAsync(h => h.Id == hierarchyObject.Id, cancellationToken);

            if (fetchedObject == null)
            {
                throw new CustomExceptions.NotFoundException("HierarchyObject not found");
            }

            var objectDto = new HierarchyDto
            {
                Id = fetchedObject.Id,
                ParentId = fetchedObject.ParentId,
                SortIndex = fetchedObject.SortIndex
            };
            var successResult = ApiResponse<HierarchyDto>.SuccesResult(objectDto);
            return successResult;
        }

        public async Task<ApiResponse<GetDtosWithCount<List<HierarchyDto>>>> GetHierarchyObjectsAsync(HierarchyDto filter, CancellationToken cancellationToken)
        {
            var query = await _repositories.HierarchyRepository.GetDataAsync(cancellationToken);

            if (query == null)
            {
                throw new CustomExceptions.NotFoundException("HierarchyObject not found");
            }

            var filteredHierarchyObjects = FilterData(filter, query);

            if (filteredHierarchyObjects.Count == 0)
            {
                throw new CustomExceptions.NotFoundException("HierarchyObject not found");
            }



            var hierarchyobjects = filteredHierarchyObjects.AsQueryable()
                                                                 .OrderBy(h => h.SortIndex)
                                                                 .ToList();
            var hierarchyDtos = await NestedHierarchyAsync(hierarchyobjects, null, cancellationToken);

            return ApiResponse<GetDtosWithCount<List<HierarchyDto>>>.SuccesResult(new GetDtosWithCount<List<HierarchyDto>>
            {
                Data = hierarchyDtos,
                Count = filteredHierarchyObjects.Count()
            });
        }

        private async Task<List<HierarchyDto>> NestedHierarchyAsync(List<Hierarchy> allObjects, int? parentId, CancellationToken cancellationToken)
        {
            var hierarchy = new List<HierarchyDto>();

            var hierarchyObjects = allObjects.Where(obj => obj.ParentId == parentId).ToList();

            foreach(var obj in hierarchyObjects)
            {
                hierarchy.Add(new HierarchyDto
                {
                    Id = obj.Id,
                    ParentId = obj.ParentId,
                    SortIndex = obj.SortIndex,
                    Children = await NestedHierarchyAsync(allObjects, obj.Id, cancellationToken)
                });
            }
            return hierarchy;
        }

        public List<Hierarchy> FilterData(HierarchyDto filter, IQueryable<Hierarchy> hierarchies)
        {
            if (filter.Id != null)
            {
                hierarchies = hierarchies.Where(h => h.Id == filter.Id);
            }
            if (filter.ParentId != null)
            {
                hierarchies = hierarchies.Where(h => h.ParentId == filter.ParentId);
            }
            if (filter.SortIndex != null)
            {
                hierarchies = hierarchies.Where(h => h.SortIndex == filter.SortIndex);
            }

            return hierarchies.ToList();
        }

        public async Task<ApiResponse<string>> UpdateHierarchyObjectAsync(HierarchyDto input, CancellationToken cancellationToken)
        {
            var objectQueryable = await _repositories.HierarchyRepository.GetDataAsync(cancellationToken);
            var hierarchyObject = await objectQueryable.AsQueryable().FirstOrDefaultAsync(h => h.Id == input.Id, cancellationToken);

            var maxSortIndex = await _context.Hierarchy.MaxAsync(h => (int?)h.SortIndex, cancellationToken) ?? 0;

            hierarchyObject.Id = input.Id.HasValue ? (int)input.Id : 0;
            hierarchyObject.ParentId = input.ParentId.HasValue ? input.ParentId : null;
            hierarchyObject.SortIndex = input.ParentId.HasValue ? (int)input.SortIndex.Value : maxSortIndex + 1;

            if (hierarchyObject == null)
            {
                throw new CustomExceptions.NotFoundException("HierarchyObject not found");
            }

            await _repositories.HierarchyRepository.SaveChangesAsync(cancellationToken);

            var sortIndex = hierarchyObject.SortIndex + 1;

            if(hierarchyObject.ParentId == null)
            {
                var allParents = await _repositories.HierarchyRepository.GetDataAsync(cancellationToken);
                var parentsQueryable = await allParents.Where(h => h.ParentId == null && h.Id != hierarchyObject.Id)
                                                 .OrderBy(h => h.SortIndex)
                                                 .ToListAsync(cancellationToken);

                foreach(var item in allParents)
                {
                    if(item.SortIndex >= hierarchyObject.SortIndex)
                    {
                        item.SortIndex = sortIndex;
                        sortIndex++;
                    }
                }
                await _repositories.HierarchyRepository.SaveChangesAsync(cancellationToken);
            }
            else
            {
                var childrenOfParentObject = await _repositories.HierarchyRepository.GetDataAsync(cancellationToken);
                var childrenOfParentObjectQueryable = await childrenOfParentObject.Where(h => h.ParentId == hierarchyObject.ParentId && h.Id != hierarchyObject.Id)
                                                                            .OrderBy(h => h.SortIndex) 
                                                                            .ToListAsync(cancellationToken);
                foreach(var item in childrenOfParentObject)
                {
                    if(item.SortIndex >= hierarchyObject.SortIndex)
                    {
                        item.SortIndex = sortIndex;
                        sortIndex++;
                    }
                }
                await _repositories.HierarchyRepository.SaveChangesAsync(cancellationToken);
            }

            var successResult = ApiResponse<string>.SuccesResult("HierarchyObject changed successfully");
            return successResult;
        }

        public async Task<ApiResponse<string>> DeleteHierarchyObjectAsync(int hierarchyObjectId, CancellationToken cancellationToken)
        {
            var hierarchyObhect = await _context.Hierarchy.FirstOrDefaultAsync(h => h.Id == hierarchyObjectId, cancellationToken);
            if (hierarchyObhect == null)
            {
                throw new CustomExceptions.NotFoundException("User not found on that Id");
            }

            if (await _repositories.HierarchyRepository.DeleteHierarchyObjectAsync(hierarchyObjectId, cancellationToken))
            {
                await _repositories.HierarchyRepository.SaveChangesAsync(cancellationToken);
                var successResult = ApiResponse<string>.SuccesResult("HierarchyObject deleted successfully");
                return successResult;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
