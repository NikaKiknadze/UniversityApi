namespace UniversityApi.Dtos
{
    public class HierarchyDto
    {
        public int? Id { get; set; }

        public int? ParentId { get; set; }

        public int? SortIndex { get; set; }

        public ICollection<HierarchyDto>? Children { get; set; }
    }
}
